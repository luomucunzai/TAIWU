using Config;
using GameData.Domains.Character;

namespace GameData.Domains.Taiwu;

public static class SharedMethods
{
	public static short GetQualificationWithSectApprovalBonus(sbyte orgTemplateId, short currQualification, LifeSkillShorts qualifications, out sbyte bonusLifeSkillType)
	{
		bonusLifeSkillType = -1;
		if (orgTemplateId == 0)
		{
			return currQualification;
		}
		foreach (sbyte requirementSubstitution in SectApprovingEffect.Instance[orgTemplateId - 1].RequirementSubstitutions)
		{
			if (currQualification < qualifications[requirementSubstitution])
			{
				currQualification = qualifications[requirementSubstitution];
				bonusLifeSkillType = requirementSubstitution;
			}
		}
		return currQualification;
	}
}
