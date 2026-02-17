using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Blade
{
	// Token: 0x020001D9 RID: 473
	public class CanDaoShi : BladeUnlockEffectBase
	{
		// Token: 0x17000170 RID: 368
		// (get) Token: 0x06002D71 RID: 11633 RVA: 0x0020BCD9 File Offset: 0x00209ED9
		private int AddAttackRange
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 40 : 20;
			}
		}

		// Token: 0x17000171 RID: 369
		// (get) Token: 0x06002D72 RID: 11634 RVA: 0x0020BCEC File Offset: 0x00209EEC
		protected override IEnumerable<short> RequireWeaponTypes
		{
			get
			{
				yield return 0;
				yield return 2;
				yield return 12;
				yield break;
			}
		}

		// Token: 0x06002D73 RID: 11635 RVA: 0x0020BD0B File Offset: 0x00209F0B
		public CanDaoShi()
		{
		}

		// Token: 0x06002D74 RID: 11636 RVA: 0x0020BD15 File Offset: 0x00209F15
		public CanDaoShi(CombatSkillKey skillKey) : base(skillKey, 9206)
		{
		}

		// Token: 0x06002D75 RID: 11637 RVA: 0x0020BD25 File Offset: 0x00209F25
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(145, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(146, EDataModifyType.Add, base.SkillTemplateId);
		}

		// Token: 0x06002D76 RID: 11638 RVA: 0x0020BD58 File Offset: 0x00209F58
		protected override void OnAffectedChanged(DataContext context)
		{
			base.OnAffectedChanged(context);
			base.InvalidateCache(context, 145);
			base.InvalidateCache(context, 146);
			bool affected = base.Affected;
			if (affected)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002D77 RID: 11639 RVA: 0x0020BDA1 File Offset: 0x00209FA1
		protected override bool CanDoAffect()
		{
			return base.CurrEnemyChar.GetAffectingMoveSkillId() >= 0;
		}

		// Token: 0x06002D78 RID: 11640 RVA: 0x0020BDB4 File Offset: 0x00209FB4
		public override void DoAffectAfterCost(DataContext context, int weaponIndex)
		{
			base.ShowSpecialEffectTips(base.IsDirect, 2, 1);
			short skillId = base.CurrEnemyChar.GetAffectingMoveSkillId();
			bool flag = !base.ClearAffectingAgileSkill(context, base.CurrEnemyChar);
			if (!flag)
			{
				DomainManager.Combat.AddGoneMadInjury(context, base.CurrEnemyChar, skillId, 0);
				DomainManager.Combat.SilenceSkill(context, base.CurrEnemyChar, skillId, 1800, 100);
			}
		}

		// Token: 0x06002D79 RID: 11641 RVA: 0x0020BE24 File Offset: 0x0020A024
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.SkillKey != this.SkillKey;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 145 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2 || !base.Affected;
			int result;
			if (flag4)
			{
				result = 0;
			}
			else
			{
				result = this.AddAttackRange;
			}
			return result;
		}

		// Token: 0x04000DAA RID: 3498
		private const int SilenceFrame = 1800;
	}
}
