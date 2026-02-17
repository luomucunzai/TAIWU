using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.AttackCommon;

namespace GameData.Domains.SpecialEffect.CombatSkill.Zhujianshanzhuang.Sword
{
	// Token: 0x020001B9 RID: 441
	public class TaiEWuLiangJian : SwordUnlockEffectBase
	{
		// Token: 0x1700014A RID: 330
		// (get) Token: 0x06002C79 RID: 11385 RVA: 0x002078EA File Offset: 0x00205AEA
		private int SelfAddAttackRange
		{
			get
			{
				return base.IsDirectOrReverseEffectDoubling ? 40 : 20;
			}
		}

		// Token: 0x1700014B RID: 331
		// (get) Token: 0x06002C7A RID: 11386 RVA: 0x002078FC File Offset: 0x00205AFC
		protected override IEnumerable<sbyte> RequirePersonalityTypes
		{
			get
			{
				yield return 2;
				yield return 3;
				yield return 0;
				yield return 1;
				yield return 4;
				yield break;
			}
		}

		// Token: 0x1700014C RID: 332
		// (get) Token: 0x06002C7B RID: 11387 RVA: 0x0020791B File Offset: 0x00205B1B
		protected override int RequirePersonalityValue
		{
			get
			{
				return 30;
			}
		}

		// Token: 0x06002C7C RID: 11388 RVA: 0x0020791F File Offset: 0x00205B1F
		public TaiEWuLiangJian()
		{
		}

		// Token: 0x06002C7D RID: 11389 RVA: 0x00207929 File Offset: 0x00205B29
		public TaiEWuLiangJian(CombatSkillKey skillKey) : base(skillKey, 9105)
		{
		}

		// Token: 0x06002C7E RID: 11390 RVA: 0x0020793C File Offset: 0x00205B3C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			base.CreateAffectedData(145, EDataModifyType.Add, base.SkillTemplateId);
			base.CreateAffectedData(146, EDataModifyType.Add, base.SkillTemplateId);
			Events.RegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
		}

		// Token: 0x06002C7F RID: 11391 RVA: 0x0020798A File Offset: 0x00205B8A
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CastSkillEnd(new Events.OnCastSkillEnd(this.OnCastSkillEnd));
			base.OnDisable(context);
		}

		// Token: 0x06002C80 RID: 11392 RVA: 0x002079A8 File Offset: 0x00205BA8
		protected override void OnAffectedChanged(DataContext context)
		{
			base.OnAffectedChanged(context);
			this.InvalidAllCaches(context);
			bool affected = base.Affected;
			if (affected)
			{
				base.ShowSpecialEffectTips(base.IsDirect, 1, 0);
			}
		}

		// Token: 0x06002C81 RID: 11393 RVA: 0x002079DF File Offset: 0x00205BDF
		protected override void OnAddedEffectCount(DataContext context)
		{
			base.OnAddedEffectCount(context);
			this.InvalidAllCaches(context);
		}

		// Token: 0x06002C82 RID: 11394 RVA: 0x002079F4 File Offset: 0x00205BF4
		private void OnCastSkillEnd(DataContext context, int charId, bool isAlly, short skillId, sbyte power, bool interrupted)
		{
			bool flag = charId != base.CharacterId || !CombatSkillTemplateHelper.IsAttack(skillId) || base.EffectCount <= 0;
			if (!flag)
			{
				base.ReduceEffectCount(1);
				bool flag2 = base.EffectCount != 0;
				if (!flag2)
				{
					this.InvalidAllCaches(context);
				}
			}
		}

		// Token: 0x06002C83 RID: 11395 RVA: 0x00207A48 File Offset: 0x00205C48
		private void InvalidAllCaches(DataContext context)
		{
			base.InvalidateCache(context, 145);
			base.InvalidateCache(context, 146);
		}

		// Token: 0x06002C84 RID: 11396 RVA: 0x00207A68 File Offset: 0x00205C68
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.IsNormalAttack;
			bool flag2 = flag;
			if (!flag2)
			{
				ushort fieldId = dataKey.FieldId;
				bool flag3 = fieldId - 145 <= 1;
				flag2 = !flag3;
			}
			bool flag4 = flag2;
			int result;
			if (flag4)
			{
				result = 0;
			}
			else
			{
				int addRange = 0;
				bool flag5 = dataKey.CombatSkillId == base.SkillTemplateId && base.Affected;
				if (flag5)
				{
					addRange += this.SelfAddAttackRange;
				}
				bool flag6 = base.EffectCount > 0;
				if (flag6)
				{
					addRange += 20;
				}
				result = addRange;
			}
			return result;
		}

		// Token: 0x04000D6C RID: 3436
		private const int EffectAddAttackRange = 20;
	}
}
