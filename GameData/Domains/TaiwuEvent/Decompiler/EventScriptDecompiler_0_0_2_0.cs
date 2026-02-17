using System;
using System.Collections.Generic;
using System.IO;

namespace GameData.Domains.TaiwuEvent.Decompiler
{
	// Token: 0x020000D6 RID: 214
	public class EventScriptDecompiler_0_0_2_0 : IEventScriptDecompiler
	{
		// Token: 0x170000D7 RID: 215
		// (get) Token: 0x060025C1 RID: 9665 RVA: 0x001CD2ED File Offset: 0x001CB4ED
		public string Version
		{
			get
			{
				return "0.0.2.0";
			}
		}

		// Token: 0x060025C2 RID: 9666 RVA: 0x001CD2F4 File Offset: 0x001CB4F4
		public void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventScript script)
		{
			script.Labels = new Dictionary<string, int>();
			int count = binaryReader.ReadInt32();
			for (int i = 0; i < count; i++)
			{
				string label = binaryReader.ReadString();
				int index = binaryReader.ReadInt32();
				script.Labels.Add(label, index);
			}
		}

		// Token: 0x060025C3 RID: 9667 RVA: 0x001CD344 File Offset: 0x001CB544
		public void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventConditionList conditionList)
		{
		}

		// Token: 0x060025C4 RID: 9668 RVA: 0x001CD348 File Offset: 0x001CB548
		public EventInstruction DecompileInstruction(EventScriptRuntime runtime, BinaryReader binaryReader)
		{
			int indent = binaryReader.ReadInt32();
			string assignToVar = binaryReader.ReadString();
			int funcId = binaryReader.ReadInt32();
			int argCount = binaryReader.ReadInt32();
			string[] args = (argCount > 0) ? new string[argCount] : Array.Empty<string>();
			for (int i = 0; i < argCount; i++)
			{
				args[i] = binaryReader.ReadString();
			}
			return runtime.CreateInst(funcId, indent, assignToVar, args);
		}

		// Token: 0x060025C5 RID: 9669 RVA: 0x001CD3B8 File Offset: 0x001CB5B8
		public EventCondition DecompileCondition(EventScriptRuntime runtime, BinaryReader binaryReader)
		{
			int indent = binaryReader.ReadInt32();
			bool reverse = binaryReader.ReadBoolean();
			int funcId = binaryReader.ReadInt32();
			int argCount = binaryReader.ReadInt32();
			string[] args = (argCount > 0) ? new string[argCount] : Array.Empty<string>();
			for (int i = 0; i < argCount; i++)
			{
				args[i] = binaryReader.ReadString();
			}
			return runtime.CreateCondition(funcId, indent, reverse, args);
		}
	}
}
