using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002CB RID: 715
	public class WuHunHuaHun : AutoCastAgileAndDefense
	{
		// Token: 0x0600328F RID: 12943 RVA: 0x0021FE93 File Offset: 0x0021E093
		public WuHunHuaHun()
		{
		}

		// Token: 0x06003290 RID: 12944 RVA: 0x0021FE9D File Offset: 0x0021E09D
		public WuHunHuaHun(CombatSkillKey skillKey) : base(skillKey, 17062)
		{
			this.AddPower = 40;
		}
	}
}
