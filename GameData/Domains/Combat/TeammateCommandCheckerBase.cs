using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D9 RID: 1753
	public abstract class TeammateCommandCheckerBase : ITeammateCommandChecker
	{
		// Token: 0x06006772 RID: 26482 RVA: 0x003B1F2D File Offset: 0x003B012D
		public virtual IEnumerable<ETeammateCommandBanReason> Check(int index, TeammateCommandCheckerContext context)
		{
			bool flag = context.CdPercent[index] > 0;
			if (flag)
			{
				yield return ETeammateCommandBanReason.CommonCd;
			}
			bool flag2 = !DomainManager.Combat.IsMainCharacter(context.CurrChar);
			if (flag2)
			{
				yield return ETeammateCommandBanReason.CommonNotMain;
			}
			bool flag3 = DomainManager.Combat.IsCharacterFallen(context.TeammateChar);
			if (flag3)
			{
				yield return ETeammateCommandBanReason.CommonFallen;
			}
			bool flag4 = context.TeammateChar.StopCommandEffectCount != 0;
			if (flag4)
			{
				yield return ETeammateCommandBanReason.CommonStop;
			}
			else
			{
				bool flag5 = !DomainManager.SpecialEffect.ModifyData(context.TeammateChar.GetId(), -1, 271, true, -1, -1, -1);
				if (flag5)
				{
					yield return ETeammateCommandBanReason.CommonStop;
				}
			}
			bool flag6 = context.CurrChar.ChangeCharId >= 0;
			if (flag6)
			{
				yield return ETeammateCommandBanReason.CommonConflict;
			}
			else
			{
				bool flag7 = context.TeammateChar.GetExecutingTeammateCommand() >= 0;
				if (flag7)
				{
					yield return ETeammateCommandBanReason.CommonConflict;
				}
				else
				{
					bool flag8 = this.CheckTeammateBefore && context.HasTeammateBefore;
					if (flag8)
					{
						yield return ETeammateCommandBanReason.CommonConflict;
					}
					else
					{
						bool flag9 = this.CheckTeammateAfter && context.HasTeammateAfter;
						if (flag9)
						{
							yield return ETeammateCommandBanReason.CommonConflict;
						}
					}
				}
			}
			foreach (ETeammateCommandBanReason banReason in this.Extra(context))
			{
				yield return banReason;
			}
			IEnumerator<ETeammateCommandBanReason> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x1700040D RID: 1037
		// (get) Token: 0x06006773 RID: 26483 RVA: 0x003B1F4B File Offset: 0x003B014B
		protected virtual bool CheckTeammateBoth
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700040E RID: 1038
		// (get) Token: 0x06006774 RID: 26484 RVA: 0x003B1F4E File Offset: 0x003B014E
		protected virtual bool CheckTeammateBefore
		{
			get
			{
				return this.CheckTeammateBoth;
			}
		}

		// Token: 0x1700040F RID: 1039
		// (get) Token: 0x06006775 RID: 26485 RVA: 0x003B1F56 File Offset: 0x003B0156
		protected virtual bool CheckTeammateAfter
		{
			get
			{
				return this.CheckTeammateBoth;
			}
		}

		// Token: 0x06006776 RID: 26486
		protected abstract IEnumerable<ETeammateCommandBanReason> Extra(TeammateCommandCheckerContext context);
	}
}
