using System;
using System.Collections.Generic;

namespace GameData.Domains.Combat.Ai.Node
{
	// Token: 0x0200072F RID: 1839
	[AiNode(EAiNodeType.Linear)]
	public class AiNodeLinear : IAiNode
	{
		// Token: 0x060068E2 RID: 26850 RVA: 0x003B8611 File Offset: 0x003B6811
		public AiNodeLinear(IReadOnlyList<int> ids)
		{
			this._ids = ids;
		}

		// Token: 0x17000447 RID: 1095
		// (get) Token: 0x060068E3 RID: 26851 RVA: 0x003B8621 File Offset: 0x003B6821
		// (set) Token: 0x060068E4 RID: 26852 RVA: 0x003B8629 File Offset: 0x003B6829
		public int RuntimeId { get; set; }

		// Token: 0x17000448 RID: 1096
		// (get) Token: 0x060068E5 RID: 26853 RVA: 0x003B8632 File Offset: 0x003B6832
		public bool IsAction
		{
			get
			{
				return false;
			}
		}

		// Token: 0x060068E6 RID: 26854 RVA: 0x003B8638 File Offset: 0x003B6838
		public IEnumerable<int> Update(AiData data)
		{
			return this._ids;
		}

		// Token: 0x04001CCB RID: 7371
		private readonly IReadOnlyList<int> _ids;
	}
}
