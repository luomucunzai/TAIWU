using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Throw
{
	// Token: 0x0200047A RID: 1146
	public class HeiShuiDuanChangSan : AddInjuryByPoisonMark
	{
		// Token: 0x17000221 RID: 545
		// (get) Token: 0x06003B84 RID: 15236 RVA: 0x002486D6 File Offset: 0x002468D6
		protected override bool AlsoAddFlaw
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06003B85 RID: 15237 RVA: 0x002486D9 File Offset: 0x002468D9
		public HeiShuiDuanChangSan()
		{
		}

		// Token: 0x06003B86 RID: 15238 RVA: 0x002486E3 File Offset: 0x002468E3
		public HeiShuiDuanChangSan(CombatSkillKey skillKey) : base(skillKey, 10405)
		{
			this.RequirePoisonType = 0;
			this.IsInnerInjury = false;
		}
	}
}
