using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C0 RID: 704
	public class CanHuTi : AddMindMarkAndReduceTrick
	{
		// Token: 0x06003265 RID: 12901 RVA: 0x0021F6BD File Offset: 0x0021D8BD
		public CanHuTi()
		{
		}

		// Token: 0x06003266 RID: 12902 RVA: 0x0021F6C7 File Offset: 0x0021D8C7
		public CanHuTi(CombatSkillKey skillKey) : base(skillKey, 17041)
		{
			this.AffectFrameCount = 60;
		}
	}
}
