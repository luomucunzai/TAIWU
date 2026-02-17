using System;
using GameData.Common;

namespace GameData.Domains.SpecialEffect.Profession
{
	// Token: 0x02000109 RID: 265
	public abstract class ProfessionEffectBase : AutoCollectEffectBase
	{
		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060029D0 RID: 10704
		protected abstract short CombatStateId { get; }

		// Token: 0x060029D1 RID: 10705 RVA: 0x002015CF File Offset: 0x001FF7CF
		protected ProfessionEffectBase()
		{
		}

		// Token: 0x060029D2 RID: 10706 RVA: 0x002015D9 File Offset: 0x001FF7D9
		protected ProfessionEffectBase(int charId) : base(charId)
		{
		}

		// Token: 0x060029D3 RID: 10707 RVA: 0x002015E4 File Offset: 0x001FF7E4
		public override void OnEnable(DataContext context)
		{
			base.OnEnable(context);
			DomainManager.Combat.AddCombatState(context, base.CombatChar, 0, this.CombatStateId, 100, false, true, base.CharacterId);
		}
	}
}
