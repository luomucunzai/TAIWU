using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000226 RID: 550
	public class XueHouXieJing : KeepSkillCanCast
	{
		// Token: 0x06002F4D RID: 12109 RVA: 0x00212709 File Offset: 0x00210909
		public XueHouXieJing()
		{
		}

		// Token: 0x06002F4E RID: 12110 RVA: 0x00212713 File Offset: 0x00210913
		public XueHouXieJing(CombatSkillKey skillKey) : base(skillKey, 15007)
		{
			this.RequireFiveElementsType = 4;
		}
	}
}
