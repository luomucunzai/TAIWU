using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.DefenseAndAssist
{
	// Token: 0x020005D5 RID: 1493
	public class BaiCaoQueZhuoZhi : AssistSkillBase
	{
		// Token: 0x06004421 RID: 17441 RVA: 0x0026E2D1 File Offset: 0x0026C4D1
		public BaiCaoQueZhuoZhi()
		{
		}

		// Token: 0x06004422 RID: 17442 RVA: 0x0026E2DB File Offset: 0x0026C4DB
		public BaiCaoQueZhuoZhi(CombatSkillKey skillKey) : base(skillKey, 3602)
		{
		}

		// Token: 0x06004423 RID: 17443 RVA: 0x0026E2EB File Offset: 0x0026C4EB
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x06004424 RID: 17444 RVA: 0x0026E308 File Offset: 0x0026C508
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			base.OnDisable(context);
		}

		// Token: 0x06004425 RID: 17445 RVA: 0x0026E328 File Offset: 0x0026C528
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !hit || pursueIndex != 0 || attacker != base.CombatChar || !base.CombatChar.GetChangeTrickAttack() || !base.CanAffect || !context.Random.CheckPercentProb(30);
			if (!flag)
			{
				bool affected = base.IsDirect ? base.CombatChar.RemoveRandomInjury(context, 1) : base.CurrEnemyChar.WorsenRandomInjury(context);
				bool flag2 = affected;
				if (flag2)
				{
					base.ShowSpecialEffectTips(0);
				}
			}
		}

		// Token: 0x04001437 RID: 5175
		private const sbyte AffectOdds = 30;
	}
}
