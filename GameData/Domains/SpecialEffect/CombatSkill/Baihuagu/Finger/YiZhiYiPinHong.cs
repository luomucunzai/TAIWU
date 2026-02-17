using System;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Finger
{
	// Token: 0x020005D3 RID: 1491
	public class YiZhiYiPinHong : TrickBuffFlaw
	{
		// Token: 0x0600441C RID: 17436 RVA: 0x0026E24E File Offset: 0x0026C44E
		public YiZhiYiPinHong()
		{
		}

		// Token: 0x0600441D RID: 17437 RVA: 0x0026E258 File Offset: 0x0026C458
		public YiZhiYiPinHong(CombatSkillKey skillKey) : base(skillKey, 3102)
		{
			this.RequireTrickType = 7;
		}

		// Token: 0x0600441E RID: 17438 RVA: 0x0026E270 File Offset: 0x0026C470
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
