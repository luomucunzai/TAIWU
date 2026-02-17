using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x0200020F RID: 527
	public class QiXingDaoFa : StrengthenByWrongWeapon
	{
		// Token: 0x06002EE7 RID: 12007 RVA: 0x002110A5 File Offset: 0x0020F2A5
		public QiXingDaoFa()
		{
		}

		// Token: 0x06002EE8 RID: 12008 RVA: 0x002110AF File Offset: 0x0020F2AF
		public QiXingDaoFa(CombatSkillKey skillKey) : base(skillKey, 5304)
		{
			this.RequireWeaponSubType = 9;
		}
	}
}
