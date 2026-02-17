using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200073F RID: 1855
	public abstract class AiConditionSpecialAffecting : AiConditionAnyAffecting
	{
		// Token: 0x0600692D RID: 26925 RVA: 0x003B9801 File Offset: 0x003B7A01
		protected AiConditionSpecialAffecting(IReadOnlyList<int> ints) : base(ints)
		{
			this.IsDirect = (ints[1] == 1);
			this.SkillId = (short)ints[2];
		}

		// Token: 0x0600692E RID: 26926 RVA: 0x003B982C File Offset: 0x003B7A2C
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatCharacter checkChar = DomainManager.Combat.GetCombatCharacter(combatChar.IsAlly == this.IsAlly, false);
			bool flag = checkChar.GetAffectingMoveSkillId() != this.SkillId;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				CombatSkill skill;
				bool flag2 = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(checkChar.GetId(), this.SkillId), out skill);
				result = (!flag2 && skill.GetDirection() == (this.IsDirect ? 0 : 1));
			}
			return result;
		}

		// Token: 0x04001CF3 RID: 7411
		protected readonly bool IsDirect;

		// Token: 0x04001CF4 RID: 7412
		protected readonly short SkillId;
	}
}
