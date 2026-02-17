using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002C8 RID: 712
	public class JianHuaWuJian : MobilityAddPower
	{
		// Token: 0x0600327D RID: 12925 RVA: 0x0021FABE File Offset: 0x0021DCBE
		public JianHuaWuJian()
		{
		}

		// Token: 0x0600327E RID: 12926 RVA: 0x0021FAC8 File Offset: 0x0021DCC8
		public JianHuaWuJian(CombatSkillKey skillKey) : base(skillKey, 17064)
		{
			this.AddPowerUnit = 2;
		}
	}
}
