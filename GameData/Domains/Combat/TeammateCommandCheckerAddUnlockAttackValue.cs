using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F6 RID: 1782
	public class TeammateCommandCheckerAddUnlockAttackValue : TeammateCommandCheckerBase
	{
		// Token: 0x1700041E RID: 1054
		// (get) Token: 0x060067BC RID: 26556 RVA: 0x003B22DD File Offset: 0x003B04DD
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067BD RID: 26557 RVA: 0x003B22E0 File Offset: 0x003B04E0
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool anyCanAddUnlockValue = false;
			ItemKey[] weapons = context.CurrChar.GetWeapons();
			List<int> unlockValues = context.CurrChar.GetUnlockPrepareValue();
			int num;
			for (int i = 0; i < weapons.Length; i = num + 1)
			{
				bool flag = context.CurrChar.CanUnlockAttackByConfig(i) && unlockValues[i] < GlobalConfig.Instance.UnlockAttackUnit;
				if (flag)
				{
					anyCanAddUnlockValue = true;
				}
				num = i;
			}
			bool flag2 = !anyCanAddUnlockValue;
			if (flag2)
			{
				yield return ETeammateCommandBanReason.AddUnlockAttackValueFull;
			}
			yield break;
		}
	}
}
