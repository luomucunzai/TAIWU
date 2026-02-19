using System.Collections.Generic;
using Config;

namespace GameData.Domains.Item;

public class ItemDomain_Calc_Drop
{
	private static Dictionary<short, List<short>[]>[] _categorizedDroppableItems;

	private static void InitializeCategorizedDroppableItems()
	{
		if (_categorizedDroppableItems == null)
		{
			_categorizedDroppableItems = new Dictionary<short, List<short>[]>[13];
		}
		int num = 0;
		Dictionary<short, List<short>[]>[] categorizedDroppableItems = _categorizedDroppableItems;
		int num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (WeaponItem item in (IEnumerable<WeaponItem>)Config.Weapon.Instance)
		{
			if (item.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item.ItemSubType, out var value))
				{
					value = new List<short>[9];
					_categorizedDroppableItems[num].Add(item.ItemSubType, value);
				}
				List<short>[] array = value;
				num2 = item.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value[item.ItemSubType].Add(item.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (ArmorItem item2 in (IEnumerable<ArmorItem>)Config.Armor.Instance)
		{
			if (item2.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item2.ItemSubType, out var value2))
				{
					value2 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item2.ItemSubType, value2);
				}
				List<short>[] array = value2;
				num2 = item2.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value2[item2.ItemSubType].Add(item2.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (AccessoryItem item3 in (IEnumerable<AccessoryItem>)Config.Accessory.Instance)
		{
			if (item3.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item3.ItemSubType, out var value3))
				{
					value3 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item3.ItemSubType, value3);
				}
				List<short>[] array = value3;
				num2 = item3.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value3[item3.ItemSubType].Add(item3.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (ClothingItem item4 in (IEnumerable<ClothingItem>)Config.Clothing.Instance)
		{
			if (item4.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item4.ItemSubType, out var value4))
				{
					value4 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item4.ItemSubType, value4);
				}
				List<short>[] array = value4;
				num2 = item4.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value4[item4.ItemSubType].Add(item4.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (CarrierItem item5 in (IEnumerable<CarrierItem>)Config.Carrier.Instance)
		{
			if (item5.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item5.ItemSubType, out var value5))
				{
					value5 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item5.ItemSubType, value5);
				}
				List<short>[] array = value5;
				num2 = item5.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value5[item5.ItemSubType].Add(item5.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (MaterialItem item6 in (IEnumerable<MaterialItem>)Config.Material.Instance)
		{
			if (item6.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item6.ItemSubType, out var value6))
				{
					value6 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item6.ItemSubType, value6);
				}
				List<short>[] array = value6;
				num2 = item6.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value6[item6.ItemSubType].Add(item6.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (CraftToolItem item7 in (IEnumerable<CraftToolItem>)Config.CraftTool.Instance)
		{
			if (item7.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item7.ItemSubType, out var value7))
				{
					value7 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item7.ItemSubType, value7);
				}
				List<short>[] array = value7;
				num2 = item7.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value7[item7.ItemSubType].Add(item7.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (FoodItem item8 in (IEnumerable<FoodItem>)Config.Food.Instance)
		{
			if (item8.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item8.ItemSubType, out var value8))
				{
					value8 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item8.ItemSubType, value8);
				}
				List<short>[] array = value8;
				num2 = item8.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value8[item8.ItemSubType].Add(item8.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (MedicineItem item9 in (IEnumerable<MedicineItem>)Config.Medicine.Instance)
		{
			if (item9.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item9.ItemSubType, out var value9))
				{
					value9 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item9.ItemSubType, value9);
				}
				List<short>[] array = value9;
				num2 = item9.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value9[item9.ItemSubType].Add(item9.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (TeaWineItem item10 in (IEnumerable<TeaWineItem>)Config.TeaWine.Instance)
		{
			if (item10.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item10.ItemSubType, out var value10))
				{
					value10 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item10.ItemSubType, value10);
				}
				List<short>[] array = value10;
				num2 = item10.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value10[item10.ItemSubType].Add(item10.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (SkillBookItem item11 in (IEnumerable<SkillBookItem>)Config.SkillBook.Instance)
		{
			if (item11.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item11.ItemSubType, out var value11))
				{
					value11 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item11.ItemSubType, value11);
				}
				List<short>[] array = value11;
				num2 = item11.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value11[item11.ItemSubType].Add(item11.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (CricketItem item12 in (IEnumerable<CricketItem>)Config.Cricket.Instance)
		{
			if (item12.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item12.ItemSubType, out var value12))
				{
					value12 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item12.ItemSubType, value12);
				}
				List<short>[] array = value12;
				num2 = item12.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value12[item12.ItemSubType].Add(item12.TemplateId);
			}
		}
		num++;
		categorizedDroppableItems = _categorizedDroppableItems;
		num2 = num;
		if (categorizedDroppableItems[num2] == null)
		{
			categorizedDroppableItems[num2] = new Dictionary<short, List<short>[]>();
		}
		_categorizedDroppableItems[num].Clear();
		foreach (MiscItem item13 in (IEnumerable<MiscItem>)Config.Misc.Instance)
		{
			if (item13.DropRate > 0)
			{
				if (!_categorizedDroppableItems[num].TryGetValue(item13.ItemSubType, out var value13))
				{
					value13 = new List<short>[9];
					_categorizedDroppableItems[num].Add(item13.ItemSubType, value13);
				}
				List<short>[] array = value13;
				num2 = item13.ItemSubType;
				if (array[num2] == null)
				{
					array[num2] = new List<short>();
				}
				value13[item13.ItemSubType].Add(item13.TemplateId);
			}
		}
	}
}
