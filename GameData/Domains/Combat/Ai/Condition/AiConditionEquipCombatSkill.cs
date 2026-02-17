using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000744 RID: 1860
	[AiCondition(EAiConditionType.EquipCombatSkill)]
	public class AiConditionEquipCombatSkill : AiConditionCheckCharBase
	{
		// Token: 0x06006937 RID: 26935 RVA: 0x003B9900 File Offset: 0x003B7B00
		public AiConditionEquipCombatSkill(IReadOnlyList<int> ints) : base(ints)
		{
			this._skillId = (short)ints[1];
		}

		// Token: 0x06006938 RID: 26936 RVA: 0x003B991C File Offset: 0x003B7B1C
		protected override bool Check(CombatCharacter checkChar)
		{
			return checkChar.GetCharacter().IsCombatSkillEquipped(this._skillId);
		}

		// Token: 0x04001CF5 RID: 7413
		private readonly short _skillId;
	}
}
