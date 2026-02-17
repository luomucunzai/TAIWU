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
	// Token: 0x020003DA RID: 986
	public class WuJiJin : DefenseSkillBase
	{
		// Token: 0x060037CF RID: 14287 RVA: 0x0023753C File Offset: 0x0023573C
		public WuJiJin()
		{
		}

		// Token: 0x060037D0 RID: 14288 RVA: 0x00237546 File Offset: 0x00235746
		public WuJiJin(CombatSkillKey skillKey) : base(skillKey, 4506)
		{
		}

		// Token: 0x060037D1 RID: 14289 RVA: 0x00237558 File Offset: 0x00235758
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._addDamage = 0;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 71, -1, -1, -1, -1), EDataModifyType.AddPercent);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 250, -1, -1, -1, -1), EDataModifyType.Custom);
			Events.RegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.RegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060037D2 RID: 14290 RVA: 0x002375E3 File Offset: 0x002357E3
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_NormalAttackCalcHitEnd(new Events.OnNormalAttackCalcHitEnd(this.OnNormalAttackCalcHitEnd));
			Events.UnRegisterHandler_NormalAttackEnd(new Events.OnNormalAttackEnd(this.OnNormalAttackEnd));
		}

		// Token: 0x060037D3 RID: 14291 RVA: 0x00237614 File Offset: 0x00235814
		private void OnNormalAttackCalcHitEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, int pursueIndex, bool hit, bool isFightBack, bool isMind)
		{
			bool flag = !isFightBack || attacker != base.CombatChar || !hit || !base.CanAffect || base.CombatChar.FightBackWithHit;
			if (!flag)
			{
				int reducedValue = Math.Abs(base.IsDirect ? base.ChangeStanceValue(context, base.CurrEnemyChar, -base.CurrEnemyChar.GetStanceValue()) : base.ChangeBreathValue(context, base.CurrEnemyChar, -base.CurrEnemyChar.GetBreathValue()));
				this._addDamage = reducedValue * 100 / (base.IsDirect ? 4000 : 30000) * 2;
				base.ShowSpecialEffectTips(0);
			}
		}

		// Token: 0x060037D4 RID: 14292 RVA: 0x002376BC File Offset: 0x002358BC
		private void OnNormalAttackEnd(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte trickType, int pursueIndex, bool hit, bool isFightBack)
		{
			bool flag = this._addDamage == 0 || !isFightBack || attacker != base.CombatChar;
			if (!flag)
			{
				this._addDamage = 0;
			}
		}

		// Token: 0x060037D5 RID: 14293 RVA: 0x002376F4 File Offset: 0x002358F4
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 71;
				if (flag2)
				{
					result = this._addDamage;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060037D6 RID: 14294 RVA: 0x00237738 File Offset: 0x00235938
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || !base.CanAffect;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 250 && dataKey.CustomParam0 != 1;
				result = (flag2 || dataValue);
			}
			return result;
		}

		// Token: 0x0400104B RID: 4171
		private const sbyte AddDamageUnit = 2;

		// Token: 0x0400104C RID: 4172
		private int _addDamage;
	}
}
