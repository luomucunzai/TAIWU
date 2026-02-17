using System;
using GameData.Common;

namespace GameData.Domains.Combat.MixPoison
{
	// Token: 0x0200070D RID: 1805
	// (Invoke) Token: 0x06006830 RID: 26672
	public delegate bool MixPoisonEffectDelegate(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList);
}
