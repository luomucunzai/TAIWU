using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.DefenseAndAssist
{
	// Token: 0x02000282 RID: 642
	public class JingWeiTianHaiShi : DefenseSkillBase
	{
		// Token: 0x060030F2 RID: 12530 RVA: 0x002192F1 File Offset: 0x002174F1
		public JingWeiTianHaiShi()
		{
		}

		// Token: 0x060030F3 RID: 12531 RVA: 0x002192FB File Offset: 0x002174FB
		public JingWeiTianHaiShi(CombatSkillKey skillKey) : base(skillKey, 8501)
		{
		}

		// Token: 0x060030F4 RID: 12532 RVA: 0x0021930B File Offset: 0x0021750B
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060030F5 RID: 12533 RVA: 0x0021933A File Offset: 0x0021753A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060030F6 RID: 12534 RVA: 0x0021936C File Offset: 0x0021756C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = defender != base.CombatChar || hit || pursueIndex > 0 || !base.CanAffect || attacker.NormalAttackHitType != 2;
			if (!flag)
			{
				this.DoAvoidEffect(context);
			}
		}

		// Token: 0x060030F7 RID: 12535 RVA: 0x002193B8 File Offset: 0x002175B8
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender != base.CombatChar || hit || !base.CanAffect || index > 2 || DomainManager.Combat.GetDamageCompareData().HitType[index] != 2;
			if (!flag)
			{
				this.DoAvoidEffect(context);
			}
		}

		// Token: 0x060030F8 RID: 12536 RVA: 0x00219418 File Offset: 0x00217618
		private void DoAvoidEffect(DataContext context)
		{
			CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				DomainManager.Combat.AddTrick(context, enemyChar, 20, false);
			}
			else
			{
				DomainManager.Combat.AppendMindDefeatMark(context, enemyChar, 1, -1, false);
			}
			base.ShowSpecialEffectTips(0);
		}
	}
}
