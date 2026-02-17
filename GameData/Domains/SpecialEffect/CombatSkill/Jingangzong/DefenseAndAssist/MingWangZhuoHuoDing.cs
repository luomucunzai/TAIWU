using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004C7 RID: 1223
	public class MingWangZhuoHuoDing : DefenseSkillBase
	{
		// Token: 0x06003D35 RID: 15669 RVA: 0x00250C50 File Offset: 0x0024EE50
		public MingWangZhuoHuoDing()
		{
		}

		// Token: 0x06003D36 RID: 15670 RVA: 0x00250C5A File Offset: 0x0024EE5A
		public MingWangZhuoHuoDing(CombatSkillKey skillKey) : base(skillKey, 11605)
		{
		}

		// Token: 0x06003D37 RID: 15671 RVA: 0x00250C6A File Offset: 0x0024EE6A
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003D38 RID: 15672 RVA: 0x00250C99 File Offset: 0x0024EE99
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003D39 RID: 15673 RVA: 0x00250CC8 File Offset: 0x0024EEC8
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = hit || defender != base.CombatChar || !base.CanAffect || attacker.NormalAttackHitType != 0;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003D3A RID: 15674 RVA: 0x00250D08 File Offset: 0x0024EF08
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.Defender != base.CombatChar || hit || !base.CanAffect || index > 2 || hitType != 0;
			if (!flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003D3B RID: 15675 RVA: 0x00250D54 File Offset: 0x0024EF54
		private void DoEffect(DataContext context)
		{
			DamageCompareData damageCompareData = DomainManager.Combat.GetDamageCompareData();
			int index = damageCompareData.HitType.IndexOf(0);
			int poisonValue = 180 * damageCompareData.AvoidValue[index] / Math.Max(damageCompareData.HitValue[index], 1);
			bool flag = poisonValue > 0;
			if (flag)
			{
				DomainManager.Combat.AddPoison(context, base.CombatChar, base.CurrEnemyChar, base.IsDirect ? 0 : 1, 2, poisonValue, -1, true, true, default(ItemKey), false, false, false);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04001203 RID: 4611
		private const int AddPoison = 180;
	}
}
