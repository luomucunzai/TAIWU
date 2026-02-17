using System;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.SpecialEffect;

namespace GameData.Domains.Combat
{
	// Token: 0x02000704 RID: 1796
	public class TeammateCommandInvokerCooldown : TeammateCommandInvokerBase, IFrameCounterHandler
	{
		// Token: 0x060067F3 RID: 26611 RVA: 0x003B2CB8 File Offset: 0x003B0EB8
		public TeammateCommandInvokerCooldown(int charId, int index) : base(charId, index)
		{
			this._coolingCounter = new FrameCounter(this, (int)base.CmdConfig.CooldownFrame, 0);
		}

		// Token: 0x060067F4 RID: 26612 RVA: 0x003B2CDC File Offset: 0x003B0EDC
		public override void Setup()
		{
			this._coolingCounter.Setup();
			Events.RegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x060067F5 RID: 26613 RVA: 0x003B2CFD File Offset: 0x003B0EFD
		public override void Close()
		{
			this._coolingCounter.Close();
			Events.UnRegisterHandler_CombatStateMachineUpdateEnd(new Events.OnCombatStateMachineUpdateEnd(this.OnCombatStateMachineUpdateEnd));
		}

		// Token: 0x060067F6 RID: 26614 RVA: 0x003B2D20 File Offset: 0x003B0F20
		private void OnCombatStateMachineUpdateEnd(DataContext context, CombatCharacter combatChar)
		{
			bool flag = combatChar != base.MainChar || TeammateCommandInvokerBase.CombatDomain.Pause || this._cooling;
			if (!flag)
			{
				this._cooling = true;
				base.IntoCombat();
			}
		}

		// Token: 0x17000429 RID: 1065
		// (get) Token: 0x060067F7 RID: 26615 RVA: 0x003B2D60 File Offset: 0x003B0F60
		public int CharacterId
		{
			get
			{
				return this.MainCharId;
			}
		}

		// Token: 0x060067F8 RID: 26616 RVA: 0x003B2D68 File Offset: 0x003B0F68
		public bool IsOn(int counterType)
		{
			return this._cooling;
		}

		// Token: 0x060067F9 RID: 26617 RVA: 0x003B2D70 File Offset: 0x003B0F70
		public void OnProcess(DataContext context, int counterType)
		{
			this._cooling = false;
		}

		// Token: 0x04001C5B RID: 7259
		private readonly FrameCounter _coolingCounter;

		// Token: 0x04001C5C RID: 7260
		private bool _cooling;
	}
}
