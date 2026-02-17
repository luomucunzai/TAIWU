using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.FistAndPalm
{
	// Token: 0x020003CF RID: 975
	public class TaiYiXiaoYaoZhang : AddPowerAndNeiliAllocationByMoving
	{
		// Token: 0x06003797 RID: 14231 RVA: 0x00236568 File Offset: 0x00234768
		public TaiYiXiaoYaoZhang()
		{
		}

		// Token: 0x06003798 RID: 14232 RVA: 0x00236572 File Offset: 0x00234772
		public TaiYiXiaoYaoZhang(CombatSkillKey skillKey) : base(skillKey, 4104)
		{
			this.AddNeiliAllocationType = 3;
		}
	}
}
