using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.Blade
{
	// Token: 0x0200052C RID: 1324
	public class FuLongDaoFa : ChangePowerByEquipType
	{
		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06003F6A RID: 16234 RVA: 0x00259C0B File Offset: 0x00257E0B
		protected override sbyte ChangePowerUnitReverse
		{
			get
			{
				return 3;
			}
		}

		// Token: 0x06003F6B RID: 16235 RVA: 0x00259C0E File Offset: 0x00257E0E
		public FuLongDaoFa()
		{
		}

		// Token: 0x06003F6C RID: 16236 RVA: 0x00259C18 File Offset: 0x00257E18
		public FuLongDaoFa(CombatSkillKey skillKey) : base(skillKey, 14200)
		{
			this.AffectEquipType = 1;
		}
	}
}
