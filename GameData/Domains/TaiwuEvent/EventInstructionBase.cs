using System;
using CompDevLib.Interpreter;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007B RID: 123
	public class EventInstructionBase
	{
		// Token: 0x060016C7 RID: 5831 RVA: 0x00153758 File Offset: 0x00151958
		public EventInstructionBase(int functionId, Instruction<EventScriptRuntime> instruction, int indent)
		{
			this.FunctionId = functionId;
			this.Instruction = instruction;
			this.Indent = indent;
		}

		// Token: 0x04000486 RID: 1158
		public readonly int Indent;

		// Token: 0x04000487 RID: 1159
		public readonly int FunctionId;

		// Token: 0x04000488 RID: 1160
		public readonly Instruction<EventScriptRuntime> Instruction;
	}
}
