using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.Common;
using GameData.Common;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Ai.PrioritizedAction;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization.TaiwuVillageStoragesRecord;
using GameData.Domains.Taiwu.Display;
using GameData.Domains.Taiwu.Display.VillagerRoleArrangement;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.VillagerRole;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class VillagerRoleMerchant : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
{
	private static class FieldIds
	{
		public const ushort ArrangementTemplateId = 0;

		public const ushort BoughtInAmount = 1;

		public const ushort ItemTemplateKey = 2;

		public const ushort DesignatedMerchantType = 3;

		public const ushort CurrentMerchantType = 4;

		public const ushort SelfDecideMerchantType = 5;

		public const ushort Count = 6;

		public static readonly string[] FieldId2FieldName = new string[6] { "ArrangementTemplateId", "BoughtInAmount", "ItemTemplateKey", "DesignatedMerchantType", "CurrentMerchantType", "SelfDecideMerchantType" };
	}

	[SerializableGameDataField]
	public TemplateKey ItemTemplateKey;

	[SerializableGameDataField]
	public int BoughtInAmount;

	[SerializableGameDataField]
	public sbyte DesignatedMerchantType;

	[SerializableGameDataField]
	public sbyte SelfDecideMerchantType;

	[SerializableGameDataField]
	public sbyte CurrentMerchantType;

	public const int FluctuationAdjustmentPerMonth = 10;

	private const int ActionBlockDistanceRange = 5;

	public override short RoleTemplateId => 3;

	public sbyte InteractTargetGrade
	{
		get
		{
			int val = VillagerRoleFormulaImpl.Calculate(24, base.Personality);
			int val2 = VillagerRoleFormulaImpl.Calculate(25, DomainManager.World.GetMaxGradeOfXiangshuInfection());
			return (sbyte)Math.Min(val, val2);
		}
	}

	internal int SellPriceRate => VillagerRoleFormulaImpl.Calculate(30, Character.GetLifeSkillAttainment(15));

	internal int BuyPriceRate => VillagerRoleFormulaImpl.Calculate(29, Character.GetLifeSkillAttainment(15));

	internal int AddFavorA => VillagerRoleFormulaImpl.Calculate(31, Character.GetLifeSkillAttainment(15), base.Personality);

	internal int AddFavorB => VillagerRoleFormulaImpl.Calculate(32, Character.GetLifeSkillAttainment(15), base.Personality);

	[Obsolete]
	public int SellActionRepeatChance => SharedMethods.CalculateMerchantSellActionRepeatChance(Character.GetPersonalities());

	[Obsolete]
	public int SellPricePercent => SharedMethods.CalculateMerchantSellPricePercent(Character.GetPersonalities());

	[Obsolete]
	public int BuyActionRepeatChance => SharedMethods.CalculateMerchantBuyActionRepeatChance(Character.GetPersonalities());

	[Obsolete]
	public int BuyPricePercent => SharedMethods.CalculateMerchantBuyPricePercent(Character.GetPersonalities());

	[Obsolete]
	public int PriceGougingPercentPerMonth => SharedMethods.CalculateMerchantPriceGougingPercentPerMonth(Character.GetPersonalities());

	[Obsolete]
	public int PriceGougingPercentCap => SharedMethods.CalculateMerchantPriceGougingPercentCap(Character.GetPersonalities());

	[Obsolete]
	public int PriceSuppressionPercentPerMonth => SharedMethods.CalculateMerchantPriceSuppressionPercentPerMonth(Character.GetPersonalities());

	[Obsolete]
	public int PriceSuppressionPercentCap => SharedMethods.CalculateMerchantPriceSuppressionPercentCap(Character.GetPersonalities());

	[Obsolete]
	public int UpgradedActionFavorChange => SharedMethods.CalculateMerchantUpgradedActionFavorChange(Character.GetPersonalities());

	public VillagerRoleMerchant()
	{
		DesignatedMerchantType = 7;
		CurrentMerchantType = 7;
	}

	void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
	{
		int arrangementTemplateId = ArrangementTemplateId;
		int num = arrangementTemplateId;
		if (num == 1)
		{
			if (ItemTemplateKey.ItemType < 0)
			{
				ApplySellAction(context, action);
			}
			else
			{
				ApplyBuyAction(context, action);
			}
		}
	}

	private void ApplySellAction(DataContext context, VillagerRoleArrangementAction action)
	{
		int id = Character.GetId();
		Location location = Character.GetLocation();
		List<MapBlockData> blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		blockList.Clear();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, blockList, 5);
		HashSet<int> charSet = ObjectPool<HashSet<int>>.Instance.Get();
		charSet.Clear();
		foreach (MapBlockData item in blockList)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				charSet.Add(item2);
			}
		}
		sbyte targetGrade = InteractTargetGrade;
		int sellPriceRate = SellPriceRate;
		TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
		Inventory inventory = stockStorage.Inventories[1];
		GameData.Domains.Character.Character character = Character.SelectRandomActionTarget(context, charSet, Condition);
		if (character == null)
		{
			OnEnd();
			return;
		}
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		int num = character.GetMaxInventoryLoad() - character.GetCurrInventoryLoad();
		foreach (KeyValuePair<ItemKey, int> item3 in inventory.Items)
		{
			item3.Deconstruct(out var key, out var value);
			ItemKey itemKey = key;
			int num2 = value;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
			if (CheckPrice(itemKey, character) && baseItem.GetWeight() <= num)
			{
				obj.Add(itemKey);
			}
		}
		if (obj.Count > 0)
		{
			ItemKey random = obj.GetRandom(context.Random);
			int num3 = GetPrice(random);
			if (num3 > 0)
			{
				GainResource(context, 6, num3);
			}
			character.ChangeResource(context, 6, -num3);
			DomainManager.Taiwu.RemoveItem(context, random, 1, ItemSourceType.StockStorageGoodsShelf, deleteItem: false, offLine: true);
			character.AddInventoryItem(context, random, 1);
			int id2 = character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddVillagerSoldItem(id, currDate, id2, location, random.ItemType, random.TemplateId, 6, num3);
			TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
			taiwuVillageStoragesRecordCollection.AddVillagerSoldItem(currDate, TaiwuVillageStorageType.Stock, id, random.ItemType, random.TemplateId, num3, 6);
		}
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		if (base.HasChickenUpgradeEffect)
		{
			ApplyChickenUpgradeEffect(context);
		}
		OnEnd();
		bool CheckPrice(ItemKey itemKey2, GameData.Domains.Character.Character character2)
		{
			int value2 = GetPrice(itemKey2);
			return character2.CheckResources(context, 6, value2);
		}
		bool Condition(GameData.Domains.Character.Character character2)
		{
			if (character2.GetInteractionGrade() > targetGrade)
			{
				return false;
			}
			int num4 = character2.GetMaxInventoryLoad() - character2.GetCurrInventoryLoad();
			if (num4 < 0)
			{
				return false;
			}
			foreach (KeyValuePair<ItemKey, int> item4 in inventory.Items)
			{
				item4.Deconstruct(out var key2, out var value2);
				ItemKey itemKey2 = key2;
				int num5 = value2;
				ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
				if (CheckPrice(itemKey2, character2) && baseItem2.GetWeight() <= num4)
				{
					return true;
				}
			}
			return false;
		}
		int GetPrice(ItemKey itemKey2)
		{
			ItemBase baseItem2 = DomainManager.Item.GetBaseItem(itemKey2);
			return Math.Max(0, baseItem2.GetValue() * sellPriceRate / 100);
		}
		void OnEnd()
		{
			blockList.Clear();
			charSet.Clear();
			ObjectPool<List<MapBlockData>>.Instance.Return(blockList);
			ObjectPool<HashSet<int>>.Instance.Return(charSet);
		}
	}

	private void ApplyBuyAction(DataContext context, VillagerRoleArrangementAction action)
	{
		Location location = Character.GetLocation();
		List<MapBlockData> blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
		blockList.Clear();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, blockList, 5);
		HashSet<int> charSet = ObjectPool<HashSet<int>>.Instance.Get();
		charSet.Clear();
		foreach (MapBlockData item in blockList)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				charSet.Add(item2);
			}
		}
		sbyte targetGrade = InteractTargetGrade;
		int buyPriceRate = BuyPriceRate;
		GameData.Domains.Character.Character character = Character.SelectRandomActionTarget(context, charSet, Condition);
		if (character == null)
		{
			OnEnd();
			return;
		}
		List<ItemKey> list = ObjectPool<List<ItemKey>>.Instance.Get();
		list.Clear();
		foreach (var (itemKey2, num2) in character.GetInventory().Items)
		{
			if (itemKey2.ItemType == ItemTemplateKey.ItemType && (ItemTemplateKey.TemplateId < 0 || ItemTemplateHelper.GetItemSubType(itemKey2.ItemType, itemKey2.TemplateId) == ItemTemplateKey.TemplateId) && CheckPrice(itemKey2))
			{
				list.Add(itemKey2);
			}
		}
		if (list.Count == 0)
		{
			ObjectPool<List<ItemKey>>.Instance.Return(list);
			OnEnd();
			return;
		}
		CollectionUtils.Sort(list, delegate(ItemKey a, ItemKey b)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(a.ItemType, a.TemplateId);
			return ItemTemplateHelper.GetGrade(b.ItemType, b.TemplateId).CompareTo(grade);
		});
		ItemKey itemKey3 = list.First();
		int num3 = GetPrice(itemKey3);
		list.Clear();
		ObjectPool<List<ItemKey>>.Instance.Return(list);
		character.ChangeResource(context, 6, num3);
		character.RemoveInventoryItem(context, itemKey3, 1, deleteItem: false);
		CostResource(context, 6, num3);
		GainItem(context, itemKey3, 1);
		BoughtInAmount++;
		DomainManager.Extra.SetVillagerRole(context, Character.GetId());
		int id = Character.GetId();
		int id2 = character.GetId();
		Location location2 = character.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddVillagerBuyItem(id, currDate, id2, location2, itemKey3.ItemType, itemKey3.TemplateId, 6, num3);
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		taiwuVillageStoragesRecordCollection.AddVillagerBuyItem(currDate, TaiwuVillageStorageType.Treasury, id, num3, 6, itemKey3.ItemType, itemKey3.TemplateId);
		if (base.HasChickenUpgradeEffect)
		{
			ApplyChickenUpgradeEffect(context);
		}
		OnEnd();
		static ItemKey CheckInventoryItemKey(Inventory inventory, sbyte itemType, short itemSubType)
		{
			return (itemSubType == -1) ? inventory.GetInventoryItemKeyByItemType(itemType) : inventory.GetInventoryItemKeyByItemSubType(itemSubType);
		}
		bool CheckPrice(ItemKey itemKey4)
		{
			int requiredResourceAmount = GetPrice(itemKey4);
			return CheckResource(6, requiredResourceAmount);
		}
		bool Condition(GameData.Domains.Character.Character character2)
		{
			if (character2.GetInteractionGrade() > targetGrade)
			{
				return false;
			}
			ItemKey itemKey4 = CheckInventoryItemKey(character2.GetInventory(), ItemTemplateKey.ItemType, ItemTemplateKey.TemplateId);
			if (!itemKey4.IsValid())
			{
				return false;
			}
			if (!CheckPrice(itemKey4))
			{
				return false;
			}
			return true;
		}
		int GetPrice(ItemKey itemKey4)
		{
			ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey4);
			return Math.Max(0, baseItem.GetValue() * buyPriceRate / 100);
		}
		void OnEnd()
		{
			blockList.Clear();
			charSet.Clear();
			ObjectPool<List<MapBlockData>>.Instance.Return(blockList);
			ObjectPool<HashSet<int>>.Instance.Return(charSet);
		}
	}

	private void ApplyChickenUpgradeEffect(DataContext context)
	{
		MapAreaData areaByAreaId = DomainManager.Map.GetAreaByAreaId(Character.GetLocation().AreaId);
		short lifeSkillAttainment = Character.GetLifeSkillAttainment(15);
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		Location location = Character.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		for (sbyte b = 0; b < MerchantType.Instance.Count; b++)
		{
			MerchantTypeItem merchantTypeItem = MerchantType.Instance[b];
			if (merchantTypeItem.HeadArea == areaByAreaId.GetTemplateId())
			{
				int addFavorA = AddFavorA;
				DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, b, addFavorA);
				lifeRecordCollection.AddVillagerGetMerchantFavorability(id, currDate, id, location, b);
				lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuCharId, currDate, id, location, b);
				break;
			}
			if (merchantTypeItem.BranchArea == areaByAreaId.GetTemplateId())
			{
				int addFavorB = AddFavorB;
				DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, b, addFavorB);
				lifeRecordCollection.AddVillagerGetMerchantFavorability(id, currDate, id, location, b);
				lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuCharId, currDate, id, location, b);
				break;
			}
			if (b == 2 && areaByAreaId.GetTemplateId() == 11)
			{
				SettlementInfo[] settlementInfos = areaByAreaId.SettlementInfos;
				for (int i = 0; i < settlementInfos.Length; i++)
				{
					SettlementInfo settlementInfo = settlementInfos[i];
					Location location2 = new Location(areaByAreaId.GetAreaId(), settlementInfo.BlockId);
					List<BuildingBlockData> buildingBlockList = DomainManager.Building.GetBuildingBlockList(location2);
					foreach (BuildingBlockData item in buildingBlockList)
					{
						if (item.TemplateId == 318)
						{
							int delta = VillagerRoleFormulaImpl.Calculate(32, lifeSkillAttainment, base.Personality);
							DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, b, delta);
							lifeRecordCollection.AddVillagerGetMerchantFavorability(id, currDate, id, location, b);
							lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuCharId, currDate, id, location, b);
						}
					}
				}
			}
		}
	}

	bool IVillagerRoleSelectLocation.NextLocationFilter(MapBlockData blockData)
	{
		if (blockData.IsNonDeveloped() || blockData.CharacterSet == null)
		{
			return false;
		}
		if (blockData.IsCityTown())
		{
			return true;
		}
		sbyte interactTargetGrade = InteractTargetGrade;
		foreach (int item in blockData.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetInteractionGrade() > interactTargetGrade)
			{
				continue;
			}
			return true;
		}
		return false;
	}

	public override void ExecuteFixedAction(DataContext context)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		if (ArrangementTemplateId < 0 && (WorkData == null || WorkData.WorkType != 1))
		{
			BoolArray64 autoActionStates = base.AutoActionStates;
			if (((BoolArray64)(ref autoActionStates))[5])
			{
				TryAddNextAutoTravelTarget(context, AutoActionBlockFilter);
				AutoMoneyAction(context);
			}
		}
	}

	private bool AutoMoneyAction(DataContext context)
	{
		Location location = Character.GetLocation();
		sbyte interactTargetGrade = InteractTargetGrade;
		MapBlockData block = DomainManager.Map.GetBlock(location);
		int id = Character.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		TaiwuVillageStoragesRecordCollection taiwuVillageStoragesRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
		List<MapBlockData> list = ObjectPool<List<MapBlockData>>.Instance.Get();
		list.Clear();
		DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, list, 5);
		GameData.Domains.Character.Character character = null;
		int num = 0;
		foreach (MapBlockData item in list)
		{
			if (item.CharacterSet == null)
			{
				continue;
			}
			foreach (int item2 in item.CharacterSet)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item2);
				if (element_Objects.GetInteractionGrade() <= interactTargetGrade && AutoActionCharacterFilter(element_Objects))
				{
					int autoActionMoneyIncome = GetAutoActionMoneyIncome(element_Objects);
					if (num < autoActionMoneyIncome)
					{
						character = element_Objects;
						num = autoActionMoneyIncome;
					}
				}
			}
		}
		list.Clear();
		ObjectPool<List<MapBlockData>>.Instance.Return(list);
		if (character == null)
		{
			return false;
		}
		GainResource(context, 6, num);
		lifeRecordCollection.AddVillagerEarnMoney(id, currDate, character.GetId(), 6, num);
		taiwuVillageStoragesRecordCollection.AddVillagerEarnMoney(currDate, TaiwuVillageStorageType.Treasury, id, character.GetId(), 6, num);
		return true;
	}

	private bool AutoActionBlockFilter(MapBlockData blockData)
	{
		if (blockData.CharacterSet == null)
		{
			return false;
		}
		sbyte interactTargetGrade = InteractTargetGrade;
		foreach (int item in blockData.CharacterSet)
		{
			GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(item);
			if (element_Objects.GetInteractionGrade() > interactTargetGrade || !AutoActionCharacterFilter(element_Objects))
			{
				continue;
			}
			return true;
		}
		return false;
	}

	private bool AutoActionCharacterFilter(GameData.Domains.Character.Character character)
	{
		if (character.GetId() == Character.GetId() || character.GetId() == DomainManager.Taiwu.GetTaiwuCharId())
		{
			return false;
		}
		if (!CharacterMatcher.DefValue.CanBeMerchantAutoActionTarget.Match(character))
		{
			return false;
		}
		int adjustedResourceSatisfyingThreshold = character.GetAdjustedResourceSatisfyingThreshold(6);
		int num = VillagerRoleFormulaImpl.Calculate(28, adjustedResourceSatisfyingThreshold);
		return character.GetResource(6) >= num;
	}

	private int GetAutoActionMoneyIncome(GameData.Domains.Character.Character character)
	{
		VillagerRoleFormulaItem formula = VillagerRoleFormula.Instance[26];
		VillagerRoleFormulaItem formula2 = VillagerRoleFormula.Instance[27];
		int adjustedResourceSatisfyingThreshold = character.GetAdjustedResourceSatisfyingThreshold(6);
		short lifeSkillAttainment = Character.GetLifeSkillAttainment(15);
		int arg = formula.Calculate(adjustedResourceSatisfyingThreshold, lifeSkillAttainment);
		return formula2.Calculate(arg);
	}

	protected override void TryAddNextAutoTravelTarget(DataContext context, Predicate<MapBlockData> condition)
	{
		if (Character.GetNpcTravelTargets().Count > 0)
		{
			return;
		}
		Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
		MapBlockData mapBlockData = DomainManager.Map.SelectBlockInCurrentOrNeighborState(context.Random, taiwuVillageLocation, condition, taiwuVillageInfluenceRangeIsLast: true);
		if (mapBlockData != null)
		{
			short taiwuVillageSettlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
			bool flag = DomainManager.Map.IsLocationInSettlementInfluenceRange(mapBlockData.GetLocation(), taiwuVillageSettlementId);
			Location location = Character.GetLocation();
			bool flag2 = DomainManager.Map.IsLocationInSettlementInfluenceRange(location, taiwuVillageSettlementId);
			if (flag != flag2 || !location.IsValid() || !condition(DomainManager.Map.GetBlock(location)))
			{
				NpcTravelTarget target = new NpcTravelTarget(mapBlockData.GetLocation(), 12);
				Character.AddTravelTarget(context, target);
			}
		}
	}

	public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
	{
		return new PeddlingDisplayData
		{
			InteractTargetGrade = InteractTargetGrade,
			BuyPriceRate = BuyPriceRate,
			SellPriceRate = SellPriceRate,
			AddFavorA = AddFavorA,
			AddFavorB = AddFavorB
		};
	}

	public override bool IsSerializedSizeFixed()
	{
		return false;
	}

	public override int GetSerializedSize()
	{
		int num = 16;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 6;
		ptr += 2;
		*(int*)ptr = ArrangementTemplateId;
		ptr += 4;
		*(int*)ptr = BoughtInAmount;
		ptr += 4;
		ptr += ItemTemplateKey.Serialize(ptr);
		*ptr = (byte)DesignatedMerchantType;
		ptr++;
		*ptr = (byte)CurrentMerchantType;
		ptr++;
		*ptr = (byte)SelfDecideMerchantType;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
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
			BoughtInAmount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			ptr += ItemTemplateKey.Deserialize(ptr);
		}
		if (num > 3)
		{
			DesignatedMerchantType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 4)
		{
			CurrentMerchantType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			SelfDecideMerchantType = (sbyte)(*ptr);
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
