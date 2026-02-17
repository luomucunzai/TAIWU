using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using Config.ConfigCells;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.Merchant
{
	// Token: 0x02000654 RID: 1620
	public static class MerchantDataHelper
	{
		// Token: 0x060048D5 RID: 18645 RVA: 0x0028FE40 File Offset: 0x0028E040
		public static void GenerateGoods(this MerchantData data, DataContext context, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
		{
			data.GenerateGoods(context, data.MerchantConfig.Level, caravanId, extraGoodsData);
		}

		// Token: 0x060048D6 RID: 18646 RVA: 0x0028FE58 File Offset: 0x0028E058
		public static void GenerateGoods(this MerchantData data, DataContext context, sbyte level, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
		{
			GameData.Domains.Character.Character character = (data.CharId >= 0) ? DomainManager.Character.GetElement_Objects(data.CharId) : null;
			int id = (character != null) ? character.GetId() : caravanId;
			bool isSpecialExtraGoodsData = extraGoodsData != null;
			bool flag = !isSpecialExtraGoodsData;
			if (flag)
			{
				DomainManager.Extra.ClearAllMerchantExtraGoods(context, id);
				bool flag2 = !DomainManager.Extra.TryGetMerchantExtraGoods(id, out extraGoodsData);
				if (flag2)
				{
					extraGoodsData = new MerchantExtraGoodsData();
				}
			}
			extraGoodsData.Clear();
			short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
			sbyte seasonTemplateId = (curAdventureId >= 0 && DomainManager.Adventure.AdventureCfg.TemplateId == 3) ? ((sbyte)context.Random.Next(Season.Instance.Count)) : EventHelper.GetCurrSeason();
			extraGoodsData.SeasonTemplateId = seasonTemplateId;
			sbyte maxGoodsLevel = Math.Min(level, 6);
			sbyte minGoodsLevel = data.GroupConfig.Level;
			for (sbyte goodsLevel = minGoodsLevel; goodsLevel <= maxGoodsLevel; goodsLevel += 1)
			{
				data.GenerateGoodsLevelList(context, extraGoodsData, goodsLevel, character, caravanId, false);
			}
			bool flag3 = !isSpecialExtraGoodsData;
			if (flag3)
			{
				DomainManager.Extra.SetMerchantExtraGoods(context, id, extraGoodsData);
			}
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x0028FF80 File Offset: 0x0028E180
		public static void RefreshSeasonExtraGoods(this MerchantData data, DataContext context, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
		{
			GameData.Domains.Character.Character character = (data.CharId >= 0) ? DomainManager.Character.GetElement_Objects(data.CharId) : null;
			int id = (character != null) ? character.GetId() : caravanId;
			bool isSpecialExtraGoodsData = extraGoodsData != null;
			bool flag = !isSpecialExtraGoodsData;
			if (flag)
			{
				bool flag2 = !DomainManager.Extra.TryGetMerchantExtraGoods(id, out extraGoodsData);
				if (flag2)
				{
					return;
				}
			}
			foreach (MerchantExtraGoodsItem merchantExtraGoodsItem in extraGoodsData.SeasonExtraGoods)
			{
				Inventory goodsList = data.GetGoodsList((int)merchantExtraGoodsItem.Index);
				foreach (KeyValuePair<ItemKey, int> keyValuePair in goodsList.Items)
				{
					ItemKey itemKey2;
					int num;
					keyValuePair.Deconstruct(out itemKey2, out num);
					ItemKey itemKey = itemKey2;
					int amount = num;
					bool flag3 = itemKey.Id == merchantExtraGoodsItem.Id;
					if (flag3)
					{
						DomainManager.Item.RemoveItem(context, itemKey);
						goodsList.OfflineRemove(itemKey, amount);
						data.PriceChangeData.Remove(itemKey);
					}
				}
			}
			extraGoodsData.SeasonExtraGoods.Clear();
			extraGoodsData.SeasonTemplateId = EventHelper.GetCurrSeason();
			sbyte maxGoodsLevel = Math.Min(data.MerchantConfig.Level, 6);
			sbyte minGoodsLevel = data.GroupConfig.Level;
			for (sbyte goodsLevel = minGoodsLevel; goodsLevel <= maxGoodsLevel; goodsLevel += 1)
			{
				data.GenerateGoodsLevelList(context, extraGoodsData, goodsLevel, character, caravanId, true);
			}
			bool flag4 = !isSpecialExtraGoodsData;
			if (flag4)
			{
				DomainManager.Extra.SetMerchantExtraGoods(context, id, extraGoodsData);
			}
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x0029014C File Offset: 0x0028E34C
		public static void GenerateGoodsLevelList(this MerchantData data, DataContext ctx, MerchantExtraGoodsData merchantExtraGoods, sbyte level, GameData.Domains.Character.Character character, int caravanId = -1, bool onlySeason = false)
		{
			MerchantDataHelper.<>c__DisplayClass3_0 CS$<>8__locals1;
			CS$<>8__locals1.ctx = ctx;
			CS$<>8__locals1.level = level;
			CS$<>8__locals1.data = data;
			CS$<>8__locals1.character = character;
			CS$<>8__locals1.goods = CS$<>8__locals1.data.GetGoodsList((int)CS$<>8__locals1.level);
			CS$<>8__locals1.itemOwnerType = ((CS$<>8__locals1.character != null) ? ItemOwnerType.Merchant : ((caravanId >= 0 || (caravanId == -1 && CS$<>8__locals1.data == DomainManager.Merchant.GetCaravanMerchantData(CS$<>8__locals1.ctx, caravanId))) ? ItemOwnerType.Caravan : ItemOwnerType.BuildingMerchant));
			GameData.Domains.Character.Character character2 = CS$<>8__locals1.character;
			CS$<>8__locals1.ownerId = ((character2 != null) ? character2.GetId() : caravanId);
			CS$<>8__locals1.createConfig = MerchantType.GetMerchantLevelData(CS$<>8__locals1.data.MerchantConfig.GroupId, CS$<>8__locals1.level);
			bool flag = CS$<>8__locals1.createConfig == null;
			if (!flag)
			{
				sbyte seasonTemplateId = merchantExtraGoods.SeasonTemplateId;
				if (!true)
				{
				}
				short[] array;
				switch (seasonTemplateId)
				{
				case 0:
					array = CS$<>8__locals1.createConfig.SeasonsGoodsRate0;
					break;
				case 1:
					array = CS$<>8__locals1.createConfig.SeasonsGoodsRate1;
					break;
				case 2:
					array = CS$<>8__locals1.createConfig.SeasonsGoodsRate2;
					break;
				case 3:
					array = CS$<>8__locals1.createConfig.SeasonsGoodsRate3;
					break;
				default:
					throw new ArgumentOutOfRangeException();
				}
				if (!true)
				{
				}
				short[] seasonGoodsRate = array;
				MerchantDataHelper.<GenerateGoodsLevelList>g__CreateRateGoods|3_0(seasonGoodsRate, merchantExtraGoods.SeasonExtraGoods, ref CS$<>8__locals1);
				if (!onlySeason)
				{
					MerchantDataHelper.<GenerateGoodsLevelList>g__CreateRateGoods|3_0(CS$<>8__locals1.createConfig.GoodsRate, null, ref CS$<>8__locals1);
					MerchantDataHelper.<GenerateGoodsLevelList>g__CreateRateGoods|3_0(CS$<>8__locals1.createConfig.CapitalistSkillExtraGoodsRate, merchantExtraGoods.CapitalistSkillExtraGoods, ref CS$<>8__locals1);
					bool flag2 = CS$<>8__locals1.level < 6;
					if (flag2)
					{
						sbyte targetLevel = Convert.ToSByte((int)(CS$<>8__locals1.level + 1));
						MerchantItem merchantConfig = MerchantType.GetMerchantLevelData(CS$<>8__locals1.createConfig.GroupId, targetLevel);
						sbyte[] array2 = CS$<>8__locals1.createConfig.ExtraGoodsIndexGroup;
						bool flag3 = array2 != null && array2.Length > 0;
						if (flag3)
						{
							sbyte normalIndex = CS$<>8__locals1.createConfig.ExtraGoodsIndexGroup.GetRandom(CS$<>8__locals1.ctx.Random);
							List<PresetItemTemplateIdGroup> normalPool = MerchantData.GetGoodsPreset(merchantConfig, (int)normalIndex).ToList<PresetItemTemplateIdGroup>();
							int normalCount = Math.Min(1, normalPool.Count);
							List<ItemKey> normalKeyList = MerchantDataHelper.<GenerateGoodsLevelList>g__CreateItem|3_1(normalPool, normalCount, false, true, ref CS$<>8__locals1);
							foreach (ItemKey key in normalKeyList)
							{
								merchantExtraGoods.NormalExtraGoods.Add(new MerchantExtraGoodsItem
								{
									Id = key.Id,
									Index = CS$<>8__locals1.level
								});
							}
							ObjectPool<List<ItemKey>>.Instance.Return(normalKeyList);
						}
						array2 = CS$<>8__locals1.createConfig.CapitalistSkillExtraGoodsIndexGroup;
						bool flag4 = array2 != null && array2.Length > 0;
						if (flag4)
						{
							sbyte professionIndex = CS$<>8__locals1.createConfig.CapitalistSkillExtraGoodsIndexGroup.GetRandom(CS$<>8__locals1.ctx.Random);
							List<PresetItemTemplateIdGroup> professionPool = MerchantData.GetGoodsPreset(merchantConfig, (int)professionIndex).ToList<PresetItemTemplateIdGroup>();
							int professionCount = Math.Min(1, professionPool.Count);
							List<ItemKey> professionKeyList = MerchantDataHelper.<GenerateGoodsLevelList>g__CreateItem|3_1(professionPool, professionCount, true, true, ref CS$<>8__locals1);
							foreach (ItemKey key2 in professionKeyList)
							{
								merchantExtraGoods.CapitalistSkillExtraGoods.Add(new MerchantExtraGoodsItem
								{
									Id = key2.Id,
									Index = CS$<>8__locals1.level
								});
							}
							ObjectPool<List<ItemKey>>.Instance.Return(professionKeyList);
						}
					}
				}
			}
		}

		// Token: 0x060048D9 RID: 18649 RVA: 0x002904E4 File Offset: 0x0028E6E4
		private static int CalculateCharacterBehaviourDiscount(this MerchantData data, DataContext ctx, ItemKey itemKey, GameData.Domains.Character.Character character)
		{
			bool flag = character == null;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				sbyte behaviorType = character.GetBehaviorType();
				int random = ctx.Random.Next(0, 100);
				bool flag2 = (random -= (int)GlobalConfig.IncreasePriceProb[(int)behaviorType]) < 0;
				if (flag2)
				{
					data.PriceChangeData[itemKey] = 25;
				}
				else
				{
					bool flag3 = random < (int)GlobalConfig.DecreasePriceProb[(int)behaviorType];
					if (!flag3)
					{
						return 0;
					}
					data.PriceChangeData[itemKey] = -25;
				}
				result = data.PriceChangeData[itemKey];
			}
			return result;
		}

		// Token: 0x060048DA RID: 18650 RVA: 0x00290570 File Offset: 0x0028E770
		public static void RemoveAllGoods(this MerchantData data, DataContext context)
		{
			for (int i = 0; i <= 6; i++)
			{
				Inventory goods = data.GetGoodsList(i);
				DomainManager.Item.RemoveItems(context, goods.Items);
				goods.Items.Clear();
			}
			data.PriceChangeData.Clear();
			Inventory buyInGoodsList = data.BuyInGoodsList;
			if (buyInGoodsList != null)
			{
				buyInGoodsList.Items.Clear();
			}
			Dictionary<ItemKey, int> buyInPrice = data.BuyInPrice;
			if (buyInPrice != null)
			{
				buyInPrice.Clear();
			}
			DomainManager.Extra.ClearAllMerchantExtraGoods(context, data.CharId);
		}

		// Token: 0x060048DB RID: 18651 RVA: 0x00290600 File Offset: 0x0028E800
		public static MerchantExtraGoodsData OfflineRefreshGoods(this MerchantData data, DataContext context, sbyte level, GameData.Domains.Character.Character character, int caravanId = -1)
		{
			MerchantDataHelper.<>c__DisplayClass6_0 CS$<>8__locals1 = new MerchantDataHelper.<>c__DisplayClass6_0();
			CS$<>8__locals1.level = level;
			MerchantExtraGoodsData newExtraGoodsData = new MerchantExtraGoodsData();
			newExtraGoodsData.SeasonTemplateId = EventHelper.GetCurrSeason();
			Dictionary<ItemKey, int> items = data.GetGoodsList((int)CS$<>8__locals1.level).Items;
			bool flag = items != null;
			if (flag)
			{
				foreach (ItemKey item in items.Keys)
				{
					data.PriceChangeData.Remove(item);
				}
				items.Clear();
			}
			data.GenerateGoodsLevelList(context, newExtraGoodsData, CS$<>8__locals1.level, character, caravanId, false);
			MerchantExtraGoodsData extraGoodsData;
			bool flag2 = DomainManager.Extra.TryGetMerchantExtraGoods((character != null) ? character.GetId() : caravanId, out extraGoodsData);
			if (flag2)
			{
				CS$<>8__locals1.<OfflineRefreshGoods>g__AssignOrAddRange|0(ref extraGoodsData.NormalExtraGoods, newExtraGoodsData.NormalExtraGoods);
				CS$<>8__locals1.<OfflineRefreshGoods>g__AssignOrAddRange|0(ref extraGoodsData.SeasonExtraGoods, newExtraGoodsData.SeasonExtraGoods);
				CS$<>8__locals1.<OfflineRefreshGoods>g__AssignOrAddRange|0(ref extraGoodsData.CapitalistSkillExtraGoods, newExtraGoodsData.CapitalistSkillExtraGoods);
			}
			return extraGoodsData ?? newExtraGoodsData;
		}

		// Token: 0x060048DC RID: 18652 RVA: 0x00290720 File Offset: 0x0028E920
		[CompilerGenerated]
		internal static void <GenerateGoodsLevelList>g__CreateRateGoods|3_0(short[] goodsRate, List<MerchantExtraGoodsItem> extraItems, ref MerchantDataHelper.<>c__DisplayClass3_0 A_2)
		{
			bool isExtra = extraItems != null;
			for (int index = 0; index < goodsRate.Length; index++)
			{
				List<PresetItemTemplateIdGroup> pool = MerchantData.GetGoodsPreset(A_2.createConfig, index).ToList<PresetItemTemplateIdGroup>();
				short rate = goodsRate[index];
				int count = (rate >= 0) ? ((int)rate * A_2.ctx.Random.Next(100, 150) / 100) : ((A_2.ctx.Random.Next(100) < (int)(-(int)rate)) ? 1 : 0);
				count = Math.Min(count, pool.Count);
				List<ItemKey> keyList = MerchantDataHelper.<GenerateGoodsLevelList>g__CreateItem|3_1(pool, count, true, isExtra, ref A_2);
				bool flag = isExtra;
				if (flag)
				{
					foreach (ItemKey key in keyList)
					{
						extraItems.Add(new MerchantExtraGoodsItem
						{
							Id = key.Id,
							Index = A_2.level
						});
					}
				}
				ObjectPool<List<ItemKey>>.Instance.Return(keyList);
			}
		}

		// Token: 0x060048DD RID: 18653 RVA: 0x00290840 File Offset: 0x0028EA40
		[CompilerGenerated]
		internal static List<ItemKey> <GenerateGoodsLevelList>g__CreateItem|3_1(IList<PresetItemTemplateIdGroup> pool, int count, bool priceEffectByBehaviour = true, bool unique = false, ref MerchantDataHelper.<>c__DisplayClass3_0 A_4)
		{
			List<ItemKey> keyList = ObjectPool<List<ItemKey>>.Instance.Get();
			for (int i = 0; i < count; i++)
			{
				int presetId = A_4.ctx.Random.Next(0, pool.Count);
				PresetItemTemplateIdGroup preset = pool[presetId];
				bool flag = ItemTemplateHelper.IsStackable(preset.ItemType, preset.StartId) && !unique;
				if (flag)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(A_4.ctx, preset.ItemType, preset.StartId);
					DomainManager.Item.SetOwner(itemKey, A_4.itemOwnerType, A_4.ownerId);
					A_4.goods.OfflineAdd(itemKey, (int)preset.GroupLength);
					if (priceEffectByBehaviour)
					{
						A_4.data.CalculateCharacterBehaviourDiscount(A_4.ctx, itemKey, A_4.character);
					}
					for (int j = 0; j < (int)preset.GroupLength; j++)
					{
						keyList.Add(itemKey);
					}
				}
				else
				{
					ItemKey firstUniqueKey = ItemKey.Invalid;
					for (int k = 0; k < (int)preset.GroupLength; k++)
					{
						ItemKey itemKey2;
						if (unique)
						{
							ItemBase itemBase = DomainManager.Item.CreateUniqueStackableItem(A_4.ctx, preset.ItemType, preset.StartId);
							byte newState = ModificationStateHelper.Activate(0, 8);
							itemBase.SetModificationState(newState, A_4.ctx);
							itemKey2 = itemBase.GetItemKey();
							if (priceEffectByBehaviour)
							{
								int price;
								bool flag2 = A_4.data.PriceChangeData.TryGetValue(firstUniqueKey, out price);
								if (flag2)
								{
									A_4.data.PriceChangeData[itemKey2] = price;
								}
								else
								{
									bool flag3 = !firstUniqueKey.IsValid();
									if (flag3)
									{
										A_4.data.CalculateCharacterBehaviourDiscount(A_4.ctx, itemKey2, A_4.character);
										bool flag4 = !firstUniqueKey.IsValid();
										if (flag4)
										{
											firstUniqueKey = itemKey2;
										}
									}
								}
							}
						}
						else
						{
							itemKey2 = DomainManager.Item.CreateItem(A_4.ctx, preset.ItemType, preset.StartId);
							if (priceEffectByBehaviour)
							{
								A_4.data.CalculateCharacterBehaviourDiscount(A_4.ctx, itemKey2, A_4.character);
							}
						}
						DomainManager.Item.SetOwner(itemKey2, A_4.itemOwnerType, A_4.ownerId);
						A_4.goods.OfflineAdd(itemKey2, 1);
						keyList.Add(itemKey2);
					}
				}
				pool.RemoveAt(presetId);
			}
			return keyList;
		}
	}
}
