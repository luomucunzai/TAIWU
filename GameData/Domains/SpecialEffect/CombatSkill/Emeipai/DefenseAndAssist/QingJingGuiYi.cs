using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000561 RID: 1377
	public class QingJingGuiYi : AssistSkillBase
	{
		// Token: 0x060040AF RID: 16559 RVA: 0x0025F693 File Offset: 0x0025D893
		public QingJingGuiYi()
		{
		}

		// Token: 0x060040B0 RID: 16560 RVA: 0x0025F69D File Offset: 0x0025D89D
		public QingJingGuiYi(CombatSkillKey skillKey) : base(skillKey, 2704)
		{
		}

		// Token: 0x060040B1 RID: 16561 RVA: 0x0025F6AD File Offset: 0x0025D8AD
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060040B2 RID: 16562 RVA: 0x0025F6DC File Offset: 0x0025D8DC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060040B3 RID: 16563 RVA: 0x0025F70C File Offset: 0x0025D90C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender != base.CombatChar || hit || pursueIndex > 0 || !base.CanAffect;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x060040B4 RID: 16564 RVA: 0x0025F74C File Offset: 0x0025D94C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender != base.CombatChar || hit || !base.CanAffect;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x060040B5 RID: 16565 RVA: 0x0025F790 File Offset: 0x0025D990
		private void DoEffect(DataContext context)
		{
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				base.ChangeStanceValue(context, base.CurrEnemyChar, -400);
			}
			else
			{
				base.ChangeBreathValue(context, base.CurrEnemyChar, -3000);
			}
			sbyte[] weaponTricks = base.CombatChar.GetWeaponTricks();
			DomainManager.Combat.AddTrick(context, base.CombatChar, weaponTricks[context.Random.Next(0, weaponTricks.Length)], true);
			base.ShowEffectTips(context);
			base.ShowSpecialEffectTips(0);
		}

		// Token: 0x040012FF RID: 4863
		private const sbyte ReduceBreathStancePercent = 10;
	}
}
