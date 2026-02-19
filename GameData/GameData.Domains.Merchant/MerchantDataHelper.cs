using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using Config.ConfigCells;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.Extra;
using GameData.Domains.Item;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

public static class MerchantDataHelper
{
	public static void GenerateGoods(this MerchantData data, DataContext context, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
	{
		data.GenerateGoods(context, data.MerchantConfig.Level, caravanId, extraGoodsData);
	}

	public static void GenerateGoods(this MerchantData data, DataContext context, sbyte level, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
	{
		GameData.Domains.Character.Character character = ((data.CharId >= 0) ? DomainManager.Character.GetElement_Objects(data.CharId) : null);
		int num = character?.GetId() ?? caravanId;
		bool flag = extraGoodsData != null;
		if (!flag)
		{
			DomainManager.Extra.ClearAllMerchantExtraGoods(context, num);
			if (!DomainManager.Extra.TryGetMerchantExtraGoods(num, out extraGoodsData))
			{
				extraGoodsData = new MerchantExtraGoodsData();
			}
		}
		extraGoodsData.Clear();
		short curAdventureId = DomainManager.Adventure.GetCurAdventureId();
		sbyte seasonTemplateId = ((curAdventureId >= 0 && DomainManager.Adventure.AdventureCfg.TemplateId == 3) ? ((sbyte)context.Random.Next(Season.Instance.Count)) : EventHelper.GetCurrSeason());
		extraGoodsData.SeasonTemplateId = seasonTemplateId;
		sbyte b = Math.Min(level, 6);
		sbyte level2 = data.GroupConfig.Level;
		for (sbyte b2 = level2; b2 <= b; b2++)
		{
			data.GenerateGoodsLevelList(context, extraGoodsData, b2, character, caravanId);
		}
		if (!flag)
		{
			DomainManager.Extra.SetMerchantExtraGoods(context, num, extraGoodsData);
		}
	}

	public static void RefreshSeasonExtraGoods(this MerchantData data, DataContext context, int caravanId = -1, MerchantExtraGoodsData extraGoodsData = null)
	{
		GameData.Domains.Character.Character character = ((data.CharId >= 0) ? DomainManager.Character.GetElement_Objects(data.CharId) : null);
		int charId = character?.GetId() ?? caravanId;
		bool flag = extraGoodsData != null;
		if (!flag && !DomainManager.Extra.TryGetMerchantExtraGoods(charId, out extraGoodsData))
		{
			return;
		}
		foreach (MerchantExtraGoodsItem seasonExtraGood in extraGoodsData.SeasonExtraGoods)
		{
			Inventory goodsList = data.GetGoodsList(seasonExtraGood.Index);
			foreach (var (itemKey2, amount) in goodsList.Items)
			{
				if (itemKey2.Id == seasonExtraGood.Id)
				{
					DomainManager.Item.RemoveItem(context, itemKey2);
					goodsList.OfflineRemove(itemKey2, amount);
					data.PriceChangeData.Remove(itemKey2);
				}
			}
		}
		extraGoodsData.SeasonExtraGoods.Clear();
		extraGoodsData.SeasonTemplateId = EventHelper.GetCurrSeason();
		sbyte b = Math.Min(data.MerchantConfig.Level, 6);
		sbyte level = data.GroupConfig.Level;
		for (sbyte b2 = level; b2 <= b; b2++)
		{
			data.GenerateGoodsLevelList(context, extraGoodsData, b2, character, caravanId, onlySeason: true);
		}
		if (!flag)
		{
			DomainManager.Extra.SetMerchantExtraGoods(context, charId, extraGoodsData);
		}
	}

	public static void GenerateGoodsLevelList(this MerchantData data, DataContext ctx, MerchantExtraGoodsData merchantExtraGoods, sbyte level, GameData.Domains.Character.Character character, int caravanId = -1, bool onlySeason = false)
	{
		Inventory goods = data.GetGoodsList(level);
		ItemOwnerType itemOwnerType = ((character != null) ? ItemOwnerType.Merchant : ((caravanId >= 0 || (caravanId == -1 && data == DomainManager.Merchant.GetCaravanMerchantData(ctx, caravanId))) ? ItemOwnerType.Caravan : ItemOwnerType.BuildingMerchant));
		int ownerId = character?.GetId() ?? caravanId;
		MerchantItem createConfig = MerchantType.GetMerchantLevelData(data.MerchantConfig.GroupId, level);
		if (createConfig == null)
		{
			return;
		}
		sbyte seasonTemplateId = merchantExtraGoods.SeasonTemplateId;
		if (1 == 0)
		{
		}
		short[] array = seasonTemplateId switch
		{
			0 => createConfig.SeasonsGoodsRate0, 
			1 => createConfig.SeasonsGoodsRate1, 
			2 => createConfig.SeasonsGoodsRate2, 
			3 => createConfig.SeasonsGoodsRate3, 
			_ => throw new ArgumentOutOfRangeException(), 
		};
		if (1 == 0)
		{
		}
		short[] goodsRate = array;
		CreateRateGoods(goodsRate, merchantExtraGoods.SeasonExtraGoods);
		if (onlySeason)
		{
			return;
		}
		CreateRateGoods(createConfig.GoodsRate, null);
		CreateRateGoods(createConfig.CapitalistSkillExtraGoodsRate, merchantExtraGoods.CapitalistSkillExtraGoods);
		if (level >= 6)
		{
			return;
		}
		sbyte level2 = Convert.ToSByte(level + 1);
		MerchantItem merchantLevelData = MerchantType.GetMerchantLevelData(createConfig.GroupId, level2);
		sbyte[] extraGoodsIndexGroup = createConfig.ExtraGoodsIndexGroup;
		if (extraGoodsIndexGroup != null && extraGoodsIndexGroup.Length > 0)
		{
			sbyte random = createConfig.ExtraGoodsIndexGroup.GetRandom(ctx.Random);
			List<PresetItemTemplateIdGroup> list = MerchantData.GetGoodsPreset(merchantLevelData, random).ToList();
			int count = Math.Min(1, list.Count);
			List<ItemKey> list2 = CreateItem(list, count, priceEffectByBehaviour: false, unique: true);
			foreach (ItemKey item in list2)
			{
				merchantExtraGoods.NormalExtraGoods.Add(new MerchantExtraGoodsItem
				{
					Id = item.Id,
					Index = level
				});
			}
			ObjectPool<List<ItemKey>>.Instance.Return(list2);
		}
		extraGoodsIndexGroup = createConfig.CapitalistSkillExtraGoodsIndexGroup;
		if (extraGoodsIndexGroup == null || extraGoodsIndexGroup.Length <= 0)
		{
			return;
		}
		sbyte random2 = createConfig.CapitalistSkillExtraGoodsIndexGroup.GetRandom(ctx.Random);
		List<PresetItemTemplateIdGroup> list3 = MerchantData.GetGoodsPreset(merchantLevelData, random2).ToList();
		int count2 = Math.Min(1, list3.Count);
		List<ItemKey> list4 = CreateItem(list3, count2, priceEffectByBehaviour: true, unique: true);
		foreach (ItemKey item2 in list4)
		{
			merchantExtraGoods.CapitalistSkillExtraGoods.Add(new MerchantExtraGoodsItem
			{
				Id = item2.Id,
				Index = level
			});
		}
		ObjectPool<List<ItemKey>>.Instance.Return(list4);
		List<ItemKey> CreateItem(IList<PresetItemTemplateIdGroup> pool, int num, bool priceEffectByBehaviour = true, bool unique = false)
		{
			List<ItemKey> list5 = ObjectPool<List<ItemKey>>.Instance.Get();
			for (int i = 0; i < num; i++)
			{
				int index = ctx.Random.Next(0, pool.Count);
				PresetItemTemplateIdGroup presetItemTemplateIdGroup = pool[index];
				if (ItemTemplateHelper.IsStackable(presetItemTemplateIdGroup.ItemType, presetItemTemplateIdGroup.StartId) && !unique)
				{
					ItemKey itemKey = DomainManager.Item.CreateItem(ctx, presetItemTemplateIdGroup.ItemType, presetItemTemplateIdGroup.StartId);
					DomainManager.Item.SetOwner(itemKey, itemOwnerType, ownerId);
					goods.OfflineAdd(itemKey, presetItemTemplateIdGroup.GroupLength);
					if (priceEffectByBehaviour)
					{
						data.CalculateCharacterBehaviourDiscount(ctx, itemKey, character);
					}
					for (int j = 0; j < presetItemTemplateIdGroup.GroupLength; j++)
					{
						list5.Add(itemKey);
					}
				}
				else
				{
					ItemKey key = ItemKey.Invalid;
					for (int k = 0; k < presetItemTemplateIdGroup.GroupLength; k++)
					{
						ItemKey itemKey2;
						if (unique)
						{
							ItemBase itemBase = DomainManager.Item.CreateUniqueStackableItem(ctx, presetItemTemplateIdGroup.ItemType, presetItemTemplateIdGroup.StartId);
							byte modificationState = ModificationStateHelper.Activate(0, 8);
							itemBase.SetModificationState(modificationState, ctx);
							itemKey2 = itemBase.GetItemKey();
							if (priceEffectByBehaviour)
							{
								if (data.PriceChangeData.TryGetValue(key, out var value))
								{
									data.PriceChangeData[itemKey2] = value;
								}
								else if (!key.IsValid())
								{
									data.CalculateCharacterBehaviourDiscount(ctx, itemKey2, character);
									if (!key.IsValid())
									{
										key = itemKey2;
									}
								}
							}
						}
						else
						{
							itemKey2 = DomainManager.Item.CreateItem(ctx, presetItemTemplateIdGroup.ItemType, presetItemTemplateIdGroup.StartId);
							if (priceEffectByBehaviour)
							{
								data.CalculateCharacterBehaviourDiscount(ctx, itemKey2, character);
							}
						}
						DomainManager.Item.SetOwner(itemKey2, itemOwnerType, ownerId);
						goods.OfflineAdd(itemKey2, 1);
						list5.Add(itemKey2);
					}
				}
				pool.RemoveAt(index);
			}
			return list5;
		}
		void CreateRateGoods(short[] array2, List<MerchantExtraGoodsItem> extraItems)
		{
			bool flag = extraItems != null;
			for (int i = 0; i < array2.Length; i++)
			{
				List<PresetItemTemplateIdGroup> list5 = MerchantData.GetGoodsPreset(createConfig, i).ToList();
				short num = array2[i];
				int val = ((num >= 0) ? (num * ctx.Random.Next(100, 150) / 100) : ((ctx.Random.Next(100) < -num) ? 1 : 0));
				val = Math.Min(val, list5.Count);
				List<ItemKey> list6 = CreateItem(list5, val, priceEffectByBehaviour: true, flag);
				if (flag)
				{
					foreach (ItemKey item3 in list6)
					{
						extraItems.Add(new MerchantExtraGoodsItem
						{
							Id = item3.Id,
							Index = level
						});
					}
				}
				ObjectPool<List<ItemKey>>.Instance.Return(list6);
			}
		}
	}

	private static int CalculateCharacterBehaviourDiscount(this MerchantData data, DataContext ctx, ItemKey itemKey, GameData.Domains.Character.Character character)
	{
		if (character == null)
		{
			return 0;
		}
		sbyte behaviorType = character.GetBehaviorType();
		int num = ctx.Random.Next(0, 100);
		if ((num -= GlobalConfig.IncreasePriceProb[behaviorType]) < 0)
		{
			data.PriceChangeData[itemKey] = 25;
		}
		else
		{
			if (num >= GlobalConfig.DecreasePriceProb[behaviorType])
			{
				return 0;
			}
			data.PriceChangeData[itemKey] = -25;
		}
		return data.PriceChangeData[itemKey];
	}

	public static void RemoveAllGoods(this MerchantData data, DataContext context)
	{
		for (int i = 0; i <= 6; i++)
		{
			Inventory goodsList = data.GetGoodsList(i);
			DomainManager.Item.RemoveItems(context, goodsList.Items);
			goodsList.Items.Clear();
		}
		data.PriceChangeData.Clear();
		data.BuyInGoodsList?.Items.Clear();
		data.BuyInPrice?.Clear();
		DomainManager.Extra.ClearAllMerchantExtraGoods(context, data.CharId);
	}

	public static MerchantExtraGoodsData OfflineRefreshGoods(this MerchantData data, DataContext context, sbyte level, GameData.Domains.Character.Character character, int caravanId = -1)
	{
		MerchantExtraGoodsData merchantExtraGoodsData = new MerchantExtraGoodsData();
		merchantExtraGoodsData.SeasonTemplateId = EventHelper.GetCurrSeason();
		Dictionary<ItemKey, int> items = data.GetGoodsList(level).Items;
		if (items != null)
		{
			foreach (ItemKey key in items.Keys)
			{
				data.PriceChangeData.Remove(key);
			}
			items.Clear();
		}
		data.GenerateGoodsLevelList(context, merchantExtraGoodsData, level, character, caravanId);
		if (DomainManager.Extra.TryGetMerchantExtraGoods(character?.GetId() ?? caravanId, out var data2))
		{
			AssignOrAddRange(ref data2.NormalExtraGoods, merchantExtraGoodsData.NormalExtraGoods);
			AssignOrAddRange(ref data2.SeasonExtraGoods, merchantExtraGoodsData.SeasonExtraGoods);
			AssignOrAddRange(ref data2.CapitalistSkillExtraGoods, merchantExtraGoodsData.CapitalistSkillExtraGoods);
		}
		return data2 ?? merchantExtraGoodsData;
		void AssignOrAddRange(ref List<MerchantExtraGoodsItem> extraGoodsList, List<MerchantExtraGoodsItem> newGoodsList)
		{
			if (extraGoodsList == null)
			{
				extraGoodsList = newGoodsList;
			}
			else
			{
				extraGoodsList.RemoveAll((MerchantExtraGoodsItem x) => x.Index == level);
				extraGoodsList.AddRange(newGoodsList);
			}
		}
	}
}
