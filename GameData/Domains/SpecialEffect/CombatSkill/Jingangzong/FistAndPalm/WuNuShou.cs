using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.FistAndPalm
{
	// Token: 0x020004C2 RID: 1218
	public class WuNuShou : AddCombatStateBySkillPower
	{
		// Token: 0x06003D0D RID: 15629 RVA: 0x0024FA9F File Offset: 0x0024DC9F
		public WuNuShou()
		{
		}

		// Token: 0x06003D0E RID: 15630 RVA: 0x0024FAAC File Offset: 0x0024DCAC
		public WuNuShou(CombatSkillKey skillKey) : base(skillKey, 11101)
		{
			this.StateTypes = new sbyte[]
			{
				1,
				1
			};
			this.StateIds = new short[]
			{
				53,
				54
			};
			this.StateAddToSelf = new bool[]
			{
				true,
				true
			};
			this.StatePowerUnit = 20;
		}
	}
}
