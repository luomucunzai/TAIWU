using System;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Creation;

public struct IntelligentCharacterCreationInfo
{
	public readonly Location Location;

	public readonly OrganizationInfo OrgInfo;

	public readonly short CharTemplateId;

	public sbyte GrowingSectId;

	public sbyte GrowingSectGrade;

	public int MotherCharId;

	public Character Mother;

	public PregnantState PregnantState;

	public int FatherCharId;

	public Character Father;

	public DeadCharacter DeadFather;

	public int ActualFatherCharId;

	public Character ActualFather;

	public DeadCharacter ActualDeadFather;

	public FullName ReferenceFullName;

	public sbyte MultipleBirthCount;

	public short Age;

	public sbyte BirthMonth;

	public short BaseAttraction;

	public AvatarData Avatar;

	public bool SpecifyGenome;

	public Genome Genome;

	public int ReincarnationCharId;

	public int DestinyType;

	public short[] LifeSkillsLowerBound;

	public short[] CombatSkillsLowerBound;

	public bool InitializeSectSkills;

	public MainAttributes ParentMainAttributeValues;

	public LifeSkillShorts ParentLifeSkillQualificationValues;

	public CombatSkillShorts ParentCombatSkillQualificationValues;

	public bool AllowRandomGrowingGradeAdjust;

	public sbyte CombatSkillQualificationGrowthType;

	public sbyte LifeSkillQualificationGrowthType;

	public sbyte Gender;

	public bool Transgender;

	public sbyte Race;

	[Obsolete]
	public short[] LifeSkillsAdjustBonus;

	[Obsolete]
	public short[] CombatSkillsAdjustBonus;

	public bool DisableBeReincarnatedBySavedSoul;

	public IntelligentCharacterCreationInfo(Location location, OrganizationInfo orgInfo, short charTemplateId)
	{
		Location = location;
		OrgInfo = orgInfo;
		CharTemplateId = charTemplateId;
		GrowingSectId = -1;
		GrowingSectGrade = -1;
		MotherCharId = -1;
		Mother = null;
		PregnantState = null;
		FatherCharId = -1;
		Father = null;
		DeadFather = null;
		ActualFatherCharId = -1;
		ActualFather = null;
		ActualDeadFather = null;
		ReferenceFullName = default(FullName);
		MultipleBirthCount = 1;
		Age = -1;
		BirthMonth = -1;
		BaseAttraction = -1;
		Avatar = null;
		SpecifyGenome = false;
		Genome = default(Genome);
		ReincarnationCharId = -1;
		DestinyType = -1;
		LifeSkillsLowerBound = null;
		CombatSkillsLowerBound = null;
		InitializeSectSkills = true;
		AllowRandomGrowingGradeAdjust = false;
		CombatSkillQualificationGrowthType = -1;
		LifeSkillQualificationGrowthType = -1;
		Gender = -1;
		Transgender = false;
		Race = -1;
		LifeSkillsAdjustBonus = null;
		CombatSkillsAdjustBonus = null;
		DisableBeReincarnatedBySavedSoul = false;
	}
}
