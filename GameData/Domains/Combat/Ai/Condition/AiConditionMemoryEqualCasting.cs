using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000782 RID: 1922
	[AiCondition(EAiConditionType.MemoryEqualCasting)]
	public class AiConditionMemoryEqualCasting : AiConditionCombatBase
	{
		// Token: 0x060069B4 RID: 27060 RVA: 0x003BA9DA File Offset: 0x003B8BDA
		public AiConditionMemoryEqualCasting(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._isAlly = (ints[0] == 1);
		}

		// Token: 0x060069B5 RID: 27061 RVA: 0x003BAA04 File Offset: 0x003B8C04
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int memoryCasting;
			bool flag = !memory.Ints.TryGetValue(this._key, out memoryCasting);
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
				result = (memoryCasting == (int)checkChar.GetPreparingSkillId());
			}
			return result;
		}

		// Token: 0x04001D2C RID: 7468
		private readonly string _key;

		// Token: 0x04001D2D RID: 7469
		private readonly bool _isAlly;
	}
}
