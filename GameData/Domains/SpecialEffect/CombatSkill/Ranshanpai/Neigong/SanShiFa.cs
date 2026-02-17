using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Neigong
{
	// Token: 0x02000453 RID: 1107
	public class SanShiFa : KeepSkillCanCast
	{
		// Token: 0x06003A94 RID: 14996 RVA: 0x002441D3 File Offset: 0x002423D3
		public SanShiFa()
		{
		}

		// Token: 0x06003A95 RID: 14997 RVA: 0x002441DD File Offset: 0x002423DD
		public SanShiFa(CombatSkillKey skillKey) : base(skillKey, 7006)
		{
			this.RequireFiveElementsType = 1;
		}
	}
}
