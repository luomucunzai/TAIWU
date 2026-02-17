using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Item.Filters
{
	// Token: 0x02000678 RID: 1656
	public static class WarehouseItemFilterWrappers
	{
		// Token: 0x06005328 RID: 21288 RVA: 0x002D0C6C File Offset: 0x002CEE6C
		public static void FindByTemplateId(sbyte itemType, short templateId, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items)
		{
			DomainManager.Taiwu.FindWarehouseItems((ItemBase item) => ItemMatchers.MatchTemplateId(item, itemType, templateId), items);
		}

		// Token: 0x06005329 RID: 21289 RVA: 0x002D0CA8 File Offset: 0x002CEEA8
		public static void FindByTemplateGroupId(sbyte itemType, short startTemplateId, int count, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items)
		{
			DomainManager.Taiwu.FindWarehouseItems((ItemBase item) => ItemMatchers.MatchTemplateIdGroup(item, itemType, startTemplateId, count), items);
		}

		// Token: 0x0600532A RID: 21290 RVA: 0x002D0CEC File Offset: 0x002CEEEC
		public static void FindByItemSubType(sbyte itemSubType, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items)
		{
			DomainManager.Taiwu.FindWarehouseItems((ItemBase item) => ItemMatchers.MatchItemSubType(item, (short)itemSubType), items);
		}
	}
}
