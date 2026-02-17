using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x02000228 RID: 552
	public class BianZouBianSha : AddPowerAndNeiliAllocationByMoving
	{
		// Token: 0x06002F51 RID: 12113 RVA: 0x0021274B File Offset: 0x0021094B
		public BianZouBianSha()
		{
		}

		// Token: 0x06002F52 RID: 12114 RVA: 0x00212755 File Offset: 0x00210955
		public BianZouBianSha(CombatSkillKey skillKey) : base(skillKey, 15304)
		{
			this.AddNeiliAllocationType = 0;
		}
	}
}
