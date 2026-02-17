using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wudangpai.DefenseAndAssist
{
	// Token: 0x020003D9 RID: 985
	public class WuDangTieBuShan : DefenseSkillBase
	{
		// Token: 0x060037C7 RID: 14279 RVA: 0x00237305 File Offset: 0x00235505
		public WuDangTieBuShan()
		{
		}

		// Token: 0x060037C8 RID: 14280 RVA: 0x0023730F File Offset: 0x0023550F
		public WuDangTieBuShan(CombatSkillKey skillKey) : base(skillKey, 4500)
		{
		}

		// Token: 0x060037C9 RID: 14281 RVA: 0x00237320 File Offset: 0x00235520
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060037CA RID: 14282 RVA: 0x00237398 File Offset: 0x00235598
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CompareDataCalcFinished(new Events.OnCompareDataCalcFinished(this.OnCompareDataCalcFinished));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
		}

		// Token: 0x060037CB RID: 14283 RVA: 0x002373E4 File Offset: 0x002355E4
		private void OnCompareDataCalcFinished(CombatContext context, DamageCompareData compareData)
		{
			bool flag = base.CombatChar != context.Defender || !base.CanAffect;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					int totalDefend = compareData.OuterDefendValue + compareData.InnerDefendValue;
					compareData.OuterDefendValue = (compareData.InnerDefendValue = totalDefend / 2);
				}
				else
				{
					int totalAttack = compareData.OuterAttackValue + compareData.InnerAttackValue;
					compareData.OuterAttackValue = (compareData.InnerAttackValue = totalAttack / 2);
				}
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060037CC RID: 14284 RVA: 0x00237470 File Offset: 0x00235670
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060037CD RID: 14285 RVA: 0x002374AC File Offset: 0x002356AC
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x060037CE RID: 14286 RVA: 0x002374EC File Offset: 0x002356EC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 102;
				if (flag2)
				{
					this._affected = true;
					result = -15;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x04001049 RID: 4169
		private const sbyte ReduceDamagePercent = -15;

		// Token: 0x0400104A RID: 4170
		private bool _affected;
	}
}
