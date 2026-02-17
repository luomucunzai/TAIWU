using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031E RID: 798
	public class TianMoXiangFu : AddPowerAndAddFlaw
	{
		// Token: 0x06003431 RID: 13361 RVA: 0x002280FC File Offset: 0x002262FC
		public TianMoXiangFu()
		{
		}

		// Token: 0x06003432 RID: 13362 RVA: 0x00228106 File Offset: 0x00226306
		public TianMoXiangFu(CombatSkillKey skillKey) : base(skillKey, 17012)
		{
			this.AddPower = 40;
			this.FlawCount = 3;
		}
	}
}
