using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Finger
{
	// Token: 0x020004FA RID: 1274
	public class ZhaiXingShi : GetTrick
	{
		// Token: 0x06003E5C RID: 15964 RVA: 0x002558E0 File Offset: 0x00253AE0
		public ZhaiXingShi()
		{
		}

		// Token: 0x06003E5D RID: 15965 RVA: 0x002558EA File Offset: 0x00253AEA
		public ZhaiXingShi(CombatSkillKey skillKey) : base(skillKey, 13101)
		{
			this.GetTrickType = 7;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				6,
				8
			};
		}
	}
}
