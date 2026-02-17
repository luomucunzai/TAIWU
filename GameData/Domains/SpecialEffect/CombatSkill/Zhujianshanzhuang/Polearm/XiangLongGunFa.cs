using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Polearm
{
	// Token: 0x020001CB RID: 459
	public class XiangLongGunFa : PolearmUnlockEffectBase, IExtraUnlockEffect
	{
		// Token: 0x17000166 RID: 358
		// (get) Token: 0x06002D0B RID: 11531 RVA: 0x0020A3F7 File Offset: 0x002085F7
		private CValuePercent CastAddUnlockPercent
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 8 : 4;
			}
		}

		// Token: 0x17000167 RID: 359
		// (get) Token: 0x06002D0C RID: 11532 RVA: 0x0020A40A File Offset: 0x0020860A
		private static CValuePercent UnlockAddUnlockPercent
		{
			get
			{
				return 15;
			}
		}

		// Token: 0x17000168 RID: 360
		// (get) Token: 0x06002D0D RID: 11533 RVA: 0x0020A413 File Offset: 0x00208613
		protected override IEnumerable<sbyte> RequireMainAttributeTypes { get; } = new sbyte[1];

		// Token: 0x17000169 RID: 361
		// (get) Token: 0x06002D0E RID: 11534 RVA: 0x0020A41B File Offset: 0x0020861B
		protected override int RequireMainAttributeValue
		{
			get
			{
				return 75;
			}
		}

		// Token: 0x06002D0F RID: 11535 RVA: 0x0020A41F File Offset: 0x0020861F
		public XiangLongGunFa()
		{
		}

		// Token: 0x06002D10 RID: 11536 RVA: 0x0020A435 File Offset: 0x00208635
		public XiangLongGunFa(CombatSkillKey skillKey) : base(skillKey, 9300)
		{
		}

		// Token: 0x06002D11 RID: 11537 RVA: 0x0020A454 File Offset: 0x00208654
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

		// Token: 0x06002D12 RID: 11538 RVA: 0x0020A49B File Offset: 0x0020869B
		protected override void DoAffect(DataContext context, int weaponIndex)
		{
			base.CombatChar.InvokeExtraUnlockEffect(this, weaponIndex);
		}

		// Token: 0x06002D13 RID: 11539 RVA: 0x0020A4AC File Offset: 0x002086AC
		public void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			base.CombatChar.ChangeAllUnlockAttackValue(context, XiangLongGunFa.UnlockAddUnlockPercent);
			base.ShowSpecialEffectTips(0);
		}
	}
}
