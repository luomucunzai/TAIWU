using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Agile
{
	// Token: 0x02000337 RID: 823
	public class ChenRan : AgileSkillBase
	{
		// Token: 0x0600349D RID: 13469 RVA: 0x002293CE File Offset: 0x002275CE
		public ChenRan()
		{
		}

		// Token: 0x0600349E RID: 13470 RVA: 0x002293D8 File Offset: 0x002275D8
		public ChenRan(CombatSkillKey skillKey) : base(skillKey, 16210)
		{
		}

		// Token: 0x0600349F RID: 13471 RVA: 0x002293E8 File Offset: 0x002275E8
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060034A0 RID: 13472 RVA: 0x00229417 File Offset: 0x00227617
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060034A1 RID: 13473 RVA: 0x00229448 File Offset: 0x00227648
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || pursueIndex != 0 || attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.SilenceEnemySkill(context);
			}
		}

		// Token: 0x060034A2 RID: 13474 RVA: 0x00229484 File Offset: 0x00227684
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !hit || index > 2 || context.Attacker != base.CombatChar || !base.CanAffect;
			if (!flag)
			{
				this.SilenceEnemySkill(context);
			}
		}

		// Token: 0x060034A3 RID: 13475 RVA: 0x002294C8 File Offset: 0x002276C8
		private void SilenceEnemySkill(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			short skillId = enemyChar.GetRandomBanableSkillId(context.Random, null, -1);
			bool flag = skillId < 0;
			if (!flag)
			{
				DomainManager.Combat.SilenceSkill(context, enemyChar, skillId, 300, 100);
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x04000F81 RID: 3969
		private const short SilenceFrame = 300;
	}
}
