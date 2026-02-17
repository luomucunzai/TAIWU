using System;
using System.Collections.Generic;
using Config;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B6 RID: 438
	public class LongYuanQiXingJianFa : SwordUnlockEffectBase
	{
		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06002C60 RID: 11360 RVA: 0x00207518 File Offset: 0x00205718
		private int SelfReduceBreathStance
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? -60 : -30;
			}
		}

		// Token: 0x17000141 RID: 321
		// (get) Token: 0x06002C61 RID: 11361 RVA: 0x00207528 File Offset: 0x00205728
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 1;
				yield break;
			}
		}

		// Token: 0x17000142 RID: 322
		// (get) Token: 0x06002C62 RID: 11362 RVA: 0x00207547 File Offset: 0x00205747
		protected override int RequirePersonalityValue
		{
			get
			{
				return 50;
			}
		}

		// Token: 0x06002C63 RID: 11363 RVA: 0x0020754B File Offset: 0x0020574B
		public LongYuanQiXingJianFa()
		{
		}

		// Token: 0x06002C64 RID: 11364 RVA: 0x00207555 File Offset: 0x00205755
		public LongYuanQiXingJianFa(CombatSkillKey skillKey) : base(skillKey, 9104)
		{
		}

		// Token: 0x06002C65 RID: 11365 RVA: 0x00207565 File Offset: 0x00205765
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(204, EDataModifyType.AddPercent, -1);
			Events.RegisterHandler_CastSkillCosted(new Events.OnCastSkillCosted(this.OnCastSkillCosted));
		}

		// Token: 0x06002C66 RID: 11366 RVA: 0x00207590 File Offset: 0x00205790
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillCosted(new Events.OnCastSkillCosted(this.OnCastSkillCosted));
			base.OnDisable(context);
		}

		// Token: 0x06002C67 RID: 11367 RVA: 0x002075B0 File Offset: 0x002057B0
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

		// Token: 0x06002C68 RID: 11368 RVA: 0x00207604 File Offset: 0x00205804
		protected override void OnAddedEffectCount(DataContext context)
		{
			base.OnAddedEffectCount(context);
			base.InvalidateCache(context, 204);
			DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
		}

		// Token: 0x06002C69 RID: 11369 RVA: 0x00207630 File Offset: 0x00205830
		private void OnCastSkillCosted(DataContext context, CombatCharacter combatChar, short skillId)
		{
			bool flag = combatChar.GetId() != base.CharacterId || base.EffectCount <= 0 || !CombatSkillTemplateHelper.IsAttack(skillId);
			if (!flag)
			{
				bool flag2 = Config.CombatSkill.Instance[skillId].BreathStanceTotalCost <= 0;
				if (!flag2)
				{
					base.ReduceEffectCount(1);
					bool flag3 = base.EffectCount > 0;
					if (!flag3)
					{
						base.InvalidateCache(context, 204);
						DomainManager.Combat.UpdateSkillCanUse(context, base.CombatChar);
					}
				}
			}
		}

		// Token: 0x06002C6A RID: 11370 RVA: 0x002076BC File Offset: 0x002058BC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.FieldId != 204;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.IsNormalAttack || !CombatSkillTemplateHelper.IsAttack(dataKey.CombatSkillId);
				if (flag2)
				{
					result = 0;
				}
				else
				{
					int value = 0;
					bool flag3 = base.Affected && dataKey.CombatSkillId == base.SkillTemplateId;
					if (flag3)
					{
						value += this.SelfReduceBreathStance;
					}
					bool flag4 = base.EffectCount > 0;
					if (flag4)
					{
						value += -30;
					}
					result = value;
				}
			}
			return result;
		}

		// Token: 0x04000D6A RID: 3434
		private const int EffectReduceBreathStance = -30;
	}
}
