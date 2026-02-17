using System;
using System.Collections.Generic;
using GameData.Domains.Character;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F7 RID: 1783
	public class TeammateCommandCheckerTransferManyMark : TeammateCommandCheckerBase
	{
		// Token: 0x1700041F RID: 1055
		// (get) Token: 0x060067BF RID: 26559 RVA: 0x003B2300 File Offset: 0x003B0500
		protected override bool CheckTeammateBefore
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067C0 RID: 26560 RVA: 0x003B2303 File Offset: 0x003B0503
		protected unsafe override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			int newDisorderOfQi = (int)(context.CurrChar.GetCharacter().GetDisorderOfQi() - context.CurrChar.GetOldDisorderOfQi());
			bool flag = newDisorderOfQi > 0 && context.TeammateChar.GetCharacter().GetDisorderOfQi() < DisorderLevelOfQi.MaxValue;
			if (flag)
			{
				yield break;
			}
			PoisonInts oldPoison = *context.CurrChar.GetOldPoison();
			PoisonInts newPoison = context.CurrChar.GetPoison().Subtract(ref oldPoison);
			PoisonInts teammatePoisons = *context.TeammateChar.GetPoison();
			int num;
			for (int i = 0; i < 6; i = num + 1)
			{
				bool flag2 = *newPoison[i] > 0 && *teammatePoisons[i] < 25000;
				if (flag2)
				{
					yield break;
				}
				num = i;
			}
			Injuries newInjuries = context.CurrChar.GetInjuries().Subtract(context.CurrChar.GetOldInjuries());
			Injuries teammateInjuries = context.TeammateChar.GetInjuries();
			sbyte b;
			for (sbyte j = 0; j < 7; j = b + 1)
			{
				ValueTuple<sbyte, sbyte> valueTuple = newInjuries.Get(j);
				sbyte newOuter = valueTuple.Item1;
				sbyte newInner = valueTuple.Item2;
				valueTuple = teammateInjuries.Get(j);
				sbyte outer = valueTuple.Item1;
				sbyte inner = valueTuple.Item2;
				bool flag3 = newOuter > 0 && outer < 6;
				if (flag3)
				{
					yield break;
				}
				bool flag4 = newInner > 0 && inner < 6;
				if (flag4)
				{
					yield break;
				}
				b = j;
			}
			yield return ETeammateCommandBanReason.TransferManyMarkNonAnyMark;
			yield break;
		}
	}
}
