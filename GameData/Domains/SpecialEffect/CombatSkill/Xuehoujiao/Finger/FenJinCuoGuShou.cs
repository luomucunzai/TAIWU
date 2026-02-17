using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x0200023A RID: 570
	public class FenJinCuoGuShou : AddBreakBodyFeature
	{
		// Token: 0x06002F9E RID: 12190 RVA: 0x00213B9F File Offset: 0x00211D9F
		public FenJinCuoGuShou()
		{
		}

		// Token: 0x06002F9F RID: 12191 RVA: 0x00213BA9 File Offset: 0x00211DA9
		public FenJinCuoGuShou(CombatSkillKey skillKey) : base(skillKey, 15202)
		{
			this.AffectBodyParts = new sbyte[]
			{
				3,
				4
			};
		}
	}
}
