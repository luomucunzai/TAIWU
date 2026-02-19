using System;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleFarmer : VillagerRoleBase
{
	private static class FieldIds
	{
		public const ushort ArrangementTemplateId = 0;

		public const ushort FarmerStorageTypes = 1;

		public const ushort MigrateFailureInfo = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "ArrangementTemplateId", "FarmerStorageTypes", "MigrateFailureInfo" };
	}

	public const int MapBlockRecoveryLockDuration = 120;

	[Obsolete]
	[SerializableGameDataField]
	public sbyte[] FarmerStorageTypes;

	[SerializableGameDataField]
	public (sbyte resourceType, int count) MigrateFailureInfo;

	public override short RoleTemplateId => 0;

	public int CollectResourceActionCount => VillagerRoleFormulaImpl.Calculate(0, base.Personality);

	public int MigrateResourceSuccessRate => MigrateResourceBaseSuccessRate + MigrateResourceSuccessRateBonus;

	public int MigrateResourceBaseSuccessRate => VillagerRoleFormulaImpl.Calculate(2, base.Personality);

	public int MigrateResourceSuccessRateBonus => VillagerRoleFormulaImpl.Calculate(47, base.Personality, MigrateFailureInfo.count);

	public int UpgradeBuildingCoreRate => VillagerRoleFormulaImpl.Calculate(3, base.Personality);

	public VillagerRoleFarmer()
	{
		FarmerStorageTypes = new sbyte[6];
		for (int i = 0; i < FarmerStorageTypes.Length; i++)
		{
			FarmerStorageTypes[i] = 1;
		}
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		if (ArrangementTemplateId >= 0)
		{
			return;
		}
		VillagerWorkData workData = WorkData;
		bool flag;
		if (workData != null)
		{
			sbyte workType = workData.WorkType;
			if (workType == 1 || workType == 10 || workType == 14)
			{
				flag = true;
				goto IL_003e;
			}
		}
		flag = false;
		goto IL_003e;
		IL_003e:
		if (flag)
		{
			return;
		}
		BoolArray64 autoActionStates = base.AutoActionStates;
		if (((BoolArray64)(ref autoActionStates))[0])
		{
			int num = CollectResourceActionCount - 1;
			while (num >= 0 && AutoCollectResourceAction(context))
			{
				num--;
			}
		}
	}

	public void ResetFailureAccumulation()
	{
		MigrateFailureInfo = (resourceType: -1, count: 0);
	}

	public void RefreshFailureAccumulation(sbyte resourceType)
	{
		if (resourceType == -1 || MigrateFailureInfo.resourceType != resourceType)
		{
			MigrateFailureInfo = (resourceType: resourceType, count: 0);
		}
	}

	public void AccumulateMigrateFailure()
	{
		MigrateFailureInfo.count++;
	}

	private static bool CollectResourceBlockFilter(MapBlockData block)
	{
		if (block.GetConfig().ResourceCollectionType < 0)
		{
			return false;
		}
		for (sbyte b = 0; b < 6; b++)
		{
			short num = block.MaxResources[b];
			if (num > 0 && block.CurrResources[b] >= block.MaxResources[b] / 2)
			{
				return true;
			}
		}
		return false;
	}

	public int GetCollectResourceAmount(MapBlockData block, sbyte resourceType)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		return block.GetCollectResourceAmount(resourceType) * CValuePercentBonus.op_Implicit(DomainManager.Building.GetBuildingBlockEffect(block.GetLocation(), EBuildingScaleEffect.CollectResourceIncomeBonus));
	}

	public bool CollectResource(DataContext context, MapBlockData block, sbyte resourceType, bool isAuto = false)
	{
		if (!block.CanCollectResource(resourceType))
		{
			return false;
		}
		int num = GetCollectResourceAmount(block, resourceType);
		if (isAuto)
		{
			num = VillagerRoleFormulaImpl.Calculate(1, num);
			ResourceTypeItem resourceTypeItem = ResourceType.Instance[resourceType];
			short num2 = block.CurrResources[resourceType];
			block.CurrResources[resourceType] = (short)Math.Max(num2 - resourceTypeItem.ResourceReducePerCollection / 5, 0);
		}
		GainResource(context, resourceType, num);
		short maxMalice = block.GetMaxMalice();
		if (maxMalice <= 0)
		{
			DomainManager.Map.SetBlockData(context, block);
			return true;
		}
		block.Malice = (short)Math.Clamp(block.Malice + 10, 0, block.GetMaxMalice());
		DomainManager.Map.SetBlockData(context, block);
		return true;
	}

	private bool AutoCollectResourceAction(DataContext context)
	{
		Location location = Character.GetLocation();
		MapBlockData block = DomainManager.Map.GetBlock(location);
		Span<sbyte> span = stackalloc sbyte[8];
		SpanList<sbyte> spanList = span;
		for (sbyte b = 0; b < 6; b++)
		{
			if (block.CurrResources[b] >= block.MaxResources[b] / 2)
			{
				spanList.Add(b);
			}
		}
		if (spanList.Count == 0)
		{
			TryAddNextAutoTravelTarget(context, CollectResourceBlockFilter);
			return false;
		}
		sbyte random = spanList.GetRandom(context.Random);
		bool flag = CollectResource(context, block, random, isAuto: true);
		if (flag)
		{
			CollectMaterial(context, block, random);
		}
		return flag;
	}

	private void CollectMaterial(DataContext context, MapBlockData blockData, sbyte resourceType)
	{
		short itemTemplateId = blockData.GetCollectItemTemplateId(context.Random, resourceType);
		int percentProb = blockData.GetCollectItemChance(resourceType) * 20 / 100;
		if (itemTemplateId >= 0 && context.Random.CheckPercentProb(percentProb))
		{
			ResourceCollectionItem resourceCollectionConfig = blockData.GetResourceCollectionConfig();
			short maxResource = Math.Max(blockData.MaxResources.Get(resourceType), (short)1);
			short currentResource = blockData.CurrResources.Get(resourceType);
			DomainManager.Map.UpgradeCollectMaterial(context.Random, resourceCollectionConfig, resourceType, maxResource, currentResource, 1, ref itemTemplateId);
			TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
			int currDate = DomainManager.World.GetCurrDate();
			int id = Character.GetId();
			ItemKey itemKey = DomainManager.Item.CreateItem(context, 5, itemTemplateId);
			ItemSourceType itemSourceType = (ItemSourceType)DomainManager.Extra.GetFarmerAutoCollectStorageType();
			if (itemSourceType != ItemSourceType.Warehouse && itemSourceType != ItemSourceType.Treasury && itemSourceType != ItemSourceType.StockStorageGoodsShelf)
			{
				itemSourceType = ItemSourceType.Warehouse;
			}
			DomainManager.Taiwu.AddItem(context, itemKey, 1, itemSourceType);
			DomainManager.LifeRecord.GetLifeRecordCollection().AddFarmerCollectMaterial(id, currDate, Character.GetLocation(), 5, itemTemplateId);
			switch (itemSourceType)
			{
			case ItemSourceType.Warehouse:
				taiwuVillageStoragesRecordCollection.AddGatherResources(currDate, TaiwuVillageStorageType.Warehouse, id, 5, itemTemplateId);
				break;
			case ItemSourceType.Treasury:
				taiwuVillageStoragesRecordCollection.AddGatherResourcesToTreasury(currDate, TaiwuVillageStorageType.Treasury, id, 5, itemTemplateId);
				break;
			case ItemSourceType.StockStorageGoodsShelf:
				taiwuVillageStoragesRecordCollection.AddGatherResourcesToStockStorageGoodsShelf(currDate, TaiwuVillageStorageType.Stock, id, 5, itemTemplateId);
				break;
			case ItemSourceType.Trough:
			case ItemSourceType.StockStorageWarehouse:
				break;
			}
		}
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 11;
		num = ((FarmerStorageTypes == null) ? (num + 2) : (num + (2 + FarmerStorageTypes.Length)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		*(int*)ptr = ArrangementTemplateId;
		ptr += 4;
		if (FarmerStorageTypes != null)
		{
			int num = FarmerStorageTypes.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				ptr[i] = (byte)FarmerStorageTypes[i];
			}
			ptr += num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += SerializationHelper.Serialize<sbyte, int>(ptr, MigrateFailureInfo);
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ArrangementTemplateId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (FarmerStorageTypes == null || FarmerStorageTypes.Length != num2)
				{
					FarmerStorageTypes = new sbyte[num2];
				}
				for (int i = 0; i < num2; i++)
				{
					FarmerStorageTypes[i] = (sbyte)ptr[i];
				}
				ptr += (int)num2;
			}
			else
			{
				FarmerStorageTypes = null;
			}
		}
		if (num > 2)
		{
			ptr += SerializationHelper.Deserialize<sbyte, int>(ptr, ref MigrateFailureInfo);
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
