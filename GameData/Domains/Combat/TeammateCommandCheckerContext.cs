using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat
{
	// Token: 0x020006FC RID: 1788
	public struct TeammateCommandCheckerContext
	{
		// Token: 0x17000421 RID: 1057
		// (get) Token: 0x060067CB RID: 26571 RVA: 0x003B23A6 File Offset: 0x003B05A6
		public IReadOnlyList<byte> CdPercent
		{
			get
			{
				return this.TeammateChar.GetTeammateCommandCdPercent();
			}
		}

		// Token: 0x17000422 RID: 1058
		// (get) Token: 0x060067CC RID: 26572 RVA: 0x003B23B3 File Offset: 0x003B05B3
		public bool HasTeammateBefore
		{
			get
			{
				return this.CurrChar.TeammateBeforeMainChar >= 0 || this._extraHasTeammateBefore;
			}
		}

		// Token: 0x17000423 RID: 1059
		// (get) Token: 0x060067CD RID: 26573 RVA: 0x003B23CC File Offset: 0x003B05CC
		public bool HasTeammateAfter
		{
			get
			{
				return this.CurrChar.TeammateAfterMainChar >= 0 || this._extraHasTeammateAfter;
			}
		}

		// Token: 0x060067CE RID: 26574 RVA: 0x003B23E8 File Offset: 0x003B05E8
		public void InitExtraFields()
		{
			int[] charList = DomainManager.Combat.GetCharacterList(this.TeammateChar.IsAlly);
			for (int i = 0; i < this.CurrChar.TeammateHasCommand.Length; i++)
			{
				bool flag = !this.CurrChar.TeammateHasCommand[i];
				if (!flag)
				{
					TeammateCommandItem cmdConfig = DomainManager.Combat.GetElement_CombatCharacterDict(charList[i + 1]).ExecutingTeammateCommandConfig;
					bool flag2 = !cmdConfig.IntoCombatField;
					if (!flag2)
					{
						bool flag3 = cmdConfig.PosOffset > 0;
						if (flag3)
						{
							this._extraHasTeammateBefore = true;
						}
						else
						{
							this._extraHasTeammateAfter = true;
						}
					}
				}
			}
		}

		// Token: 0x04001C49 RID: 7241
		public CombatCharacter CurrChar;

		// Token: 0x04001C4A RID: 7242
		public CombatCharacter TeammateChar;

		// Token: 0x04001C4B RID: 7243
		private bool _extraHasTeammateBefore;

		// Token: 0x04001C4C RID: 7244
		private bool _extraHasTeammateAfter;
	}
}
