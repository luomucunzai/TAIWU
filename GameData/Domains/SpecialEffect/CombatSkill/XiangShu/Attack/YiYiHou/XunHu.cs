using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Attack.YiYiHou
{
	// Token: 0x020002C5 RID: 709
	public class XunHu : AddWuTrick
	{
		// Token: 0x0600326F RID: 12911 RVA: 0x0021F764 File Offset: 0x0021D964
		public XunHu()
		{
		}

		// Token: 0x06003270 RID: 12912 RVA: 0x0021F76E File Offset: 0x0021D96E
		public XunHu(CombatSkillKey skillKey) : base(skillKey, 17040)
		{
			this.AddTrickCount = 3;
		}
	}
}
