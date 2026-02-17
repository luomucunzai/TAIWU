using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Node
{
	// Token: 0x0200072E RID: 1838
	[AiNode(EAiNodeType.Branch)]
	public class AiNodeBranch : IAiNode
	{
		// Token: 0x060068DD RID: 26845 RVA: 0x003B85BA File Offset: 0x003B67BA
		public AiNodeBranch(IReadOnlyList<int> ids)
		{
			this._ids = ids;
			Tester.Assert(this._ids.Count % 3 == 0, "");
		}

		// Token: 0x17000445 RID: 1093
		// (get) Token: 0x060068DE RID: 26846 RVA: 0x003B85E6 File Offset: 0x003B67E6
		// (set) Token: 0x060068DF RID: 26847 RVA: 0x003B85EE File Offset: 0x003B67EE
		public int RuntimeId { get; set; }

		// Token: 0x17000446 RID: 1094
		// (get) Token: 0x060068E0 RID: 26848 RVA: 0x003B85F7 File Offset: 0x003B67F7
		public bool IsAction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068E1 RID: 26849 RVA: 0x003B85FA File Offset: 0x003B67FA
		public IEnumerable<int> Update(AiData data)
		{
			for (int i = 0; i < this._ids.Count; i += 3)
			{
				int conditionId = this._ids[i];
				int trueNodeId = this._ids[i + 1];
				int falseNodeId = this._ids[i + 2];
				bool flag = data.CheckCondition(conditionId);
				if (flag)
				{
					yield return trueNodeId;
				}
				else
				{
					yield return falseNodeId;
				}
			}
			yield break;
		}

		// Token: 0x04001CC9 RID: 7369
		private readonly IReadOnlyList<int> _ids;
	}
}
