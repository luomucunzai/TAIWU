using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F0 RID: 496
	public class DaMoJianFa : ExtraBreathOrStance
	{
		// Token: 0x1700019F RID: 415
		// (get) Token: 0x06002E3F RID: 11839 RVA: 0x0020E3D8 File Offset: 0x0020C5D8
		protected override bool IsBreath
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06002E40 RID: 11840 RVA: 0x0020E3DB File Offset: 0x0020C5DB
		public DaMoJianFa()
		{
		}

		// Token: 0x06002E41 RID: 11841 RVA: 0x0020E3E5 File Offset: 0x0020C5E5
		public DaMoJianFa(CombatSkillKey skillKey) : base(skillKey, 5207)
		{
		}
	}
}
