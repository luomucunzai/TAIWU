using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001DA RID: 474
	public class ChiDaoQiJue : BladeUnlockEffectBase
	{
		// Token: 0x17000172 RID: 370
		// (get) Token: 0x06002D7A RID: 11642 RVA: 0x0020BE8F File Offset: 0x0020A08F
		private int ReduceBreathStance
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? -50 : -25;
			}
		}

		// Token: 0x17000173 RID: 371
		// (get) Token: 0x06002D7B RID: 11643 RVA: 0x0020BEA0 File Offset: 0x0020A0A0
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 1;
				yield return 5;
				yield return 13;
				yield break;
			}
		}

		// Token: 0x06002D7C RID: 11644 RVA: 0x0020BEBF File Offset: 0x0020A0BF
		public ChiDaoQiJue()
		{
		}

		// Token: 0x06002D7D RID: 11645 RVA: 0x0020BEC9 File Offset: 0x0020A0C9
		public ChiDaoQiJue(CombatSkillKey skillKey) : base(skillKey, 9205)
		{
		}

		// Token: 0x06002D7E RID: 11646 RVA: 0x0020BED9 File Offset: 0x0020A0D9
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(204, EDataModifyType.AddPercent, -1);
		}

		// Token: 0x06002D7F RID: 11647 RVA: 0x0020BEF4 File Offset: 0x0020A0F4
		protected override void OnAffectedChanged(DataContext context)
		{
			base.OnAffectedChanged(context);
			base.InvalidateCache(context, 204);
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar, base.SkillTemplateId);
			bool affected = base.Affected;
			if (affected)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002D80 RID: 11648 RVA: 0x0020BF48 File Offset: 0x0020A148
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			base.AbsorbBreathValue(context, base.CurrEnemyChar, 40);
			base.AbsorbStanceValue(context, base.CurrEnemyChar, 40);
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
		}

		// Token: 0x06002D81 RID: 11649 RVA: 0x0020BF84 File Offset: 0x0020A184
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey || dataKey.FieldId != 204 || !base.Affected;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				result = this.ReduceBreathStance;
			}
			return result;
		}

		// Token: 0x04000DAB RID: 3499
		private const int AbsorbBreathAndStancePercent = 40;
	}
}
