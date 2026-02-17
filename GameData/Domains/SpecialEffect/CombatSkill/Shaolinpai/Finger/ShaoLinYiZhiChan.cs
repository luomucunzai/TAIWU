using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shaolinpai.Finger
{
	// Token: 0x02000430 RID: 1072
	public class ShaoLinYiZhiChan : AddCombatStateBySkillPower
	{
		// Token: 0x060039AD RID: 14765 RVA: 0x0023FCE3 File Offset: 0x0023DEE3
		public ShaoLinYiZhiChan()
		{
		}

		// Token: 0x060039AE RID: 14766 RVA: 0x0023FCF0 File Offset: 0x0023DEF0
		public ShaoLinYiZhiChan(CombatSkillKey skillKey) : base(skillKey, 1201)
		{
			this.StateTypes = new sbyte[]
			{
				1,
				2
			};
			this.StateIds = new short[]
			{
				8,
				9
			};
			bool[] array = new bool[2];
			array[0] = true;
			this.StateAddToSelf = array;
			this.StatePowerUnit = 20;
		}
	}
}
