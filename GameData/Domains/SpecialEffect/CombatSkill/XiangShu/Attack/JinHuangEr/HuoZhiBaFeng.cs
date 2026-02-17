using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030D RID: 781
	public class HuoZhiBaFeng : AddFlaw
	{
		// Token: 0x060033E9 RID: 13289 RVA: 0x00226FBD File Offset: 0x002251BD
		public HuoZhiBaFeng()
		{
		}

		// Token: 0x060033EA RID: 13290 RVA: 0x00226FC7 File Offset: 0x002251C7
		public HuoZhiBaFeng(CombatSkillKey skillKey) : base(skillKey, 17031)
		{
			this.FlawCount = 1;
		}
	}
}
