using System;
using GameData.Common;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate
{
	// Token: 0x0200006C RID: 108
	public class DebateStrategyActionParams
	{
		// Token: 0x0400038B RID: 907
		public DataContext Context;

		// Token: 0x0400038C RID: 908
		public bool IsCastedByTaiwu;

		// Token: 0x0400038D RID: 909
		public int PawnId;

		// Token: 0x0400038E RID: 910
		public int PawnId2;

		// Token: 0x0400038F RID: 911
		public int Value;

		// Token: 0x04000390 RID: 912
		public sbyte Grade;

		// Token: 0x04000391 RID: 913
		public IntPair Coordinate;

		// Token: 0x04000392 RID: 914
		public int UsingCard;

		// Token: 0x04000393 RID: 915
		public int Card;

		// Token: 0x04000394 RID: 916
		public short StrategyTemplateId;

		// Token: 0x04000395 RID: 917
		public DebateNodeEffectState NodeEffect = DebateNodeEffectState.Invalid;
	}
}
