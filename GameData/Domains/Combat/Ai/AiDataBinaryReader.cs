using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using Config;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000718 RID: 1816
	public static class AiDataBinaryReader
	{
		// Token: 0x06006881 RID: 26753 RVA: 0x003B7018 File Offset: 0x003B5218
		public static void Analysis(Stream stream, out IReadOnlyList<IAiNode> dataNodes, out IReadOnlyList<IAiCondition> dataConditions, out IReadOnlyList<IAiAction> dataActions)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8))
			{
				int nodeCount = reader.ReadInt32();
				List<IAiNode> nodes = new List<IAiNode>(nodeCount);
				dataNodes = nodes;
				for (int i = 0; i < nodeCount; i++)
				{
					EAiNodeType type = (EAiNodeType)reader.ReadInt32();
					int nodeOrActionIdCount = reader.ReadInt32();
					List<int> nodeOrActionIds = new List<int>(nodeOrActionIdCount);
					for (int pi = 0; pi < nodeOrActionIdCount; pi++)
					{
						nodeOrActionIds.Add(reader.ReadInt32());
					}
					nodes.Add(AiNodeFactory.Create(type, i, nodeOrActionIds));
				}
				int conditionCount = reader.ReadInt32();
				List<IAiCondition> conditions = new List<IAiCondition>(conditionCount);
				dataConditions = conditions;
				for (int j = 0; j < conditionCount; j++)
				{
					EAiConditionType type2 = (EAiConditionType)reader.ReadInt32();
					AiConditionItem config = AiCondition.Instance[(int)type2];
					ValueTuple<IReadOnlyList<string>, IReadOnlyList<int>> valueTuple = AiDataBinaryReader.ReadParam(reader, config.ParamStrings, config.ParamInts);
					IReadOnlyList<string> strings = valueTuple.Item1;
					IReadOnlyList<int> ints = valueTuple.Item2;
					conditions.Add(AiConditionFactory.Create(type2, j, strings, ints));
				}
				int actionCount = reader.ReadInt32();
				List<IAiAction> actions = new List<IAiAction>(actionCount);
				dataActions = actions;
				for (int k = 0; k < actionCount; k++)
				{
					EAiActionType type3 = (EAiActionType)reader.ReadInt32();
					AiActionItem config2 = AiAction.Instance[(int)type3];
					ValueTuple<IReadOnlyList<string>, IReadOnlyList<int>> valueTuple2 = AiDataBinaryReader.ReadParam(reader, config2.ParamStrings, config2.ParamInts);
					IReadOnlyList<string> strings2 = valueTuple2.Item1;
					IReadOnlyList<int> ints2 = valueTuple2.Item2;
					actions.Add(AiActionFactory.Create(type3, k, strings2, ints2));
				}
			}
		}

		// Token: 0x06006882 RID: 26754 RVA: 0x003B71CC File Offset: 0x003B53CC
		[return: TupleElementNames(new string[]
		{
			"strings",
			"ints"
		})]
		private static ValueTuple<IReadOnlyList<string>, IReadOnlyList<int>> ReadParam(BinaryReader reader, IReadOnlyList<int> configStrings, IReadOnlyList<int> configInts)
		{
			List<string> strings = (configStrings != null && configStrings.Count > 0) ? new List<string>(configStrings.Count) : null;
			bool flag = strings != null;
			if (flag)
			{
				for (int pi = 0; pi < configStrings.Count; pi++)
				{
					strings.Add(reader.ReadString());
				}
			}
			List<int> ints = (configInts != null && configInts.Count > 0) ? new List<int>(configInts.Count) : null;
			bool flag2 = ints != null;
			if (flag2)
			{
				for (int pi2 = 0; pi2 < configInts.Count; pi2++)
				{
					ints.Add(reader.ReadInt32());
				}
			}
			return new ValueTuple<IReadOnlyList<string>, IReadOnlyList<int>>(strings, ints);
		}
	}
}
