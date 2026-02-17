using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007B0 RID: 1968
	[AiCondition(EAiConditionType.InMemoryCombatSkillRange)]
	public class AiConditionInMemoryCombatSkillRange : AiConditionCombatBase
	{
		// Token: 0x06006A16 RID: 27158 RVA: 0x003BBE23 File Offset: 0x003BA023
		public AiConditionInMemoryCombatSkillRange(IReadOnlyList<string> strings, IReadOnlyList<int> ints)
		{
			this._key = strings[0];
			this._isAlly = (ints[0] == 1);
			this._offset = ints[1];
		}

		// Token: 0x06006A17 RID: 27159 RVA: 0x003BBE58 File Offset: 0x003BA058
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			int skillId;
			bool flag = !memory.Ints.TryGetValue(this._key, out skillId) || skillId < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
				short distance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(this._offset);
				short num;
				short num2;
				checkChar.CalcAttackRangeImmediate((short)skillId, -1).Deconstruct(out num, out num2);
				short min = num;
				short max = num2;
				result = (min <= distance && distance <= max);
			}
			return result;
		}

		// Token: 0x04001D45 RID: 7493
		private readonly string _key;

		// Token: 0x04001D46 RID: 7494
		private readonly bool _isAlly;

		// Token: 0x04001D47 RID: 7495
		private readonly int _offset;
	}
}
