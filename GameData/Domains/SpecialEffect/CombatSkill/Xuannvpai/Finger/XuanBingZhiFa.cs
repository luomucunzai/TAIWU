using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuannvpai.Finger
{
	// Token: 0x0200027C RID: 636
	public class XuanBingZhiFa : AddOrReduceNeiliAllocation
	{
		// Token: 0x170001C7 RID: 455
		// (get) Token: 0x060030D1 RID: 12497 RVA: 0x00218A88 File Offset: 0x00216C88
		protected override sbyte NeiliAllocationChange
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x060030D2 RID: 12498 RVA: 0x00218A8B File Offset: 0x00216C8B
		public XuanBingZhiFa()
		{
		}

		// Token: 0x060030D3 RID: 12499 RVA: 0x00218A95 File Offset: 0x00216C95
		public XuanBingZhiFa(CombatSkillKey skillKey) : base(skillKey, 8201)
		{
			this.AffectNeiliAllocationType = 1;
		}
	}
}
