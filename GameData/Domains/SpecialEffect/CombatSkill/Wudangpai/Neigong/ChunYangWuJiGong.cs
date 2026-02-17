using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.Neigong
{
	// Token: 0x020003C6 RID: 966
	public class ChunYangWuJiGong : KeepSkillCanCast
	{
		// Token: 0x0600376F RID: 14191 RVA: 0x00235931 File Offset: 0x00233B31
		public ChunYangWuJiGong()
		{
		}

		// Token: 0x06003770 RID: 14192 RVA: 0x0023593B File Offset: 0x00233B3B
		public ChunYangWuJiGong(CombatSkillKey skillKey) : base(skillKey, 4007)
		{
			this.RequireFiveElementsType = 3;
		}
	}
}
