using System;
using CompDevLib.Interpreter;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000074 RID: 116
	public class EventCondition : EventInstructionBase
	{
		// Token: 0x060016BF RID: 5823 RVA: 0x001536E0 File Offset: 0x001518E0
		public EventCondition(int functionId, Instruction<EventScriptRuntime> instruction, bool reverse, int indent) : base(functionId, instruction, indent)
		{
			this.Reverse = reverse;
		}

		// Token: 0x060016C0 RID: 5824 RVA: 0x001536F5 File Offset: 0x001518F5
		public bool Check(EventScriptRuntime runtime)
		{
			return this.Reverse != this.Instruction.Execute<bool>(runtime);
		}

		// Token: 0x0400047D RID: 1149
		public readonly bool Reverse;
	}
}
