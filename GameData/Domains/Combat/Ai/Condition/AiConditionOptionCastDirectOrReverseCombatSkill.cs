using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000799 RID: 1945
	[AiCondition(EAiConditionType.OptionCastDirectOrReverseCombatSkill)]
	public class AiConditionOptionCastDirectOrReverseCombatSkill : AiConditionOptionCastSpecialCombatSkill
	{
		// Token: 0x060069EA RID: 27114 RVA: 0x003BB7CE File Offset: 0x003B99CE
		public AiConditionOptionCastDirectOrReverseCombatSkill(IReadOnlyList<int> ints) : base(ints)
		{
			this._isDirect = (ints[1] == 1);
		}

		// Token: 0x060069EB RID: 27115 RVA: 0x003BB7E8 File Offset: 0x003B99E8
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			CombatSkill skill;
			bool flag = !DomainManager.CombatSkill.TryGetElement_CombatSkills(new ValueTuple<int, short>(combatChar.GetId(), this.SkillId), out skill);
			return !flag && skill.GetDirection() == (this._isDirect ? 0 : 1) && base.Check(memory, combatChar);
		}

		// Token: 0x04001D3B RID: 7483
		private readonly bool _isDirect;
	}
}
