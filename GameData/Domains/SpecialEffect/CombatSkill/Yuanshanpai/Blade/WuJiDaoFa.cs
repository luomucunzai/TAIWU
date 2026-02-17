using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Blade
{
	// Token: 0x02000213 RID: 531
	public class WuJiDaoFa : AutoMoveAndCast
	{
		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x06002EF1 RID: 12017 RVA: 0x00211173 File Offset: 0x0020F373
		protected override bool MoveForward
		{
			get
			{
				return !base.IsDirect;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x06002EF2 RID: 12018 RVA: 0x0021117E File Offset: 0x0020F37E
		protected override short RequireWeaponType
		{
			get
			{
				return 9;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x06002EF3 RID: 12019 RVA: 0x00211182 File Offset: 0x0020F382
		protected override sbyte RequireSkillType
		{
			get
			{
				return 7;
			}
		}

		// Token: 0x06002EF4 RID: 12020 RVA: 0x00211185 File Offset: 0x0020F385
		public WuJiDaoFa()
		{
		}

		// Token: 0x06002EF5 RID: 12021 RVA: 0x0021118F File Offset: 0x0020F38F
		public WuJiDaoFa(CombatSkillKey skillKey) : base(skillKey, 5305)
		{
		}
	}
}
