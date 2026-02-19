using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Character;

public static class EatingItemsHelper
{
	public static int IndexOfWug(this EatingItems eatingItems, MedicineItem wugConfig)
	{
		return eatingItems.IndexOfWug(wugConfig.WugType, wugConfig.WugGrowthType == 5);
	}

	public unsafe static int IndexOfWug(this EatingItems eatingItems, sbyte wugType, bool isKing = false)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (!(isKing ? itemKey.IsValid() : (!itemKey.IsValid())))
			{
				continue;
			}
			foreach (short item in ItemDomain.GetWugTemplateIdGroup(wugType, isKing))
			{
				if (itemKey.TemplateId == item)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public unsafe static int IndexOfWug(this EatingItems eatingItems, short itemTemplateId)
	{
		for (int i = 0; i < 9; i++)
		{
			if (((ItemKey)eatingItems.ItemKeys[i]).TemplateId == itemTemplateId)
			{
				return i;
			}
		}
		return -1;
	}

	public unsafe static void ChangeDuration(this ref EatingItems eatingItems, DataContext context, int index, short deltaDuration, ref List<short> wugsToBeRemoved)
	{
		int num = eatingItems.Durations[index] + deltaDuration;
		eatingItems.Durations[index] = (short)num;
		if (num > 0)
		{
			return;
		}
		ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[index];
		eatingItems.ItemKeys[index] = (ulong)ItemKey.Invalid;
		if (itemKey.IsValid())
		{
			DomainManager.Item.RemoveItem(context, itemKey);
		}
		if (EatingItems.IsWug(itemKey))
		{
			if (wugsToBeRemoved == null)
			{
				wugsToBeRemoved = new List<short>();
			}
			wugsToBeRemoved.Add(itemKey.TemplateId);
		}
	}

	public unsafe static void ChangeDuration(this ref EatingItems eatingItems, DataContext context, int index, short deltaDuration)
	{
		int num = eatingItems.Durations[index] + deltaDuration;
		eatingItems.Durations[index] = (short)num;
		if (num <= 0)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[index];
			eatingItems.ItemKeys[index] = (ulong)ItemKey.Invalid;
			if (itemKey.IsValid())
			{
				DomainManager.Item.RemoveItem(context, itemKey);
			}
		}
	}

	public unsafe static CValueModify GetCharacterPropertyBonus(this EatingItems eatingItems, ECharacterPropertyReferencedType propertyType, bool isTaiwu = false)
	{
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0213: Unknown result type (might be due to invalid IL or missing references)
		//IL_0217: Unknown result type (might be due to invalid IL or missing references)
		//IL_021d: Unknown result type (might be due to invalid IL or missing references)
		//IL_021f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0228: Unknown result type (might be due to invalid IL or missing references)
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (!EatingItems.IsValid(itemKey))
			{
				continue;
			}
			sbyte itemType = itemKey.ItemType;
			if (1 == 0)
			{
			}
			int num3 = itemType switch
			{
				7 => GameData.Domains.Item.Food.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType), 
				8 => GameData.Domains.Item.Medicine.GetCharacterPropertyBonusValue(itemKey.TemplateId, propertyType), 
				9 => GameData.Domains.Item.TeaWine.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType), 
				5 => GameData.Domains.Item.Material.GetCharacterPropertyBonus(itemKey.TemplateId, propertyType), 
				_ => throw new Exception($"Unsupported eating item type: {itemKey.ItemType}"), 
			};
			if (1 == 0)
			{
			}
			int num4 = num3;
			sbyte itemType2 = itemKey.ItemType;
			if (1 == 0)
			{
			}
			num3 = itemType2 switch
			{
				8 => GameData.Domains.Item.Medicine.GetCharacterPropertyBonusPercentage(itemKey.TemplateId, propertyType), 
				7 => 0, 
				9 => 0, 
				5 => 0, 
				_ => throw new Exception($"Unsupported eating item type: {itemKey.ItemType}"), 
			};
			if (1 == 0)
			{
			}
			int num5 = num3;
			if (isTaiwu)
			{
				short itemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
				if (itemSubType == 900 && DomainManager.Extra.IsProfessionalSkillUnlocked(16, 1))
				{
					num4 = Math.Max(num4, 0);
					num5 = Math.Max(num5, 0);
				}
				if (itemSubType == 901 && DomainManager.Extra.IsProfessionalSkillUnlocked(7, 1))
				{
					num4 = Math.Max(num4, 0);
					num5 = Math.Max(num5, 0);
				}
			}
			num += num4;
			num2 += num5;
		}
		return new CValueModify(num, CValuePercentBonus.op_Implicit(num2), default(CValuePercentBonus), default(CValuePercentBonus));
	}

	public unsafe static bool ContainsPoisonedItem(this EatingItems eatingItems, short poisonTemplateId)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (EatingItems.IsValid(itemKey) && ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
			{
				FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
				if (poisonTemplateId == poisonEffects.GetMedicineTemplateId())
				{
					return true;
				}
			}
		}
		return false;
	}

	public unsafe static void GetMixedPoisonTypes(this EatingItems eatingItems, ref SpanList<sbyte> mixedPoisonTypes)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (EatingItems.IsValid(itemKey) && ModificationStateHelper.IsActive(itemKey.ModificationState, 1))
			{
				FullPoisonEffects poisonEffects = DomainManager.Item.GetPoisonEffects(itemKey);
				short medicineTemplateId = poisonEffects.GetMedicineTemplateId();
				sbyte b = MixedPoisonType.FromMedicineTemplateId(medicineTemplateId);
				if (b >= 0 && b < 35)
				{
					mixedPoisonTypes.Add(b);
				}
			}
		}
	}

	public unsafe static bool ContainsWine(this EatingItems eatingItems)
	{
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = (ItemKey)eatingItems.ItemKeys[i];
			if (itemKey.IsValid() && DomainManager.Item.GetBaseItem(itemKey).GetItemSubType() == 901)
			{
				return true;
			}
		}
		return false;
	}

	public static CValuePercentBonus CalcDamageStepBonus(this EatingItems eatingItems, EMarkType markType)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		if (1 == 0)
		{
		}
		int num = markType switch
		{
			EMarkType.Outer => 16, 
			EMarkType.Inner => 17, 
			EMarkType.Fatal => 20, 
			EMarkType.Mind => 19, 
			_ => -1, 
		};
		if (1 == 0)
		{
		}
		int num2 = num;
		if (num2 < 0)
		{
			return CValuePercentBonus.op_Implicit(0);
		}
		int num3 = 0;
		for (int i = 0; i < 9; i++)
		{
			ItemKey itemKey = eatingItems.Get(i);
			if (itemKey.IsValid() && itemKey.ItemType == 8)
			{
				MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
				if (medicineItem.BreakBonusEffect == num2)
				{
					num3 = Math.Max(num3, medicineItem.DamageStepBonus);
				}
			}
		}
		return CValuePercentBonus.op_Implicit(num3);
	}
}
