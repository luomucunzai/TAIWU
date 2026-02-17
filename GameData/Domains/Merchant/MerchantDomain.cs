using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DLC.FiveLoong;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.Relation;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.Item.Display;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Organization.Display;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.Taiwu.VillagerRole;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Domains.World.Notification;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.Merchant
{
	// Token: 0x02000655 RID: 1621
	[GameDataDomain(14)]
	public class MerchantDomain : BaseGameDataDomain
	{
		// Token: 0x060048DE RID: 18654 RVA: 0x00290AD8 File Offset: 0x0028ECD8
		private void OnInitializedDomainData()
		{
		}

		// Token: 0x060048DF RID: 18655 RVA: 0x00290ADB File Offset: 0x0028ECDB
		private void InitializeOnInitializeGameDataModule()
		{
		}

		// Token: 0x060048E0 RID: 18656 RVA: 0x00290AE0 File Offset: 0x0028ECE0
		private void InitializeOnEnterNewWorld()
		{
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			this.InitMerchantMoney(context);
		}

		// Token: 0x060048E1 RID: 18657 RVA: 0x00290AFC File Offset: 0x0028ECFC
		private void OnLoadedArchiveData()
		{
		}

		// Token: 0x060048E2 RID: 18658 RVA: 0x00290B00 File Offset: 0x0028ED00
		public override void FixAbnormalDomainArchiveData(DataContext context)
		{
			bool flag = DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 69, 71);
			if (flag)
			{
				this.FixNonExistingMerchantGoods(context);
			}
			bool flag2 = DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 74, 32);
			if (flag2)
			{
				this.ReCalcCaravanPath(context);
			}
			bool flag3 = DomainManager.World.GetCurrWorldGameVersion() == null || DomainManager.World.IsCurrWorldBeforeVersion(0, 0, 77, 0);
			if (flag3)
			{
				this.InitMerchantBuyBackData(context);
			}
		}

		// Token: 0x060048E3 RID: 18659 RVA: 0x00290B80 File Offset: 0x0028ED80
		private void FixNonExistingMerchantGoods(DataContext context)
		{
			MerchantDomain.<>c__DisplayClass12_0 CS$<>8__locals1;
			CS$<>8__locals1.itemsToRemove = new List<ItemKey>();
			List<int> modifiedMerchantIds = new List<int>();
			foreach (KeyValuePair<int, MerchantData> keyValuePair in this._merchantData)
			{
				int num;
				MerchantData merchantData3;
				keyValuePair.Deconstruct(out num, out merchantData3);
				int charId = num;
				MerchantData merchantData = merchantData3;
				bool flag = MerchantDomain.<FixNonExistingMerchantGoods>g__FixAbnormalMerchantData|12_0(merchantData, ref CS$<>8__locals1);
				if (flag)
				{
					modifiedMerchantIds.Add(charId);
				}
			}
			foreach (int charId2 in modifiedMerchantIds)
			{
				this.SetElement_MerchantData(charId2, this._merchantData[charId2], context);
			}
			for (int i = 0; i < this._merchantMaxLevelData.Length; i++)
			{
				MerchantData merchant = this._merchantMaxLevelData[i];
				bool flag2 = MerchantDomain.<FixNonExistingMerchantGoods>g__FixAbnormalMerchantData|12_0(this._merchantMaxLevelData[i], ref CS$<>8__locals1);
				if (flag2)
				{
					this.SetElement_MerchantMaxLevelData(i, merchant, context);
				}
			}
			modifiedMerchantIds.Clear();
			foreach (KeyValuePair<int, MerchantData> keyValuePair in this._caravanData)
			{
				int num;
				MerchantData merchantData3;
				keyValuePair.Deconstruct(out num, out merchantData3);
				int caravanId = num;
				MerchantData merchantData2 = merchantData3;
				bool flag3 = MerchantDomain.<FixNonExistingMerchantGoods>g__FixAbnormalMerchantData|12_0(merchantData2, ref CS$<>8__locals1);
				if (flag3)
				{
					modifiedMerchantIds.Add(caravanId);
				}
			}
			foreach (int caravanId2 in modifiedMerchantIds)
			{
				this.SetElement_CaravanData(caravanId2, this._caravanData[caravanId2], context);
			}
		}

		// Token: 0x060048E4 RID: 18660 RVA: 0x00290D74 File Offset: 0x0028EF74
		public void SetMerchantData(int charId, MerchantData merchantData, DataContext context)
		{
			this.SetElement_MerchantData(charId, merchantData, context);
		}

		// Token: 0x060048E5 RID: 18661 RVA: 0x00290D80 File Offset: 0x0028EF80
		[DomainMethod]
		public MerchantData GetMerchantData(DataContext context, int charId)
		{
			MerchantData data;
			bool flag = !this._merchantData.TryGetValue(charId, out data);
			if (flag)
			{
				data = this.CreateMerchantData(context, charId);
			}
			else
			{
				sbyte merchantType;
				DomainManager.Extra.TryGetMerchantCharToType(charId, out merchantType);
				sbyte merchantType2 = data.MerchantType;
				bool flag2 = merchantType2 - 7 <= 1;
				bool flag3 = flag2;
				if (flag3)
				{
					merchantType = data.MerchantType;
				}
				bool flag4 = data.MerchantType != merchantType;
				if (flag4)
				{
					this.RemoveMerchantData(context, charId);
					data = this.CreateMerchantData(context, charId);
				}
				else
				{
					data.Money = this.GetMerchantMoney(context, merchantType);
				}
				MerchantExtraGoodsData merchantExtraGoods;
				DomainManager.Extra.TryGetMerchantExtraGoods(charId, out merchantExtraGoods);
				sbyte oldSeason = (merchantExtraGoods != null) ? merchantExtraGoods.SeasonTemplateId : -1;
				bool flag5 = oldSeason != EventHelper.GetCurrSeason();
				if (flag5)
				{
					data.RefreshSeasonExtraGoods(context, charId, null);
					this.SetMerchantData(charId, data, context);
				}
			}
			return data;
		}

		// Token: 0x060048E6 RID: 18662 RVA: 0x00290E70 File Offset: 0x0028F070
		public int GetMerchantMoney(DataContext context, sbyte merchantType)
		{
			bool flag = !this._merchantMoney.CheckIndex((int)merchantType);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int money = this._merchantMoney[(int)merchantType];
				bool flag2 = money < 0;
				if (flag2)
				{
					money = 0;
					this.SetMerchantMoney(context, merchantType, money);
				}
				result = money;
			}
			return result;
		}

		// Token: 0x060048E7 RID: 18663 RVA: 0x00290EBC File Offset: 0x0028F0BC
		public int SetMerchantMoney(DataContext context, sbyte merchantType, int money)
		{
			bool flag = !this._merchantMoney.CheckIndex((int)merchantType);
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				this._merchantMoney[(int)merchantType] = money;
				this.SetMerchantMoney(this._merchantMoney, context);
				result = money;
			}
			return result;
		}

		// Token: 0x060048E8 RID: 18664 RVA: 0x00290F00 File Offset: 0x0028F100
		[DomainMethod]
		public void SettleTrade(DataContext context, MerchantTradeArguments merchantTradeArguments)
		{
			MerchantDomain.<>c__DisplayClass17_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.merchantTradeArguments = merchantTradeArguments;
			CS$<>8__locals1.tradeMoneySources = CS$<>8__locals1.merchantTradeArguments.TradeMoneySources;
			sbyte buildingMerchantType = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.BuildingMerchantType;
			int buildingMerchantCaravanId = GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsHeadBuildingMerchant);
			int buyMoney = CS$<>8__locals1.merchantTradeArguments.BuyMoney;
			List<ItemSourceChange> itemChangeList = CS$<>8__locals1.merchantTradeArguments.ItemChangeList;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int merchantId = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.Id;
			CS$<>8__locals1.merchantData = CS$<>8__locals1.merchantTradeArguments.MerchantData;
			MerchantExtraGoodsData extraGoods = DomainManager.Extra.GetMerchantExtraGoods(merchantId);
			CS$<>8__locals1.isDebtAreaShop = (CS$<>8__locals1.merchantData.MerchantTemplateId == 53);
			foreach (ItemSourceChange change in itemChangeList)
			{
				foreach (ItemKeyAndCount itemKeyAndCount in change.Items)
				{
					ItemKey itemKey2;
					int num;
					itemKeyAndCount.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					int count = num;
					ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
					int priceChange = change.PriceChanges[itemKey];
					bool flag = count > 0;
					if (flag)
					{
						bool flag2 = CS$<>8__locals1.merchantData.CharId >= 0;
						if (flag2)
						{
							item.RemoveOwner(ItemOwnerType.Merchant, CS$<>8__locals1.merchantData.CharId);
						}
						else
						{
							bool isFromBuilding = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsFromBuilding;
							if (isFromBuilding)
							{
								item.RemoveOwner(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId);
							}
							else
							{
								bool isSpecialBuilding = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsSpecialBuilding;
								if (isSpecialBuilding)
								{
									item.RemoveOwner(ItemOwnerType.BuildingMerchant, -1);
								}
								else
								{
									item.RemoveOwner(ItemOwnerType.Caravan, merchantId);
								}
							}
						}
						bool flag3 = ModificationStateHelper.IsActive(item.GetModificationState(), 8);
						if (flag3)
						{
							if (extraGoods != null)
							{
								extraGoods.Remove(itemKey.Id);
							}
							bool flag4 = ItemTemplateHelper.IsStackable(itemKey.ItemType, itemKey.TemplateId);
							if (flag4)
							{
								DomainManager.Item.RemoveItem(CS$<>8__locals1.context, itemKey);
								ItemKey newItemKey = DomainManager.Item.CreateItem(CS$<>8__locals1.context, itemKey.ItemType, itemKey.TemplateId);
								DomainManager.Taiwu.AddItem(CS$<>8__locals1.context, newItemKey, count, change.ItemSourceType, false);
							}
							else
							{
								DomainManager.Taiwu.AddItem(CS$<>8__locals1.context, itemKey, count, change.ItemSourceType, false);
							}
						}
						else
						{
							DomainManager.Taiwu.AddItem(CS$<>8__locals1.context, itemKey, count, change.ItemSourceType, false);
						}
						bool flag5 = priceChange < 0;
						if (flag5)
						{
							int value = item.GetValue();
							int seniority = ProfessionFormulaImpl.Calculate(95, value) * Math.Abs(count);
							DomainManager.Extra.ChangeProfessionSeniority(CS$<>8__locals1.context, 15, seniority, true, false);
						}
					}
					else
					{
						bool flag6 = count < 0;
						if (flag6)
						{
							DomainManager.Taiwu.RemoveItem(CS$<>8__locals1.context, itemKey, -count, change.ItemSourceType, false, false);
							bool flag7 = CS$<>8__locals1.merchantData.CharId >= 0;
							if (flag7)
							{
								item.SetOwner(ItemOwnerType.Merchant, CS$<>8__locals1.merchantData.CharId);
							}
							else
							{
								bool isFromBuilding2 = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsFromBuilding;
								if (isFromBuilding2)
								{
									item.SetOwner(ItemOwnerType.BuildingMerchant, buildingMerchantCaravanId);
								}
								else
								{
									bool isSpecialBuilding2 = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsSpecialBuilding;
									if (isSpecialBuilding2)
									{
										item.SetOwner(ItemOwnerType.BuildingMerchant, -1);
									}
									else
									{
										item.SetOwner(ItemOwnerType.Caravan, merchantId);
									}
								}
							}
							bool flag8 = priceChange > 0;
							if (flag8)
							{
								int value2 = item.GetValue();
								int seniority2 = ProfessionFormulaImpl.Calculate(96, value2) * Math.Abs(count);
								DomainManager.Extra.ChangeProfessionSeniority(CS$<>8__locals1.context, 15, seniority2, true, false);
							}
						}
					}
				}
			}
			bool flag9 = extraGoods != null;
			if (flag9)
			{
				DomainManager.Extra.SetMerchantExtraGoods(CS$<>8__locals1.context, merchantId, extraGoods);
			}
			CS$<>8__locals1.oldBuyBackData = this.GetMerchantBuyBackData(CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments);
			this.SetMerchantBuyBackData(CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments, CS$<>8__locals1.merchantTradeArguments.MerchantBuyBackData);
			int handledValue = CS$<>8__locals1.tradeMoneySources.Values.Sum();
			switch (CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.MerchantSourceTypeEnum)
			{
			case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
			case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
			{
				bool flag10 = CS$<>8__locals1.merchantData.CharId >= 0;
				if (flag10)
				{
					int realMoney = this.<SettleTrade>g__HandleMerchantMoney|17_1(ref CS$<>8__locals1);
					this.<SettleTrade>g__HandleHeadMoney|17_0(realMoney, ref CS$<>8__locals1);
					this.SetElement_MerchantData(CS$<>8__locals1.merchantData.CharId, CS$<>8__locals1.merchantData, CS$<>8__locals1.context);
				}
				goto IL_638;
			}
			case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
			case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
			{
				bool flag11 = buildingMerchantType > -1;
				if (flag11)
				{
					int realMoney2 = this.<SettleTrade>g__HandleMerchantMoney|17_1(ref CS$<>8__locals1);
					this.<SettleTrade>g__HandleHeadMoney|17_0(realMoney2, ref CS$<>8__locals1);
					bool isHeadBuildingMerchant = CS$<>8__locals1.merchantTradeArguments.OpenShopEventArguments.IsHeadBuildingMerchant;
					if (isHeadBuildingMerchant)
					{
						this.SetElement_MerchantMaxLevelData((int)buildingMerchantType, CS$<>8__locals1.merchantData, CS$<>8__locals1.context);
					}
					else
					{
						DomainManager.Extra.SetBranchMerchantData(CS$<>8__locals1.context, buildingMerchantType, CS$<>8__locals1.merchantData);
					}
				}
				goto IL_638;
			}
			case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
			{
				int realMoney3 = this.<SettleTrade>g__HandleMerchantMoney|17_1(ref CS$<>8__locals1);
				this.<SettleTrade>g__HandleHeadMoney|17_0(realMoney3, ref CS$<>8__locals1);
				this.SetSectStorySpecialMerchantData(CS$<>8__locals1.context, CS$<>8__locals1.merchantData);
				goto IL_638;
			}
			case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
			case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
			case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
			{
				bool flag12 = merchantId > -1;
				if (flag12)
				{
					handledValue = this.<SettleTrade>g__HandleMerchantMoney|17_1(ref CS$<>8__locals1);
					this.SetElement_CaravanData(merchantId, CS$<>8__locals1.merchantData, CS$<>8__locals1.context);
				}
				else
				{
					MerchantData merchantData;
					bool flag13 = merchantId == -1 && this.TryGetElement_CaravanData(-1, out merchantData);
					if (flag13)
					{
						int realMoney4 = this.<SettleTrade>g__HandleMerchantMoney|17_1(ref CS$<>8__locals1);
						this.<SettleTrade>g__HandleHeadMoney|17_0(realMoney4, ref CS$<>8__locals1);
						this.SetElement_CaravanData(merchantId, CS$<>8__locals1.merchantData, CS$<>8__locals1.context);
						this._totalBuyMoney += buyMoney;
						DomainManager.TaiwuEvent.SetListenerEventActionIntArg("ShopActionComplete", "ConchShip_PresetKey_ShopBuyMoney", this._totalBuyMoney);
					}
				}
				goto IL_638;
			}
			}
			throw new ArgumentOutOfRangeException();
			IL_638:
			DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("ShopActionComplete", "ConchShip_PresetKey_ShopHasAnyTrade", true);
			DomainManager.TaiwuEvent.SetListenerEventActionBoolArg("MerchantShopClose", "ConchShip_PresetKey_ShopHasAnyTrade", true);
			bool isDebtAreaShop = CS$<>8__locals1.isDebtAreaShop;
			if (isDebtAreaShop)
			{
				DomainManager.Extra.ChangeAreaSpiritualDebt(CS$<>8__locals1.context, taiwu.GetLocation().AreaId, handledValue, true, true);
			}
			else
			{
				taiwu.ChangeResource(CS$<>8__locals1.context, 6, handledValue);
			}
			bool flag14 = buyMoney > 0;
			if (flag14)
			{
				this.ChangeMerchantCumulativeMoney(CS$<>8__locals1.context, CS$<>8__locals1.merchantData.MerchantType, buyMoney);
			}
			DomainManager.Extra.SetMerchantOverFavorData(CS$<>8__locals1.context, CS$<>8__locals1.merchantData.MerchantType, CS$<>8__locals1.merchantTradeArguments.OverFavorData);
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x00291634 File Offset: 0x0028F834
		public void ChangeMerchantCumulativeMoney(DataContext context, sbyte merchantType, int delta)
		{
			bool flag = delta > 0 && DomainManager.Extra.IsProfessionalSkillUnlockedAndEquipped(71);
			if (flag)
			{
				delta *= 3;
			}
			int[] favorabilityList = DomainManager.Merchant.GetMerchantFavorability();
			bool flag2 = !favorabilityList.CheckIndex((int)merchantType);
			if (!flag2)
			{
				int curCumulativeMoney = favorabilityList[(int)merchantType] + delta;
				int limitCumulativeMoney = this.GetCumulativeMoney(100);
				curCumulativeMoney = Math.Clamp(curCumulativeMoney, 0, limitCumulativeMoney);
				favorabilityList[(int)merchantType] = curCumulativeMoney;
				DomainManager.Merchant.SetMerchantFavorability(favorabilityList, context);
			}
		}

		// Token: 0x060048EA RID: 18666 RVA: 0x002916A4 File Offset: 0x0028F8A4
		[Obsolete]
		public void ChangeMerchantFavorability(DataContext context, sbyte merchantType, int delta)
		{
			int[] favorabilityList = DomainManager.Merchant.GetMerchantFavorability();
			int curFavorability = DomainManager.Merchant.GetFavorability(favorabilityList[(int)merchantType]) + delta;
			curFavorability = Math.Clamp(curFavorability, 0, 100);
			int curMoney = DomainManager.Merchant.GetCumulativeMoney(curFavorability);
			int deltaMoney = curMoney - favorabilityList[(int)merchantType];
			DomainManager.Merchant.ChangeMerchantCumulativeMoney(context, merchantType, deltaMoney);
		}

		// Token: 0x060048EB RID: 18667 RVA: 0x002916F8 File Offset: 0x0028F8F8
		public int GetCumulativeMoney(int favorability)
		{
			int length = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements.Length;
			int[] cumulativeMoneyStages = new int[length];
			for (int i = 0; i < length; i++)
			{
				int cumulative = 0;
				int j = i - 1;
				bool flag = j >= 0 && j < length;
				if (flag)
				{
					cumulative = cumulativeMoneyStages[j];
				}
				int diff = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements[i];
				cumulativeMoneyStages[i] = cumulative + diff;
			}
			int level = favorability / length - 1;
			int offset = favorability % length;
			int result = 0;
			bool flag2 = level >= 0;
			if (flag2)
			{
				int tempLevel = Math.Clamp(level, 0, length - 1);
				result += cumulativeMoneyStages[tempLevel];
			}
			bool flag3 = offset > 0;
			if (flag3)
			{
				int tempLevel2 = Math.Clamp(level + 1, 0, length - 1);
				result += (int)(1f * (float)offset / (float)length * (float)cumulativeMoneyStages[tempLevel2]);
			}
			return result;
		}

		// Token: 0x060048EC RID: 18668 RVA: 0x002917D8 File Offset: 0x0028F9D8
		public int GetFavorability(int cumulativeMoney)
		{
			int favorability = 0;
			for (int i = 0; i < GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements.Length; i++)
			{
				int stage = GlobalConfig.Instance.MerchantFavorabilityMoneyRequirements[i];
				cumulativeMoney -= stage;
				bool flag = cumulativeMoney <= 0;
				if (flag)
				{
					favorability += (int)Math.Floor((double)(10f * (float)(stage + cumulativeMoney) / (float)stage));
					break;
				}
				favorability += 10;
			}
			return favorability;
		}

		// Token: 0x060048ED RID: 18669 RVA: 0x0029184A File Offset: 0x0028FA4A
		[DomainMethod]
		public int GetCurFavorability(sbyte merchantType)
		{
			return Math.Max(0, this.GetFavorabilityWithDelta(merchantType, 0));
		}

		// Token: 0x060048EE RID: 18670 RVA: 0x0029185C File Offset: 0x0028FA5C
		[DomainMethod]
		public int GetFavorabilityWithDelta(sbyte merchantType, int delta = 0)
		{
			int[] merchantFavorabilities = DomainManager.Merchant.GetMerchantFavorability();
			int money = merchantFavorabilities.CheckIndex((int)merchantType) ? merchantFavorabilities[(int)merchantType] : 0;
			bool flag = money + delta < 0;
			int result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				int favorability = this.GetFavorability(money + delta);
				result = favorability;
			}
			return result;
		}

		// Token: 0x060048EF RID: 18671 RVA: 0x002918A8 File Offset: 0x0028FAA8
		[DomainMethod]
		public int[] GetAllFavorability()
		{
			int[] merchantFavorabilities = DomainManager.Merchant.GetMerchantFavorability();
			int[] favorability = new int[merchantFavorabilities.Length];
			for (int i = 0; i < merchantFavorabilities.Length; i++)
			{
				favorability[i] = this.GetFavorability(merchantFavorabilities[i]);
			}
			return favorability;
		}

		// Token: 0x060048F0 RID: 18672 RVA: 0x002918F0 File Offset: 0x0028FAF0
		public bool TryGetMerchantData(int charId, out MerchantData value)
		{
			return this.TryGetElement_MerchantData(charId, out value);
		}

		// Token: 0x060048F1 RID: 18673 RVA: 0x0029190C File Offset: 0x0028FB0C
		public bool MerchantHasTargetItem(int charId, ItemKey itemKey, int amount)
		{
			MerchantData merchantData;
			bool flag = !this._merchantData.TryGetValue(charId, out merchantData);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (int i = 0; i < 7; i++)
				{
					Inventory goods = merchantData.GetGoodsList(i);
					int hasAmount;
					bool flag2 = goods.Items.TryGetValue(itemKey, out hasAmount) && hasAmount >= amount;
					if (flag2)
					{
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060048F2 RID: 18674 RVA: 0x0029197C File Offset: 0x0028FB7C
		public void RemoveExistingMerchantItem(DataContext context, int charId, ItemKey itemKey, int amount)
		{
			MerchantData merchantData;
			bool flag = !this._merchantData.TryGetValue(charId, out merchantData);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
				defaultInterpolatedStringHandler.AppendLiteral("merchant ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
				defaultInterpolatedStringHandler.AppendLiteral(" has no item ");
				defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			for (int i = 0; i < 7; i++)
			{
				Inventory goods = merchantData.GetGoodsList(i);
				int hasAmount;
				bool flag2 = goods.Items.TryGetValue(itemKey, out hasAmount) && hasAmount >= amount;
				if (flag2)
				{
					DomainManager.Item.RemoveOwner(itemKey, ItemOwnerType.Merchant, charId);
					goods.OfflineRemove(itemKey, amount);
					this.SetElement_MerchantData(charId, merchantData, context);
					return;
				}
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(22, 2);
			defaultInterpolatedStringHandler.AppendLiteral("merchant ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
			defaultInterpolatedStringHandler.AppendLiteral(" has no item ");
			defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x060048F3 RID: 18675 RVA: 0x00291A8C File Offset: 0x0028FC8C
		private MerchantData CreateMerchantData(DataContext context, int charId)
		{
			sbyte merchantTemplateId = this.GetMerchantTemplateId(charId);
			MerchantData merchantData = new MerchantData(charId, merchantTemplateId);
			merchantData.GenerateGoods(context, -1, null);
			merchantData.Money = this.GetMerchantMoney(context, merchantData.MerchantType);
			this.AddElement_MerchantData(charId, merchantData, context);
			return merchantData;
		}

		// Token: 0x060048F4 RID: 18676 RVA: 0x00291AD8 File Offset: 0x0028FCD8
		[DomainMethod]
		public sbyte GetMerchantTemplateId(int charId)
		{
			GameData.Domains.Character.Character character;
			bool flag = !DomainManager.Character.TryGetElement_Objects(charId, out character);
			sbyte result;
			if (flag)
			{
				result = -1;
			}
			else
			{
				sbyte merchantType;
				DomainManager.Extra.TryGetMerchantCharToType(charId, out merchantType);
				sbyte merchantLevel = 0;
				short settlementId = character.GetOrganizationInfo().SettlementId;
				bool flag2 = settlementId >= 0;
				if (flag2)
				{
					merchantLevel = DomainManager.Merchant.GetMerchantLevel(merchantType, settlementId);
				}
				sbyte merchantTemplateId = MerchantData.FindMerchantTemplateId(merchantType, merchantLevel);
				result = merchantTemplateId;
			}
			return result;
		}

		// Token: 0x060048F5 RID: 18677 RVA: 0x00291B48 File Offset: 0x0028FD48
		public void RemoveMerchantData(DataContext context, int merchantId)
		{
			MerchantData merchantData;
			bool flag = this.TryGetMerchantData(merchantId, out merchantData);
			if (flag)
			{
				merchantData.RemoveAllGoods(context);
				this.RemoveElement_MerchantData(merchantId, context);
			}
		}

		// Token: 0x060048F6 RID: 18678 RVA: 0x00291B78 File Offset: 0x0028FD78
		public void RemoveObsoleteMerchantData(DataContext context)
		{
			this.RemoveAllGoodsInMerchantBuyBackData(context);
			foreach (int merchantId in this._merchantData.Keys)
			{
				MerchantData data = this._merchantData[merchantId];
				data.RemoveAllGoods(context);
				this.RemoveElement_MerchantData(merchantId, context);
			}
			this.ClearBuildingMerchantData(context);
			this.ClearTempCaravan(context);
		}

		// Token: 0x060048F7 RID: 18679 RVA: 0x00291C04 File Offset: 0x0028FE04
		public void SetVillagerRoleMerchantType(DataContext context)
		{
			List<int> charIdList = new List<int>();
			DomainManager.Extra.GetVillagerRoleCharactersByTemplateId(3, ref charIdList);
			foreach (int charId in charIdList)
			{
				VillagerRoleBase role = DomainManager.Extra.GetVillagerRole(charId);
				VillagerRoleMerchant merchantRole = role as VillagerRoleMerchant;
				bool flag = merchantRole != null;
				if (flag)
				{
					DomainManager.Taiwu.SetMerchantType(context, charId, merchantRole.DesignatedMerchantType, true);
				}
			}
		}

		// Token: 0x060048F8 RID: 18680 RVA: 0x00291C98 File Offset: 0x0028FE98
		private void ClearBuildingMerchantData(DataContext context)
		{
			for (sbyte merchantTypeId = 0; merchantTypeId < 7; merchantTypeId += 1)
			{
				MerchantData data = this.GetElement_MerchantMaxLevelData((int)merchantTypeId);
				bool flag = data == null;
				if (!flag)
				{
					data.RemoveAllGoods(context);
					data.GenerateGoods(context, GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(merchantTypeId, true), null);
					this.SetElement_MerchantMaxLevelData((int)merchantTypeId, data, context);
				}
			}
			for (sbyte merchantTypeId2 = 0; merchantTypeId2 < 7; merchantTypeId2 += 1)
			{
				MerchantData data2 = DomainManager.Extra.BranchMerchantData[(int)merchantTypeId2];
				bool flag2 = data2 == null;
				if (!flag2)
				{
					data2.RemoveAllGoods(context);
					data2.GenerateGoods(context, GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(merchantTypeId2, false), null);
					DomainManager.Extra.SetBranchMerchantData(context, merchantTypeId2, data2);
				}
			}
		}

		// Token: 0x060048F9 RID: 18681 RVA: 0x00291D50 File Offset: 0x0028FF50
		public void InitializeOwnedItems()
		{
			foreach (KeyValuePair<int, MerchantData> keyValuePair in this._merchantData)
			{
				int num;
				MerchantData merchantData3;
				keyValuePair.Deconstruct(out num, out merchantData3);
				int merchantId = num;
				MerchantData merchant = merchantData3;
				MerchantDomain.<InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType.Merchant, merchantId, merchant);
			}
			foreach (KeyValuePair<int, MerchantData> keyValuePair in this._caravanData)
			{
				int num;
				MerchantData merchantData3;
				keyValuePair.Deconstruct(out num, out merchantData3);
				int caravanId = num;
				MerchantData caravan = merchantData3;
				MerchantDomain.<InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType.Caravan, caravanId, caravan);
			}
			sbyte i = 0;
			while ((int)i < this._merchantMaxLevelData.Length)
			{
				MerchantData merchantData = this._merchantMaxLevelData[(int)i];
				bool flag = merchantData == null;
				if (!flag)
				{
					int caravanId2 = GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(i, true);
					MerchantDomain.<InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType.BuildingMerchant, caravanId2, merchantData);
				}
				i += 1;
			}
			sbyte j = 0;
			while ((int)j < DomainManager.Extra.BranchMerchantData.Length)
			{
				MerchantData merchantData2 = DomainManager.Extra.BranchMerchantData[(int)j];
				bool flag2 = merchantData2 == null;
				if (!flag2)
				{
					int caravanId3 = GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(j, false);
					MerchantDomain.<InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType.BuildingMerchant, caravanId3, merchantData2);
				}
				j += 1;
			}
			SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
			bool flag3 = sectStorySpecialMerchant != null;
			if (flag3)
			{
				bool flag4 = sectStorySpecialMerchant.MerchantData != null;
				if (flag4)
				{
					MerchantDomain.<InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType.BuildingMerchant, -1, sectStorySpecialMerchant.MerchantData);
				}
			}
		}

		// Token: 0x060048FA RID: 18682 RVA: 0x00291EF8 File Offset: 0x002900F8
		public bool HasNewGoods(GameData.Domains.Character.Character character)
		{
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			OrganizationItem orgConfig = Organization.Instance[orgInfo.OrgTemplateId];
			OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(orgInfo);
			return orgConfig.IsCivilian && orgInfo.OrgTemplateId != 0 && orgInfo.Grade == 4 && character.GetCurrAge() >= orgMemberConfig.IdentityActiveAge && !this._merchantData.ContainsKey(character.GetId());
		}

		// Token: 0x060048FB RID: 18683 RVA: 0x00291F68 File Offset: 0x00290168
		public sbyte GetMerchantLevel(sbyte merchantType, short settlementId)
		{
			Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
			sbyte orgTemplateId = settlement.GetOrgTemplateId();
			OrganizationItem orgConfig = Organization.Instance[orgTemplateId];
			MerchantTypeItem merchantTypeConfig = MerchantType.Instance[merchantType];
			List<Settlement> civilianSettlements = new List<Settlement>();
			DomainManager.Organization.GetAllCivilianSettlements(civilianSettlements);
			foreach (Settlement civilianSettlement in civilianSettlements)
			{
				bool flag = civilianSettlement.GetOrgTemplateId() == orgTemplateId;
				if (flag)
				{
					settlement = civilianSettlement;
					break;
				}
			}
			short resource = (merchantTypeConfig.CityAttributeType == EMerchantTypeCityAttributeType.Safety) ? settlement.GetSafety() : settlement.GetCulture();
			sbyte level = orgConfig.MerchantLevel + ((resource >= 50) ? 1 : -1);
			return Math.Clamp(level, 0, 6);
		}

		// Token: 0x060048FC RID: 18684 RVA: 0x0029204C File Offset: 0x0029024C
		[DomainMethod]
		public void GmCmd_AddItem(DataContext context, int charId, sbyte itemType, short templateId, int count, int level)
		{
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			bool flag = !character.IsMerchant(character.GetOrganizationInfo());
			if (!flag)
			{
				MerchantData merchantData = this.GetMerchantData(context, charId);
				sbyte maxLevel = Math.Min(merchantData.MerchantConfig.Level, 6);
				level = Math.Clamp(level, 0, (int)maxLevel);
				Inventory inventory = merchantData.GetGoodsList(level);
				ItemKey itemKey = DomainManager.Item.CreateItem(context, itemType, templateId);
				inventory.OfflineAdd(itemKey, count);
				DomainManager.Item.SetOwner(itemKey, ItemOwnerType.Merchant, charId);
				this.SetElement_MerchantData(charId, merchantData, context);
			}
		}

		// Token: 0x060048FD RID: 18685 RVA: 0x002920E1 File Offset: 0x002902E1
		[DomainMethod]
		public MerchantOverFavorData GetMerchantOverFavorData(sbyte merchantType)
		{
			return DomainManager.Extra.GetMerchantOverFavorData(merchantType);
		}

		// Token: 0x060048FE RID: 18686 RVA: 0x002920F0 File Offset: 0x002902F0
		[DomainMethod]
		public List<MerchantInfoAreaData> GetMerchantInfoAreaDataList(sbyte merchantType)
		{
			List<MerchantInfoAreaData> merchantInfoAreaDataList = new List<MerchantInfoAreaData>();
			Dictionary<short, HashSet<int>> areaMerchantCharDict = DomainManager.Character.GetAreaMerchantCharDict((int)merchantType);
			foreach (KeyValuePair<short, HashSet<int>> keyValuePair in areaMerchantCharDict)
			{
				short num;
				HashSet<int> hashSet;
				keyValuePair.Deconstruct(out num, out hashSet);
				short areaTemplateId2 = num;
				HashSet<int> set = hashSet;
				MerchantInfoAreaData merchantInfoAreaData = new MerchantInfoAreaData
				{
					AreaTemplateId = areaTemplateId2,
					MerchantCount = set.Count
				};
				merchantInfoAreaDataList.Add(merchantInfoAreaData);
			}
			foreach (KeyValuePair<int, CaravanPath> keyValuePair2 in this._caravanDict)
			{
				int num2;
				CaravanPath caravanPath2;
				keyValuePair2.Deconstruct(out num2, out caravanPath2);
				int caravanId = num2;
				CaravanPath caravanPath = caravanPath2;
				bool flag = this._caravanData[caravanId].MerchantType != merchantType;
				if (!flag)
				{
					short areaId = caravanPath.GetCurrLocation().AreaId;
					MapAreaData areaData = DomainManager.Map.GetAreaByAreaId(areaId);
					short areaTemplateId = areaData.GetTemplateId();
					int index = merchantInfoAreaDataList.FindIndex((MerchantInfoAreaData d) => d.AreaTemplateId == areaTemplateId);
					bool flag2 = index >= 0;
					MerchantInfoAreaData merchantInfoAreaData2;
					if (flag2)
					{
						merchantInfoAreaData2 = merchantInfoAreaDataList[index];
					}
					else
					{
						merchantInfoAreaData2 = new MerchantInfoAreaData
						{
							AreaTemplateId = areaTemplateId
						};
						merchantInfoAreaDataList.Add(merchantInfoAreaData2);
					}
					merchantInfoAreaData2.CaravanCount++;
				}
			}
			return merchantInfoAreaDataList;
		}

		// Token: 0x060048FF RID: 18687 RVA: 0x0029229C File Offset: 0x0029049C
		[DomainMethod]
		public List<MerchantInfoCaravanData> GetMerchantInfoCaravanDataList(DataContext context, sbyte merchantType)
		{
			List<MerchantInfoCaravanData> merchantInfoCaravanDataList = new List<MerchantInfoCaravanData>();
			foreach (KeyValuePair<int, CaravanPath> keyValuePair in this._caravanDict)
			{
				int num;
				CaravanPath caravanPath2;
				keyValuePair.Deconstruct(out num, out caravanPath2);
				int caravanId = num;
				CaravanPath caravanPath = caravanPath2;
				MerchantData merchantData = this._caravanData[caravanId];
				bool flag = merchantType >= 0 && merchantData.MerchantType != merchantType;
				if (!flag)
				{
					CaravanExtraData caravanExtraData;
					bool flag2 = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
					if (flag2)
					{
						caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
					}
					MerchantInfoCaravanData merchantInfoAreaData = new MerchantInfoCaravanData
					{
						CaravanId = caravanId,
						MerchantTemplateId = (short)merchantData.MerchantTemplateId,
						CurrentAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetCurrLocation().AreaId).GetTemplateId(),
						TargetAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetDestLocation().AreaId).GetTemplateId(),
						StartAreaTemplateId = DomainManager.Map.GetAreaByAreaId(caravanPath.GetSrcLocation().AreaId).GetTemplateId(),
						RemainSettlementInfoList = new List<SettlementDisplayData>(),
						RemainNodeCount = caravanPath.MoveNodes.Count - 1,
						ExtraData = caravanExtraData,
						IsInBrokenArea = MapAreaData.IsBrokenArea(caravanPath.GetCurrLocation().AreaId),
						CaravanPath = new CaravanPath(caravanPath)
					};
					bool flag3 = caravanExtraData.SettlementIdList != null;
					if (flag3)
					{
						foreach (short id in caravanExtraData.SettlementIdList)
						{
							SettlementDisplayData displayData = DomainManager.Organization.GetDisplayData(id);
							merchantInfoAreaData.RemainSettlementInfoList.Add(displayData);
						}
					}
					merchantInfoCaravanDataList.Add(merchantInfoAreaData);
				}
			}
			return merchantInfoCaravanDataList;
		}

		// Token: 0x06004900 RID: 18688 RVA: 0x002924B8 File Offset: 0x002906B8
		[DomainMethod]
		public List<MerchantInfoMerchantData> GetMerchantInfoMerchantDataList(sbyte merchantType)
		{
			List<MerchantInfoMerchantData> merchantInfoMerchantDataList = new List<MerchantInfoMerchantData>();
			Dictionary<short, HashSet<int>> areaMerchantCharDict = DomainManager.Character.GetAreaMerchantCharDict((int)merchantType);
			foreach (KeyValuePair<short, HashSet<int>> keyValuePair in areaMerchantCharDict)
			{
				short num;
				HashSet<int> hashSet;
				keyValuePair.Deconstruct(out num, out hashSet);
				HashSet<int> set = hashSet;
				foreach (int charId in set)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
					int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
					RelatedCharacter relatedCharacter;
					short favorability = DomainManager.Character.TryGetRelation(charId, taiwuCharId, out relatedCharacter) ? relatedCharacter.Favorability : short.MinValue;
					short areaId = character.GetValidLocation().AreaId;
					MapAreaData areaData = DomainManager.Map.GetAreaByAreaId(areaId);
					short areaTemplateId = areaData.GetTemplateId();
					SettlementCharacter settlementChar;
					DomainManager.Organization.TryGetSettlementCharacter(charId, out settlementChar);
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementChar.GetSettlementId());
					MerchantInfoMerchantData merchantInfoMerchantData = new MerchantInfoMerchantData
					{
						CharId = charId,
						BehaviorType = character.GetBehaviorType(),
						Favorability = favorability,
						NameRelatedData = DomainManager.Character.GetNameRelatedData(charId),
						MerchantTemplateId = (short)DomainManager.Merchant.GetMerchantTemplateId(charId),
						CurrentAreaTemplateId = areaTemplateId,
						OrgTemplateId = (short)settlementChar.GetOrgTemplateId(),
						FullBlockName = DomainManager.Map.GetBlockFullName(settlement.GetLocation())
					};
					merchantInfoMerchantDataList.Add(merchantInfoMerchantData);
				}
			}
			return merchantInfoMerchantDataList;
		}

		// Token: 0x06004901 RID: 18689 RVA: 0x00292690 File Offset: 0x00290890
		[DomainMethod]
		public SectStorySpecialMerchant GetSectStorySpecialMerchantData(DataContext context)
		{
			int curData = DomainManager.World.GetCurrDate();
			SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
			MerchantItem merchantItem = Merchant.Instance[51];
			bool flag = ((sectStorySpecialMerchant != null) ? sectStorySpecialMerchant.MerchantData : null) == null;
			if (flag)
			{
				sectStorySpecialMerchant = new SectStorySpecialMerchant();
				sectStorySpecialMerchant.MerchantExtraGoodsData = new MerchantExtraGoodsData();
				MerchantData merchantData = new MerchantData(-1, merchantItem.TemplateId);
				merchantData.GenerateGoods(context, merchantItem.Level, -1, sectStorySpecialMerchant.MerchantExtraGoodsData);
				sectStorySpecialMerchant.MerchantExtraGoodsData.SeasonTemplateId = -1;
				sectStorySpecialMerchant.MerchantData = merchantData;
				sectStorySpecialMerchant.RefreshTime = curData;
				DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
			}
			else
			{
				bool flag2 = sectStorySpecialMerchant.RefreshTime + (int)merchantItem.RefreshInterval <= curData;
				if (flag2)
				{
					sectStorySpecialMerchant.MerchantData.RemoveAllGoods(context);
					MerchantBuyBackData sectStorySpecialMerchantBuyBackData = this._sectStorySpecialMerchantBuyBackData;
					if (sectStorySpecialMerchantBuyBackData != null)
					{
						sectStorySpecialMerchantBuyBackData.RemoveAllGoods(context);
					}
					MerchantData merchantData2 = new MerchantData(-1, merchantItem.TemplateId);
					sectStorySpecialMerchant.MerchantExtraGoodsData.Clear();
					merchantData2.GenerateGoods(context, merchantItem.Level, -1, sectStorySpecialMerchant.MerchantExtraGoodsData);
					sectStorySpecialMerchant.MerchantData = merchantData2;
					sectStorySpecialMerchant.RefreshTime = curData;
				}
			}
			sectStorySpecialMerchant.MerchantData.Money = DomainManager.Merchant.GetMerchantMoney(context, merchantItem.MerchantType);
			DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
			return sectStorySpecialMerchant;
		}

		// Token: 0x06004902 RID: 18690 RVA: 0x002927E0 File Offset: 0x002909E0
		private void SetSectStorySpecialMerchantData(DataContext context, MerchantData merchantData)
		{
			SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
			sectStorySpecialMerchant.MerchantData = merchantData;
			DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
		}

		// Token: 0x06004903 RID: 18691 RVA: 0x00292810 File Offset: 0x00290A10
		public ItemKey CreateMerchantRandomItem(DataContext context, short merchantTemplateId)
		{
			ItemKey itemKey = ItemKey.Invalid;
			MerchantItem config = Merchant.Instance[(int)merchantTemplateId];
			List<PresetItemTemplateIdGroup> allPresetList = new List<PresetItemTemplateIdGroup>();
			for (int i = 0; i <= 7; i++)
			{
				IList<PresetItemTemplateIdGroup> presetList = MerchantData.GetGoodsPreset(config, i);
				bool flag = presetList == null || presetList.Count <= 0;
				if (!flag)
				{
					allPresetList.AddRange(presetList);
				}
			}
			bool flag2 = allPresetList.Count > 0;
			if (flag2)
			{
				PresetItemTemplateIdGroup preset = allPresetList.GetRandom(context.Random);
				itemKey = DomainManager.Item.CreateItem(context, preset.ItemType, preset.StartId);
			}
			return itemKey;
		}

		// Token: 0x06004904 RID: 18692 RVA: 0x002928BC File Offset: 0x00290ABC
		[DomainMethod]
		public bool CanRefreshMerchantGoods(DataContext context, bool consume = false)
		{
			bool flag = DomainManager.Extra.GetTotalActionPointsRemaining() < GlobalConfig.Instance.RefreshItemApCost;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				if (consume)
				{
					DomainManager.World.ConsumeActionPoint(context, GlobalConfig.Instance.RefreshItemApCost);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004905 RID: 18693 RVA: 0x0029290C File Offset: 0x00290B0C
		[DomainMethod]
		public bool RefreshMerchantGoods(DataContext context, int charOrCaravanId, bool isChar, sbyte level, bool isFromBuilding, bool isHeadBuildingMerchant, sbyte buildingMerchantType)
		{
			bool flag = !this.CanRefreshMerchantGoods(context, true);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameData.Domains.Character.Character character = null;
				int caravanId = -1;
				MerchantData merchantData;
				if (isChar)
				{
					character = DomainManager.Character.GetElement_Objects(charOrCaravanId);
					merchantData = this.GetMerchantData(context, charOrCaravanId);
				}
				else if (isFromBuilding)
				{
					caravanId = GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, isHeadBuildingMerchant);
					merchantData = this.GetBuildingMerchantData(context, buildingMerchantType, isHeadBuildingMerchant);
				}
				else
				{
					caravanId = charOrCaravanId;
					merchantData = this.GetCaravanMerchantData(context, charOrCaravanId);
				}
				if (isFromBuilding)
				{
					DomainManager.Extra.SetMerchantExtraGoods(context, GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(buildingMerchantType, isHeadBuildingMerchant), merchantData.OfflineRefreshGoods(context, level, character, caravanId));
				}
				else
				{
					DomainManager.Extra.SetMerchantExtraGoods(context, charOrCaravanId, merchantData.OfflineRefreshGoods(context, level, character, caravanId));
				}
				if (isChar)
				{
					this.SetMerchantData(charOrCaravanId, merchantData, context);
				}
				else if (isFromBuilding)
				{
					if (isHeadBuildingMerchant)
					{
						this.SetElement_MerchantMaxLevelData((int)buildingMerchantType, merchantData, context);
					}
					else
					{
						DomainManager.Extra.SetBranchMerchantData(context, buildingMerchantType, merchantData);
					}
				}
				else
				{
					this.SetCaravanData(charOrCaravanId, merchantData, context);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004906 RID: 18694 RVA: 0x00292A28 File Offset: 0x00290C28
		private static MerchantDomain.SkillBookTradeInfo CreateSkillBookTradeInfo(DataContext context, int charId)
		{
			MerchantDomain.SkillBookTradeInfo skillBookTradeInfo = new MerchantDomain.SkillBookTradeInfo(charId);
			GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
			DomainManager.Character.GetSkillBookLibrary(context, character, skillBookTradeInfo.PrivateSkillBooks, skillBookTradeInfo.PrivateSkillBooks);
			OrganizationInfo orgInfo = character.GetOrganizationInfo();
			bool flag = OrganizationDomain.IsSect(orgInfo.OrgTemplateId);
			if (flag)
			{
				foreach (ItemKey itemKey in skillBookTradeInfo.PrivateSkillBooks)
				{
					SkillBookItem bookCfg = Config.SkillBook.Instance[itemKey.TemplateId];
					bool flag2 = bookCfg.CombatSkillTemplateId < 0;
					if (flag2)
					{
						break;
					}
					CombatSkillItem skillCfg = CombatSkill.Instance[bookCfg.CombatSkillTemplateId];
					bool flag3 = skillCfg.SectId != orgInfo.OrgTemplateId;
					if (!flag3)
					{
						skillBookTradeInfo.SectSkillBooks.Add(itemKey);
					}
				}
			}
			return skillBookTradeInfo;
		}

		// Token: 0x06004907 RID: 18695 RVA: 0x00292B2C File Offset: 0x00290D2C
		[DomainMethod]
		public void FinishBookTrade(DataContext context, int charId, bool isFavor)
		{
			bool flag = this._skillBookTradeInfo == null;
			if (!flag)
			{
				GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(charId);
				bool flag2 = this._skillBookTradeInfo.BoughtBooksFromTaiwu.Count > 0;
				if (flag2)
				{
					character.AddInventoryItemList(context, this._skillBookTradeInfo.BoughtBooksFromTaiwu);
				}
				DomainManager.Character.DealWithSkillBookLibraryAfterTrading(context, character, this._skillBookTradeInfo.SoldBooksToTaiwu, this._skillBookTradeInfo.PrivateSkillBooks);
				this._skillBookTradeInfo = null;
			}
		}

		// Token: 0x06004908 RID: 18696 RVA: 0x00292BAC File Offset: 0x00290DAC
		[DomainMethod]
		public void ExchangeBook(DataContext context, int npcId, List<ItemDisplayData> boughtItems, List<ItemDisplayData> soldItems, int selfAuthority, int npcAuthority)
		{
			bool flag = boughtItems != null;
			if (flag)
			{
				foreach (ItemDisplayData itemData in boughtItems)
				{
					ItemKey itemKey = itemData.Key;
					ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
					item.RemoveOwner(ItemOwnerType.Library, npcId);
					DomainManager.Taiwu.AddItem(context, itemKey, itemData.Amount, itemData.ItemSourceType, false);
					bool flag2 = this._skillBookTradeInfo.BoughtBooksFromTaiwu.Contains(itemKey);
					if (flag2)
					{
						this._skillBookTradeInfo.BoughtBooksFromTaiwu.Remove(itemKey);
					}
					else
					{
						this._skillBookTradeInfo.PrivateSkillBooks.Remove(itemKey);
						this._skillBookTradeInfo.SectSkillBooks.Remove(itemKey);
						this._skillBookTradeInfo.SoldBooksToTaiwu.Add(itemKey);
						short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
						bool flag3 = itemSubType == 1000;
						if (flag3)
						{
							int seniority = ProfessionFormulaImpl.Calculate(102, item.GetValue());
							DomainManager.Extra.ChangeProfessionSeniority(context, 16, seniority, true, false);
						}
						else
						{
							int seniority2 = ProfessionFormulaImpl.Calculate(51, item.GetValue());
							DomainManager.Extra.ChangeProfessionSeniority(context, 7, seniority2, true, false);
						}
					}
				}
			}
			bool flag4 = soldItems != null;
			if (flag4)
			{
				foreach (ItemDisplayData itemData2 in soldItems)
				{
					ItemKey itemKey2 = itemData2.Key;
					this._skillBookTradeInfo.BoughtBooksFromTaiwu.Add(itemKey2);
					DomainManager.Taiwu.RemoveItem(context, itemKey2, itemData2.Amount, itemData2.ItemSourceType, false, false);
					ItemBase item2 = DomainManager.Item.GetBaseItem(itemKey2);
					item2.SetOwner(ItemOwnerType.Library, npcId);
				}
			}
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			taiwu.SpecifyResource(context, 7, selfAuthority);
			GameData.Domains.Character.Character npc = DomainManager.Character.GetElement_Objects(npcId);
			npc.SpecifyResource(context, 7, npcAuthority);
			DomainManager.TaiwuEvent.CheckTaiwuStatusImmediately();
		}

		// Token: 0x06004909 RID: 18697 RVA: 0x00292E18 File Offset: 0x00291018
		[DomainMethod]
		public List<ItemDisplayData> GetTradeBookDisplayData(DataContext context, int npcId, bool isFavor)
		{
			bool flag = this._skillBookTradeInfo == null || this._skillBookTradeInfo.CharId != npcId;
			if (flag)
			{
				this._skillBookTradeInfo = MerchantDomain.CreateSkillBookTradeInfo(context, npcId);
			}
			return DomainManager.Item.GetItemDisplayDataListOptional(isFavor ? this._skillBookTradeInfo.PrivateSkillBooks : this._skillBookTradeInfo.SectSkillBooks, -1, -1, false);
		}

		// Token: 0x0600490A RID: 18698 RVA: 0x00292E7F File Offset: 0x0029107F
		[DomainMethod]
		public List<ItemDisplayData> GetTradeBackBookDisplayData()
		{
			return DomainManager.Item.GetItemDisplayDataListOptional(this._skillBookTradeInfo.BoughtBooksFromTaiwu, -1, -1, false);
		}

		// Token: 0x0600490B RID: 18699 RVA: 0x00292E9C File Offset: 0x0029109C
		[DomainMethod]
		public MerchantBuyBackData GetMerchantBuyBackData(OpenShopEventArguments openShopEventArguments)
		{
			switch (openShopEventArguments.MerchantSourceTypeEnum)
			{
			case OpenShopEventArguments.EMerchantSourceType.None:
				break;
			case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
			{
				MerchantBuyBackData buyBackData;
				return this._merchantBuyBackData.TryGetValue(openShopEventArguments.Id, out buyBackData) ? buyBackData : null;
			}
			case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
				return this._merchantMaxLevelBuyBackData[(int)openShopEventArguments.BuildingMerchantType];
			case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
				return this._branchMerchantBuyBackData[(int)openShopEventArguments.BuildingMerchantType];
			case OpenShopEventArguments.EMerchantSourceType.SettlementTreasury:
				break;
			case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
				return this._sectStorySpecialMerchantBuyBackData;
			case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
			{
				MerchantBuyBackData buyBackData;
				return this._caravanBuyBackData.TryGetValue(openShopEventArguments.Id, out buyBackData) ? buyBackData : null;
			}
			case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
				return this._tempMerchantBuyBackData;
			case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
				return this._tempMerchantBuyBackData;
			case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
			{
				MerchantBuyBackData buyBackData;
				return this._merchantBuyBackData.TryGetValue(openShopEventArguments.Id, out buyBackData) ? buyBackData : null;
			}
			default:
				throw new ArgumentOutOfRangeException();
			}
			return null;
		}

		// Token: 0x0600490C RID: 18700 RVA: 0x00292F8C File Offset: 0x0029118C
		private void SetMerchantBuyBackData(OpenShopEventArguments openShopEventArguments, MerchantBuyBackData merchantBuyBackData)
		{
			switch (openShopEventArguments.MerchantSourceTypeEnum)
			{
			case OpenShopEventArguments.EMerchantSourceType.None:
				break;
			case OpenShopEventArguments.EMerchantSourceType.NormalCharacter:
				this._merchantBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.MerchantHeadBuilding:
				this._merchantMaxLevelBuyBackData[(int)openShopEventArguments.BuildingMerchantType] = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.MerchantBranchBuilding:
				this._branchMerchantBuyBackData[(int)openShopEventArguments.BuildingMerchantType] = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.SettlementTreasury:
				break;
			case OpenShopEventArguments.EMerchantSourceType.SpecialBuilding:
				this._sectStorySpecialMerchantBuyBackData = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.NormalCaravan:
				this._caravanBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.AdventureCaravan:
				this._tempMerchantBuyBackData = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan:
				this._tempMerchantBuyBackData = merchantBuyBackData;
				break;
			case OpenShopEventArguments.EMerchantSourceType.SpecifiedOnBuildingMerchantType:
				this._merchantBuyBackData[openShopEventArguments.Id] = merchantBuyBackData;
				break;
			default:
				throw new ArgumentOutOfRangeException();
			}
		}

		// Token: 0x0600490D RID: 18701 RVA: 0x00293060 File Offset: 0x00291260
		public bool RemoveBuyBackItem(ItemKey itemKey)
		{
			MerchantDomain.<>c__DisplayClass62_0 CS$<>8__locals1;
			CS$<>8__locals1.itemKey = itemKey;
			bool flag = MerchantDomain.<RemoveBuyBackItem>g__RemoveButBackItemInInventory|62_0(this._merchantBuyBackData.Values, ref CS$<>8__locals1);
			bool result;
			if (flag)
			{
				result = true;
			}
			else
			{
				bool flag2 = MerchantDomain.<RemoveBuyBackItem>g__RemoveButBackItemInInventory|62_0(this._caravanBuyBackData.Values, ref CS$<>8__locals1);
				if (flag2)
				{
					result = true;
				}
				else
				{
					bool flag3 = MerchantDomain.<RemoveBuyBackItem>g__RemoveButBackItemInInventory|62_0(this._merchantMaxLevelBuyBackData, ref CS$<>8__locals1);
					if (flag3)
					{
						result = true;
					}
					else
					{
						bool flag4 = MerchantDomain.<RemoveBuyBackItem>g__RemoveButBackItemInInventory|62_0(this._branchMerchantBuyBackData, ref CS$<>8__locals1);
						result = flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x0600490E RID: 18702 RVA: 0x002930E0 File Offset: 0x002912E0
		public ItemKey TryGetBuyBackItemForPersonalNeed(DataContext context, GameData.Domains.Character.Ai.PersonalNeed personalNeed)
		{
			MerchantDomain.<>c__DisplayClass63_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			CS$<>8__locals1.personalNeed = personalNeed;
			ItemKey itemKey = MerchantDomain.<TryGetBuyBackItemForPersonalNeed>g__GetBuyBackItemInInventory|63_0(this._merchantBuyBackData.Values, ref CS$<>8__locals1);
			bool flag = itemKey.IsValid();
			ItemKey result;
			if (flag)
			{
				result = itemKey;
			}
			else
			{
				itemKey = MerchantDomain.<TryGetBuyBackItemForPersonalNeed>g__GetBuyBackItemInInventory|63_0(this._caravanBuyBackData.Values, ref CS$<>8__locals1);
				bool flag2 = itemKey.IsValid();
				if (flag2)
				{
					result = itemKey;
				}
				else
				{
					itemKey = MerchantDomain.<TryGetBuyBackItemForPersonalNeed>g__GetBuyBackItemInInventory|63_0(this._merchantMaxLevelBuyBackData, ref CS$<>8__locals1);
					bool flag3 = itemKey.IsValid();
					if (flag3)
					{
						result = itemKey;
					}
					else
					{
						itemKey = MerchantDomain.<TryGetBuyBackItemForPersonalNeed>g__GetBuyBackItemInInventory|63_0(this._branchMerchantBuyBackData, ref CS$<>8__locals1);
						bool flag4 = itemKey.IsValid();
						if (flag4)
						{
							result = itemKey;
						}
						else
						{
							result = ItemKey.Invalid;
						}
					}
				}
			}
			return result;
		}

		// Token: 0x0600490F RID: 18703 RVA: 0x0029318C File Offset: 0x0029138C
		private void RemoveAllGoodsInMerchantBuyBackData(DataContext context)
		{
			MerchantDomain.<>c__DisplayClass64_0 CS$<>8__locals1;
			CS$<>8__locals1.context = context;
			MerchantDomain.<RemoveAllGoodsInMerchantBuyBackData>g__RemoveAllGoods|64_0(this._merchantBuyBackData.Values, ref CS$<>8__locals1);
			MerchantDomain.<RemoveAllGoodsInMerchantBuyBackData>g__RemoveAllGoods|64_0(this._caravanBuyBackData.Values, ref CS$<>8__locals1);
			MerchantDomain.<RemoveAllGoodsInMerchantBuyBackData>g__RemoveAllGoods|64_0(this._merchantMaxLevelBuyBackData, ref CS$<>8__locals1);
			MerchantDomain.<RemoveAllGoodsInMerchantBuyBackData>g__RemoveAllGoods|64_0(this._branchMerchantBuyBackData, ref CS$<>8__locals1);
			MerchantData tempMerchantData = this._tempMerchantData;
			if (tempMerchantData != null)
			{
				tempMerchantData.RemoveAllGoods(CS$<>8__locals1.context);
			}
			MerchantBuyBackData sectStorySpecialMerchantBuyBackData = this._sectStorySpecialMerchantBuyBackData;
			if (sectStorySpecialMerchantBuyBackData != null)
			{
				sectStorySpecialMerchantBuyBackData.RemoveAllGoods(CS$<>8__locals1.context);
			}
			this._merchantBuyBackData.Clear();
			this._caravanBuyBackData.Clear();
		}

		// Token: 0x06004910 RID: 18704 RVA: 0x00293230 File Offset: 0x00291430
		private void InitMerchantBuyBackData(DataContext context)
		{
			foreach (int key in this._merchantData.Keys)
			{
				MerchantData merchantData = this._merchantData[key];
				MerchantBuyBackData merchantBuyBackData;
				bool flag = this.TransferMerchantBuyBackData(merchantData, out merchantBuyBackData);
				if (flag)
				{
					this.SetMerchantData(key, merchantData, context);
					this._merchantBuyBackData[key] = merchantBuyBackData;
				}
			}
			foreach (int key2 in this._caravanData.Keys)
			{
				MerchantData merchantData2 = this._caravanData[key2];
				MerchantBuyBackData merchantBuyBackData2;
				bool flag2 = this.TransferMerchantBuyBackData(merchantData2, out merchantBuyBackData2);
				if (flag2)
				{
					this.SetCaravanData(key2, merchantData2, context);
					this._caravanBuyBackData[key2] = merchantBuyBackData2;
				}
			}
			sbyte index = 0;
			while ((int)index < this._merchantMaxLevelData.Length)
			{
				MerchantData merchantData3 = this._merchantMaxLevelData[(int)index];
				bool flag3 = merchantData3 == null;
				if (!flag3)
				{
					MerchantBuyBackData merchantBuyBackData3;
					bool flag4 = this.TransferMerchantBuyBackData(merchantData3, out merchantBuyBackData3);
					if (flag4)
					{
						this.SetElement_MerchantMaxLevelData((int)index, merchantData3, context);
						this._merchantMaxLevelBuyBackData[(int)index] = merchantBuyBackData3;
					}
				}
				index += 1;
			}
			sbyte index2 = 0;
			while ((int)index2 < DomainManager.Extra.BranchMerchantData.Length)
			{
				MerchantData merchantData4 = DomainManager.Extra.BranchMerchantData[(int)index2];
				bool flag5 = merchantData4 == null;
				if (!flag5)
				{
					MerchantBuyBackData merchantBuyBackData4;
					bool flag6 = this.TransferMerchantBuyBackData(merchantData4, out merchantBuyBackData4);
					if (flag6)
					{
						DomainManager.Extra.SetBranchMerchantData(context, index2, merchantData4);
						this._branchMerchantBuyBackData[(int)index2] = merchantBuyBackData4;
					}
				}
				index2 += 1;
			}
			SectStorySpecialMerchant sectStorySpecialMerchant = DomainManager.Extra.GetSectStorySpecialMerchant();
			bool flag7 = ((sectStorySpecialMerchant != null) ? sectStorySpecialMerchant.MerchantData : null) != null;
			if (flag7)
			{
				MerchantBuyBackData merchantBuyBackData5;
				bool flag8 = this.TransferMerchantBuyBackData((sectStorySpecialMerchant != null) ? sectStorySpecialMerchant.MerchantData : null, out merchantBuyBackData5);
				if (flag8)
				{
					DomainManager.Extra.SetSectStorySpecialMerchant(sectStorySpecialMerchant, context);
					this._sectStorySpecialMerchantBuyBackData = merchantBuyBackData5;
				}
			}
		}

		// Token: 0x06004911 RID: 18705 RVA: 0x00293464 File Offset: 0x00291664
		private bool TransferMerchantBuyBackData(MerchantData merchantData, out MerchantBuyBackData merchantBuyBackData)
		{
			merchantBuyBackData = null;
			bool flag = merchantData.BuyInGoodsList == null || merchantData.BuyInGoodsList.Items.Count == 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				merchantBuyBackData = new MerchantBuyBackData();
				merchantBuyBackData.MerchantType = merchantData.MerchantType;
				foreach (KeyValuePair<ItemKey, int> keyValuePair in merchantData.BuyInGoodsList.Items)
				{
					ItemKey itemKey2;
					int num;
					keyValuePair.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					int amount = num;
					merchantBuyBackData.BuyInGoodsList.OfflineAdd(itemKey, amount);
					int price;
					bool flag2 = merchantData.BuyInPrice.TryGetValue(itemKey, out price);
					if (flag2)
					{
						merchantBuyBackData.BuyInPrice[itemKey] = price;
					}
				}
				Inventory buyInGoodsList = merchantData.BuyInGoodsList;
				if (buyInGoodsList != null)
				{
					buyInGoodsList.Items.Clear();
				}
				Dictionary<ItemKey, int> buyInPrice = merchantData.BuyInPrice;
				if (buyInPrice != null)
				{
					buyInPrice.Clear();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06004912 RID: 18706 RVA: 0x00293574 File Offset: 0x00291774
		[DomainMethod]
		public void PullTradeCaravanLocations(DataContext context)
		{
			this.RefreshCaravanInTaiwuState(context);
		}

		// Token: 0x06004913 RID: 18707 RVA: 0x00293580 File Offset: 0x00291780
		[DomainMethod]
		public MerchantData GetCaravanMerchantData(DataContext context, int caravanId)
		{
			bool flag = caravanId == -1 && this._tempMerchantData != null;
			MerchantData result;
			if (flag)
			{
				result = this._tempMerchantData;
			}
			else
			{
				MerchantData merchantData;
				bool flag2 = !this.TryGetElement_CaravanData(caravanId, out merchantData);
				if (flag2)
				{
					result = null;
				}
				else
				{
					bool flag3 = caravanId >= 0;
					if (flag3)
					{
						MerchantExtraGoodsData merchantExtraGoods;
						DomainManager.Extra.TryGetMerchantExtraGoods(caravanId, out merchantExtraGoods);
						sbyte oldSeason = (merchantExtraGoods != null) ? merchantExtraGoods.SeasonTemplateId : -1;
						bool flag4 = oldSeason != EventHelper.GetCurrSeason();
						if (flag4)
						{
							merchantData.RefreshSeasonExtraGoods(context, caravanId, null);
							this.SetCaravanData(caravanId, merchantData, context);
						}
					}
					result = merchantData;
				}
			}
			return result;
		}

		// Token: 0x06004914 RID: 18708 RVA: 0x0029361C File Offset: 0x0029181C
		[DomainMethod]
		public MerchantData GetBuildingMerchantData(DataContext context, sbyte merchantType, bool isHead)
		{
			MerchantData[] dataArray = isHead ? this._merchantMaxLevelData : DomainManager.Extra.BranchMerchantData;
			MerchantBuyBackData[] buyBackDataArray = isHead ? this._merchantMaxLevelBuyBackData : this._branchMerchantBuyBackData;
			MerchantTypeItem merchantTypeItem = MerchantType.Instance[merchantType];
			sbyte targetLevel = isHead ? merchantTypeItem.HeadLevel : merchantTypeItem.BranchLevel;
			MerchantData merchantData = dataArray[(int)merchantType];
			bool flag = merchantData != null && merchantData.MerchantLevel != targetLevel;
			if (flag)
			{
				MerchantBuyBackData merchantBuyBackData = buyBackDataArray[(int)merchantType];
				if (merchantBuyBackData != null)
				{
					merchantBuyBackData.RemoveAllGoods(context);
				}
				merchantData.RemoveAllGoods(context);
				merchantData = null;
			}
			int caravanId = GameData.Domains.Merchant.SharedMethods.GetBuildingMerchantCaravanId(merchantType, isHead);
			bool flag2 = merchantData == null;
			if (flag2)
			{
				MerchantItem merchantItem = Merchant.Instance.FirstOrDefault((MerchantItem m) => m.Level == targetLevel && m.MerchantType == merchantType);
				Tester.Assert(merchantItem != null, "");
				dataArray[(int)merchantType] = new MerchantData(-1, merchantItem.TemplateId);
				dataArray[(int)merchantType].GenerateGoods(context, caravanId, null);
				merchantData = dataArray[(int)merchantType];
				if (isHead)
				{
					this.SetElement_MerchantMaxLevelData((int)merchantType, merchantData, context);
				}
				else
				{
					DomainManager.Extra.SetBranchMerchantData(context, merchantType, merchantData);
				}
			}
			else
			{
				MerchantExtraGoodsData merchantExtraGoods;
				DomainManager.Extra.TryGetMerchantExtraGoods(caravanId, out merchantExtraGoods);
				sbyte oldSeason = (merchantExtraGoods != null) ? merchantExtraGoods.SeasonTemplateId : -1;
				bool flag3 = oldSeason != EventHelper.GetCurrSeason();
				if (flag3)
				{
					merchantData.RefreshSeasonExtraGoods(context, caravanId, null);
					if (isHead)
					{
						this.SetElement_MerchantMaxLevelData((int)merchantType, merchantData, context);
					}
					else
					{
						DomainManager.Extra.SetBranchMerchantData(context, merchantType, merchantData);
					}
				}
			}
			merchantData.Money = this.GetMerchantMoney(context, merchantType);
			return merchantData;
		}

		// Token: 0x06004915 RID: 18709 RVA: 0x00293804 File Offset: 0x00291A04
		private void InitMerchantMoney(DataContext context)
		{
			for (int i = 0; i < Merchant.Instance.Count; i++)
			{
				MerchantItem configData = Merchant.Instance[i];
				bool flag = configData.Level == 6;
				if (flag)
				{
					int money = configData.Money * context.Random.Next(80, 120) / 100;
					this.SetMerchantMoney(context, configData.MerchantType, money);
				}
			}
		}

		// Token: 0x06004916 RID: 18710 RVA: 0x00293874 File Offset: 0x00291A74
		public void GenTradeCaravansOnAdvanceMonth(DataContext context)
		{
			MerchantDomain.<>c__DisplayClass77_0 CS$<>8__locals1;
			CS$<>8__locals1.<>4__this = this;
			CS$<>8__locals1.context = context;
			bool flag = DomainManager.World.GetMainStoryLineProgress() < 16;
			if (!flag)
			{
				foreach (MerchantItem config in ((IEnumerable<MerchantItem>)Merchant.Instance))
				{
					bool flag2 = config.Money <= 0 || config.Level == 6 || DomainManager.World.GetCurrDate() % (int)config.GenerateInterval != 0;
					if (!flag2)
					{
						MerchantDomain.<>c__DisplayClass77_1 CS$<>8__locals2;
						CS$<>8__locals2.needMoney = config.Money * CS$<>8__locals1.context.Random.Next(50, 150) / 100;
						int curMoney = this.GetMerchantMoney(CS$<>8__locals1.context, config.MerchantType);
						bool flag3 = CS$<>8__locals2.needMoney > curMoney;
						if (flag3)
						{
							bool flag4 = config.Level == 0;
							if (flag4)
							{
								curMoney += config.Money * CS$<>8__locals1.context.Random.Next(80, 120) / 100;
								this.SetMerchantMoney(CS$<>8__locals1.context, config.MerchantType, curMoney);
							}
						}
						else
						{
							short destAreaId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)CS$<>8__locals1.context.Random.Next(1, 15));
							Location destLocation = new Location(destAreaId, DomainManager.Map.GetElement_Areas((int)destAreaId).SettlementInfos[0].BlockId);
							this.<GenTradeCaravansOnAdvanceMonth>g__CreateCaravan|77_0(config, destLocation, ref CS$<>8__locals1, ref CS$<>8__locals2);
							curMoney -= CS$<>8__locals2.needMoney;
							this.SetMerchantMoney(CS$<>8__locals1.context, config.MerchantType, curMoney);
							bool needCreateExtra = true;
							List<int> keys = DomainManager.Extra.GetCaravanStayDaysKeys();
							foreach (int id in keys)
							{
								MerchantData merchant = this._caravanData[id];
								bool flag5 = merchant.MerchantType == config.MerchantType;
								if (flag5)
								{
									needCreateExtra = false;
									break;
								}
							}
							bool flag6 = config.Level >= 3 && needCreateExtra;
							if (flag6)
							{
								int random = CS$<>8__locals1.context.Random.Next(100);
								int targetLevel = 0;
								bool flag7 = random <= 10;
								if (flag7)
								{
									targetLevel = 2;
								}
								else
								{
									bool flag8 = random <= 20;
									if (flag8)
									{
										targetLevel = 1;
									}
								}
								MerchantItem targetConfig = null;
								foreach (MerchantItem tempConfig in ((IEnumerable<MerchantItem>)Merchant.Instance))
								{
									bool flag9 = (int)tempConfig.Level == targetLevel && tempConfig.MerchantType == config.MerchantType;
									if (flag9)
									{
										targetConfig = tempConfig;
										break;
									}
								}
								Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
								int caravanId = this.<GenTradeCaravansOnAdvanceMonth>g__CreateCaravan|77_0(targetConfig, taiwuVillageLocation, ref CS$<>8__locals1, ref CS$<>8__locals2);
								DomainManager.Extra.AddCaravanStayDays(caravanId, 90, CS$<>8__locals1.context);
							}
						}
					}
				}
			}
		}

		// Token: 0x06004917 RID: 18711 RVA: 0x00293BC0 File Offset: 0x00291DC0
		private CaravanPath CreateCaravanPath([TupleElementNames(new string[]
		{
			"location",
			"cost"
		})] List<ValueTuple<Location, short>> path)
		{
			CaravanPath caravanPath = new CaravanPath();
			int leftDaysCounter = 15;
			Location lastLocation = path[0].Item1;
			caravanPath.FullPath.Add(path[0].Item1);
			caravanPath.MoveNodes.Add(0);
			for (int index = 1; index < path.Count; index++)
			{
				ValueTuple<Location, short> pathNode = path[index];
				short areaId = pathNode.Item1.AreaId;
				caravanPath.FullPath.Add(pathNode.Item1);
				MapBlockData mapBlockData = DomainManager.Map.GetBlock(pathNode.Item1);
				MapBlockData lastMapBlockData = DomainManager.Map.GetBlock(lastLocation);
				bool flag = mapBlockData.IsCityTown() && !lastMapBlockData.IsCityTown();
				if (flag)
				{
					leftDaysCounter = 15;
					caravanPath.MoveNodes.Add(index);
				}
				else
				{
					leftDaysCounter -= (int)pathNode.Item2;
					while (leftDaysCounter <= 0)
					{
						leftDaysCounter += 15;
						caravanPath.MoveNodes.Add((leftDaysCounter > 0) ? index : ((areaId == lastLocation.AreaId) ? (index - 1) : -1));
					}
				}
				lastLocation = pathNode.Item1;
			}
			bool flag2 = caravanPath.MoveNodes[caravanPath.MoveNodes.Count - 1] != path.Count - 1;
			if (flag2)
			{
				caravanPath.MoveNodes.Add(path.Count - 1);
			}
			caravanPath.MoveWaitDays = 30;
			return caravanPath;
		}

		// Token: 0x06004918 RID: 18712 RVA: 0x00293D40 File Offset: 0x00291F40
		private void ReCalcCaravanPath(DataContext context)
		{
			foreach (int caravanId in this._caravanDict.Keys)
			{
				CaravanPath caravanPath = this._caravanDict[caravanId];
				bool flag = caravanPath.MoveNodes.Count <= 1;
				if (!flag)
				{
					bool flag2 = caravanPath.MoveWaitDays > 30;
					if (!flag2)
					{
						Location curLocation = caravanPath.GetCurrLocation();
						Location destLocation = caravanPath.GetDestLocation();
						int curIndex = caravanPath.FullPath.IndexOf(curLocation);
						List<Location> lastPath = (curIndex - 1 > 0) ? caravanPath.FullPath.Take(curIndex - 1).ToList<Location>() : null;
						List<ValueTuple<Location, short>> path = DomainManager.Map.CalcBlockTravelRoute(context.Random, curLocation, destLocation, true);
						path.Insert(0, new ValueTuple<Location, short>(curLocation, 0));
						CaravanPath newCaravanPath = this.CreateCaravanPath(path);
						bool flag3 = lastPath != null;
						if (flag3)
						{
							newCaravanPath.FullPath.InsertRange(0, lastPath);
							for (int i = 0; i < newCaravanPath.MoveNodes.Count; i++)
							{
								bool flag4 = newCaravanPath.MoveNodes[i] >= 0;
								if (flag4)
								{
									List<int> moveNodes = newCaravanPath.MoveNodes;
									int index = i;
									moveNodes[index] += lastPath.Count;
								}
							}
						}
						this.SetElement_CaravanDict(caravanId, newCaravanPath, context);
						CaravanExtraData caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
						List<short> settlementIdList = caravanExtraData.SettlementIdList;
						bool flag5 = settlementIdList != null && settlementIdList.Count > 0;
						if (flag5)
						{
							this.RefreshCaravanExtraDataSettlementIdList(caravanId, ref caravanExtraData.SettlementIdList);
							DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
						}
					}
				}
			}
		}

		// Token: 0x06004919 RID: 18713 RVA: 0x00293F28 File Offset: 0x00292128
		private CaravanExtraData CreateCaravanExtraData(DataContext context, int caravanId)
		{
			CaravanExtraData extraData;
			bool flag = DomainManager.Extra.TryGetCaravanExtraData(caravanId, out extraData);
			CaravanExtraData result;
			if (flag)
			{
				result = extraData;
			}
			else
			{
				extraData = new CaravanExtraData();
				short incomeCriticalResultMin = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.First<short>();
				short incomeCriticalResultMax = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.Last<short>();
				extraData.IncomeCriticalResult = (short)context.Random.Next((int)incomeCriticalResultMin, (int)incomeCriticalResultMax);
				this.RefreshCaravanExtraDataSettlementIdList(caravanId, ref extraData.SettlementIdList);
				DomainManager.Extra.SetCaravanExtraData(context, caravanId, extraData);
				result = extraData;
			}
			return result;
		}

		// Token: 0x0600491A RID: 18714 RVA: 0x00293FAC File Offset: 0x002921AC
		private void RefreshCaravanExtraDataSettlementIdList(int caravanId, ref List<short> settlementIdList)
		{
			CaravanPath caravanPath = this._caravanDict[caravanId];
			if (settlementIdList == null)
			{
				settlementIdList = new List<short>();
			}
			settlementIdList.Clear();
			for (int i = 1; i < caravanPath.MoveNodes.Count; i++)
			{
				int index = caravanPath.MoveNodes[i];
				bool flag = !caravanPath.FullPath.CheckIndex(index);
				if (!flag)
				{
					Location location = caravanPath.FullPath[index];
					MapBlockData blockData = DomainManager.Map.GetBelongSettlementBlock(location);
					bool flag2 = blockData == null;
					if (!flag2)
					{
						Settlement settlement = DomainManager.Organization.GetSettlementByLocation(blockData.GetLocation());
						short id = (settlement != null) ? settlement.GetId() : -1;
						bool flag3 = id >= 0 && !settlementIdList.Contains(id);
						if (flag3)
						{
							settlementIdList.Add(id);
						}
					}
				}
			}
		}

		// Token: 0x0600491B RID: 18715 RVA: 0x00294090 File Offset: 0x00292290
		public void CaravanMonthEvent(DataContext context)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			bool flag = !DomainManager.Map.GetElement_Areas((int)taiwuVillageLocation.AreaId).StationUnlocked || !DomainManager.World.GetWorldFunctionsStatus(4);
			if (!flag)
			{
				EventArgBox globalEventArgBox = DomainManager.TaiwuEvent.GetGlobalEventArgumentBox();
				bool caravanVisitMonthEventTriggered = false;
				globalEventArgBox.Get("CS_PK_CaravanVisit", ref caravanVisitMonthEventTriggered);
				bool flag2 = caravanVisitMonthEventTriggered;
				if (!flag2)
				{
					int taiwuVillageStationOpenDate = int.MaxValue;
					bool flag3 = !globalEventArgBox.Get("CS_PK_StationOpenDate", ref taiwuVillageStationOpenDate);
					if (!flag3)
					{
						int currDate = DomainManager.World.GetCurrDate();
						bool flag4 = currDate < taiwuVillageStationOpenDate + 6;
						if (!flag4)
						{
							int caravanId = this.GetMonthEventCaravanId(context);
							bool flag5 = caravanId < 0;
							if (!flag5)
							{
								DomainManager.World.GetMonthlyEventCollection().AddMerchantVisit();
							}
						}
					}
				}
			}
		}

		// Token: 0x0600491C RID: 18716 RVA: 0x00294160 File Offset: 0x00292360
		public int GetMonthEventCaravanId(DataContext context)
		{
			Location taiwuVillageLocation = DomainManager.Taiwu.GetTaiwuVillageLocation();
			sbyte stateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(taiwuVillageLocation.AreaId);
			sbyte mainAreaTemplateId = MapState.Instance[stateTemplateId].MainAreaID;
			int goodCount = 2;
			int goodIndex = 1;
			sbyte merchantType = -1;
			bool flag = mainAreaTemplateId == 11;
			if (flag)
			{
				merchantType = ((context.Random.Next(0, 2) == 0) ? 0 : 1);
			}
			else
			{
				short mapBlockTemplateId = MapArea.Instance[(short)mainAreaTemplateId].SettlementBlockCore[0];
				List<short> presetBuildingList = MapBlock.Instance[mapBlockTemplateId].PresetBuildingList;
				for (int i = 0; i < presetBuildingList.Count; i++)
				{
					bool flag2 = presetBuildingList[i] >= 274 && presetBuildingList[i] <= 280;
					if (flag2)
					{
						BuildingBlockItem buildingBlockItem = BuildingBlock.Instance[presetBuildingList[i]];
						merchantType = buildingBlockItem.MerchantId;
						break;
					}
				}
			}
			int caravanId = DomainManager.Merchant.TryGetCaravanIdByTypeAndLevel(merchantType, 3, goodIndex, goodCount);
			bool flag3 = caravanId < 0;
			if (flag3)
			{
				for (int index = 1; index < 6; index++)
				{
					for (sbyte level = 0; level <= 6; level += 1)
					{
						caravanId = DomainManager.Merchant.TryGetCaravanIdByTypeAndLevel(merchantType, level, index, goodCount);
						bool flag4 = caravanId < 0;
						if (!flag4)
						{
							MerchantData caravanMerchantData = DomainManager.Merchant.GetCaravanMerchantData(context, caravanId);
							bool flag5 = caravanMerchantData == null;
							if (!flag5)
							{
								break;
							}
						}
					}
					bool flag6 = caravanId >= 0;
					if (flag6)
					{
						break;
					}
				}
			}
			return caravanId;
		}

		// Token: 0x0600491D RID: 18717 RVA: 0x0029430C File Offset: 0x0029250C
		public void UpdateCaravansMove(DataContext context)
		{
			bool anyCaravanAddedOrMoved = false;
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			int taiwuState = (int)(taiwuLocation.IsValid() ? DomainManager.Map.GetStateIdByAreaId(taiwuLocation.AreaId) : -1);
			foreach (int caravanId in this._caravanDict.Keys)
			{
				CaravanExtraData extraData;
				bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out extraData);
				if (!flag)
				{
					MerchantData merchantData = this._caravanData[caravanId];
					CaravanPath path = this._caravanDict[caravanId];
					CaravanState state = (CaravanState)extraData.State;
					CaravanState caravanState = state;
					if (caravanState != CaravanState.Robbed)
					{
						if (caravanState != CaravanState.RobEnd)
						{
							Location destLocation = path.GetDestLocation();
							MapBlockData destMapBlockData = DomainManager.Map.GetBlock(destLocation).GetRootBlock();
							Settlement targetSettlement = DomainManager.Organization.GetSettlementByLocation(destMapBlockData.GetLocation());
							bool inTaiwuState = (int)DomainManager.Map.GetStateIdByAreaId(path.GetCurrLocation().AreaId) == taiwuState;
							bool flag2 = inTaiwuState;
							if (flag2)
							{
								anyCaravanAddedOrMoved = true;
							}
							bool flag3 = path.MoveNodes.Count > 0;
							if (flag3)
							{
								int pathIndex = path.MoveNodes.First<int>();
								for (int i = 1; i < path.MoveNodes.Count; i++)
								{
									int curIndex = path.MoveNodes[i];
									bool flag4 = curIndex < 0;
									if (!flag4)
									{
										bool flag5 = pathIndex > curIndex;
										if (flag5)
										{
											Logger logger = MerchantDomain.Logger;
											DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(23, 1);
											defaultInterpolatedStringHandler.AppendLiteral("caravan ");
											defaultInterpolatedStringHandler.AppendFormatted<int>(caravanId);
											defaultInterpolatedStringHandler.AppendLiteral(" path has error");
											logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
											break;
										}
										pathIndex = curIndex;
									}
								}
								CaravanPath caravanPath = path;
								caravanPath.MoveWaitDays -= 30;
								short stayDays;
								bool flag6 = path.MoveNodes.Count == 1 && DomainManager.Extra.TryGetCaravanStayDays(caravanId, out stayDays);
								if (flag6)
								{
									path.MoveWaitDays = Convert.ToInt16((int)(stayDays - 30));
									DomainManager.Extra.SetCaravanStayDays(caravanId, path.MoveWaitDays, context);
								}
								bool flag7 = path.MoveWaitDays > 0;
								if (flag7)
								{
									continue;
								}
								CaravanPath caravanPath2 = path;
								caravanPath2.MoveWaitDays += 30;
								path.MoveNodes.RemoveAt(0);
								bool flag8 = path.MoveNodes.Count > 0;
								if (flag8)
								{
									int arriveIndex = path.MoveNodes.First<int>();
									bool flag9 = !path.FullPath.CheckIndex(arriveIndex);
									if (flag9)
									{
										continue;
									}
									Location arriveLocation = path.FullPath[arriveIndex];
									MapBlockData mapBlockData = DomainManager.Map.GetBlock(arriveLocation).GetRootBlock();
									Location location = mapBlockData.GetLocation();
									bool flag10 = mapBlockData.IsCityTown();
									if (flag10)
									{
										Settlement settlement = DomainManager.Organization.GetSettlementByLocation(location);
										short settlementId = (settlement != null) ? settlement.GetId() : -1;
										bool flag11 = settlementId >= 0 && extraData.SettlementIdList != null && extraData.SettlementIdList.Contains(settlementId);
										if (flag11)
										{
											extraData.SettlementIdList.Remove(settlementId);
											short culture = settlement.GetCulture();
											bool isHighCulture = culture > 50;
											bool isLowCulture = culture < 50;
											bool hasChangedIncomeBonus = false;
											bool hasChangedIncomeCriticalRate = false;
											bool flag12 = isHighCulture;
											if (flag12)
											{
												short newIncomeBonus = (short)Math.Clamp((int)(extraData.IncomeBonus + Convert.ToInt16((int)(culture * 5 - 250))), 0, 32767);
												bool flag13 = extraData.IncomeBonus != newIncomeBonus;
												if (flag13)
												{
													hasChangedIncomeBonus = true;
													extraData.IncomeBonus = newIncomeBonus;
												}
											}
											else
											{
												bool flag14 = isLowCulture;
												if (flag14)
												{
													short newIncomeCriticalRate = (short)Math.Clamp((int)(extraData.IncomeCriticalRate + Convert.ToInt16((int)(200 - 4 * culture))), 0, 1000);
													bool flag15 = extraData.IncomeCriticalRate != newIncomeCriticalRate;
													if (flag15)
													{
														hasChangedIncomeCriticalRate = true;
														extraData.IncomeCriticalRate = newIncomeCriticalRate;
													}
												}
											}
											short safety = settlement.GetSafety();
											bool isHighSafety = safety > 50;
											bool isLowSafety = safety < 50;
											bool hasChangedRobbedRate = false;
											short newRobbedRate = (short)Math.Clamp((int)(extraData.RobbedRate + Convert.ToInt16((int)(200 - 4 * safety))), 0, 1000);
											bool flag16 = extraData.RobbedRate != newRobbedRate;
											if (flag16)
											{
												hasChangedRobbedRate = true;
												extraData.RobbedRate = newRobbedRate;
											}
											DomainManager.Extra.SetCaravanExtraData(context, caravanId, extraData);
											this.AddInvestedCaravanPassSettlementMonthlyNotification(isLowSafety, isHighSafety, hasChangedRobbedRate, hasChangedIncomeBonus, hasChangedIncomeCriticalRate, settlementId, targetSettlement.GetId(), extraData, merchantData, path);
										}
									}
									else
									{
										int random = context.Random.Next(1000);
										int rate = GameData.Domains.Merchant.SharedMethods.GetCaravanRobbedRate((int)extraData.RobbedRate, MapAreaData.IsBrokenArea(location.AreaId));
										bool flag17 = random < rate;
										if (flag17)
										{
											extraData.State = 1;
											DomainManager.Extra.SetCaravanExtraData(context, caravanId, extraData);
											this.AddInvestedCaravanRobbedMonthlyNotification(location, extraData, merchantData, false);
										}
									}
								}
							}
							bool flag18 = path.MoveNodes.Count == 0;
							if (flag18)
							{
								this.OnCaravanArrive(context, caravanId, extraData, merchantData, targetSettlement.GetId());
							}
							else
							{
								this.SetElement_CaravanDict(caravanId, path, context);
							}
						}
						else
						{
							extraData.State = 0;
							DomainManager.Extra.SetCaravanExtraData(context, caravanId, extraData);
						}
					}
					else
					{
						this.ApplyMerchantRobbedResult(context, caravanId, false, false);
						this.AddInvestedCaravanRobbedMonthlyNotification(path.GetCurrLocation(), extraData, merchantData, true);
					}
				}
			}
			bool flag19 = !DomainManager.Map.IsTraveling;
			if (flag19)
			{
				bool flag20 = anyCaravanAddedOrMoved;
				if (flag20)
				{
					this.RefreshCaravanInTaiwuState(context);
				}
			}
		}

		// Token: 0x0600491E RID: 18718 RVA: 0x002948D0 File Offset: 0x00292AD0
		private void AddInvestedCaravanPassSettlementMonthlyNotification(bool isLowSafety, bool isHighSafety, bool hasChangedRobbedRate, bool hasChangedIncomeBonus, bool hasChangedIncomeCriticalRate, short curSettlementId, short targetSettlementId, CaravanExtraData extraData, MerchantData merchantData, CaravanPath caravanPath)
		{
			bool flag = !extraData.IsInvested;
			if (!flag)
			{
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				int robbedRate = (int)(extraData.RobbedRate / 10);
				int incomeBonus = (int)(extraData.IncomeBonus / 10);
				int incomeCriticalRate = (int)(extraData.IncomeCriticalRate / 10);
				int costTime = caravanPath.MoveNodes.Count - 1;
				if (hasChangedRobbedRate)
				{
					if (isHighSafety)
					{
						if (hasChangedIncomeBonus)
						{
							monthlyNotificationCollection.AddInvestedCaravanPassHighSafetyHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate, incomeBonus);
						}
						else if (hasChangedIncomeCriticalRate)
						{
							monthlyNotificationCollection.AddInvestedCaravanPassHighSafetyLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate, incomeCriticalRate);
						}
						else
						{
							monthlyNotificationCollection.AddInvestedCaravanPassHighSafetySettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate);
						}
					}
					else if (isLowSafety)
					{
						if (hasChangedIncomeBonus)
						{
							monthlyNotificationCollection.AddInvestedCaravanPassLowSafetyHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate, incomeBonus);
						}
						else if (hasChangedIncomeCriticalRate)
						{
							monthlyNotificationCollection.AddInvestedCaravanPassLowSafetyLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate, incomeCriticalRate);
						}
						else
						{
							monthlyNotificationCollection.AddInvestedCaravanPassLowSafetySettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, robbedRate);
						}
					}
				}
				else if (hasChangedIncomeBonus)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassHighCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, incomeBonus);
				}
				else if (hasChangedIncomeCriticalRate)
				{
					monthlyNotificationCollection.AddInvestedCaravanPassLowCultureSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId, incomeCriticalRate);
				}
				else
				{
					monthlyNotificationCollection.AddInvestedCaravanPassSettlement(merchantData.MerchantTemplateId, curSettlementId, costTime, targetSettlementId);
				}
			}
		}

		// Token: 0x0600491F RID: 18719 RVA: 0x00294A64 File Offset: 0x00292C64
		private void AddInvestedCaravanRobbedMonthlyNotification(Location location, CaravanExtraData extraData, MerchantData merchantData, bool finished)
		{
			bool flag = !extraData.IsInvested;
			if (!flag)
			{
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				bool flag2 = !finished;
				if (flag2)
				{
					monthlyNotificationCollection.AddInvestedCaravanIsRobbed(merchantData.MerchantTemplateId, location);
				}
				else
				{
					monthlyNotificationCollection.AddInvestedCaravanIsRobbedAndFailed(merchantData.MerchantTemplateId, location, (int)(extraData.IncomeBonus / 10));
				}
			}
		}

		// Token: 0x06004920 RID: 18720 RVA: 0x00294ABC File Offset: 0x00292CBC
		public void ApplyMerchantRobbedResult(DataContext context, int caravanId, bool win, bool refresh = false)
		{
			CaravanExtraData extraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out extraData);
			if (!flag)
			{
				if (win)
				{
					MerchantData merchantData = this.GetCaravanMerchantData(context, caravanId);
					int delta = GlobalConfig.Instance.CaravanRobbedEventWinAddMerchantFavorability[(int)merchantData.MerchantLevel];
					this.ChangeMerchantCumulativeMoney(context, merchantData.MerchantType, delta);
				}
				else
				{
					short reduceIncomeBonus = GlobalConfig.Instance.CaravanRobbedEventLoseReduceIncomeBonus;
					extraData.IncomeBonus = Convert.ToInt16((int)(extraData.IncomeBonus * (100 - reduceIncomeBonus) / 100));
				}
				sbyte reduceRobbedRate = GlobalConfig.Instance.CaravanRobbedEventEndReduceRobbedRate;
				extraData.RobbedRate = Convert.ToInt16((int)(extraData.RobbedRate * (short)(100 - reduceRobbedRate) / 100));
				extraData.State = 2;
				DomainManager.Extra.SetCaravanExtraData(context, caravanId, extraData);
				if (refresh)
				{
					this.RefreshCaravanInTaiwuState(context);
				}
			}
		}

		// Token: 0x06004921 RID: 18721 RVA: 0x00294B8C File Offset: 0x00292D8C
		public void RefreshCaravanInTaiwuState(DataContext context)
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			if (!flag)
			{
				sbyte taiwuState = DomainManager.Map.GetStateIdByAreaId(taiwuLocation.AreaId);
				List<CaravanDisplayData> caravanDataList = new List<CaravanDisplayData>();
				foreach (int caravanId in this._caravanDict.Keys)
				{
					CaravanPath path = this._caravanDict[caravanId];
					Location caravanLocation = path.GetCurrLocation();
					bool flag2 = DomainManager.Map.GetStateIdByAreaId(caravanLocation.AreaId) != taiwuState;
					if (!flag2)
					{
						CaravanDisplayData displayData = this.GetCaravanDisplayData(context, caravanId);
						caravanDataList.Add(displayData);
					}
				}
				caravanDataList = caravanDataList.OrderByDescending(delegate(CaravanDisplayData d)
				{
					CaravanExtraData extraData = d.ExtraData;
					return ((extraData != null) ? new CaravanState?(extraData.StateEnum) : null) == CaravanState.Robbed;
				}).ToList<CaravanDisplayData>();
				GameDataBridge.AddDisplayEvent<List<CaravanDisplayData>>(DisplayEventType.RefreshCaravanData, caravanDataList);
			}
		}

		// Token: 0x06004922 RID: 18722 RVA: 0x00294CA0 File Offset: 0x00292EA0
		[DomainMethod]
		public CaravanDisplayData GetCaravanDisplayData(DataContext context, int caravanId)
		{
			CaravanPath path;
			bool flag = !this._caravanDict.TryGetValue(caravanId, out path);
			CaravanDisplayData result;
			if (flag)
			{
				result = null;
			}
			else
			{
				short merchantTemplateId = (short)this._caravanData[caravanId].MerchantTemplateId;
				MerchantItem merchantConfig = Merchant.Instance[(int)merchantTemplateId];
				CaravanExtraData caravanExtraData;
				bool flag2 = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
				if (flag2)
				{
					caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
				}
				CaravanDisplayData caravanDisplayData = new CaravanDisplayData
				{
					CaravanId = caravanId,
					MerchantTemplateId = merchantTemplateId,
					TargetArea = path.GetDestLocation().AreaId,
					Favorability = DomainManager.Merchant.GetCurFavorability(merchantConfig.MerchantType),
					PathInArea = path.GetRemainCaravanPathInCurrentArea(),
					ExtraData = caravanExtraData
				};
				List<short> settlementIdList = caravanExtraData.SettlementIdList;
				bool flag3 = settlementIdList != null && settlementIdList.Count > 0;
				if (flag3)
				{
					caravanDisplayData.SettlementDisplayDataList = new List<SettlementDisplayData>();
					foreach (short settlementId in caravanExtraData.SettlementIdList)
					{
						SettlementDisplayData settlementDisplayData = DomainManager.Organization.GetDisplayData(settlementId);
						caravanDisplayData.SettlementDisplayDataList.Add(settlementDisplayData);
					}
				}
				result = caravanDisplayData;
			}
			return result;
		}

		// Token: 0x06004923 RID: 18723 RVA: 0x00294DF0 File Offset: 0x00292FF0
		private void OnCaravanArrive(DataContext context, int caravanId, CaravanExtraData extraData, MerchantData merchantData, short targetSettlementId)
		{
			short incomeCriticalRate = (extraData != null) ? extraData.IncomeCriticalRate : 0;
			short incomeBonus = (extraData != null) ? extraData.IncomeBonus : 1000;
			short incomeCriticalResult = (extraData != null) ? extraData.IncomeCriticalResult : 100;
			int random = context.Random.Next(1000);
			bool critical = random < (int)incomeCriticalRate;
			int income = merchantData.Money * (int)incomeBonus / 1000;
			bool flag = critical;
			if (flag)
			{
				income = income * (int)incomeCriticalResult / 100;
			}
			int money = this.GetMerchantMoney(context, merchantData.MerchantType);
			money += income;
			this.SetMerchantMoney(context, merchantData.MerchantType, money);
			bool flag2 = extraData != null && extraData.IsInvested;
			if (flag2)
			{
				int investedMoney = GlobalConfig.Instance.InvestCaravanNeedMoney[(int)merchantData.MerchantLevel];
				int taiwuIncome = investedMoney * (int)incomeBonus / 1000;
				bool flag3 = critical;
				if (flag3)
				{
					taiwuIncome = taiwuIncome * (int)incomeCriticalResult / 100;
				}
				DomainManager.Taiwu.GetTaiwu().ChangeResource(context, 6, taiwuIncome);
				MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
				monthlyNotificationCollection.AddInvestedCaravanArrive(merchantData.MerchantTemplateId, targetSettlementId, taiwuIncome);
				int seniority = ProfessionFormulaImpl.Calculate(98, investedMoney, taiwuIncome);
				DomainManager.Extra.ChangeProfessionSeniority(context, 15, seniority, true, false);
			}
			merchantData.RemoveAllGoods(context);
			this.RemoveElement_CaravanData(caravanId, context);
			this.RemoveElement_CaravanDict(caravanId, context);
			bool flag4 = extraData != null;
			if (flag4)
			{
				DomainManager.Extra.RemoveCaravanExtraData(context, caravanId);
			}
			short num;
			bool flag5 = DomainManager.Extra.TryGetCaravanStayDays(caravanId, out num);
			if (flag5)
			{
				DomainManager.Extra.RemoveCaravanStayDays(caravanId, context);
			}
			MerchantExtraGoodsData merchantExtraGoodsData;
			bool flag6 = DomainManager.Extra.TryGetMerchantExtraGoods(caravanId, out merchantExtraGoodsData);
			if (flag6)
			{
				DomainManager.Extra.RemoveMerchantExtraGoods(context, caravanId);
			}
		}

		// Token: 0x06004924 RID: 18724 RVA: 0x00294F98 File Offset: 0x00293198
		[DomainMethod]
		public void InvestCaravan(DataContext context, int caravanId)
		{
			MerchantData merchantData;
			this._caravanData.TryGetValue(caravanId, out merchantData);
			Tester.Assert(merchantData != null, "");
			CaravanPath caravanPath = this._caravanDict[caravanId];
			Tester.Assert(caravanPath.FullPath.Count >= 1, "");
			CaravanExtraData caravanExtraData;
			DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			int needMoney = GlobalConfig.Instance.InvestCaravanNeedMoney[(int)merchantData.MerchantLevel];
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int curMoney = taiwu.GetResource(6);
			Tester.Assert(needMoney <= curMoney, "");
			taiwu.ChangeResource(context, 6, -needMoney);
			caravanExtraData.IsInvested = true;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		}

		// Token: 0x06004925 RID: 18725 RVA: 0x00295058 File Offset: 0x00293258
		[DomainMethod]
		public void ProtectCaravan(DataContext context, int caravanId)
		{
			MerchantData merchantData;
			this._caravanData.TryGetValue(caravanId, out merchantData);
			Tester.Assert(merchantData != null, "");
			CaravanExtraData caravanExtraData;
			DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			int authorityFactor = GlobalConfig.Instance.InvestedCaravanAvoidRobbedNeedAuthorityFactor[(int)merchantData.MerchantLevel];
			int costAuthority = authorityFactor * (int)caravanExtraData.RobbedRate / 2;
			GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
			int curAuthority = taiwu.GetResource(7);
			Tester.Assert(costAuthority <= curAuthority, "");
			int time = DomainManager.World.GetCurrDate();
			taiwu.ChangeResource(context, 7, -costAuthority);
			CaravanExtraData caravanExtraData2 = caravanExtraData;
			caravanExtraData2.RobbedRate /= 2;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
			DomainManager.Extra.SetProtectCaravanTime(time, context);
		}

		// Token: 0x06004926 RID: 18726 RVA: 0x0029511C File Offset: 0x0029331C
		private void CreateTempCaravan(DataContext context, sbyte merchantType, sbyte merchantLevel)
		{
			MerchantData merchantData;
			bool flag = !this.TryGetElement_CaravanData(-1, out merchantData);
			if (flag)
			{
				this.AddElement_CaravanData(-1, merchantData, context);
			}
			MerchantBuyBackData tempMerchantBuyBackData = this._tempMerchantBuyBackData;
			if (tempMerchantBuyBackData != null)
			{
				tempMerchantBuyBackData.RemoveAllGoods(context);
			}
			if (merchantData != null)
			{
				merchantData.RemoveAllGoods(context);
			}
			merchantData = new MerchantData(-1, MerchantData.FindMerchantTemplateId(merchantType, merchantLevel));
			this._caravanData[-1] = merchantData;
			merchantData.GenerateGoods(context, -1, null);
			merchantData.Money = this.GetMerchantMoney(context, merchantData.MerchantType);
			this.SetElement_CaravanData(-1, merchantData, context);
		}

		// Token: 0x06004927 RID: 18727 RVA: 0x002951A8 File Offset: 0x002933A8
		public void StartTempCaravanAction(sbyte merchantType, sbyte merchantLevel, bool refresh, bool ignoreWorldProgress, bool ignoreFavorability, bool isOpenedByProfessionSkill)
		{
			this._totalBuyMoney = 0;
			MerchantData merchantData;
			bool flag = this.TryGetElement_CaravanData(-1, out merchantData);
			if (flag)
			{
				MerchantItem config = Merchant.Instance[merchantData.MerchantTemplateId];
				bool flag2 = refresh || merchantData.MerchantType != merchantType || config.Level != merchantLevel;
				if (flag2)
				{
					this.CreateTempCaravan(DomainManager.TaiwuEvent.MainThreadDataContext, merchantType, merchantLevel);
				}
			}
			else
			{
				this.CreateTempCaravan(DomainManager.TaiwuEvent.MainThreadDataContext, merchantType, merchantLevel);
			}
			OpenShopEventArguments.EMerchantSourceType merchantSourceType = isOpenedByProfessionSkill ? OpenShopEventArguments.EMerchantSourceType.ProfessionSkillCaravan : OpenShopEventArguments.EMerchantSourceType.AdventureCaravan;
			OpenShopEventArguments openShopEventArguments = new OpenShopEventArguments
			{
				Id = -1,
				Refresh = refresh,
				IgnoreWorldProgress = ignoreWorldProgress,
				IgnoreFavorability = ignoreFavorability,
				BuildingMerchantType = -1,
				MerchantSourceType = (sbyte)merchantSourceType
			};
			GameDataBridge.AddDisplayEvent<OpenShopEventArguments>(DisplayEventType.OpenShop, openShopEventArguments);
		}

		// Token: 0x06004928 RID: 18728 RVA: 0x00295270 File Offset: 0x00293470
		public void StartSpecificCharIdAndMerchantTypeAction(int charId, sbyte merchantType, sbyte merchantLevel, bool refresh)
		{
			this._totalBuyMoney = 0;
			DataContext ctx = DomainManager.TaiwuEvent.MainThreadDataContext;
			bool isUnique = merchantType == 8;
			bool flag = isUnique;
			if (flag)
			{
				MerchantData merchantData = this._merchantData.Values.FirstOrDefault((MerchantData mct) => mct.MerchantType == merchantType);
				bool flag2 = merchantData != null && merchantData.CharId != charId;
				if (flag2)
				{
					this.RemoveElement_MerchantData(merchantData.CharId, ctx);
					merchantData.CharId = charId;
					bool flag3 = this._merchantData.ContainsKey(charId);
					if (flag3)
					{
						this.SetElement_MerchantData(charId, merchantData, ctx);
					}
					else
					{
						this.AddElement_MerchantData(charId, merchantData, ctx);
					}
				}
				else
				{
					bool flag4 = merchantData == null;
					if (flag4)
					{
						bool existing = this.TryGetElement_MerchantData(charId, out merchantData);
						sbyte templateId = MerchantData.FindMerchantTemplateId(merchantType, merchantLevel);
						bool flag5 = !existing;
						if (flag5)
						{
							merchantData = new MerchantData(charId, templateId);
						}
						else
						{
							merchantData.MerchantTemplateId = templateId;
						}
						merchantData.GenerateGoods(ctx, -1, null);
						merchantData.Money = 0;
						bool flag6 = existing;
						if (flag6)
						{
							this.SetElement_MerchantData(charId, merchantData, ctx);
						}
						else
						{
							this.AddElement_MerchantData(charId, merchantData, ctx);
						}
					}
				}
			}
			else
			{
				MerchantData merchantData;
				bool existing2 = this.TryGetElement_MerchantData(charId, out merchantData);
				sbyte templateId2 = MerchantData.FindMerchantTemplateId(merchantType, merchantLevel);
				bool flag7 = !existing2;
				if (flag7)
				{
					merchantData = new MerchantData(charId, templateId2);
					merchantData.GenerateGoods(ctx, -1, null);
					merchantData.Money = 0;
				}
				else
				{
					bool flag8 = merchantData.MerchantTemplateId != templateId2;
					if (flag8)
					{
						merchantData.MerchantTemplateId = templateId2;
						merchantData.GenerateGoods(ctx, -1, null);
						merchantData.Money = 0;
					}
				}
				bool flag9 = existing2;
				if (flag9)
				{
					this.SetElement_MerchantData(charId, merchantData, ctx);
				}
				else
				{
					this.AddElement_MerchantData(charId, merchantData, ctx);
				}
			}
			short settlementId = DomainManager.Character.GetAliveOrgDeadCharacterOrgInfo(charId).SettlementId;
			bool favorabilityFull = false;
			Sect sect;
			bool flag10 = DomainManager.Organization.TryGetElement_Sects(settlementId, out sect);
			if (flag10)
			{
				favorabilityFull = (sect.CalcApprovingRate() >= 800);
			}
			OpenShopEventArguments openShopEventArguments = new OpenShopEventArguments
			{
				Id = charId,
				Refresh = refresh,
				IgnoreWorldProgress = false,
				IgnoreFavorability = favorabilityFull,
				MerchantSourceType = 8,
				SettlementId = settlementId
			};
			GameDataBridge.AddDisplayEvent<OpenShopEventArguments>(DisplayEventType.OpenShop, openShopEventArguments);
		}

		// Token: 0x06004929 RID: 18729 RVA: 0x002954AC File Offset: 0x002936AC
		public void StartBuildingShopAction(OpenShopEventArguments.EMerchantSourceType merchantSourceType, sbyte merchantType)
		{
			OpenShopEventArguments openShopEventArguments = new OpenShopEventArguments
			{
				BuildingMerchantType = merchantType,
				MerchantSourceType = (sbyte)merchantSourceType
			};
			GameDataBridge.AddDisplayEvent<OpenShopEventArguments>(DisplayEventType.OpenShop, openShopEventArguments);
		}

		// Token: 0x0600492A RID: 18730 RVA: 0x002954D8 File Offset: 0x002936D8
		public void ClearTempCaravan(DataContext context)
		{
			MerchantData merchantData;
			bool flag = this.TryGetElement_CaravanData(-1, out merchantData);
			if (flag)
			{
				merchantData.RemoveAllGoods(context);
				this.RemoveElement_CaravanData(-1, context);
			}
		}

		// Token: 0x0600492B RID: 18731 RVA: 0x00295506 File Offset: 0x00293706
		public void SetCaravanData(int caravanId, MerchantData merchantData, DataContext context)
		{
			this.SetElement_CaravanData(caravanId, merchantData, context);
		}

		// Token: 0x0600492C RID: 18732 RVA: 0x00295514 File Offset: 0x00293714
		[DomainMethod]
		public List<int> GetTaiwuLocationMaxLevelCaravanIdList()
		{
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			List<int> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<int> caravanIdList = new List<int>();
				foreach (KeyValuePair<int, CaravanPath> keyValuePair in this._caravanDict)
				{
					int num;
					CaravanPath caravanPath;
					keyValuePair.Deconstruct(out num, out caravanPath);
					int id = num;
					CaravanPath path = caravanPath;
					Location caravanLocation = path.GetCurrLocation();
					bool flag2 = caravanLocation != taiwuLocation;
					if (!flag2)
					{
						caravanIdList.Add(id);
					}
				}
				result = caravanIdList;
			}
			return result;
		}

		// Token: 0x0600492D RID: 18733 RVA: 0x002955CC File Offset: 0x002937CC
		public bool TryGetFirstTaiwuLocationCaravanId(out int res)
		{
			res = -1;
			Location taiwuLocation = DomainManager.Taiwu.GetTaiwu().GetLocation();
			bool flag = !taiwuLocation.IsValid();
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				foreach (KeyValuePair<int, CaravanPath> keyValuePair in this._caravanDict)
				{
					int num;
					CaravanPath caravanPath;
					keyValuePair.Deconstruct(out num, out caravanPath);
					int id = num;
					CaravanPath path = caravanPath;
					Location caravanLocation = path.GetCurrLocation();
					bool flag2 = caravanLocation == taiwuLocation;
					if (flag2)
					{
						res = id;
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x0600492E RID: 18734 RVA: 0x0029567C File Offset: 0x0029387C
		private void CorrectCaravanCurrentLocation(int caravanId)
		{
			CaravanPath path = this._caravanDict[caravanId];
			int nodeIndex = 0;
			while (path.MoveNodes[nodeIndex] < 0)
			{
				nodeIndex++;
			}
			Location currLocation = path.FullPath[path.MoveNodes[nodeIndex]];
			MapBlockData mapBlockData = DomainManager.Map.GetBlockData(currLocation.AreaId, currLocation.BlockId);
			bool flag = FiveLoongDlcEntry.IsBlockLoongBlock(mapBlockData);
			if (flag)
			{
				List<MapBlockData> neighborBlockList = ObjectPool<List<MapBlockData>>.Instance.Get();
				neighborBlockList.Clear();
				DomainManager.Map.GetNeighborBlocks(currLocation.AreaId, currLocation.BlockId, neighborBlockList, 4);
				foreach (MapBlockData neighbor in neighborBlockList)
				{
					bool flag2 = FiveLoongDlcEntry.IsBlockLoongBlock(neighbor);
					if (flag2)
					{
						path.FullPath[path.MoveNodes[nodeIndex]] = new Location(neighbor.AreaId, neighbor.BlockId);
						break;
					}
				}
				ObjectPool<List<MapBlockData>>.Instance.Return(neighborBlockList);
			}
		}

		// Token: 0x0600492F RID: 18735 RVA: 0x002957AC File Offset: 0x002939AC
		public int TryGetCaravanIdByTypeAndLevel(sbyte merchantType, sbyte level, int goodIndex, int goodCount)
		{
			foreach (KeyValuePair<int, MerchantData> pair in this._caravanData)
			{
				bool flag = pair.Value.MerchantType != merchantType;
				if (!flag)
				{
					bool flag2 = pair.Value.MerchantConfig.Level != level;
					if (!flag2)
					{
						bool flag3 = pair.Value.GetGoodsList(goodIndex).Items.Keys.Count < goodCount;
						if (!flag3)
						{
							return pair.Key;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x06004930 RID: 18736 RVA: 0x0029586C File Offset: 0x00293A6C
		public void DeleteCaravanItem(DataContext context, int caravanId, int index, ItemKey itemKey)
		{
			MerchantData caravanMerchantData = DomainManager.Merchant.GetCaravanMerchantData(context, caravanId);
			Inventory inventory = caravanMerchantData.GetGoodsList(index);
			ItemBase item = DomainManager.Item.GetBaseItem(itemKey);
			item.RemoveOwner(ItemOwnerType.Caravan, caravanId);
			inventory.OfflineRemove(itemKey, 1);
			this.SetCaravanData(caravanId, caravanMerchantData, context);
		}

		// Token: 0x06004931 RID: 18737 RVA: 0x002958BC File Offset: 0x00293ABC
		[DomainMethod]
		public void GmCmd_SetCaravanInvested(DataContext context, int caravanId, bool isInvested)
		{
			CaravanExtraData caravanExtraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			if (flag)
			{
				caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
			}
			caravanExtraData.IsInvested = isInvested;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		}

		// Token: 0x06004932 RID: 18738 RVA: 0x002958FC File Offset: 0x00293AFC
		[DomainMethod]
		public void GmCmd_SetAllCaravanInvested(DataContext context, bool isInvested)
		{
			foreach (int caravanId in this._caravanData.Keys)
			{
				CaravanExtraData caravanExtraData;
				bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
				if (flag)
				{
					caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
				}
				caravanExtraData.IsInvested = isInvested;
				DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
			}
		}

		// Token: 0x06004933 RID: 18739 RVA: 0x00295984 File Offset: 0x00293B84
		[DomainMethod]
		public void GmCmd_SetCaravanState(DataContext context, int caravanId, sbyte caravanState)
		{
			CaravanExtraData caravanExtraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			if (flag)
			{
				caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
			}
			caravanExtraData.State = caravanState;
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
			this.RefreshCaravanInTaiwuState(context);
		}

		// Token: 0x06004934 RID: 18740 RVA: 0x002959CC File Offset: 0x00293BCC
		[DomainMethod]
		public void GmCmd_SetCaravanRobbedRate(DataContext context, int caravanId, short robbedRate)
		{
			CaravanExtraData caravanExtraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			if (flag)
			{
				caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
			}
			caravanExtraData.RobbedRate = (short)Math.Clamp((int)robbedRate, 0, 1000);
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		}

		// Token: 0x06004935 RID: 18741 RVA: 0x00295A18 File Offset: 0x00293C18
		[DomainMethod]
		public void GmCmd_SetCaravanIncomeData(DataContext context, int caravanId, short incomeBonus, short incomeCriticalRate, short incomeCriticalResult)
		{
			CaravanExtraData caravanExtraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData);
			if (flag)
			{
				caravanExtraData = this.CreateCaravanExtraData(context, caravanId);
			}
			caravanExtraData.IncomeBonus = (short)Math.Clamp((int)incomeBonus, 0, 1000);
			caravanExtraData.IncomeCriticalRate = (short)Math.Clamp((int)incomeCriticalRate, 0, 1000);
			short incomeCriticalResultMin = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.First<short>();
			short incomeCriticalResultMax = GlobalConfig.Instance.CaravanIncomeCriticalResultRange.Last<short>();
			caravanExtraData.IncomeCriticalResult = (short)Math.Clamp((int)incomeCriticalResult, (int)incomeCriticalResultMin, (int)incomeCriticalResultMax);
			DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
		}

		// Token: 0x06004936 RID: 18742 RVA: 0x00295AA8 File Offset: 0x00293CA8
		[DomainMethod]
		public void GmCmd_ProtectCaravan(DataContext context, int caravanId)
		{
			CaravanExtraData caravanExtraData;
			bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData) || caravanExtraData.RobbedRate == 0;
			if (!flag)
			{
				int time = DomainManager.World.GetCurrDate();
				CaravanExtraData caravanExtraData2 = caravanExtraData;
				caravanExtraData2.RobbedRate /= 2;
				DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
				DomainManager.Extra.SetProtectCaravanTime(time, context);
			}
		}

		// Token: 0x06004937 RID: 18743 RVA: 0x00295B0C File Offset: 0x00293D0C
		[DomainMethod]
		public void GmCmd_ProtectAllCaravan(DataContext context)
		{
			foreach (int caravanId in this._caravanData.Keys)
			{
				CaravanExtraData caravanExtraData;
				bool flag = !DomainManager.Extra.TryGetCaravanExtraData(caravanId, out caravanExtraData) || caravanExtraData.RobbedRate == 0;
				if (!flag)
				{
					CaravanExtraData caravanExtraData2 = caravanExtraData;
					caravanExtraData2.RobbedRate /= 2;
					DomainManager.Extra.SetCaravanExtraData(context, caravanId, caravanExtraData);
				}
			}
			int time = DomainManager.World.GetCurrDate();
			DomainManager.Extra.SetProtectCaravanTime(time, context);
		}

		// Token: 0x06004938 RID: 18744 RVA: 0x00295BBC File Offset: 0x00293DBC
		public MerchantDomain() : base(7)
		{
			this._merchantData = new Dictionary<int, MerchantData>(0);
			this._merchantFavorability = new int[7];
			this._merchantMoney = new int[7];
			this._merchantMaxLevelData = new MerchantData[7];
			this._nextCaravanId = 0;
			this._caravanData = new Dictionary<int, MerchantData>(0);
			this._caravanDict = new Dictionary<int, CaravanPath>(0);
			this.OnInitializedDomainData();
		}

		// Token: 0x06004939 RID: 18745 RVA: 0x00295C64 File Offset: 0x00293E64
		private MerchantData GetElement_MerchantData(int elementId)
		{
			return this._merchantData[elementId];
		}

		// Token: 0x0600493A RID: 18746 RVA: 0x00295C84 File Offset: 0x00293E84
		private bool TryGetElement_MerchantData(int elementId, out MerchantData value)
		{
			return this._merchantData.TryGetValue(elementId, out value);
		}

		// Token: 0x0600493B RID: 18747 RVA: 0x00295CA4 File Offset: 0x00293EA4
		private unsafe void AddElement_MerchantData(int elementId, MerchantData value, DataContext context)
		{
			this._merchantData.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(14, 0, elementId, 0);
			}
		}

		// Token: 0x0600493C RID: 18748 RVA: 0x00295D08 File Offset: 0x00293F08
		private unsafe void SetElement_MerchantData(int elementId, MerchantData value, DataContext context)
		{
			this._merchantData[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(14, 0, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(14, 0, elementId, 0);
			}
		}

		// Token: 0x0600493D RID: 18749 RVA: 0x00295D6A File Offset: 0x00293F6A
		private void RemoveElement_MerchantData(int elementId, DataContext context)
		{
			this._merchantData.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(14, 0, elementId);
		}

		// Token: 0x0600493E RID: 18750 RVA: 0x00295D97 File Offset: 0x00293F97
		private void ClearMerchantData(DataContext context)
		{
			this._merchantData.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(0, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(14, 0);
		}

		// Token: 0x0600493F RID: 18751 RVA: 0x00295DC4 File Offset: 0x00293FC4
		public int[] GetMerchantFavorability()
		{
			return this._merchantFavorability;
		}

		// Token: 0x06004940 RID: 18752 RVA: 0x00295DDC File Offset: 0x00293FDC
		public unsafe void SetMerchantFavorability(int[] value, DataContext context)
		{
			this._merchantFavorability = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(1, this.DataStates, MerchantDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(14, 1, 28);
			for (int i = 0; i < 7; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._merchantFavorability[i];
			}
			pData += 28;
		}

		// Token: 0x06004941 RID: 18753 RVA: 0x00295E34 File Offset: 0x00294034
		public int[] GetMerchantMoney()
		{
			return this._merchantMoney;
		}

		// Token: 0x06004942 RID: 18754 RVA: 0x00295E4C File Offset: 0x0029404C
		public unsafe void SetMerchantMoney(int[] value, DataContext context)
		{
			this._merchantMoney = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(2, this.DataStates, MerchantDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(14, 2, 28);
			for (int i = 0; i < 7; i++)
			{
				*(int*)(pData + (IntPtr)i * 4) = this._merchantMoney[i];
			}
			pData += 28;
		}

		// Token: 0x06004943 RID: 18755 RVA: 0x00295EA4 File Offset: 0x002940A4
		public MerchantData GetElement_MerchantMaxLevelData(int index)
		{
			return this._merchantMaxLevelData[index];
		}

		// Token: 0x06004944 RID: 18756 RVA: 0x00295EC0 File Offset: 0x002940C0
		public unsafe void SetElement_MerchantMaxLevelData(int index, MerchantData value, DataContext context)
		{
			this._merchantMaxLevelData[index] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(index, this._dataStatesMerchantMaxLevelData, MerchantDomain.CacheInfluencesMerchantMaxLevelData, context);
			bool flag = value != null;
			if (flag)
			{
				int dataSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicElementList_Set(14, 3, index, dataSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicElementList_Set(14, 3, index, 0);
			}
		}

		// Token: 0x06004945 RID: 18757 RVA: 0x00295F20 File Offset: 0x00294120
		private int GetNextCaravanId()
		{
			return this._nextCaravanId;
		}

		// Token: 0x06004946 RID: 18758 RVA: 0x00295F38 File Offset: 0x00294138
		private unsafe void SetNextCaravanId(int value, DataContext context)
		{
			this._nextCaravanId = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(4, this.DataStates, MerchantDomain.CacheInfluences, context);
			byte* pData = OperationAdder.FixedSingleValue_Set(14, 4, 4);
			*(int*)pData = this._nextCaravanId;
			pData += 4;
		}

		// Token: 0x06004947 RID: 18759 RVA: 0x00295F78 File Offset: 0x00294178
		private MerchantData GetElement_CaravanData(int elementId)
		{
			return this._caravanData[elementId];
		}

		// Token: 0x06004948 RID: 18760 RVA: 0x00295F98 File Offset: 0x00294198
		private bool TryGetElement_CaravanData(int elementId, out MerchantData value)
		{
			return this._caravanData.TryGetValue(elementId, out value);
		}

		// Token: 0x06004949 RID: 18761 RVA: 0x00295FB8 File Offset: 0x002941B8
		private unsafe void AddElement_CaravanData(int elementId, MerchantData value, DataContext context)
		{
			this._caravanData.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 5, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(14, 5, elementId, 0);
			}
		}

		// Token: 0x0600494A RID: 18762 RVA: 0x0029601C File Offset: 0x0029421C
		private unsafe void SetElement_CaravanData(int elementId, MerchantData value, DataContext context)
		{
			this._caravanData[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(14, 5, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(14, 5, elementId, 0);
			}
		}

		// Token: 0x0600494B RID: 18763 RVA: 0x0029607E File Offset: 0x0029427E
		private void RemoveElement_CaravanData(int elementId, DataContext context)
		{
			this._caravanData.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(14, 5, elementId);
		}

		// Token: 0x0600494C RID: 18764 RVA: 0x002960AB File Offset: 0x002942AB
		private void ClearCaravanData(DataContext context)
		{
			this._caravanData.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(5, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(14, 5);
		}

		// Token: 0x0600494D RID: 18765 RVA: 0x002960D8 File Offset: 0x002942D8
		private CaravanPath GetElement_CaravanDict(int elementId)
		{
			return this._caravanDict[elementId];
		}

		// Token: 0x0600494E RID: 18766 RVA: 0x002960F8 File Offset: 0x002942F8
		private bool TryGetElement_CaravanDict(int elementId, out CaravanPath value)
		{
			return this._caravanDict.TryGetValue(elementId, out value);
		}

		// Token: 0x0600494F RID: 18767 RVA: 0x00296118 File Offset: 0x00294318
		private unsafe void AddElement_CaravanDict(int elementId, CaravanPath value, DataContext context)
		{
			this._caravanDict.Add(elementId, value);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 6, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Add<int>(14, 6, elementId, 0);
			}
		}

		// Token: 0x06004950 RID: 18768 RVA: 0x0029617C File Offset: 0x0029437C
		private unsafe void SetElement_CaravanDict(int elementId, CaravanPath value, DataContext context)
		{
			this._caravanDict[elementId] = value;
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MerchantDomain.CacheInfluences, context);
			bool flag = value != null;
			if (flag)
			{
				int contentSize = value.GetSerializedSize();
				byte* pData = OperationAdder.DynamicSingleValueCollection_Set<int>(14, 6, elementId, contentSize);
				pData += value.Serialize(pData);
			}
			else
			{
				OperationAdder.DynamicSingleValueCollection_Set<int>(14, 6, elementId, 0);
			}
		}

		// Token: 0x06004951 RID: 18769 RVA: 0x002961DE File Offset: 0x002943DE
		private void RemoveElement_CaravanDict(int elementId, DataContext context)
		{
			this._caravanDict.Remove(elementId);
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Remove<int>(14, 6, elementId);
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x0029620B File Offset: 0x0029440B
		private void ClearCaravanDict(DataContext context)
		{
			this._caravanDict.Clear();
			BaseGameDataDomain.SetModifiedAndInvalidateInfluencedCache(6, this.DataStates, MerchantDomain.CacheInfluences, context);
			OperationAdder.DynamicSingleValueCollection_Clear(14, 6);
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x00296236 File Offset: 0x00294436
		public override void OnInitializeGameDataModule()
		{
			this.InitializeOnInitializeGameDataModule();
		}

		// Token: 0x06004954 RID: 18772 RVA: 0x00296240 File Offset: 0x00294440
		public unsafe override void OnEnterNewWorld()
		{
			this.InitializeOnEnterNewWorld();
			this.InitializeInternalDataOfCollections();
			foreach (KeyValuePair<int, MerchantData> entry in this._merchantData)
			{
				int elementId = entry.Key;
				MerchantData value = entry.Value;
				bool flag = value != null;
				if (flag)
				{
					int contentSize = value.GetSerializedSize();
					byte* pData = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 0, elementId, contentSize);
					pData += value.Serialize(pData);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(14, 0, elementId, 0);
				}
			}
			byte* pData2 = OperationAdder.FixedSingleValue_Set(14, 1, 28);
			for (int i = 0; i < 7; i++)
			{
				*(int*)(pData2 + (IntPtr)i * 4) = this._merchantFavorability[i];
			}
			pData2 += 28;
			byte* pData3 = OperationAdder.FixedSingleValue_Set(14, 2, 28);
			for (int j = 0; j < 7; j++)
			{
				*(int*)(pData3 + (IntPtr)j * 4) = this._merchantMoney[j];
			}
			pData3 += 28;
			int dataSize = 0;
			for (int k = 0; k < 7; k++)
			{
				MerchantData element = this._merchantMaxLevelData[k];
				bool flag2 = element != null;
				if (flag2)
				{
					dataSize += 4 + element.GetSerializedSize();
				}
				else
				{
					dataSize += 4;
				}
			}
			byte* pData4 = OperationAdder.DynamicElementList_InsertRange(14, 3, 0, 7, dataSize);
			for (int l = 0; l < 7; l++)
			{
				MerchantData element2 = this._merchantMaxLevelData[l];
				bool flag3 = element2 != null;
				if (flag3)
				{
					byte* pSubContentSize = pData4;
					pData4 += 4;
					int subContentSize = element2.Serialize(pData4);
					pData4 += subContentSize;
					*(int*)pSubContentSize = subContentSize;
				}
				else
				{
					*(int*)pData4 = 0;
					pData4 += 4;
				}
			}
			byte* pData5 = OperationAdder.FixedSingleValue_Set(14, 4, 4);
			*(int*)pData5 = this._nextCaravanId;
			pData5 += 4;
			foreach (KeyValuePair<int, MerchantData> entry2 in this._caravanData)
			{
				int elementId2 = entry2.Key;
				MerchantData value2 = entry2.Value;
				bool flag4 = value2 != null;
				if (flag4)
				{
					int contentSize2 = value2.GetSerializedSize();
					byte* pData6 = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 5, elementId2, contentSize2);
					pData6 += value2.Serialize(pData6);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(14, 5, elementId2, 0);
				}
			}
			foreach (KeyValuePair<int, CaravanPath> entry3 in this._caravanDict)
			{
				int elementId3 = entry3.Key;
				CaravanPath value3 = entry3.Value;
				bool flag5 = value3 != null;
				if (flag5)
				{
					int contentSize3 = value3.GetSerializedSize();
					byte* pData7 = OperationAdder.DynamicSingleValueCollection_Add<int>(14, 6, elementId3, contentSize3);
					pData7 += value3.Serialize(pData7);
				}
				else
				{
					OperationAdder.DynamicSingleValueCollection_Add<int>(14, 6, elementId3, 0);
				}
			}
		}

		// Token: 0x06004955 RID: 18773 RVA: 0x0029655C File Offset: 0x0029475C
		public override void OnLoadWorld()
		{
			this._pendingLoadingOperationIds = new Queue<uint>();
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 0));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 1));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 2));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicElementList_GetAll(14, 3));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.FixedSingleValue_Get(14, 4));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 5));
			this._pendingLoadingOperationIds.Enqueue(OperationAdder.DynamicSingleValueCollection_GetAll(14, 6));
		}

		// Token: 0x06004956 RID: 18774 RVA: 0x00296604 File Offset: 0x00294804
		public override int GetData(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool, bool resetModified)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				result = Serializer.Serialize(this._merchantFavorability, dataPool);
				break;
			case 2:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				result = Serializer.Serialize(this._merchantMoney, dataPool);
				break;
			case 3:
				if (resetModified)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
				}
				result = Serializer.Serialize(this._merchantMaxLevelData[(int)subId0], dataPool);
				break;
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to get value of dataId: ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004957 RID: 18775 RVA: 0x002967A0 File Offset: 0x002949A0
		public override void SetData(ushort dataId, ulong subId0, uint subId1, int valueOffset, RawDataPool dataPool, DataContext context)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
				Serializer.Deserialize(dataPool, valueOffset, ref this._merchantFavorability);
				this.SetMerchantFavorability(this._merchantFavorability, context);
				break;
			case 2:
				Serializer.Deserialize(dataPool, valueOffset, ref this._merchantMoney);
				this.SetMerchantMoney(this._merchantMoney, context);
				break;
			case 3:
			{
				MerchantData value = this._merchantMaxLevelData[(int)subId0];
				Serializer.Deserialize(dataPool, valueOffset, ref value);
				this._merchantMaxLevelData[(int)subId0] = value;
				this.SetElement_MerchantMaxLevelData((int)subId0, value, context);
				break;
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to set value of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x06004958 RID: 18776 RVA: 0x00296938 File Offset: 0x00294B38
		public override int CallMethod(Operation operation, RawDataPool argDataPool, RawDataPool returnDataPool, DataContext context)
		{
			int argsOffset = operation.ArgsOffset;
			int result;
			switch (operation.MethodId)
			{
			case 0:
			{
				int argsCount = operation.ArgsCount;
				int num = argsCount;
				if (num != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId);
				MerchantData returnValue = this.GetMerchantData(context, charId);
				result = Serializer.Serialize(returnValue, returnDataPool);
				break;
			}
			case 1:
			{
				int argsCount2 = operation.ArgsCount;
				int num2 = argsCount2;
				if (num2 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				MerchantTradeArguments merchantTradeArguments = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantTradeArguments);
				this.SettleTrade(context, merchantTradeArguments);
				result = -1;
				break;
			}
			case 2:
			{
				int argsCount3 = operation.ArgsCount;
				int num3 = argsCount3;
				if (num3 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int npcId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref npcId);
				List<ItemDisplayData> boughtItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref boughtItems);
				List<ItemDisplayData> soldItems = null;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref soldItems);
				int selfAuthority = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref selfAuthority);
				int npcAuthority = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref npcAuthority);
				this.ExchangeBook(context, npcId, boughtItems, soldItems, selfAuthority, npcAuthority);
				result = -1;
				break;
			}
			case 3:
			{
				int argsCount4 = operation.ArgsCount;
				int num4 = argsCount4;
				if (num4 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.PullTradeCaravanLocations(context);
				result = -1;
				break;
			}
			case 4:
			{
				int argsCount5 = operation.ArgsCount;
				int num5 = argsCount5;
				if (num5 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId);
				MerchantData returnValue2 = this.GetCaravanMerchantData(context, caravanId);
				result = Serializer.Serialize(returnValue2, returnDataPool);
				break;
			}
			case 5:
			{
				int argsCount6 = operation.ArgsCount;
				int num6 = argsCount6;
				if (num6 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int npcId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref npcId2);
				bool isFavor = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isFavor);
				List<ItemDisplayData> returnValue3 = this.GetTradeBookDisplayData(context, npcId2, isFavor);
				result = Serializer.Serialize(returnValue3, returnDataPool);
				break;
			}
			case 6:
			{
				int argsCount7 = operation.ArgsCount;
				int num7 = argsCount7;
				if (num7 != 5)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId2);
				sbyte itemType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref itemType);
				short templateId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref templateId);
				int count = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref count);
				int level = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref level);
				this.GmCmd_AddItem(context, charId2, itemType, templateId, count, level);
				result = -1;
				break;
			}
			case 7:
			{
				int argsCount8 = operation.ArgsCount;
				int num8 = argsCount8;
				if (num8 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<int> returnValue4 = this.GetTaiwuLocationMaxLevelCaravanIdList();
				result = Serializer.Serialize(returnValue4, returnDataPool);
				break;
			}
			case 8:
			{
				int argsCount9 = operation.ArgsCount;
				int num9 = argsCount9;
				if (num9 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType);
				int returnValue5 = this.GetCurFavorability(merchantType);
				result = Serializer.Serialize(returnValue5, returnDataPool);
				break;
			}
			case 9:
			{
				int argsCount10 = operation.ArgsCount;
				int num10 = argsCount10;
				if (num10 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId3);
				bool isFavor2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isFavor2);
				this.FinishBookTrade(context, charId3, isFavor2);
				result = -1;
				break;
			}
			case 10:
			{
				int argsCount11 = operation.ArgsCount;
				int num11 = argsCount11;
				if (num11 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				List<ItemDisplayData> returnValue6 = this.GetTradeBackBookDisplayData();
				result = Serializer.Serialize(returnValue6, returnDataPool);
				break;
			}
			case 11:
			{
				int argsCount12 = operation.ArgsCount;
				int num12 = argsCount12;
				if (num12 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType2);
				List<MerchantInfoCaravanData> returnValue7 = this.GetMerchantInfoCaravanDataList(context, merchantType2);
				result = Serializer.Serialize(returnValue7, returnDataPool);
				break;
			}
			case 12:
			{
				int argsCount13 = operation.ArgsCount;
				int num13 = argsCount13;
				if (num13 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType3);
				List<MerchantInfoAreaData> returnValue8 = this.GetMerchantInfoAreaDataList(merchantType3);
				result = Serializer.Serialize(returnValue8, returnDataPool);
				break;
			}
			case 13:
			{
				int argsCount14 = operation.ArgsCount;
				int num14 = argsCount14;
				if (num14 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType4);
				List<MerchantInfoMerchantData> returnValue9 = this.GetMerchantInfoMerchantDataList(merchantType4);
				result = Serializer.Serialize(returnValue9, returnDataPool);
				break;
			}
			case 14:
			{
				int argsCount15 = operation.ArgsCount;
				int num15 = argsCount15;
				if (num15 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType5);
				MerchantOverFavorData returnValue10 = this.GetMerchantOverFavorData(merchantType5);
				result = Serializer.Serialize(returnValue10, returnDataPool);
				break;
			}
			case 15:
			{
				int argsCount16 = operation.ArgsCount;
				int num16 = argsCount16;
				if (num16 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				sbyte merchantType6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType6);
				bool isHead = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isHead);
				MerchantData returnValue11 = this.GetBuildingMerchantData(context, merchantType6, isHead);
				result = Serializer.Serialize(returnValue11, returnDataPool);
				break;
			}
			case 16:
			{
				int argsCount17 = operation.ArgsCount;
				int num17 = argsCount17;
				if (num17 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId2);
				CaravanDisplayData returnValue12 = this.GetCaravanDisplayData(context, caravanId2);
				result = Serializer.Serialize(returnValue12, returnDataPool);
				break;
			}
			case 17:
			{
				int argsCount18 = operation.ArgsCount;
				int num18 = argsCount18;
				if (num18 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId3 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId3);
				this.InvestCaravan(context, caravanId3);
				result = -1;
				break;
			}
			case 18:
			{
				int argsCount19 = operation.ArgsCount;
				int num19 = argsCount19;
				if (num19 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int[] returnValue13 = this.GetAllFavorability();
				result = Serializer.Serialize(returnValue13, returnDataPool);
				break;
			}
			case 19:
			{
				int argsCount20 = operation.ArgsCount;
				int num20 = argsCount20;
				if (num20 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId4);
				this.ProtectCaravan(context, caravanId4);
				result = -1;
				break;
			}
			case 20:
			{
				int argsCount21 = operation.ArgsCount;
				int num21 = argsCount21;
				if (num21 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId5 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId5);
				this.GmCmd_ProtectCaravan(context, caravanId5);
				result = -1;
				break;
			}
			case 21:
			{
				int argsCount22 = operation.ArgsCount;
				int num22 = argsCount22;
				if (num22 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				this.GmCmd_ProtectAllCaravan(context);
				result = -1;
				break;
			}
			case 22:
			{
				int argsCount23 = operation.ArgsCount;
				int num23 = argsCount23;
				if (num23 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId6 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId6);
				short robbedRate = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref robbedRate);
				this.GmCmd_SetCaravanRobbedRate(context, caravanId6, robbedRate);
				result = -1;
				break;
			}
			case 23:
			{
				int argsCount24 = operation.ArgsCount;
				int num24 = argsCount24;
				if (num24 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId7 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId7);
				bool isInvested = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInvested);
				this.GmCmd_SetCaravanInvested(context, caravanId7, isInvested);
				result = -1;
				break;
			}
			case 24:
			{
				int argsCount25 = operation.ArgsCount;
				int num25 = argsCount25;
				if (num25 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				bool isInvested2 = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isInvested2);
				this.GmCmd_SetAllCaravanInvested(context, isInvested2);
				result = -1;
				break;
			}
			case 25:
			{
				int argsCount26 = operation.ArgsCount;
				int num26 = argsCount26;
				if (num26 != 2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId8 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId8);
				sbyte caravanState = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanState);
				this.GmCmd_SetCaravanState(context, caravanId8, caravanState);
				result = -1;
				break;
			}
			case 26:
			{
				int argsCount27 = operation.ArgsCount;
				int num27 = argsCount27;
				if (num27 != 4)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int caravanId9 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref caravanId9);
				short incomeBonus = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref incomeBonus);
				short incomeCriticalRate = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref incomeCriticalRate);
				short incomeCriticalResult = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref incomeCriticalResult);
				this.GmCmd_SetCaravanIncomeData(context, caravanId9, incomeBonus, incomeCriticalRate, incomeCriticalResult);
				result = -1;
				break;
			}
			case 27:
			{
				int argsCount28 = operation.ArgsCount;
				int num28 = argsCount28;
				if (num28 != 0)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				SectStorySpecialMerchant returnValue14 = this.GetSectStorySpecialMerchantData(context);
				result = Serializer.Serialize(returnValue14, returnDataPool);
				break;
			}
			case 28:
			{
				int argsCount29 = operation.ArgsCount;
				int num29 = argsCount29;
				if (num29 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charId4 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charId4);
				sbyte returnValue15 = this.GetMerchantTemplateId(charId4);
				result = Serializer.Serialize(returnValue15, returnDataPool);
				break;
			}
			case 29:
			{
				int argsCount30 = operation.ArgsCount;
				int num30 = argsCount30;
				if (num30 != 1)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				OpenShopEventArguments openShopEventArguments = new OpenShopEventArguments();
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref openShopEventArguments);
				MerchantBuyBackData returnValue16 = this.GetMerchantBuyBackData(openShopEventArguments);
				result = Serializer.Serialize(returnValue16, returnDataPool);
				break;
			}
			case 30:
			{
				int argsCount31 = operation.ArgsCount;
				int num31 = argsCount31;
				if (num31 != 6)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
					defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				int charOrCaravanId = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref charOrCaravanId);
				bool isChar = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isChar);
				sbyte level2 = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref level2);
				bool isFromBuilding = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isFromBuilding);
				bool isHeadBuildingMerchant = false;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref isHeadBuildingMerchant);
				sbyte buildingMerchantType = 0;
				argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref buildingMerchantType);
				bool returnValue17 = this.RefreshMerchantGoods(context, charOrCaravanId, isChar, level2, isFromBuilding, isHeadBuildingMerchant, buildingMerchantType);
				result = Serializer.Serialize(returnValue17, returnDataPool);
				break;
			}
			case 31:
			{
				int argsCount32 = operation.ArgsCount;
				int num32 = argsCount32;
				if (num32 != 1)
				{
					if (num32 != 2)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					sbyte merchantType7 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType7);
					int delta = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref delta);
					int returnValue18 = this.GetFavorabilityWithDelta(merchantType7, delta);
					result = Serializer.Serialize(returnValue18, returnDataPool);
				}
				else
				{
					sbyte merchantType8 = 0;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref merchantType8);
					int returnValue19 = this.GetFavorabilityWithDelta(merchantType8, 0);
					result = Serializer.Serialize(returnValue19, returnDataPool);
				}
				break;
			}
			case 32:
			{
				int argsCount33 = operation.ArgsCount;
				int num33 = argsCount33;
				if (num33 != 0)
				{
					if (num33 != 1)
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(35, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported argsCount of methodId: ");
						defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					bool consume = false;
					argsOffset += Serializer.Deserialize(argDataPool, argsOffset, ref consume);
					bool returnValue20 = this.CanRefreshMerchantGoods(context, consume);
					result = Serializer.Serialize(returnValue20, returnDataPool);
				}
				else
				{
					bool returnValue21 = this.CanRefreshMerchantGoods(context, false);
					result = Serializer.Serialize(returnValue21, returnDataPool);
				}
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(21, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported methodId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.MethodId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x06004959 RID: 18777 RVA: 0x00297998 File Offset: 0x00295B98
		public override void OnMonitorData(ushort dataId, ulong subId0, uint subId1, bool monitoring)
		{
			switch (dataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0600495A RID: 18778 RVA: 0x00297A0C File Offset: 0x00295C0C
		public override int CheckModified(ushort dataId, ulong subId0, uint subId1, RawDataPool dataPool)
		{
			int result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (flag)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
					result = Serializer.Serialize(this._merchantFavorability, dataPool);
				}
				break;
			}
			case 2:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (flag2)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
					result = Serializer.Serialize(this._merchantMoney, dataPool);
				}
				break;
			}
			case 3:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
				if (flag3)
				{
					result = -1;
				}
				else
				{
					BaseGameDataDomain.ResetModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
					result = Serializer.Serialize(this._merchantMaxLevelData[(int)subId0], dataPool);
				}
				break;
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to check modification of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x0600495B RID: 18779 RVA: 0x00297BE8 File Offset: 0x00295DE8
		public override void ResetModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
			{
				bool flag = !BaseGameDataDomain.IsModified(this.DataStates, 1);
				if (!flag)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 1);
				}
				break;
			}
			case 2:
			{
				bool flag2 = !BaseGameDataDomain.IsModified(this.DataStates, 2);
				if (!flag2)
				{
					BaseGameDataDomain.ResetModified(this.DataStates, 2);
				}
				break;
			}
			case 3:
			{
				bool flag3 = !BaseGameDataDomain.IsModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
				if (!flag3)
				{
					BaseGameDataDomain.ResetModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
				}
				break;
			}
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to reset modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
		}

		// Token: 0x0600495C RID: 18780 RVA: 0x00297D8C File Offset: 0x00295F8C
		public override bool IsModifiedWrapper(ushort dataId, ulong subId0, uint subId1)
		{
			bool result;
			switch (dataId)
			{
			case 0:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 1:
				result = BaseGameDataDomain.IsModified(this.DataStates, 1);
				break;
			case 2:
				result = BaseGameDataDomain.IsModified(this.DataStates, 2);
				break;
			case 3:
				result = BaseGameDataDomain.IsModified(this._dataStatesMerchantMaxLevelData, (int)subId0);
				break;
			case 4:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 5:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			case 6:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(49, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Not allow to verify modification state of dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(dataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x0600495D RID: 18781 RVA: 0x00297EE0 File Offset: 0x002960E0
		public override void InvalidateCache(BaseGameDataObject sourceObject, DataInfluence influence, DataContext context, bool unconditionallyInfluenceAll)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			switch (influence.TargetIndicator.DataId)
			{
			case 0:
				break;
			case 1:
				break;
			case 2:
				break;
			case 3:
				break;
			case 4:
				break;
			case 5:
				break;
			case 6:
				break;
			default:
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(48, 1);
			defaultInterpolatedStringHandler.AppendLiteral("Cannot invalidate cache state of non-cache data ");
			defaultInterpolatedStringHandler.AppendFormatted<ushort>(influence.TargetIndicator.DataId);
			throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
		}

		// Token: 0x0600495E RID: 18782 RVA: 0x00297F9C File Offset: 0x0029619C
		public unsafe override void ProcessArchiveResponse(OperationWrapper operation, byte* pResult)
		{
			switch (operation.DataId)
			{
			case 0:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<int, MerchantData>(operation, pResult, this._merchantData);
				break;
			case 1:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array<int>(operation, pResult, this._merchantFavorability, 7);
				break;
			case 2:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Array<int>(operation, pResult, this._merchantMoney, 7);
				break;
			case 3:
				ResponseProcessor.ProcessElementList_CustomType_Dynamic_Ref<MerchantData>(operation, pResult, this._merchantMaxLevelData, 7);
				break;
			case 4:
				ResponseProcessor.ProcessSingleValue_BasicType_Fixed_Value_Single<int>(operation, pResult, ref this._nextCaravanId);
				break;
			case 5:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<int, MerchantData>(operation, pResult, this._caravanData);
				break;
			case 6:
				ResponseProcessor.ProcessSingleValueCollection_CustomType_Dynamic_Ref<int, CaravanPath>(operation, pResult, this._caravanDict);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(19, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported dataId ");
				defaultInterpolatedStringHandler.AppendFormatted<ushort>(operation.DataId);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			bool flag = this._pendingLoadingOperationIds != null;
			if (flag)
			{
				uint currPendingOperationId = this._pendingLoadingOperationIds.Peek();
				bool flag2 = currPendingOperationId == operation.Id;
				if (flag2)
				{
					this._pendingLoadingOperationIds.Dequeue();
					bool flag3 = this._pendingLoadingOperationIds.Count <= 0;
					if (flag3)
					{
						this._pendingLoadingOperationIds = null;
						this.InitializeInternalDataOfCollections();
						this.OnLoadedArchiveData();
						DomainManager.Global.CompleteLoading(14);
					}
				}
			}
		}

		// Token: 0x0600495F RID: 18783 RVA: 0x002980FC File Offset: 0x002962FC
		private void InitializeInternalDataOfCollections()
		{
		}

		// Token: 0x06004961 RID: 18785 RVA: 0x00298124 File Offset: 0x00296324
		[CompilerGenerated]
		internal static bool <FixNonExistingMerchantGoods>g__FixAbnormalMerchantData|12_0(MerchantData merchantData, ref MerchantDomain.<>c__DisplayClass12_0 A_1)
		{
			bool flag = merchantData == null || merchantData.BuyInGoodsList == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				A_1.itemsToRemove.Clear();
				foreach (KeyValuePair<ItemKey, int> keyValuePair in merchantData.BuyInGoodsList.Items)
				{
					ItemKey itemKey3;
					int num;
					keyValuePair.Deconstruct(out itemKey3, out num);
					ItemKey itemKey = itemKey3;
					bool flag2 = !DomainManager.Item.ItemExists(itemKey);
					if (flag2)
					{
						A_1.itemsToRemove.Add(itemKey);
						Logger logger = MerchantDomain.Logger;
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(42, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Removing non-existing item ");
						defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
						defaultInterpolatedStringHandler.AppendLiteral(" from merchant.");
						logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					else
					{
						bool flag3 = ItemType.IsEquipmentItemType(itemKey.ItemType);
						if (flag3)
						{
							EquipmentBase item = DomainManager.Item.GetBaseEquipment(itemKey);
							int charId = item.GetEquippedCharId();
							bool flag4 = item.GetEquippedCharId() >= 0 || item.PrevOwner.OwnerType == ItemOwnerType.CharacterInventory || item.PrevOwner.OwnerType == ItemOwnerType.CharacterEquipment;
							if (flag4)
							{
								A_1.itemsToRemove.Add(itemKey);
								Logger logger2 = MerchantDomain.Logger;
								DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(66, 2);
								defaultInterpolatedStringHandler.AppendLiteral("Removing already item ");
								defaultInterpolatedStringHandler.AppendFormatted<ItemKey>(itemKey);
								defaultInterpolatedStringHandler.AppendLiteral(" from merchant which is already equipped by ");
								defaultInterpolatedStringHandler.AppendFormatted<int>(charId);
								logger2.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
							}
						}
					}
				}
				bool flag5 = A_1.itemsToRemove.Count <= 0;
				if (flag5)
				{
					result = false;
				}
				else
				{
					foreach (ItemKey itemKey2 in A_1.itemsToRemove)
					{
						merchantData.BuyInGoodsList.Items.Remove(itemKey2);
					}
					result = true;
				}
			}
			return result;
		}

		// Token: 0x06004962 RID: 18786 RVA: 0x00298360 File Offset: 0x00296560
		[CompilerGenerated]
		private void <SettleTrade>g__HandleHeadMoney|17_0(int argTradeMoney, ref MerchantDomain.<>c__DisplayClass17_0 A_2)
		{
			bool isDebtAreaShop = A_2.isDebtAreaShop;
			if (!isDebtAreaShop)
			{
				int money = this.GetMerchantMoney(A_2.context, A_2.merchantData.MerchantType);
				money -= argTradeMoney;
				this.SetMerchantMoney(A_2.context, A_2.merchantData.MerchantType, money);
			}
		}

		// Token: 0x06004963 RID: 18787 RVA: 0x002983B0 File Offset: 0x002965B0
		[CompilerGenerated]
		private int <SettleTrade>g__HandleMerchantMoney|17_1(ref MerchantDomain.<>c__DisplayClass17_0 A_1)
		{
			bool isDebtAreaShop = A_1.isDebtAreaShop;
			int result;
			if (isDebtAreaShop)
			{
				result = A_1.tradeMoneySources.Values.Sum();
			}
			else
			{
				bool flag = A_1.merchantData.CharId >= 0;
				if (flag)
				{
					GameData.Domains.Character.Character character = DomainManager.Character.GetElement_Objects(A_1.merchantData.CharId);
					int totalTradeMoney = 0;
					MerchantBuyBackData oldBuyBackData = A_1.oldBuyBackData;
					ICollection<ItemKey> collection2;
					if (((oldBuyBackData != null) ? oldBuyBackData.BuyInGoodsList.Items : null) == null)
					{
						ICollection<ItemKey> collection = Array.Empty<ItemKey>();
						collection2 = collection;
					}
					else
					{
						ICollection<ItemKey> collection = A_1.oldBuyBackData.BuyInGoodsList.Items.Keys;
						collection2 = collection;
					}
					ICollection<ItemKey> oldBuyBackList = collection2;
					MerchantBuyBackData merchantBuyBackData = A_1.merchantTradeArguments.MerchantBuyBackData;
					ICollection<ItemKey> collection3;
					if (((merchantBuyBackData != null) ? merchantBuyBackData.BuyInGoodsList.Items : null) == null)
					{
						ICollection<ItemKey> collection = Array.Empty<ItemKey>();
						collection3 = collection;
					}
					else
					{
						ICollection<ItemKey> collection = A_1.merchantTradeArguments.MerchantBuyBackData.BuyInGoodsList.Items.Keys;
						collection3 = collection;
					}
					ICollection<ItemKey> currentBuyBackList = collection3;
					foreach (KeyValuePair<ItemKey, int> keyValuePair in A_1.tradeMoneySources)
					{
						ItemKey itemKey2;
						int num;
						keyValuePair.Deconstruct(out itemKey2, out num);
						ItemKey itemKey = itemKey2;
						int tradeMoney = num;
						bool isBuyBack = oldBuyBackList.Contains(itemKey) && !currentBuyBackList.Contains(itemKey);
						bool flag2 = tradeMoney < 0 && !isBuyBack;
						if (flag2)
						{
							int personalMoney = -tradeMoney * 20 / 100;
							int publicMoney = -tradeMoney - personalMoney;
							character.ChangeResource(A_1.context, 6, personalMoney);
							A_1.merchantData.Money += publicMoney;
						}
						else
						{
							A_1.merchantData.Money -= tradeMoney;
						}
						totalTradeMoney += tradeMoney;
					}
					result = totalTradeMoney;
				}
				else
				{
					int tradeMoney2 = Math.Min(A_1.tradeMoneySources.Values.Sum(), A_1.merchantData.Money);
					A_1.merchantData.Money -= tradeMoney2;
					result = tradeMoney2;
				}
			}
			return result;
		}

		// Token: 0x06004964 RID: 18788 RVA: 0x002985C0 File Offset: 0x002967C0
		[CompilerGenerated]
		internal static void <InitializeOwnedItems>g__InitializeOwnedItemsFromMerchant|34_0(ItemOwnerType ownerType, int id, MerchantData merchantData)
		{
			for (int i = 0; i < 7; i++)
			{
				Inventory goodsList = merchantData.GetGoodsList(i);
				foreach (KeyValuePair<ItemKey, int> keyValuePair in goodsList.Items)
				{
					ItemKey itemKey3;
					int num;
					keyValuePair.Deconstruct(out itemKey3, out num);
					ItemKey itemKey = itemKey3;
					DomainManager.Item.SetOwner(itemKey, ownerType, id);
				}
			}
			bool flag = merchantData.BuyInGoodsList != null;
			if (flag)
			{
				foreach (KeyValuePair<ItemKey, int> keyValuePair in merchantData.BuyInGoodsList.Items)
				{
					ItemKey itemKey3;
					int num;
					keyValuePair.Deconstruct(out itemKey3, out num);
					ItemKey itemKey2 = itemKey3;
					DomainManager.Item.SetOwner(itemKey2, ownerType, id);
				}
			}
		}

		// Token: 0x06004965 RID: 18789 RVA: 0x002986BC File Offset: 0x002968BC
		[CompilerGenerated]
		internal static bool <RemoveBuyBackItem>g__RemoveButBackItemInInventory|62_0(IEnumerable<MerchantBuyBackData> buyBackDataCollection, ref MerchantDomain.<>c__DisplayClass62_0 A_1)
		{
			foreach (MerchantBuyBackData buyBackData in buyBackDataCollection)
			{
				bool flag = buyBackData == null || !buyBackData.BuyInGoodsList.Items.ContainsKey(A_1.itemKey);
				if (!flag)
				{
					buyBackData.BuyInGoodsList.OfflineRemove(A_1.itemKey, 1);
					ItemBase item = DomainManager.Item.GetBaseItem(A_1.itemKey);
					item.RemoveOwner(item.Owner.OwnerType, item.Owner.OwnerId);
					return true;
				}
			}
			return false;
		}

		// Token: 0x06004966 RID: 18790 RVA: 0x00298774 File Offset: 0x00296974
		[CompilerGenerated]
		internal static ItemKey <TryGetBuyBackItemForPersonalNeed>g__GetBuyBackItemInInventory|63_0(IEnumerable<MerchantBuyBackData> buyBackDataCollection, ref MerchantDomain.<>c__DisplayClass63_0 A_1)
		{
			foreach (MerchantBuyBackData buyBackData in buyBackDataCollection)
			{
				bool flag = buyBackData == null;
				if (!flag)
				{
					ItemKey selectedItemKey = buyBackData.TryGetGoodInSameGroup(A_1.context, A_1.personalNeed.ItemType, A_1.personalNeed.ItemTemplateId, -2, true);
					bool flag2 = selectedItemKey.IsValid();
					if (flag2)
					{
						return selectedItemKey;
					}
				}
			}
			return ItemKey.Invalid;
		}

		// Token: 0x06004967 RID: 18791 RVA: 0x00298808 File Offset: 0x00296A08
		[CompilerGenerated]
		internal static void <RemoveAllGoodsInMerchantBuyBackData>g__RemoveAllGoods|64_0(IEnumerable<MerchantBuyBackData> buyBackDataCollection, ref MerchantDomain.<>c__DisplayClass64_0 A_1)
		{
			foreach (MerchantBuyBackData buyBackData in buyBackDataCollection)
			{
				if (buyBackData != null)
				{
					buyBackData.RemoveAllGoods(A_1.context);
				}
			}
		}

		// Token: 0x06004968 RID: 18792 RVA: 0x00298860 File Offset: 0x00296A60
		[CompilerGenerated]
		private int <GenTradeCaravansOnAdvanceMonth>g__CreateCaravan|77_0(MerchantItem config, Location destLocation, ref MerchantDomain.<>c__DisplayClass77_0 A_3, ref MerchantDomain.<>c__DisplayClass77_1 A_4)
		{
			short srcAreaId = DomainManager.Map.GetAreaIdByAreaTemplateId(MerchantType.Instance[config.MerchantType].HeadArea);
			Location srcLocation = new Location(srcAreaId, DomainManager.Map.GetElement_Areas((int)srcAreaId).SettlementInfos[0].BlockId);
			int caravanId = this.GetNextCaravanId();
			MerchantData merchantData = new MerchantData(-1, config.TemplateId);
			this.SetNextCaravanId(caravanId + 1, A_3.context);
			merchantData.Money = A_4.needMoney;
			merchantData.GenerateGoods(A_3.context, caravanId, null);
			this.AddElement_CaravanData(caravanId, merchantData, A_3.context);
			List<ValueTuple<Location, short>> path = DomainManager.Map.CalcBlockTravelRoute(A_3.context.Random, srcLocation, destLocation, true);
			path.Insert(0, new ValueTuple<Location, short>(srcLocation, 0));
			this.AddElement_CaravanDict(caravanId, this.CreateCaravanPath(path), A_3.context);
			this.CreateCaravanExtraData(A_3.context, caravanId);
			MonthlyNotificationCollection monthlyNotifications = DomainManager.World.GetMonthlyNotificationCollection();
			Settlement settlement = DomainManager.Organization.GetSettlementByLocation(srcLocation);
			monthlyNotifications.AddMerchantGoTravelling(settlement.GetId(), config.MerchantType);
			return caravanId;
		}

		// Token: 0x04001541 RID: 5441
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04001542 RID: 5442
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<int, MerchantData> _merchantData;

		// Token: 0x04001543 RID: 5443
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 7)]
		private int[] _merchantFavorability;

		// Token: 0x04001544 RID: 5444
		[DomainData(DomainDataType.SingleValue, true, false, true, true, ArrayElementsCount = 7)]
		private int[] _merchantMoney;

		// Token: 0x04001545 RID: 5445
		[DomainData(DomainDataType.ElementList, true, false, true, true, ArrayElementsCount = 7)]
		private MerchantData[] _merchantMaxLevelData;

		// Token: 0x04001546 RID: 5446
		public const sbyte MaxLevel = 6;

		// Token: 0x04001547 RID: 5447
		public const int MaxFavorability = 100;

		// Token: 0x04001548 RID: 5448
		private MerchantDomain.SkillBookTradeInfo _skillBookTradeInfo;

		// Token: 0x04001549 RID: 5449
		private readonly Dictionary<int, MerchantBuyBackData> _merchantBuyBackData = new Dictionary<int, MerchantBuyBackData>();

		// Token: 0x0400154A RID: 5450
		private readonly Dictionary<int, MerchantBuyBackData> _caravanBuyBackData = new Dictionary<int, MerchantBuyBackData>();

		// Token: 0x0400154B RID: 5451
		private readonly MerchantBuyBackData[] _merchantMaxLevelBuyBackData = new MerchantBuyBackData[7];

		// Token: 0x0400154C RID: 5452
		private readonly MerchantBuyBackData[] _branchMerchantBuyBackData = new MerchantBuyBackData[7];

		// Token: 0x0400154D RID: 5453
		private MerchantBuyBackData _tempMerchantBuyBackData;

		// Token: 0x0400154E RID: 5454
		private MerchantBuyBackData _sectStorySpecialMerchantBuyBackData;

		// Token: 0x0400154F RID: 5455
		[DomainData(DomainDataType.SingleValue, true, false, false, false)]
		private int _nextCaravanId;

		// Token: 0x04001550 RID: 5456
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<int, MerchantData> _caravanData;

		// Token: 0x04001551 RID: 5457
		[DomainData(DomainDataType.SingleValueCollection, true, false, false, false)]
		private readonly Dictionary<int, CaravanPath> _caravanDict;

		// Token: 0x04001552 RID: 5458
		private const short CaravanStayDaysInTaiwuVillage = 90;

		// Token: 0x04001553 RID: 5459
		private MerchantData _tempMerchantData;

		// Token: 0x04001554 RID: 5460
		private int _totalBuyMoney;

		// Token: 0x04001555 RID: 5461
		public const int TempCaravanId = -1;

		// Token: 0x04001556 RID: 5462
		private static readonly DataInfluence[][] CacheInfluences = new DataInfluence[7][];

		// Token: 0x04001557 RID: 5463
		private static readonly DataInfluence[][] CacheInfluencesMerchantMaxLevelData = new DataInfluence[7][];

		// Token: 0x04001558 RID: 5464
		private readonly byte[] _dataStatesMerchantMaxLevelData = new byte[2];

		// Token: 0x04001559 RID: 5465
		private Queue<uint> _pendingLoadingOperationIds;

		// Token: 0x02000A90 RID: 2704
		private class SkillBookTradeInfo
		{
			// Token: 0x06008887 RID: 34951 RVA: 0x004ECA95 File Offset: 0x004EAC95
			public SkillBookTradeInfo(int charId)
			{
				this.CharId = charId;
				this.PrivateSkillBooks = new List<ItemKey>();
				this.SectSkillBooks = new List<ItemKey>();
				this.BoughtBooksFromTaiwu = new List<ItemKey>();
				this.SoldBooksToTaiwu = new List<ItemKey>();
			}

			// Token: 0x04002B75 RID: 11125
			public int CharId;

			// Token: 0x04002B76 RID: 11126
			public List<ItemKey> PrivateSkillBooks;

			// Token: 0x04002B77 RID: 11127
			public List<ItemKey> SectSkillBooks;

			// Token: 0x04002B78 RID: 11128
			public List<ItemKey> BoughtBooksFromTaiwu;

			// Token: 0x04002B79 RID: 11129
			public List<ItemKey> SoldBooksToTaiwu;
		}
	}
}
