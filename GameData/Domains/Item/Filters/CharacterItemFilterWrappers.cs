using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Character;

namespace GameData.Domains.Item.Filters
{
	// Token: 0x02000676 RID: 1654
	public static class CharacterItemFilterWrappers
	{
		// Token: 0x0600531B RID: 21275 RVA: 0x002D08B8 File Offset: 0x002CEAB8
		public static void FindByTemplateId(GameData.Domains.Character.Character character, sbyte itemType, short templateId, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			character.FindItems((ItemBase item) => ItemMatchers.MatchTemplateId(item, itemType, templateId), items, searchInventory, searchEquipment);
		}

		// Token: 0x0600531C RID: 21276 RVA: 0x002D08F4 File Offset: 0x002CEAF4
		public static void FindByTemplateIdGroup(GameData.Domains.Character.Character character, sbyte itemType, short startTemplateId, int count, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			character.FindItems((ItemBase item) => ItemMatchers.MatchTemplateIdGroup(item, itemType, startTemplateId, count), items, searchInventory, searchEquipment);
		}

		// Token: 0x0600531D RID: 21277 RVA: 0x002D0938 File Offset: 0x002CEB38
		public static void FindByItemSubType(GameData.Domains.Character.Character character, short itemSubType, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			character.FindItems((ItemBase item) => ItemMatchers.MatchItemSubType(item, itemSubType), items, searchInventory, searchEquipment);
		}

		// Token: 0x0600531E RID: 21278 RVA: 0x002D096C File Offset: 0x002CEB6C
		public static void FindByItemType(GameData.Domains.Character.Character character, sbyte itemType, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			character.FindItems((ItemBase item) => ItemMatchers.MatchItemType(item, itemType), items, searchInventory, searchEquipment);
		}

		// Token: 0x0600531F RID: 21279 RVA: 0x002D099E File Offset: 0x002CEB9E
		public static void FindEquipment(GameData.Domains.Character.Character character, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			character.FindItems(new Predicate<ItemBase>(ItemMatchers.MatchEquipment), items, searchInventory, searchEquipment);
		}

		// Token: 0x06005320 RID: 21280 RVA: 0x002D09B8 File Offset: 0x002CEBB8
		public static void FindByItemFilterRule(GameData.Domains.Character.Character character, short filterRuleTemplateId, [TupleElementNames(new string[]
		{
			"itemKey",
			"amount"
		})] List<ValueTuple<ItemKey, int>> items, bool searchInventory, bool searchEquipment)
		{
			ItemFilterRulesItem config = ItemFilterRules.Instance[filterRuleTemplateId];
			character.FindItems((ItemBase item) => ItemMatchers.MatchItemFilterRule(item, config), items, searchInventory, searchEquipment);
		}
	}
}
