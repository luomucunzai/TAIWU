using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Polearm
{
	// Token: 0x02000416 RID: 1046
	public class ShaoLinYinYangGun : GetTrick
	{
		// Token: 0x06003933 RID: 14643 RVA: 0x0023DAF7 File Offset: 0x0023BCF7
		public ShaoLinYinYangGun()
		{
		}

		// Token: 0x06003934 RID: 14644 RVA: 0x0023DB01 File Offset: 0x0023BD01
		public ShaoLinYinYangGun(CombatSkillKey skillKey) : base(skillKey, 1301)
		{
			this.GetTrickType = 5;
			this.DirectCanChangeTrickType = new sbyte[]
			{
				3,
				4
			};
		}
	}
}
