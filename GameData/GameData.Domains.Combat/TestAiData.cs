using GameData.Serializer;

namespace GameData.Domains.Combat;

public class TestAiData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CurrHazardValue;

	[SerializableGameDataField]
	public int MaxHazardValue;

	[SerializableGameDataField]
	public bool IsHazard;

	[SerializableGameDataField]
	public bool HasAttackPlan;

	[SerializableGameDataField]
	public short AttackPlanTargetSkill;

	[SerializableGameDataField]
	public short AttackPlanTargetWeapon;

	[SerializableGameDataField]
	public short AttackPlanTempSkill;

	[SerializableGameDataField]
	public short AttackPlanTempWeapon;

	[SerializableGameDataField]
	public int AttackPlanCurrFrame;

	[SerializableGameDataField]
	public int AttackPlanMaxFrame;

	[SerializableGameDataField]
	public short AttackPlanTargetDistance;

	[SerializableGameDataField]
	public bool HasDefendPlan;

	[SerializableGameDataField]
	public int DefendSkillDelayFrameCount;

	[SerializableGameDataField]
	public sbyte DefendPlanSkillDefendType;

	[SerializableGameDataField]
	public int ContinuousInjuryCount;

	[SerializableGameDataField]
	public int ContinuousInjuryNeedDefendCount;

	[SerializableGameDataField]
	public int ContinuousInjuryFrameCount;

	[SerializableGameDataField]
	public int ContinuousInjuryMaxFrame;

	[SerializableGameDataField]
	public short DefendPlanTargetDistance;

	[SerializableGameDataField]
	public bool HasSurvivePlan;

	[SerializableGameDataField]
	public bool CanFlee;

	[SerializableGameDataField]
	public int SurvivePlanCurrDodgeFrame;

	[SerializableGameDataField]
	public int SurvivePlanMaxDodgeFrame;

	[SerializableGameDataField]
	public int SurvivePlanCurrFleeFrame;

	[SerializableGameDataField]
	public int SurvivePlanMaxFleeFrame;

	[SerializableGameDataField]
	public short SurvivePlanTargetDistance;

	public TestAiData()
	{
	}

	public TestAiData(TestAiData other)
	{
		CurrHazardValue = other.CurrHazardValue;
		MaxHazardValue = other.MaxHazardValue;
		IsHazard = other.IsHazard;
		HasAttackPlan = other.HasAttackPlan;
		AttackPlanTargetSkill = other.AttackPlanTargetSkill;
		AttackPlanTargetWeapon = other.AttackPlanTargetWeapon;
		AttackPlanTempSkill = other.AttackPlanTempSkill;
		AttackPlanTempWeapon = other.AttackPlanTempWeapon;
		AttackPlanCurrFrame = other.AttackPlanCurrFrame;
		AttackPlanMaxFrame = other.AttackPlanMaxFrame;
		AttackPlanTargetDistance = other.AttackPlanTargetDistance;
		HasDefendPlan = other.HasDefendPlan;
		DefendSkillDelayFrameCount = other.DefendSkillDelayFrameCount;
		DefendPlanSkillDefendType = other.DefendPlanSkillDefendType;
		ContinuousInjuryCount = other.ContinuousInjuryCount;
		ContinuousInjuryNeedDefendCount = other.ContinuousInjuryNeedDefendCount;
		ContinuousInjuryFrameCount = other.ContinuousInjuryFrameCount;
		ContinuousInjuryMaxFrame = other.ContinuousInjuryMaxFrame;
		DefendPlanTargetDistance = other.DefendPlanTargetDistance;
		HasSurvivePlan = other.HasSurvivePlan;
		CanFlee = other.CanFlee;
		SurvivePlanCurrDodgeFrame = other.SurvivePlanCurrDodgeFrame;
		SurvivePlanMaxDodgeFrame = other.SurvivePlanMaxDodgeFrame;
		SurvivePlanCurrFleeFrame = other.SurvivePlanCurrFleeFrame;
		SurvivePlanMaxFleeFrame = other.SurvivePlanMaxFleeFrame;
		SurvivePlanTargetDistance = other.SurvivePlanTargetDistance;
	}

	public void Assign(TestAiData other)
	{
		CurrHazardValue = other.CurrHazardValue;
		MaxHazardValue = other.MaxHazardValue;
		IsHazard = other.IsHazard;
		HasAttackPlan = other.HasAttackPlan;
		AttackPlanTargetSkill = other.AttackPlanTargetSkill;
		AttackPlanTargetWeapon = other.AttackPlanTargetWeapon;
		AttackPlanTempSkill = other.AttackPlanTempSkill;
		AttackPlanTempWeapon = other.AttackPlanTempWeapon;
		AttackPlanCurrFrame = other.AttackPlanCurrFrame;
		AttackPlanMaxFrame = other.AttackPlanMaxFrame;
		AttackPlanTargetDistance = other.AttackPlanTargetDistance;
		HasDefendPlan = other.HasDefendPlan;
		DefendSkillDelayFrameCount = other.DefendSkillDelayFrameCount;
		DefendPlanSkillDefendType = other.DefendPlanSkillDefendType;
		ContinuousInjuryCount = other.ContinuousInjuryCount;
		ContinuousInjuryNeedDefendCount = other.ContinuousInjuryNeedDefendCount;
		ContinuousInjuryFrameCount = other.ContinuousInjuryFrameCount;
		ContinuousInjuryMaxFrame = other.ContinuousInjuryMaxFrame;
		DefendPlanTargetDistance = other.DefendPlanTargetDistance;
		HasSurvivePlan = other.HasSurvivePlan;
		CanFlee = other.CanFlee;
		SurvivePlanCurrDodgeFrame = other.SurvivePlanCurrDodgeFrame;
		SurvivePlanMaxDodgeFrame = other.SurvivePlanMaxDodgeFrame;
		SurvivePlanCurrFleeFrame = other.SurvivePlanCurrFleeFrame;
		SurvivePlanMaxFleeFrame = other.SurvivePlanMaxFleeFrame;
		SurvivePlanTargetDistance = other.SurvivePlanTargetDistance;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 72;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CurrHazardValue;
		ptr += 4;
		*(int*)ptr = MaxHazardValue;
		ptr += 4;
		*ptr = (IsHazard ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (HasAttackPlan ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = AttackPlanTargetSkill;
		ptr += 2;
		*(short*)ptr = AttackPlanTargetWeapon;
		ptr += 2;
		*(short*)ptr = AttackPlanTempSkill;
		ptr += 2;
		*(short*)ptr = AttackPlanTempWeapon;
		ptr += 2;
		*(int*)ptr = AttackPlanCurrFrame;
		ptr += 4;
		*(int*)ptr = AttackPlanMaxFrame;
		ptr += 4;
		*(short*)ptr = AttackPlanTargetDistance;
		ptr += 2;
		*ptr = (HasDefendPlan ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = DefendSkillDelayFrameCount;
		ptr += 4;
		*ptr = (byte)DefendPlanSkillDefendType;
		ptr++;
		*(int*)ptr = ContinuousInjuryCount;
		ptr += 4;
		*(int*)ptr = ContinuousInjuryNeedDefendCount;
		ptr += 4;
		*(int*)ptr = ContinuousInjuryFrameCount;
		ptr += 4;
		*(int*)ptr = ContinuousInjuryMaxFrame;
		ptr += 4;
		*(short*)ptr = DefendPlanTargetDistance;
		ptr += 2;
		*ptr = (HasSurvivePlan ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (CanFlee ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = SurvivePlanCurrDodgeFrame;
		ptr += 4;
		*(int*)ptr = SurvivePlanMaxDodgeFrame;
		ptr += 4;
		*(int*)ptr = SurvivePlanCurrFleeFrame;
		ptr += 4;
		*(int*)ptr = SurvivePlanMaxFleeFrame;
		ptr += 4;
		*(short*)ptr = SurvivePlanTargetDistance;
		ptr += 2;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CurrHazardValue = *(int*)ptr;
		ptr += 4;
		MaxHazardValue = *(int*)ptr;
		ptr += 4;
		IsHazard = *ptr != 0;
		ptr++;
		HasAttackPlan = *ptr != 0;
		ptr++;
		AttackPlanTargetSkill = *(short*)ptr;
		ptr += 2;
		AttackPlanTargetWeapon = *(short*)ptr;
		ptr += 2;
		AttackPlanTempSkill = *(short*)ptr;
		ptr += 2;
		AttackPlanTempWeapon = *(short*)ptr;
		ptr += 2;
		AttackPlanCurrFrame = *(int*)ptr;
		ptr += 4;
		AttackPlanMaxFrame = *(int*)ptr;
		ptr += 4;
		AttackPlanTargetDistance = *(short*)ptr;
		ptr += 2;
		HasDefendPlan = *ptr != 0;
		ptr++;
		DefendSkillDelayFrameCount = *(int*)ptr;
		ptr += 4;
		DefendPlanSkillDefendType = (sbyte)(*ptr);
		ptr++;
		ContinuousInjuryCount = *(int*)ptr;
		ptr += 4;
		ContinuousInjuryNeedDefendCount = *(int*)ptr;
		ptr += 4;
		ContinuousInjuryFrameCount = *(int*)ptr;
		ptr += 4;
		ContinuousInjuryMaxFrame = *(int*)ptr;
		ptr += 4;
		DefendPlanTargetDistance = *(short*)ptr;
		ptr += 2;
		HasSurvivePlan = *ptr != 0;
		ptr++;
		CanFlee = *ptr != 0;
		ptr++;
		SurvivePlanCurrDodgeFrame = *(int*)ptr;
		ptr += 4;
		SurvivePlanMaxDodgeFrame = *(int*)ptr;
		ptr += 4;
		SurvivePlanCurrFleeFrame = *(int*)ptr;
		ptr += 4;
		SurvivePlanMaxFleeFrame = *(int*)ptr;
		ptr += 4;
		SurvivePlanTargetDistance = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
