using System.Collections.Generic;

namespace GameData.Domains.Combat;

public class TeammateCommandCheckerReduceHitAndAvoid : TeammateCommandCheckerBase
{
	protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
	{
		yield break;
	}
}
