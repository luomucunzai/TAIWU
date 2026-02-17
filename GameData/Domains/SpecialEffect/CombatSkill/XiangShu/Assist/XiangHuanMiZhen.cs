using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000331 RID: 817
	public class XiangHuanMiZhen : AddDamageByHitType
	{
		// Token: 0x06003480 RID: 13440 RVA: 0x00228DF9 File Offset: 0x00226FF9
		public XiangHuanMiZhen()
		{
		}

		// Token: 0x06003481 RID: 13441 RVA: 0x00228E03 File Offset: 0x00227003
		public XiangHuanMiZhen(CombatSkillKey skillKey) : base(skillKey, 16404)
		{
			this.HitType = 3;
		}
	}
}
