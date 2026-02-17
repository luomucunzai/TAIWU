using System;
using System.Collections.Generic;
using System.IO;
using Config;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x0200071B RID: 1819
	public class AiDataFile : AiData
	{
		// Token: 0x1700043D RID: 1085
		// (get) Token: 0x0600688E RID: 26766 RVA: 0x003B7365 File Offset: 0x003B5565
		protected override IReadOnlyList<IAiNode> Nodes { get; }

		// Token: 0x1700043E RID: 1086
		// (get) Token: 0x0600688F RID: 26767 RVA: 0x003B736D File Offset: 0x003B556D
		protected sealed override IReadOnlyList<IAiCondition> Conditions { get; }

		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x06006890 RID: 26768 RVA: 0x003B7375 File Offset: 0x003B5575
		protected sealed override IReadOnlyList<IAiAction> Actions { get; }

		// Token: 0x06006891 RID: 26769 RVA: 0x003B737D File Offset: 0x003B557D
		public AiDataFile(AiDataItem data) : this("../Combat/CombatAi/" + data.Path + ".aid")
		{
		}

		// Token: 0x06006892 RID: 26770 RVA: 0x003B739C File Offset: 0x003B559C
		public AiDataFile(string path)
		{
			try
			{
				using (FileStream stream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read))
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
			catch (Exception e)
			{
				PredefinedLog.Show(23, path, e.Message);
				this.Nodes = Array.Empty<IAiNode>();
				this.Conditions = Array.Empty<IAiCondition>();
				this.Actions = Array.Empty<IAiAction>();
			}
		}
	}
}
