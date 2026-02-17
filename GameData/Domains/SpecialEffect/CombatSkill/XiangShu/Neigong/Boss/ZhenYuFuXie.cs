using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.Boss
{
	// Token: 0x020002A7 RID: 679
	public class ZhenYuFuXie : BossNeigongBase
	{
		// Token: 0x060031CF RID: 12751 RVA: 0x0021C4E0 File Offset: 0x0021A6E0
		public ZhenYuFuXie()
		{
		}

		// Token: 0x060031D0 RID: 12752 RVA: 0x0021C4EA File Offset: 0x0021A6EA
		public ZhenYuFuXie(CombatSkillKey skillKey) : base(skillKey, 16101)
		{
		}

		// Token: 0x060031D1 RID: 12753 RVA: 0x0021C4FA File Offset: 0x0021A6FA
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060031D2 RID: 12754 RVA: 0x0021C517 File Offset: 0x0021A717
		protected override void ActivePhase2Effect(DataContext context)
		{
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060031D3 RID: 12755 RVA: 0x0021C52C File Offset: 0x0021A72C
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || pursueIndex != 0 || attacker != base.CombatChar || base.CurrEnemyChar.GetPreparingSkillId() < 0;
			if (!flag)
			{
				bool flag2 = DomainManager.Combat.InterruptSkill(context, base.CurrEnemyChar, 100);
				if (flag2)
				{
					base.CurrEnemyChar.SetAnimationToPlayOnce(DomainManager.Combat.GetHittedAni(base.CurrEnemyChar, 2), context);
					DomainManager.Combat.SetProperLoopAniAndParticle(context, base.CurrEnemyChar, false);
					base.ShowSpecialEffectTips(1);
				}
			}
		}
	}
}
