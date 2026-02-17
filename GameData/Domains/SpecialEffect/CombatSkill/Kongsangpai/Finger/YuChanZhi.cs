using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000497 RID: 1175
	public class YuChanZhi : ChangePoisonType
	{
		// Token: 0x06003C34 RID: 15412 RVA: 0x0024C583 File Offset: 0x0024A783
		public YuChanZhi()
		{
		}

		// Token: 0x06003C35 RID: 15413 RVA: 0x0024C58D File Offset: 0x0024A78D
		public YuChanZhi(CombatSkillKey skillKey) : base(skillKey, 10203)
		{
			this.CanChangePoisonType = new sbyte[]
			{
				1,
				2,
				5
			};
			this.AddPowerPoisonType = 5;
		}
	}
}
