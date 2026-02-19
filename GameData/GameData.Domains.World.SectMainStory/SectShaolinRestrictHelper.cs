using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.World.SectMainStory;

public static class SectShaolinRestrictHelper
{
	public static short GetCombatConfigId(this IEnumerable<DemonSlayerTrialRestrictItem> restricts)
	{
		short result = 211;
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		List<short> preferCombatConfigs = ObjectPool<List<short>>.Instance.Get();
		foreach (DemonSlayerTrialRestrictItem restrict in restricts)
		{
			if (list.Count == 0)
			{
				list.AddRange(restrict.EffectiveCombatConfigs);
			}
			else
			{
				list.RemoveAll((short x) => !restrict.EffectiveCombatConfigs.Contains(x));
				if (list.Count == 0)
				{
					break;
				}
			}
			if (!preferCombatConfigs.Contains(restrict.PreferCombatConfig))
			{
				preferCombatConfigs.Add(restrict.PreferCombatConfig);
			}
		}
		if (list.Count > 0)
		{
			if (preferCombatConfigs.Any(list.Contains))
			{
				list.RemoveAll((short x) => !preferCombatConfigs.Contains(x));
			}
			result = list[0];
		}
		ObjectPool<List<short>>.Instance.Return(list);
		ObjectPool<List<short>>.Instance.Return(preferCombatConfigs);
		return result;
	}

	public static bool Check(this DemonSlayerTrialRestrictItem restrict)
	{
		GameData.Domains.Character.Character taiwu = DomainManager.Taiwu.GetTaiwu();
		CombatSkillEquipment combatSkillEquipment = taiwu.GetCombatSkillEquipment();
		if (restrict.MinCombatSkillGrade >= 0 && combatSkillEquipment.GetMinGrade() < restrict.MinCombatSkillGrade)
		{
			return false;
		}
		if (restrict.MaxCombatSkillGrade >= 0 && combatSkillEquipment.GetMaxGrade() > restrict.MaxCombatSkillGrade)
		{
			return false;
		}
		if (restrict.MaxAttackSkillSlotCount >= 0 && taiwu.GetSlotCount(1) > restrict.MaxAttackSkillSlotCount)
		{
			return false;
		}
		if (restrict.MaxAgileSkillSlotCount >= 0 && taiwu.GetSlotCount(2) > restrict.MaxAgileSkillSlotCount)
		{
			return false;
		}
		if (restrict.MaxDefenseSkillSlotCount >= 0 && taiwu.GetSlotCount(3) > restrict.MaxDefenseSkillSlotCount)
		{
			return false;
		}
		if (restrict.MaxAssistSkillSlotCount >= 0 && taiwu.GetSlotCount(4) > restrict.MaxAssistSkillSlotCount)
		{
			return false;
		}
		return restrict.MaxWeaponSlotCount < 0 || taiwu.GetWeaponCount() <= restrict.MaxWeaponSlotCount;
	}

	private static int GetMinGrade(this CombatSkillEquipment combatSkillEquipment)
	{
		sbyte b = 8;
		foreach (short item in combatSkillEquipment)
		{
			b = Math.Min(b, Config.CombatSkill.Instance[item].Grade);
		}
		return b;
	}

	private static int GetMaxGrade(this CombatSkillEquipment combatSkillEquipment)
	{
		sbyte b = 0;
		foreach (short item in combatSkillEquipment)
		{
			b = Math.Max(b, Config.CombatSkill.Instance[item].Grade);
		}
		return b;
	}

	private static int GetSlotCount(this GameData.Domains.Character.Character character, sbyte equipType)
	{
		return character.GetCombatSkillTypeRequireGrid(equipType);
	}

	private static int GetWeaponCount(this GameData.Domains.Character.Character character)
	{
		ItemKey[] equipment = character.GetEquipment();
		int num = 0;
		for (sbyte b = 0; b <= 2; b++)
		{
			if (equipment[b].IsValid())
			{
				num++;
			}
		}
		return num;
	}
}
