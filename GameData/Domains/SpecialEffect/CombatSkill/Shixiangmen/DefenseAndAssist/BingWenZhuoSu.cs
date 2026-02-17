using System;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Shixiangmen.DefenseAndAssist
{
	// Token: 0x02000400 RID: 1024
	public class BingWenZhuoSu : AssistSkillBase
	{
		// Token: 0x060038B2 RID: 14514 RVA: 0x0023B87B File Offset: 0x00239A7B
		public BingWenZhuoSu()
		{
		}

		// Token: 0x060038B3 RID: 14515 RVA: 0x0023B885 File Offset: 0x00239A85
		public BingWenZhuoSu(CombatSkillKey skillKey) : base(skillKey, 6601)
		{
		}

		// Token: 0x060038B4 RID: 14516 RVA: 0x0023B895 File Offset: 0x00239A95
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._affected = false;
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038B5 RID: 14517 RVA: 0x0023B8CB File Offset: 0x00239ACB
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x060038B6 RID: 14518 RVA: 0x0023B8FC File Offset: 0x00239AFC
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = !base.CanAffect || (outerMarkCount <= 0 && innerMarkCount <= 0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool flag2 = defenderId == base.CharacterId && attackerId == enemyChar.GetId();
				if (flag2)
				{
					this.ChangeMobility(context, base.IsDirect ? base.CombatChar : enemyChar);
				}
			}
		}

		// Token: 0x060038B7 RID: 14519 RVA: 0x0023B978 File Offset: 0x00239B78
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = !base.CanAffect || (outerMarkCount <= 0 && innerMarkCount <= 0);
			if (!flag)
			{
				CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
				bool flag2 = defenderId == base.CharacterId && attackerId == enemyChar.GetId();
				if (flag2)
				{
					this.ChangeMobility(context, base.IsDirect ? base.CombatChar : enemyChar);
				}
			}
		}

		// Token: 0x060038B8 RID: 14520 RVA: 0x0023B9F4 File Offset: 0x00239BF4
		private void ChangeMobility(DataContext context, CombatCharacter affectChar)
		{
			bool affected = this._affected;
			if (!affected)
			{
				int changeMobility = MoveSpecialConstants.MaxMobility * BingWenZhuoSu.ChangeMobilityPercent;
				base.ChangeMobilityValue(context, affectChar, base.IsDirect ? changeMobility : (-changeMobility));
				base.ShowSpecialEffectTips(0);
				base.ShowEffectTips(context);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x060038B9 RID: 14521 RVA: 0x0023BA5D File Offset: 0x00239C5D
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x0400109B RID: 4251
		private static readonly CValuePercent ChangeMobilityPercent = 12;

		// Token: 0x0400109C RID: 4252
		private bool _affected;
	}
}
