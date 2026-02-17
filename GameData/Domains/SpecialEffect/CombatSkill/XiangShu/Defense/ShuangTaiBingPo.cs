using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Defense
{
	// Token: 0x020002B0 RID: 688
	public class ShuangTaiBingPo : DefenseSkillBase
	{
		// Token: 0x060031FD RID: 12797 RVA: 0x0021D6DD File Offset: 0x0021B8DD
		public ShuangTaiBingPo()
		{
		}

		// Token: 0x060031FE RID: 12798 RVA: 0x0021D6E7 File Offset: 0x0021B8E7
		public ShuangTaiBingPo(CombatSkillKey skillKey) : base(skillKey, 16302)
		{
		}

		// Token: 0x060031FF RID: 12799 RVA: 0x0021D6F7 File Offset: 0x0021B8F7
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003200 RID: 12800 RVA: 0x0021D726 File Offset: 0x0021B926
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003201 RID: 12801 RVA: 0x0021D758 File Offset: 0x0021B958
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || pursueIndex != 0 || defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.ReduceEnemyBreathStance(context);
			}
		}

		// Token: 0x06003202 RID: 12802 RVA: 0x0021D794 File Offset: 0x0021B994
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !hit || index > 2 || context.Defender != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.ReduceEnemyBreathStance(context);
			}
		}

		// Token: 0x06003203 RID: 12803 RVA: 0x0021D7D8 File Offset: 0x0021B9D8
		private void ReduceEnemyBreathStance(DataContext context)
		{
			base.ChangeStanceValue(context, base.CurrEnemyChar, -400);
			base.ChangeBreathValue(context, base.CurrEnemyChar, -3000);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x04000ED1 RID: 3793
		private const sbyte ReduceBreathStance = 10;
	}
}
