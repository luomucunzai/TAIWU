using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect.CombatSkill.Jingangzong.DefenseAndAssist
{
	// Token: 0x020004CA RID: 1226
	public class QiLunGanYingFaStateEffect
	{
		// Token: 0x06003D49 RID: 15689 RVA: 0x002510ED File Offset: 0x0024F2ED
		public void Setup()
		{
			this._setupCount++;
			this.UpdateAffecting();
		}

		// Token: 0x06003D4A RID: 15690 RVA: 0x00251105 File Offset: 0x0024F305
		public void Close()
		{
			this._setupCount--;
			this.UpdateAffecting();
		}

		// Token: 0x06003D4B RID: 15691 RVA: 0x00251120 File Offset: 0x0024F320
		public void Reset()
		{
			bool affecting = this._affecting;
			if (affecting)
			{
				Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
			}
			this._affecting = false;
			this._setupCount = 0;
		}

		// Token: 0x06003D4C RID: 15692 RVA: 0x00251158 File Offset: 0x0024F358
		private void UpdateAffecting()
		{
			bool affecting = this._setupCount > 0;
			bool flag = affecting == this._affecting;
			if (!flag)
			{
				this._affecting = affecting;
				bool flag2 = affecting;
				if (flag2)
				{
					Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
				}
				else
				{
					Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
				}
			}
		}

		// Token: 0x06003D4D RID: 15693 RVA: 0x002511B4 File Offset: 0x0024F3B4
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool pause = DomainManager.Combat.Pause;
			if (!pause)
			{
				this.DoAffect(context, combatChar, true);
				this.DoAffect(context, combatChar, false);
			}
		}

		// Token: 0x06003D4E RID: 15694 RVA: 0x002511E8 File Offset: 0x0024F3E8
		private void DoAffect(DataContext context, CombatCharacter affectChar, bool buff)
		{
			sbyte stateType = buff ? 1 : 2;
			short stateId = buff ? 166 : 167;
			Dictionary<int, int> counter = buff ? this._buffAffectingCounter : this._debuffAffectingCounter;
			CValuePercent power = affectChar.GetCombatStatePower(stateType, stateId);
			int changeValue = 1 * power;
			bool flag = changeValue <= 0;
			if (flag)
			{
				counter[affectChar.GetId()] = 0;
			}
			else
			{
				counter[affectChar.GetId()] = counter.GetOrDefault(affectChar.GetId()) + 1;
				bool flag2 = counter[affectChar.GetId()] != 240;
				if (!flag2)
				{
					counter[affectChar.GetId()] = 0;
					short effectId = buff ? 1697 : 1698;
					bool flag3 = affectChar.ChangeNeiliAllocationRandom(context, buff ? changeValue : (-changeValue), true);
					if (flag3)
					{
						DomainManager.Combat.ShowSpecialEffectTips(affectChar.GetId(), (int)effectId, 0);
					}
				}
			}
		}

		// Token: 0x04001209 RID: 4617
		private const int ChangeNeiliAllocationUnit = 1;

		// Token: 0x0400120A RID: 4618
		private const int StateAffectFrame = 240;

		// Token: 0x0400120B RID: 4619
		private bool _affecting;

		// Token: 0x0400120C RID: 4620
		private int _setupCount;

		// Token: 0x0400120D RID: 4621
		private readonly Dictionary<int, int> _buffAffectingCounter = new Dictionary<int, int>();

		// Token: 0x0400120E RID: 4622
		private readonly Dictionary<int, int> _debuffAffectingCounter = new Dictionary<int, int>();
	}
}
