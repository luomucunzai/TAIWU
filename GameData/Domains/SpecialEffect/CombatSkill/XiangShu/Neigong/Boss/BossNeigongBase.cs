using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029B RID: 667
	public class BossNeigongBase : CombatSkillEffectBase
	{
		// Token: 0x0600318B RID: 12683 RVA: 0x0021B58B File Offset: 0x0021978B
		protected BossNeigongBase()
		{
		}

		// Token: 0x0600318C RID: 12684 RVA: 0x0021B595 File Offset: 0x00219795
		protected BossNeigongBase(CombatSkillKey skillKey, int type) : base(skillKey, type, -1)
		{
		}

		// Token: 0x0600318D RID: 12685 RVA: 0x0021B5A2 File Offset: 0x002197A2
		public override void OnEnable(DataContext context)
		{
			CombatDomain.RegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x0600318E RID: 12686 RVA: 0x0021B5B7 File Offset: 0x002197B7
		public override void OnDisable(DataContext context)
		{
			CombatDomain.UnRegisterHandler_CombatCharAboutToFall(new CombatDomain.OnCombatCharAboutToFall(this.OnCharAboutToFall));
		}

		// Token: 0x0600318F RID: 12687 RVA: 0x0021B5CC File Offset: 0x002197CC
		private void OnCharAboutToFall(DataContext context, CombatCharacter combatChar, int eventIndex)
		{
			bool outTomb = 45 <= DomainManager.Combat.CombatConfig.TemplateId && DomainManager.Combat.CombatConfig.TemplateId <= 53;
			bool flag = combatChar != base.CombatChar || eventIndex != 3 || base.CombatChar.GetBossPhase() > 0 || (!DomainManager.Combat.IsCharacterFallen(base.CombatChar) && !DomainManager.Combat.CombatConfig.StartInSecondPhase) || outTomb;
			if (!flag)
			{
				DomainManager.Combat.Reset(context, base.CombatChar);
				DomainManager.Combat.AddBossPhase(context, base.CombatChar, base.EffectId);
				this.ActivePhase2Effect(context);
			}
		}

		// Token: 0x06003190 RID: 12688 RVA: 0x0021B686 File Offset: 0x00219886
		protected virtual void ActivePhase2Effect(DataContext context)
		{
		}
	}
}
