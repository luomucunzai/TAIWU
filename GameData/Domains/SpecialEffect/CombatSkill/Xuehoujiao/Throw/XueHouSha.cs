using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Throw
{
	// Token: 0x0200021D RID: 541
	public class XueHouSha : StrengthenPoison
	{
		// Token: 0x06002F2A RID: 12074 RVA: 0x002120C8 File Offset: 0x002102C8
		public XueHouSha()
		{
		}

		// Token: 0x06002F2B RID: 12075 RVA: 0x002120D2 File Offset: 0x002102D2
		public XueHouSha(CombatSkillKey skillKey) : base(skillKey, 15406)
		{
			this.AffectPoisonType = 3;
		}
	}
}
