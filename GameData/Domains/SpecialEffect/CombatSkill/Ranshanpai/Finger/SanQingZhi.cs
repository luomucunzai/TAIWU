using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Ranshanpai.Finger
{
	// Token: 0x0200045F RID: 1119
	public class SanQingZhi : ReduceEnemyTrick
	{
		// Token: 0x06003AE6 RID: 15078 RVA: 0x00245BCF File Offset: 0x00243DCF
		public SanQingZhi()
		{
		}

		// Token: 0x06003AE7 RID: 15079 RVA: 0x00245BD9 File Offset: 0x00243DD9
		public SanQingZhi(CombatSkillKey skillKey) : base(skillKey, 7100)
		{
			this.AffectTrickType = 7;
		}
	}
}
