using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.DefenseAndAssist
{
	// Token: 0x02000439 RID: 1081
	public class ZhanYiShiBaDie : DefenseSkillBase
	{
		// Token: 0x060039E5 RID: 14821 RVA: 0x00240F1F File Offset: 0x0023F11F
		public ZhanYiShiBaDie()
		{
		}

		// Token: 0x060039E6 RID: 14822 RVA: 0x00240F29 File Offset: 0x0023F129
		public ZhanYiShiBaDie(CombatSkillKey skillKey) : base(skillKey, 1501)
		{
		}

		// Token: 0x060039E7 RID: 14823 RVA: 0x00240F39 File Offset: 0x0023F139
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060039E8 RID: 14824 RVA: 0x00240F56 File Offset: 0x0023F156
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060039E9 RID: 14825 RVA: 0x00240F74 File Offset: 0x0023F174
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !isFightBack || attacker != base.CombatChar || !hit || !base.CanAffect;
			if (!flag)
			{
				DomainManager.Combat.AddCombatState(context, base.IsDirect ? base.CombatChar : base.CurrEnemyChar, base.IsDirect ? 1 : 2, base.IsDirect ? 11 : 12, 50);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x040010EC RID: 4332
		private const sbyte AddStatePowerUnit = 50;
	}
}
