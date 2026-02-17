using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073A RID: 1850
	[AiCondition(EAiConditionType.CastingSkill)]
	public class AiConditionCastingSkill : AiConditionCombatBase
	{
		// Token: 0x06006922 RID: 26914 RVA: 0x003B9640 File Offset: 0x003B7840
		public AiConditionCastingSkill(IReadOnlyList<int> ints)
		{
			this._isAlly = (ints[0] == 1);
			this._skillId = (short)ints[1];
		}

		// Token: 0x06006923 RID: 26915 RVA: 0x003B9668 File Offset: 0x003B7868
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
			return checkChar.GetPreparingSkillId() == this._skillId;
		}

		// Token: 0x04001CEE RID: 7406
		private readonly bool _isAlly;

		// Token: 0x04001CEF RID: 7407
		private readonly short _skillId;
	}
}
