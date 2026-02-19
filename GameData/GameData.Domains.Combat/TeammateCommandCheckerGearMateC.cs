using System.Collections.Generic;

namespace GameData.Domains.Combat;

public class TeammateCommandCheckerGearMateC : TeammateCommandCheckerBase
{
	protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
	{
		yield break;
	}
}
