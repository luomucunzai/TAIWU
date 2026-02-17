using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DD RID: 1757
	public class TeammateCommandCheckerPull : TeammateCommandCheckerBase
	{
		// Token: 0x17000413 RID: 1043
		// (get) Token: 0x06006781 RID: 26497 RVA: 0x003B1FD0 File Offset: 0x003B01D0
		protected override bool CheckTeammateBoth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006782 RID: 26498 RVA: 0x003B1FD3 File Offset: 0x003B01D3
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			ValueTuple<byte, byte> distanceRange = DomainManager.Combat.GetDistanceRange();
			bool flag = DomainManager.Combat.GetCurrentDistance() >= (short)distanceRange.Item2;
			if (flag)
			{
				yield return ETeammateCommandBanReason.PullInEdge;
			}
			yield break;
		}
	}
}
