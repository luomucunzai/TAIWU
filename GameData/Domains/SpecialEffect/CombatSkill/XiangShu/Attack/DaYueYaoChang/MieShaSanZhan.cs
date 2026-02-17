using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x02000319 RID: 793
	public class MieShaSanZhan : AddPowerAndRepeat
	{
		// Token: 0x06003423 RID: 13347 RVA: 0x00227F10 File Offset: 0x00226110
		public MieShaSanZhan()
		{
		}

		// Token: 0x06003424 RID: 13348 RVA: 0x00227F1A File Offset: 0x0022611A
		public MieShaSanZhan(CombatSkillKey skillKey) : base(skillKey, 17011)
		{
			this.AutoCastReducePower = -60;
		}
	}
}
