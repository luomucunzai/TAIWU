using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.FistAndPalm
{
	// Token: 0x02000490 RID: 1168
	public class WeiLingXianHuaGuZhang : AddCombatStateBySkillPower
	{
		// Token: 0x06003C16 RID: 15382 RVA: 0x0024C047 File Offset: 0x0024A247
		public WeiLingXianHuaGuZhang()
		{
		}

		// Token: 0x06003C17 RID: 15383 RVA: 0x0024C054 File Offset: 0x0024A254
		public WeiLingXianHuaGuZhang(CombatSkillKey skillKey) : base(skillKey, 10103)
		{
			this.StateTypes = new sbyte[]
			{
				2,
				2
			};
			this.StateIds = new short[]
			{
				45,
				46
			};
			this.StateAddToSelf = new bool[2];
			this.StatePowerUnit = 20;
		}
	}
}
