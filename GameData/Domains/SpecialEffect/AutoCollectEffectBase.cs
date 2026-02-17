using System;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000DA RID: 218
	public class AutoCollectEffectBase : SpecialEffectBase
	{
		// Token: 0x06002872 RID: 10354 RVA: 0x001EFAE7 File Offset: 0x001EDCE7
		protected AutoCollectEffectBase()
		{
		}

		// Token: 0x06002873 RID: 10355 RVA: 0x001EFAF1 File Offset: 0x001EDCF1
		protected AutoCollectEffectBase(int charId) : base(charId, -1)
		{
		}

		// Token: 0x06002874 RID: 10356 RVA: 0x001EFAFD File Offset: 0x001EDCFD
		public override void OnEnable(DataContext context)
		{
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002875 RID: 10357 RVA: 0x001EFB12 File Offset: 0x001EDD12
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x06002876 RID: 10358 RVA: 0x001EFB27 File Offset: 0x001EDD27
		protected virtual void BeforeRemove(DataContext context)
		{
		}

		// Token: 0x06002877 RID: 10359 RVA: 0x001EFB2A File Offset: 0x001EDD2A
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			this.BeforeRemove(context);
			DomainManager.SpecialEffect.Remove(context, this.Id);
		}
	}
}
