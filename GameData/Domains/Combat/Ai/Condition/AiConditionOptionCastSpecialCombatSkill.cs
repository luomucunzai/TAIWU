using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000798 RID: 1944
	[AiCondition(EAiConditionType.OptionCastSpecialCombatSkill)]
	public class AiConditionOptionCastSpecialCombatSkill : AiConditionOptionCastCombatSkillBase
	{
		// Token: 0x17000457 RID: 1111
		// (get) Token: 0x060069E8 RID: 27112 RVA: 0x003BB7AF File Offset: 0x003B99AF
		protected override short SkillId { get; }

		// Token: 0x060069E9 RID: 27113 RVA: 0x003BB7B7 File Offset: 0x003B99B7
		public AiConditionOptionCastSpecialCombatSkill(IReadOnlyList<int> ints)
		{
			this.SkillId = (short)ints[0];
		}
	}
}
