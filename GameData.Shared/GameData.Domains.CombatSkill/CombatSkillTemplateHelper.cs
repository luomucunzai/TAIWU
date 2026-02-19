using Config;

namespace GameData.Domains.CombatSkill;

public class CombatSkillTemplateHelper
{
	public static bool IsMindHitSkill(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].PerHitDamageRateDistribution[3] == 100;
	}

	public static bool IsAttack(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].EquipType == 1;
	}

	public static bool IsAgile(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].EquipType == 2;
	}

	public static bool IsDefense(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].EquipType == 3;
	}

	public static bool IsAssist(short skillId)
	{
		return Config.CombatSkill.Instance[skillId].EquipType == 4;
	}
}
