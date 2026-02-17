using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D5 RID: 1749
	public enum ETeammateCommandBanReason : sbyte
	{
		// Token: 0x04001C25 RID: 7205
		Negative = -2,
		// Token: 0x04001C26 RID: 7206
		Internal,
		// Token: 0x04001C27 RID: 7207
		CommonNotMain,
		// Token: 0x04001C28 RID: 7208
		CommonCd,
		// Token: 0x04001C29 RID: 7209
		CommonFallen,
		// Token: 0x04001C2A RID: 7210
		CommonStop,
		// Token: 0x04001C2B RID: 7211
		CommonConflict,
		// Token: 0x04001C2C RID: 7212
		AccelerateNotPreparing,
		// Token: 0x04001C2D RID: 7213
		PushInEdge,
		// Token: 0x04001C2E RID: 7214
		PullInEdge,
		// Token: 0x04001C2F RID: 7215
		HealInjuryNonInjury,
		// Token: 0x04001C30 RID: 7216
		HealInjuryAttainmentLack,
		// Token: 0x04001C31 RID: 7217
		HealInjuryCountLack,
		// Token: 0x04001C32 RID: 7218
		HealInjuryHerbLack,
		// Token: 0x04001C33 RID: 7219
		HealPoisonNonPoison,
		// Token: 0x04001C34 RID: 7220
		HealPoisonAttainmentLack,
		// Token: 0x04001C35 RID: 7221
		HealPoisonCountLack,
		// Token: 0x04001C36 RID: 7222
		HealPoisonHerbLack,
		// Token: 0x04001C37 RID: 7223
		HealFlawNonFlaw,
		// Token: 0x04001C38 RID: 7224
		HealAcupointNonAcupoint,
		// Token: 0x04001C39 RID: 7225
		TransferInjuryNonInjury,
		// Token: 0x04001C3A RID: 7226
		TransferNeiliAllocationLack,
		// Token: 0x04001C3B RID: 7227
		AttackNonTrick,
		// Token: 0x04001C3C RID: 7228
		AttackSkillNonSkill,
		// Token: 0x04001C3D RID: 7229
		AttackSkillBanned,
		// Token: 0x04001C3E RID: 7230
		DefendSkillNonSkill,
		// Token: 0x04001C3F RID: 7231
		DefendSkillBanned,
		// Token: 0x04001C40 RID: 7232
		AddUnlockAttackValueFull,
		// Token: 0x04001C41 RID: 7233
		TransferManyMarkNonAnyMark,
		// Token: 0x04001C42 RID: 7234
		RepairItemNonAnyRepairable
	}
}
