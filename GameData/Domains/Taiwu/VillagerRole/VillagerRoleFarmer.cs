using System;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x0200004F RID: 79
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleFarmer : VillagerRoleBase
	{
		// Token: 0x1700003B RID: 59
		// (get) Token: 0x060013B1 RID: 5041 RVA: 0x00139F08 File Offset: 0x00138108
		public override short RoleTemplateId
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x060013B2 RID: 5042 RVA: 0x00139F0C File Offset: 0x0013810C
		public VillagerRoleFarmer()
		{
			this.FarmerStorageTypes = new sbyte[6];
			for (int i = 0; i < this.FarmerStorageTypes.Length; i++)
			{
				this.FarmerStorageTypes[i] = 1;
			}
		}

		// Token: 0x060013B3 RID: 5043 RVA: 0x00139F50 File Offset: 0x00138150
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				VillagerWorkData workData = this.WorkData;
				bool flag2;
				if (workData != null)
				{
					sbyte workType = workData.WorkType;
					if (workType == 1 || workType == 10 || workType == 14)
					{
						flag2 = true;
						goto IL_3E;
					}
				}
				flag2 = false;
				IL_3E:
				bool flag3 = flag2;
				if (!flag3)
				{
					bool flag4 = base.AutoActionStates[0];
					if (flag4)
					{
						for (int i = this.CollectResourceActionCount - 1; i >= 0; i--)
						{
							bool flag5 = !this.AutoCollectResourceAction(context);
							if (flag5)
							{
								break;
							}
						}
					}
				}
			}
		}

		// Token: 0x060013B4 RID: 5044 RVA: 0x00139FEC File Offset: 0x001381EC
		public void ResetFailureAccumulation()
		{
			this.MigrateFailureInfo = new ValueTuple<sbyte, int>(-1, 0);
		}

		// Token: 0x060013B5 RID: 5045 RVA: 0x00139FFC File Offset: 0x001381FC
		public void RefreshFailureAccumulation(sbyte resourceType)
		{
			bool flag = resourceType != -1 && this.MigrateFailureInfo.Item1 == resourceType;
			if (!flag)
			{
				this.MigrateFailureInfo = new ValueTuple<sbyte, int>(resourceType, 0);
			}
		}

		// Token: 0x060013B6 RID: 5046 RVA: 0x0013A032 File Offset: 0x00138232
		public void AccumulateMigrateFailure()
		{
			this.MigrateFailureInfo.Item2 = this.MigrateFailureInfo.Item2 + 1;
		}

		// Token: 0x060013B7 RID: 5047 RVA: 0x0013A048 File Offset: 0x00138248
		private unsafe static bool CollectResourceBlockFilter(MapBlockData block)
		{
			bool flag = block.GetConfig().ResourceCollectionType < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (sbyte i = 0; i < 6; i += 1)
				{
					short maxResource = *block.MaxResources[(int)i];
					bool flag2 = maxResource > 0 && *block.CurrResources[(int)i] >= *block.MaxResources[(int)i] / 2;
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060013B8 RID: 5048 RVA: 0x0013A0C6 File Offset: 0x001382C6
		public int GetCollectResourceAmount(MapBlockData block, sbyte resourceType)
		{
			return block.GetCollectResourceAmount(resourceType) * DomainManager.Building.GetBuildingBlockEffect(block.GetLocation(), EBuildingScaleEffect.CollectResourceIncomeBonus, -1);
		}

		// Token: 0x060013B9 RID: 5049 RVA: 0x0013A0EC File Offset: 0x001382EC
		public unsafe bool CollectResource(DataContext context, MapBlockData block, sbyte resourceType, bool isAuto = false)
		{
			bool flag = !block.CanCollectResource(resourceType);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				int addResource = this.GetCollectResourceAmount(block, resourceType);
				if (isAuto)
				{
					addResource = VillagerRoleFormulaImpl.Calculate(1, addResource);
					ResourceTypeItem resourceConfig = ResourceType.Instance[resourceType];
					short currentResource = *block.CurrResources[(int)resourceType];
					*block.CurrResources[(int)resourceType] = (short)Math.Max((int)(currentResource - (short)(resourceConfig.ResourceReducePerCollection / 5)), 0);
				}
				base.GainResource(context, resourceType, addResource);
				short maxMalice = block.GetMaxMalice();
				bool flag2 = maxMalice <= 0;
				if (flag2)
				{
					DomainManager.Map.SetBlockData(context, block);
					result = true;
				}
				else
				{
					block.Malice = (short)Math.Clamp((int)(block.Malice + 10), 0, (int)block.GetMaxMalice());
					DomainManager.Map.SetBlockData(context, block);
					result = true;
				}
			}
			return result;
		}

		// Token: 0x060013BA RID: 5050 RVA: 0x0013A1C4 File Offset: 0x001383C4
		private unsafe bool AutoCollectResourceAction(DataContext context)
		{
			Location location = this.Character.GetLocation();
			MapBlockData block = DomainManager.Map.GetBlock(location);
			Span<sbyte> span = new Span<sbyte>(stackalloc byte[(UIntPtr)8], 8);
			SpanList<sbyte> availableResources = span;
			for (sbyte i = 0; i < 6; i += 1)
			{
				bool flag = *block.CurrResources[(int)i] < *block.MaxResources[(int)i] / 2;
				if (!flag)
				{
					availableResources.Add(i);
				}
			}
			bool flag2 = availableResources.Count == 0;
			bool result;
			if (flag2)
			{
				this.TryAddNextAutoTravelTarget(context, new Predicate<MapBlockData>(VillagerRoleFarmer.CollectResourceBlockFilter));
				result = false;
			}
			else
			{
				sbyte resourceType = availableResources.GetRandom(context.Random);
				bool res = this.CollectResource(context, block, resourceType, true);
				bool flag3 = res;
				if (flag3)
				{
					this.CollectMaterial(context, block, resourceType);
				}
				result = res;
			}
			return result;
		}

		// Token: 0x060013BB RID: 5051 RVA: 0x0013A2A8 File Offset: 0x001384A8
		private void CollectMaterial(DataContext context, MapBlockData blockData, sbyte resourceType)
		{
			short itemTemplateId = blockData.GetCollectItemTemplateId(context.Random, resourceType);
			int chance = blockData.GetCollectItemChance(resourceType) * 20 / 100;
			bool flag = itemTemplateId >= 0 && context.Random.CheckPercentProb(chance);
			if (flag)
			{
				ResourceCollectionItem collectionConfig = blockData.GetResourceCollectionConfig();
				short maxResource = Math.Max(blockData.MaxResources.Get((int)resourceType), 1);
				short currentResource = blockData.CurrResources.Get((int)resourceType);
				DomainManager.Map.UpgradeCollectMaterial(context.Random, collectionConfig, resourceType, maxResource, currentResource, 1, ref itemTemplateId);
				TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
				int currDate = DomainManager.World.GetCurrDate();
				int charId = this.Character.GetId();
				ItemKey itemKey = DomainManager.Item.CreateItem(context, 5, itemTemplateId);
				ItemSourceType target = (ItemSourceType)DomainManager.Extra.GetFarmerAutoCollectStorageType();
				bool flag2 = target != ItemSourceType.Warehouse && target != ItemSourceType.Treasury && target != ItemSourceType.StockStorageGoodsShelf;
				if (flag2)
				{
					target = ItemSourceType.Warehouse;
				}
				DomainManager.Taiwu.AddItem(context, itemKey, 1, target, false);
				DomainManager.LifeRecord.GetLifeRecordCollection().AddFarmerCollectMaterial(charId, currDate, this.Character.GetLocation(), 5, itemTemplateId);
				switch (target)
				{
				case ItemSourceType.Warehouse:
					storageRecordCollection.AddGatherResources(currDate, TaiwuVillageStorageType.Warehouse, charId, 5, itemTemplateId);
					break;
				case ItemSourceType.Treasury:
					storageRecordCollection.AddGatherResourcesToTreasury(currDate, TaiwuVillageStorageType.Treasury, charId, 5, itemTemplateId);
					break;
				case ItemSourceType.StockStorageGoodsShelf:
					storageRecordCollection.AddGatherResourcesToStockStorageGoodsShelf(currDate, TaiwuVillageStorageType.Stock, charId, 5, itemTemplateId);
					break;
				}
			}
		}

		// Token: 0x1700003C RID: 60
		// (get) Token: 0x060013BC RID: 5052 RVA: 0x0013A41D File Offset: 0x0013861D
		public int CollectResourceActionCount
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(0, base.Personality);
			}
		}

		// Token: 0x1700003D RID: 61
		// (get) Token: 0x060013BD RID: 5053 RVA: 0x0013A42B File Offset: 0x0013862B
		public int MigrateResourceSuccessRate
		{
			get
			{
				return this.MigrateResourceBaseSuccessRate + this.MigrateResourceSuccessRateBonus;
			}
		}

		// Token: 0x1700003E RID: 62
		// (get) Token: 0x060013BE RID: 5054 RVA: 0x0013A43A File Offset: 0x0013863A
		public int MigrateResourceBaseSuccessRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(2, base.Personality);
			}
		}

		// Token: 0x1700003F RID: 63
		// (get) Token: 0x060013BF RID: 5055 RVA: 0x0013A448 File Offset: 0x00138648
		public int MigrateResourceSuccessRateBonus
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(47, base.Personality, this.MigrateFailureInfo.Item2);
			}
		}

		// Token: 0x17000040 RID: 64
		// (get) Token: 0x060013C0 RID: 5056 RVA: 0x0013A462 File Offset: 0x00138662
		public int UpgradeBuildingCoreRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(3, base.Personality);
			}
		}

		// Token: 0x060013C1 RID: 5057 RVA: 0x0013A470 File Offset: 0x00138670
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060013C2 RID: 5058 RVA: 0x0013A484 File Offset: 0x00138684
		public override int GetSerializedSize()
		{
			int totalSize = 11;
			bool flag = this.FarmerStorageTypes != null;
			if (flag)
			{
				totalSize += 2 + this.FarmerStorageTypes.Length;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060013C3 RID: 5059 RVA: 0x0013A4CC File Offset: 0x001386CC
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 3;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ArrangementTemplateId;
			pCurrData += 4;
			bool flag = this.FarmerStorageTypes != null;
			if (flag)
			{
				int elementsCount = this.FarmerStorageTypes.Length;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData[i] = (byte)this.FarmerStorageTypes[i];
				}
				pCurrData += elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.Serialize<sbyte, int>(pCurrData, this.MigrateFailureInfo);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060013C4 RID: 5060 RVA: 0x0013A58C File Offset: 0x0013878C
		public unsafe override int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				this.ArrangementTemplateId = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag2 = fieldCount > 1;
			if (flag2)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag3 = elementsCount > 0;
				if (flag3)
				{
					bool flag4 = this.FarmerStorageTypes == null || this.FarmerStorageTypes.Length != (int)elementsCount;
					if (flag4)
					{
						this.FarmerStorageTypes = new sbyte[(int)elementsCount];
					}
					for (int i = 0; i < (int)elementsCount; i++)
					{
						this.FarmerStorageTypes[i] = *(sbyte*)(pCurrData + i);
					}
					pCurrData += elementsCount;
				}
				else
				{
					this.FarmerStorageTypes = null;
				}
			}
			bool flag5 = fieldCount > 2;
			if (flag5)
			{
				pCurrData += SerializationHelper.Deserialize<sbyte, int>(pCurrData, out this.MigrateFailureInfo);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400031A RID: 794
		public const int MapBlockRecoveryLockDuration = 120;

		// Token: 0x0400031B RID: 795
		[Obsolete]
		[SerializableGameDataField]
		public sbyte[] FarmerStorageTypes;

		// Token: 0x0400031C RID: 796
		[TupleElementNames(new string[]
		{
			"resourceType",
			"count"
		})]
		[SerializableGameDataField]
		public ValueTuple<sbyte, int> MigrateFailureInfo;

		// Token: 0x02000961 RID: 2401
		private static class FieldIds
		{
			// Token: 0x04002759 RID: 10073
			public const ushort ArrangementTemplateId = 0;

			// Token: 0x0400275A RID: 10074
			public const ushort FarmerStorageTypes = 1;

			// Token: 0x0400275B RID: 10075
			public const ushort MigrateFailureInfo = 2;

			// Token: 0x0400275C RID: 10076
			public const ushort Count = 3;

			// Token: 0x0400275D RID: 10077
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ArrangementTemplateId",
				"FarmerStorageTypes",
				"MigrateFailureInfo"
			};
		}
	}
}
