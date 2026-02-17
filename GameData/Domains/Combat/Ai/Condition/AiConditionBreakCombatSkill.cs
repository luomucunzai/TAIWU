using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000745 RID: 1861
	[AiCondition(EAiConditionType.BreakCombatSkill)]
	public class AiConditionBreakCombatSkill : AiConditionCheckCharBase
	{
		// Token: 0x06006939 RID: 26937 RVA: 0x003B993F File Offset: 0x003B7B3F
		public AiConditionBreakCombatSkill(IReadOnlyList<int> ints) : base(ints)
		{
			this._skillId = (short)ints[1];
			this._isDirect = (ints[2] == 1);
		}

		// Token: 0x0600693A RID: 26938 RVA: 0x003B9968 File Offset: 0x003B7B68
		protected override bool Check(CombatCharacter checkChar)
		{
			CombatSkill skill;
			return DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(checkChar.GetId(), this._skillId), out skill) && skill.GetDirection() == (this._isDirect ? 0 : 1);
		}

		// Token: 0x04001CF6 RID: 7414
		private readonly short _skillId;

		// Token: 0x04001CF7 RID: 7415
		private readonly bool _isDirect;
	}
}
