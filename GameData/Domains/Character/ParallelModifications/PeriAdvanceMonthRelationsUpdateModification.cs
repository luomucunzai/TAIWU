using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082F RID: 2095
	public class PeriAdvanceMonthRelationsUpdateModification
	{
		// Token: 0x0600757E RID: 30078 RVA: 0x0044A56A File Offset: 0x0044876A
		public PeriAdvanceMonthRelationsUpdateModification(Character character)
		{
			this.Character = character;
			this.NewBoyOrGirlFriend = new ValueTuple<Character, bool>(null, false);
		}

		// Token: 0x04001F89 RID: 8073
		public readonly Character Character;

		// Token: 0x04001F8A RID: 8074
		public List<Character> NewlyMetCharacters;

		// Token: 0x04001F8B RID: 8075
		[TupleElementNames(new string[]
		{
			"targetChar",
			"relationType",
			"succeed"
		})]
		public List<ValueTuple<Character, ushort, bool>> NewRegularRelations;

		// Token: 0x04001F8C RID: 8076
		[TupleElementNames(new string[]
		{
			"targetChar",
			"relationType",
			"targetStillHasRelation"
		})]
		public List<ValueTuple<Character, ushort, bool>> RemovedRegularRelations;

		// Token: 0x04001F8D RID: 8077
		[TupleElementNames(new string[]
		{
			"targetChar",
			"succeed"
		})]
		public ValueTuple<Character, bool> NewBoyOrGirlFriend;
	}
}
