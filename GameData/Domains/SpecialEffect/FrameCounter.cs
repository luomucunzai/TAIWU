using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Combat;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000DE RID: 222
	public class FrameCounter
	{
		// Token: 0x0600288C RID: 10380 RVA: 0x001EFE5F File Offset: 0x001EE05F
		public FrameCounter(IFrameCounterHandler handler, int period, int counterType = 0)
		{
			this._handler = handler;
			this._period = period;
			this._counterType = counterType;
			Tester.Assert(this._handler != null, "_handler != null");
		}

		// Token: 0x0600288D RID: 10381 RVA: 0x001EFE92 File Offset: 0x001EE092
		public void Setup()
		{
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x0600288E RID: 10382 RVA: 0x001EFEA7 File Offset: 0x001EE0A7
		public void Close()
		{
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x0600288F RID: 10383 RVA: 0x001EFEBC File Offset: 0x001EE0BC
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar.GetId() != this._handler.CharacterId || DomainManager.Combat.Pause;
			if (!flag)
			{
				bool flag2 = this._handler.IsOn(this._counterType);
				if (flag2)
				{
					this._counter++;
					bool flag3 = this._counter < this._period;
					if (!flag3)
					{
						this._counter = 0;
						this._handler.OnProcess(context, this._counterType);
					}
				}
				else
				{
					this._counter = 0;
				}
			}
		}

		// Token: 0x04000822 RID: 2082
		private readonly IFrameCounterHandler _handler;

		// Token: 0x04000823 RID: 2083
		private readonly int _period;

		// Token: 0x04000824 RID: 2084
		private readonly int _counterType;

		// Token: 0x04000825 RID: 2085
		private int _counter;
	}
}
