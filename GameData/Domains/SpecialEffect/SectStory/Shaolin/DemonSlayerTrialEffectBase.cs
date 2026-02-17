using System;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F5 RID: 245
	public abstract class DemonSlayerTrialEffectBase : SpecialEffectBase
	{
		// Token: 0x06002989 RID: 10633 RVA: 0x00200FF0 File Offset: 0x001FF1F0
		protected DemonSlayerTrialEffectBase(int charId) : base(charId, -1)
		{
		}

		// Token: 0x0600298A RID: 10634 RVA: 0x00200FFC File Offset: 0x001FF1FC
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
		}

		// Token: 0x0600298B RID: 10635 RVA: 0x00201019 File Offset: 0x001FF219
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatSettlement(new Events.OnCombatSettlement(this.OnCombatSettlement));
			base.OnDisable(context);
		}

		// Token: 0x0600298C RID: 10636 RVA: 0x00201036 File Offset: 0x001FF236
		private void OnCombatSettlement(DataContext context, sbyte combatStatus)
		{
			DomainManager.SpecialEffect.Remove(context, this.Id);
		}
	}
}
