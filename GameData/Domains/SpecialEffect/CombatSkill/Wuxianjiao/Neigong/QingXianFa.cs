using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.Neigong
{
	// Token: 0x02000391 RID: 913
	public class QingXianFa : TransferFiveElementsNeili
	{
		// Token: 0x06003652 RID: 13906 RVA: 0x0023054B File Offset: 0x0022E74B
		public QingXianFa()
		{
		}

		// Token: 0x06003653 RID: 13907 RVA: 0x00230555 File Offset: 0x0022E755
		public QingXianFa(CombatSkillKey skillKey) : base(skillKey, 12004)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 2 : 4);
			this.DestFiveElementsType = 1;
		}
	}
}
