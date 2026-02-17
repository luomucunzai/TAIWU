using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiXiang
{
	// Token: 0x020002CC RID: 716
	public class WuJianHuaJian : MobilityAddPower
	{
		// Token: 0x06003291 RID: 12945 RVA: 0x0021FEB5 File Offset: 0x0021E0B5
		public WuJianHuaJian()
		{
		}

		// Token: 0x06003292 RID: 12946 RVA: 0x0021FEBF File Offset: 0x0021E0BF
		public WuJianHuaJian(CombatSkillKey skillKey) : base(skillKey, 17061)
		{
			this.AddPowerUnit = 1;
		}
	}
}
