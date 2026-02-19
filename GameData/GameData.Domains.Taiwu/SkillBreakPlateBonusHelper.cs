using Config;
using GameData.Domains.Character.Relation;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;

namespace GameData.Domains.Taiwu;

public static class SkillBreakPlateBonusHelper
{
	public static SkillBreakPlateBonus CreateItem(ItemKey key)
	{
		return SkillBreakPlateBonus.CreateItem(key);
	}

	public static SkillBreakPlateBonus CreateExp(int level)
	{
		return SkillBreakPlateBonus.CreateExp(level);
	}

	public static SkillBreakPlateBonus CreateRelation(int charId, int relatedCharId, ushort relationType)
	{
		if ((relationType != 16384 && relationType != 32768) || 1 == 0)
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (!DomainManager.Character.HasRelation(charId, relatedCharId, relationType) || !DomainManager.Character.HasRelation(relatedCharId, charId, relationType))
		{
			return SkillBreakPlateBonus.Invalid;
		}
		RelationKey relation = new RelationKey(charId, relatedCharId);
		short favorability = DomainManager.Character.GetFavorability(charId, relatedCharId);
		return SkillBreakPlateBonus.CreateRelation(relation, relationType, favorability);
	}

	public static SkillBreakPlateBonus CreateFriend(int charId, int relatedCharId, short skillId)
	{
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[skillId];
		if (combatSkillItem == null)
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (!DomainManager.Character.TryGetElement_Objects(charId, out var element))
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (element.GetCreatingType() != 1)
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (!element.GetLearnedCombatSkills().Contains(skillId))
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (!DomainManager.CombatSkill.TryGetElement_CombatSkills((charId: charId, skillId: skillId), out var element2))
		{
			return SkillBreakPlateBonus.Invalid;
		}
		if (!CombatSkillStateHelper.IsBrokenOut(element2.GetActivationState()))
		{
			return SkillBreakPlateBonus.Invalid;
		}
		RelationKey relation = new RelationKey(charId, relatedCharId);
		short combatSkillAttainment = element.GetCombatSkillAttainment(combatSkillItem.Type);
		short favorability = DomainManager.Character.GetFavorability(charId, relatedCharId);
		return SkillBreakPlateBonus.CreateFriend(relation, combatSkillAttainment, favorability);
	}
}
