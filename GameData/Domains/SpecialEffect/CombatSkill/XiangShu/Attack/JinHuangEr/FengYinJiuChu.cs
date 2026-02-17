using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.JinHuangEr
{
	// Token: 0x0200030B RID: 779
	public class FengYinJiuChu : AddFlaw
	{
		// Token: 0x060033E5 RID: 13285 RVA: 0x00226F7B File Offset: 0x0022517B
		public FengYinJiuChu()
		{
		}

		// Token: 0x060033E6 RID: 13286 RVA: 0x00226F85 File Offset: 0x00225185
		public FengYinJiuChu(CombatSkillKey skillKey) : base(skillKey, 17034)
		{
			this.FlawCount = 2;
		}
	}
}
