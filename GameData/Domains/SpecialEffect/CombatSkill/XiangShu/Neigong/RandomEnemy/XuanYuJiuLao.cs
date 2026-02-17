using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.SpecialEffect.CombatSkill.XiangShu.Neigong.RandomEnemy
{
	// Token: 0x02000297 RID: 663
	public class XuanYuJiuLao : MinionBase
	{
		// Token: 0x0600316A RID: 12650 RVA: 0x0021AE39 File Offset: 0x00219039
		public XuanYuJiuLao()
		{
		}

		// Token: 0x0600316B RID: 12651 RVA: 0x0021AE43 File Offset: 0x00219043
		public XuanYuJiuLao(CombatSkillKey skillKey) : base(skillKey, 16008)
		{
		}

		// Token: 0x0600316C RID: 12652 RVA: 0x0021AE53 File Offset: 0x00219053
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x0600316D RID: 12653 RVA: 0x0021AE81 File Offset: 0x00219081
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
		}

		// Token: 0x0600316E RID: 12654 RVA: 0x0021AEA8 File Offset: 0x002190A8
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || (outerMarkCount <= 0 && innerMarkCount <= 0);
			if (!flag)
			{
				this.ChangeNeiliAllocation(context, combatSkillId);
			}
		}

		// Token: 0x0600316F RID: 12655 RVA: 0x0021AEE4 File Offset: 0x002190E4
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = attackerId != base.CharacterId || (outerMarkCount <= 0 && innerMarkCount <= 0);
			if (!flag)
			{
				this.ChangeNeiliAllocation(context, combatSkillId);
			}
		}

		// Token: 0x06003170 RID: 12656 RVA: 0x0021AF20 File Offset: 0x00219120
		private void ChangeNeiliAllocation(DataContext context, short combatSkillId)
		{
			bool flag = this._affected || !MinionBase.CanAffect;
			if (!flag)
			{
				int changeValue = (combatSkillId < 0) ? 1 : 5;
				bool flag2 = !base.CombatChar.AbsorbNeiliAllocationRandom(context, base.CurrEnemyChar, changeValue);
				if (!flag2)
				{
					base.ShowSpecialEffectTips(0);
					this._affected = true;
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
				}
			}
		}

		// Token: 0x06003171 RID: 12657 RVA: 0x0021AF8D File Offset: 0x0021918D
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x04000EA6 RID: 3750
		private const sbyte NormalAttackValue = 1;

		// Token: 0x04000EA7 RID: 3751
		private const sbyte SkillAttackValue = 5;

		// Token: 0x04000EA8 RID: 3752
		private bool _affected;
	}
}
