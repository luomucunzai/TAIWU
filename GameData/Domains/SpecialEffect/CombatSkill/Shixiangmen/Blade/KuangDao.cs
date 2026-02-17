using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Blade
{
	// Token: 0x02000409 RID: 1033
	public class KuangDao : CombatSkillEffectBase
	{
		// Token: 0x17000203 RID: 515
		// (get) Token: 0x060038E3 RID: 14563 RVA: 0x0023C552 File Offset: 0x0023A752
		private int AddPowerUnit
		{
			get
			{
				return base.IsDirect ? 5 : 10;
			}
		}

		// Token: 0x060038E4 RID: 14564 RVA: 0x0023C561 File Offset: 0x0023A761
		public KuangDao()
		{
		}

		// Token: 0x060038E5 RID: 14565 RVA: 0x0023C56B File Offset: 0x0023A76B
		public KuangDao(CombatSkillKey skillKey) : base(skillKey, 6208, -1)
		{
		}

		// Token: 0x060038E6 RID: 14566 RVA: 0x0023C57C File Offset: 0x0023A77C
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 220, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 248, base.SkillTemplateId, -1, -1, -1), EDataModifyType.Custom);
			this.AffectDatas.Add(new AffectedDataKey(base.CharacterId, 74, base.SkillTemplateId, -1, -1, -1), EDataModifyType.TotalPercent);
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038E7 RID: 14567 RVA: 0x0023C62F File Offset: 0x0023A82F
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038E8 RID: 14568 RVA: 0x0023C658 File Offset: 0x0023A858
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && (base.IsDirect ? (attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly) : (defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				this.AddPower(context, (int)(outerMarkCount + innerMarkCount));
			}
		}

		// Token: 0x060038E9 RID: 14569 RVA: 0x0023C6E0 File Offset: 0x0023A8E0
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && (base.IsDirect ? (attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly) : (defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				this.AddPower(context, outerMarkCount + innerMarkCount);
			}
		}

		// Token: 0x060038EA RID: 14570 RVA: 0x0023C768 File Offset: 0x0023A968
		private void AddPower(DataContext context, int defeatMarkCount)
		{
			this._affectingDefeatMarkCount += defeatMarkCount;
			DomainManager.Combat.AddSkillPowerInCombat(context, this.SkillKey, new SkillEffectKey(base.SkillTemplateId, base.IsDirect), this.AddPowerUnit * defeatMarkCount);
			bool affected = this._affected;
			if (!affected)
			{
				this._affected = true;
				base.ShowSpecialEffectTips(0);
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x060038EB RID: 14571 RVA: 0x0023C7DC File Offset: 0x0023A9DC
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060038EC RID: 14572 RVA: 0x0023C7F8 File Offset: 0x0023A9F8
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 74 && this._affectingDefeatMarkCount > 0;
				if (flag2)
				{
					base.ShowSpecialEffectTips(1);
					result = Math.Min(30, this._affectingDefeatMarkCount) * -3;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x060038ED RID: 14573 RVA: 0x0023C86C File Offset: 0x0023AA6C
		public override bool GetModifiedValue(AffectedDataKey dataKey, bool dataValue)
		{
			bool flag = dataKey.CharId != base.CharacterId || dataKey.CombatSkillId != base.SkillTemplateId;
			bool result;
			if (flag)
			{
				result = dataValue;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 220;
				if (flag2)
				{
					result = false;
				}
				else
				{
					bool flag3 = dataKey.FieldId == 248;
					result = (flag3 || dataValue);
				}
			}
			return result;
		}

		// Token: 0x040010A2 RID: 4258
		private const sbyte AddHitOddsUnit = -3;

		// Token: 0x040010A3 RID: 4259
		private const int AddHitOddsUnitMaxCount = 30;

		// Token: 0x040010A4 RID: 4260
		private bool _affected;

		// Token: 0x040010A5 RID: 4261
		private int _affectingDefeatMarkCount;
	}
}
