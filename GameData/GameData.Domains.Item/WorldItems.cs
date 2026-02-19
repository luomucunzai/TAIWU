using System.Collections.Generic;
using System.Text;
using GameData.Domains.Map;

namespace GameData.Domains.Item;

public class WorldItems
{
	public readonly Dictionary<int, HashSet<ItemKey>> CharacterItems;

	public readonly Dictionary<int, HashSet<ItemKey>> GraveItems;

	public readonly Dictionary<int, HashSet<ItemKey>> KidnapperItems;

	public readonly HashSet<ItemKey> WarehouseItems;

	public readonly HashSet<ItemKey> JiaoPoolItems;

	public readonly HashSet<ItemKey> CricketShowroomItems;

	public readonly Dictionary<Location, HashSet<ItemKey>> MapBlockItems;

	public readonly Dictionary<int, HashSet<ItemKey>> MerchantItems;

	public readonly Dictionary<int, HashSet<ItemKey>> CaravanItems;

	public readonly HashSet<ItemKey> ShopBuildingEarnItems;

	public readonly HashSet<ItemKey> TeaHorseCaravanItems;

	public WorldItems()
	{
		CharacterItems = new Dictionary<int, HashSet<ItemKey>>();
		GraveItems = new Dictionary<int, HashSet<ItemKey>>();
		KidnapperItems = new Dictionary<int, HashSet<ItemKey>>();
		WarehouseItems = new HashSet<ItemKey>();
		JiaoPoolItems = new HashSet<ItemKey>();
		CricketShowroomItems = new HashSet<ItemKey>();
		MapBlockItems = new Dictionary<Location, HashSet<ItemKey>>();
		MerchantItems = new Dictionary<int, HashSet<ItemKey>>();
		CaravanItems = new Dictionary<int, HashSet<ItemKey>>();
		ShopBuildingEarnItems = new HashSet<ItemKey>();
		TeaHorseCaravanItems = new HashSet<ItemKey>();
	}

	public override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("Character: ");
		stringBuilder.Append(GetItemsCount(CharacterItems));
		stringBuilder.Append(", Grave: ");
		stringBuilder.Append(GetItemsCount(GraveItems));
		stringBuilder.Append(", Kidnapper: ");
		stringBuilder.Append(GetItemsCount(KidnapperItems));
		stringBuilder.Append(", Warehouse: ");
		stringBuilder.Append(GetItemsCount(WarehouseItems));
		stringBuilder.Append(", CricketShowroom: ");
		stringBuilder.Append(GetItemsCount(CricketShowroomItems));
		stringBuilder.Append(", MapBlock: ");
		stringBuilder.Append(GetItemsCount(MapBlockItems));
		stringBuilder.Append(", Merchant: ");
		stringBuilder.Append(GetItemsCount(MerchantItems));
		stringBuilder.Append(", Caravan: ");
		stringBuilder.Append(GetItemsCount(CaravanItems));
		stringBuilder.Append(", ShopBuildingEarnItems: ");
		stringBuilder.Append(GetItemsCount(ShopBuildingEarnItems));
		stringBuilder.Append(", TeaHorseCaravanItems: ");
		stringBuilder.Append(GetItemsCount(TeaHorseCaravanItems));
		stringBuilder.Append(", JiaoPool: ");
		stringBuilder.Append(GetItemsCount(JiaoPoolItems));
		return stringBuilder.ToString();
	}

	private static int GetItemsCount<T>(Dictionary<T, HashSet<ItemKey>> collection)
	{
		int num = 0;
		foreach (KeyValuePair<T, HashSet<ItemKey>> item in collection)
		{
			num += item.Value.Count;
		}
		return num;
	}

	private static int GetItemsCount(HashSet<ItemKey> collection)
	{
		return collection.Count;
	}
}
