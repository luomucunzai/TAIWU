using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

public static class MerchantBuyBackDataHelper
{
	public static ItemKey TryGetGoodInSameGroup(this MerchantBuyBackData data, DataContext context, sbyte itemType, short itemTemplateId, int minGradeOffset = -9, bool requireDurability = true)
	{
		if (data.BuyInGoodsList == null || data.BuyInPrice == null)
		{
			return ItemKey.Invalid;
		}
		List<ItemKey> obj = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
		data.BuyInGoodsList.HasItemInSameGroup(itemType, itemTemplateId, minGradeOffset, obj);
		if (requireDurability)
		{
			for (int num = obj.Count - 1; num >= 0; num--)
			{
				ItemKey itemKey = obj[num];
				ItemBase baseItem = DomainManager.Item.GetBaseItem(itemKey);
				if (baseItem.GetMaxDurability() > 0 && baseItem.GetCurrDurability() <= 0)
				{
					CollectionUtils.SwapAndRemove(obj, num);
				}
			}
		}
		ItemKey randomOrDefault = obj.GetRandomOrDefault(context.Random, ItemKey.Invalid);
		context.AdvanceMonthRelatedData.ItemKeys.Release(ref obj);
		return randomOrDefault;
	}

	public static void RemoveAllGoods(this MerchantBuyBackData data, DataContext context)
	{
		if (data.BuyInGoodsList == null || data.BuyInPrice == null)
		{
			return;
		}
		int num = DomainManager.Merchant.GetMerchantMoney(context, data.MerchantType);
		foreach (KeyValuePair<ItemKey, int> item in data.BuyInGoodsList.Items)
		{
			ItemKey key = item.Key;
			ItemBase baseItem = DomainManager.Item.GetBaseItem(key);
			int num2 = DomainManager.Item.GetValue(key);
			if (baseItem is GameData.Domains.Item.Cricket cricket)
			{
				CricketPartsItem cricketPartsItem = CricketParts.Instance[cricket.GetColorId()];
				CricketPartsItem cricketPartsItem2 = CricketParts.Instance[cricket.GetPartId()];
				num2 = ((cricket.GetPartId() > 0) ? Math.Max(cricketPartsItem.Price, cricketPartsItem2.Price) : cricketPartsItem.Price);
			}
			short currDurability = baseItem.GetCurrDurability();
			short maxDurability = baseItem.GetMaxDurability();
			int num3 = ((maxDurability == 0) ? 100 : (currDurability * 100 / maxDurability));
			int num4 = 50 + num3 / 2;
			int num5 = Math.Max(0, num2 * 20 / 100 * num4 / 100);
			if (num5 > 0)
			{
				num += num5;
			}
		}
		DomainManager.Merchant.SetMerchantMoney(context, data.MerchantType, num);
		DomainManager.Item.RemoveItems(context, data.BuyInGoodsList.Items);
		data.BuyInGoodsList.Items.Clear();
		data.BuyInPrice.Clear();
	}
}
