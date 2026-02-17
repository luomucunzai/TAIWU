using System;
using System.Collections.Generic;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008C9 RID: 2249
	public abstract class AdvBaseMapBranch
	{
		// Token: 0x170004EF RID: 1263
		// (get) Token: 0x06007F7C RID: 32636 RVA: 0x004CA0A3 File Offset: 0x004C82A3
		// (set) Token: 0x06007F7D RID: 32637 RVA: 0x004CA0AB File Offset: 0x004C82AB
		public AdvMapBranch BaseBranch { get; protected set; }

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x06007F7E RID: 32638 RVA: 0x004CA0B4 File Offset: 0x004C82B4
		// (set) Token: 0x06007F7F RID: 32639 RVA: 0x004CA0BC File Offset: 0x004C82BC
		public virtual AdvMapNodeNormal FirstNode { get; protected set; }

		// Token: 0x170004F1 RID: 1265
		// (get) Token: 0x06007F80 RID: 32640 RVA: 0x004CA0C5 File Offset: 0x004C82C5
		// (set) Token: 0x06007F81 RID: 32641 RVA: 0x004CA0CD File Offset: 0x004C82CD
		public virtual AdvMapNodeNormal LastNode { get; protected set; }

		// Token: 0x040022F5 RID: 8949
		public int BranchIndex = -1;

		// Token: 0x040022F6 RID: 8950
		public readonly List<AdvMapNodeNormal> Nodes = new List<AdvMapNodeNormal>();
	}
}
