using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004DC RID: 1244
	public class JieQingShiJue : TrickBuffFlaw
	{
		// Token: 0x06003DA8 RID: 15784 RVA: 0x00252B19 File Offset: 0x00250D19
		public JieQingShiJue()
		{
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00252B23 File Offset: 0x00250D23
		public JieQingShiJue(CombatSkillKey skillKey) : base(skillKey, 13300)
		{
			this.RequireTrickType = 1;
		}

		// Token: 0x06003DAA RID: 15786 RVA: 0x00252B3C File Offset: 0x00250D3C
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
