using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000747 RID: 1863
	[AiCondition(EAiConditionType.CombatSkillEffectCountMoreOrEqual)]
	public class AiConditionCombatSkillEffectCountMoreOrEqual : AiConditionCheckCharBase
	{
		// Token: 0x0600693D RID: 26941 RVA: 0x003B99F8 File Offset: 0x003B7BF8
		public AiConditionCombatSkillEffectCountMoreOrEqual(IReadOnlyList<int> ints) : base(ints)
		{
			bool isDirect = ints[1] == 1;
			short skillId = (short)ints[2];
			this._effectKey = new SkillEffectKey(skillId, isDirect);
			this._count = ints[3];
		}

		// Token: 0x0600693E RID: 26942 RVA: 0x003B9A3C File Offset: 0x003B7C3C
		protected override bool Check(CombatCharacter checkChar)
		{
			SkillEffectCollection effectCollection = checkChar.GetSkillEffectCollection();
			Dictionary<SkillEffectKey, short> effectDict = (effectCollection != null) ? effectCollection.EffectDict : null;
			bool flag = effectDict == null;
			short count;
			return !flag && effectDict.TryGetValue(this._effectKey, out count) && (int)count >= this._count;
		}

		// Token: 0x04001CF9 RID: 7417
		private readonly SkillEffectKey _effectKey;

		// Token: 0x04001CFA RID: 7418
		private readonly int _count;
	}
}
