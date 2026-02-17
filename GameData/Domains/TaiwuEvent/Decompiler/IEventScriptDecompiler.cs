using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;

namespace GameData.Domains.TaiwuEvent.Decompiler
{
	// Token: 0x020000D7 RID: 215
	public interface IEventScriptDecompiler
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060025C7 RID: 9671
		string Version { get; }

		// Token: 0x060025C8 RID: 9672 RVA: 0x001CD430 File Offset: 0x001CB630
		Dictionary<EventScriptId, EventScriptBase> DecompileEventScriptPackage(EventScriptRuntime runtime, BinaryReader binaryReader)
		{
			Dictionary<EventScriptId, EventScriptBase> scripts = new Dictionary<EventScriptId, EventScriptBase>();
			while (binaryReader.BaseStream.Position < binaryReader.BaseStream.Length)
			{
				EventScriptId id = this.DecompileScriptId(runtime, binaryReader);
				EventScriptBase script = EventScriptId.IsConditionList(id.Type) ? this.DecompileConditionList(runtime, id, binaryReader) : this.DecompileEventScript(runtime, id, binaryReader);
				scripts.Add(script.Id, script);
			}
			return scripts;
		}

		// Token: 0x060025C9 RID: 9673 RVA: 0x001CD4A4 File Offset: 0x001CB6A4
		EventConditionList DecompileConditionList(EventScriptRuntime runtime, EventScriptId id, BinaryReader binaryReader)
		{
			EventConditionList script = new EventConditionList();
			script.Id = id;
			this.DecompileMetaData(runtime, binaryReader, script);
			int instCount = binaryReader.ReadInt32();
			script.Conditions = new EventCondition[instCount];
			for (int i = 0; i < instCount; i++)
			{
				try
				{
					script.Conditions[i] = this.DecompileCondition(runtime, binaryReader);
				}
				catch (Exception e)
				{
					throw new TaiwuEventScriptException("Failed to decompile instruction.", script.Id, i, e);
				}
			}
			return script;
		}

		// Token: 0x060025CA RID: 9674 RVA: 0x001CD530 File Offset: 0x001CB730
		EventScript DecompileEventScript(EventScriptRuntime runtime, EventScriptId id, BinaryReader binaryReader)
		{
			EventScript script = new EventScript();
			script.Id = id;
			this.DecompileMetaData(runtime, binaryReader, script);
			int instCount = binaryReader.ReadInt32();
			script.Instructions = new EventInstruction[instCount];
			for (int i = 0; i < instCount; i++)
			{
				try
				{
					script.Instructions[i] = this.DecompileInstruction(runtime, binaryReader);
				}
				catch (Exception e)
				{
					throw new TaiwuEventScriptException("Failed to decompile condition.", script.Id, i, e);
				}
			}
			return script;
		}

		// Token: 0x060025CB RID: 9675 RVA: 0x001CD5BC File Offset: 0x001CB7BC
		unsafe EventScriptId DecompileScriptId(EventScriptRuntime runtime, BinaryReader binaryReader)
		{
			sbyte type = binaryReader.ReadSByte();
			Span<byte> span = new Span<byte>(stackalloc byte[(UIntPtr)16], 16);
			Span<byte> buffer = span;
			bool flag = binaryReader.Read(buffer) < 16;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed read guid of size ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(16);
				defaultInterpolatedStringHandler.AppendLiteral(" at offset ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(binaryReader.BaseStream.Position);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Guid guid = new Guid(buffer);
			bool flag2 = !EventScriptId.IsOptionType(type);
			EventScriptId result;
			if (flag2)
			{
				result = new EventScriptId(type, guid);
			}
			else
			{
				bool flag3 = binaryReader.Read(buffer) < 16;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(37, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Failed read guid of size ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(16);
					defaultInterpolatedStringHandler.AppendLiteral(" at offset ");
					defaultInterpolatedStringHandler.AppendFormatted<long>(binaryReader.BaseStream.Position);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				Guid subGuid = new Guid(buffer);
				result = new EventScriptId(type, guid, subGuid);
			}
			return result;
		}

		// Token: 0x060025CC RID: 9676
		void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventScript script);

		// Token: 0x060025CD RID: 9677
		void DecompileMetaData(EventScriptRuntime runtime, BinaryReader binaryReader, EventConditionList conditionList);

		// Token: 0x060025CE RID: 9678
		EventInstruction DecompileInstruction(EventScriptRuntime runtime, BinaryReader binaryReader);

		// Token: 0x060025CF RID: 9679
		EventCondition DecompileCondition(EventScriptRuntime runtime, BinaryReader binaryReader);
	}
}
