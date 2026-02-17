using System;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.Animal
{
	// Token: 0x020005E7 RID: 1511
	public abstract class CarrierEffectBase : SpecialEffectBase
	{
		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06004482 RID: 17538
		protected abstract short CombatStateId { get; }

		// Token: 0x06004483 RID: 17539 RVA: 0x0026FCC2 File Offset: 0x0026DEC2
		protected CarrierEffectBase()
		{
		}

		// Token: 0x06004484 RID: 17540 RVA: 0x0026FCCC File Offset: 0x0026DECC
		protected CarrierEffectBase(int charId) : base(charId, -1)
		{
		}

		// Token: 0x06004485 RID: 17541 RVA: 0x0026FCD8 File Offset: 0x0026DED8
		public sealed override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			this.OnEnableSubClass(context);
		}

		// Token: 0x06004486 RID: 17542 RVA: 0x0026FD0F File Offset: 0x0026DF0F
		public sealed override void OnDisable(DataContext context)
		{
			this.OnDisableSubClass(context);
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			base.OnDisable(context);
		}

		// Token: 0x06004487 RID: 17543 RVA: 0x0026FD46 File Offset: 0x0026DF46
		protected virtual void OnEnableSubClass(DataContext context)
		{
		}

		// Token: 0x06004488 RID: 17544 RVA: 0x0026FD49 File Offset: 0x0026DF49
		protected virtual void OnDisableSubClass(DataContext context)
		{
		}

		// Token: 0x06004489 RID: 17545 RVA: 0x0026FD4C File Offset: 0x0026DF4C
		private void OnCombatBegin(DataContext context)
		{
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 0, this.CombatStateId, 100, false, true, base.CharacterId);
		}

		// Token: 0x0600448A RID: 17546 RVA: 0x0026FD7D File Offset: 0x0026DF7D
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			DomainManager.SpecialEffect.Remove(context, this.Id);
		}
	}
}
