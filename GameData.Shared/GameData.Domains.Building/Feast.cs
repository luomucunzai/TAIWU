using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Item;
using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class Feast : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort BuildingBlockKey = 0;

		public const ushort Dish = 1;

		public const ushort DishDurability = 2;

		public const ushort Gift = 3;

		public const ushort GiftCount = 4;

		public const ushort AutoRefill = 5;

		public const ushort TargetType = 6;

		public const ushort Count = 7;

		public static readonly string[] FieldId2FieldName = new string[7] { "BuildingBlockKey", "Dish", "DishDurability", "Gift", "GiftCount", "AutoRefill", "TargetType" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public BuildingBlockKey BuildingBlockKey;

	[SerializableGameDataField(FieldIndex = 1)]
	public Dictionary<int, ItemKey> Dish;

	[SerializableGameDataField(FieldIndex = 2)]
	public Dictionary<int, int> DishDurability;

	[SerializableGameDataField(FieldIndex = 3)]
	public Dictionary<int, ItemKey> Gift;

	[SerializableGameDataField(FieldIndex = 4)]
	public Dictionary<int, int> GiftCount;

	[SerializableGameDataField(FieldIndex = 5)]
	public bool AutoRefill;

	[SerializableGameDataField(FieldIndex = 6)]
	public short TargetType;

	public bool IsFull => GetInUseDishSlotCount() >= GlobalConfig.Instance.FeastCount;

	public Feast()
	{
		BuildingBlockKey = BuildingBlockKey.Invalid;
		Dish = new Dictionary<int, ItemKey>();
		DishDurability = new Dictionary<int, int>();
		Gift = new Dictionary<int, ItemKey>();
		GiftCount = new Dictionary<int, int>();
		AutoRefill = false;
		TargetType = -1;
	}

	public Feast(BuildingBlockKey key)
	{
		BuildingBlockKey = key;
		Dish = new Dictionary<int, ItemKey>();
		DishDurability = new Dictionary<int, int>();
		Gift = new Dictionary<int, ItemKey>();
		GiftCount = new Dictionary<int, int>();
		AutoRefill = false;
		TargetType = -1;
	}

	public ItemKey GetDish(int index)
	{
		if (!Dish.TryGetValue(index, out var value))
		{
			return ItemKey.Invalid;
		}
		return value;
	}

	public ItemKey GetGift(int index)
	{
		if (!Gift.TryGetValue(index, out var value))
		{
			return ItemKey.Invalid;
		}
		return value;
	}

	public int GetInUseDishSlotCount()
	{
		int num = 0;
		foreach (ItemKey value in Dish.Values)
		{
			if (value.IsValid())
			{
				num++;
			}
		}
		return num;
	}

	public int GetAvailableDishSlot()
	{
		for (int i = 0; i < GlobalConfig.Instance.FeastCount; i++)
		{
			if (!GetDish(i).IsValid())
			{
				return i;
			}
		}
		return -1;
	}

	public bool IsDishEaten(int index)
	{
		return DishDurability[index] != GlobalConfig.Instance.FeastDurability;
	}

	public int GetInUseGiftSlotCount()
	{
		int num = 0;
		foreach (ItemKey value in Gift.Values)
		{
			if (value != ItemKey.Invalid)
			{
				num++;
			}
		}
		return num;
	}

	public bool CheckAvoidAutoCheckIn()
	{
		if (AutoRefill)
		{
			return GetInUseDishSlotCount() <= 0;
		}
		return false;
	}

	public short GetFeastType()
	{
		int feastCount = GlobalConfig.Instance.FeastCount;
		FeastItem feastItem = Config.Feast.DefValue.None;
		if (Dish.Count < feastCount)
		{
			return feastItem.TemplateId;
		}
		Dictionary<EFoodFoodType, int> dictionary = new Dictionary<EFoodFoodType, int>();
		Dictionary<short, int> dictionary2 = new Dictionary<short, int>
		{
			{ 701, 0 },
			{ 700, 0 },
			{ 900, 0 },
			{ 901, 0 }
		};
		sbyte b = sbyte.MaxValue;
		foreach (object value in Enum.GetValues(typeof(EFoodFoodType)))
		{
			dictionary.Add((EFoodFoodType)value, 0);
		}
		foreach (ItemKey value2 in Dish.Values)
		{
			if (!value2.IsValid())
			{
				return feastItem.TemplateId;
			}
			b = Math.Min(b, ItemTemplateHelper.GetGrade(value2.ItemType, value2.TemplateId));
			switch (ItemTemplateHelper.GetItemSubType(value2.ItemType, value2.TemplateId))
			{
			case 900:
				dictionary[EFoodFoodType.Tea]++;
				dictionary2[900]++;
				continue;
			case 901:
				dictionary[EFoodFoodType.Wine]++;
				dictionary2[901]++;
				continue;
			}
			FoodItem foodItem = Food.Instance[value2.TemplateId];
			if (foodItem.FoodType == null)
			{
				continue;
			}
			foreach (EFoodFoodType item in foodItem.FoodType)
			{
				dictionary[item]++;
				dictionary2[foodItem.ItemSubType]++;
			}
		}
		foreach (FeastItem item2 in (IEnumerable<FeastItem>)Config.Feast.Instance)
		{
			if (Check(dictionary, dictionary2, item2, feastItem.Priority, b))
			{
				feastItem = item2;
			}
		}
		return feastItem.TemplateId;
	}

	private bool Check(Dictionary<EFoodFoodType, int> countByFoodType, Dictionary<short, int> countBySubType, FeastItem config, int currPriority, int lowestGrade)
	{
		if (currPriority >= config.Priority)
		{
			return false;
		}
		if (config.RequirementType == null || config.RequirementType.Count == 0)
		{
			return false;
		}
		for (int i = 0; i < config.RequirementType.Count; i++)
		{
			EFeastRequirementType eFeastRequirementType = config.RequirementType[i];
			int[] array = config.RequirementData[i];
			switch (eFeastRequirementType)
			{
			case EFeastRequirementType.FoodTypeBird:
				if (countByFoodType[EFoodFoodType.Bird] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.FoodTypeBeast:
				if (countByFoodType[EFoodFoodType.Beast] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.FoodTypeFish:
				if (countByFoodType[EFoodFoodType.Fish] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.FoodTypeVegetable:
				if (countByFoodType[EFoodFoodType.Vegetable] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.FoodTypeFruit:
				if (countByFoodType[EFoodFoodType.Fruit] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.FoodTypeVegetarian:
				if (countByFoodType[EFoodFoodType.Vegetarian] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.SubTypeTea:
				if (countBySubType[900] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.SubTypeWine:
				if (countBySubType[901] < array[0])
				{
					return false;
				}
				break;
			case EFeastRequirementType.SubTypeDiff:
			{
				int num = 0;
				foreach (KeyValuePair<short, int> item in countBySubType)
				{
					item.Deconstruct(out var _, out var value);
					if (value > 0)
					{
						num++;
					}
				}
				if (num < array[0])
				{
					return false;
				}
				break;
			}
			}
			if (lowestGrade < array[1])
			{
				return false;
			}
		}
		return true;
	}

	public static List<EFoodFoodType> GetFoodTypeList(ItemKey itemKey, List<EFoodFoodType> typeList = null)
	{
		if (typeList == null)
		{
			typeList = new List<EFoodFoodType>();
		}
		typeList.Clear();
		switch (ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId))
		{
		case 900:
			typeList.Add(EFoodFoodType.Tea);
			break;
		case 901:
			typeList.Add(EFoodFoodType.Wine);
			break;
		default:
		{
			FoodItem foodItem = Food.Instance[itemKey.TemplateId];
			typeList.AddRange(foodItem.FoodType);
			break;
		}
		}
		return typeList;
	}

	public static EFoodFoodType GetFoodType(EFeastRequirementType requirementType)
	{
		return requirementType switch
		{
			EFeastRequirementType.Invalid => EFoodFoodType.Invalid, 
			EFeastRequirementType.FoodTypeBird => EFoodFoodType.Bird, 
			EFeastRequirementType.FoodTypeBeast => EFoodFoodType.Beast, 
			EFeastRequirementType.FoodTypeFish => EFoodFoodType.Fish, 
			EFeastRequirementType.FoodTypeVegetable => EFoodFoodType.Vegetable, 
			EFeastRequirementType.FoodTypeFruit => EFoodFoodType.Fruit, 
			EFeastRequirementType.FoodTypeVegetarian => EFoodFoodType.Vegetarian, 
			EFeastRequirementType.SubTypeTea => EFoodFoodType.Tea, 
			EFeastRequirementType.SubTypeWine => EFoodFoodType.Wine, 
			EFeastRequirementType.SubTypeDiff => EFoodFoodType.Invalid, 
			EFeastRequirementType.Count => EFoodFoodType.Invalid, 
			_ => throw new ArgumentOutOfRangeException("requirementType", requirementType, null), 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 5;
		num += BuildingBlockKey.GetSerializedSize();
		num += 4;
		if (Dish != null)
		{
			foreach (KeyValuePair<int, ItemKey> item in Dish)
			{
				num += 4;
				num += item.Value.GetSerializedSize();
			}
		}
		num += 4;
		if (DishDurability != null)
		{
			foreach (KeyValuePair<int, int> item2 in DishDurability)
			{
				_ = item2;
				num += 4;
				num += 4;
			}
		}
		num += 4;
		if (Gift != null)
		{
			foreach (KeyValuePair<int, ItemKey> item3 in Gift)
			{
				num += 4;
				num += item3.Value.GetSerializedSize();
			}
		}
		num += 4;
		if (GiftCount != null)
		{
			foreach (KeyValuePair<int, int> item4 in GiftCount)
			{
				_ = item4;
				num += 4;
				num += 4;
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 7;
		ptr += 2;
		ptr += BuildingBlockKey.Serialize(ptr);
		if (Dish != null)
		{
			*(int*)ptr = Dish.Count;
			ptr += 4;
			foreach (KeyValuePair<int, ItemKey> item in Dish)
			{
				*(int*)ptr = item.Key;
				ptr += 4;
				ptr += item.Value.Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (DishDurability != null)
		{
			*(int*)ptr = DishDurability.Count;
			ptr += 4;
			foreach (KeyValuePair<int, int> item2 in DishDurability)
			{
				*(int*)ptr = item2.Key;
				ptr += 4;
				*(int*)ptr = item2.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (Gift != null)
		{
			*(int*)ptr = Gift.Count;
			ptr += 4;
			foreach (KeyValuePair<int, ItemKey> item3 in Gift)
			{
				*(int*)ptr = item3.Key;
				ptr += 4;
				ptr += item3.Value.Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (GiftCount != null)
		{
			*(int*)ptr = GiftCount.Count;
			ptr += 4;
			foreach (KeyValuePair<int, int> item4 in GiftCount)
			{
				*(int*)ptr = item4.Key;
				ptr += 4;
				*(int*)ptr = item4.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		*ptr = (AutoRefill ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = TargetType;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += BuildingBlockKey.Deserialize(ptr);
		}
		if (num > 1)
		{
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				if (Dish == null)
				{
					Dish = new Dictionary<int, ItemKey>();
				}
				else
				{
					Dish.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					int key = *(int*)ptr;
					ptr += 4;
					ItemKey value = default(ItemKey);
					ptr += value.Deserialize(ptr);
					Dish.Add(key, value);
				}
			}
			else
			{
				Dish?.Clear();
			}
		}
		if (num > 2)
		{
			int num3 = *(int*)ptr;
			ptr += 4;
			if (num3 > 0)
			{
				if (DishDurability == null)
				{
					DishDurability = new Dictionary<int, int>();
				}
				else
				{
					DishDurability.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					int key2 = *(int*)ptr;
					ptr += 4;
					int value2 = *(int*)ptr;
					ptr += 4;
					DishDurability.Add(key2, value2);
				}
			}
			else
			{
				DishDurability?.Clear();
			}
		}
		if (num > 3)
		{
			int num4 = *(int*)ptr;
			ptr += 4;
			if (num4 > 0)
			{
				if (Gift == null)
				{
					Gift = new Dictionary<int, ItemKey>();
				}
				else
				{
					Gift.Clear();
				}
				for (int k = 0; k < num4; k++)
				{
					int key3 = *(int*)ptr;
					ptr += 4;
					ItemKey value3 = default(ItemKey);
					ptr += value3.Deserialize(ptr);
					Gift.Add(key3, value3);
				}
			}
			else
			{
				Gift?.Clear();
			}
		}
		if (num > 4)
		{
			int num5 = *(int*)ptr;
			ptr += 4;
			if (num5 > 0)
			{
				if (GiftCount == null)
				{
					GiftCount = new Dictionary<int, int>();
				}
				else
				{
					GiftCount.Clear();
				}
				for (int l = 0; l < num5; l++)
				{
					int key4 = *(int*)ptr;
					ptr += 4;
					int value4 = *(int*)ptr;
					ptr += 4;
					GiftCount.Add(key4, value4);
				}
			}
			else
			{
				GiftCount?.Clear();
			}
		}
		if (num > 5)
		{
			AutoRefill = *ptr != 0;
			ptr++;
		}
		if (num > 6)
		{
			TargetType = *(short*)ptr;
			ptr += 2;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
