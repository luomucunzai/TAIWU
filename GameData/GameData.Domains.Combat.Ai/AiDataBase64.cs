using System;
using System.Collections.Generic;
using System.IO;

namespace GameData.Domains.Combat.Ai;

public class AiDataBase64 : AiData
{
	protected override IReadOnlyList<IAiNode> Nodes { get; }

	protected override IReadOnlyList<IAiCondition> Conditions { get; }

	protected override IReadOnlyList<IAiAction> Actions { get; }

	public AiDataBase64(string base64Data)
	{
		byte[] buffer = Convert.FromBase64String(base64Data);
		using MemoryStream stream = new MemoryStream(buffer);
		AiDataBinaryReader.Analysis(stream, out var dataNodes, out var dataConditions, out var dataActions);
		Nodes = dataNodes;
		Conditions = dataConditions;
		Actions = dataActions;
	}
}
