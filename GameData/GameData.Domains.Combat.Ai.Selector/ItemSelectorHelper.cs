using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Selector;

public static class ItemSelectorHelper
{
	private static readonly Dictionary<sbyte, List<ItemKey>> GradeCache = new Dictionary<sbyte, List<ItemKey>>();

	public static readonly Dictionary<EItemSelectorType, ItemSelectorPredicate> Predicates = new Dictionary<EItemSelectorType, ItemSelectorPredicate>
	{
		{
			EItemSelectorType.HealInjury,
			HealInjury
		},
		{
			EItemSelectorType.HealPoison,
			HealPoison
		},
		{
			EItemSelectorType.HealQiDisorder,
			HealQiDisorder
		},
		{
			EItemSelectorType.Buff,
			Buff
		},
		{
			EItemSelectorType.ThrowPoison,
			ThrowPoison
		},
		{
			EItemSelectorType.Neili,
			Neili
		},
		{
			EItemSelectorType.Wine,
			Wine
		}
	};

	public static ItemKey SelectBestGradeItem(IRandomSource random, IEnumerable<ItemKey> pool)
	{
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		foreach (List<ItemKey> value in GradeCache.Values)
		{
			value.Clear();
		}
		foreach (ItemKey item in pool)
		{
			sbyte grade = ItemTemplateHelper.GetGrade(item.ItemType, item.TemplateId);
			if (!list.Contains(grade))
			{
				list.Add(grade);
			}
			if (!GradeCache.ContainsKey(grade))
			{
				GradeCache.Add(grade, new List<ItemKey>());
			}
			GradeCache[grade].Add(item);
		}
		list.Sort();
		sbyte num;
		if (DomainManager.Combat.GetCombatType() != 1)
		{
			num = list[list.Count - 1];
		}
		else
		{
			num = list[Math.Max(list.Count - 1, 0) / 2];
		}
		sbyte key = num;
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return GradeCache[key].GetRandom(random);
	}

	private static bool HealInjury(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 8)
		{
			return false;
		}
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		EMedicineEffectType effectType = medicineItem.EffectType;
		if ((uint)effectType > 1u)
		{
			return false;
		}
		bool isInnerInjury = medicineItem.EffectType == EMedicineEffectType.RecoverInnerInjury;
		Injuries injuries = combatChar.GetInjuries();
		int num = 0;
		int num2 = 0;
		for (sbyte b = 0; b < 7; b++)
		{
			sbyte b2 = injuries.Get(b, isInnerInjury);
			if (b2 > 0 && b2 <= medicineItem.EffectThresholdValue)
			{
				num++;
				num2 = Math.Max(num2, b2);
			}
		}
		if (DomainManager.Combat.IsCharacterHalfFallen(combatChar))
		{
			return num > 0;
		}
		return num >= medicineItem.InjuryRecoveryTimes / 2 && num2 >= medicineItem.EffectValue / 2;
	}

	private static bool HealPoison(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 8)
		{
			return false;
		}
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		if (medicineItem.EffectType != EMedicineEffectType.DetoxPoison)
		{
			return false;
		}
		sbyte detoxPoisonType = medicineItem.DetoxPoisonType;
		if (detoxPoisonType < 0)
		{
			return false;
		}
		PoisonInts poisoned = combatChar.GetCharacter().GetPoisoned();
		sbyte b = PoisonsAndLevels.CalcPoisonedLevel(poisoned[detoxPoisonType]);
		return b <= medicineItem.EffectThresholdValue;
	}

	private static bool HealQiDisorder(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 8)
		{
			return false;
		}
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		return medicineItem.EffectType == EMedicineEffectType.ChangeDisorderOfQi;
	}

	private static bool Buff(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 8)
		{
			return false;
		}
		MedicineItem config = Config.Medicine.Instance[itemKey.TemplateId];
		return GameData.Domains.Character.Character.BonusPropertyTypes.Any((ECharacterPropertyReferencedType type) => config.GetCharacterPropertyBonusInt(type) > 0);
	}

	private static bool ThrowPoison(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 8)
		{
			return false;
		}
		short currentDistance = DomainManager.Combat.GetCurrentDistance();
		MedicineItem medicineItem = Config.Medicine.Instance[itemKey.TemplateId];
		if (medicineItem.MaxUseDistance < currentDistance)
		{
			return false;
		}
		GameData.Domains.Character.Character character = combatChar.GetCharacter();
		if (EatingItems.IsWugKing(itemKey))
		{
			return !character.GetLearnedCombatSkills().Contains(WugKing.Instance.First((WugKingItem x) => x.WugMedicine == itemKey.TemplateId).WugFinger);
		}
		return medicineItem.ItemSubType == 801 && medicineItem.EffectType == EMedicineEffectType.ApplyPoison;
	}

	private static bool Neili(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 12)
		{
			return false;
		}
		return Config.Misc.Instance[itemKey.TemplateId].Neili > 0;
	}

	private static bool Wine(CombatCharacter combatChar, ItemKey itemKey)
	{
		if (itemKey.ItemType != 9)
		{
			return false;
		}
		return Config.TeaWine.Instance[itemKey.TemplateId].ItemSubType == 901;
	}
}
