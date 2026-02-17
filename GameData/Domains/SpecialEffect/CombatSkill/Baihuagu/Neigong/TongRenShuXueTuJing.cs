using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Baihuagu.Neigong
{
	// Token: 0x020005C4 RID: 1476
	public class TongRenShuXueTuJing : BaseSectNeigong
	{
		// Token: 0x060043C0 RID: 17344 RVA: 0x0026C758 File Offset: 0x0026A958
		public TongRenShuXueTuJing()
		{
		}

		// Token: 0x060043C1 RID: 17345 RVA: 0x0026C762 File Offset: 0x0026A962
		public TongRenShuXueTuJing(CombatSkillKey skillKey) : base(skillKey, 3000)
		{
			this.SectId = 3;
		}
	}
}
