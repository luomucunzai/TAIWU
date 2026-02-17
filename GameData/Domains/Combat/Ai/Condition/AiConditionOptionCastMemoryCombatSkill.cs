using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200079A RID: 1946
	[AiCondition(EAiConditionType.OptionCastMemoryCombatSkill)]
	public class AiConditionOptionCastMemoryCombatSkill : AiConditionOptionCastCombatSkillBase
	{
		// Token: 0x17000458 RID: 1112
		// (get) Token: 0x060069EC RID: 27116 RVA: 0x003BB846 File Offset: 0x003B9A46
		protected override short SkillId
		{
			get
			{
				return (short)base.Memory.Ints.GetValueOrDefault(this._key, -1);
			}
		}

		// Token: 0x060069ED RID: 27117 RVA: 0x003BB860 File Offset: 0x003B9A60
		public AiConditionOptionCastMemoryCombatSkill(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x04001D3C RID: 7484
		private readonly string _key;
	}
}
