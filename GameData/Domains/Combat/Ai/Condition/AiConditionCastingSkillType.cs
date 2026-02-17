using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000739 RID: 1849
	[AiCondition(EAiConditionType.CastingSkillType)]
	public class AiConditionCastingSkillType : AiConditionCombatBase
	{
		// Token: 0x06006920 RID: 26912 RVA: 0x003B95B9 File Offset: 0x003B77B9
		public AiConditionCastingSkillType(IReadOnlyList<int> ints)
		{
			this._isAlly = (ints[0] == 1);
			this._equipType = (sbyte)ints[1];
		}

		// Token: 0x06006921 RID: 26913 RVA: 0x003B95E4 File Offset: 0x003B77E4
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this._isAlly, false);
			bool flag = checkChar.GetPreparingSkillId() < 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				sbyte equipType = CombatSkill.Instance[checkChar.GetPreparingSkillId()].EquipType;
				result = (equipType == this._equipType);
			}
			return result;
		}

		// Token: 0x04001CEC RID: 7404
		private readonly bool _isAlly;

		// Token: 0x04001CED RID: 7405
		private readonly sbyte _equipType;
	}
}
