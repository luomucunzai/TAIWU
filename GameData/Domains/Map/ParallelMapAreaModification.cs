using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.Item;
using GameData.Domains.Organization.ParallelModifications;

namespace GameData.Domains.Map
{
	// Token: 0x020008BB RID: 2235
	public class ParallelMapAreaModification
	{
		// Token: 0x04002248 RID: 8776
		public short AreaId;

		// Token: 0x04002249 RID: 8777
		public readonly Dictionary<short, ParallelSettlementModification> SettlementDict = new Dictionary<short, ParallelSettlementModification>();

		// Token: 0x0400224A RID: 8778
		[TupleElementNames(new string[]
		{
			"character",
			"param"
		})]
		public readonly List<ValueTuple<Character, AddOrIncreaseInjuryParams>> CharInjuries = new List<ValueTuple<Character, AddOrIncreaseInjuryParams>>();

		// Token: 0x0400224B RID: 8779
		public readonly List<short> DisasterBlocks = new List<short>();

		// Token: 0x0400224C RID: 8780
		public readonly List<int> DeadCharList = new List<int>();

		// Token: 0x0400224D RID: 8781
		public readonly List<int> DamageGraveList = new List<int>();

		// Token: 0x0400224E RID: 8782
		public readonly Dictionary<short, short> DisasterAdventureId = new Dictionary<short, short>();

		// Token: 0x0400224F RID: 8783
		public readonly List<ItemKey> DestroyedUniqueItems = new List<ItemKey>();
	}
}
