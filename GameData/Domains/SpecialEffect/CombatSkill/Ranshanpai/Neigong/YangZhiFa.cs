using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000459 RID: 1113
	public class YangZhiFa : BaseSectNeigong
	{
		// Token: 0x06003AB7 RID: 15031 RVA: 0x00244D67 File Offset: 0x00242F67
		public YangZhiFa()
		{
		}

		// Token: 0x06003AB8 RID: 15032 RVA: 0x00244D71 File Offset: 0x00242F71
		public YangZhiFa(CombatSkillKey skillKey) : base(skillKey, 7000)
		{
			this.SectId = 7;
		}
	}
}
