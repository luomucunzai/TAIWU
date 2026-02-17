using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030A RID: 778
	public class ChiYuPoKong : AddDistanceAndAddInjury
	{
		// Token: 0x060033E3 RID: 13283 RVA: 0x00226F5A File Offset: 0x0022515A
		public ChiYuPoKong()
		{
		}

		// Token: 0x060033E4 RID: 13284 RVA: 0x00226F64 File Offset: 0x00225164
		public ChiYuPoKong(CombatSkillKey skillKey) : base(skillKey, 17032)
		{
			this.InjuryCount = 3;
		}
	}
}
