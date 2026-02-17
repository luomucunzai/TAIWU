using System;
using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat
{
	// Token: 0x020006EB RID: 1771
	public class TeammateCommandCheckerPushOrPullIntoDanger : TeammateCommandCheckerBase
	{
		// Token: 0x1700041D RID: 1053
		// (get) Token: 0x060067A8 RID: 26536 RVA: 0x003B21BF File Offset: 0x003B03BF
		protected override bool CheckTeammateBoth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067A9 RID: 26537 RVA: 0x003B21C2 File Offset: 0x003B03C2
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!context.CurrChar.IsAlly, false);
			bool flag = enemyChar == null;
			if (flag)
			{
				yield break;
			}
			bool flag2 = DomainManager.Combat.InAttackRange(enemyChar);
			if (flag2)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			ValueTuple<byte, byte> distanceRange = DomainManager.Combat.GetDistanceRange();
			short distance = DomainManager.Combat.GetCurrentDistance();
			bool flag3 = distance <= (short)distanceRange.Item1 || distance >= (short)distanceRange.Item2;
			if (flag3)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			OuterAndInnerShorts attackRange = enemyChar.GetAttackRange();
			bool flag4 = distance - attackRange.Inner > 10;
			if (flag4)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			bool flag5 = attackRange.Outer - distance > 10;
			if (flag5)
			{
				yield return ETeammateCommandBanReason.Negative;
			}
			yield break;
		}
	}
}
