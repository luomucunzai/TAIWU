using Config;

namespace GameData.Domains.Taiwu.Profession;

public static class SharedMethods
{
	public static int GetSkillId(int professionSkillId, int skillIndex)
	{
		ProfessionItem professionItem = Config.Profession.Instance[professionSkillId];
		if (skillIndex != 3)
		{
			return professionItem.ProfessionSkills[skillIndex];
		}
		return professionItem.ExtraProfessionSkill;
	}

	public static int GetSkillUnlockSeniority(int professionSkillId)
	{
		return ProfessionSkill.Instance[professionSkillId].UnlockSeniority * 3000000 / 100;
	}
}
