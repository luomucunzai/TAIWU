using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.DomainEvents;

namespace GameData.Domains.SpecialEffect.SectStory.Shaolin
{
	// Token: 0x020000F4 RID: 244
	public class CostNeiliAllocation : DemonSlayerTrialEffectBase
	{
		// Token: 0x06002985 RID: 10629 RVA: 0x00200F86 File Offset: 0x001FF186
		public CostNeiliAllocation(int charId, IReadOnlyList<int> parameters) : base(charId)
		{
			this._costNeiliAllocationPercent = -parameters[0];
		}

		// Token: 0x06002986 RID: 10630 RVA: 0x00200F9F File Offset: 0x001FF19F
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			Events.RegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
		}

		// Token: 0x06002987 RID: 10631 RVA: 0x00200FBC File Offset: 0x001FF1BC
		public override void OnDisable(DataContext context)
		{
			Events.UnRegisterHandler_CombatBegin(new Events.OnCombatBegin(this.OnCombatBegin));
			base.OnDisable(context);
		}

		// Token: 0x06002988 RID: 10632 RVA: 0x00200FD9 File Offset: 0x001FF1D9
		private void OnCombatBegin(DataContext context)
		{
			base.CombatChar.ChangeAllNeiliAllocation(context, this._costNeiliAllocationPercent, false);
		}

		// Token: 0x04000CD1 RID: 3281
		private readonly int _costNeiliAllocationPercent;
	}
}
