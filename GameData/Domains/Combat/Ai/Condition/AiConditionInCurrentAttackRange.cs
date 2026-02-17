using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AE RID: 1966
	[AiCondition(EAiConditionType.InCurrentAttackRange)]
	public class AiConditionInCurrentAttackRange : AiConditionCheckCharBase
	{
		// Token: 0x06006A12 RID: 27154 RVA: 0x003BBD34 File Offset: 0x003B9F34
		public AiConditionInCurrentAttackRange(IReadOnlyList<int> ints) : base(ints)
		{
			this._offset = ints[1];
		}

		// Token: 0x06006A13 RID: 27155 RVA: 0x003BBD4C File Offset: 0x003B9F4C
		protected override bool Check(CombatCharacter checkChar)
		{
			bool canAttackOutRange = checkChar.GetCanAttackOutRange();
			bool result;
			if (canAttackOutRange)
			{
				result = true;
			}
			else
			{
				short distance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(this._offset);
				short num;
				short num2;
				checkChar.GetAttackRange().Deconstruct(out num, out num2);
				short min = num;
				short max = num2;
				result = (min <= distance && distance <= max);
			}
			return result;
		}

		// Token: 0x04001D42 RID: 7490
		private readonly int _offset;
	}
}
