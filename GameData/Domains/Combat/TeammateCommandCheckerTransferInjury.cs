using System;
using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat
{
	// Token: 0x020006E9 RID: 1769
	public class TeammateCommandCheckerTransferInjury : TeammateCommandCheckerBase
	{
		// Token: 0x1700041C RID: 1052
		// (get) Token: 0x060067A2 RID: 26530 RVA: 0x003B216B File Offset: 0x003B036B
		protected override bool CheckTeammateBefore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067A3 RID: 26531 RVA: 0x003B216E File Offset: 0x003B036E
		public override IEnumerable<ETeammateCommandBanReason> Check(int index, TeammateCommandCheckerContext context)
		{
			bool flag = index != 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.Internal;
				yield break;
			}
			foreach (ETeammateCommandBanReason banReason in base.Check(index, context))
			{
				yield return banReason;
			}
			IEnumerator<ETeammateCommandBanReason> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060067A4 RID: 26532 RVA: 0x003B218C File Offset: 0x003B038C
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			DefeatMarkCollection defeatMarkCollection = context.CurrChar.GetDefeatMarkCollection();
			bool flag = defeatMarkCollection.FatalDamageMarkCount > 0;
			if (flag)
			{
				yield break;
			}
			Injuries mainCharInjuries = context.CurrChar.GetInjuries();
			Injuries teammateInjuries = context.TeammateChar.GetInjuries();
			sbyte b;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart = b + 1)
			{
				sbyte mainCharInjuryCount = mainCharInjuries.Get(bodyPart, context.CurrChar.TransferInjuryCommandIsInner);
				sbyte teammateCharInjuryCount = teammateInjuries.Get(bodyPart, context.CurrChar.TransferInjuryCommandIsInner);
				bool flag2 = Math.Min((int)mainCharInjuryCount, (int)(6 - teammateCharInjuryCount)) > 0;
				if (flag2)
				{
					yield break;
				}
				b = bodyPart;
			}
			yield return ETeammateCommandBanReason.TransferInjuryNonInjury;
			yield break;
		}
	}
}
