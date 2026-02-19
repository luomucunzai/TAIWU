using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai;

public class AdvanceMonthRelatedData
{
	public readonly TempListContainer<(ItemBase item, int amount)> ItemsWithAmount = new TempListContainer<(ItemBase, int)>();

	public readonly TempListContainer<TemplateKey> ItemTemplateKeys = new TempListContainer<TemplateKey>();

	public readonly TempListContainer<ItemKey> ItemKeys = new TempListContainer<ItemKey>();

	public readonly StrictTempObjectContainer<List<(ItemBase item, int amount)>[]> ClassifiedItems = new StrictTempObjectContainer<List<(ItemBase, int)>[]>(new List<(ItemBase, int)>[3]
	{
		new List<(ItemBase, int)>(),
		new List<(ItemBase, int)>(),
		new List<(ItemBase, int)>()
	}, Clear);

	public readonly TempDictionaryContainer<short, (sbyte maxGrade, int totalAmount)> ItemSubTypeStats = new TempDictionaryContainer<short, (sbyte, int)>();

	public readonly StrictTempObjectContainer<List<(GameData.Domains.Item.Medicine item, int amount)>[]> CategorizedMedicines = new StrictTempObjectContainer<List<(GameData.Domains.Item.Medicine, int)>[]>(new List<(GameData.Domains.Item.Medicine, int)>[7]
	{
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>(),
		new List<(GameData.Domains.Item.Medicine, int)>()
	}, Clear);

	public readonly TempListContainer<(GameData.Domains.Item.Misc item, int amount)> ItemsForNeili = new TempListContainer<(GameData.Domains.Item.Misc, int)>();

	public readonly StrictTempObjectContainer<List<(GameData.Domains.Item.Food item, int amount)>[]> FoodsForMainAttributes = new StrictTempObjectContainer<List<(GameData.Domains.Item.Food, int)>[]>(new List<(GameData.Domains.Item.Food, int)>[6]
	{
		new List<(GameData.Domains.Item.Food, int)>(),
		new List<(GameData.Domains.Item.Food, int)>(),
		new List<(GameData.Domains.Item.Food, int)>(),
		new List<(GameData.Domains.Item.Food, int)>(),
		new List<(GameData.Domains.Item.Food, int)>(),
		new List<(GameData.Domains.Item.Food, int)>()
	}, Clear);

	public readonly TempListContainer<(GameData.Domains.Item.TeaWine item, int amount)> TeaWinesForHappiness = new TempListContainer<(GameData.Domains.Item.TeaWine, int)>();

	public readonly TempObjectContainer<PotentialRelatedCharacters> CurrBlockCanStartRelationChars = new TempObjectContainer<PotentialRelatedCharacters>(delegate(PotentialRelatedCharacters obj)
	{
		obj.OfflineClear();
	});

	public readonly TempObjectContainer<PotentialRelatedCharacters> CurrBlockCanEndRelationChars = new TempObjectContainer<PotentialRelatedCharacters>(delegate(PotentialRelatedCharacters obj)
	{
		obj.OfflineClear();
	});

	public readonly TempListContainer<int> CharIdList = new TempListContainer<int>();

	public readonly TempListContainer<int> TargetCharIdList = new TempListContainer<int>();

	public readonly TempHashsetContainer<int> BlockCharSet = new TempHashsetContainer<int>();

	public readonly TempHashsetContainer<int> RelatedCharIds = new TempHashsetContainer<int>();

	public readonly TempHashsetContainer<int> CaringCharIds = new TempHashsetContainer<int>();

	public readonly List<ItemKey> WorldItemsToBeRemoved = new List<ItemKey>();

	public readonly StrictTempObjectContainer<List<int>[]> DemandActionTargets = new StrictTempObjectContainer<List<int>[]>(new List<int>[5]
	{
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>()
	}, Clear);

	public readonly StrictTempObjectContainer<List<int>[]> PrioritizedTargets = new StrictTempObjectContainer<List<int>[]>(new List<int>[9]
	{
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>(),
		new List<int>()
	}, Clear);

	public readonly TempListContainer<(short, short)> WeightTable = new TempListContainer<(short, short)>();

	public readonly TempListContainer<short> BlockIds = new TempListContainer<short>();

	public readonly TempListContainer<MapBlockData> Blocks = new TempListContainer<MapBlockData>();

	public readonly TempHashsetContainer<short> TemplateIdSet = new TempHashsetContainer<short>();

