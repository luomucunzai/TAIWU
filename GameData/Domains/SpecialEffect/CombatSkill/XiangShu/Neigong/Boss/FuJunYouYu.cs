using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x0200029E RID: 670
	public class FuJunYouYu : BossNeigongBase
	{
		// Token: 0x0600319A RID: 12698 RVA: 0x0021B873 File Offset: 0x00219A73
		public FuJunYouYu()
		{
		}

		// Token: 0x0600319B RID: 12699 RVA: 0x0021B885 File Offset: 0x00219A85
		public FuJunYouYu(CombatSkillKey skillKey) : base(skillKey, 16111)
		{
		}

		// Token: 0x0600319C RID: 12700 RVA: 0x0021B89D File Offset: 0x00219A9D
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600319D RID: 12701 RVA: 0x0021B8CC File Offset: 0x00219ACC
		protected override void ActivePhase2Effect(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x0600319E RID: 12702 RVA: 0x0021B8F4 File Offset: 0x00219AF4
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = pursueIndex != 0;
			if (!flag)
			{
				sbyte hitType = DomainManager.Combat.GetDamageCompareData().HitType[0];
				bool flag2 = base.CombatChar == attacker && !hit;
				if (flag2)
				{
					this.AddCombatState(context, true, hitType);
				}
				else
				{
					bool flag3 = base.CombatChar == defender && hit;
					if (flag3)
					{
						this.AddCombatState(context, false, hitType);
					}
				}
			}
		}

		// Token: 0x0600319F RID: 12703 RVA: 0x0021B95C File Offset: 0x00219B5C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = index > 2 || hitType < 0;
			if (!flag)
			{
				bool flag2 = base.CombatChar == context.Attacker && !hit;
				if (flag2)
				{
					this.AddCombatState(context, true, hitType);
				}
				else
				{
					bool flag3 = base.CombatChar == context.Defender && hit;
					if (flag3)
					{
						this.AddCombatState(context, false, hitType);
					}
				}
			}
		}

		// Token: 0x060031A0 RID: 12704 RVA: 0x0021B9CB File Offset: 0x00219BCB
		private void AddCombatState(DataContext context, bool addHit, sbyte hitType)
		{
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 1, (short)((addHit ? 75 : 79) + hitType), (int)this.AddStatePowerUnit);
			base.ShowSpecialEffectTips(addHit, 1, 2);
		}

		// Token: 0x04000EB5 RID: 3765
		private sbyte AddStatePowerUnit = 25;
	}
}
