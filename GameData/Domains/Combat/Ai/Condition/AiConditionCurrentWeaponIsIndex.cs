using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000754 RID: 1876
	[AiCondition(EAiConditionType.CurrentWeaponIsIndex)]
	public class AiConditionCurrentWeaponIsIndex : AiConditionCheckCharBase
	{
		// Token: 0x06006957 RID: 26967 RVA: 0x003B9D51 File Offset: 0x003B7F51
		public AiConditionCurrentWeaponIsIndex(IReadOnlyList<int> ints) : base(ints)
		{
			this._index = (short)ints[1];
		}

		// Token: 0x06006958 RID: 26968 RVA: 0x003B9D69 File Offset: 0x003B7F69
		protected override bool Check(CombatCharacter checkChar)
		{
			return checkChar.GetUsingWeaponIndex() == (int)this._index;
		}

		// Token: 0x04001D04 RID: 7428
		private readonly short _index;
	}
}
