using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x0200023C RID: 572
	public class SanZhiDuoYinZhua : AddBreakBodyFeature
	{
		// Token: 0x06002FA5 RID: 12197 RVA: 0x00213BF8 File Offset: 0x00211DF8
		public SanZhiDuoYinZhua()
		{
		}

		// Token: 0x06002FA6 RID: 12198 RVA: 0x00213C02 File Offset: 0x00211E02
		public SanZhiDuoYinZhua(CombatSkillKey skillKey) : base(skillKey, 15203)
		{
			this.AffectBodyParts = new sbyte[]
			{
				1
			};
		}
	}
}
