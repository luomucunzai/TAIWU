using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Sword
{
	// Token: 0x02000444 RID: 1092
	public class SanQingJianFa : AddOrReduceNeiliAllocation
	{
		// Token: 0x1700020C RID: 524
		// (get) Token: 0x06003A21 RID: 14881 RVA: 0x0024225E File Offset: 0x0024045E
		protected override sbyte NeiliAllocationChange
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06003A22 RID: 14882 RVA: 0x00242261 File Offset: 0x00240461
		public SanQingJianFa()
		{
		}

		// Token: 0x06003A23 RID: 14883 RVA: 0x0024226B File Offset: 0x0024046B
		public SanQingJianFa(CombatSkillKey skillKey) : base(skillKey, 7201)
		{
			this.AffectNeiliAllocationType = 3;
		}
	}
}
