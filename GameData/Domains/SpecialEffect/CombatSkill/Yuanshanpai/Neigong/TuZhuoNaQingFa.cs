using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Neigong
{
	// Token: 0x020001FA RID: 506
	public class TuZhuoNaQingFa : BaseSectNeigong
	{
		// Token: 0x06002E67 RID: 11879 RVA: 0x0020E9D1 File Offset: 0x0020CBD1
		public TuZhuoNaQingFa()
		{
		}

		// Token: 0x06002E68 RID: 11880 RVA: 0x0020E9DB File Offset: 0x0020CBDB
		public TuZhuoNaQingFa(CombatSkillKey skillKey) : base(skillKey, 5000)
		{
			this.SectId = 5;
		}
	}
}
