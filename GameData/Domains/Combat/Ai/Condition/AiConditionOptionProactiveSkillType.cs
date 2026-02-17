using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000794 RID: 1940
	[AiCondition(EAiConditionType.OptionProactiveSkillType)]
	public class AiConditionOptionProactiveSkillType : AiConditionCombatBase
	{
		// Token: 0x060069DB RID: 27099 RVA: 0x003BB436 File Offset: 0x003B9636
		private static bool IsValid(short skillId)
		{
			return skillId >= 0;
		}

		// Token: 0x060069DC RID: 27100 RVA: 0x003BB43F File Offset: 0x003B963F
		public AiConditionOptionProactiveSkillType(IReadOnlyList<int> ints)
		{
			this._equipType = (sbyte)ints[0];
		}

		// Token: 0x060069DD RID: 27101 RVA: 0x003BB458 File Offset: 0x003B9658
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			sbyte equipType = this._equipType;
			if (!true)
			{
			}
			int num;
			switch (equipType)
			{
			case 1:
				num = 0;
				break;
			case 2:
				num = 1;
				break;
			case 3:
				num = 2;
				break;
			default:
				num = -1;
				break;
			}
			if (!true)
			{
			}
			int index = num;
			bool flag = combatChar.IsAlly && (index < 0 || !DomainManager.Combat.AiOptions.AutoCastSkill[index]);
			return !flag && combatChar.GetCombatSkillList(this._equipType).Where(new Func<short, bool>(AiConditionOptionProactiveSkillType.IsValid)).Where(new Func<short, bool>(combatChar.AiCanCast)).Any<short>();
		}

		// Token: 0x04001D38 RID: 7480
		private readonly sbyte _equipType;
	}
}
