using System;
using System.Collections.Generic;
using System.IO;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000719 RID: 1817
	public class AiDataBase64 : AiData
	{
		// Token: 0x17000437 RID: 1079
		// (get) Token: 0x06006883 RID: 26755 RVA: 0x003B727B File Offset: 0x003B547B
		protected override IReadOnlyList<IAiNode> Nodes { get; }

		// Token: 0x17000438 RID: 1080
		// (get) Token: 0x06006884 RID: 26756 RVA: 0x003B7283 File Offset: 0x003B5483
		protected override IReadOnlyList<IAiCondition> Conditions { get; }

		// Token: 0x17000439 RID: 1081
		// (get) Token: 0x06006885 RID: 26757 RVA: 0x003B728B File Offset: 0x003B548B
		protected override IReadOnlyList<IAiAction> Actions { get; }

		// Token: 0x06006886 RID: 26758 RVA: 0x003B7294 File Offset: 0x003B5494
		public AiDataBase64(string base64Data)
		{
			byte[] dataArray = Convert.FromBase64String(base64Data);
			using (MemoryStream stream = new MemoryStream(dataArray))
			{
				IReadOnlyList<IAiNode> nodes;
				IReadOnlyList<IAiCondition> conditions;
				IReadOnlyList<IAiAction> actions;
				AiDataBinaryReader.Analysis(stream, out nodes, out conditions, out actions);
				this.Nodes = nodes;
				this.Conditions = conditions;
				this.Actions = actions;
			}
		}
	}
}
