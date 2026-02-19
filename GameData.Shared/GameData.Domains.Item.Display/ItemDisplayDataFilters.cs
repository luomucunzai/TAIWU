using System;

namespace GameData.Domains.Item.Display;

public static class ItemDisplayDataFilters
{
	public static Predicate<ItemDisplayData> GetFilter(ushort filterId)
	{
		return filterId switch
		{
			1 => IsItemRepairable, 
			2 => IsItemTransferable, 
			3 => IsItemFullDurability, 
			4 => CanItemAsGift, 
			5 => IsItemTransferableOrLegendaryBook, 
			_ => null, 
		};
	}

	public static bool IsItemRepairable(ItemDisplayData itemDisplayData)
	{
		if (ItemTemplateHelper.IsRepairable(itemDisplayData.Key.ItemType, itemDisplayData.Key.TemplateId))
		{
			return itemDisplayData.Durability < itemDisplayData.MaxDurability;
		}
		return false;
	}

	public static bool IsItemTransferable(ItemDisplayData itemDisplayData)
	{
		if (!ItemTemplateHelper.IsTransferable(itemDisplayData.Key.ItemType, itemDisplayData.Key.TemplateId))
		{
			return ItemTemplateHelper.MiscResourceCanExchange(itemDisplayData.Key.ItemType, itemDisplayData.Key.TemplateId);
		}
		return true;
	}

	public static bool IsItemTransferableOrLegendaryBook(ItemDisplayData itemDisplayData)
	{
		if (!IsItemTransferable(itemDisplayData))
		{
			return ItemTemplateHelper.GetItemSubType(itemDisplayData.Key.ItemType, itemDisplayData.Key.TemplateId) == 1202;
		}
		return true;
	}

	public static bool IsItemFullDurability(ItemDisplayData itemDisplayData)
	{
		return itemDisplayData.Durability >= itemDisplayData.MaxDurability;
	}

	public static bool CanItemAsGift(ItemDisplayData itemDisplayData)
	{
		if (itemDisplayData.Key.ItemType == 11)
		{
			return true;
		}
		return IsItemFullDurability(itemDisplayData);
	}
}
