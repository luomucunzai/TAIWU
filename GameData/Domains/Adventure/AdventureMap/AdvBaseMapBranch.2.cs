using System;
using System.Collections.Generic;
using System.Linq;

namespace GameData.Domains.Adventure.AdventureMap
{
	// Token: 0x020008CA RID: 2250
	public abstract class AdvBaseMapBranch<T> : AdvBaseMapBranch where T : AdvMapNode
	{
		// Token: 0x170004F2 RID: 1266
		// (get) Token: 0x06007F83 RID: 32643 RVA: 0x004CA0F1 File Offset: 0x004C82F1
		// (set) Token: 0x06007F84 RID: 32644 RVA: 0x004CA0F9 File Offset: 0x004C82F9
		public T EnterNode { get; protected set; }

		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x06007F85 RID: 32645 RVA: 0x004CA102 File Offset: 0x004C8302
		// (set) Token: 0x06007F86 RID: 32646 RVA: 0x004CA10A File Offset: 0x004C830A
		public T ExitNode { get; protected set; }

		// Token: 0x06007F87 RID: 32647 RVA: 0x004CA114 File Offset: 0x004C8314
		public AdvMapNode GetNode(AdvMapPos pos)
		{
			bool flag = pos.Equals(this.EnterNode.AdjustedPos);
			AdvMapNode result;
			if (flag)
			{
				result = this.EnterNode;
			}
			else
			{
				bool flag2 = pos.Equals(this.ExitNode.AdjustedPos);
				if (flag2)
				{
					result = this.ExitNode;
				}
				else
				{
					result = this.Nodes.FirstOrDefault((AdvMapNodeNormal a) => a.AdjustedPos.Equals(pos));
				}
			}
			return result;
		}

		// Token: 0x06007F88 RID: 32648 RVA: 0x004CA1A4 File Offset: 0x004C83A4
		public AdvMapPos GetOffset(AdvMapPos src)
		{
			return this.Nodes.First((AdvMapNodeNormal a) => a.AdjustedPos.Equals(src)).Offset;
		}

		// Token: 0x06007F89 RID: 32649 RVA: 0x004CA1E0 File Offset: 0x004C83E0
		public List<AdvMapNodeNormal> FindNextNodes(AdvMapNodeNormal src)
		{
			bool flag = !this.Nodes.Contains(src);
			if (flag)
			{
				throw new Exception("This node doesn't belong to the branch");
			}
			return (from a in this.Nodes
			where a.Offset.Equals(src.Offset + new AdvMapPos(2, 0)) || a.Offset.Equals(src.Offset + new AdvMapPos(1, 1)) || a.Offset.Equals(src.Offset + new AdvMapPos(1, -1))
			select a).ToList<AdvMapNodeNormal>();
		}
	}
}
