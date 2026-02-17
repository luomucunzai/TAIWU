using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007F RID: 127
	public class TaiwuEventScriptException : Exception
	{
		// Token: 0x0600171A RID: 5914 RVA: 0x001556B4 File Offset: 0x001538B4
		public TaiwuEventScriptException(string message, EventScriptId id, int line, EventInstruction instruction, Exception innerException)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 4);
			defaultInterpolatedStringHandler.AppendFormatted(message);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<EventScriptId>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" line ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(line);
			defaultInterpolatedStringHandler.AppendLiteral(".\n\n ");
			defaultInterpolatedStringHandler.AppendFormatted(instruction.Instruction.InstructionStr);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
		}

		// Token: 0x0600171B RID: 5915 RVA: 0x00155740 File Offset: 0x00153940
		public TaiwuEventScriptException(string message, EventScriptId id, int line, string instStr, Exception innerException)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 4);
			defaultInterpolatedStringHandler.AppendFormatted(message);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<EventScriptId>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" line ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(line);
			defaultInterpolatedStringHandler.AppendLiteral(".\n\n ");
			defaultInterpolatedStringHandler.AppendFormatted(instStr);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
		}

		// Token: 0x0600171C RID: 5916 RVA: 0x001557C4 File Offset: 0x001539C4
		public TaiwuEventScriptException(string message, EventScriptId id, int line, Exception innerException)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 3);
			defaultInterpolatedStringHandler.AppendFormatted(message);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<EventScriptId>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" line ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(line);
			defaultInterpolatedStringHandler.AppendLiteral(".\n\n");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
		}
	}
}
