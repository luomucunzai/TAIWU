using System;
using System.Collections.Generic;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CE RID: 2254
	public class AdvMapNodeVertex : AdvMapNode
	{
		// Token: 0x170004FE RID: 1278
		// (get) Token: 0x06007FA8 RID: 32680 RVA: 0x004CC17E File Offset: 0x004CA37E
		public AdvMapBranch PrevBranch
		{
			get
			{
				return (this.PrevVertex != null) ? this.ConnectedBranchDict[this.PrevVertex] : null;
			}
		}

		// Token: 0x170004FF RID: 1279
		// (get) Token: 0x06007FA9 RID: 32681 RVA: 0x004CC19C File Offset: 0x004CA39C
		public ELinkDir EnterDir
		{
			get
			{
				return ELinkDir.Right;
			}
		}

		// Token: 0x06007FAA RID: 32682 RVA: 0x004CC19F File Offset: 0x004CA39F
		public AdvMapNodeVertex(ENodeType nodeType, int index, int terrainId, string nodeKey) : base(nodeType)
		{
			this.Index = index;
			this.ConnectedBranchDict = new Dictionary<AdvMapNodeVertex, AdvMapBranch>();
			this.TerrainId = terrainId;
			this.NodeKey = nodeKey;
		}

		// Token: 0x17000500 RID: 1280
		// (get) Token: 0x06007FAB RID: 32683 RVA: 0x004CC1CC File Offset: 0x004CA3CC
		public override AdvMapPos AdjustedPos
		{
			get
			{
				bool flag = this.PrevVertex == null;
				AdvMapPos result;
				if (flag)
				{
					result = default(AdvMapPos);
				}
				else
				{
					bool flag2 = this.PrevBranch.BranchLength <= 0 || this.PrevBranch.LastNode == null;
					if (flag2)
					{
						result = this.PrevVertex.AdjustedPos + this.EnterDir.Offset(2);
					}
					else
					{
						bool anchorByStart = this.PrevBranch.AnchorByStart;
						if (anchorByStart)
						{
							result = this.PrevBranch.LastNode.AdjustedPos + this.EnterDir.Offset((this.NodeType == ENodeType.Start) ? 1 : 2);
						}
						else
						{
							AdvMapPos offset = this.PrevBranch.LastNode.Offset;
							offset = new AdvMapPos((int)(-(int)offset.X), (int)(-(int)offset.Y));
							result = this.PrevVertex.AdjustedPos + this.EnterDir.Offset((this.PrevVertex.NodeType == ENodeType.Start) ? 1 : 2) + this.EnterDir.Reverse().Rotate(offset) + this.EnterDir.Offset((this.NodeType == ENodeType.Start) ? 1 : 2);
						}
					}
				}
				return result;
			}
		}

		// Token: 0x0400230F RID: 8975
		public string NodeKey;

		// Token: 0x04002310 RID: 8976
		public readonly Dictionary<AdvMapNodeVertex, AdvMapBranch> ConnectedBranchDict;

		// Token: 0x04002311 RID: 8977
		public AdvMapNodeVertex PrevVertex;
	}
}
