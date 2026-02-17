using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.BreakBodyEffect;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.FistAndPalm
{
	// Token: 0x02000237 RID: 567
	public class XueShaZhang : AddBreakBodyFeature
	{
		// Token: 0x06002F95 RID: 12181 RVA: 0x00213A5B File Offset: 0x00211C5B
		public XueShaZhang()
		{
		}

		// Token: 0x06002F96 RID: 12182 RVA: 0x00213A65 File Offset: 0x00211C65
		public XueShaZhang(CombatSkillKey skillKey) : base(skillKey, 15104)
		{
			this.AffectBodyParts = new sbyte[1];
		}
	}
}