	public readonly TempListContainer<Wager> Wagers = new TempListContainer<Wager>();

	public readonly TempListContainer<int> IntList = new TempListContainer<int>();

	public static void Clear<T>(List<T>[] listArray)
	{
		foreach (List<T> list in listArray)
		{
			list.Clear();
		}
	}

	public unsafe void CategorizedRegenItems(IReadOnlyDictionary<ItemKey, int> inventoryItems)
	{
		List<(GameData.Domains.Item.Medicine, int)>[] array = CategorizedMedicines.Occupy();
		List<(GameData.Domains.Item.Food, int)>[] array2 = FoodsForMainAttributes.Occupy();
		List<(GameData.Domains.Item.TeaWine, int)> list = TeaWinesForHappiness.Occupy();
		List<(GameData.Domains.Item.Misc, int)> list2 = ItemsForNeili.Occupy();
		foreach (var (itemKey2, item) in inventoryItems)
		{
			switch (itemKey2.ItemType)
			{
			case 7:
			{
				FoodItem foodItem = Config.Food.Instance[itemKey2.TemplateId];
				for (sbyte b = 0; b < 6; b++)
				{
					if (foodItem.MainAttributesRegen.Items[b] > 0)
					{
						GameData.Domains.Item.Food element_Foods = DomainManager.Item.GetElement_Foods(itemKey2.Id);
						array2[b].Add((element_Foods, item));
						break;
					}
				}
				break;
			}
			case 8:
			{
				MedicineItem medicineItem = Config.Medicine.Instance[itemKey2.TemplateId];
				if (medicineItem.EffectType != EMedicineEffectType.Invalid)
				{
					GameData.Domains.Item.Medicine element_Medicines = DomainManager.Item.GetElement_Medicines(itemKey2.Id);
					array[(int)medicineItem.EffectType].Add((element_Medicines, item));
				}
				break;
			}
			case 9:
				if (DomainManager.Item.GetBaseItem(itemKey2).GetHappinessChange() > 0)
				{
					GameData.Domains.Item.TeaWine element_TeaWines = DomainManager.Item.GetElement_TeaWines(itemKey2.Id);
					list.Add((element_TeaWines, item));
				}
				break;
			case 12:
			{
				MiscItem miscItem = Config.Misc.Instance[itemKey2.TemplateId];
				if (miscItem.Neili > 0)
				{
					GameData.Domains.Item.Misc element_Misc = DomainManager.Item.GetElement_Misc(itemKey2.Id);
					list2.Add((element_Misc, item));
				}
				break;
			}
			}
		}
	}

	public void ReleaseCategorizedRegenItems()
	{
		QuickRelease(CategorizedMedicines);
		QuickRelease(FoodsForMainAttributes);
		QuickRelease(TeaWinesForHappiness);
		QuickRelease(ItemsForNeili);
	}

	public void SummarizeItemSubTypeStats(Dictionary<ItemKey, int> inventoryItems, ItemKey[] equipmentItems)
	{
		Dictionary<short, (sbyte, int)> dictionary = ItemSubTypeStats.Occupy();
		dictionary.Clear();
		foreach (KeyValuePair<ItemKey, int> inventoryItem in inventoryItems)
		{
			ItemKey key = inventoryItem.Key;
			int num = inventoryItem.Value;
			short itemSubType = ItemTemplateHelper.GetItemSubType(key.ItemType, key.TemplateId);
			sbyte b = ItemTemplateHelper.GetGrade(key.ItemType, key.TemplateId);
			if (dictionary.TryGetValue(itemSubType, out var value))
			{
				b = Math.Max(value.Item1, b);
				num = value.Item2 + num;
			}
			dictionary[itemSubType] = (b, num);
		}
		for (int i = 0; i < equipmentItems.Length; i++)
		{
			ItemKey itemKey = equipmentItems[i];
			if (itemKey.IsValid())
			{
				int num2 = 1;
				short itemSubType2 = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
				sbyte b2 = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				if (dictionary.TryGetValue(itemSubType2, out var value2))
				{
					b2 = Math.Max(value2.Item1, b2);
					num2 = value2.Item2 + num2;
				}
				dictionary[itemSubType2] = (b2, num2);
			}
		}
	}

	public void ReleaseItemSubTypeStats()
	{
		QuickRelease(ItemSubTypeStats);
	}

	private static void QuickRelease<T>(TempObjectContainerBase<T> container) where T : class
	{
		T obj = container.Get();
		container.Release(ref obj);
	}
}
