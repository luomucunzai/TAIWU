using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E2 RID: 482
	public abstract class PolearmUnlockEffectBase : WeaponUnlockEffectBase
	{
		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06002DCE RID: 11726 RVA: 0x0020D126 File Offset: 0x0020B326
		protected override short WeaponType
		{
			get
			{
				return 10;
			}
		}

		// Token: 0x17000187 RID: 391
		// (get) Token: 0x06002DCF RID: 11727 RVA: 0x0020D12A File Offset: 0x0020B32A
		protected override CValuePercent DirectAddUnlockPercent
		{
			get
			{
				return 16;
			}
		}

		// Token: 0x17000188 RID: 392
		// (get) Token: 0x06002DD0 RID: 11728 RVA: 0x0020D133 File Offset: 0x0020B333
		protected override bool ReverseEffectDoubling
		{
			get
			{
				return this.RequireMainAttributeTypes.All((sbyte x) => (int)base.CombatChar.GetCharacter().GetMaxMainAttribute(x) >= this.RequireMainAttributeValue);
			}
		}

		// Token: 0x17000189 RID: 393
		// (get) Token: 0x06002DD1 RID: 11729
		protected abstract IEnumerable<sbyte> RequireMainAttributeTypes { get; }

		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06002DD2 RID: 11730
		protected abstract int RequireMainAttributeValue { get; }

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0020D14C File Offset: 0x0020B34C
		protected PolearmUnlockEffectBase()
		{
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0020D156 File Offset: 0x0020B356
		protected PolearmUnlockEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}
	}
}
