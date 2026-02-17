using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B7 RID: 1207
	public class JinGangSiJiaXingFa : BaseSectNeigong
	{
		// Token: 0x06003CEB RID: 15595 RVA: 0x0024F4F9 File Offset: 0x0024D6F9
		public JinGangSiJiaXingFa()
		{
		}

		// Token: 0x06003CEC RID: 15596 RVA: 0x0024F503 File Offset: 0x0024D703
		public JinGangSiJiaXingFa(CombatSkillKey skillKey) : base(skillKey, 11000)
		{
			this.SectId = 11;
		}
	}
}
