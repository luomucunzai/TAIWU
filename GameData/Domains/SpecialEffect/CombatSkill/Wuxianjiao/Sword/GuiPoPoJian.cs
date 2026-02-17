using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Sword
{
	// Token: 0x02000387 RID: 903
	public class GuiPoPoJian : TrickBuffFlaw
	{
		// Token: 0x0600361C RID: 13852 RVA: 0x0022F564 File Offset: 0x0022D764
		public GuiPoPoJian()
		{
		}

		// Token: 0x0600361D RID: 13853 RVA: 0x0022F56E File Offset: 0x0022D76E
		public GuiPoPoJian(CombatSkillKey skillKey) : base(skillKey, 12302)
		{
			this.RequireTrickType = 4;
		}

		// Token: 0x0600361E RID: 13854 RVA: 0x0022F588 File Offset: 0x0022D788
		protected override bool OnReverseAffect(DataContext context, int trickCount)
		{
			int affectCount = trickCount / 2;
			int upgradeCount = 0;
			for (int i = 0; i < affectCount; i++)
			{
				upgradeCount += base.CurrEnemyChar.UpgradeRandomFlawOrAcupoint(context, true, 1, -1);
			}
			return upgradeCount > 0;
		}
	}
}
