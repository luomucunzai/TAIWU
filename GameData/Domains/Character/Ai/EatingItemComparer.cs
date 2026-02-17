using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Domains.Item;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x0200084C RID: 2124
	public static class EatingItemComparer
	{
		// Token: 0x0600761B RID: 30235 RVA: 0x0044F594 File Offset: 0x0044D794
		private static int CompareInjuryMedicines([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> b)
		{
			int aEffect = (int)(a.Item1.GetEffectValue() * (short)a.Item1.GetInjuryRecoveryTimes() * a.Item1.GetDuration());
			int bEffect = (int)(b.Item1.GetEffectValue() * (short)b.Item1.GetInjuryRecoveryTimes() * b.Item1.GetDuration());
			return aEffect.CompareTo(bEffect);
		}

		// Token: 0x0600761C RID: 30236 RVA: 0x0044F5F8 File Offset: 0x0044D7F8
		private static int CompareMedicines([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> b)
		{
			short aEffect = a.Item1.GetEffectValue();
			short bEffect = b.Item1.GetEffectValue();
			return aEffect.CompareTo(bEffect);
		}

		// Token: 0x0600761D RID: 30237 RVA: 0x0044F62C File Offset: 0x0044D82C
		private static int CompareMedicinesByGrade([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> b)
		{
			return a.Item1.GetGrade().CompareTo(b.Item1.GetGrade());
		}

		// Token: 0x0600761E RID: 30238 RVA: 0x0044F65C File Offset: 0x0044D85C
		private static int CompareQiDisorderMedicines([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Medicine, int> b)
		{
			int aEffect = (int)(-(int)a.Item1.GetEffectValue());
			int bEffect = (int)(-(int)b.Item1.GetEffectValue());
			return aEffect.CompareTo(bEffect);
		}

		// Token: 0x0600761F RID: 30239 RVA: 0x0044F690 File Offset: 0x0044D890
		private static int CompareItemsForNeili([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Misc, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Misc, int> b)
		{
			return a.Item1.GetNeili().CompareTo(b.Item1.GetNeili());
		}

		// Token: 0x06007620 RID: 30240 RVA: 0x0044F6C0 File Offset: 0x0044D8C0
		private unsafe static int CompareFoodForMainAttributes([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.Food, int> b, sbyte attrType)
		{
			FoodItem aConfig = Config.Food.Instance[a.Item1.GetTemplateId()];
			short aRegen = *(ref aConfig.MainAttributesRegen.Items.FixedElementField + (IntPtr)attrType * 2);
			FoodItem bConfig = Config.Food.Instance[b.Item1.GetTemplateId()];
			short bRegen = *(ref bConfig.MainAttributesRegen.Items.FixedElementField + (IntPtr)attrType * 2);
			return aRegen.CompareTo(bRegen);
		}

		// Token: 0x06007621 RID: 30241 RVA: 0x0044F738 File Offset: 0x0044D938
		private static int CompareTeaWineForHappiness([TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.TeaWine, int> a, [TupleElementNames(new string[]
		{
			"item",
			"amount"
		})] ValueTuple<GameData.Domains.Item.TeaWine, int> b)
		{
			sbyte aHappiness = a.Item1.GetHappinessChange();
			sbyte bHappiness = b.Item1.GetHappinessChange();
			return aHappiness.CompareTo(bHappiness);
		}

		// Token: 0x04002091 RID: 8337
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>> MedicineInjury = Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.Medicine, int>>(EatingItemComparer.CompareInjuryMedicines));

		// Token: 0x04002092 RID: 8338
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>> MedicineEffect = Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.Medicine, int>>(EatingItemComparer.CompareMedicines));

		// Token: 0x04002093 RID: 8339
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>> MedicineGrade = Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.Medicine, int>>(EatingItemComparer.CompareMedicinesByGrade));

		// Token: 0x04002094 RID: 8340
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>> MedicineQiDisorder = Comparer<ValueTuple<GameData.Domains.Item.Medicine, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.Medicine, int>>(EatingItemComparer.CompareQiDisorderMedicines));

		// Token: 0x04002095 RID: 8341
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Misc, int>> MiscNeili = Comparer<ValueTuple<GameData.Domains.Item.Misc, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.Misc, int>>(EatingItemComparer.CompareItemsForNeili));

		// Token: 0x04002096 RID: 8342
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.Food, int>>[] FoodMainAttributes = new Comparer<ValueTuple<GameData.Domains.Item.Food, int>>[]
		{
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 0)),
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 1)),
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 2)),
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 3)),
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 4)),
			Comparer<ValueTuple<GameData.Domains.Item.Food, int>>.Create(([TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> a, [TupleElementNames(new string[]
			{
				"item",
				"amount"
			})] ValueTuple<GameData.Domains.Item.Food, int> b) => EatingItemComparer.CompareFoodForMainAttributes(a, b, 5))
		};

		// Token: 0x04002097 RID: 8343
		[TupleElementNames(new string[]
		{
			"item",
			"amount"
		})]
		public static Comparer<ValueTuple<GameData.Domains.Item.TeaWine, int>> TeaWineHappiness = Comparer<ValueTuple<GameData.Domains.Item.TeaWine, int>>.Create(new Comparison<ValueTuple<GameData.Domains.Item.TeaWine, int>>(EatingItemComparer.CompareTeaWineForHappiness));
	}
}
