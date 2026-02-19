using System;
using System.Collections.Generic;
using System.IO;

namespace GameData.Domains.TaiwuEvent.Decompiler;

public class EventScriptDecompiler_0_0_2_0 : IEventScriptDecompiler
{
	public string Version => "0.0.2.0";

	public void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventScript script)
	{
		script.Labels = new Dictionary<string, int>();
		int num = binaryReader.ReadInt32();
		for (int i = 0; i < num; i++)
		{
			string key = binaryReader.ReadString();
			int value = binaryReader.ReadInt32();
			script.Labels.Add(key, value);
		}
	}

	public void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventConditionList conditionList)
	{
	}

	public EventInstruction DecompileInstruction(EventScriptRuntime runtime, BinaryReader binaryReader)
	{
		int indentAmount = binaryReader.ReadInt32();
		string assignToVar = binaryReader.ReadString();
		int functionId = binaryReader.ReadInt32();
		int num = binaryReader.ReadInt32();
		string[] array = ((num > 0) ? new string[num] : Array.Empty<string>());
		for (int i = 0; i < num; i++)
		{
			array[i] = binaryReader.ReadString();
		}
		return runtime.CreateInst(functionId, indentAmount, assignToVar, array);
	}

	public EventCondition DecompileCondition(EventScriptRuntime runtime, BinaryReader binaryReader)
	{
		int indent = binaryReader.ReadInt32();
		bool reverse = binaryReader.ReadBoolean();
		int functionId = binaryReader.ReadInt32();
		int num = binaryReader.ReadInt32();
		string[] array = ((num > 0) ? new string[num] : Array.Empty<string>());
		for (int i = 0; i < num; i++)
		{
			array[i] = binaryReader.ReadString();
		}
		return runtime.CreateCondition(functionId, indent, reverse, array);
	}
}
