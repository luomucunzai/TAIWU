using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat
{
	// Token: 0x02000705 RID: 1797
	public class TeammateCommandInvokerAutoDefend : TeammateCommandInvokerBase
	{
		// Token: 0x060067FA RID: 26618 RVA: 0x003B2D79 File Offset: 0x003B0F79
		public TeammateCommandInvokerAutoDefend(int charId, int index) : base(charId, index)
		{
		}

		// Token: 0x060067FB RID: 26619 RVA: 0x003B2D85 File Offset: 0x003B0F85
		public override void Setup()
		{
			Events.RegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060067FC RID: 26620 RVA: 0x003B2D9A File Offset: 0x003B0F9A
		public override void Close()
		{
			Events.UnRegisterHandler_PrepareSkillBegin(new Events.OnPrepareSkillBegin(this.OnPrepareSkillBegin));
		}

		// Token: 0x060067FD RID: 26621 RVA: 0x003B2DB0 File Offset: 0x003B0FB0
		private void OnPrepareSkillBegin(DataContext context, int charId, bool isAlly, short skillId)
		{
			bool flag = isAlly == this.CombatChar.IsAlly || !CombatSkillTemplateHelper.IsAttack(skillId);
			if (!flag)
			{
				base.IntoCombat();
			}
		}
	}
}
