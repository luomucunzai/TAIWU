using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200074F RID: 1871
	[AiCondition(EAiConditionType.ConsummateLevelMoreOrEqual)]
	public class AiConditionConsummateLevelMoreOrEqual : AiConditionCheckCharBase
	{
		// Token: 0x0600694D RID: 26957 RVA: 0x003B9BCA File Offset: 0x003B7DCA
		public AiConditionConsummateLevelMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			this._consummateLevel = ints[0];
		}

		// Token: 0x0600694E RID: 26958 RVA: 0x003B9BE4 File Offset: 0x003B7DE4
		protected override bool Check(CombatCharacter checkChar)
		{
			return checkChar.IsAlly || (int)checkChar.GetCharacter().GetConsummateLevel() >= this._consummateLevel;
		}

		// Token: 0x04001CFE RID: 7422
		private readonly int _consummateLevel;
	}
}
