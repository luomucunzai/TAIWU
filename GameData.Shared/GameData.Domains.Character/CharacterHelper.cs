using System.Collections.Generic;

namespace GameData.Domains.Character;

public static class CharacterHelper
{
	public static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort TemplateId = 1;

		public const ushort CreatingType = 2;

		public const ushort Gender = 3;

		public const ushort ActualAge = 4;

		public const ushort BirthMonth = 5;

		public const ushort Happiness = 6;

		public const ushort BaseMorality = 7;

		public const ushort OrganizationInfo = 8;

		public const ushort IdealSect = 9;

		public const ushort LifeSkillTypeInterest = 10;

		public const ushort CombatSkillTypeInterest = 11;

		public const ushort MainAttributeInterest = 12;

		public const ushort Transgender = 13;

		public const ushort Bisexual = 14;

		public const ushort XiangshuType = 15;

		public const ushort MonkType = 16;

		public const ushort FeatureIds = 17;

		public const ushort BaseMainAttributes = 18;

		public const ushort Health = 19;

		public const ushort BaseMaxHealth = 20;

		public const ushort DisorderOfQi = 21;

		public const ushort HaveLeftArm = 22;

		public const ushort HaveRightArm = 23;

		public const ushort HaveLeftLeg = 24;

		public const ushort HaveRightLeg = 25;

		public const ushort Injuries = 26;

		public const ushort ExtraNeili = 27;

		public const ushort ConsummateLevel = 28;

		public const ushort LearnedLifeSkills = 29;

		public const ushort BaseLifeSkillQualifications = 30;

		public const ushort LifeSkillQualificationGrowthType = 31;

		public const ushort BaseCombatSkillQualifications = 32;

		public const ushort CombatSkillQualificationGrowthType = 33;

		public const ushort Resources = 34;

		public const ushort LovingItemSubType = 35;

		public const ushort HatingItemSubType = 36;

		public const ushort FullName = 37;

		public const ushort MonasticTitle = 38;

		public const ushort Avatar = 39;

		public const ushort PotentialFeatureIds = 40;

		public const ushort FameActionRecords = 41;

		public const ushort Genome = 42;

		public const ushort CurrMainAttributes = 43;

		public const ushort Poisoned = 44;

		public const ushort InjuriesRecoveryProgress = 45;

		public const ushort CurrNeili = 46;

		public const ushort LoopingNeigong = 47;

		public const ushort BaseNeiliAllocation = 48;

		public const ushort ExtraNeiliAllocation = 49;

		public const ushort BaseNeiliProportionOfFiveElements = 50;

		public const ushort HobbyExpirationDate = 51;

		public const ushort LovingItemRevealed = 52;

		public const ushort HatingItemRevealed = 53;

		public const ushort LegitimateBoysCount = 54;

		public const ushort BirthLocation = 55;

		public const ushort Location = 56;

		public const ushort Equipment = 57;

		public const ushort Inventory = 58;

		public const ushort EatingItems = 59;

		public const ushort LearnedCombatSkills = 60;

		public const ushort EquippedCombatSkills = 61;

		public const ushort CombatSkillAttainmentPanels = 62;

		public const ushort SkillQualificationBonuses = 63;

		public const ushort PreexistenceCharIds = 64;

		public const ushort XiangshuInfection = 65;

		public const ushort CurrAge = 66;

		public const ushort Exp = 67;

		public const ushort ExternalRelationState = 68;

		public const ushort KidnapperId = 69;

		public const ushort LeaderId = 70;

		public const ushort FactionId = 71;

		public const ushort PersonalNeeds = 72;

		public const ushort ActionEnergies = 73;

		public const ushort NpcTravelTargets = 74;

		public const ushort PrioritizedActionCooldowns = 75;

		public const ushort PhysiologicalAge = 76;

		public const ushort Fame = 77;

		public const ushort Morality = 78;

		public const ushort Attraction = 79;

		public const ushort MaxMainAttributes = 80;

		public const ushort HitValues = 81;

		public const ushort Penetrations = 82;

		public const ushort AvoidValues = 83;

