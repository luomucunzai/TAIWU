using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006DC RID: 1756
	public class TeammateCommandCheckerPush : TeammateCommandCheckerBase
	{
		// Token: 0x17000412 RID: 1042
		// (get) Token: 0x0600677E RID: 26494 RVA: 0x003B1FAD File Offset: 0x003B01AD
		protected override bool CheckTeammateBoth
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600677F RID: 26495 RVA: 0x003B1FB0 File Offset: 0x003B01B0
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			ValueTuple<byte, byte> distanceRange = DomainManager.Combat.GetDistanceRange();
			bool flag = DomainManager.Combat.GetCurrentDistance() <= (short)distanceRange.Item1;
			if (flag)
			{
				yield return ETeammateCommandBanReason.PushInEdge;
			}
			yield break;
		}
	}
}
