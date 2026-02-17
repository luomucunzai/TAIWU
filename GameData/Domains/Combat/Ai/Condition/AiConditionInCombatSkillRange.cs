using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007AF RID: 1967
	[AiCondition(EAiConditionType.InCombatSkillRange)]
	public class AiConditionInCombatSkillRange : AiConditionCheckCharBase
	{
		// Token: 0x06006A14 RID: 27156 RVA: 0x003BBDA8 File Offset: 0x003B9FA8
		public AiConditionInCombatSkillRange(IReadOnlyList<int> ints) : base(ints)
		{
			this._offset = ints[1];
			this._skillId = (short)ints[2];
		}

		// Token: 0x06006A15 RID: 27157 RVA: 0x003BBDD0 File Offset: 0x003B9FD0
		protected override bool Check(CombatCharacter checkChar)
		{
			short distance = DomainManager.Combat.GetMoveRangeOffsetCurrentDistance(this._offset);
			short num;
			short num2;
			checkChar.CalcAttackRangeImmediate(this._skillId, -1).Deconstruct(out num, out num2);
			short min = num;
			short max = num2;
			return min <= distance && distance <= max;
		}

		// Token: 0x04001D43 RID: 7491
		private readonly int _offset;

		// Token: 0x04001D44 RID: 7492
		private readonly short _skillId;
	}
}
