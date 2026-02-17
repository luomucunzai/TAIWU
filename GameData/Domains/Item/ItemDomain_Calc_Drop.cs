using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Item
{
	// Token: 0x0200066A RID: 1642
	public class ItemDomain_Calc_Drop
	{
		// Token: 0x060050F4 RID: 20724 RVA: 0x002C8FAC File Offset: 0x002C71AC
		private static void InitializeCategorizedDroppableItems()
		{
			if (ItemDomain_Calc_Drop._categorizedDroppableItems == null)
			{
				ItemDomain_Calc_Drop._categorizedDroppableItems = new Dictionary<short, List<short>[]>[13];
			}
			int itemType = 0;
			Dictionary<short, List<short>[]>[] categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			int num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (WeaponItem config in ((IEnumerable<WeaponItem>)Weapon.Instance))
			{
				bool flag = config.DropRate <= 0;
				if (!flag)
				{
					List<short>[] droppable;
					bool flag2 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config.ItemSubType, out droppable);
					if (flag2)
					{
						droppable = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config.ItemSubType, droppable);
					}
					List<short>[] array = droppable;
					num = (int)config.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable[(int)config.ItemSubType].Add(config.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (ArmorItem config2 in ((IEnumerable<ArmorItem>)Armor.Instance))
			{
				bool flag3 = config2.DropRate <= 0;
				if (!flag3)
				{
					List<short>[] droppable2;
					bool flag4 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config2.ItemSubType, out droppable2);
					if (flag4)
					{
						droppable2 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config2.ItemSubType, droppable2);
					}
					List<short>[] array = droppable2;
					num = (int)config2.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable2[(int)config2.ItemSubType].Add(config2.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (AccessoryItem config3 in ((IEnumerable<AccessoryItem>)Accessory.Instance))
			{
				bool flag5 = config3.DropRate <= 0;
				if (!flag5)
				{
					List<short>[] droppable3;
					bool flag6 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config3.ItemSubType, out droppable3);
					if (flag6)
					{
						droppable3 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config3.ItemSubType, droppable3);
					}
					List<short>[] array = droppable3;
					num = (int)config3.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable3[(int)config3.ItemSubType].Add(config3.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (ClothingItem config4 in ((IEnumerable<ClothingItem>)Clothing.Instance))
			{
				bool flag7 = config4.DropRate <= 0;
				if (!flag7)
				{
					List<short>[] droppable4;
					bool flag8 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config4.ItemSubType, out droppable4);
					if (flag8)
					{
						droppable4 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config4.ItemSubType, droppable4);
					}
					List<short>[] array = droppable4;
					num = (int)config4.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable4[(int)config4.ItemSubType].Add(config4.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (CarrierItem config5 in ((IEnumerable<CarrierItem>)Carrier.Instance))
			{
				bool flag9 = config5.DropRate <= 0;
				if (!flag9)
				{
					List<short>[] droppable5;
					bool flag10 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config5.ItemSubType, out droppable5);
					if (flag10)
					{
						droppable5 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config5.ItemSubType, droppable5);
					}
					List<short>[] array = droppable5;
					num = (int)config5.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable5[(int)config5.ItemSubType].Add(config5.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (MaterialItem config6 in ((IEnumerable<MaterialItem>)Material.Instance))
			{
				bool flag11 = config6.DropRate <= 0;
				if (!flag11)
				{
					List<short>[] droppable6;
					bool flag12 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config6.ItemSubType, out droppable6);
					if (flag12)
					{
						droppable6 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config6.ItemSubType, droppable6);
					}
					List<short>[] array = droppable6;
					num = (int)config6.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable6[(int)config6.ItemSubType].Add(config6.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (CraftToolItem config7 in ((IEnumerable<CraftToolItem>)CraftTool.Instance))
			{
				bool flag13 = config7.DropRate <= 0;
				if (!flag13)
				{
					List<short>[] droppable7;
					bool flag14 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config7.ItemSubType, out droppable7);
					if (flag14)
					{
						droppable7 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config7.ItemSubType, droppable7);
					}
					List<short>[] array = droppable7;
					num = (int)config7.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable7[(int)config7.ItemSubType].Add(config7.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (FoodItem config8 in ((IEnumerable<FoodItem>)Food.Instance))
			{
				bool flag15 = config8.DropRate <= 0;
				if (!flag15)
				{
					List<short>[] droppable8;
					bool flag16 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config8.ItemSubType, out droppable8);
					if (flag16)
					{
						droppable8 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config8.ItemSubType, droppable8);
					}
					List<short>[] array = droppable8;
					num = (int)config8.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable8[(int)config8.ItemSubType].Add(config8.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (MedicineItem config9 in ((IEnumerable<MedicineItem>)Medicine.Instance))
			{
				bool flag17 = config9.DropRate <= 0;
				if (!flag17)
				{
					List<short>[] droppable9;
					bool flag18 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config9.ItemSubType, out droppable9);
					if (flag18)
					{
						droppable9 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config9.ItemSubType, droppable9);
					}
					List<short>[] array = droppable9;
					num = (int)config9.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable9[(int)config9.ItemSubType].Add(config9.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (TeaWineItem config10 in ((IEnumerable<TeaWineItem>)TeaWine.Instance))
			{
				bool flag19 = config10.DropRate <= 0;
				if (!flag19)
				{
					List<short>[] droppable10;
					bool flag20 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config10.ItemSubType, out droppable10);
					if (flag20)
					{
						droppable10 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config10.ItemSubType, droppable10);
					}
					List<short>[] array = droppable10;
					num = (int)config10.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable10[(int)config10.ItemSubType].Add(config10.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (SkillBookItem config11 in ((IEnumerable<SkillBookItem>)SkillBook.Instance))
			{
				bool flag21 = config11.DropRate <= 0;
				if (!flag21)
				{
					List<short>[] droppable11;
					bool flag22 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config11.ItemSubType, out droppable11);
					if (flag22)
					{
						droppable11 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config11.ItemSubType, droppable11);
					}
					List<short>[] array = droppable11;
					num = (int)config11.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable11[(int)config11.ItemSubType].Add(config11.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (CricketItem config12 in ((IEnumerable<CricketItem>)Cricket.Instance))
			{
				bool flag23 = config12.DropRate <= 0;
				if (!flag23)
				{
					List<short>[] droppable12;
					bool flag24 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config12.ItemSubType, out droppable12);
					if (flag24)
					{
						droppable12 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config12.ItemSubType, droppable12);
					}
					List<short>[] array = droppable12;
					num = (int)config12.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable12[(int)config12.ItemSubType].Add(config12.TemplateId);
				}
			}
			itemType++;
			categorizedDroppableItems = ItemDomain_Calc_Drop._categorizedDroppableItems;
			num = itemType;
			if (categorizedDroppableItems[num] == null)
			{
				categorizedDroppableItems[num] = new Dictionary<short, List<short>[]>();
			}
			ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Clear();
			foreach (MiscItem config13 in ((IEnumerable<MiscItem>)Misc.Instance))
			{
				bool flag25 = config13.DropRate <= 0;
				if (!flag25)
				{
					List<short>[] droppable13;
					bool flag26 = !ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].TryGetValue(config13.ItemSubType, out droppable13);
					if (flag26)
					{
						droppable13 = new List<short>[9];
						ItemDomain_Calc_Drop._categorizedDroppableItems[itemType].Add(config13.ItemSubType, droppable13);
					}
					List<short>[] array = droppable13;
					num = (int)config13.ItemSubType;
					if (array[num] == null)
					{
						array[num] = new List<short>();
					}
					droppable13[(int)config13.ItemSubType].Add(config13.TemplateId);
				}
			}
		}

		// Token: 0x040015FC RID: 5628
		private static Dictionary<short, List<short>[]>[] _categorizedDroppableItems;
	}
}
