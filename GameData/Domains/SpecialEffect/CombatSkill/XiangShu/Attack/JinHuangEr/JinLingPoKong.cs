using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030E RID: 782
	public class JinLingPoKong : AddDistanceAndAddInjury
	{
		// Token: 0x060033EB RID: 13291 RVA: 0x00226FDE File Offset: 0x002251DE
		public JinLingPoKong()
		{
		}

		// Token: 0x060033EC RID: 13292 RVA: 0x00226FE8 File Offset: 0x002251E8
		public JinLingPoKong(CombatSkillKey skillKey) : base(skillKey, 17035)
		{
			this.InjuryCount = 6;
		}
	}
}
