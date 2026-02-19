using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Combat;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.CombatSkill;

[SerializableGameData(NotForArchive = true)]
public class CombatSkillDisplayData : ISerializableGameData, IFilterableCombatSkill
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public short TemplateId;

	[Obsolete("This field is deprecated in favour of the new combat skill.")]
	public sbyte PracticeLevel;

	[SerializableGameDataField]
	public ushort ReadingState;

	[SerializableGameDataField]
	public ushort ActivationState;

	[SerializableGameDataField]
	public bool CanAffect;

	[SerializableGameDataField]
	public bool Conflicting;

	[SerializableGameDataField]
	public sbyte GridCount;

	[SerializableGameDataField]
	public short Power;

	[SerializableGameDataField]
	public short MaxPower;

	[SerializableGameDataField]
	public short RequirementsPower;

	[SerializableGameDataField]
	public List<(int type, int required, int actual)> Requirements;

	[SerializableGameDataField]
	public List<(short id, short bonus, bool isExtra)> BreakAddProperty;

	[SerializableGameDataField]
	public List<(short propertyId, int bonus)> NeiliAllocationAddProperty;

	[SerializableGameDataField]
	public sbyte BreakPlateIndex;

	[SerializableGameDataField]
	public sbyte EffectType;

	[SerializableGameDataField]
	public bool Mastered;

	[SerializableGameDataField]
	public bool PreviewMastered;

	[SerializableGameDataField]
	public bool Revoked;

	[SerializableGameDataField]
	public short JumpThreshold;

	[SerializableGameDataField]
	public sbyte BaseInnerRatio;

	[SerializableGameDataField]
	public sbyte InnerRatioChangeRange;

	[SerializableGameDataField]
	public sbyte CurrInnerRatio;

	[SerializableGameDataField]
	public sbyte ExpectInnerRatio;

	[SerializableGameDataField]
	public int NewUnderstandingNeedExp;

	[SerializableGameDataField]
	public bool BreakSuccess;

	[SerializableGameDataField]
	public CombatSkillEffectDescriptionDisplayData EffectDescription;

	[SerializableGameDataField]
	public CombatSkillDamageStepBonusDisplayData DamageStepBonus;

	[SerializableGameDataField]
	public List<sbyte> BreakBonusGrades;

	[SerializableGameDataField]
	public short MaxObtainableNeili;

	[SerializableGameDataField]
	public short ObtainedNeili;

	[SerializableGameDataField]
	public sbyte[] SpecificGrids = new sbyte[4];

	[SerializableGameDataField]
	public sbyte GenericGrid;

	[SerializableGameDataField]
	public short AddAttackDistanceForward;

	[SerializableGameDataField]
	public short AddAttackDistanceBackward;

	[SerializableGameDataField]
	public int HitValueStrength;

	[SerializableGameDataField]
	public int HitValueTechnique;

	[SerializableGameDataField]
	public int HitValueSpeed;

	[SerializableGameDataField]
	public int HitValueMind;

	[SerializableGameDataField]
	public int PenetrateValueOuter;

	[SerializableGameDataField]
	public int PenetrateValueInner;

	[SerializableGameDataField]
	public PoisonsAndLevels Poisons;

	[SerializableGameDataField]
	public HitOrAvoidInts HitDistribution;

	[SerializableGameDataField]
	public List<int> BodyPartWeights;

	[SerializableGameDataField]
	public sbyte FullPowerCastTimes;

	[SerializableGameDataField]
	public int JumpSpeed;

	[SerializableGameDataField]
	public short AddMoveSpeed;

	[SerializableGameDataField]
	public short AddPercentMoveSpeed;

	[SerializableGameDataField]
	public int AddHitStrength;

	[SerializableGameDataField]
	public int AddHitTechnique;

	[SerializableGameDataField]
	public int AddHitSpeed;

	[SerializableGameDataField]
	public int AddHitMind;

	[SerializableGameDataField]
	public int AddInnerDef;

	[SerializableGameDataField]
	public int AddOuterDef;

	[SerializableGameDataField]
	public int AddAvoidStrength;

	[SerializableGameDataField]
	public int AddAvoidTechnique;

	[SerializableGameDataField]
	public int AddAvoidSpeed;

	[SerializableGameDataField]
	public int AddAvoidMind;

	[SerializableGameDataField]
	public int BouncePowerOuter;

	[SerializableGameDataField]
	public int BouncePowerInner;

	[SerializableGameDataField]
	public short BounceDistance;

	[SerializableGameDataField]
	public int FightbackPower;

	[SerializableGameDataField]
	public short EffectDuration;

	[SerializableGameDataField]
	public short CostMobility;

	[SerializableGameDataField]
	public sbyte CostMobilityFontType;

	[SerializableGameDataField]
	public List<NeedTrick> CostTricks;

	[SerializableGameDataField]
	public List<sbyte> CostTricksFontType;

	[SerializableGameDataField]
	public sbyte CostBreath;

	[SerializableGameDataField]
	public sbyte CostBreathFontType;

	[SerializableGameDataField]
	public sbyte CostStance;

	[SerializableGameDataField]
	public sbyte CostStanceFontType;

	[SerializableGameDataField]
	public (sbyte, sbyte) CostNeiliAllocation;

	[SerializableGameDataField]
	public sbyte CostNeiliAllocationFontType;

	[SerializableGameDataField]
	public sbyte CostWeaponDurabilityFontType;

	[SerializableGameDataField]
	public sbyte CostWugFontType;

	[SerializableGameDataField]
	public sbyte FiveElementDestTypeWhileLooping;

	[SerializableGameDataField]
	public sbyte FiveElementTransferTypeWhileLooping;

	[SerializableGameDataField]
	public List<CombatSkillEffectData> EffectData;

	short IFilterableCombatSkill.SkillTemplateId => TemplateId;

	sbyte IFilterableCombatSkill.Type => SkillConfig.Type;

	sbyte IFilterableCombatSkill.SectId => SkillConfig.SectId;

	private CombatSkillItem SkillConfig => Config.CombatSkill.Instance[TemplateId];

	public CombatSkillDisplayData()
	{
	}

	public CombatSkillDisplayData(CombatSkillDisplayData other)
	{
		CharId = other.CharId;
		TemplateId = other.TemplateId;
		ReadingState = other.ReadingState;
		ActivationState = other.ActivationState;
		CanAffect = other.CanAffect;
		Conflicting = other.Conflicting;
		GridCount = other.GridCount;
		Power = other.Power;
		MaxPower = other.MaxPower;
		RequirementsPower = other.RequirementsPower;
		Requirements = ((other.Requirements == null) ? null : new List<(int, int, int)>(other.Requirements));
		BreakAddProperty = ((other.BreakAddProperty == null) ? null : new List<(short, short, bool)>(other.BreakAddProperty));
		NeiliAllocationAddProperty = ((other.NeiliAllocationAddProperty == null) ? null : new List<(short, int)>(other.NeiliAllocationAddProperty));
		BreakPlateIndex = other.BreakPlateIndex;
		EffectType = other.EffectType;
		Mastered = other.Mastered;
		PreviewMastered = other.PreviewMastered;
		Revoked = other.Revoked;
		JumpThreshold = other.JumpThreshold;
		BaseInnerRatio = other.BaseInnerRatio;
		InnerRatioChangeRange = other.InnerRatioChangeRange;
		CurrInnerRatio = other.CurrInnerRatio;
		ExpectInnerRatio = other.ExpectInnerRatio;
		NewUnderstandingNeedExp = other.NewUnderstandingNeedExp;
		BreakSuccess = other.BreakSuccess;
		EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
		DamageStepBonus = other.DamageStepBonus;
		BreakBonusGrades = ((other.BreakBonusGrades == null) ? null : new List<sbyte>(other.BreakBonusGrades));
		MaxObtainableNeili = other.MaxObtainableNeili;
		ObtainedNeili = other.ObtainedNeili;
		sbyte[] specificGrids = other.SpecificGrids;
		int num = specificGrids.Length;
		SpecificGrids = new sbyte[num];
		for (int i = 0; i < num; i++)
		{
			SpecificGrids[i] = specificGrids[i];
		}
		GenericGrid = other.GenericGrid;
		AddAttackDistanceForward = other.AddAttackDistanceForward;
		AddAttackDistanceBackward = other.AddAttackDistanceBackward;
		HitValueStrength = other.HitValueStrength;
		HitValueTechnique = other.HitValueTechnique;
		HitValueSpeed = other.HitValueSpeed;
		HitValueMind = other.HitValueMind;
		PenetrateValueOuter = other.PenetrateValueOuter;
		PenetrateValueInner = other.PenetrateValueInner;
		Poisons = other.Poisons;
		HitDistribution = other.HitDistribution;
		BodyPartWeights = ((other.BodyPartWeights == null) ? null : new List<int>(other.BodyPartWeights));
		FullPowerCastTimes = other.FullPowerCastTimes;
		JumpSpeed = other.JumpSpeed;
		AddMoveSpeed = other.AddMoveSpeed;
		AddPercentMoveSpeed = other.AddPercentMoveSpeed;
		AddHitStrength = other.AddHitStrength;
		AddHitTechnique = other.AddHitTechnique;
		AddHitSpeed = other.AddHitSpeed;
		AddHitMind = other.AddHitMind;
		AddInnerDef = other.AddInnerDef;
		AddOuterDef = other.AddOuterDef;
		AddAvoidStrength = other.AddAvoidStrength;
		AddAvoidTechnique = other.AddAvoidTechnique;
		AddAvoidSpeed = other.AddAvoidSpeed;
		AddAvoidMind = other.AddAvoidMind;
		BouncePowerOuter = other.BouncePowerOuter;
		BouncePowerInner = other.BouncePowerInner;
		BounceDistance = other.BounceDistance;
		FightbackPower = other.FightbackPower;
		EffectDuration = other.EffectDuration;
		CostMobility = other.CostMobility;
		CostMobilityFontType = other.CostMobilityFontType;
		CostTricks = ((other.CostTricks == null) ? null : new List<NeedTrick>(other.CostTricks));
		CostTricksFontType = ((other.CostTricksFontType == null) ? null : new List<sbyte>(other.CostTricksFontType));
		CostBreath = other.CostBreath;
		CostBreathFontType = other.CostBreathFontType;
		CostStance = other.CostStance;
		CostStanceFontType = other.CostStanceFontType;
		CostNeiliAllocation = other.CostNeiliAllocation;
		CostNeiliAllocationFontType = other.CostNeiliAllocationFontType;
		CostWeaponDurabilityFontType = other.CostWeaponDurabilityFontType;
		CostWugFontType = other.CostWugFontType;
		FiveElementDestTypeWhileLooping = other.FiveElementDestTypeWhileLooping;
		FiveElementTransferTypeWhileLooping = other.FiveElementTransferTypeWhileLooping;
		EffectData = ((other.EffectData == null) ? null : new List<CombatSkillEffectData>(other.EffectData));
	}

	public void Assign(CombatSkillDisplayData other)
	{
		CharId = other.CharId;
		TemplateId = other.TemplateId;
		ReadingState = other.ReadingState;
		ActivationState = other.ActivationState;
		CanAffect = other.CanAffect;
		Conflicting = other.Conflicting;
		GridCount = other.GridCount;
		Power = other.Power;
		MaxPower = other.MaxPower;
		RequirementsPower = other.RequirementsPower;
		Requirements = ((other.Requirements == null) ? null : new List<(int, int, int)>(other.Requirements));
		BreakAddProperty = ((other.BreakAddProperty == null) ? null : new List<(short, short, bool)>(other.BreakAddProperty));
		NeiliAllocationAddProperty = ((other.NeiliAllocationAddProperty == null) ? null : new List<(short, int)>(other.NeiliAllocationAddProperty));
		BreakPlateIndex = other.BreakPlateIndex;
		EffectType = other.EffectType;
		Mastered = other.Mastered;
		PreviewMastered = other.PreviewMastered;
		Revoked = other.Revoked;
		JumpThreshold = other.JumpThreshold;
		BaseInnerRatio = other.BaseInnerRatio;
		InnerRatioChangeRange = other.InnerRatioChangeRange;
		CurrInnerRatio = other.CurrInnerRatio;
		ExpectInnerRatio = other.ExpectInnerRatio;
		NewUnderstandingNeedExp = other.NewUnderstandingNeedExp;
		BreakSuccess = other.BreakSuccess;
		EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
		DamageStepBonus = other.DamageStepBonus;
		BreakBonusGrades = ((other.BreakBonusGrades == null) ? null : new List<sbyte>(other.BreakBonusGrades));
		MaxObtainableNeili = other.MaxObtainableNeili;
		ObtainedNeili = other.ObtainedNeili;
		sbyte[] specificGrids = other.SpecificGrids;
		int num = specificGrids.Length;
		SpecificGrids = new sbyte[num];
		for (int i = 0; i < num; i++)
		{
			SpecificGrids[i] = specificGrids[i];
		}
		GenericGrid = other.GenericGrid;
		AddAttackDistanceForward = other.AddAttackDistanceForward;
		AddAttackDistanceBackward = other.AddAttackDistanceBackward;
		HitValueStrength = other.HitValueStrength;
		HitValueTechnique = other.HitValueTechnique;
		HitValueSpeed = other.HitValueSpeed;
		HitValueMind = other.HitValueMind;
		PenetrateValueOuter = other.PenetrateValueOuter;
		PenetrateValueInner = other.PenetrateValueInner;
		Poisons = other.Poisons;
		HitDistribution = other.HitDistribution;
		BodyPartWeights = ((other.BodyPartWeights == null) ? null : new List<int>(other.BodyPartWeights));
		FullPowerCastTimes = other.FullPowerCastTimes;
		JumpSpeed = other.JumpSpeed;
		AddMoveSpeed = other.AddMoveSpeed;
		AddPercentMoveSpeed = other.AddPercentMoveSpeed;
		AddHitStrength = other.AddHitStrength;
		AddHitTechnique = other.AddHitTechnique;
		AddHitSpeed = other.AddHitSpeed;
		AddHitMind = other.AddHitMind;
		AddInnerDef = other.AddInnerDef;
		AddOuterDef = other.AddOuterDef;
		AddAvoidStrength = other.AddAvoidStrength;
		AddAvoidTechnique = other.AddAvoidTechnique;
		AddAvoidSpeed = other.AddAvoidSpeed;
		AddAvoidMind = other.AddAvoidMind;
		BouncePowerOuter = other.BouncePowerOuter;
		BouncePowerInner = other.BouncePowerInner;
		BounceDistance = other.BounceDistance;
		FightbackPower = other.FightbackPower;
		EffectDuration = other.EffectDuration;
		CostMobility = other.CostMobility;
		CostMobilityFontType = other.CostMobilityFontType;
		CostTricks = ((other.CostTricks == null) ? null : new List<NeedTrick>(other.CostTricks));
		CostTricksFontType = ((other.CostTricksFontType == null) ? null : new List<sbyte>(other.CostTricksFontType));
		CostBreath = other.CostBreath;
		CostBreathFontType = other.CostBreathFontType;
		CostStance = other.CostStance;
		CostStanceFontType = other.CostStanceFontType;
		CostNeiliAllocation = other.CostNeiliAllocation;
		CostNeiliAllocationFontType = other.CostNeiliAllocationFontType;
		CostWeaponDurabilityFontType = other.CostWeaponDurabilityFontType;
		CostWugFontType = other.CostWugFontType;
		FiveElementDestTypeWhileLooping = other.FiveElementDestTypeWhileLooping;
		FiveElementTransferTypeWhileLooping = other.FiveElementTransferTypeWhileLooping;
		EffectData = ((other.EffectData == null) ? null : new List<CombatSkillEffectData>(other.EffectData));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 197;
		if (Requirements != null)
		{
			num += 2;
			int count = Requirements.Count;
			for (int i = 0; i < count; i++)
			{
				(int, int, int) tuple = Requirements[i];
				num += SerializationHelper.GetSerializedSize<int, int, int>(tuple);
			}
		}
		else
		{
			num += 2;
		}
		if (BreakAddProperty != null)
		{
			num += 2;
			int count2 = BreakAddProperty.Count;
			for (int j = 0; j < count2; j++)
			{
				(short, short, bool) tuple2 = BreakAddProperty[j];
				num += SerializationHelper.GetSerializedSize<short, short, bool>(tuple2);
			}
		}
		else
		{
			num += 2;
		}
		if (NeiliAllocationAddProperty != null)
		{
			num += 2;
			int count3 = NeiliAllocationAddProperty.Count;
			for (int k = 0; k < count3; k++)
			{
				(short, int) tuple3 = NeiliAllocationAddProperty[k];
				num += SerializationHelper.GetSerializedSize<short, int>(tuple3);
			}
		}
		else
		{
			num += 2;
		}
		num += EffectDescription.GetSerializedSize();
		num = ((SpecificGrids == null) ? (num + 2) : (num + (2 + SpecificGrids.Length)));
		num = ((BodyPartWeights == null) ? (num + 2) : (num + (2 + 4 * BodyPartWeights.Count)));
		num = ((BreakBonusGrades == null) ? (num + 2) : (num + (2 + BreakBonusGrades.Count)));
		num = ((CostTricks == null) ? (num + 2) : (num + (2 + 4 * CostTricks.Count)));
		num = ((CostTricksFontType == null) ? (num + 2) : (num + (2 + CostTricksFontType.Count)));
		num = ((EffectData == null) ? (num + 2) : (num + (2 + 8 * EffectData.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		*(short*)ptr = TemplateId;
		ptr += 2;
		*(ushort*)ptr = ReadingState;
		ptr += 2;
		*(ushort*)ptr = ActivationState;
		ptr += 2;
		*ptr = (CanAffect ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Conflicting ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)GridCount;
		ptr++;
		*(short*)ptr = Power;
		ptr += 2;
		*(short*)ptr = MaxPower;
		ptr += 2;
		*(short*)ptr = RequirementsPower;
		ptr += 2;
		if (Requirements != null)
		{
			int count = Requirements.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				(int, int, int) tuple = Requirements[i];
				ptr += SerializationHelper.Serialize<int, int, int>(ptr, tuple);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BreakAddProperty != null)
		{
			int count2 = BreakAddProperty.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				(short, short, bool) tuple2 = BreakAddProperty[j];
				ptr += SerializationHelper.Serialize<short, short, bool>(ptr, tuple2);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (NeiliAllocationAddProperty != null)
		{
			int count3 = NeiliAllocationAddProperty.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				(short, int) tuple3 = NeiliAllocationAddProperty[k];
				ptr += SerializationHelper.Serialize<short, int>(ptr, tuple3);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)BreakPlateIndex;
		ptr++;
		*ptr = (byte)EffectType;
		ptr++;
		*ptr = (Mastered ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (PreviewMastered ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (Revoked ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = JumpThreshold;
		ptr += 2;
		*ptr = (byte)BaseInnerRatio;
		ptr++;
		*ptr = (byte)InnerRatioChangeRange;
		ptr++;
		*ptr = (byte)CurrInnerRatio;
		ptr++;
		*ptr = (byte)ExpectInnerRatio;
		ptr++;
		*(int*)ptr = NewUnderstandingNeedExp;
		ptr += 4;
		*ptr = (BreakSuccess ? ((byte)1) : ((byte)0));
		ptr++;
		int num = EffectDescription.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		ptr += DamageStepBonus.Serialize(ptr);
		if (BreakBonusGrades != null)
		{
			int count4 = BreakBonusGrades.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				ptr[l] = (byte)BreakBonusGrades[l];
			}
			ptr += count4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = MaxObtainableNeili;
		ptr += 2;
		*(short*)ptr = ObtainedNeili;
		ptr += 2;
		if (SpecificGrids != null)
		{
			int num2 = SpecificGrids.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int m = 0; m < num2; m++)
			{
				ptr[m] = (byte)SpecificGrids[m];
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)GenericGrid;
		ptr++;
		*(short*)ptr = AddAttackDistanceForward;
		ptr += 2;
		*(short*)ptr = AddAttackDistanceBackward;
		ptr += 2;
		*(int*)ptr = HitValueStrength;
		ptr += 4;
		*(int*)ptr = HitValueTechnique;
		ptr += 4;
		*(int*)ptr = HitValueSpeed;
		ptr += 4;
		*(int*)ptr = HitValueMind;
		ptr += 4;
		*(int*)ptr = PenetrateValueOuter;
		ptr += 4;
		*(int*)ptr = PenetrateValueInner;
		ptr += 4;
		ptr += Poisons.Serialize(ptr);
		ptr += HitDistribution.Serialize(ptr);
		if (BodyPartWeights != null)
		{
			int count5 = BodyPartWeights.Count;
			Tester.Assert(count5 <= 65535);
			*(ushort*)ptr = (ushort)count5;
			ptr += 2;
			for (int n = 0; n < count5; n++)
			{
				((int*)ptr)[n] = BodyPartWeights[n];
			}
			ptr += 4 * count5;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)FullPowerCastTimes;
		ptr++;
		*(int*)ptr = JumpSpeed;
		ptr += 4;
		*(short*)ptr = AddMoveSpeed;
		ptr += 2;
		*(short*)ptr = AddPercentMoveSpeed;
		ptr += 2;
		*(int*)ptr = AddHitStrength;
		ptr += 4;
		*(int*)ptr = AddHitTechnique;
		ptr += 4;
		*(int*)ptr = AddHitSpeed;
		ptr += 4;
		*(int*)ptr = AddHitMind;
		ptr += 4;
		*(int*)ptr = AddInnerDef;
		ptr += 4;
		*(int*)ptr = AddOuterDef;
		ptr += 4;
		*(int*)ptr = AddAvoidStrength;
		ptr += 4;
		*(int*)ptr = AddAvoidTechnique;
		ptr += 4;
		*(int*)ptr = AddAvoidSpeed;
		ptr += 4;
		*(int*)ptr = AddAvoidMind;
		ptr += 4;
		*(int*)ptr = BouncePowerOuter;
		ptr += 4;
		*(int*)ptr = BouncePowerInner;
		ptr += 4;
		*(short*)ptr = BounceDistance;
		ptr += 2;
		*(int*)ptr = FightbackPower;
		ptr += 4;
		*(short*)ptr = EffectDuration;
		ptr += 2;
		*(short*)ptr = CostMobility;
		ptr += 2;
		*ptr = (byte)CostMobilityFontType;
		ptr++;
		if (CostTricks != null)
		{
			int count6 = CostTricks.Count;
			Tester.Assert(count6 <= 65535);
			*(ushort*)ptr = (ushort)count6;
			ptr += 2;
			for (int num3 = 0; num3 < count6; num3++)
			{
				ptr += CostTricks[num3].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CostTricksFontType != null)
		{
			int count7 = CostTricksFontType.Count;
			Tester.Assert(count7 <= 65535);
			*(ushort*)ptr = (ushort)count7;
			ptr += 2;
			for (int num4 = 0; num4 < count7; num4++)
			{
				ptr[num4] = (byte)CostTricksFontType[num4];
			}
			ptr += count7;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)CostBreath;
		ptr++;
		*ptr = (byte)CostBreathFontType;
		ptr++;
		*ptr = (byte)CostStance;
		ptr++;
		*ptr = (byte)CostStanceFontType;
		ptr++;
		ptr += SerializationHelper.Serialize<sbyte, sbyte>(ptr, CostNeiliAllocation);
		*ptr = (byte)CostNeiliAllocationFontType;
		ptr++;
		*ptr = (byte)CostWeaponDurabilityFontType;
		ptr++;
		*ptr = (byte)CostWugFontType;
		ptr++;
		*ptr = (byte)FiveElementDestTypeWhileLooping;
		ptr++;
		*ptr = (byte)FiveElementTransferTypeWhileLooping;
		ptr++;
		if (EffectData != null)
		{
			int count8 = EffectData.Count;
			Tester.Assert(count8 <= 65535);
			*(ushort*)ptr = (ushort)count8;
			ptr += 2;
			for (int num5 = 0; num5 < count8; num5++)
			{
				ptr += EffectData[num5].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharId = *(int*)ptr;
		ptr += 4;
		TemplateId = *(short*)ptr;
		ptr += 2;
		ReadingState = *(ushort*)ptr;
		ptr += 2;
		ActivationState = *(ushort*)ptr;
		ptr += 2;
		CanAffect = *ptr != 0;
		ptr++;
		Conflicting = *ptr != 0;
		ptr++;
		GridCount = (sbyte)(*ptr);
		ptr++;
		Power = *(short*)ptr;
		ptr += 2;
		MaxPower = *(short*)ptr;
		ptr += 2;
		RequirementsPower = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Requirements == null)
			{
				Requirements = new List<(int, int, int)>(num);
			}
			else
			{
				Requirements.Clear();
			}
			(int, int, int) item = default((int, int, int));
			for (int i = 0; i < num; i++)
			{
				ptr += SerializationHelper.Deserialize<int, int, int>(ptr, ref item);
				Requirements.Add(item);
			}
		}
		else
		{
			Requirements?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (BreakAddProperty == null)
			{
				BreakAddProperty = new List<(short, short, bool)>(num2);
			}
			else
			{
				BreakAddProperty.Clear();
			}
			(short, short, bool) item2 = default((short, short, bool));
			for (int j = 0; j < num2; j++)
			{
				ptr += SerializationHelper.Deserialize<short, short, bool>(ptr, ref item2);
				BreakAddProperty.Add(item2);
			}
		}
		else
		{
			BreakAddProperty?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (NeiliAllocationAddProperty == null)
			{
				NeiliAllocationAddProperty = new List<(short, int)>(num3);
			}
			else
			{
				NeiliAllocationAddProperty.Clear();
			}
			(short, int) item3 = default((short, int));
			for (int k = 0; k < num3; k++)
			{
				ptr += SerializationHelper.Deserialize<short, int>(ptr, ref item3);
				NeiliAllocationAddProperty.Add(item3);
			}
		}
		else
		{
			NeiliAllocationAddProperty?.Clear();
		}
		BreakPlateIndex = (sbyte)(*ptr);
		ptr++;
		EffectType = (sbyte)(*ptr);
		ptr++;
		Mastered = *ptr != 0;
		ptr++;
		PreviewMastered = *ptr != 0;
		ptr++;
		Revoked = *ptr != 0;
		ptr++;
		JumpThreshold = *(short*)ptr;
		ptr += 2;
		BaseInnerRatio = (sbyte)(*ptr);
		ptr++;
		InnerRatioChangeRange = (sbyte)(*ptr);
		ptr++;
		CurrInnerRatio = (sbyte)(*ptr);
		ptr++;
		ExpectInnerRatio = (sbyte)(*ptr);
		ptr++;
		NewUnderstandingNeedExp = *(int*)ptr;
		ptr += 4;
		BreakSuccess = *ptr != 0;
		ptr++;
		ptr += EffectDescription.Deserialize(ptr);
		ptr += DamageStepBonus.Deserialize(ptr);
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (BreakBonusGrades == null)
			{
				BreakBonusGrades = new List<sbyte>(num4);
			}
			else
			{
				BreakBonusGrades.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				BreakBonusGrades.Add((sbyte)ptr[l]);
			}
			ptr += (int)num4;
		}
		else
		{
			BreakBonusGrades?.Clear();
		}
		MaxObtainableNeili = *(short*)ptr;
		ptr += 2;
		ObtainedNeili = *(short*)ptr;
		ptr += 2;
		ushort num5 = *(ushort*)ptr;
		ptr += 2;
		if (num5 > 0)
		{
			if (SpecificGrids == null || SpecificGrids.Length != num5)
			{
				SpecificGrids = new sbyte[num5];
			}
			for (int m = 0; m < num5; m++)
			{
				SpecificGrids[m] = (sbyte)ptr[m];
			}
			ptr += (int)num5;
		}
		else
		{
			SpecificGrids = null;
		}
		GenericGrid = (sbyte)(*ptr);
		ptr++;
		AddAttackDistanceForward = *(short*)ptr;
		ptr += 2;
		AddAttackDistanceBackward = *(short*)ptr;
		ptr += 2;
		HitValueStrength = *(int*)ptr;
		ptr += 4;
		HitValueTechnique = *(int*)ptr;
		ptr += 4;
		HitValueSpeed = *(int*)ptr;
		ptr += 4;
		HitValueMind = *(int*)ptr;
		ptr += 4;
		PenetrateValueOuter = *(int*)ptr;
		ptr += 4;
		PenetrateValueInner = *(int*)ptr;
		ptr += 4;
		ptr += Poisons.Deserialize(ptr);
		ptr += HitDistribution.Deserialize(ptr);
		ushort num6 = *(ushort*)ptr;
		ptr += 2;
		if (num6 > 0)
		{
			if (BodyPartWeights == null)
			{
				BodyPartWeights = new List<int>(num6);
			}
			else
			{
				BodyPartWeights.Clear();
			}
			for (int n = 0; n < num6; n++)
			{
				BodyPartWeights.Add(((int*)ptr)[n]);
			}
			ptr += 4 * num6;
		}
		else
		{
			BodyPartWeights?.Clear();
		}
		FullPowerCastTimes = (sbyte)(*ptr);
		ptr++;
		JumpSpeed = *(int*)ptr;
		ptr += 4;
		AddMoveSpeed = *(short*)ptr;
		ptr += 2;
		AddPercentMoveSpeed = *(short*)ptr;
		ptr += 2;
		AddHitStrength = *(int*)ptr;
		ptr += 4;
		AddHitTechnique = *(int*)ptr;
		ptr += 4;
		AddHitSpeed = *(int*)ptr;
		ptr += 4;
		AddHitMind = *(int*)ptr;
		ptr += 4;
		AddInnerDef = *(int*)ptr;
		ptr += 4;
		AddOuterDef = *(int*)ptr;
		ptr += 4;
		AddAvoidStrength = *(int*)ptr;
		ptr += 4;
		AddAvoidTechnique = *(int*)ptr;
		ptr += 4;
		AddAvoidSpeed = *(int*)ptr;
		ptr += 4;
		AddAvoidMind = *(int*)ptr;
		ptr += 4;
		BouncePowerOuter = *(int*)ptr;
		ptr += 4;
		BouncePowerInner = *(int*)ptr;
		ptr += 4;
		BounceDistance = *(short*)ptr;
		ptr += 2;
		FightbackPower = *(int*)ptr;
		ptr += 4;
		EffectDuration = *(short*)ptr;
		ptr += 2;
		CostMobility = *(short*)ptr;
		ptr += 2;
		CostMobilityFontType = (sbyte)(*ptr);
		ptr++;
		ushort num7 = *(ushort*)ptr;
		ptr += 2;
		if (num7 > 0)
		{
			if (CostTricks == null)
			{
				CostTricks = new List<NeedTrick>(num7);
			}
			else
			{
				CostTricks.Clear();
			}
			for (int num8 = 0; num8 < num7; num8++)
			{
				NeedTrick item4 = default(NeedTrick);
				ptr += item4.Deserialize(ptr);
				CostTricks.Add(item4);
			}
		}
		else
		{
			CostTricks?.Clear();
		}
		ushort num9 = *(ushort*)ptr;
		ptr += 2;
		if (num9 > 0)
		{
			if (CostTricksFontType == null)
			{
				CostTricksFontType = new List<sbyte>(num9);
			}
			else
			{
				CostTricksFontType.Clear();
			}
			for (int num10 = 0; num10 < num9; num10++)
			{
				CostTricksFontType.Add((sbyte)ptr[num10]);
			}
			ptr += (int)num9;
		}
		else
		{
			CostTricksFontType?.Clear();
		}
		CostBreath = (sbyte)(*ptr);
		ptr++;
		CostBreathFontType = (sbyte)(*ptr);
		ptr++;
		CostStance = (sbyte)(*ptr);
		ptr++;
		CostStanceFontType = (sbyte)(*ptr);
		ptr++;
		ptr += SerializationHelper.Deserialize<sbyte, sbyte>(ptr, ref CostNeiliAllocation);
		CostNeiliAllocationFontType = (sbyte)(*ptr);
		ptr++;
		CostWeaponDurabilityFontType = (sbyte)(*ptr);
		ptr++;
		CostWugFontType = (sbyte)(*ptr);
		ptr++;
		FiveElementDestTypeWhileLooping = (sbyte)(*ptr);
		ptr++;
		FiveElementTransferTypeWhileLooping = (sbyte)(*ptr);
		ptr++;
		ushort num11 = *(ushort*)ptr;
		ptr += 2;
		if (num11 > 0)
		{
			if (EffectData == null)
			{
				EffectData = new List<CombatSkillEffectData>(num11);
			}
			else
			{
				EffectData.Clear();
			}
			for (int num12 = 0; num12 < num11; num12++)
			{
				CombatSkillEffectData item5 = default(CombatSkillEffectData);
				ptr += item5.Deserialize(ptr);
				EffectData.Add(item5);
			}
		}
		else
		{
			EffectData?.Clear();
		}
		int num13 = (int)(ptr - pData);
		if (num13 > 4)
		{
			return (num13 + 3) / 4 * 4;
		}
		return num13;
	}
}
