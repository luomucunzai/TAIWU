using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004BB RID: 1211
	public class WuShangYuJiaFa : KeepSkillCanCast
	{
		// Token: 0x06003CF7 RID: 15607 RVA: 0x0024F74A File Offset: 0x0024D94A
		public WuShangYuJiaFa()
		{
		}

		// Token: 0x06003CF8 RID: 15608 RVA: 0x0024F754 File Offset: 0x0024D954
		public WuShangYuJiaFa(CombatSkillKey skillKey) : base(skillKey, 11008)
		{
			this.RequireFiveElementsType = 0;
		}
	}
}
