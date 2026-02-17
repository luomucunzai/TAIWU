using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Neigong
{
	// Token: 0x020003F1 RID: 1009
	public class FengKouGuQiFa : BaseSectNeigong
	{
		// Token: 0x06003865 RID: 14437 RVA: 0x0023A408 File Offset: 0x00238608
		public FengKouGuQiFa()
		{
		}

		// Token: 0x06003866 RID: 14438 RVA: 0x0023A412 File Offset: 0x00238612
		public FengKouGuQiFa(CombatSkillKey skillKey) : base(skillKey, 6000)
		{
			this.SectId = 6;
		}
	}
}
