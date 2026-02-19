using System.Collections.Generic;
using System.IO;
using System.Text;
using Config;

namespace GameData.Domains.Combat.Ai;

public static class AiDataBinaryReader
{
	public static void Analysis(Stream stream, out IReadOnlyList<IAiNode> dataNodes, out IReadOnlyList<IAiCondition> dataConditions, out IReadOnlyList<IAiAction> dataActions)
	{
		using BinaryReader binaryReader = new BinaryReader(stream, Encoding.UTF8);
		int num = binaryReader.ReadInt32();
		List<IAiNode> list = (List<IAiNode>)(dataNodes = new List<IAiNode>(num));
		for (int i = 0; i < num; i++)
		{
			EAiNodeType type = (EAiNodeType)binaryReader.ReadInt32();
			int num2 = binaryReader.ReadInt32();
			List<int> list2 = new List<int>(num2);
			for (int j = 0; j < num2; j++)
			{
				list2.Add(binaryReader.ReadInt32());
			}
			list.Add(AiNodeFactory.Create(type, i, list2));
		}
		int num3 = binaryReader.ReadInt32();
		List<IAiCondition> list3 = (List<IAiCondition>)(dataConditions = new List<IAiCondition>(num3));
		for (int k = 0; k < num3; k++)
		{
			EAiConditionType eAiConditionType = (EAiConditionType)binaryReader.ReadInt32();
			AiConditionItem aiConditionItem = AiCondition.Instance[(int)eAiConditionType];
			var (strings, ints) = ReadParam(binaryReader, aiConditionItem.ParamStrings, aiConditionItem.ParamInts);
			list3.Add(AiConditionFactory.Create(eAiConditionType, k, strings, ints));
		}
		int num4 = binaryReader.ReadInt32();
		List<IAiAction> list4 = (List<IAiAction>)(dataActions = new List<IAiAction>(num4));
		for (int l = 0; l < num4; l++)
		{
			EAiActionType eAiActionType = (EAiActionType)binaryReader.ReadInt32();
			AiActionItem aiActionItem = AiAction.Instance[(int)eAiActionType];
			var (strings2, ints2) = ReadParam(binaryReader, aiActionItem.ParamStrings, aiActionItem.ParamInts);
			list4.Add(AiActionFactory.Create(eAiActionType, l, strings2, ints2));
		}
	}

	private static (IReadOnlyList<string> strings, IReadOnlyList<int> ints) ReadParam(BinaryReader reader, IReadOnlyList<int> configStrings, IReadOnlyList<int> configInts)
	{
		List<string> list = ((configStrings != null && configStrings.Count > 0) ? new List<string>(configStrings.Count) : null);
		if (list != null)
		{
			for (int i = 0; i < configStrings.Count; i++)
			{
				list.Add(reader.ReadString());
			}
		}
		List<int> list2 = ((configInts != null && configInts.Count > 0) ? new List<int>(configInts.Count) : null);
		if (list2 != null)
		{
			for (int j = 0; j < configInts.Count; j++)
			{
				list2.Add(reader.ReadInt32());
			}
		}
		return (strings: list, ints: list2);
	}
}
