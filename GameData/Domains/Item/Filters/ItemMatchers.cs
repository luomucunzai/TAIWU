using System;
using System.Collections.Generic;
using Config;
using Config.ConfigCells;

namespace GameData.Domains.Item.Filters
{
	// Token: 0x02000677 RID: 1655
	public static class ItemMatchers
	{
		// Token: 0x06005321 RID: 21281 RVA: 0x002D09F4 File Offset: 0x002CEBF4
		public static bool MatchAll(ItemBase item, List<Predicate<ItemBase>> predicates)
		{
			int i = 0;
			int count = predicates.Count;
			while (i < count)
			{
				bool flag = !predicates[i](item);
				if (flag)
				{
					return false;
				}
				i++;
			}
			return true;
		}

		// Token: 0x06005322 RID: 21282 RVA: 0x002D0A38 File Offset: 0x002CEC38
		public static bool MatchItemType(ItemBase item, sbyte itemType)
		{
			return item.GetItemType() == itemType;
		}

		// Token: 0x06005323 RID: 21283 RVA: 0x002D0A54 File Offset: 0x002CEC54
		public static bool MatchEquipment(ItemBase item)
		{
			return ItemType.IsEquipmentItemType(item.GetItemType());
		}

		// Token: 0x06005324 RID: 21284 RVA: 0x002D0A74 File Offset: 0x002CEC74
		public static bool MatchTemplateId(ItemBase item, sbyte itemType, short templateId)
		{
			return item.GetItemType() == itemType && item.GetTemplateId() == templateId;
		}

		// Token: 0x06005325 RID: 21285 RVA: 0x002D0A9C File Offset: 0x002CEC9C
		public static bool MatchTemplateIdGroup(ItemBase item, sbyte itemType, short startTemplateId, int count)
		{
			bool flag = item.GetItemType() != itemType;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				short templateId = item.GetTemplateId();
				result = (templateId >= startTemplateId && (int)templateId < (int)startTemplateId + count);
			}
			return result;
		}

		// Token: 0x06005326 RID: 21286 RVA: 0x002D0AD8 File Offset: 0x002CECD8
		public static bool MatchItemSubType(ItemBase item, short itemSubType)
		{
			return item.GetItemSubType() == itemSubType;
		}

		// Token: 0x06005327 RID: 21287 RVA: 0x002D0AF4 File Offset: 0x002CECF4
		public static bool MatchItemFilterRule(ItemBase itemBase, ItemFilterRulesItem rule)
		{
			bool flag = rule.AppointId.TemplateId != -1;
			bool result;
			if (flag)
			{
				result = (itemBase.GetItemType() == rule.AppointId.ItemType && itemBase.GetTemplateId() == rule.AppointId.TemplateId);
			}
			else
			{
				List<PresetItemSubTypeWithGradeRange> appointOrSubTypeCore = rule.AppointOrSubTypeCore;
				bool flag2 = appointOrSubTypeCore != null && appointOrSubTypeCore.Count > 0;
				if (flag2)
				{
					sbyte grade = itemBase.GetGrade();
					foreach (PresetItemSubTypeWithGradeRange itemSubTypeWithGrade in rule.AppointOrSubTypeCore)
					{
						bool flag3 = itemBase.GetItemSubType() == itemSubTypeWithGrade.SubType && grade >= itemSubTypeWithGrade.GradeMin && grade <= itemSubTypeWithGrade.GradeMax;
						if (flag3)
						{
							return true;
						}
					}
				}
				List<PresetItemTemplateIdGroup> appointOrIdCore = rule.AppointOrIdCore;
				bool flag4 = appointOrIdCore != null && appointOrIdCore.Count > 0;
				if (flag4)
				{
					foreach (PresetItemTemplateIdGroup coreCell in rule.AppointOrIdCore)
					{
						bool flag5 = ItemMatchers.MatchTemplateIdGroup(itemBase, coreCell.ItemType, coreCell.StartId, (int)coreCell.GroupLength);
						if (flag5)
						{
							return true;
						}
					}
				}
				result = false;
			}
			return result;
		}
	}
}
