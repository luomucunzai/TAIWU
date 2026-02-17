using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Node
{
	// Token: 0x0200072D RID: 1837
	[AiNode(EAiNodeType.Action)]
	public class AiNodeAction : IAiNode
	{
		// Token: 0x060068D8 RID: 26840 RVA: 0x003B857F File Offset: 0x003B677F
		public AiNodeAction(IReadOnlyList<int> ids)
		{
			this._ids = ids;
		}

		// Token: 0x17000443 RID: 1091
		// (get) Token: 0x060068D9 RID: 26841 RVA: 0x003B858F File Offset: 0x003B678F
		// (set) Token: 0x060068DA RID: 26842 RVA: 0x003B8597 File Offset: 0x003B6797
		public int RuntimeId { get; set; }

		// Token: 0x17000444 RID: 1092
		// (get) Token: 0x060068DB RID: 26843 RVA: 0x003B85A0 File Offset: 0x003B67A0
		public bool IsAction
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060068DC RID: 26844 RVA: 0x003B85A3 File Offset: 0x003B67A3
		public IEnumerable<int> Update(AiData data)
		{
			foreach (int actionId in this._ids)
			{
				data.ExecuteAction(actionId);
			}
			IEnumerator<int> enumerator = null;
			yield break;
		}

		// Token: 0x04001CC7 RID: 7367
		private readonly IReadOnlyList<int> _ids;
	}
}
