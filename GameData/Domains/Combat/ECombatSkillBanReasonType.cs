using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BB RID: 1723
	public enum ECombatSkillBanReasonType : sbyte
	{
		// Token: 0x04001BB4 RID: 7092
		None = -1,
		// Token: 0x04001BB5 RID: 7093
		Undefined,
		// Token: 0x04001BB6 RID: 7094
		StanceNotEnough,
		// Token: 0x04001BB7 RID: 7095
		BreathNotEnough,
		// Token: 0x04001BB8 RID: 7096
		MobilityNotEnough,
		// Token: 0x04001BB9 RID: 7097
		TrickNotEnough,
		// Token: 0x04001BBA RID: 7098
		WugNotEnough,
		// Token: 0x04001BBB RID: 7099
		NeiliAllocationNotEnough,
		// Token: 0x04001BBC RID: 7100
		WeaponTrickMismatch,
		// Token: 0x04001BBD RID: 7101
		WeaponDestroyed,
		// Token: 0x04001BBE RID: 7102
		BodyPartBroken,
		// Token: 0x04001BBF RID: 7103
		SpecialEffectBan,
		// Token: 0x04001BC0 RID: 7104
		CombatConfigBan,
		// Token: 0x04001BC1 RID: 7105
		Silencing
	}
}
