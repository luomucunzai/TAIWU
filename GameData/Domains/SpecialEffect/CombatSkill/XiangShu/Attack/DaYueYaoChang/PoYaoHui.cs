using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031A RID: 794
	public class PoYaoHui : ReduceResources
	{
		// Token: 0x06003425 RID: 13349 RVA: 0x00227F32 File Offset: 0x00226132
		public PoYaoHui()
		{
		}

		// Token: 0x06003426 RID: 13350 RVA: 0x00227F3C File Offset: 0x0022613C
		public PoYaoHui(CombatSkillKey skillKey) : base(skillKey, 17010)
		{
			this.AffectFrameCount = 120;
		}
	}
}
