using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Neigong
{
	// Token: 0x020004ED RID: 1261
	public class HunTianXingTu : KeepSkillCanCast
	{
		// Token: 0x06003E23 RID: 15907 RVA: 0x00254D30 File Offset: 0x00252F30
		public HunTianXingTu()
		{
		}

		// Token: 0x06003E24 RID: 15908 RVA: 0x00254D3A File Offset: 0x00252F3A
		public HunTianXingTu(CombatSkillKey skillKey) : base(skillKey, 13006)
		{
			this.RequireFiveElementsType = 2;
		}
	}
}
