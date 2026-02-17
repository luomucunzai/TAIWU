using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002C7 RID: 711
	public class HunHuaWuHun : AutoCastAgileAndDefense
	{
		// Token: 0x0600327B RID: 12923 RVA: 0x0021FA9C File Offset: 0x0021DC9C
		public HunHuaWuHun()
		{
		}

		// Token: 0x0600327C RID: 12924 RVA: 0x0021FAA6 File Offset: 0x0021DCA6
		public HunHuaWuHun(CombatSkillKey skillKey) : base(skillKey, 17065)
		{
			this.AddPower = 80;
		}
	}
}
