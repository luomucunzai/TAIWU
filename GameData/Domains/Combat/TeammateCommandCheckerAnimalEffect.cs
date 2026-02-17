using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F2 RID: 1778
	public class TeammateCommandCheckerAnimalEffect : TeammateCommandCheckerBase
	{
		// Token: 0x060067B6 RID: 26550 RVA: 0x003B228B File Offset: 0x003B048B
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			bool flag = context.TeammateChar.AnimalConfig == null || context.TeammateChar.AnimalConfig.CarrierId < 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.Internal;
			}
			yield break;
		}
	}
}
