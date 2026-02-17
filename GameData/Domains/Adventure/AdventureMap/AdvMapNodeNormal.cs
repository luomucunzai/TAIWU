using System;
using GameData.Common;
using Redzen.Random;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CF RID: 2255
	public class AdvMapNodeNormal : AdvMapNode
	{
		// Token: 0x17000501 RID: 1281
		// (get) Token: 0x06007FAC RID: 32684 RVA: 0x004CC30A File Offset: 0x004CA50A
		// (set) Token: 0x06007FAD RID: 32685 RVA: 0x004CC312 File Offset: 0x004CA512
		public AdvMapPos Offset { get; set; }

		// Token: 0x06007FAE RID: 32686 RVA: 0x004CC31C File Offset: 0x004CA51C
		public AdvMapNodeNormal(int terrainId, AdvBaseMapBranch mapBranch, DataContext context, sbyte sevenElementType) : base(ENodeType.Normal)
		{
			this.TerrainId = terrainId;
			this.AffiliatedBranch = mapBranch;
			this.BranchIndex = (sbyte)this.AffiliatedBranch.BranchIndex;
			this.SevenElementType = sevenElementType;
			bool flag = this.SevenElementType != -1;
			if (flag)
			{
				this.SevenElementCost = (sbyte)context.Random.Next((int)GlobalConfig.Instance.AdventureNodePersonalityMinCost, (int)(GlobalConfig.Instance.AdventureNodePersonalityMaxCost + 1));
			}
			else
			{
				this.SevenElementCost = 0;
			}
		}

		// Token: 0x17000502 RID: 1282
		// (get) Token: 0x06007FAF RID: 32687 RVA: 0x004CC3A4 File Offset: 0x004CA5A4
		public override AdvMapPos AdjustedPos
		{
			get
			{
				return this.AffiliatedBranch.BaseBranch.BasePos + this.Offset;
			}
		}

		// Token: 0x06007FB0 RID: 32688 RVA: 0x004CC3C4 File Offset: 0x004CA5C4
		internal int GetRandomValue(IRandomSource random)
		{
			bool flag = this._randValue != -1;
			int result;
			if (flag)
			{
				result = this._randValue;
			}
			else
			{
				result = (this._randValue = random.Next(0, 100));
			}
			return result;
		}

		// Token: 0x04002312 RID: 8978
		public readonly AdvBaseMapBranch AffiliatedBranch;

		// Token: 0x04002314 RID: 8980
		private int _randValue = -1;
	}
}
