using System;

namespace GameData.Domains.Combat
{
	// Token: 0x020006B8 RID: 1720
	public enum ECombatReserveType : sbyte
	{
		// Token: 0x04001BA5 RID: 7077
		Invalid,
		// Token: 0x04001BA6 RID: 7078
		Skill,
		// Token: 0x04001BA7 RID: 7079
		ChangeTrick,
		// Token: 0x04001BA8 RID: 7080
		ChangeWeapon,
		// Token: 0x04001BA9 RID: 7081
		UnlockAttack,
		// Token: 0x04001BAA RID: 7082
		UseItem,
		// Token: 0x04001BAB RID: 7083
		OtherAction,
		// Token: 0x04001BAC RID: 7084
		TeammateCommand
	}
}
