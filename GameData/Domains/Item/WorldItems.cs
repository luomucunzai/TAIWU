using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.Map;

namespace GameData.Domains.Item
{
	// Token: 0x0200065E RID: 1630
	public class WorldItems
	{
		// Token: 0x06004E80 RID: 20096 RVA: 0x002B0AC4 File Offset: 0x002AECC4
		public WorldItems()
		{
			this.CharacterItems = new Dictionary<int, HashSet<ItemKey>>();
			this.GraveItems = new Dictionary<int, HashSet<ItemKey>>();
			this.KidnapperItems = new Dictionary<int, HashSet<ItemKey>>();
			this.WarehouseItems = new HashSet<ItemKey>();
			this.JiaoPoolItems = new HashSet<ItemKey>();
			this.CricketShowroomItems = new HashSet<ItemKey>();
			this.MapBlockItems = new Dictionary<Location, HashSet<ItemKey>>();
			this.MerchantItems = new Dictionary<int, HashSet<ItemKey>>();
			this.CaravanItems = new Dictionary<int, HashSet<ItemKey>>();
			this.ShopBuildingEarnItems = new HashSet<ItemKey>();
			this.TeaHorseCaravanItems = new HashSet<ItemKey>();
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x002B0B54 File Offset: 0x002AED54
		public override string ToString()
		{
			StringBuilder text = new StringBuilder();
			text.Append("Character: ");
			text.Append(WorldItems.GetItemsCount<int>(this.CharacterItems));
			text.Append(", Grave: ");
			text.Append(WorldItems.GetItemsCount<int>(this.GraveItems));
			text.Append(", Kidnapper: ");
			text.Append(WorldItems.GetItemsCount<int>(this.KidnapperItems));
			text.Append(", Warehouse: ");
			text.Append(WorldItems.GetItemsCount(this.WarehouseItems));
			text.Append(", CricketShowroom: ");
			text.Append(WorldItems.GetItemsCount(this.CricketShowroomItems));
			text.Append(", MapBlock: ");
			text.Append(WorldItems.GetItemsCount<Location>(this.MapBlockItems));
			text.Append(", Merchant: ");
			text.Append(WorldItems.GetItemsCount<int>(this.MerchantItems));
			text.Append(", Caravan: ");
			text.Append(WorldItems.GetItemsCount<int>(this.CaravanItems));
			text.Append(", ShopBuildingEarnItems: ");
			text.Append(WorldItems.GetItemsCount(this.ShopBuildingEarnItems));
			text.Append(", TeaHorseCaravanItems: ");
			text.Append(WorldItems.GetItemsCount(this.TeaHorseCaravanItems));
			text.Append(", JiaoPool: ");
			text.Append(WorldItems.GetItemsCount(this.JiaoPoolItems));
			return text.ToString();
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x002B0CBC File Offset: 0x002AEEBC
		private static int GetItemsCount<T>(Dictionary<T, HashSet<ItemKey>> collection)
		{
			int count = 0;
			foreach (KeyValuePair<T, HashSet<ItemKey>> entry in collection)
			{
				count += entry.Value.Count;
			}
			return count;
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x002B0D1C File Offset: 0x002AEF1C
		private static int GetItemsCount(HashSet<ItemKey> collection)
		{
			return collection.Count;
		}

		// Token: 0x04001582 RID: 5506
		public readonly Dictionary<int, HashSet<ItemKey>> CharacterItems;

		// Token: 0x04001583 RID: 5507
		public readonly Dictionary<int, HashSet<ItemKey>> GraveItems;

		// Token: 0x04001584 RID: 5508
		public readonly Dictionary<int, HashSet<ItemKey>> KidnapperItems;

		// Token: 0x04001585 RID: 5509
		public readonly HashSet<ItemKey> WarehouseItems;

		// Token: 0x04001586 RID: 5510
		public readonly HashSet<ItemKey> JiaoPoolItems;

		// Token: 0x04001587 RID: 5511
		public readonly HashSet<ItemKey> CricketShowroomItems;

		// Token: 0x04001588 RID: 5512
		public readonly Dictionary<Location, HashSet<ItemKey>> MapBlockItems;

		// Token: 0x04001589 RID: 5513
		public readonly Dictionary<int, HashSet<ItemKey>> MerchantItems;

		// Token: 0x0400158A RID: 5514
		public readonly Dictionary<int, HashSet<ItemKey>> CaravanItems;

		// Token: 0x0400158B RID: 5515
		public readonly HashSet<ItemKey> ShopBuildingEarnItems;

		// Token: 0x0400158C RID: 5516
		public readonly HashSet<ItemKey> TeaHorseCaravanItems;
	}
}
