using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Assist
{
	// Token: 0x02000327 RID: 807
	public class JiuYinShiYou : AddDamageByFiveElementsType
	{
		// Token: 0x06003451 RID: 13393 RVA: 0x00228589 File Offset: 0x00226789
		public JiuYinShiYou()
		{
		}

		// Token: 0x06003452 RID: 13394 RVA: 0x00228593 File Offset: 0x00226793
		public JiuYinShiYou(CombatSkillKey skillKey) : base(skillKey, 16402)
		{
			this.CounteringType = 3;
			this.CounteredType = 4;
		}
	}
}
