using System;
using Config.Common;
using Config.ConfigCells.Character;

namespace Config;

[Serializable]
public class CharacterFeatureItem : ConfigItem<CharacterFeatureItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string SmallVillageName;

	public readonly bool Hidden;

	public readonly ECharacterFeatureType Type;

	public readonly string Desc;

	public readonly string SmallVillageDesc;

	public readonly string EffectDesc;

	public readonly FeatureMedals[] FeatureMedals;

	public readonly sbyte Level;

	public readonly ECharacterFeatureInfectedType InfectedType;

	public readonly bool IgnoreHealthMark;

	public readonly bool IsTreasuryGuard;

	public readonly bool CanBeModified;

	public readonly bool CanBeExchanged;

	public readonly bool Mergeable;

	public readonly bool Basic;

	public readonly bool Inscribable;

	public readonly bool SoulTransform;

	public readonly bool CanDeleteManually;

	public readonly bool CanCrossArchive;

	public readonly bool InheritableThroughSamsara;

	public readonly bool InheritableTransferTaiwu;

	public readonly ECharacterFeatureDarkAshProtector DarkAshProtector;

	public readonly sbyte RequiredOrganization;

	public readonly short MutexGroupId;

	public readonly short DisplayPriority;

	public readonly sbyte AppearProb;

	public readonly sbyte Duration;

	public readonly sbyte Gender;

	public readonly sbyte CandidateGroupId;

	public readonly sbyte ProtagonistAppearProb;

	public readonly sbyte GeneticProb;

	public readonly string AssociatedSpecialEffect;

	public readonly short QiDisorderDebuffPercent;

	public readonly short QiDisorderBuffPercent;

	public readonly int SilenceFramePercent;

	public readonly short XiangshuInfectionChange;

	public readonly short MaxHealthPercentBonus;

	public readonly short AttractionPercentBonus;

	public readonly short FavorabilityIncrementFactor;

	public readonly short FavorabilityDecrementFactor;

	public readonly short AdoreMultiplePeopleChanceFactor;

	public readonly bool MakeConsummateLevelRelated;

	public readonly short[] CombatSkillPowerBonuses;

	public readonly sbyte[] FiveElementPowerBonuses;

	public readonly sbyte[] CombatSkillSlotBonuses;

	public readonly sbyte PersonalityCalm;

	public readonly sbyte PersonalityClever;

	public readonly sbyte PersonalityEnthusiastic;

	public readonly sbyte PersonalityBrave;

	public readonly sbyte PersonalityFirm;

	public readonly sbyte PersonalityLucky;

	public readonly sbyte PersonalityPerceptive;

	public readonly short Strength;

	public readonly short Vitality;

	public readonly short Dexterity;

	public readonly short Energy;

	public readonly short Intelligence;

	public readonly short Concentration;

	public readonly short Fertility;

	public readonly sbyte HobbyChangingPeriod;

	public readonly short Attraction;

	public readonly short HitRateStrength;

	public readonly short HitRateTechnique;

	public readonly short HitRateSpeed;

	public readonly short HitRateMind;

	public readonly short PenetrateOfOuter;

	public readonly short PenetrateOfInner;

	public readonly int AvoidRateStrength;

	public readonly int AvoidRateTechnique;

	public readonly int AvoidRateSpeed;

	public readonly int AvoidRateMind;

	public readonly int PenetrateResistOfOuter;

	public readonly int PenetrateResistOfInner;

	public readonly short RecoveryOfStance;

	public readonly short RecoveryOfBreath;

	public readonly short MoveSpeed;

	public readonly short RecoveryOfFlaw;

	public readonly short CastSpeed;

	public readonly short RecoveryOfBlockedAcupoint;

	public readonly short AttackSpeed;

	public readonly short WeaponSwitchSpeed;

	public readonly short InnerRatio;

	public readonly short RecoveryOfQiDisorder;

	public readonly int ResistOfHotPoison;

	public readonly int ResistOfGloomyPoison;

	public readonly int ResistOfColdPoison;

	public readonly int ResistOfRedPoison;

	public readonly int ResistOfRottenPoison;

	public readonly int ResistOfIllusoryPoison;

	public readonly short QualificationMusic;

	public readonly short QualificationChess;

	public readonly short QualificationPoem;

	public readonly short QualificationPainting;

	public readonly short QualificationMath;

	public readonly short QualificationAppraisal;

	public readonly short QualificationForging;

	public readonly short QualificationWoodworking;

	public readonly short QualificationMedicine;

	public readonly short QualificationToxicology;

	public readonly short QualificationWeaving;

	public readonly short QualificationJade;

	public readonly short QualificationTaoism;

	public readonly short QualificationBuddhism;

	public readonly short QualificationCooking;

	public readonly short QualificationEclectic;

	public readonly short QualificationNeigong;

	public readonly short QualificationPosing;

	public readonly short QualificationStunt;

	public readonly short QualificationFistAndPalm;

	public readonly short QualificationFinger;

	public readonly short QualificationLeg;

	public readonly short QualificationThrow;

	public readonly short QualificationSword;

	public readonly short QualificationBlade;

	public readonly short QualificationPolearm;

	public readonly short QualificationSpecial;

	public readonly short QualificationWhip;

	public readonly short QualificationControllableShot;

	public readonly short QualificationCombatMusic;

	public readonly bool IsChickenFeature;

	public readonly short QiDisorderDelta;

	public readonly int HealthDelta;

	public readonly int MaxNeiliAllocationDebuff;

	public readonly bool LoseConsummateBonus;

	public readonly int LifeSkillBookReadEfficiency;

	public readonly int CombatSkillBookReadEfficiency;

	public readonly sbyte[] SectFameBonus;

	public readonly sbyte NotSectFameBonu;

	public readonly sbyte TaiwuFameBonu;

	public CharacterFeatureItem(short templateId, int name, int smallVillageName, bool hidden, ECharacterFeatureType type, int desc, int smallVillageDesc, int effectDesc, FeatureMedals[] featureMedals, sbyte level, ECharacterFeatureInfectedType infectedType, bool ignoreHealthMark, bool isTreasuryGuard, bool canBeModified, bool canBeExchanged, bool mergeable, bool basic, bool inscribable, bool soulTransform, bool canDeleteManually, bool canCrossArchive, bool inheritableThroughSamsara, bool inheritableTransferTaiwu, ECharacterFeatureDarkAshProtector darkAshProtector, sbyte requiredOrganization, short mutexGroupId, short displayPriority, sbyte appearProb, sbyte duration, sbyte gender, sbyte candidateGroupId, sbyte protagonistAppearProb, sbyte geneticProb, string associatedSpecialEffect, short qiDisorderDebuffPercent, short qiDisorderBuffPercent, int silenceFramePercent, short xiangshuInfectionChange, short maxHealthPercentBonus, short attractionPercentBonus, short favorabilityIncrementFactor, short favorabilityDecrementFactor, short adoreMultiplePeopleChanceFactor, bool makeConsummateLevelRelated, short[] combatSkillPowerBonuses, sbyte[] fiveElementPowerBonuses, sbyte[] combatSkillSlotBonuses, sbyte personalityCalm, sbyte personalityClever, sbyte personalityEnthusiastic, sbyte personalityBrave, sbyte personalityFirm, sbyte personalityLucky, sbyte personalityPerceptive, short strength, short vitality, short dexterity, short energy, short intelligence, short concentration, short fertility, sbyte hobbyChangingPeriod, short attraction, short hitRateStrength, short hitRateTechnique, short hitRateSpeed, short hitRateMind, short penetrateOfOuter, short penetrateOfInner, int avoidRateStrength, int avoidRateTechnique, int avoidRateSpeed, int avoidRateMind, int penetrateResistOfOuter, int penetrateResistOfInner, short recoveryOfStance, short recoveryOfBreath, short moveSpeed, short recoveryOfFlaw, short castSpeed, short recoveryOfBlockedAcupoint, short attackSpeed, short weaponSwitchSpeed, short innerRatio, short recoveryOfQiDisorder, int resistOfHotPoison, int resistOfGloomyPoison, int resistOfColdPoison, int resistOfRedPoison, int resistOfRottenPoison, int resistOfIllusoryPoison, short qualificationMusic, short qualificationChess, short qualificationPoem, short qualificationPainting, short qualificationMath, short qualificationAppraisal, short qualificationForging, short qualificationWoodworking, short qualificationMedicine, short qualificationToxicology, short qualificationWeaving, short qualificationJade, short qualificationTaoism, short qualificationBuddhism, short qualificationCooking, short qualificationEclectic, short qualificationNeigong, short qualificationPosing, short qualificationStunt, short qualificationFistAndPalm, short qualificationFinger, short qualificationLeg, short qualificationThrow, short qualificationSword, short qualificationBlade, short qualificationPolearm, short qualificationSpecial, short qualificationWhip, short qualificationControllableShot, short qualificationCombatMusic, bool isChickenFeature, short qiDisorderDelta, int healthDelta, int maxNeiliAllocationDebuff, bool loseConsummateBonus, int lifeSkillBookReadEfficiency, int combatSkillBookReadEfficiency, sbyte[] sectFameBonus, sbyte notSectFameBonu, sbyte taiwuFameBonu)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("CharacterFeature_language", name);
		SmallVillageName = LocalStringManager.GetConfig("CharacterFeature_language", smallVillageName);
		Hidden = hidden;
		Type = type;
		Desc = LocalStringManager.GetConfig("CharacterFeature_language", desc);
		SmallVillageDesc = LocalStringManager.GetConfig("CharacterFeature_language", smallVillageDesc);
		EffectDesc = LocalStringManager.GetConfig("CharacterFeature_language", effectDesc);
		FeatureMedals = featureMedals;
		Level = level;
		InfectedType = infectedType;
		IgnoreHealthMark = ignoreHealthMark;
		IsTreasuryGuard = isTreasuryGuard;
		CanBeModified = canBeModified;
		CanBeExchanged = canBeExchanged;
		Mergeable = mergeable;
		Basic = basic;
		Inscribable = inscribable;
		SoulTransform = soulTransform;
		CanDeleteManually = canDeleteManually;
		CanCrossArchive = canCrossArchive;
		InheritableThroughSamsara = inheritableThroughSamsara;
		InheritableTransferTaiwu = inheritableTransferTaiwu;
		DarkAshProtector = darkAshProtector;
		RequiredOrganization = requiredOrganization;
		MutexGroupId = mutexGroupId;
		DisplayPriority = displayPriority;
		AppearProb = appearProb;
		Duration = duration;
		Gender = gender;
		CandidateGroupId = candidateGroupId;
		ProtagonistAppearProb = protagonistAppearProb;
		GeneticProb = geneticProb;
		AssociatedSpecialEffect = associatedSpecialEffect;
		QiDisorderDebuffPercent = qiDisorderDebuffPercent;
		QiDisorderBuffPercent = qiDisorderBuffPercent;
		SilenceFramePercent = silenceFramePercent;
		XiangshuInfectionChange = xiangshuInfectionChange;
		MaxHealthPercentBonus = maxHealthPercentBonus;
		AttractionPercentBonus = attractionPercentBonus;
		FavorabilityIncrementFactor = favorabilityIncrementFactor;
		FavorabilityDecrementFactor = favorabilityDecrementFactor;
		AdoreMultiplePeopleChanceFactor = adoreMultiplePeopleChanceFactor;
		MakeConsummateLevelRelated = makeConsummateLevelRelated;
		CombatSkillPowerBonuses = combatSkillPowerBonuses;
		FiveElementPowerBonuses = fiveElementPowerBonuses;
		CombatSkillSlotBonuses = combatSkillSlotBonuses;
		PersonalityCalm = personalityCalm;
		PersonalityClever = personalityClever;
		PersonalityEnthusiastic = personalityEnthusiastic;
		PersonalityBrave = personalityBrave;
		PersonalityFirm = personalityFirm;
		PersonalityLucky = personalityLucky;
		PersonalityPerceptive = personalityPerceptive;
		Strength = strength;
		Vitality = vitality;
		Dexterity = dexterity;
		Energy = energy;
		Intelligence = intelligence;
		Concentration = concentration;
		Fertility = fertility;
		HobbyChangingPeriod = hobbyChangingPeriod;
		Attraction = attraction;
		HitRateStrength = hitRateStrength;
		HitRateTechnique = hitRateTechnique;
		HitRateSpeed = hitRateSpeed;
		HitRateMind = hitRateMind;
		PenetrateOfOuter = penetrateOfOuter;
		PenetrateOfInner = penetrateOfInner;
		AvoidRateStrength = avoidRateStrength;
		AvoidRateTechnique = avoidRateTechnique;
		AvoidRateSpeed = avoidRateSpeed;
		AvoidRateMind = avoidRateMind;
		PenetrateResistOfOuter = penetrateResistOfOuter;
		PenetrateResistOfInner = penetrateResistOfInner;
		RecoveryOfStance = recoveryOfStance;
		RecoveryOfBreath = recoveryOfBreath;
		MoveSpeed = moveSpeed;
		RecoveryOfFlaw = recoveryOfFlaw;
		CastSpeed = castSpeed;
		RecoveryOfBlockedAcupoint = recoveryOfBlockedAcupoint;
		AttackSpeed = attackSpeed;
		WeaponSwitchSpeed = weaponSwitchSpeed;
		InnerRatio = innerRatio;
		RecoveryOfQiDisorder = recoveryOfQiDisorder;
		ResistOfHotPoison = resistOfHotPoison;
		ResistOfGloomyPoison = resistOfGloomyPoison;
		ResistOfColdPoison = resistOfColdPoison;
		ResistOfRedPoison = resistOfRedPoison;
		ResistOfRottenPoison = resistOfRottenPoison;
		ResistOfIllusoryPoison = resistOfIllusoryPoison;
		QualificationMusic = qualificationMusic;
		QualificationChess = qualificationChess;
		QualificationPoem = qualificationPoem;
		QualificationPainting = qualificationPainting;
		QualificationMath = qualificationMath;
		QualificationAppraisal = qualificationAppraisal;
		QualificationForging = qualificationForging;
		QualificationWoodworking = qualificationWoodworking;
		QualificationMedicine = qualificationMedicine;
		QualificationToxicology = qualificationToxicology;
		QualificationWeaving = qualificationWeaving;
		QualificationJade = qualificationJade;
		QualificationTaoism = qualificationTaoism;
		QualificationBuddhism = qualificationBuddhism;
		QualificationCooking = qualificationCooking;
		QualificationEclectic = qualificationEclectic;
		QualificationNeigong = qualificationNeigong;
		QualificationPosing = qualificationPosing;
		QualificationStunt = qualificationStunt;
		QualificationFistAndPalm = qualificationFistAndPalm;
		QualificationFinger = qualificationFinger;
		QualificationLeg = qualificationLeg;
		QualificationThrow = qualificationThrow;
		QualificationSword = qualificationSword;
		QualificationBlade = qualificationBlade;
		QualificationPolearm = qualificationPolearm;
		QualificationSpecial = qualificationSpecial;
		QualificationWhip = qualificationWhip;
		QualificationControllableShot = qualificationControllableShot;
		QualificationCombatMusic = qualificationCombatMusic;
		IsChickenFeature = isChickenFeature;
		QiDisorderDelta = qiDisorderDelta;
		HealthDelta = healthDelta;
		MaxNeiliAllocationDebuff = maxNeiliAllocationDebuff;
		LoseConsummateBonus = loseConsummateBonus;
		LifeSkillBookReadEfficiency = lifeSkillBookReadEfficiency;
		CombatSkillBookReadEfficiency = combatSkillBookReadEfficiency;
		SectFameBonus = sectFameBonus;
		NotSectFameBonu = notSectFameBonu;
		TaiwuFameBonu = taiwuFameBonu;
	}

	public CharacterFeatureItem()
	{
		TemplateId = 0;
		Name = null;
		SmallVillageName = null;
		Hidden = false;
		Type = ECharacterFeatureType.Special;
		Desc = null;
		SmallVillageDesc = null;
		EffectDesc = null;
		FeatureMedals = new FeatureMedals[3]
		{
			new FeatureMedals(),
			new FeatureMedals(),
			new FeatureMedals()
		};
		Level = 0;
		InfectedType = ECharacterFeatureInfectedType.NotInfected;
		IgnoreHealthMark = false;
		IsTreasuryGuard = false;
		CanBeModified = false;
		CanBeExchanged = false;
		Mergeable = false;
		Basic = false;
		Inscribable = false;
		SoulTransform = true;
		CanDeleteManually = false;
		CanCrossArchive = true;
		InheritableThroughSamsara = false;
		InheritableTransferTaiwu = false;
		DarkAshProtector = ECharacterFeatureDarkAshProtector.None;
		RequiredOrganization = 0;
		MutexGroupId = 0;
		DisplayPriority = 0;
		AppearProb = 0;
		Duration = 0;
		Gender = -1;
		CandidateGroupId = -1;
		ProtagonistAppearProb = 0;
		GeneticProb = 0;
		AssociatedSpecialEffect = null;
		QiDisorderDebuffPercent = 0;
		QiDisorderBuffPercent = 0;
		SilenceFramePercent = 0;
		XiangshuInfectionChange = 0;
		MaxHealthPercentBonus = 0;
		AttractionPercentBonus = 0;
		FavorabilityIncrementFactor = 100;
		FavorabilityDecrementFactor = 100;
		AdoreMultiplePeopleChanceFactor = 100;
		MakeConsummateLevelRelated = false;
		CombatSkillPowerBonuses = new short[5];
		FiveElementPowerBonuses = new sbyte[5];
		CombatSkillSlotBonuses = new sbyte[5];
		PersonalityCalm = 0;
		PersonalityClever = 0;
		PersonalityEnthusiastic = 0;
		PersonalityBrave = 0;
		PersonalityFirm = 0;
		PersonalityLucky = 0;
		PersonalityPerceptive = 0;
		Strength = 0;
		Vitality = 0;
		Dexterity = 0;
		Energy = 0;
		Intelligence = 0;
		Concentration = 0;
		Fertility = 0;
		HobbyChangingPeriod = 0;
		Attraction = 0;
		HitRateStrength = 0;
		HitRateTechnique = 0;
		HitRateSpeed = 0;
		HitRateMind = 0;
		PenetrateOfOuter = 0;
		PenetrateOfInner = 0;
		AvoidRateStrength = 0;
		AvoidRateTechnique = 0;
		AvoidRateSpeed = 0;
		AvoidRateMind = 0;
		PenetrateResistOfOuter = 0;
		PenetrateResistOfInner = 0;
		RecoveryOfStance = 0;
		RecoveryOfBreath = 0;
		MoveSpeed = 0;
		RecoveryOfFlaw = 0;
		CastSpeed = 0;
		RecoveryOfBlockedAcupoint = 0;
		AttackSpeed = 0;
		WeaponSwitchSpeed = 0;
		InnerRatio = 0;
		RecoveryOfQiDisorder = 0;
		ResistOfHotPoison = 0;
		ResistOfGloomyPoison = 0;
		ResistOfColdPoison = 0;
		ResistOfRedPoison = 0;
		ResistOfRottenPoison = 0;
		ResistOfIllusoryPoison = 0;
		QualificationMusic = 0;
		QualificationChess = 0;
		QualificationPoem = 0;
		QualificationPainting = 0;
		QualificationMath = 0;
		QualificationAppraisal = 0;
		QualificationForging = 0;
		QualificationWoodworking = 0;
		QualificationMedicine = 0;
		QualificationToxicology = 0;
		QualificationWeaving = 0;
		QualificationJade = 0;
		QualificationTaoism = 0;
		QualificationBuddhism = 0;
		QualificationCooking = 0;
		QualificationEclectic = 0;
		QualificationNeigong = 0;
		QualificationPosing = 0;
		QualificationStunt = 0;
		QualificationFistAndPalm = 0;
		QualificationFinger = 0;
		QualificationLeg = 0;
		QualificationThrow = 0;
		QualificationSword = 0;
		QualificationBlade = 0;
		QualificationPolearm = 0;
		QualificationSpecial = 0;
		QualificationWhip = 0;
		QualificationControllableShot = 0;
		QualificationCombatMusic = 0;
		IsChickenFeature = false;
		QiDisorderDelta = 0;
		HealthDelta = 0;
		MaxNeiliAllocationDebuff = 0;
		LoseConsummateBonus = false;
		LifeSkillBookReadEfficiency = 0;
		CombatSkillBookReadEfficiency = 0;
		SectFameBonus = new sbyte[9];
		NotSectFameBonu = 0;
		TaiwuFameBonu = 0;
	}

	public CharacterFeatureItem(short templateId, CharacterFeatureItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		SmallVillageName = other.SmallVillageName;
		Hidden = other.Hidden;
		Type = other.Type;
		Desc = other.Desc;
		SmallVillageDesc = other.SmallVillageDesc;
		EffectDesc = other.EffectDesc;
		FeatureMedals = other.FeatureMedals;
		Level = other.Level;
		InfectedType = other.InfectedType;
		IgnoreHealthMark = other.IgnoreHealthMark;
		IsTreasuryGuard = other.IsTreasuryGuard;
		CanBeModified = other.CanBeModified;
		CanBeExchanged = other.CanBeExchanged;
		Mergeable = other.Mergeable;
		Basic = other.Basic;
		Inscribable = other.Inscribable;
		SoulTransform = other.SoulTransform;
		CanDeleteManually = other.CanDeleteManually;
		CanCrossArchive = other.CanCrossArchive;
		InheritableThroughSamsara = other.InheritableThroughSamsara;
		InheritableTransferTaiwu = other.InheritableTransferTaiwu;
		DarkAshProtector = other.DarkAshProtector;
		RequiredOrganization = other.RequiredOrganization;
		MutexGroupId = other.MutexGroupId;
		DisplayPriority = other.DisplayPriority;
		AppearProb = other.AppearProb;
		Duration = other.Duration;
		Gender = other.Gender;
		CandidateGroupId = other.CandidateGroupId;
		ProtagonistAppearProb = other.ProtagonistAppearProb;
		GeneticProb = other.GeneticProb;
		AssociatedSpecialEffect = other.AssociatedSpecialEffect;
		QiDisorderDebuffPercent = other.QiDisorderDebuffPercent;
		QiDisorderBuffPercent = other.QiDisorderBuffPercent;
		SilenceFramePercent = other.SilenceFramePercent;
		XiangshuInfectionChange = other.XiangshuInfectionChange;
		MaxHealthPercentBonus = other.MaxHealthPercentBonus;
		AttractionPercentBonus = other.AttractionPercentBonus;
		FavorabilityIncrementFactor = other.FavorabilityIncrementFactor;
		FavorabilityDecrementFactor = other.FavorabilityDecrementFactor;
		AdoreMultiplePeopleChanceFactor = other.AdoreMultiplePeopleChanceFactor;
		MakeConsummateLevelRelated = other.MakeConsummateLevelRelated;
		CombatSkillPowerBonuses = other.CombatSkillPowerBonuses;
		FiveElementPowerBonuses = other.FiveElementPowerBonuses;
		CombatSkillSlotBonuses = other.CombatSkillSlotBonuses;
		PersonalityCalm = other.PersonalityCalm;
		PersonalityClever = other.PersonalityClever;
		PersonalityEnthusiastic = other.PersonalityEnthusiastic;
		PersonalityBrave = other.PersonalityBrave;
		PersonalityFirm = other.PersonalityFirm;
		PersonalityLucky = other.PersonalityLucky;
		PersonalityPerceptive = other.PersonalityPerceptive;
		Strength = other.Strength;
		Vitality = other.Vitality;
		Dexterity = other.Dexterity;
		Energy = other.Energy;
		Intelligence = other.Intelligence;
		Concentration = other.Concentration;
		Fertility = other.Fertility;
		HobbyChangingPeriod = other.HobbyChangingPeriod;
		Attraction = other.Attraction;
		HitRateStrength = other.HitRateStrength;
		HitRateTechnique = other.HitRateTechnique;
		HitRateSpeed = other.HitRateSpeed;
		HitRateMind = other.HitRateMind;
		PenetrateOfOuter = other.PenetrateOfOuter;
		PenetrateOfInner = other.PenetrateOfInner;
		AvoidRateStrength = other.AvoidRateStrength;
		AvoidRateTechnique = other.AvoidRateTechnique;
		AvoidRateSpeed = other.AvoidRateSpeed;
		AvoidRateMind = other.AvoidRateMind;
		PenetrateResistOfOuter = other.PenetrateResistOfOuter;
		PenetrateResistOfInner = other.PenetrateResistOfInner;
		RecoveryOfStance = other.RecoveryOfStance;
		RecoveryOfBreath = other.RecoveryOfBreath;
		MoveSpeed = other.MoveSpeed;
		RecoveryOfFlaw = other.RecoveryOfFlaw;
		CastSpeed = other.CastSpeed;
		RecoveryOfBlockedAcupoint = other.RecoveryOfBlockedAcupoint;
		AttackSpeed = other.AttackSpeed;
		WeaponSwitchSpeed = other.WeaponSwitchSpeed;
		InnerRatio = other.InnerRatio;
		RecoveryOfQiDisorder = other.RecoveryOfQiDisorder;
		ResistOfHotPoison = other.ResistOfHotPoison;
		ResistOfGloomyPoison = other.ResistOfGloomyPoison;
		ResistOfColdPoison = other.ResistOfColdPoison;
		ResistOfRedPoison = other.ResistOfRedPoison;
		ResistOfRottenPoison = other.ResistOfRottenPoison;
		ResistOfIllusoryPoison = other.ResistOfIllusoryPoison;
		QualificationMusic = other.QualificationMusic;
		QualificationChess = other.QualificationChess;
		QualificationPoem = other.QualificationPoem;
		QualificationPainting = other.QualificationPainting;
		QualificationMath = other.QualificationMath;
		QualificationAppraisal = other.QualificationAppraisal;
		QualificationForging = other.QualificationForging;
		QualificationWoodworking = other.QualificationWoodworking;
		QualificationMedicine = other.QualificationMedicine;
		QualificationToxicology = other.QualificationToxicology;
		QualificationWeaving = other.QualificationWeaving;
		QualificationJade = other.QualificationJade;
		QualificationTaoism = other.QualificationTaoism;
		QualificationBuddhism = other.QualificationBuddhism;
		QualificationCooking = other.QualificationCooking;
		QualificationEclectic = other.QualificationEclectic;
		QualificationNeigong = other.QualificationNeigong;
		QualificationPosing = other.QualificationPosing;
		QualificationStunt = other.QualificationStunt;
		QualificationFistAndPalm = other.QualificationFistAndPalm;
		QualificationFinger = other.QualificationFinger;
		QualificationLeg = other.QualificationLeg;
		QualificationThrow = other.QualificationThrow;
		QualificationSword = other.QualificationSword;
		QualificationBlade = other.QualificationBlade;
		QualificationPolearm = other.QualificationPolearm;
		QualificationSpecial = other.QualificationSpecial;
		QualificationWhip = other.QualificationWhip;
		QualificationControllableShot = other.QualificationControllableShot;
		QualificationCombatMusic = other.QualificationCombatMusic;
		IsChickenFeature = other.IsChickenFeature;
		QiDisorderDelta = other.QiDisorderDelta;
		HealthDelta = other.HealthDelta;
		MaxNeiliAllocationDebuff = other.MaxNeiliAllocationDebuff;
		LoseConsummateBonus = other.LoseConsummateBonus;
		LifeSkillBookReadEfficiency = other.LifeSkillBookReadEfficiency;
		CombatSkillBookReadEfficiency = other.CombatSkillBookReadEfficiency;
		SectFameBonus = other.SectFameBonus;
		NotSectFameBonu = other.NotSectFameBonu;
		TaiwuFameBonu = other.TaiwuFameBonu;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CharacterFeatureItem Duplicate(int templateId)
	{
		return new CharacterFeatureItem((short)templateId, this);
	}

	public int GetCharacterPropertyBonusInt(ECharacterPropertyReferencedType key)
	{
		return key switch
		{
			ECharacterPropertyReferencedType.PersonalityCalm => PersonalityCalm, 
			ECharacterPropertyReferencedType.PersonalityClever => PersonalityClever, 
			ECharacterPropertyReferencedType.PersonalityEnthusiastic => PersonalityEnthusiastic, 
			ECharacterPropertyReferencedType.PersonalityBrave => PersonalityBrave, 
			ECharacterPropertyReferencedType.PersonalityFirm => PersonalityFirm, 
			ECharacterPropertyReferencedType.PersonalityLucky => PersonalityLucky, 
			ECharacterPropertyReferencedType.PersonalityPerceptive => PersonalityPerceptive, 
			ECharacterPropertyReferencedType.Strength => Strength, 
			ECharacterPropertyReferencedType.Vitality => Vitality, 
			ECharacterPropertyReferencedType.Dexterity => Dexterity, 
			ECharacterPropertyReferencedType.Energy => Energy, 
			ECharacterPropertyReferencedType.Intelligence => Intelligence, 
			ECharacterPropertyReferencedType.Concentration => Concentration, 
			ECharacterPropertyReferencedType.Fertility => Fertility, 
			ECharacterPropertyReferencedType.HobbyChangingPeriod => HobbyChangingPeriod, 
			ECharacterPropertyReferencedType.Attraction => Attraction, 
			ECharacterPropertyReferencedType.HitRateStrength => HitRateStrength, 
			ECharacterPropertyReferencedType.HitRateTechnique => HitRateTechnique, 
			ECharacterPropertyReferencedType.HitRateSpeed => HitRateSpeed, 
			ECharacterPropertyReferencedType.HitRateMind => HitRateMind, 
			ECharacterPropertyReferencedType.PenetrateOfOuter => PenetrateOfOuter, 
			ECharacterPropertyReferencedType.PenetrateOfInner => PenetrateOfInner, 
			ECharacterPropertyReferencedType.AvoidRateStrength => AvoidRateStrength, 
			ECharacterPropertyReferencedType.AvoidRateTechnique => AvoidRateTechnique, 
			ECharacterPropertyReferencedType.AvoidRateSpeed => AvoidRateSpeed, 
			ECharacterPropertyReferencedType.AvoidRateMind => AvoidRateMind, 
			ECharacterPropertyReferencedType.PenetrateResistOfOuter => PenetrateResistOfOuter, 
			ECharacterPropertyReferencedType.PenetrateResistOfInner => PenetrateResistOfInner, 
			ECharacterPropertyReferencedType.RecoveryOfStance => RecoveryOfStance, 
			ECharacterPropertyReferencedType.RecoveryOfBreath => RecoveryOfBreath, 
			ECharacterPropertyReferencedType.MoveSpeed => MoveSpeed, 
			ECharacterPropertyReferencedType.RecoveryOfFlaw => RecoveryOfFlaw, 
			ECharacterPropertyReferencedType.CastSpeed => CastSpeed, 
			ECharacterPropertyReferencedType.RecoveryOfBlockedAcupoint => RecoveryOfBlockedAcupoint, 
			ECharacterPropertyReferencedType.AttackSpeed => AttackSpeed, 
			ECharacterPropertyReferencedType.WeaponSwitchSpeed => WeaponSwitchSpeed, 
			ECharacterPropertyReferencedType.InnerRatio => InnerRatio, 
			ECharacterPropertyReferencedType.RecoveryOfQiDisorder => RecoveryOfQiDisorder, 
			ECharacterPropertyReferencedType.ResistOfHotPoison => ResistOfHotPoison, 
			ECharacterPropertyReferencedType.ResistOfGloomyPoison => ResistOfGloomyPoison, 
			ECharacterPropertyReferencedType.ResistOfColdPoison => ResistOfColdPoison, 
			ECharacterPropertyReferencedType.ResistOfRedPoison => ResistOfRedPoison, 
			ECharacterPropertyReferencedType.ResistOfRottenPoison => ResistOfRottenPoison, 
			ECharacterPropertyReferencedType.ResistOfIllusoryPoison => ResistOfIllusoryPoison, 
			ECharacterPropertyReferencedType.QualificationMusic => QualificationMusic, 
			ECharacterPropertyReferencedType.QualificationChess => QualificationChess, 
			ECharacterPropertyReferencedType.QualificationPoem => QualificationPoem, 
			ECharacterPropertyReferencedType.QualificationPainting => QualificationPainting, 
			ECharacterPropertyReferencedType.QualificationMath => QualificationMath, 
			ECharacterPropertyReferencedType.QualificationAppraisal => QualificationAppraisal, 
			ECharacterPropertyReferencedType.QualificationForging => QualificationForging, 
			ECharacterPropertyReferencedType.QualificationWoodworking => QualificationWoodworking, 
			ECharacterPropertyReferencedType.QualificationMedicine => QualificationMedicine, 
			ECharacterPropertyReferencedType.QualificationToxicology => QualificationToxicology, 
			ECharacterPropertyReferencedType.QualificationWeaving => QualificationWeaving, 
			ECharacterPropertyReferencedType.QualificationJade => QualificationJade, 
			ECharacterPropertyReferencedType.QualificationTaoism => QualificationTaoism, 
			ECharacterPropertyReferencedType.QualificationBuddhism => QualificationBuddhism, 
			ECharacterPropertyReferencedType.QualificationCooking => QualificationCooking, 
			ECharacterPropertyReferencedType.QualificationEclectic => QualificationEclectic, 
			ECharacterPropertyReferencedType.QualificationNeigong => QualificationNeigong, 
			ECharacterPropertyReferencedType.QualificationPosing => QualificationPosing, 
			ECharacterPropertyReferencedType.QualificationStunt => QualificationStunt, 
			ECharacterPropertyReferencedType.QualificationFistAndPalm => QualificationFistAndPalm, 
			ECharacterPropertyReferencedType.QualificationFinger => QualificationFinger, 
			ECharacterPropertyReferencedType.QualificationLeg => QualificationLeg, 
			ECharacterPropertyReferencedType.QualificationThrow => QualificationThrow, 
			ECharacterPropertyReferencedType.QualificationSword => QualificationSword, 
			ECharacterPropertyReferencedType.QualificationBlade => QualificationBlade, 
			ECharacterPropertyReferencedType.QualificationPolearm => QualificationPolearm, 
			ECharacterPropertyReferencedType.QualificationSpecial => QualificationSpecial, 
			ECharacterPropertyReferencedType.QualificationWhip => QualificationWhip, 
			ECharacterPropertyReferencedType.QualificationControllableShot => QualificationControllableShot, 
			ECharacterPropertyReferencedType.QualificationCombatMusic => QualificationCombatMusic, 
			ECharacterPropertyReferencedType.LifeSkillBookReadEfficiency => LifeSkillBookReadEfficiency, 
			ECharacterPropertyReferencedType.CombatSkillBookReadEfficiency => CombatSkillBookReadEfficiency, 
			_ => 0, 
		};
	}
}
