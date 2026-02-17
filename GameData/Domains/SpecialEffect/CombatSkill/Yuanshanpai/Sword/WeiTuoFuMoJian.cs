using System;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Attack;

namespace GameData.Domains.SpecialEffect.CombatSkill.Yuanshanpai.Sword
{
	// Token: 0x020001F4 RID: 500
	public class WeiTuoFuMoJian : AutoMoveAndCast
	{
		// Token: 0x170001A2 RID: 418
		// (get) Token: 0x06002E4F RID: 11855 RVA: 0x0020E6A8 File Offset: 0x0020C8A8
		protected override bool MoveForward
		{
			get
			{
				return base.IsDirect;
			}
		}

		// Token: 0x170001A3 RID: 419
		// (get) Token: 0x06002E50 RID: 11856 RVA: 0x0020E6B0 File Offset: 0x0020C8B0
		protected override short RequireWeaponType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x170001A4 RID: 420
		// (get) Token: 0x06002E51 RID: 11857 RVA: 0x0020E6B3 File Offset: 0x0020C8B3
		protected override sbyte RequireSkillType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x06002E52 RID: 11858 RVA: 0x0020E6B6 File Offset: 0x0020C8B6
		public WeiTuoFuMoJian()
		{
		}

		// Token: 0x06002E53 RID: 11859 RVA: 0x0020E6C0 File Offset: 0x0020C8C0
		public WeiTuoFuMoJian(CombatSkillKey skillKey) : base(skillKey, 5205)
		{
		}
	}
}
