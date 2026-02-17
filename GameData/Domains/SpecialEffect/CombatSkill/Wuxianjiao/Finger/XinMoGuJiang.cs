using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.WugEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Finger
{
	// Token: 0x020003A3 RID: 931
	public class XinMoGuJiang : AddWug
	{
		// Token: 0x06003696 RID: 13974 RVA: 0x00231228 File Offset: 0x0022F428
		public XinMoGuJiang()
		{
		}

		// Token: 0x06003697 RID: 13975 RVA: 0x00231232 File Offset: 0x0022F432
		public XinMoGuJiang(CombatSkillKey skillKey) : base(skillKey, 12204)
		{
			this.WugType = 3;
		}
	}
}
