using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Agile;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.Agile
{
	// Token: 0x02000410 RID: 1040
	public class ZhenShanBu : AgileSkillBase
	{
		// Token: 0x06003908 RID: 14600 RVA: 0x0023CD7F File Offset: 0x0023AF7F
		public ZhenShanBu()
		{
		}

		// Token: 0x06003909 RID: 14601 RVA: 0x0023CD89 File Offset: 0x0023AF89
		public ZhenShanBu(CombatSkillKey skillKey) : base(skillKey, 6400)
		{
		}

		// Token: 0x0600390A RID: 14602 RVA: 0x0023CD9C File Offset: 0x0023AF9C
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affected = false;
			bool isDirect = base.IsDirect;
			if (isDirect)
			{
				this.AffectDatas = new Dictionary<AffectedDataKey, EDataModifyType>();
				int[] charList = DomainManager.Combat.GetCharacterList(!base.CombatChar.IsAlly);
				for (int i = 0; i < charList.Length; i++)
				{
					bool flag = charList[i] >= 0;
					if (flag)
					{
						this.AffectDatas.Add(new AffectedDataKey(charList[i], 150, -1, -1, -1, -1), EDataModifyType.TotalPercent);
					}
				}
				base.ShowSpecialEffectTips(0);
			}
			else
			{
				Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
				Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			}
		}

		// Token: 0x0600390B RID: 14603 RVA: 0x0023CE5C File Offset: 0x0023B05C
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			bool flag = !base.IsDirect;
			if (flag)
			{
				Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
				Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			}
		}

		// Token: 0x0600390C RID: 14604 RVA: 0x0023CEA8 File Offset: 0x0023B0A8
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly == isAlly || (outerMarkCount <= 0 && innerMarkCount <= 0) || !base.CanAffect;
			if (!flag)
			{
				this.ReduceMobility(context);
			}
		}

		// Token: 0x0600390D RID: 14605 RVA: 0x0023CEF8 File Offset: 0x0023B0F8
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly == isAlly || (outerMarkCount <= 0 && innerMarkCount <= 0) || !base.CanAffect;
			if (!flag)
			{
				this.ReduceMobility(context);
			}
		}

		// Token: 0x0600390E RID: 14606 RVA: 0x0023CF48 File Offset: 0x0023B148
		private void ReduceMobility(DataContext context)
		{
			bool affected = this._affected;
			if (!affected)
			{
				base.ChangeMobilityValue(context, base.CurrEnemyChar, -MoveSpecialConstants.MaxMobility * ZhenShanBu.ReverseCost);
				base.ShowSpecialEffectTips(0);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x0600390F RID: 14607 RVA: 0x0023CFA1 File Offset: 0x0023B1A1
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x06003910 RID: 14608 RVA: 0x0023CFC0 File Offset: 0x0023B1C0
		public override int GetModifyValue(AffectedDataKey dataKey, int currModifyValue)
		{
			bool flag = !base.CanAffect || dataKey.CustomParam0 != base.CharacterId;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = dataKey.FieldId == 150;
				if (flag2)
				{
					result = 50;
				}
				else
				{
					result = 0;
				}
			}
			return result;
		}

		// Token: 0x040010AF RID: 4271
		private const sbyte DirectAddCost = 50;

		// Token: 0x040010B0 RID: 4272
		private static readonly CValuePercent ReverseCost = 4;

		// Token: 0x040010B1 RID: 4273
		private bool _affected;
	}
}
