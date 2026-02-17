using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.DefenseAndAssist
{
	// Token: 0x02000463 RID: 1123
	public class LiuDingLiuJiaZhen : BuffTeammateCommand
	{
		// Token: 0x06003AFB RID: 15099 RVA: 0x00246103 File Offset: 0x00244303
		public LiuDingLiuJiaZhen()
		{
		}

		// Token: 0x06003AFC RID: 15100 RVA: 0x0024610D File Offset: 0x0024430D
		public LiuDingLiuJiaZhen(CombatSkillKey skillKey) : base(skillKey, 7604)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.Fight,
				ETeammateCommandImplement.StopEnemy
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
