using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Neigong
{
	// Token: 0x02000222 RID: 546
	public class TaiYinLianXing : TransferFiveElementsNeili
	{
		// Token: 0x06002F43 RID: 12099 RVA: 0x00212662 File Offset: 0x00210862
		public TaiYinLianXing()
		{
		}

		// Token: 0x06002F44 RID: 12100 RVA: 0x0021266C File Offset: 0x0021086C
		public TaiYinLianXing(CombatSkillKey skillKey) : base(skillKey, 15004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 3 : 2);
			this.DestFiveElementsType = 4;
		}
	}
}
