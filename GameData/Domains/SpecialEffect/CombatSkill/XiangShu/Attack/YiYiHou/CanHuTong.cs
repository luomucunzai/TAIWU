using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C1 RID: 705
	public class CanHuTong : AddMindMarkAndReduceTrick
	{
		// Token: 0x06003267 RID: 12903 RVA: 0x0021F6DF File Offset: 0x0021D8DF
		public CanHuTong()
		{
		}

		// Token: 0x06003268 RID: 12904 RVA: 0x0021F6E9 File Offset: 0x0021D8E9
		public CanHuTong(CombatSkillKey skillKey) : base(skillKey, 17044)
		{
			this.AffectFrameCount = 30;
		}
	}
}