		public const ushort PenetrationResists = 84;

		public const ushort RecoveryOfStanceAndBreath = 85;

		public const ushort MoveSpeed = 86;

		public const ushort RecoveryOfFlaw = 87;

		public const ushort CastSpeed = 88;

		public const ushort RecoveryOfBlockedAcupoint = 89;

		public const ushort WeaponSwitchSpeed = 90;

		public const ushort AttackSpeed = 91;

		public const ushort InnerRatio = 92;

		public const ushort RecoveryOfQiDisorder = 93;

		public const ushort PoisonResists = 94;

		public const ushort MaxHealth = 95;

		public const ushort Fertility = 96;

		public const ushort LifeSkillQualifications = 97;

		public const ushort LifeSkillAttainments = 98;

		public const ushort CombatSkillQualifications = 99;

		public const ushort CombatSkillAttainments = 100;

		public const ushort Personalities = 101;

		public const ushort HobbyChangingPeriod = 102;

		public const ushort FavorabilityChangingFactor = 103;

		public const ushort MaxInventoryLoad = 104;

		public const ushort CurrInventoryLoad = 105;

		public const ushort MaxEquipmentLoad = 106;

		public const ushort CurrEquipmentLoad = 107;

		public const ushort InventoryTotalValue = 108;

		public const ushort MaxNeili = 109;

		public const ushort NeiliAllocation = 110;

		public const ushort NeiliProportionOfFiveElements = 111;

		public const ushort NeiliType = 112;

		public const ushort CombatPower = 113;

		public const ushort AttackTendencyOfInnerAndOuter = 114;

		public const ushort AllocatedNeiliEffects = 115;

		public const ushort MaxConsummateLevel = 116;

		public const ushort CombatSkillEquipment = 117;

		public const ushort DarkAshProtector = 118;

		public const ushort Surname = 119;

		public const ushort GivenName = 120;

		public const ushort AnonymousTitle = 121;

		public const ushort RandomFeaturesAtCreating = 122;

		public const ushort AllowUseFreeWeapon = 123;

		public const ushort AllowEscape = 124;

		public const ushort AllowHeal = 125;

		public const ushort CanDefeat = 126;

		public const ushort RandomEnemyId = 127;

		public const ushort LeadingEnemyNestId = 128;

		public const ushort FixedAvatarName = 129;

		public const ushort PresetBodyType = 130;

		public const ushort HideAge = 131;

		public const ushort Race = 132;

		public const ushort PresetFame = 133;

		public const ushort BaseAttraction = 134;

		public const ushort CanBeKidnapped = 135;

		public const ushort FixWeaponPower = 136;

		public const ushort FixArmorPower = 137;

		public const ushort FixCombatSkillPower = 138;

		public const ushort BaseHitValues = 139;

		public const ushort BasePenetrations = 140;

		public const ushort BaseAvoidValues = 141;

		public const ushort BasePenetrationResists = 142;

		public const ushort BaseRecoveryOfStanceAndBreath = 143;

		public const ushort BaseMoveSpeed = 144;

		public const ushort BaseRecoveryOfFlaw = 145;

		public const ushort BaseCastSpeed = 146;

		public const ushort BaseRecoveryOfBlockedAcupoint = 147;

		public const ushort BaseWeaponSwitchSpeed = 148;

		public const ushort BaseAttackSpeed = 149;

		public const ushort BaseInnerRatio = 150;

		public const ushort BaseRecoveryOfQiDisorder = 151;

		public const ushort BasePoisonResists = 152;

		public const ushort InnerInjuryImmunity = 153;

		public const ushort OuterInjuryImmunity = 154;

		public const ushort MindImmunity = 155;

		public const ushort FlawImmunity = 156;

		public const ushort AcupointImmunity = 157;

		public const ushort PoisonImmunities = 158;

		public const ushort PresetEquipment = 159;

		public const ushort PresetInventory = 160;

		public const ushort PresetCombatSkills = 161;

		public const ushort PresetNeiliProportionOfFiveElements = 162;

		public const ushort MinionGroupId = 163;

		public const ushort DamageSteps = 164;

