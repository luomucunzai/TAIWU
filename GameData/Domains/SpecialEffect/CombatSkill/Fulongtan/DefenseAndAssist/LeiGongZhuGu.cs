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
	// Token: 0x02000522 RID: 1314
	public class LeiGongZhuGu : DefenseSkillBase
	{
		// Token: 0x06003F2A RID: 16170 RVA: 0x00258C1B File Offset: 0x00256E1B
		public LeiGongZhuGu()
		{
		}

		// Token: 0x06003F2B RID: 16171 RVA: 0x00258C25 File Offset: 0x00256E25
		public LeiGongZhuGu(CombatSkillKey skillKey) : base(skillKey, 14504)
		{
		}

		// Token: 0x06003F2C RID: 16172 RVA: 0x00258C38 File Offset: 0x00256E38
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
				Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			}
			else
			{
				this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 70, -1, -1, -1, -1), EDataModifyType.AddPercent);
				Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x06003F2D RID: 16173 RVA: 0x00258CDC File Offset: 0x00256EDC
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
				Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			}
			else
			{
				Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
			}
		}

		// Token: 0x06003F2E RID: 16174 RVA: 0x00258D38 File Offset: 0x00256F38
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F2F RID: 16175 RVA: 0x00258D74 File Offset: 0x00256F74
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F30 RID: 16176 RVA: 0x00258DB4 File Offset: 0x00256FB4
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = !this._affected || attackerId != base.CharacterId;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F31 RID: 16177 RVA: 0x00258DF0 File Offset: 0x00256FF0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || dataKey.CustomParam0 != 0;
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
					result = -45;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 70;
					if (flag3)
					{
						this._affected = true;
						result = 30;
					}
					else
					{
						result = 0;
					}
				}
			}
			return result;
		}

		// Token: 0x0400129D RID: 4765
		private const sbyte DirectReduceDamagePercent = -45;

		// Token: 0x0400129E RID: 4766
		private const sbyte ReverseAddDamagePercent = 30;

		// Token: 0x0400129F RID: 4767
		private bool _affected;
	}
}
