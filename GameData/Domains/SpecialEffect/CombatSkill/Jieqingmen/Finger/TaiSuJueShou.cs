using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F4 RID: 1268
	public class TaiSuJueShou : CombatSkillEffectBase
	{
		// Token: 0x06003E3D RID: 15933 RVA: 0x002550B6 File Offset: 0x002532B6
		public TaiSuJueShou()
		{
		}

		// Token: 0x06003E3E RID: 15934 RVA: 0x002550C0 File Offset: 0x002532C0
		public TaiSuJueShou(CombatSkillKey skillKey) : base(skillKey, 13106, -1)
		{
		}

		// Token: 0x06003E3F RID: 15935 RVA: 0x002550D1 File Offset: 0x002532D1
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003E40 RID: 15936 RVA: 0x002550E6 File Offset: 0x002532E6
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x06003E41 RID: 15937 RVA: 0x002550FC File Offset: 0x002532FC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = context.SkillKey != this.SkillKey || index != 3;
			if (!flag)
			{
				int power = (int)context.Attacker.GetAttackSkillPower();
				bool flag2 = power > 0;
				if (flag2)
				{
					short enemySkillId = base.CurrEnemyChar.GetPreparingSkillId();
					bool flag3 = enemySkillId >= 0 && DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 8 * power / 10);
					if (flag3)
					{
						CombatSkillKey enemySkillKey = new CombatSkillKey(base.CurrEnemyChar.GetId(), enemySkillId);
						SkillEffectKey effectKey = new SkillEffectKey(base.SkillTemplateId, base.IsDirect);
						bool isDirect = base.IsDirect;
						if (isDirect)
						{
							DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, effectKey, (int)(DomainManager.CombatSkill.GetElement_CombatSkills(enemySkillKey).GetPower() * 20 / 100));
						}
						else
						{
							DomainManager.Combat.ReduceSkillPowerInCombat(context, enemySkillKey, effectKey, (int)(-base.SkillInstance.GetPower() * 20 / 100));
						}
						base.CurrEnemyChar.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(base.CurrEnemyChar, 2), context);
						DomainManager.Combat.SetProperLoopAniAndParticle(context, base.CurrEnemyChar, false);
						DomainManager.Combat.SilenceSkill(context, base.CurrEnemyChar, enemySkillId, 1200, 100);
						base.ShowSpecialEffectTips(0);
						base.ShowSpecialEffectTips(1);
					}
				}
				base.RemoveSelf(context);
			}
		}

		// Token: 0x06003E42 RID: 15938 RVA: 0x00255288 File Offset: 0x00253488
		public static int CalcInterruptOdds(CombatSkillKey selfSkill, bool isDirect, CombatSkillKey enemySkill)
		{
			return 80;
		}

		// Token: 0x0400125C RID: 4700
		private const sbyte InterruptOddsUnit = 8;

		// Token: 0x0400125D RID: 4701
		private const sbyte ChangePowerPercent = 20;

		// Token: 0x0400125E RID: 4702
		private const int SilenceFrame = 1200;
	}
}
