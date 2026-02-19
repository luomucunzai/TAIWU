using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Domains.CombatSkill;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Selector;

public class CombatSkillSelector
{
	private readonly sbyte _equipType;

	private readonly CombatSkillSelectorPredicate _predicate;

	private readonly CombatSkillSelectorComparison _comparison;

	private AiMemoryNew _updatingMemory;

	private static bool IsValid(short skillId)
	{
		return skillId >= 0;
	}

	public CombatSkillSelector(sbyte equipType)
		: this(equipType, null, null)
	{
	}

	public CombatSkillSelector(sbyte equipType, CombatSkillSelectorPredicate predicate, CombatSkillSelectorComparison comparison)
	{
		_equipType = equipType;
		_predicate = predicate;
		_comparison = comparison;
	}

	private int Comparison(short skillA, short skillB)
	{
		EAiPriority priority = _updatingMemory.GetPriority(skillA);
		EAiPriority priority2 = _updatingMemory.GetPriority(skillB);
		if (priority != priority2)
		{
			int num = (int)priority;
			return num.CompareTo((int)priority2);
		}
		CombatSkillItem configA = Config.CombatSkill.Instance[skillA];
		CombatSkillItem configB = Config.CombatSkill.Instance[skillB];
		return Comparison(configA, configB);
	}

	private int Comparison(CombatSkillItem configA, CombatSkillItem configB)
	{
		int num = _comparison?.Invoke(configA, configB) ?? 0;
		if (num != 0)
		{
			return num;
		}
		if (configA.Grade != configB.Grade)
		{
			return configB.Grade.CompareTo(configA.Grade);
		}
		return configA.TemplateId.CompareTo(configB.TemplateId);
	}

	private bool Predicate(short skillId)
	{
		return Predicate(Config.CombatSkill.Instance[skillId]);
	}

	private bool Predicate(CombatSkillItem config)
	{
		return _predicate?.Invoke(config) ?? true;
	}

	public short Select(AiMemoryNew memory, CombatCharacter combatChar)
	{
		_updatingMemory = memory;
		IEnumerable<short> enumerable = combatChar.GetCombatSkillList(_equipType).Where(IsValid);
		sbyte equipType = _equipType;
		if (1 == 0)
		{
		}
		IEnumerable<short> enumerable2 = equipType switch
		{
			1 => enumerable.Where(CombatSkillTemplateHelper.IsAttack), 
			3 => enumerable.Where(CombatSkillTemplateHelper.IsDefense), 
			2 => enumerable.Where(CombatSkillTemplateHelper.IsAgile), 
			_ => enumerable, 
		};
		if (1 == 0)
		{
		}
		enumerable = enumerable2;
		enumerable = enumerable.Where(combatChar.AiCanCast).Where(Predicate);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(enumerable);
		list.Sort(Comparison);
		short result = (short)((list.Count > 0) ? list[0] : (-1));
		ObjectPool<List<short>>.Instance.Return(list);
		_updatingMemory = null;
		return result;
	}
}
