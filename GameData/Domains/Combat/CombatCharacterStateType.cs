using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006A8 RID: 1704
	public enum CombatCharacterStateType : sbyte
	{
		// Token: 0x04001ACE RID: 6862
		Invalid = -1,
		// Token: 0x04001ACF RID: 6863
		Idle,
		// Token: 0x04001AD0 RID: 6864
		SelectChangeTrick,
		// Token: 0x04001AD1 RID: 6865
		PrepareAttack,
		// Token: 0x04001AD2 RID: 6866
		BreakAttack,
		// Token: 0x04001AD3 RID: 6867
		Attack,
		// Token: 0x04001AD4 RID: 6868
		PrepareUnlockAttack,
		// Token: 0x04001AD5 RID: 6869
		UnlockAttack,
		// Token: 0x04001AD6 RID: 6870
		RawCreate,
		// Token: 0x04001AD7 RID: 6871
		PrepareSkill,
		// Token: 0x04001AD8 RID: 6872
		CastSkill,
		// Token: 0x04001AD9 RID: 6873
		PrepareOtherAction,
		// Token: 0x04001ADA RID: 6874
		PrepareUseItem,
		// Token: 0x04001ADB RID: 6875
		UseItem,
		// Token: 0x04001ADC RID: 6876
		SelectMercy,
		// Token: 0x04001ADD RID: 6877
		DelaySettlement,
		// Token: 0x04001ADE RID: 6878
		ChangeCharacter,
		// Token: 0x04001ADF RID: 6879
		TeammateCommand,
		// Token: 0x04001AE0 RID: 6880
		ChangeBossPhase,
		// Token: 0x04001AE1 RID: 6881
		JumpMove,
		// Token: 0x04001AE2 RID: 6882
		AnimalAttack,
		// Token: 0x04001AE3 RID: 6883
		SpecialShow
	}
}
