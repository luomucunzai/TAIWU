using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Assist;

namespace GameData.Domains.SpecialEffect.CombatSkill.Kongsangpai.DefenseAndAssist
{
	// Token: 0x020004A0 RID: 1184
	public class ZiWuAnXiXiang : AssistSkillBase
	{
		// Token: 0x06003C76 RID: 15478 RVA: 0x0024DAE3 File Offset: 0x0024BCE3
		public ZiWuAnXiXiang()
		{
		}

		// Token: 0x06003C77 RID: 15479 RVA: 0x0024DAED File Offset: 0x0024BCED
		public ZiWuAnXiXiang(CombatSkillKey skillKey) : base(skillKey, 10704)
		{
		}

		// Token: 0x06003C78 RID: 15480 RVA: 0x0024DAFD File Offset: 0x0024BCFD
		public override void OnEnable(DataContext context)
		{
			this._affected = false;
			Events.RegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.RegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.RegisterHandler_AddDirectPoisonMark(new Events.OnAddDirectPoisonMark(this.AddDirectPoisonMark));
		}

		// Token: 0x06003C79 RID: 15481 RVA: 0x0024DB3D File Offset: 0x0024BD3D
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_AddDirectInjury(new Events.OnAddDirectInjury(this.OnAddDirectInjury));
			Events.UnRegisterHandler_AddDirectFatalDamageMark(new Events.OnAddDirectFatalDamageMark(this.OnAddDirectFatalDamageMark));
			Events.UnRegisterHandler_AddDirectPoisonMark(new Events.OnAddDirectPoisonMark(this.AddDirectPoisonMark));
		}

		// Token: 0x06003C7A RID: 15482 RVA: 0x0024DB78 File Offset: 0x0024BD78
		private void OnAddDirectInjury(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, sbyte outerMarkCount, sbyte innerMarkCount, short combatSkillId)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				bool flag2 = (outerMarkCount > 0 || innerMarkCount > 0) && attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly;
				if (flag2)
				{
					this.ChangeNeiliAllocation(context);
				}
			}
		}

		// Token: 0x06003C7B RID: 15483 RVA: 0x0024DBD0 File Offset: 0x0024BDD0
		private void OnAddDirectFatalDamageMark(DataContext context, int attackerId, int defenderId, bool isAlly, sbyte bodyPart, int outerMarkCount, int innerMarkCount, short combatSkillId)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				bool flag2 = (outerMarkCount > 0 || innerMarkCount > 0) && attackerId == base.CharacterId && DomainManager.Combat.GetElement_CombatCharacterDict(defenderId).IsAlly != isAlly;
				if (flag2)
				{
					this.ChangeNeiliAllocation(context);
				}
			}
		}

		// Token: 0x06003C7C RID: 15484 RVA: 0x0024DC28 File Offset: 0x0024BE28
		private void AddDirectPoisonMark(DataContext context, CombatCharacter attacker, CombatCharacter defender, sbyte poisonType, short skillId, int markCount)
		{
			bool flag = !base.CanAffect;
			if (!flag)
			{
				bool flag2 = markCount > 0 && attacker.GetId() == base.CharacterId;
				if (flag2)
				{
					this.ChangeNeiliAllocation(context);
				}
			}
		}

		// Token: 0x06003C7D RID: 15485 RVA: 0x0024DC68 File Offset: 0x0024BE68
		private void ChangeNeiliAllocation(DataContext context)
		{
			bool affected = this._affected;
			if (!affected)
			{
				byte neiliAllocationType = (byte)context.Random.Next(0, 4);
				bool isDirect = base.IsDirect;
				if (isDirect)
				{
					base.CombatChar.ChangeNeiliAllocation(context, neiliAllocationType, 5, true, true);
				}
				else
				{
					DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false).ChangeNeiliAllocation(context, neiliAllocationType, -5, true, true);
				}
				base.ShowSpecialEffectTips(0);
				base.ShowEffectTips(context);
				this._affected = true;
				Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
			}
		}

		// Token: 0x06003C7E RID: 15486 RVA: 0x0024DCFC File Offset: 0x0024BEFC
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			this._affected = false;
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x040011CC RID: 4556
		private const int ChangeNeiliAllocationValue = 5;

		// Token: 0x040011CD RID: 4557
		private bool _affected;
	}
}
