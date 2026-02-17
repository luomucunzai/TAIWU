using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat
{
	// Token: 0x020006F8 RID: 1784
	public class TeammateCommandCheckerRepairItem : TeammateCommandCheckerBase
	{
		// Token: 0x17000420 RID: 1056
		// (get) Token: 0x060067C2 RID: 26562 RVA: 0x003B2323 File Offset: 0x003B0523
		protected override bool CheckTeammateAfter
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060067C3 RID: 26563 RVA: 0x003B2326 File Offset: 0x003B0526
		protected override IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context)
		{
			foreach (ItemKey equipKey in context.CurrChar.GetCharacter().GetEquipment())
			{
				bool flag = context.TeammateChar.CalcTeammateCommandRepairDurabilityValue(equipKey) > 0;
				if (flag)
				{
					yield break;
				}
				equipKey = default(ItemKey);
			}
			ItemKey[] array = null;
			yield return ETeammateCommandBanReason.RepairItemNonAnyRepairable;
			yield break;
		}
	}
}
