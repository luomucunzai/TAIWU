using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001C4 RID: 452
	public class GuiTouBangFa : RawCreateUnlockEffectBase
	{
		// Token: 0x17000150 RID: 336
		// (get) Token: 0x06002CD1 RID: 11473 RVA: 0x002099A8 File Offset: 0x00207BA8
		private CValuePercent CastAddUnlockPercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 12 : 6;
			}
		}

		// Token: 0x17000151 RID: 337
		// (get) Token: 0x06002CD2 RID: 11474 RVA: 0x002099BC File Offset: 0x00207BBC
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[]
		{
			1,
			5
		};

		// Token: 0x17000152 RID: 338
		// (get) Token: 0x06002CD3 RID: 11475 RVA: 0x002099C4 File Offset: 0x00207BC4
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 65;
			}
		}

		// Token: 0x06002CD4 RID: 11476 RVA: 0x002099C8 File Offset: 0x00207BC8
		public GuiTouBangFa()
		{
		}

		// Token: 0x06002CD5 RID: 11477 RVA: 0x002099E6 File Offset: 0x00207BE6
		public GuiTouBangFa(CombatSkillKey skillKey) : base(skillKey, 9303)
		{
		}

		// Token: 0x06002CD6 RID: 11478 RVA: 0x00209A0C File Offset: 0x00207C0C
		protected override void OnCastAddUnlockAttackValue(DataContext context, CValuePercent power)
		{
			base.OnCastAddUnlockAttackValue(context, power);
			bool flag = !base.IsReverseOrUsingDirectWeapon;
			if (!flag)
			{
				base.CombatChar.ChangeAllUnlockAttackValue(context, this.CastAddUnlockPercent * power);
				base.ShowSpecialEffectTipsOnceInFrame(0);
			}
		}
	}
}
