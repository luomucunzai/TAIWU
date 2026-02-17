using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.Finger
{
	// Token: 0x02000492 RID: 1170
	public class ChaiShanQinDieShou : AddCombatStateBySkillPower
	{
		// Token: 0x06003C1D RID: 15389 RVA: 0x0024C0D8 File Offset: 0x0024A2D8
		public ChaiShanQinDieShou()
		{
		}

		// Token: 0x06003C1E RID: 15390 RVA: 0x0024C0E4 File Offset: 0x0024A2E4
		public ChaiShanQinDieShou(CombatSkillKey skillKey) : base(skillKey, 10200)
		{
			this.StateTypes = new sbyte[]
			{
				2,
				2
			};
			this.StateIds = new short[]
			{
				43,
				44
			};
			this.StateAddToSelf = new bool[2];
			this.StatePowerUnit = 20;
		}
	}
}
