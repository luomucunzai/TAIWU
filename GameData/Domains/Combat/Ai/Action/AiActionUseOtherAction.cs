using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007EF RID: 2031
	[AiAction(EAiActionType.UseOtherAction)]
	public class AiActionUseOtherAction : AiActionCombatBase
	{
		// Token: 0x06006AAD RID: 27309 RVA: 0x003BD348 File Offset: 0x003BB548
		public AiActionUseOtherAction(IReadOnlyList<int> ints)
		{
			this._otherActionType = (sbyte)ints[0];
		}

		// Token: 0x06006AAE RID: 27310 RVA: 0x003BD360 File Offset: 0x003BB560
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.StartPrepareOtherAction(context, this._otherActionType, combatChar.IsAlly);
		}

		// Token: 0x04001D77 RID: 7543
		private readonly sbyte _otherActionType;
	}
}
