using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.Taiwu.VillagerRole
{
	// Token: 0x02000052 RID: 82
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class VillagerRoleMerchant : VillagerRoleBase, IVillagerRoleArrangementExecutor, IVillagerRoleSelectLocation
	{
		// Token: 0x17000058 RID: 88
		// (get) Token: 0x060013FD RID: 5117 RVA: 0x0013C375 File Offset: 0x0013A575
		public override short RoleTemplateId
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060013FE RID: 5118 RVA: 0x0013C378 File Offset: 0x0013A578
		public sbyte InteractTargetGrade
		{
			get
			{
				int grade = VillagerRoleFormulaImpl.Calculate(24, base.Personality);
				int maxGrade = VillagerRoleFormulaImpl.Calculate(25, (int)DomainManager.World.GetMaxGradeOfXiangshuInfection());
				return (sbyte)Math.Min(grade, maxGrade);
			}
		}

		// Token: 0x060013FF RID: 5119 RVA: 0x0013C3B2 File Offset: 0x0013A5B2
		public VillagerRoleMerchant()
		{
			this.DesignatedMerchantType = 7;
			this.CurrentMerchantType = 7;
		}

		// Token: 0x06001400 RID: 5120 RVA: 0x0013C3CC File Offset: 0x0013A5CC
		void IVillagerRoleArrangementExecutor.ExecuteArrangementAction(DataContext context, VillagerRoleArrangementAction action)
		{
			int arrangementTemplateId = this.ArrangementTemplateId;
			int num = arrangementTemplateId;
			if (num == 1)
			{
				bool flag = this.ItemTemplateKey.ItemType < 0;
				if (flag)
				{
					this.ApplySellAction(context, action);
				}
				else
				{
					this.ApplyBuyAction(context, action);
				}
			}
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x06001401 RID: 5121 RVA: 0x0013C411 File Offset: 0x0013A611
		internal int SellPriceRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(30, (int)this.Character.GetLifeSkillAttainment(15));
			}
		}

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x06001402 RID: 5122 RVA: 0x0013C427 File Offset: 0x0013A627
		internal int BuyPriceRate
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(29, (int)this.Character.GetLifeSkillAttainment(15));
			}
		}

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x06001403 RID: 5123 RVA: 0x0013C43D File Offset: 0x0013A63D
		internal int AddFavorA
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(31, (int)this.Character.GetLifeSkillAttainment(15), base.Personality);
			}
		}

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x06001404 RID: 5124 RVA: 0x0013C459 File Offset: 0x0013A659
		internal int AddFavorB
		{
			get
			{
				return VillagerRoleFormulaImpl.Calculate(32, (int)this.Character.GetLifeSkillAttainment(15), base.Personality);
			}
		}

		// Token: 0x06001405 RID: 5125 RVA: 0x0013C478 File Offset: 0x0013A678
		private void ApplySellAction(DataContext context, VillagerRoleArrangementAction action)
		{
			VillagerRoleMerchant.<>c__DisplayClass21_0 CS$<>8__locals1 = new VillagerRoleMerchant.<>c__DisplayClass21_0();
			CS$<>8__locals1.context = context;
			int selfCharId = this.Character.GetId();
			Location location = this.Character.GetLocation();
			CS$<>8__locals1.blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			CS$<>8__locals1.blockList.Clear();
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, CS$<>8__locals1.blockList, 5);
			CS$<>8__locals1.charSet = ObjectPool<HashSet<int>>.Instance.Get();
			CS$<>8__locals1.charSet.Clear();
			foreach (MapBlockData mapBlockData in CS$<>8__locals1.blockList)
			{
				bool flag = mapBlockData.CharacterSet != null;
				if (flag)
				{
					foreach (int id in mapBlockData.CharacterSet)
					{
						CS$<>8__locals1.charSet.Add(id);
					}
				}
			}
			CS$<>8__locals1.targetGrade = this.InteractTargetGrade;
			CS$<>8__locals1.sellPriceRate = this.SellPriceRate;
			TaiwuVillageStorage stockStorage = DomainManager.Extra.GetStockStorage();
			CS$<>8__locals1.inventory = stockStorage.Inventories[1];
			GameData.Domains.Character.Character targetChar = this.Character.SelectRandomActionTarget(CS$<>8__locals1.context, CS$<>8__locals1.charSet, new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<ApplySellAction>g__Condition|0), false);
			bool flag2 = targetChar == null;
			if (flag2)
			{
				CS$<>8__locals1.<ApplySellAction>g__OnEnd|3();
			}
			else
			{
				List<ItemKey> itemKeys = CS$<>8__locals1.context.AdvanceMonthRelatedData.ItemKeys.Occupy();
				int availableLoad = targetChar.GetMaxInventoryLoad() - targetChar.GetCurrInventoryLoad();
				foreach (KeyValuePair<ItemKey, int> keyValuePair in CS$<>8__locals1.inventory.Items)
				{
					ItemKey itemKey2;
					int num;
					keyValuePair.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
					bool flag3 = CS$<>8__locals1.<ApplySellAction>g__CheckPrice|2(itemKey, targetChar) && itemBase.GetWeight() <= availableLoad;
					if (flag3)
					{
						itemKeys.Add(itemKey);
					}
				}
				bool flag4 = itemKeys.Count > 0;
				if (flag4)
				{
					ItemKey selectedItemKey = itemKeys.GetRandom(CS$<>8__locals1.context.Random);
					int price = CS$<>8__locals1.<ApplySellAction>g__GetPrice|1(selectedItemKey);
					bool flag5 = price > 0;
					if (flag5)
					{
						base.GainResource(CS$<>8__locals1.context, 6, price);
					}
					targetChar.ChangeResource(CS$<>8__locals1.context, 6, -price);
					DomainManager.Taiwu.RemoveItem(CS$<>8__locals1.context, selectedItemKey, 1, ItemSourceType.StockStorageGoodsShelf, false, true);
					targetChar.AddInventoryItem(CS$<>8__locals1.context, selectedItemKey, 1, false);
					int targetCharId = targetChar.GetId();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					lifeRecordCollection.AddVillagerSoldItem(selfCharId, currDate, targetCharId, location, selectedItemKey.ItemType, selectedItemKey.TemplateId, 6, price);
					TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
					storageRecordCollection.AddVillagerSoldItem(currDate, TaiwuVillageStorageType.Stock, selfCharId, selectedItemKey.ItemType, selectedItemKey.TemplateId, price, 6);
				}
				CS$<>8__locals1.context.AdvanceMonthRelatedData.ItemKeys.Release(ref itemKeys);
				bool hasChickenUpgradeEffect = base.HasChickenUpgradeEffect;
				if (hasChickenUpgradeEffect)
				{
					this.ApplyChickenUpgradeEffect(CS$<>8__locals1.context);
				}
				CS$<>8__locals1.<ApplySellAction>g__OnEnd|3();
			}
		}

		// Token: 0x06001406 RID: 5126 RVA: 0x0013C800 File Offset: 0x0013AA00
		private void ApplyBuyAction(DataContext context, VillagerRoleArrangementAction action)
		{
			VillagerRoleMerchant.<>c__DisplayClass22_0 CS$<>8__locals1 = new VillagerRoleMerchant.<>c__DisplayClass22_0();
			CS$<>8__locals1.<>4__this = this;
			Location location = this.Character.GetLocation();
			CS$<>8__locals1.blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			CS$<>8__locals1.blockList.Clear();
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, CS$<>8__locals1.blockList, 5);
			CS$<>8__locals1.charSet = ObjectPool<HashSet<int>>.Instance.Get();
			CS$<>8__locals1.charSet.Clear();
			foreach (MapBlockData mapBlockData in CS$<>8__locals1.blockList)
			{
				bool flag = mapBlockData.CharacterSet != null;
				if (flag)
				{
					foreach (int id in mapBlockData.CharacterSet)
					{
						CS$<>8__locals1.charSet.Add(id);
					}
				}
			}
			CS$<>8__locals1.targetGrade = this.InteractTargetGrade;
			CS$<>8__locals1.buyPriceRate = this.BuyPriceRate;
			GameData.Domains.Character.Character targetChar = this.Character.SelectRandomActionTarget(context, CS$<>8__locals1.charSet, new Predicate<GameData.Domains.Character.Character>(CS$<>8__locals1.<ApplyBuyAction>g__Condition|1), false);
			bool flag2 = targetChar == null;
			if (flag2)
			{
				CS$<>8__locals1.<ApplyBuyAction>g__OnEnd|5();
			}
			else
			{
				List<ItemKey> targetItemKeyList = ObjectPool<List<ItemKey>>.Instance.Get();
				targetItemKeyList.Clear();
				foreach (KeyValuePair<ItemKey, int> keyValuePair in targetChar.GetInventory().Items)
				{
					ItemKey itemKey2;
					int num;
					keyValuePair.Deconstruct(out itemKey2, out num);
					ItemKey key = itemKey2;
					bool flag3 = key.ItemType != this.ItemTemplateKey.ItemType;
					if (!flag3)
					{
						bool flag4 = this.ItemTemplateKey.TemplateId >= 0 && ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId) != this.ItemTemplateKey.TemplateId;
						if (!flag4)
						{
							bool flag5 = !CS$<>8__locals1.<ApplyBuyAction>g__CheckPrice|4(key);
							if (!flag5)
							{
								targetItemKeyList.Add(key);
							}
						}
					}
				}
				bool flag6 = targetItemKeyList.Count == 0;
				if (flag6)
				{
					ObjectPool<List<ItemKey>>.Instance.Return(targetItemKeyList);
					CS$<>8__locals1.<ApplyBuyAction>g__OnEnd|5();
				}
				else
				{
					CollectionUtils.Sort<ItemKey>(targetItemKeyList, delegate(ItemKey a, ItemKey b)
					{
						sbyte aPrice = ItemTemplateHelper.GetGrade(a.ItemType, a.TemplateId);
						return ItemTemplateHelper.GetGrade(b.ItemType, b.TemplateId).CompareTo(aPrice);
					});
					ItemKey itemKey = targetItemKeyList.First<ItemKey>();
					int price = CS$<>8__locals1.<ApplyBuyAction>g__GetPrice|3(itemKey);
					targetItemKeyList.Clear();
					ObjectPool<List<ItemKey>>.Instance.Return(targetItemKeyList);
					targetChar.ChangeResource(context, 6, price);
					targetChar.RemoveInventoryItem(context, itemKey, 1, false, false);
					base.CostResource(context, 6, price);
					base.GainItem(context, itemKey, 1);
					this.BoughtInAmount++;
					DomainManager.Extra.SetVillagerRole(context, this.Character.GetId());
					int selfCharId = this.Character.GetId();
					int targetCharId = targetChar.GetId();
					Location targetLocation = targetChar.GetLocation();
					int currDate = DomainManager.World.GetCurrDate();
					LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
					lifeRecordCollection.AddVillagerBuyItem(selfCharId, currDate, targetCharId, targetLocation, itemKey.ItemType, itemKey.TemplateId, 6, price);
					TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
					storageRecordCollection.AddVillagerBuyItem(currDate, TaiwuVillageStorageType.Treasury, selfCharId, price, 6, itemKey.ItemType, itemKey.TemplateId);
					bool hasChickenUpgradeEffect = base.HasChickenUpgradeEffect;
					if (hasChickenUpgradeEffect)
					{
						this.ApplyChickenUpgradeEffect(context);
					}
					CS$<>8__locals1.<ApplyBuyAction>g__OnEnd|5();
				}
			}
		}

		// Token: 0x06001407 RID: 5127 RVA: 0x0013CBBC File Offset: 0x0013ADBC
		private void ApplyChickenUpgradeEffect(DataContext context)
		{
			MapAreaData area = DomainManager.Map.GetAreaByAreaId(this.Character.GetLocation().AreaId);
			short eclecticAttainment = this.Character.GetLifeSkillAttainment(15);
			int merchantId = this.Character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			Location charLocation = this.Character.GetLocation();
			int taiwuId = DomainManager.Taiwu.GetTaiwuCharId();
			sbyte i = 0;
			while ((int)i < MerchantType.Instance.Count)
			{
				MerchantTypeItem config = MerchantType.Instance[i];
				bool flag = config.HeadArea == area.GetTemplateId();
				if (flag)
				{
					int addFavor = this.AddFavorA;
					DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, i, addFavor);
					lifeRecordCollection.AddVillagerGetMerchantFavorability(merchantId, currDate, merchantId, charLocation, i);
					lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuId, currDate, merchantId, charLocation, i);
					break;
				}
				bool flag2 = config.BranchArea == area.GetTemplateId();
				if (flag2)
				{
					int addFavor2 = this.AddFavorB;
					DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, i, addFavor2);
					lifeRecordCollection.AddVillagerGetMerchantFavorability(merchantId, currDate, merchantId, charLocation, i);
					lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuId, currDate, merchantId, charLocation, i);
					break;
				}
				bool flag3 = i == 2 && area.GetTemplateId() == 11;
				if (flag3)
				{
					foreach (SettlementInfo settlementInfo in area.SettlementInfos)
					{
						Location location = new Location(area.GetAreaId(), settlementInfo.BlockId);
						List<BuildingBlockData> buildingBlockList = DomainManager.Building.GetBuildingBlockList(location);
						foreach (BuildingBlockData blockData in buildingBlockList)
						{
							bool flag4 = blockData.TemplateId == 318;
							if (flag4)
							{
								int addFavor3 = VillagerRoleFormulaImpl.Calculate(32, (int)eclecticAttainment, base.Personality);
								DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, i, addFavor3);
								lifeRecordCollection.AddVillagerGetMerchantFavorability(merchantId, currDate, merchantId, charLocation, i);
								lifeRecordCollection.AddVillagerGetMerchantFavorabilityTaiwu(taiwuId, currDate, merchantId, charLocation, i);
							}
						}
					}
				}
				i += 1;
			}
		}

		// Token: 0x06001408 RID: 5128 RVA: 0x0013CE10 File Offset: 0x0013B010
		bool IVillagerRoleSelectLocation.NextLocationFilter(MapBlockData blockData)
		{
			bool flag = blockData.IsNonDeveloped() || blockData.CharacterSet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = blockData.IsCityTown();
				if (flag2)
				{
					result = true;
				}
				else
				{
					sbyte targetGrade = this.InteractTargetGrade;
					foreach (int charId in blockData.CharacterSet)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag3 = character.GetInteractionGrade() > targetGrade;
						if (!flag3)
						{
							return true;
						}
					}
					result = false;
				}
			}
			return result;
		}

		// Token: 0x06001409 RID: 5129 RVA: 0x0013CEBC File Offset: 0x0013B0BC
		public override void ExecuteFixedAction(DataContext context)
		{
			bool flag = this.ArrangementTemplateId >= 0;
			if (!flag)
			{
				bool flag2 = this.WorkData != null && this.WorkData.WorkType == 1;
				if (!flag2)
				{
					bool flag3 = base.AutoActionStates[5];
					if (flag3)
					{
						this.TryAddNextAutoTravelTarget(context, new Predicate<MapBlockData>(this.AutoActionBlockFilter));
						this.AutoMoneyAction(context);
					}
				}
			}
		}

		// Token: 0x0600140A RID: 5130 RVA: 0x0013CF2C File Offset: 0x0013B12C
		private bool AutoMoneyAction(DataContext context)
		{
			Location location = this.Character.GetLocation();
			sbyte targetGrade = this.InteractTargetGrade;
			MapBlockData block = DomainManager.Map.GetBlock(location);
			int merchantId = this.Character.GetId();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			TaiwuVillageStoragesRecordCollection storageRecordCollection = DomainManager.Extra.GetTaiwuVillageStoragesRecordCollection();
			List<MapBlockData> blockList = ObjectPool<List<MapBlockData>>.Instance.Get();
			blockList.Clear();
			DomainManager.Map.GetNeighborBlocks(location.AreaId, location.BlockId, blockList, 5);
			GameData.Domains.Character.Character targetChar = null;
			int targetIncome = 0;
			foreach (MapBlockData blockData in blockList)
			{
				bool flag = blockData.CharacterSet == null;
				if (!flag)
				{
					foreach (int charId in blockData.CharacterSet)
					{
						GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
						bool flag2 = character.GetInteractionGrade() > targetGrade;
						if (!flag2)
						{
							bool flag3 = !this.AutoActionCharacterFilter(character);
							if (!flag3)
							{
								int income = this.GetAutoActionMoneyIncome(character);
								bool flag4 = targetIncome < income;
								if (flag4)
								{
									targetChar = character;
									targetIncome = income;
								}
							}
						}
					}
				}
			}
			blockList.Clear();
			ObjectPool<List<MapBlockData>>.Instance.Return(blockList);
			bool flag5 = targetChar == null;
			bool result;
			if (flag5)
			{
				result = false;
			}
			else
			{
				base.GainResource(context, 6, targetIncome);
				lifeRecordCollection.AddVillagerEarnMoney(merchantId, currDate, targetChar.GetId(), 6, targetIncome);
				storageRecordCollection.AddVillagerEarnMoney(currDate, TaiwuVillageStorageType.Treasury, merchantId, targetChar.GetId(), 6, targetIncome);
				result = true;
			}
			return result;
		}

		// Token: 0x0600140B RID: 5131 RVA: 0x0013D110 File Offset: 0x0013B310
		private bool AutoActionBlockFilter(MapBlockData blockData)
		{
			bool flag = blockData.CharacterSet == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte interactTargetGrade = this.InteractTargetGrade;
				foreach (int charId in blockData.CharacterSet)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					bool flag2 = character.GetInteractionGrade() > interactTargetGrade;
					if (!flag2)
					{
						bool flag3 = this.AutoActionCharacterFilter(character);
						if (flag3)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600140C RID: 5132 RVA: 0x0013D1B4 File Offset: 0x0013B3B4
		private bool AutoActionCharacterFilter(GameData.Domains.Character.Character character)
		{
			bool flag = character.GetId() == this.Character.GetId() || character.GetId() == DomainManager.Taiwu.GetTaiwuCharId();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = !CharacterMatcher.DefValue.CanBeMerchantAutoActionTarget.Match(character);
				if (flag2)
				{
					result = false;
				}
				else
				{
					int moneyThreshold = character.GetAdjustedResourceSatisfyingThreshold(6);
					int moneyRequirement = VillagerRoleFormulaImpl.Calculate(28, moneyThreshold);
					result = (character.GetResource(6) >= moneyRequirement);
				}
			}
			return result;
		}

		// Token: 0x0600140D RID: 5133 RVA: 0x0013D22C File Offset: 0x0013B42C
		private int GetAutoActionMoneyIncome(GameData.Domains.Character.Character character)
		{
			VillagerRoleFormulaItem baseFormula = VillagerRoleFormula.Instance[26];
			VillagerRoleFormulaItem adjustFormula = VillagerRoleFormula.Instance[27];
			int moneyThreshold = character.GetAdjustedResourceSatisfyingThreshold(6);
			short eclecticAttainment = this.Character.GetLifeSkillAttainment(15);
			int baseValue = baseFormula.Calculate(moneyThreshold, (int)eclecticAttainment);
			return adjustFormula.Calculate(baseValue);
		}

		// Token: 0x0600140E RID: 5134 RVA: 0x0013D284 File Offset: 0x0013B484
		protected override void TryAddNextAutoTravelTarget(DataContext context, Predicate<MapBlockData> condition)
		{
			bool flag = this.Character.GetNpcTravelTargets().Count > 0;
			if (!flag)
			{
				Location villageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
				MapBlockData targetBlock = DomainManager.Map.SelectBlockInCurrentOrNeighborState(context.Random, villageLocation, condition, true);
				bool flag2 = targetBlock == null;
				if (!flag2)
				{
					short settlementId = DomainManager.Taiwu.GetTaiwuVillageSettlementId();
					bool targetIsInTaiwuVillageInfluenceRange = DomainManager.Map.IsLocationInSettlementInfluenceRange(targetBlock.GetLocation(), settlementId);
					Location location = this.Character.GetLocation();
					bool currentIsInTaiwuVillageInfluenceRange = DomainManager.Map.IsLocationInSettlementInfluenceRange(location, settlementId);
					bool isMeetOrder = targetIsInTaiwuVillageInfluenceRange == currentIsInTaiwuVillageInfluenceRange;
					bool flag3 = isMeetOrder && location.IsValid() && condition(DomainManager.Map.GetBlock(location));
					if (!flag3)
					{
						NpcTravelTarget travelTarget = new NpcTravelTarget(targetBlock.GetLocation(), 12);
						this.Character.AddTravelTarget(context, travelTarget);
					}
				}
			}
		}

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x0600140F RID: 5135 RVA: 0x0013D366 File Offset: 0x0013B566
		[Obsolete]
		public int SellActionRepeatChance
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantSellActionRepeatChance(this.Character.GetPersonalities());
			}
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06001410 RID: 5136 RVA: 0x0013D378 File Offset: 0x0013B578
		[Obsolete]
		public int SellPricePercent
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantSellPricePercent(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000060 RID: 96
		// (get) Token: 0x06001411 RID: 5137 RVA: 0x0013D38A File Offset: 0x0013B58A
		[Obsolete]
		public int BuyActionRepeatChance
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantBuyActionRepeatChance(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000061 RID: 97
		// (get) Token: 0x06001412 RID: 5138 RVA: 0x0013D39C File Offset: 0x0013B59C
		[Obsolete]
		public int BuyPricePercent
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantBuyPricePercent(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000062 RID: 98
		// (get) Token: 0x06001413 RID: 5139 RVA: 0x0013D3AE File Offset: 0x0013B5AE
		[Obsolete]
		public int PriceGougingPercentPerMonth
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantPriceGougingPercentPerMonth(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000063 RID: 99
		// (get) Token: 0x06001414 RID: 5140 RVA: 0x0013D3C0 File Offset: 0x0013B5C0
		[Obsolete]
		public int PriceGougingPercentCap
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantPriceGougingPercentCap(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000064 RID: 100
		// (get) Token: 0x06001415 RID: 5141 RVA: 0x0013D3D2 File Offset: 0x0013B5D2
		[Obsolete]
		public int PriceSuppressionPercentPerMonth
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantPriceSuppressionPercentPerMonth(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000065 RID: 101
		// (get) Token: 0x06001416 RID: 5142 RVA: 0x0013D3E4 File Offset: 0x0013B5E4
		[Obsolete]
		public int PriceSuppressionPercentCap
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantPriceSuppressionPercentCap(this.Character.GetPersonalities());
			}
		}

		// Token: 0x17000066 RID: 102
		// (get) Token: 0x06001417 RID: 5143 RVA: 0x0013D3F6 File Offset: 0x0013B5F6
		[Obsolete]
		public int UpgradedActionFavorChange
		{
			get
			{
				return GameData.Domains.Taiwu.VillagerRole.SharedMethods.CalculateMerchantUpgradedActionFavorChange(this.Character.GetPersonalities());
			}
		}

		// Token: 0x06001418 RID: 5144 RVA: 0x0013D408 File Offset: 0x0013B608
		public override IVillagerRoleArrangementDisplayData GetArrangementDisplayData()
		{
			return new PeddlingDisplayData
			{
				InteractTargetGrade = this.InteractTargetGrade,
				BuyPriceRate = this.BuyPriceRate,
				SellPriceRate = this.SellPriceRate,
				AddFavorA = this.AddFavorA,
				AddFavorB = this.AddFavorB
			};
		}

		// Token: 0x06001419 RID: 5145 RVA: 0x0013D45C File Offset: 0x0013B65C
		public override bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600141A RID: 5146 RVA: 0x0013D470 File Offset: 0x0013B670
		public override int GetSerializedSize()
		{
			int totalSize = 16;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600141B RID: 5147 RVA: 0x0013D498 File Offset: 0x0013B698
		public unsafe override int Serialize(byte* pData)
		{
			*(short*)pData = 6;
			byte* pCurrData = pData + 2;
			*(int*)pCurrData = this.ArrangementTemplateId;
			pCurrData += 4;
			*(int*)pCurrData = this.BoughtInAmount;
			pCurrData += 4;
			pCurrData += this.ItemTemplateKey.Serialize(pCurrData);
			*pCurrData = (byte)this.DesignatedMerchantType;
			pCurrData++;
			*pCurrData = (byte)this.CurrentMerchantType;
			pCurrData++;
			*pCurrData = (byte)this.SelfDecideMerchantType;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600141C RID: 5148 RVA: 0x0013D518 File Offset: 0x0013B718
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
				this.BoughtInAmount = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag3 = fieldCount > 2;
			if (flag3)
			{
				pCurrData += this.ItemTemplateKey.Deserialize(pCurrData);
			}
			bool flag4 = fieldCount > 3;
			if (flag4)
			{
				this.DesignatedMerchantType = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag5 = fieldCount > 4;
			if (flag5)
			{
				this.CurrentMerchantType = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag6 = fieldCount > 5;
			if (flag6)
			{
				this.SelfDecideMerchantType = *(sbyte*)pCurrData;
				pCurrData++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600141D RID: 5149 RVA: 0x0013D5E0 File Offset: 0x0013B7E0
		[CompilerGenerated]
		internal static ItemKey <ApplyBuyAction>g__CheckInventoryItemKey|22_2(Inventory inventory, sbyte itemType, short itemSubType)
		{
			return (itemSubType == -1) ? inventory.GetInventoryItemKeyByItemType((short)itemType) : inventory.GetInventoryItemKeyByItemSubType(itemSubType);
		}

		// Token: 0x0400031E RID: 798
		[SerializableGameDataField]
		public TemplateKey ItemTemplateKey;

		// Token: 0x0400031F RID: 799
		[SerializableGameDataField]
		public int BoughtInAmount;

		// Token: 0x04000320 RID: 800
		[SerializableGameDataField]
		public sbyte DesignatedMerchantType;

		// Token: 0x04000321 RID: 801
		[SerializableGameDataField]
		public sbyte SelfDecideMerchantType;

		// Token: 0x04000322 RID: 802
		[SerializableGameDataField]
		public sbyte CurrentMerchantType;

		// Token: 0x04000323 RID: 803
		public const int FluctuationAdjustmentPerMonth = 10;

		// Token: 0x04000324 RID: 804
		private const int ActionBlockDistanceRange = 5;

		// Token: 0x02000965 RID: 2405
		private static class FieldIds
		{
			// Token: 0x04002767 RID: 10087
			public const ushort ArrangementTemplateId = 0;

			// Token: 0x04002768 RID: 10088
			public const ushort BoughtInAmount = 1;

			// Token: 0x04002769 RID: 10089
			public const ushort ItemTemplateKey = 2;

			// Token: 0x0400276A RID: 10090
			public const ushort DesignatedMerchantType = 3;

			// Token: 0x0400276B RID: 10091
			public const ushort CurrentMerchantType = 4;

			// Token: 0x0400276C RID: 10092
			public const ushort SelfDecideMerchantType = 5;

			// Token: 0x0400276D RID: 10093
			public const ushort Count = 6;

			// Token: 0x0400276E RID: 10094
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"ArrangementTemplateId",
				"BoughtInAmount",
				"ItemTemplateKey",
				"DesignatedMerchantType",
				"CurrentMerchantType",
				"SelfDecideMerchantType"
			};
		}
	}
}
