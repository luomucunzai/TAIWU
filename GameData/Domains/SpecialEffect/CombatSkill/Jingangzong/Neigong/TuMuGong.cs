using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004BA RID: 1210
	public class TuMuGong : FiveElementsAddPenetrateAndResist
	{
		// Token: 0x06003CF5 RID: 15605 RVA: 0x0024F729 File Offset: 0x0024D929
		public TuMuGong()
		{
		}

		// Token: 0x06003CF6 RID: 15606 RVA: 0x0024F733 File Offset: 0x0024D933
		public TuMuGong(CombatSkillKey skillKey) : base(skillKey, 11001)
		{
			this.RequireFiveElementsType = 0;
		}
	}
}
