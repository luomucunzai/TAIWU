using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Neigong;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.Neigong
{
	// Token: 0x020004B4 RID: 1204
	public class DaRiJing : TransferFiveElementsNeili
	{
		// Token: 0x06003CE3 RID: 15587 RVA: 0x0024F485 File Offset: 0x0024D685
		public DaRiJing()
		{
		}

		// Token: 0x06003CE4 RID: 15588 RVA: 0x0024F48F File Offset: 0x0024D68F
		public DaRiJing(CombatSkillKey skillKey) : base(skillKey, 11005)
		{
			this.SrcFiveElementsType = (base.IsDirect ? 4 : 3);
			this.DestFiveElementsType = 0;
		}
	}
}
