using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Throw
{
	// Token: 0x02000510 RID: 1296
	public class LongTuZhu : AddOrReduceNeiliAllocation
	{
		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06003EDB RID: 16091 RVA: 0x0025771F File Offset: 0x0025591F
		protected override sbyte NeiliAllocationChange
		{
			get
			{
				return 5;
			}
		}

		// Token: 0x06003EDC RID: 16092 RVA: 0x00257722 File Offset: 0x00255922
		public LongTuZhu()
		{
		}

		// Token: 0x06003EDD RID: 16093 RVA: 0x0025772C File Offset: 0x0025592C
		public LongTuZhu(CombatSkillKey skillKey) : base(skillKey, 14302)
		{
			this.AffectNeiliAllocationType = 0;
		}
	}
}