		public const ushort IdeaAllocationProportion = 165;

		public const ushort ExtraEquipmentLoad = 166;

		public const ushort InitCurrAge = 167;

		public const ushort PresetTeammateCommands = 168;

		public const ushort IsFavorabilityDisplay = 169;

		public const ushort FixedCharacterShowNameOnMap = 170;

		public const ushort SpecialCombatSkeleton = 171;

		public const ushort DieImmunity = 172;

		public const ushort FatalImmunity = 173;

		public const ushort LearnedLifeSkillGrades = 174;

		public const ushort CombatAi = 175;

		public const ushort CanMove = 176;

		public const ushort CanOpenCharacterMenu = 177;

		public const ushort RandomAnimalAttack = 178;

		public const ushort DropResources = 179;

		public const ushort SpecialGradeName = 180;

		public const ushort ExtraNeiliAllocationProgress = 181;

		public const ushort PresetEatingItems = 182;

		public const ushort CanSpeak = 183;

		public const ushort RandomEnemyFavorability = 184;

		public const ushort GroupId = 185;

		public const ushort ExtraCombatSkillGrids = 186;

		public const ushort SpecialTemmateType = 187;

		public const ushort RandomIdealSects = 188;

		public const ushort AllowDropWugKing = 189;

		public const ushort AllowFavorabilitySkipCd = 190;

		public const ushort SpecialMuteBubbleEnemy = 191;

		public const ushort SpecialMuteBubbleSelf = 192;

		public const ushort DropRatePercentAsTeammate = 193;

