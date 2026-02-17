using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Map;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x0200082B RID: 2091
	public class PeriAdvanceMonthFixedActionModification
	{
		// Token: 0x06007576 RID: 30070 RVA: 0x0044A484 File Offset: 0x00448684
		public PeriAdvanceMonthFixedActionModification(Character character)
		{
			this.Character = character;
			this.NewGroupLeader = -1;
			this.NewGroupActionTemplateId = -1;
		}

		// Token: 0x1700048A RID: 1162
		// (get) Token: 0x06007577 RID: 30071 RVA: 0x0044A4A3 File Offset: 0x004486A3
		public bool IsChanged
		{
			get
			{
				return this.MakeLoveTargetList != null || this.ReleaseKidnappedCharList != null || this.ModifiedMapBlocks != null || this.LeaveGroup || this.NewGroupLeader >= 0;
			}
		}

		// Token: 0x04001F78 RID: 8056
		public readonly Character Character;

		// Token: 0x04001F79 RID: 8057
		[TupleElementNames(new string[]
		{
			"target",
			"makeLoveState",
			"isPregnant"
		})]
		public List<ValueTuple<Character, PeriAdvanceMonthFixedActionModification.MakeLoveState, bool>> MakeLoveTargetList;

		// Token: 0x04001F7A RID: 8058
		public List<int> ReleaseKidnappedCharList;

		// Token: 0x04001F7B RID: 8059
		public List<MapBlockData> ModifiedMapBlocks;

		// Token: 0x04001F7C RID: 8060
		public bool LeaveGroup;

		// Token: 0x04001F7D RID: 8061
		public int NewGroupLeader;

		// Token: 0x04001F7E RID: 8062
		public short NewGroupActionTemplateId;

		// Token: 0x04001F7F RID: 8063
		public bool TravelTargetsChanged;

		// Token: 0x02000C0A RID: 3082
		public enum MakeLoveState
		{
			// Token: 0x0400341A RID: 13338
			Legal,
			// Token: 0x0400341B RID: 13339
			Illegal,
			// Token: 0x0400341C RID: 13340
			Wug,
			// Token: 0x0400341D RID: 13341
			RapeSucceed,
			// Token: 0x0400341E RID: 13342
			RapeFail
		}
	}
}
