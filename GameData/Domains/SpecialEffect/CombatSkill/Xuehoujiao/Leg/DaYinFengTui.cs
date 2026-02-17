using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Xuehoujiao.Leg
{
	// Token: 0x02000229 RID: 553
	public class DaYinFengTui : AddInjuryByPoisonMark
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06002F53 RID: 12115 RVA: 0x0021276C File Offset: 0x0021096C
		protected override bool AlsoAddAcupoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002F54 RID: 12116 RVA: 0x0021276F File Offset: 0x0021096F
		public DaYinFengTui()
		{
		}

		// Token: 0x06002F55 RID: 12117 RVA: 0x00212779 File Offset: 0x00210979
		public DaYinFengTui(CombatSkillKey skillKey) : base(skillKey, 15305)
		{
			this.RequirePoisonType = 1;
			this.IsInnerInjury = true;
		}
	}
}
