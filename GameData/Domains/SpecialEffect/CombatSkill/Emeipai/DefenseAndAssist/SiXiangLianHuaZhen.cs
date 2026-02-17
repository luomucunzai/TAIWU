using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.DefenseAndAssist
{
	// Token: 0x02000564 RID: 1380
	public class SiXiangLianHuaZhen : BuffTeammateCommand
	{
		// Token: 0x060040C8 RID: 16584 RVA: 0x00260094 File Offset: 0x0025E294
		public SiXiangLianHuaZhen()
		{
		}

		// Token: 0x060040C9 RID: 16585 RVA: 0x0026009E File Offset: 0x0025E29E
		public SiXiangLianHuaZhen(CombatSkillKey skillKey) : base(skillKey, 2705)
		{
			this.AffectTeammateCommandType = new ETeammateCommandImplement[]
			{
				ETeammateCommandImplement.HealInjury,
				ETeammateCommandImplement.HealPoison
			};
			this.CommandPowerUpPercent = 50;
		}
	}
}
