using System;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Whip
{
	// Token: 0x0200037E RID: 894
	public class GouHunSuiGuBian : AttackBodyPart
	{
		// Token: 0x060035DF RID: 13791 RVA: 0x0022E3B6 File Offset: 0x0022C5B6
		public GouHunSuiGuBian()
		{
		}

		// Token: 0x060035E0 RID: 13792 RVA: 0x0022E3C0 File Offset: 0x0022C5C0
		public GouHunSuiGuBian(CombatSkillKey skillKey) : base(skillKey, 12403)
		{
			this.BodyParts = new sbyte[]
			{
				5,
				6
			};
			this.ReverseAddDamagePercent = 45;
		}

		// Token: 0x060035E1 RID: 13793 RVA: 0x0022E3EC File Offset: 0x0022C5EC
		protected override void OnCastAffectPower(DataContext context)
		{
			CombatCharacter enemyChar = base.CurrEnemyChar;
			FlawOrAcupointCollection flaw = enemyChar.GetFlawCollection();
			flaw.OfflineRecoverKeepTimePercent(100);
			enemyChar.SetFlawCollection(flaw, context);
			base.ShowSpecialEffectTips(1);
		}

		// Token: 0x04000FB4 RID: 4020
		private const int RecoverFlawKeepTimePercent = 100;
	}
}
