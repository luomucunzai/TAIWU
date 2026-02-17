using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.DefenseAndAssist
{
	// Token: 0x020004FF RID: 1279
	public class NanDouLiuXingZhen : BuffTeammateCommand
	{
		// Token: 0x06003E76 RID: 15990 RVA: 0x00255F07 File Offset: 0x00254107
		public NanDouLiuXingZhen()
		{
		}

		// Token: 0x06003E77 RID: 15991 RVA: 0x00255F11 File Offset: 0x00254111
		public NanDouLiuXingZhen(CombatSkillKey skillKey) : base(skillKey, 13604)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.AccelerateCast
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
