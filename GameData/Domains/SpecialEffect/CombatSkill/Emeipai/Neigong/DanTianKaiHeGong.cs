using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x02000548 RID: 1352
	public class DanTianKaiHeGong : BaseSectNeigong
	{
		// Token: 0x06004021 RID: 16417 RVA: 0x0025D0CA File Offset: 0x0025B2CA
		public DanTianKaiHeGong()
		{
		}

		// Token: 0x06004022 RID: 16418 RVA: 0x0025D0D4 File Offset: 0x0025B2D4
		public DanTianKaiHeGong(CombatSkillKey skillKey) : base(skillKey, 2000)
		{
			this.SectId = 2;
		}
	}
}
