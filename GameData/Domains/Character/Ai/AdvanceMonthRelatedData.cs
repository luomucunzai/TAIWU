using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000845 RID: 2117
	public class AdvanceMonthRelatedData
	{
		// Token: 0x06007602 RID: 30210 RVA: 0x0044EA3C File Offset: 0x0044CC3C
		public static void Clear<T>(List<T>[] listArray)
		{
			foreach (List<T> list in listArray)
			{
				list.Clear();
			}
		}

		// Token: 0x06007603 RID: 30211 RVA: 0x0044EA6C File Offset: 0x0044CC6C
		public unsafe void CategorizedRegenItems(IReadOnlyDictionary<ItemKey, int> inventoryItems)
		{
			List<ValueTuple<GameData.Domains.Item.Medicine, int>>[] categorizedRegenItems = this.CategorizedMedicines.Occupy();
			List<ValueTuple<GameData.Domains.Item.Food, int>>[] foods = this.FoodsForMainAttributes.Occupy();
			List<ValueTuple<GameData.Domains.Item.TeaWine, int>> teaWines = this.TeaWinesForHappiness.Occupy();
			List<ValueTuple<GameData.Domains.Item.Misc, int>> itemsForNeili = this.ItemsForNeili.Occupy();
			foreach (KeyValuePair<ItemKey, int> keyValuePair in inventoryItems)
			{
				ItemKey itemKey2;
				int num;
				keyValuePair.Deconstruct(out itemKey2, out num);
				ItemKey itemKey = itemKey2;
				int amount = num;
				switch (itemKey.ItemType)
				{
				case 7:
				{
					FoodItem config = Config.Food.Instance[itemKey.TemplateId];
					for (sbyte attrType = 0; attrType < 6; attrType += 1)
					{
						bool flag = *(ref config.MainAttributesRegen.Items.FixedElementField + (IntPtr)attrType * 2) <= 0;
						if (!flag)
						{
							GameData.Domains.Item.Food item = DomainManager.Item.GetElement_Foods(itemKey.Id);
							foods[(int)attrType].Add(new ValueTuple<GameData.Domains.Item.Food, int>(item, amount));
							break;
						}
					}
					break;
				}
				case 8:
				{
					MedicineItem config2 = Config.Medicine.Instance[itemKey.TemplateId];
					bool flag2 = config2.EffectType == EMedicineEffectType.Invalid;
					if (!flag2)
					{
						GameData.Domains.Item.Medicine item2 = DomainManager.Item.GetElement_Medicines(itemKey.Id);
						categorizedRegenItems[(int)config2.EffectType].Add(new ValueTuple<GameData.Domains.Item.Medicine, int>(item2, amount));
					}
					break;
				}
				case 9:
				{
					bool flag3 = DomainManager.Item.GetBaseItem(itemKey).GetHappinessChange() <= 0;
					if (!flag3)
					{
						GameData.Domains.Item.TeaWine item3 = DomainManager.Item.GetElement_TeaWines(itemKey.Id);
						teaWines.Add(new ValueTuple<GameData.Domains.Item.TeaWine, int>(item3, amount));
					}
					break;
				}
				case 12:
				{
					MiscItem config3 = Config.Misc.Instance[itemKey.TemplateId];
					bool flag4 = config3.Neili <= 0;
					if (!flag4)
					{
						GameData.Domains.Item.Misc item4 = DomainManager.Item.GetElement_Misc(itemKey.Id);
						itemsForNeili.Add(new ValueTuple<GameData.Domains.Item.Misc, int>(item4, amount));
					}
					break;
				}
				}
			}
		}

		// Token: 0x06007604 RID: 30212 RVA: 0x0044ECAC File Offset: 0x0044CEAC
		public void ReleaseCategorizedRegenItems()
		{
			AdvanceMonthRelatedData.QuickRelease<List<ValueTuple<GameData.Domains.Item.Medicine, int>>[]>(this.CategorizedMedicines);
			AdvanceMonthRelatedData.QuickRelease<List<ValueTuple<GameData.Domains.Item.Food, int>>[]>(this.FoodsForMainAttributes);
			AdvanceMonthRelatedData.QuickRelease<List<ValueTuple<GameData.Domains.Item.TeaWine, int>>>(this.TeaWinesForHappiness);
			AdvanceMonthRelatedData.QuickRelease<List<ValueTuple<GameData.Domains.Item.Misc, int>>>(this.ItemsForNeili);
		}

		// Token: 0x06007605 RID: 30213 RVA: 0x0044ECE0 File Offset: 0x0044CEE0
		public void SummarizeItemSubTypeStats(Dictionary<ItemKey, int> inventoryItems, ItemKey[] equipmentItems)
		{
			Dictionary<short, ValueTuple<sbyte, int>> itemSubTypeStats = this.ItemSubTypeStats.Occupy();
			itemSubTypeStats.Clear();
			foreach (KeyValuePair<ItemKey, int> pair in inventoryItems)
			{
				ItemKey itemKey = pair.Key;
				int amount = pair.Value;
				short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
				sbyte itemGrade = ItemTemplateHelper.GetGrade(itemKey.ItemType, itemKey.TemplateId);
				ValueTuple<sbyte, int> itemInfo;
				bool flag = itemSubTypeStats.TryGetValue(itemSubType, out itemInfo);
				if (flag)
				{
					itemGrade = Math.Max(itemInfo.Item1, itemGrade);
					amount = itemInfo.Item2 + amount;
				}
				itemSubTypeStats[itemSubType] = new ValueTuple<sbyte, int>(itemGrade, amount);
			}
			foreach (ItemKey itemKey2 in equipmentItems)
			{
				bool flag2 = !itemKey2.IsValid();
				if (!flag2)
				{
					int amount2 = 1;
					short itemSubType2 = ItemTemplateHelper.GetItemSubType(itemKey2.ItemType, itemKey2.TemplateId);
					sbyte itemGrade2 = ItemTemplateHelper.GetGrade(itemKey2.ItemType, itemKey2.TemplateId);
					ValueTuple<sbyte, int> itemInfo2;
					bool flag3 = itemSubTypeStats.TryGetValue(itemSubType2, out itemInfo2);
					if (flag3)
					{
						itemGrade2 = Math.Max(itemInfo2.Item1, itemGrade2);
						amount2 = itemInfo2.Item2 + amount2;
					}
					itemSubTypeStats[itemSubType2] = new ValueTuple<sbyte, int>(itemGrade2, amount2);
				}
			}
		}

		// Token: 0x06007606 RID: 30214 RVA: 0x0044EE68 File Offset: 0x0044D068
		public void ReleaseItemSubTypeStats()
		{
			AdvanceMonthRelatedData.QuickRelease<Dictionary<short, ValueTuple<sbyte, int>>>(this.ItemSubTypeStats);
		}

		// Token: 0x06007607 RID: 30215 RVA: 0x0044EE78 File Offset: 0x0044D078
		private static void QuickRelease<T>(TempObjectContainerBase<T> container) where T : class
		{
			T obj = container.Get();
			container.Release(ref obj);
		}

		// Token: 0x04002063 RID: 8291
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly TempListContainer<ValueTuple<ItemBase, int>> ItemsWithAmount = new TempListContainer<ValueTuple<ItemBase, int>>();

		// Token: 0x04002064 RID: 8292
		public readonly TempListContainer<TemplateKey> ItemTemplateKeys = new TempListContainer<TemplateKey>();

		// Token: 0x04002065 RID: 8293
		public readonly TempListContainer<ItemKey> ItemKeys = new TempListContainer<ItemKey>();

		// Token: 0x04002066 RID: 8294
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly StrictTempObjectContainer<List<ValueTuple<ItemBase, int>>[]> ClassifiedItems = new StrictTempObjectContainer<List<ValueTuple<ItemBase, int>>[]>(new List<ValueTuple<ItemBase, int>>[]
		{
			new List<ValueTuple<ItemBase, int>>(),
			new List<ValueTuple<ItemBase, int>>(),
			new List<ValueTuple<ItemBase, int>>()
		}, new Action<List<ValueTuple<ItemBase, int>>[]>(AdvanceMonthRelatedData.Clear<ValueTuple<ItemBase, int>>));

		// Token: 0x04002067 RID: 8295
		[TupleElementNames(new string[]
		{
			"maxGrade",
			"totalAmount"
		})]
		public readonly TempDictionaryContainer<short, ValueTuple<sbyte, int>> ItemSubTypeStats = new TempDictionaryContainer<short, ValueTuple<sbyte, int>>();

		// Token: 0x04002068 RID: 8296
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly StrictTempObjectContainer<List<ValueTuple<GameData.Domains.Item.Medicine, int>>[]> CategorizedMedicines = new StrictTempObjectContainer<List<ValueTuple<GameData.Domains.Item.Medicine, int>>[]>(new List<ValueTuple<GameData.Domains.Item.Medicine, int>>[]
		{
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Medicine, int>>()
		}, new Action<List<ValueTuple<GameData.Domains.Item.Medicine, int>>[]>(AdvanceMonthRelatedData.Clear<ValueTuple<GameData.Domains.Item.Medicine, int>>));

		// Token: 0x04002069 RID: 8297
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly TempListContainer<ValueTuple<GameData.Domains.Item.Misc, int>> ItemsForNeili = new TempListContainer<ValueTuple<GameData.Domains.Item.Misc, int>>();

		// Token: 0x0400206A RID: 8298
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly StrictTempObjectContainer<List<ValueTuple<GameData.Domains.Item.Food, int>>[]> FoodsForMainAttributes = new StrictTempObjectContainer<List<ValueTuple<GameData.Domains.Item.Food, int>>[]>(new List<ValueTuple<GameData.Domains.Item.Food, int>>[]
		{
			new List<ValueTuple<GameData.Domains.Item.Food, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Food, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Food, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Food, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Food, int>>(),
			new List<ValueTuple<GameData.Domains.Item.Food, int>>()
		}, new Action<List<ValueTuple<GameData.Domains.Item.Food, int>>[]>(AdvanceMonthRelatedData.Clear<ValueTuple<GameData.Domains.Item.Food, int>>));

		// Token: 0x0400206B RID: 8299
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public readonly TempListContainer<ValueTuple<GameData.Domains.Item.TeaWine, int>> TeaWinesForHappiness = new TempListContainer<ValueTuple<GameData.Domains.Item.TeaWine, int>>();

		// Token: 0x0400206C RID: 8300
		public readonly TempObjectContainer<PotentialRelatedCharacters> CurrBlockCanStartRelationChars = new TempObjectContainer<PotentialRelatedCharacters>(delegate(PotentialRelatedCharacters obj)
		{
			obj.OfflineClear();
		});

		// Token: 0x0400206D RID: 8301
		public readonly TempObjectContainer<PotentialRelatedCharacters> CurrBlockCanEndRelationChars = new TempObjectContainer<PotentialRelatedCharacters>(delegate(PotentialRelatedCharacters obj)
		{
			obj.OfflineClear();
		});

		// Token: 0x0400206E RID: 8302
		public readonly TempListContainer<int> CharIdList = new TempListContainer<int>();

		// Token: 0x0400206F RID: 8303
		public readonly TempListContainer<int> TargetCharIdList = new TempListContainer<int>();

		// Token: 0x04002070 RID: 8304
		public readonly TempHashsetContainer<int> BlockCharSet = new TempHashsetContainer<int>();

		// Token: 0x04002071 RID: 8305
		public readonly TempHashsetContainer<int> RelatedCharIds = new TempHashsetContainer<int>();

		// Token: 0x04002072 RID: 8306
		public readonly TempHashsetContainer<int> CaringCharIds = new TempHashsetContainer<int>();

		// Token: 0x04002073 RID: 8307
		public readonly List<ItemKey> WorldItemsToBeRemoved = new List<ItemKey>();

		// Token: 0x04002074 RID: 8308
		public readonly StrictTempObjectContainer<List<int>[]> DemandActionTargets = new StrictTempObjectContainer<List<int>[]>(new List<int>[]
		{
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>(),
			new List<int>()
		}, new Action<List<int>[]>(AdvanceMonthRelatedData.Clear<int>));

		// Token: 0x04002075 RID: 8309
		public readonly StrictTempObjectContainer<List<int>[]> PrioritizedTargets = new StrictTempObjectContainer<List<int>[]>(new List<int>[]
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
		}, new Action<List<int>[]>(AdvanceMonthRelatedData.Clear<int>));

		// Token: 0x04002076 RID: 8310
		public readonly TempListContainer<ValueTuple<short, short>> WeightTable = new TempListContainer<ValueTuple<short, short>>();

		// Token: 0x04002077 RID: 8311
		public readonly TempListContainer<short> BlockIds = new TempListContainer<short>();

		// Token: 0x04002078 RID: 8312
		public readonly TempListContainer<MapBlockData> Blocks = new TempListContainer<MapBlockData>();

		// Token: 0x04002079 RID: 8313
		public readonly TempHashsetContainer<short> TemplateIdSet = new TempHashsetContainer<short>();

		// Token: 0x0400207A RID: 8314
		public readonly TempListContainer<Wager> Wagers = new TempListContainer<Wager>();

		// Token: 0x0400207B RID: 8315
		public readonly TempListContainer<int> IntList = new TempListContainer<int>();
	}
}
