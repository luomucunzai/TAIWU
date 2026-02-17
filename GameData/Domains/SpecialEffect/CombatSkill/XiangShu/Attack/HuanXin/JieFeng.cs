using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.HuanXin
{
	// Token: 0x02000315 RID: 789
	public class JieFeng : CombatSkillEffectBase
	{
		// Token: 0x0600340B RID: 13323 RVA: 0x00227928 File Offset: 0x00225B28
		public JieFeng()
		{
		}

		// Token: 0x0600340C RID: 13324 RVA: 0x00227932 File Offset: 0x00225B32
		public JieFeng(CombatSkillKey skillKey) : base(skillKey, 17100, -1)
		{
		}

		// Token: 0x0600340D RID: 13325 RVA: 0x00227944 File Offset: 0x00225B44
		public override void OnEnable(DataContext context)
		{
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, (short)(83 + context.Random.Next(0, 10)));
			base.ShowSpecialEffectTips(0);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600340E RID: 13326 RVA: 0x00227991 File Offset: 0x00225B91
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x0600340F RID: 13327 RVA: 0x002279A8 File Offset: 0x00225BA8
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || skillId != base.SkillTemplateId;
			if (!flag)
			{
				base.RemoveSelf(context);
			}
		}
	}
}
