using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Domains.CombatSkill;
using GameData.Domains.SpecialEffect.CombatSkill.Common.Defense;

namespace GameData.Domains.SpecialEffect.CombatSkill.Wuxianjiao.DefenseAndAssist
{
	// Token: 0x020003A6 RID: 934
	public class LuanQingZhang : DefenseSkillBase
	{
		// Token: 0x060036A7 RID: 13991 RVA: 0x0023171C File Offset: 0x0022F91C
		public LuanQingZhang()
		{
		}

		// Token: 0x060036A8 RID: 13992 RVA: 0x00231726 File Offset: 0x0022F926
		public LuanQingZhang(CombatSkillKey skillKey) : base(skillKey, 12701)
		{
		}

		// Token: 0x060036A9 RID: 13993 RVA: 0x00231736 File Offset: 0x0022F936
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			this._frameCounter = 0;
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060036AA RID: 13994 RVA: 0x0023175A File Offset: 0x0022F95A
		public override void OnDisable(DataContext context)
		{
			base.OnDisable(context);
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnStateMachineUpdateEnd));
		}

		// Token: 0x060036AB RID: 13995 RVA: 0x00231778 File Offset: 0x0022F978
		private void OnStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly != base.CombatChar.IsAlly || DomainManager.Combat.Pause;
			if (!flag)
			{
				this._frameCounter += 1;
				bool flag2 = this._frameCounter < 100;
				if (!flag2)
				{
					this._frameCounter = 0;
					short distance = DomainManager.Combat.GetCurrentDistance();
					bool flag3 = (base.IsDirect ? (distance > 70) : (distance < 70)) || !base.CanAffect;
					if (!flag3)
					{
						CombatCharacter enemyChar = DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false);
						bool flag4 = enemyChar.GetHitValue(3, -1, 0, -1) + enemyChar.GetAvoidValue(3, -1, -1, false) > base.CombatChar.GetHitValue(3, -1, 0, -1) + base.CombatChar.GetAvoidValue(3, -1, -1, false);
						if (!flag4)
						{
							DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, DomainManager.Combat.GetCombatCharacter(!base.CombatChar.IsAlly, false), 100, false);
							base.ShowSpecialEffectTips(0);
						}
					}
				}
			}
		}

		// Token: 0x04000FEA RID: 4074
		private const sbyte RequireDistance = 70;

		// Token: 0x04000FEB RID: 4075
		private const short AddQiDisorderFrame = 100;

		// Token: 0x04000FEC RID: 4076
		private const short AddQiDisorder = 100;

		// Token: 0x04000FED RID: 4077
		private short _frameCounter;
	}
}
