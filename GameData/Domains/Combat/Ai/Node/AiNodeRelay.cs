using System;
using System.Collections.Generic;
using GameData.Utilities;

namespace GameData.Domains.Combat.Ai.Node
{
	// Token: 0x02000730 RID: 1840
	[AiNode(EAiNodeType.Relay)]
	public class AiNodeRelay : IAiNode
	{
		// Token: 0x060068E7 RID: 26855 RVA: 0x003B8650 File Offset: 0x003B6850
		public AiNodeRelay(IReadOnlyList<int> ids)
		{
			this._ids = ids;
			Tester.Assert(this._ids.Count % 2 == 0, "");
		}

		// Token: 0x17000449 RID: 1097
		// (get) Token: 0x060068E8 RID: 26856 RVA: 0x003B867C File Offset: 0x003B687C
		// (set) Token: 0x060068E9 RID: 26857 RVA: 0x003B8684 File Offset: 0x003B6884
		public int RuntimeId { get; set; }

		// Token: 0x1700044A RID: 1098
		// (get) Token: 0x060068EA RID: 26858 RVA: 0x003B868D File Offset: 0x003B688D
		public bool IsAction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068EB RID: 26859 RVA: 0x003B8690 File Offset: 0x003B6890
		public IEnumerable<int> Update(AiData data)
		{
			for (int i = 0; i < this._ids.Count; i += 2)
			{
				int nextNodeId = this._ids[i];
				int relayNodeId = this._ids[i + 1];
				data.RelayEntry(relayNodeId);
				yield return nextNodeId;
			}
			yield break;
		}

		// Token: 0x04001CCD RID: 7373
		private readonly IReadOnlyList<int> _ids;
	}
}
