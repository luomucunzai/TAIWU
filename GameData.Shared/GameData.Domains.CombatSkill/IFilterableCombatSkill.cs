namespace GameData.Domains.CombatSkill;

public interface IFilterableCombatSkill
{
	short SkillTemplateId { get; }

	sbyte Type { get; }

	sbyte SectId { get; }
}
