using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C9 RID: 457
	public class SaoXiaGunFa : RawCreateUnlockEffectBase
	{
		// Token: 0x17000160 RID: 352
		// (get) Token: 0x06002CFD RID: 11517 RVA: 0x0020A205 File Offset: 0x00208405
		private CValuePercent ReduceMobilityPercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? -20 : -10;
			}
		}

		// Token: 0x17000161 RID: 353
		// (get) Token: 0x06002CFE RID: 11518 RVA: 0x0020A21C File Offset: 0x0020841C
		protected override IEnumerable<sbyte> RequireMainAttributeTypes
		{
			get
			{
				yield return 1;
				yield break;
			}
		}

		// Token: 0x17000162 RID: 354
		// (get) Token: 0x06002CFF RID: 11519 RVA: 0x0020A23B File Offset: 0x0020843B
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x06002D00 RID: 11520 RVA: 0x0020A23F File Offset: 0x0020843F
		public SaoXiaGunFa()
		{
		}

		// Token: 0x06002D01 RID: 11521 RVA: 0x0020A249 File Offset: 0x00208449
		public SaoXiaGunFa(CombatSkillKey skillKey) : base(skillKey, 9301)
		{
		}

		// Token: 0x06002D02 RID: 11522 RVA: 0x0020A25C File Offset: 0x0020845C
		protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
		{
			base.OnCastAddUnlockAttackValue(context, power);
			bool flag = !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				int changeValue = MoveSpecialConstants.MaxMobility * this.ReduceMobilityPercent * power;
				base.ChangeMobilityValue(context, base.EnemyChar, changeValue);
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}
	}
}
