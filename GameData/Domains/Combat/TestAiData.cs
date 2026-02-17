using System;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x02000706 RID: 1798
	public class TestAiData : ISerializableGameData
	{
		// Token: 0x060067FE RID: 26622 RVA: 0x003B2DE6 File Offset: 0x003B0FE6
		public TestAiData()
		{
		}

		// Token: 0x060067FF RID: 26623 RVA: 0x003B2DF0 File Offset: 0x003B0FF0
		public TestAiData(TestAiData other)
		{
			this.CurrHazardValue = other.CurrHazardValue;
			this.MaxHazardValue = other.MaxHazardValue;
			this.IsHazard = other.IsHazard;
			this.HasAttackPlan = other.HasAttackPlan;
			this.AttackPlanTargetSkill = other.AttackPlanTargetSkill;
			this.AttackPlanTargetWeapon = other.AttackPlanTargetWeapon;
			this.AttackPlanTempSkill = other.AttackPlanTempSkill;
			this.AttackPlanTempWeapon = other.AttackPlanTempWeapon;
			this.AttackPlanCurrFrame = other.AttackPlanCurrFrame;
			this.AttackPlanMaxFrame = other.AttackPlanMaxFrame;
			this.AttackPlanTargetDistance = other.AttackPlanTargetDistance;
			this.HasDefendPlan = other.HasDefendPlan;
			this.DefendSkillDelayFrameCount = other.DefendSkillDelayFrameCount;
			this.DefendPlanSkillDefendType = other.DefendPlanSkillDefendType;
			this.ContinuousInjuryCount = other.ContinuousInjuryCount;
			this.ContinuousInjuryNeedDefendCount = other.ContinuousInjuryNeedDefendCount;
			this.ContinuousInjuryFrameCount = other.ContinuousInjuryFrameCount;
			this.ContinuousInjuryMaxFrame = other.ContinuousInjuryMaxFrame;
			this.DefendPlanTargetDistance = other.DefendPlanTargetDistance;
			this.HasSurvivePlan = other.HasSurvivePlan;
			this.CanFlee = other.CanFlee;
			this.SurvivePlanCurrDodgeFrame = other.SurvivePlanCurrDodgeFrame;
			this.SurvivePlanMaxDodgeFrame = other.SurvivePlanMaxDodgeFrame;
			this.SurvivePlanCurrFleeFrame = other.SurvivePlanCurrFleeFrame;
			this.SurvivePlanMaxFleeFrame = other.SurvivePlanMaxFleeFrame;
			this.SurvivePlanTargetDistance = other.SurvivePlanTargetDistance;
		}

		// Token: 0x06006800 RID: 26624 RVA: 0x003B2F40 File Offset: 0x003B1140
		public void Assign(TestAiData other)
		{
			this.CurrHazardValue = other.CurrHazardValue;
			this.MaxHazardValue = other.MaxHazardValue;
			this.IsHazard = other.IsHazard;
			this.HasAttackPlan = other.HasAttackPlan;
			this.AttackPlanTargetSkill = other.AttackPlanTargetSkill;
			this.AttackPlanTargetWeapon = other.AttackPlanTargetWeapon;
			this.AttackPlanTempSkill = other.AttackPlanTempSkill;
			this.AttackPlanTempWeapon = other.AttackPlanTempWeapon;
			this.AttackPlanCurrFrame = other.AttackPlanCurrFrame;
			this.AttackPlanMaxFrame = other.AttackPlanMaxFrame;
			this.AttackPlanTargetDistance = other.AttackPlanTargetDistance;
			this.HasDefendPlan = other.HasDefendPlan;
			this.DefendSkillDelayFrameCount = other.DefendSkillDelayFrameCount;
			this.DefendPlanSkillDefendType = other.DefendPlanSkillDefendType;
			this.ContinuousInjuryCount = other.ContinuousInjuryCount;
			this.ContinuousInjuryNeedDefendCount = other.ContinuousInjuryNeedDefendCount;
			this.ContinuousInjuryFrameCount = other.ContinuousInjuryFrameCount;
			this.ContinuousInjuryMaxFrame = other.ContinuousInjuryMaxFrame;
			this.DefendPlanTargetDistance = other.DefendPlanTargetDistance;
			this.HasSurvivePlan = other.HasSurvivePlan;
			this.CanFlee = other.CanFlee;
			this.SurvivePlanCurrDodgeFrame = other.SurvivePlanCurrDodgeFrame;
			this.SurvivePlanMaxDodgeFrame = other.SurvivePlanMaxDodgeFrame;
			this.SurvivePlanCurrFleeFrame = other.SurvivePlanCurrFleeFrame;
			this.SurvivePlanMaxFleeFrame = other.SurvivePlanMaxFleeFrame;
			this.SurvivePlanTargetDistance = other.SurvivePlanTargetDistance;
		}

		// Token: 0x06006801 RID: 26625 RVA: 0x003B3088 File Offset: 0x003B1288
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06006802 RID: 26626 RVA: 0x003B309C File Offset: 0x003B129C
		public int GetSerializedSize()
		{
			int totalSize = 72;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006803 RID: 26627 RVA: 0x003B30C4 File Offset: 0x003B12C4
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = this.CurrHazardValue;
			byte* pCurrData = pData + 4;
			*(int*)pCurrData = this.MaxHazardValue;
			pCurrData += 4;
			*pCurrData = (this.IsHazard ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.HasAttackPlan ? 1 : 0);
			pCurrData++;
			*(short*)pCurrData = this.AttackPlanTargetSkill;
			pCurrData += 2;
			*(short*)pCurrData = this.AttackPlanTargetWeapon;
			pCurrData += 2;
			*(short*)pCurrData = this.AttackPlanTempSkill;
			pCurrData += 2;
			*(short*)pCurrData = this.AttackPlanTempWeapon;
			pCurrData += 2;
			*(int*)pCurrData = this.AttackPlanCurrFrame;
			pCurrData += 4;
			*(int*)pCurrData = this.AttackPlanMaxFrame;
			pCurrData += 4;
			*(short*)pCurrData = this.AttackPlanTargetDistance;
			pCurrData += 2;
			*pCurrData = (this.HasDefendPlan ? 1 : 0);
			pCurrData++;
			*(int*)pCurrData = this.DefendSkillDelayFrameCount;
			pCurrData += 4;
			*pCurrData = (byte)this.DefendPlanSkillDefendType;
			pCurrData++;
			*(int*)pCurrData = this.ContinuousInjuryCount;
			pCurrData += 4;
			*(int*)pCurrData = this.ContinuousInjuryNeedDefendCount;
			pCurrData += 4;
			*(int*)pCurrData = this.ContinuousInjuryFrameCount;
			pCurrData += 4;
			*(int*)pCurrData = this.ContinuousInjuryMaxFrame;
			pCurrData += 4;
			*(short*)pCurrData = this.DefendPlanTargetDistance;
			pCurrData += 2;
			*pCurrData = (this.HasSurvivePlan ? 1 : 0);
			pCurrData++;
			*pCurrData = (this.CanFlee ? 1 : 0);
			pCurrData++;
			*(int*)pCurrData = this.SurvivePlanCurrDodgeFrame;
			pCurrData += 4;
			*(int*)pCurrData = this.SurvivePlanMaxDodgeFrame;
			pCurrData += 4;
			*(int*)pCurrData = this.SurvivePlanCurrFleeFrame;
			pCurrData += 4;
			*(int*)pCurrData = this.SurvivePlanMaxFleeFrame;
			pCurrData += 4;
			*(short*)pCurrData = this.SurvivePlanTargetDistance;
			pCurrData += 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006804 RID: 26628 RVA: 0x003B3228 File Offset: 0x003B1428
		public unsafe int Deserialize(byte* pData)
		{
			this.CurrHazardValue = *(int*)pData;
			byte* pCurrData = pData + 4;
			this.MaxHazardValue = *(int*)pCurrData;
			pCurrData += 4;
			this.IsHazard = (*pCurrData != 0);
			pCurrData++;
			this.HasAttackPlan = (*pCurrData != 0);
			pCurrData++;
			this.AttackPlanTargetSkill = *(short*)pCurrData;
			pCurrData += 2;
			this.AttackPlanTargetWeapon = *(short*)pCurrData;
			pCurrData += 2;
			this.AttackPlanTempSkill = *(short*)pCurrData;
			pCurrData += 2;
			this.AttackPlanTempWeapon = *(short*)pCurrData;
			pCurrData += 2;
			this.AttackPlanCurrFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.AttackPlanMaxFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.AttackPlanTargetDistance = *(short*)pCurrData;
			pCurrData += 2;
			this.HasDefendPlan = (*pCurrData != 0);
			pCurrData++;
			this.DefendSkillDelayFrameCount = *(int*)pCurrData;
			pCurrData += 4;
			this.DefendPlanSkillDefendType = *(sbyte*)pCurrData;
			pCurrData++;
			this.ContinuousInjuryCount = *(int*)pCurrData;
			pCurrData += 4;
			this.ContinuousInjuryNeedDefendCount = *(int*)pCurrData;
			pCurrData += 4;
			this.ContinuousInjuryFrameCount = *(int*)pCurrData;
			pCurrData += 4;
			this.ContinuousInjuryMaxFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.DefendPlanTargetDistance = *(short*)pCurrData;
			pCurrData += 2;
			this.HasSurvivePlan = (*pCurrData != 0);
			pCurrData++;
			this.CanFlee = (*pCurrData != 0);
			pCurrData++;
			this.SurvivePlanCurrDodgeFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.SurvivePlanMaxDodgeFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.SurvivePlanCurrFleeFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.SurvivePlanMaxFleeFrame = *(int*)pCurrData;
			pCurrData += 4;
			this.SurvivePlanTargetDistance = *(short*)pCurrData;
			pCurrData += 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C5D RID: 7261
		[SerializableGameDataField]
		public int CurrHazardValue;

		// Token: 0x04001C5E RID: 7262
		[SerializableGameDataField]
		public int MaxHazardValue;

		// Token: 0x04001C5F RID: 7263
		[SerializableGameDataField]
		public bool IsHazard;

		// Token: 0x04001C60 RID: 7264
		[SerializableGameDataField]
		public bool HasAttackPlan;

		// Token: 0x04001C61 RID: 7265
		[SerializableGameDataField]
		public short AttackPlanTargetSkill;

		// Token: 0x04001C62 RID: 7266
		[SerializableGameDataField]
		public short AttackPlanTargetWeapon;

		// Token: 0x04001C63 RID: 7267
		[SerializableGameDataField]
		public short AttackPlanTempSkill;

		// Token: 0x04001C64 RID: 7268
		[SerializableGameDataField]
		public short AttackPlanTempWeapon;

		// Token: 0x04001C65 RID: 7269
		[SerializableGameDataField]
		public int AttackPlanCurrFrame;

		// Token: 0x04001C66 RID: 7270
		[SerializableGameDataField]
		public int AttackPlanMaxFrame;

		// Token: 0x04001C67 RID: 7271
		[SerializableGameDataField]
		public short AttackPlanTargetDistance;

		// Token: 0x04001C68 RID: 7272
		[SerializableGameDataField]
		public bool HasDefendPlan;

		// Token: 0x04001C69 RID: 7273
		[SerializableGameDataField]
		public int DefendSkillDelayFrameCount;

		// Token: 0x04001C6A RID: 7274
		[SerializableGameDataField]
		public sbyte DefendPlanSkillDefendType;

		// Token: 0x04001C6B RID: 7275
		[SerializableGameDataField]
		public int ContinuousInjuryCount;

		// Token: 0x04001C6C RID: 7276
		[SerializableGameDataField]
		public int ContinuousInjuryNeedDefendCount;

		// Token: 0x04001C6D RID: 7277
		[SerializableGameDataField]
		public int ContinuousInjuryFrameCount;

		// Token: 0x04001C6E RID: 7278
		[SerializableGameDataField]
		public int ContinuousInjuryMaxFrame;

		// Token: 0x04001C6F RID: 7279
		[SerializableGameDataField]
		public short DefendPlanTargetDistance;

		// Token: 0x04001C70 RID: 7280
		[SerializableGameDataField]
		public bool HasSurvivePlan;

		// Token: 0x04001C71 RID: 7281
		[SerializableGameDataField]
		public bool CanFlee;

		// Token: 0x04001C72 RID: 7282
		[SerializableGameDataField]
		public int SurvivePlanCurrDodgeFrame;

		// Token: 0x04001C73 RID: 7283
		[SerializableGameDataField]
		public int SurvivePlanMaxDodgeFrame;

		// Token: 0x04001C74 RID: 7284
		[SerializableGameDataField]
		public int SurvivePlanCurrFleeFrame;

		// Token: 0x04001C75 RID: 7285
		[SerializableGameDataField]
		public int SurvivePlanMaxFleeFrame;

		// Token: 0x04001C76 RID: 7286
		[SerializableGameDataField]
		public short SurvivePlanTargetDistance;
	}
}
