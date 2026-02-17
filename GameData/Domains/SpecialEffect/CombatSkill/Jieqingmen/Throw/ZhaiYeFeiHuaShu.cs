using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jieqingmen.Throw
{
	// Token: 0x020004E2 RID: 1250
	public class ZhaiYeFeiHuaShu : GetTrick
	{
		// Token: 0x06003DDA RID: 15834 RVA: 0x00253AA3 File Offset: 0x00251CA3
		public ZhaiYeFeiHuaShu()
		{
		}

		// Token: 0x06003DDB RID: 15835 RVA: 0x00253AAD File Offset: 0x00251CAD
		public ZhaiYeFeiHuaShu(CombatSkillKey skillKey) : base(skillKey, 13301)
		{
			this.GetTrickType = 1;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				0,
				2
			};
		}
	}
}
