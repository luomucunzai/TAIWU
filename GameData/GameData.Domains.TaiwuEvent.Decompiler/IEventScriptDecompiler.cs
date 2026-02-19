using System;
using System.Collections.Generic;
using System.IO;

namespace GameData.Domains.TaiwuEvent.Decompiler;

public interface IEventScriptDecompiler
{
	string Version { get; }

	Dictionary<EventScriptId, EventScriptBase> DecompileEventScriptPackage(EventScriptRuntime runtime, BinaryReader binaryReader)
	{
		Dictionary<EventScriptId, EventScriptBase> dictionary = new Dictionary<EventScriptId, EventScriptBase>();
		while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
		{
			EventScriptId id = DecompileScriptId(runtime, binaryReader);
			EventScriptBase eventScriptBase = (EventScriptId.IsConditionList(id.Type) ? ((EventScriptBase)DecompileConditionList(runtime, id, binaryReader)) : ((EventScriptBase)DecompileEventScript(runtime, id, binaryReader)));
			dictionary.Add(eventScriptBase.Id, eventScriptBase);
		}
		return dictionary;
	}

	EventConditionList DecompileConditionList(EventScriptRuntime runtime, EventScriptId id, BinaryReader binaryReader)
	{
		EventConditionList eventConditionList = new EventConditionList();
		eventConditionList.Id = id;
		DecompileMetaData(runtime, binaryReader, eventConditionList);
		int num = binaryReader.ReadInt32();
		eventConditionList.Conditions = new EventCondition[num];
		for (int i = 0; i < num; i++)
		{
			try
			{
				eventConditionList.Conditions[i] = DecompileCondition(runtime, binaryReader);
			}
			catch (Exception innerException)
			{
				throw new TaiwuEventScriptException("Failed to decompile instruction.", eventConditionList.Id, i, innerException);
			}
		}
		return eventConditionList;
	}

	EventScript DecompileEventScript(EventScriptRuntime runtime, EventScriptId id, BinaryReader binaryReader)
	{
		EventScript eventScript = new EventScript();
		eventScript.Id = id;
		DecompileMetaData(runtime, binaryReader, eventScript);
		int num = binaryReader.ReadInt32();
		eventScript.Instructions = new EventInstruction[num];
		for (int i = 0; i < num; i++)
		{
			try
			{
				eventScript.Instructions[i] = DecompileInstruction(runtime, binaryReader);
			}
			catch (Exception innerException)
			{
				throw new TaiwuEventScriptException("Failed to decompile condition.", eventScript.Id, i, innerException);
			}
		}
		return eventScript;
	}

	EventScriptId DecompileScriptId(EventScriptRuntime runtime, BinaryReader binaryReader)
	{
		sbyte type = binaryReader.ReadSByte();
		Span<byte> span = stackalloc byte[16];
		if (binaryReader.Read(span) < 16)
		{
			throw new Exception($"Failed read guid of size {16} at offset {binaryReader.BaseStream.Position}.");
		}
		Guid guid = new Guid(span);
		if (!EventScriptId.IsOptionType(type))
		{
			return new EventScriptId(type, guid);
		}
		if (binaryReader.Read(span) < 16)
		{
			throw new Exception($"Failed read guid of size {16} at offset {binaryReader.BaseStream.Position}.");
		}
		Guid subGuid = new Guid(span);
		return new EventScriptId(type, guid, subGuid);
	}

	protected void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventScript script);

	protected void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventConditionList conditionList);

	protected EventInstruction DecompileInstruction(EventScriptRuntime runtime, BinaryReader binaryReader);

	protected EventCondition DecompileCondition(EventScriptRuntime runtime, BinaryReader binaryReader);
}
