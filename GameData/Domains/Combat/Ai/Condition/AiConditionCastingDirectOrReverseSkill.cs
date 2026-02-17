using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073B RID: 1851
	[AiCondition(EAiConditionType.CastingDirectOrReverseSkill)]
	public class AiConditionCastingDirectOrReverseSkill : AiConditionCheckCharBase
	{
		// Token: 0x06006924 RID: 26916 RVA: 0x003B96A2 File Offset: 0x003B78A2
		public AiConditionCastingDirectOrReverseSkill(IReadOnlyList<int> ints) : base(ints)
		{
			this._isDirect = (ints[1] == 1);
			this._skillId = (short)ints[2];
		}

		// Token: 0x06006925 RID: 26917 RVA: 0x003B96CC File Offset: 0x003B78CC
		protected override bool Check(CombatCharacter checkChar)
		{
			short casting = checkChar.GetPreparingSkillId();
			bool flag = casting != this._skillId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatSkill skill;
				bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(checkChar.GetId(), casting), out skill);
				result = (!flag2 && skill.GetDirection() == (this._isDirect ? 0 : 1));
			}
			return result;
		}

		// Token: 0x04001CF0 RID: 7408
		private readonly bool _isDirect;

		// Token: 0x04001CF1 RID: 7409
		private readonly short _skillId;
	}
}
