using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Finger
{
	// Token: 0x02000238 RID: 568
	public class ChuoYanXiaoErGong : AddBreakBodyFeature
	{
		// Token: 0x06002F97 RID: 12183 RVA: 0x00213A81 File Offset: 0x00211C81
		public ChuoYanXiaoErGong()
		{
		}

		// Token: 0x06002F98 RID: 12184 RVA: 0x00213A8B File Offset: 0x00211C8B
		public ChuoYanXiaoErGong(CombatSkillKey skillKey) : base(skillKey, 15200)
		{
			this.AffectBodyParts = new sbyte[]
			{
				2
			};
		}
	}
}
