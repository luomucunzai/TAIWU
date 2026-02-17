using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000759 RID: 1881
	[AiCondition(EAiConditionType.AnyTeammate)]
	public class AiConditionAnyTeammate : AiConditionCheckCharBase
	{
		// Token: 0x06006961 RID: 26977 RVA: 0x003B9EC6 File Offset: 0x003B80C6
		public AiConditionAnyTeammate(IReadOnlyList<int> ints) : base(ints)
		{
		}

		// Token: 0x06006962 RID: 26978 RVA: 0x003B9ED4 File Offset: 0x003B80D4
		protected override bool Check(CombatCharacter checkChar)
		{
			foreach (int charId in DomainManager.Combat.GetCharacterList(checkChar.IsAlly))
			{
				bool flag = charId >= 0 && charId != checkChar.GetId();
				if (flag)
				{
					return true;
				}
			}
			return false;
		}
	}
}
