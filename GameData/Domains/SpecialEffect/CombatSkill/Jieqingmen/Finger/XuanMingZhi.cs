using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004F8 RID: 1272
	public class XuanMingZhi : AddInjuryByPoisonMark
	{
		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06003E52 RID: 15954 RVA: 0x002555F0 File Offset: 0x002537F0
		protected override bool AlsoAddAcupoint
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003E53 RID: 15955 RVA: 0x002555F3 File Offset: 0x002537F3
		public XuanMingZhi()
		{
		}

		// Token: 0x06003E54 RID: 15956 RVA: 0x002555FD File Offset: 0x002537FD
		public XuanMingZhi(CombatSkillKey skillKey) : base(skillKey, 13105)
		{
			this.RequirePoisonType = 2;
			this.IsInnerInjury = true;
		}
	}
}
