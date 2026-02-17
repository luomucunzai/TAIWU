using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x0200020C RID: 524
	public class BaGuaWuXingDao : WeaponAddAttackPrepareValue
	{
		// Token: 0x170001AD RID: 429
		// (get) Token: 0x06002EDC RID: 11996 RVA: 0x00210FA9 File Offset: 0x0020F1A9
		protected override int RequireWeaponSubType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x06002EDD RID: 11997 RVA: 0x00210FAC File Offset: 0x0020F1AC
		protected override int DirectSrcWeaponSubType
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x06002EDE RID: 11998 RVA: 0x00210FB0 File Offset: 0x0020F1B0
		public BaGuaWuXingDao()
		{
		}

		// Token: 0x06002EDF RID: 11999 RVA: 0x00210FBA File Offset: 0x0020F1BA
		public BaGuaWuXingDao(CombatSkillKey skillKey) : base(skillKey, 5300)
		{
		}
	}
}
