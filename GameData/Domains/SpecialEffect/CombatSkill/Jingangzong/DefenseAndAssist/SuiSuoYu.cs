using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004CB RID: 1227
	public class SuiSuoYu : AssistSkillBase
	{
		// Token: 0x06003D50 RID: 15696 RVA: 0x002512FD File Offset: 0x0024F4FD
		public SuiSuoYu()
		{
		}

		// Token: 0x06003D51 RID: 15697 RVA: 0x00251307 File Offset: 0x0024F507
		public SuiSuoYu(CombatSkillKey skillKey) : base(skillKey, 11700)
		{
		}

		// Token: 0x06003D52 RID: 15698 RVA: 0x00251317 File Offset: 0x0024F517
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affected = false;
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x06003D53 RID: 15699 RVA: 0x0025134D File Offset: 0x0024F54D
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x06003D54 RID: 15700 RVA: 0x0025137C File Offset: 0x0024F57C
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && base.CanAffect && (base.IsDirect ? (attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly) : (defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003D55 RID: 15701 RVA: 0x00251404 File Offset: 0x0024F604
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = (outerMarkCount > 0 || innerMarkCount > 0) && base.CanAffect && (base.IsDirect ? (attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly) : (defenderId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(attackerId).IsAlly != base.CombatChar.IsAlly));
			if (flag)
			{
				this.DoEffect(context);
			}
		}

		// Token: 0x06003D56 RID: 15702 RVA: 0x0025148C File Offset: 0x0024F68C
		private void DoEffect(DataContext context)
		{
			bool affected = this._affected;
			if (!affected)
			{
				bool addNeiliAllocation = this._compensationAdd || context.Random.CheckPercentProb(66);
				this._compensationAdd = !addNeiliAllocation;
				List<sbyte> trickRandomPool = ObjectPool<List<sbyte>>.Instance.Get();
				sbyte addValue = addNeiliAllocation ? 5 : -3;
				bool flag = base.CombatChar.ChangeNeiliAllocationRandom(context, (int)addValue, true);
				if (flag)
				{
					base.ShowSpecialEffectTips(0);
				}
				trickRandomPool.Clear();
				trickRandomPool.AddRange(base.CombatChar.GetTricks().Tricks.Values);
				trickRandomPool.RemoveAll(new Predicate<sbyte>(base.CombatChar.IsTrickUsable));
				bool flag2 = trickRandomPool.Count > 0;
				if (flag2)
				{
					DomainManager.Combat.RemoveTrick(context, base.CombatChar, trickRandomPool.GetRandom(context.Random), 1, true, -1);
					base.ShowSpecialEffectTips(1);
				}
				ObjectPool<List<sbyte>>.Instance.Return(trickRandomPool);
				base.ShowEffectTips(context);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06003D57 RID: 15703 RVA: 0x0025159E File Offset: 0x0024F79E
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x0400120F RID: 4623
		private const sbyte AddNeiliAllocation = 5;

		// Token: 0x04001210 RID: 4624
		private const sbyte ReduceNeiliAllocation = -3;

		// Token: 0x04001211 RID: 4625
		private const sbyte BuffOdds = 66;

		// Token: 0x04001212 RID: 4626
		private bool _affected;

		// Token: 0x04001213 RID: 4627
		private bool _compensationAdd;
	}
}
