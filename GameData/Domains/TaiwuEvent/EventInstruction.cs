using System;
using CompDevLib.Interpreter;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000077 RID: 119
	public class EventInstruction : EventInstructionBase
	{
		// Token: 0x060016C3 RID: 5827 RVA: 0x00153728 File Offset: 0x00151928
		public EventInstruction(int functionId, int indentAmount, string assignToVar, Instruction<EventScriptRuntime> instruction) : base(functionId, instruction, indentAmount)
		{
			this.AssignToVar = assignToVar;
		}

		// Token: 0x04000480 RID: 1152
		public readonly string AssignToVar;
	}
}