		public const ushort DropRatePercentAsMainChar = 194;
	}

	public const ushort ArchiveFieldsCount = 76;

	public const ushort CacheFieldsCount = 43;

	public const ushort PureTemplateFieldsCount = 76;

	public const ushort WritableFieldsCount = 119;

	public const ushort ReadonlyFieldsCount = 76;

	public static readonly Dictionary<string, ushort> FieldName2FieldId = new Dictionary<string, ushort>
	{
		{ "Id", 0 },
		{ "TemplateId", 1 },
		{ "CreatingType", 2 },
		{ "Gender", 3 },
		{ "ActualAge", 4 },
		{ "BirthMonth", 5 },
		{ "Happiness", 6 },
		{ "BaseMorality", 7 },
		{ "OrganizationInfo", 8 },
		{ "IdealSect", 9 },
		{ "LifeSkillTypeInterest", 10 },
		{ "CombatSkillTypeInterest", 11 },
		{ "MainAttributeInterest", 12 },
		{ "Transgender", 13 },
		{ "Bisexual", 14 },
		{ "XiangshuType", 15 },
		{ "MonkType", 16 },
		{ "FeatureIds", 17 },
		{ "BaseMainAttributes", 18 },
		{ "Health", 19 },
		{ "BaseMaxHealth", 20 },
		{ "DisorderOfQi", 21 },
		{ "HaveLeftArm", 22 },
		{ "HaveRightArm", 23 },
		{ "HaveLeftLeg", 24 },
		{ "HaveRightLeg", 25 },
		{ "Injuries", 26 },
		{ "ExtraNeili", 27 },
		{ "ConsummateLevel", 28 },
		{ "LearnedLifeSkills", 29 },
		{ "BaseLifeSkillQualifications", 30 },
		{ "LifeSkillQualificationGrowthType", 31 },
		{ "BaseCombatSkillQualifications", 32 },
		{ "CombatSkillQualificationGrowthType", 33 },
		{ "Resources", 34 },
		{ "LovingItemSubType", 35 },
		{ "HatingItemSubType", 36 },
		{ "FullName", 37 },
		{ "MonasticTitle", 38 },
		{ "Avatar", 39 },
		{ "PotentialFeatureIds", 40 },
		{ "FameActionRecords", 41 },
		{ "Genome", 42 },
		{ "CurrMainAttributes", 43 },
		{ "Poisoned", 44 },
		{ "InjuriesRecoveryProgress", 45 },
		{ "CurrNeili", 46 },
		{ "LoopingNeigong", 47 },
		{ "BaseNeiliAllocation", 48 },
		{ "ExtraNeiliAllocation", 49 },
		{ "BaseNeiliProportionOfFiveElements", 50 },
		{ "HobbyExpirationDate", 51 },
		{ "LovingItemRevealed", 52 },
		{ "HatingItemRevealed", 53 },
		{ "LegitimateBoysCount", 54 },
		{ "BirthLocation", 55 },
		{ "Location", 56 },
		{ "Equipment", 57 },
		{ "Inventory", 58 },
		{ "EatingItems", 59 },
		{ "LearnedCombatSkills", 60 },
		{ "EquippedCombatSkills", 61 },
		{ "CombatSkillAttainmentPanels", 62 },
		{ "SkillQualificationBonuses", 63 },
		{ "PreexistenceCharIds", 64 },
		{ "XiangshuInfection", 65 },
		{ "CurrAge", 66 },
		{ "Exp", 67 },
		{ "ExternalRelationState", 68 },
		{ "KidnapperId", 69 },
		{ "LeaderId", 70 },
		{ "FactionId", 71 },
		{ "PersonalNeeds", 72 },
		{ "ActionEnergies", 73 },
		{ "NpcTravelTargets", 74 },
		{ "PrioritizedActionCooldowns", 75 },
		{ "PhysiologicalAge", 76 },
		{ "Fame", 77 },
		{ "Morality", 78 },
		{ "Attraction", 79 },
		{ "MaxMainAttributes", 80 },
		{ "HitValues", 81 },
		{ "Penetrations", 82 },
		{ "AvoidValues", 83 },
		{ "PenetrationResists", 84 },
		{ "RecoveryOfStanceAndBreath", 85 },
		{ "MoveSpeed", 86 },
		{ "RecoveryOfFlaw", 87 },
		{ "CastSpeed", 88 },
		{ "RecoveryOfBlockedAcupoint", 89 },
		{ "WeaponSwitchSpeed", 90 },
		{ "AttackSpeed", 91 },
		{ "InnerRatio", 92 },
		{ "RecoveryOfQiDisorder", 93 },
		{ "PoisonResists", 94 },
		{ "MaxHealth", 95 },
		{ "Fertility", 96 },
		{ "LifeSkillQualifications", 97 },
		{ "LifeSkillAttainments", 98 },
		{ "CombatSkillQualifications", 99 },
		{ "CombatSkillAttainments", 100 },
		{ "Personalities", 101 },
		{ "HobbyChangingPeriod", 102 },
		{ "FavorabilityChangingFactor", 103 },
		{ "MaxInventoryLoad", 104 },
		{ "CurrInventoryLoad", 105 },
		{ "MaxEquipmentLoad", 106 },
		{ "CurrEquipmentLoad", 107 },
		{ "InventoryTotalValue", 108 },
		{ "MaxNeili", 109 },
		{ "NeiliAllocation", 110 },
		{ "NeiliProportionOfFiveElements", 111 },
		{ "NeiliType", 112 },
		{ "CombatPower", 113 },
		{ "AttackTendencyOfInnerAndOuter", 114 },
		{ "AllocatedNeiliEffects", 115 },
		{ "MaxConsummateLevel", 116 },
		{ "CombatSkillEquipment", 117 },
		{ "DarkAshProtector", 118 },
		{ "Surname", 119 },
		{ "GivenName", 120 },
		{ "AnonymousTitle", 121 },
		{ "RandomFeaturesAtCreating", 122 },
		{ "AllowUseFreeWeapon", 123 },
		{ "AllowEscape", 124 },
		{ "AllowHeal", 125 },
		{ "CanDefeat", 126 },
		{ "RandomEnemyId", 127 },
		{ "LeadingEnemyNestId", 128 },
		{ "FixedAvatarName", 129 },
		{ "PresetBodyType", 130 },
		{ "HideAge", 131 },
		{ "Race", 132 },
		{ "PresetFame", 133 },
		{ "BaseAttraction", 134 },
		{ "CanBeKidnapped", 135 },
		{ "FixWeaponPower", 136 },
		{ "FixArmorPower", 137 },
		{ "FixCombatSkillPower", 138 },
		{ "BaseHitValues", 139 },
		{ "BasePenetrations", 140 },
		{ "BaseAvoidValues", 141 },
		{ "BasePenetrationResists", 142 },
		{ "BaseRecoveryOfStanceAndBreath", 143 },
		{ "BaseMoveSpeed", 144 },
		{ "BaseRecoveryOfFlaw", 145 },
		{ "BaseCastSpeed", 146 },
		{ "BaseRecoveryOfBlockedAcupoint", 147 },
		{ "BaseWeaponSwitchSpeed", 148 },
		{ "BaseAttackSpeed", 149 },
		{ "BaseInnerRatio", 150 },
		{ "BaseRecoveryOfQiDisorder", 151 },
		{ "BasePoisonResists", 152 },
		{ "InnerInjuryImmunity", 153 },
		{ "OuterInjuryImmunity", 154 },
		{ "MindImmunity", 155 },
		{ "FlawImmunity", 156 },
		{ "AcupointImmunity", 157 },
		{ "PoisonImmunities", 158 },
		{ "PresetEquipment", 159 },
		{ "PresetInventory", 160 },
		{ "PresetCombatSkills", 161 },
		{ "PresetNeiliProportionOfFiveElements", 162 },
		{ "MinionGroupId", 163 },
		{ "DamageSteps", 164 },
		{ "IdeaAllocationProportion", 165 },
		{ "ExtraEquipmentLoad", 166 },
		{ "InitCurrAge", 167 },
		{ "PresetTeammateCommands", 168 },
		{ "IsFavorabilityDisplay", 169 },
		{ "FixedCharacterShowNameOnMap", 170 },
		{ "SpecialCombatSkeleton", 171 },
		{ "DieImmunity", 172 },
		{ "FatalImmunity", 173 },
		{ "LearnedLifeSkillGrades", 174 },
		{ "CombatAi", 175 },
		{ "CanMove", 176 },
		{ "CanOpenCharacterMenu", 177 },
		{ "RandomAnimalAttack", 178 },
		{ "DropResources", 179 },
		{ "SpecialGradeName", 180 },
		{ "ExtraNeiliAllocationProgress", 181 },
		{ "PresetEatingItems", 182 },
		{ "CanSpeak", 183 },
		{ "RandomEnemyFavorability", 184 },
		{ "GroupId", 185 },
		{ "ExtraCombatSkillGrids", 186 },
		{ "SpecialTemmateType", 187 },
		{ "RandomIdealSects", 188 },
		{ "AllowDropWugKing", 189 },
		{ "AllowFavorabilitySkipCd", 190 },
		{ "SpecialMuteBubbleEnemy", 191 },
		{ "SpecialMuteBubbleSelf", 192 },
		{ "DropRatePercentAsTeammate", 193 },
		{ "DropRatePercentAsMainChar", 194 }
	};

	public static readonly string[] FieldId2FieldName = new string[195]
	{
		"Id", "TemplateId", "CreatingType", "Gender", "ActualAge", "BirthMonth", "Happiness", "BaseMorality", "OrganizationInfo", "IdealSect",
		"LifeSkillTypeInterest", "CombatSkillTypeInterest", "MainAttributeInterest", "Transgender", "Bisexual", "XiangshuType", "MonkType", "FeatureIds", "BaseMainAttributes", "Health",
		"BaseMaxHealth", "DisorderOfQi", "HaveLeftArm", "HaveRightArm", "HaveLeftLeg", "HaveRightLeg", "Injuries", "ExtraNeili", "ConsummateLevel", "LearnedLifeSkills",
		"BaseLifeSkillQualifications", "LifeSkillQualificationGrowthType", "BaseCombatSkillQualifications", "CombatSkillQualificationGrowthType", "Resources", "LovingItemSubType", "HatingItemSubType", "FullName", "MonasticTitle", "Avatar",
		"PotentialFeatureIds", "FameActionRecords", "Genome", "CurrMainAttributes", "Poisoned", "InjuriesRecoveryProgress", "CurrNeili", "LoopingNeigong", "BaseNeiliAllocation", "ExtraNeiliAllocation",
		"BaseNeiliProportionOfFiveElements", "HobbyExpirationDate", "LovingItemRevealed", "HatingItemRevealed", "LegitimateBoysCount", "BirthLocation", "Location", "Equipment", "Inventory", "EatingItems",
		"LearnedCombatSkills", "EquippedCombatSkills", "CombatSkillAttainmentPanels", "SkillQualificationBonuses", "PreexistenceCharIds", "XiangshuInfection", "CurrAge", "Exp", "ExternalRelationState", "KidnapperId",
		"LeaderId", "FactionId", "PersonalNeeds", "ActionEnergies", "NpcTravelTargets", "PrioritizedActionCooldowns", "PhysiologicalAge", "Fame", "Morality", "Attraction",
		"MaxMainAttributes", "HitValues", "Penetrations", "AvoidValues", "PenetrationResists", "RecoveryOfStanceAndBreath", "MoveSpeed", "RecoveryOfFlaw", "CastSpeed", "RecoveryOfBlockedAcupoint",
		"WeaponSwitchSpeed", "AttackSpeed", "InnerRatio", "RecoveryOfQiDisorder", "PoisonResists", "MaxHealth", "Fertility", "LifeSkillQualifications", "LifeSkillAttainments", "CombatSkillQualifications",
		"CombatSkillAttainments", "Personalities", "HobbyChangingPeriod", "FavorabilityChangingFactor", "MaxInventoryLoad", "CurrInventoryLoad", "MaxEquipmentLoad", "CurrEquipmentLoad", "InventoryTotalValue", "MaxNeili",
		"NeiliAllocation", "NeiliProportionOfFiveElements", "NeiliType", "CombatPower", "AttackTendencyOfInnerAndOuter", "AllocatedNeiliEffects", "MaxConsummateLevel", "CombatSkillEquipment", "DarkAshProtector", "Surname",
		"GivenName", "AnonymousTitle", "RandomFeaturesAtCreating", "AllowUseFreeWeapon", "AllowEscape", "AllowHeal", "CanDefeat", "RandomEnemyId", "LeadingEnemyNestId", "FixedAvatarName",
		"PresetBodyType", "HideAge", "Race", "PresetFame", "BaseAttraction", "CanBeKidnapped", "FixWeaponPower", "FixArmorPower", "FixCombatSkillPower", "BaseHitValues",
		"BasePenetrations", "BaseAvoidValues", "BasePenetrationResists", "BaseRecoveryOfStanceAndBreath", "BaseMoveSpeed", "BaseRecoveryOfFlaw", "BaseCastSpeed", "BaseRecoveryOfBlockedAcupoint", "BaseWeaponSwitchSpeed", "BaseAttackSpeed",
		"BaseInnerRatio", "BaseRecoveryOfQiDisorder", "BasePoisonResists", "InnerInjuryImmunity", "OuterInjuryImmunity", "MindImmunity", "FlawImmunity", "AcupointImmunity", "PoisonImmunities", "PresetEquipment",
		"PresetInventory", "PresetCombatSkills", "PresetNeiliProportionOfFiveElements", "MinionGroupId", "DamageSteps", "IdeaAllocationProportion", "ExtraEquipmentLoad", "InitCurrAge", "PresetTeammateCommands", "IsFavorabilityDisplay",
		"FixedCharacterShowNameOnMap", "SpecialCombatSkeleton", "DieImmunity", "FatalImmunity", "LearnedLifeSkillGrades", "CombatAi", "CanMove", "CanOpenCharacterMenu", "RandomAnimalAttack", "DropResources",
		"SpecialGradeName", "ExtraNeiliAllocationProgress", "PresetEatingItems", "CanSpeak", "RandomEnemyFavorability", "GroupId", "ExtraCombatSkillGrids", "SpecialTemmateType", "RandomIdealSects", "AllowDropWugKing",
		"AllowFavorabilitySkipCd", "SpecialMuteBubbleEnemy", "SpecialMuteBubbleSelf", "DropRatePercentAsTeammate", "DropRatePercentAsMainChar"
	};
}
