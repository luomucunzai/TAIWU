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
	// Token: 0x02000527 RID: 1319
	public class ShenHuoZhaoGong : DefenseSkillBase
	{
		// Token: 0x06003F48 RID: 16200 RVA: 0x00259320 File Offset: 0x00257520
		public ShenHuoZhaoGong()
		{
		}

		// Token: 0x06003F49 RID: 16201 RVA: 0x0025932A File Offset: 0x0025752A
		public ShenHuoZhaoGong(CombatSkillKey skillKey) : base(skillKey, 14502)
		{
		}

		// Token: 0x06003F4A RID: 16202 RVA: 0x0025933C File Offset: 0x0025753C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 102, -1, -1, -1, -1), EDataModifyType.AddPercent);
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.RegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.RegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003F4B RID: 16203 RVA: 0x002593B4 File Offset: 0x002575B4
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
			Events.UnRegisterHandler_AttackSkillAttackEnd(new Events.OnAttackSkillAttackEnd(this.OnAttackSkillAttackEnd));
			Events.UnRegisterHandler_BounceInjury(new Events.OnBounceInjury(this.OnBounceInjury));
		}

		// Token: 0x06003F4C RID: 16204 RVA: 0x00259400 File Offset: 0x00257600
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = !this._affected || defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F4D RID: 16205 RVA: 0x0025943C File Offset: 0x0025763C
		private void OnAttackSkillAttackEnd(CombatContext context, sbyte hitType, bool hit, int index)
		{
			bool flag = !this._affected || context.Defender != base.CombatChar;
			if (!flag)
			{
				this._affected = false;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x06003F4E RID: 16206 RVA: 0x0025947C File Offset: 0x0025767C
		private void OnBounceInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount)
		{
			bool flag = attackerId != base.CharacterId || !base.CanAffect;
			if (!flag)
			{
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.CombatChar.ChangeNeiliAllocation(context, 2, 1, true, true);
				}
				else
				{
					base.CurrEnemyChar.ChangeNeiliAllocation(context, 2, -1, true, true);
				}
				base.ShowSpecialEffectTips(1);
			}
		}

		// Token: 0x06003F4F RID: 16207 RVA: 0x002594DC File Offset: 0x002576DC
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect || dataKey.CustomParam0 != 1;
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

		// Token: 0x040012A8 RID: 4776
		private const sbyte ReduceDamagePercent = -30;

		// Token: 0x040012A9 RID: 4777
		private const sbyte ChangeNeiliAllocation = 1;

		// Token: 0x040012AA RID: 4778
		private bool _affected;
	}
}
