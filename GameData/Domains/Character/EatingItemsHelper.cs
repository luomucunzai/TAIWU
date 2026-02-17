using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Character
{
	// Token: 0x0200080F RID: 2063
	public static class EatingItemsHelper
	{
		// Token: 0x0600746D RID: 29805 RVA: 0x004436C9 File Offset: 0x004418C9
		public static int IndexOfWug(this EatingItems eatingItems, MedicineItem wugConfig)
		{
			return eatingItems.IndexOfWug(wugConfig.WugType, wugConfig.WugGrowthType == 5);
		}

		// Token: 0x0600746E RID: 29806 RVA: 0x004436E0 File Offset: 0x004418E0
		public unsafe static int IndexOfWug(this EatingItems eatingItems, sbyte wugType, bool isKing = false)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool validMatch = isKing ? itemKey.IsValid() : (!itemKey.IsValid());
				bool flag = !validMatch;
				if (!flag)
				{
					foreach (short wugTemplateId in ItemDomain.GetWugTemplateIdGroup(wugType, isKing))
					{
						bool flag2 = itemKey.TemplateId == wugTemplateId;
						if (flag2)
						{
							return i;
						}
					}
				}
			}
			return -1;
		}

		// Token: 0x0600746F RID: 29807 RVA: 0x004437A0 File Offset: 0x004419A0
		public unsafe static int IndexOfWug(this EatingItems eatingItems, short itemTemplateId)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = itemKey.TemplateId == itemTemplateId;
				if (flag)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x06007470 RID: 29808 RVA: 0x004437F4 File Offset: 0x004419F4
		public unsafe static void ChangeDuration(this EatingItems eatingItems, DataContext context, int index, short deltaDuration, ref List<short> wugsToBeRemoved)
		{
			int duration = (int)(*(ref eatingItems.Durations.FixedElementField + (IntPtr)index * 2) + deltaDuration);
			*(ref eatingItems.Durations.FixedElementField + (IntPtr)index * 2) = (short)duration;
			bool flag = duration <= 0;
			if (flag)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)index * 8));
				*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)index * 8) = (ulong)ItemKey.Invalid;
				bool flag2 = itemKey.IsValid();
				if (flag2)
				{
					DomainManager.Item.RemoveItem(context, itemKey);
				}
				bool flag3 = EatingItems.IsWug(itemKey);
				if (flag3)
				{
					if (wugsToBeRemoved == null)
					{
						wugsToBeRemoved = new List<short>();
					}
					wugsToBeRemoved.Add(itemKey.TemplateId);
				}
			}
		}

		// Token: 0x06007471 RID: 29809 RVA: 0x004438AC File Offset: 0x00441AAC
		public unsafe static void ChangeDuration(this EatingItems eatingItems, DataContext context, int index, short deltaDuration)
		{
			int duration = (int)(*(ref eatingItems.Durations.FixedElementField + (IntPtr)index * 2) + deltaDuration);
			*(ref eatingItems.Durations.FixedElementField + (IntPtr)index * 2) = (short)duration;
			bool flag = duration <= 0;
			if (flag)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)index * 8));
				*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)index * 8) = (ulong)ItemKey.Invalid;
				bool flag2 = itemKey.IsValid();
				if (flag2)
				{
					DomainManager.Item.RemoveItem(context, itemKey);
				}
			}
		}

		// Token: 0x06007472 RID: 29810 RVA: 0x00443938 File Offset: 0x00441B38
		public unsafe static CValueModify GetCharacterPropertyBonus(this EatingItems eatingItems, ECharacterPropertyReferencedType propertyType, bool isTaiwu = false)
		{
			int add = 0;
			int addPercent = 0;
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = !EatingItems.IsValid(itemKey);
				if (!flag)
				{
					sbyte itemType = itemKey.ItemType;
					if (!true)
					{
					}
					int num;
					switch (itemType)
					{
					case 5:
						num = GameData.Domains.Item.Material.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType);
						break;
					case 6:
						goto IL_A4;
					case 7:
						num = GameData.Domains.Item.Food.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType);
						break;
					case 8:
						num = GameData.Domains.Item.Medicine.GetCharacterPropertyBonusValue(itemKey.TemplateId, propertyType);
						break;
					case 9:
						num = GameData.Domains.Item.TeaWine.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType);
						break;
					default:
						goto IL_A4;
					}
					if (!true)
					{
					}
					int valueA = num;
					sbyte itemType2 = itemKey.ItemType;
					if (!true)
					{
					}
					switch (itemType2)
					{
					case 5:
						num = 0;
						break;
					case 6:
						goto IL_128;
					case 7:
						num = 0;
						break;
					case 8:
						num = GameData.Domains.Item.Medicine.GetCharacterPropertyBonusPercentage(itemKey.TemplateId, propertyType);
						break;
					case 9:
						num = 0;
						break;
					default:
						goto IL_128;
					}
					if (!true)
					{
					}
					int valueB = num;
					if (isTaiwu)
					{
						short subType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
						bool flag2 = subType == 900 && DomainManager.Extra.IsProfessionalSkillUnlocked(16, 1);
						if (flag2)
						{
							valueA = Math.Max(valueA, 0);
							valueB = Math.Max(valueB, 0);
						}
						bool flag3 = subType == 901 && DomainManager.Extra.IsProfessionalSkillUnlocked(7, 1);
						if (flag3)
						{
							valueA = Math.Max(valueA, 0);
							valueB = Math.Max(valueB, 0);
						}
					}
					add += valueA;
					addPercent += valueB;
					goto IL_1F2;
					IL_128:
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported eating item type: ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemKey.ItemType);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					IL_A4:
					defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported eating item type: ");
					defaultInterpolatedStringHandler.AppendFormatted<sbyte>(itemKey.ItemType);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				IL_1F2:;
			}
			return new CValueModify(add, addPercent, default(CValuePercentBonus), default(CValuePercentBonus));
		}

		// Token: 0x06007473 RID: 29811 RVA: 0x00443B70 File Offset: 0x00441D70
		public unsafe static bool ContainsPoisonedItem(this EatingItems eatingItems, short poisonTemplateId)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = !EatingItems.IsValid(itemKey);
				if (!flag)
				{
					bool flag2 = !ModificationStateHelper.IsActive(itemKey.ModificationState, 1);
					if (!flag2)
					{
						FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
						bool flag3 = poisonTemplateId == poisonEffects.GetMedicineTemplateId();
						if (flag3)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x06007474 RID: 29812 RVA: 0x00443BF8 File Offset: 0x00441DF8
		public unsafe static void GetMixedPoisonTypes(this EatingItems eatingItems, ref SpanList<sbyte> mixedPoisonTypes)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = !EatingItems.IsValid(itemKey);
				if (!flag)
				{
					bool flag2 = !ModificationStateHelper.IsActive(itemKey.ModificationState, 1);
					if (!flag2)
					{
						FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
						short poisonTemplateId = poisonEffects.GetMedicineTemplateId();
						sbyte mixedPoisonType = MixedPoisonType.FromMedicineTemplateId(poisonTemplateId);
						bool flag3 = mixedPoisonType >= 0 && mixedPoisonType < 35;
						if (flag3)
						{
							mixedPoisonTypes.Add(mixedPoisonType);
						}
					}
				}
			}
		}

		// Token: 0x06007475 RID: 29813 RVA: 0x00443C98 File Offset: 0x00441E98
		public unsafe static bool ContainsWine(this EatingItems eatingItems)
		{
			for (int i = 0; i < 9; i++)
			{
				ItemKey itemKey = (ItemKey)(*(ref eatingItems.ItemKeys.FixedElementField + (IntPtr)i * 8));
				bool flag = itemKey.IsValid() && DomainManager.Item.GetBaseItem(itemKey).GetItemSubType() == 901;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06007476 RID: 29814 RVA: 0x00443D04 File Offset: 0x00441F04
		public static CValuePercentBonus CalcDamageStepBonus(this EatingItems eatingItems, EMarkType markType)
		{
			if (!true)
			{
			}
			int num;
			switch (markType)
			{
			case EMarkType.Outer:
				num = 16;
				goto IL_41;
			case EMarkType.Inner:
				num = 17;
				goto IL_41;
			case EMarkType.Mind:
				num = 19;
				goto IL_41;
			case EMarkType.Fatal:
				num = 20;
				goto IL_41;
			}
			num = -1;
			IL_41:
			if (!true)
			{
			}
			int expectBreakBonusEffect = num;
			bool flag = expectBreakBonusEffect < 0;
			CValuePercentBonus result2;
			if (flag)
			{
				result2 = 0;
			}
			else
			{
				int result = 0;
				for (int i = 0; i < 9; i++)
				{
					ItemKey itemKey = eatingItems.Get(i);
					bool flag2 = itemKey.IsValid() && itemKey.ItemType == 8;
					if (flag2)
					{
						MedicineItem medicineConfig = Config.Medicine.Instance[itemKey.TemplateId];
						bool flag3 = (int)medicineConfig.BreakBonusEffect == expectBreakBonusEffect;
						if (flag3)
						{
							result = Math.Max(result, (int)medicineConfig.DamageStepBonus);
						}
					}
				}
				result2 = result;
			}
			return result2;
		}
	}
}
