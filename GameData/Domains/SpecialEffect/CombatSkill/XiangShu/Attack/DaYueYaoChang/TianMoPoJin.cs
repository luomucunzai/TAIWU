using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.DaYueYaoChang
{
	// Token: 0x0200031D RID: 797
	public class TianMoPoJin : AddPowerAndAddFlaw
	{
		// Token: 0x0600342F RID: 13359 RVA: 0x002280D3 File Offset: 0x002262D3
		public TianMoPoJin()
		{
		}

		// Token: 0x06003430 RID: 13360 RVA: 0x002280DD File Offset: 0x002262DD
		public TianMoPoJin(CombatSkillKey skillKey) : base(skillKey, 17015)
		{
			this.AddPower = 80;
			this.FlawCount = 6;
		}
	}
}
