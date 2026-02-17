using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C3 RID: 707
	public class GuiHu : AddWuTrick
	{
		// Token: 0x0600326B RID: 12907 RVA: 0x0021F722 File Offset: 0x0021D922
		public GuiHu()
		{
		}

		// Token: 0x0600326C RID: 12908 RVA: 0x0021F72C File Offset: 0x0021D92C
		public GuiHu(CombatSkillKey skillKey) : base(skillKey, 17043)
		{
			this.AddTrickCount = 6;
		}
	}
}
