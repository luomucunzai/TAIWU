using System;

namespace Config.ConfigCells.Character;

[Serializable]
public struct PresetOrgMemberCombatSkill
{
	public short SkillGroupId;

	public sbyte MaxGrade;

	public PresetOrgMemberCombatSkill(short skillGroupId, sbyte maxGrade)
	{
		SkillGroupId = skillGroupId;
		MaxGrade = maxGrade;
	}
}
