using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F1 RID: 497
	public class LuoHanJianFa : WeaponAddAttackPrepareValue
	{
		// Token: 0x170001A0 RID: 416
		// (get) Token: 0x06002E42 RID: 11842 RVA: 0x0020E3F5 File Offset: 0x0020C5F5
		protected override int RequireWeaponSubType
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170001A1 RID: 417
		// (get) Token: 0x06002E43 RID: 11843 RVA: 0x0020E3F9 File Offset: 0x0020C5F9
		protected override int DirectSrcWeaponSubType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002E44 RID: 11844 RVA: 0x0020E3FC File Offset: 0x0020C5FC
		public LuoHanJianFa()
		{
		}

		// Token: 0x06002E45 RID: 11845 RVA: 0x0020E406 File Offset: 0x0020C606
		public LuoHanJianFa(CombatSkillKey skillKey) : base(skillKey, 5200)
		{
		}
	}
}
