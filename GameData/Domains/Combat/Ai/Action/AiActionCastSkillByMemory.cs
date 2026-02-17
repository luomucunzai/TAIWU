using System;
using System.Collections.Generic;
using GameData.Common;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007BF RID: 1983
	[AiAction(EAiActionType.CastSkillByMemory)]
	public class AiActionCastSkillByMemory : AiActionCombatBase
	{
		// Token: 0x06006A41 RID: 27201 RVA: 0x003BC3A5 File Offset: 0x003BA5A5
		public AiActionCastSkillByMemory(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006A42 RID: 27202 RVA: 0x003BC3BC File Offset: 0x003BA5BC
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0;
			if (!flag)
			{
				DataContext context = DomainManager.Combat.Context;
				DomainManager.Combat.StartPrepareSkill(context, (short)skillId, combatChar.IsAlly);
			}
		}

		// Token: 0x04001D56 RID: 7510
		private readonly string _key;
	}
}
