using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000758 RID: 1880
	[AiCondition(EAiConditionType.AnyNotInfinitySilenceSkill)]
	public class AiConditionAnyNotInfinitySilenceSkill : AiConditionCheckCharBase
	{
		// Token: 0x0600695F RID: 26975 RVA: 0x003B9E9A File Offset: 0x003B809A
		public AiConditionAnyNotInfinitySilenceSkill(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006960 RID: 26976 RVA: 0x003B9EA8 File Offset: 0x003B80A8
		protected override bool Check(CombatCharacter checkChar)
		{
			return checkChar.GetBannedSkillIds(true).Any<short>();
		}
	}
}
