using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Merchant
{
	// Token: 0x02000653 RID: 1619
	public static class MerchantBuyBackDataHelper
	{
		// Token: 0x060048D3 RID: 18643 RVA: 0x0028FBA8 File Offset: 0x0028DDA8
		public static ItemKey TryGetGoodInSameGroup(this MerchantBuyBackData data, DataContext context, sbyte itemType, short itemTemplateId, int minGradeOffset = -9, bool requireDurability = true)
		{
			bool flag = data.BuyInGoodsList == null || data.BuyInPrice == null;
			ItemKey result;
			if (flag)
			{
				result = ItemKey.Invalid;
			}
			else
			{
				List<ItemKey> itemKeys = context.AdvanceMonthRelatedData.ItemKeys.Occupy();
				data.BuyInGoodsList.HasItemInSameGroup(itemType, itemTemplateId, minGradeOffset, itemKeys);
				if (requireDurability)
				{
					for (int i = itemKeys.Count - 1; i >= 0; i--)
					{
						ItemKey itemKey = itemKeys[i];
						ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
						bool flag2 = itemBase.GetMaxDurability() <= 0 || itemBase.GetCurrDurability() > 0;
						if (!flag2)
						{
							CollectionUtils.SwapAndRemove<ItemKey>(itemKeys, i);
						}
					}
				}
				ItemKey selectedItemKey = itemKeys.GetRandomOrDefault(context.Random, ItemKey.Invalid);
				context.AdvanceMonthRelatedData.ItemKeys.Release(ref itemKeys);
				result = selectedItemKey;
			}
			return result;
		}

		// Token: 0x060048D4 RID: 18644 RVA: 0x0028FC90 File Offset: 0x0028DE90
		public static void RemoveAllGoods(this MerchantBuyBackData data, DataContext context)
		{
			bool flag = data.BuyInGoodsList == null || data.BuyInPrice == null;
			if (!flag)
			{
				int merchantMoney = DomainManager.Merchant.GetMerchantMoney(context, data.MerchantType);
				foreach (KeyValuePair<ItemKey, int> goods in data.BuyInGoodsList.Items)
				{
					ItemKey itemKey = goods.Key;
					ItemBase itemBase = DomainManager.Item.GetBaseItem(itemKey);
					int value = DomainManager.Item.GetValue(itemKey);
					GameData.Domains.Item.Cricket cricket = itemBase as GameData.Domains.Item.Cricket;
					bool flag2 = cricket != null;
					if (flag2)
					{
						CricketPartsItem colorConfig = CricketParts.Instance[cricket.GetColorId()];
						CricketPartsItem partConfig = CricketParts.Instance[cricket.GetPartId()];
						value = ((cricket.GetPartId() > 0) ? Math.Max(colorConfig.Price, partConfig.Price) : colorConfig.Price);
					}
					short durability = itemBase.GetCurrDurability();
					short maxDurability = itemBase.GetMaxDurability();
					int durabilityRate = (int)((maxDurability == 0) ? 100 : (durability * 100 / maxDurability));
					int durabilityBidding = 50 + durabilityRate / 2;
					int money = Math.Max(0, value * 20 / 100 * durabilityBidding / 100);
					bool flag3 = money > 0;
					if (flag3)
					{
						merchantMoney += money;
					}
				}
				DomainManager.Merchant.SetMerchantMoney(context, data.MerchantType, merchantMoney);
				DomainManager.Item.RemoveItems(context, data.BuyInGoodsList.Items);
				data.BuyInGoodsList.Items.Clear();
				data.BuyInPrice.Clear();
			}
		}
	}
}
