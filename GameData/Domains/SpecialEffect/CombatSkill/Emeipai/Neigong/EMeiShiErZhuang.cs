using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Neigong
{
	// Token: 0x02000549 RID: 1353
	public class EMeiShiErZhuang : FiveElementsAddPenetrateAndResist
	{
		// Token: 0x06004023 RID: 16419 RVA: 0x0025D0EB File Offset: 0x0025B2EB
		public EMeiShiErZhuang()
		{
		}

		// Token: 0x06004024 RID: 16420 RVA: 0x0025D0F5 File Offset: 0x0025B2F5
		public EMeiShiErZhuang(CombatSkillKey skillKey) : base(skillKey, 2001)
		{
			this.RequireFiveElementsType = 1;
		}
	}
}
