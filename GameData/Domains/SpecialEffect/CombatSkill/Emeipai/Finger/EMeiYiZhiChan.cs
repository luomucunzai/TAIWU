using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Emeipai.Finger
{
	// Token: 0x02000559 RID: 1369
	public class EMeiYiZhiChan : AddCombatStateBySkillPower
	{
		// Token: 0x06004079 RID: 16505 RVA: 0x0025E3D1 File Offset: 0x0025C5D1
		public EMeiYiZhiChan()
		{
		}

		// Token: 0x0600407A RID: 16506 RVA: 0x0025E3DC File Offset: 0x0025C5DC
		public EMeiYiZhiChan(CombatSkillKey skillKey) : base(skillKey, 2201)
		{
			this.StateTypes = new sbyte[]
			{
				1,
				2
			};
			this.StateIds = new short[]
			{
				17,
				18
			};
			bool[] array = new bool[2];
			array[0] = true;
			this.StateAddToSelf = array;
			this.StatePowerUnit = 20;
		}
	}
}
