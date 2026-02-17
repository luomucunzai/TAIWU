using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Fulongtan.DefenseAndAssist
{
	// Token: 0x02000524 RID: 1316
	public class LongHuiShou : DefenseSkillBase
	{
		// Token: 0x06003F34 RID: 16180 RVA: 0x00258E88 File Offset: 0x00257088
		public LongHuiShou()
		{
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x00258E92 File Offset: 0x00257092
		public LongHuiShou(CombatSkillKey skillKey) : base(skillKey, 14501)
		{
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x00258EA4 File Offset: 0x002570A4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00258F1C File Offset: 0x0025711C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x00258F68 File Offset: 0x00257168
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F39 RID: 16185 RVA: 0x00258FA4 File Offset: 0x002571A4
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F3A RID: 16186 RVA: 0x00258FE4 File Offset: 0x002571E4
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.CombatChar.ChangeNeiliAllocation(context, 0, 1, true, true);
				}
				else
				{
					base.CurrEnemyChar.ChangeNeiliAllocation(context, 0, -1, true, true);
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003F3B RID: 16187 RVA: 0x00259044 File Offset: 0x00257244
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || dataKey.CustomParam0 != 0;
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
					result = -30;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040012A0 RID: 4768
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x040012A1 RID: 4769
		private const sbyte ChangeNeiliAllocation = 1;

		// Token: 0x040012A2 RID: 4770
		private bool _affected;
	}
}
