using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon
{
	// Token: 0x020001E6 RID: 486
	public abstract class SwordUnlockEffectBase : WeaponUnlockEffectBase, IExtraUnlockEffect
	{
		// Token: 0x17000191 RID: 401
		// (get) Token: 0x06002DEF RID: 11759 RVA: 0x0020D644 File Offset: 0x0020B844
		protected override short WeaponType
		{
			get
			{
				return 8;
			}
		}

		// Token: 0x17000192 RID: 402
		// (get) Token: 0x06002DF0 RID: 11760 RVA: 0x0020D647 File Offset: 0x0020B847
		protected override CValuePercent DirectAddUnlockPercent
		{
			get
			{
				return 32;
			}
		}

		// Token: 0x17000193 RID: 403
		// (get) Token: 0x06002DF1 RID: 11761 RVA: 0x0020D650 File Offset: 0x0020B850
		protected override bool ReverseEffectDoubling
		{
			get
			{
				return this.RequirePersonalityTypes.All((sbyte x) => (int)base.CombatChar.GetCharacter().GetPersonality(x) >= this.RequirePersonalityValue);
			}
		}

		// Token: 0x17000194 RID: 404
		// (get) Token: 0x06002DF2 RID: 11762
		protected abstract IEnumerable<sbyte> RequirePersonalityTypes { get; }

		// Token: 0x17000195 RID: 405
		// (get) Token: 0x06002DF3 RID: 11763
		protected abstract int RequirePersonalityValue { get; }

		// Token: 0x06002DF4 RID: 11764 RVA: 0x0020D669 File Offset: 0x0020B869
		protected SwordUnlockEffectBase()
		{
		}

		// Token: 0x06002DF5 RID: 11765 RVA: 0x0020D673 File Offset: 0x0020B873
		protected SwordUnlockEffectBase(CombatSkillKey skillKey, int type) : base(skillKey, type)
		{
		}

		// Token: 0x06002DF6 RID: 11766 RVA: 0x0020D67F File Offset: 0x0020B87F
		protected sealed override void DoAffect(DataContext context, int weaponIndex)
		{
			base.CombatChar.InvokeExtraUnlockEffect(this, weaponIndex);
		}

		// Token: 0x06002DF7 RID: 11767 RVA: 0x0020D690 File Offset: 0x0020B890
		public void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			base.AddMaxEffectCount(true);
			this.OnAddedEffectCount(context);
		}

		// Token: 0x06002DF8 RID: 11768 RVA: 0x0020D6A3 File Offset: 0x0020B8A3
		protected virtual void OnAddedEffectCount(DataContext context)
		{
		}
	}
}
