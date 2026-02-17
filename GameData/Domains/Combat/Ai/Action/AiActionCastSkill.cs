using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BE RID: 1982
	[AiAction(EAiActionType.CastSkill)]
	public class AiActionCastSkill : AiActionCombatBase
	{
		// Token: 0x06006A3F RID: 27199 RVA: 0x003BC35C File Offset: 0x003BA55C
		public AiActionCastSkill(IReadOnlyList<int> ints)
		{
			this._skillId = (short)ints[0];
		}

		// Token: 0x06006A40 RID: 27200 RVA: 0x003BC374 File Offset: 0x003BA574
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			DomainManager.Combat.StartPrepareSkill(context, this._skillId, combatChar.IsAlly);
		}

		// Token: 0x04001D55 RID: 7509
		private readonly short _skillId;
	}
}
