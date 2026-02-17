using System;
using System.Runtime.CompilerServices;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x02000080 RID: 128
	public class TaiwuEventConditionException : Exception
	{
		// Token: 0x0600171D RID: 5917 RVA: 0x00155830 File Offset: 0x00153A30
		public TaiwuEventConditionException(string message, EventScriptId id, int line, EventCondition condition, Exception innerException)
		{
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(15, 4);
			defaultInterpolatedStringHandler.AppendFormatted(message);
			defaultInterpolatedStringHandler.AppendLiteral(" at ");
			defaultInterpolatedStringHandler.AppendFormatted<EventScriptId>(id);
			defaultInterpolatedStringHandler.AppendLiteral(" line ");
			defaultInterpolatedStringHandler.AppendFormatted<int>(line);
			defaultInterpolatedStringHandler.AppendLiteral(".\n\n ");
			defaultInterpolatedStringHandler.AppendFormatted(condition.Instruction.InstructionStr);
			defaultInterpolatedStringHandler.AppendLiteral("\n");
			base..ctor(defaultInterpolatedStringHandler.ToStringAndClear(), innerException);
		}

		// Token: 0x0600171E RID: 5918 RVA: 0x001558BC File Offset: 0x00153ABC
		public TaiwuEventConditionException(string message, EventScriptId id, int line, string instStr, Exception innerException)
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
	}
}
