using System;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000846 RID: 2118
	public static class AdvanceMonthStatus
	{
		// Token: 0x0400207C RID: 8316
		public const byte ForbiddenToExecuteGeneralAction = 1;

		// Token: 0x0400207D RID: 8317
		public const byte ForbiddenToExecutePrioritizedAction = 2;

		// Token: 0x0400207E RID: 8318
		public const byte ForbiddenToExecuteFixedAction = 4;

		// Token: 0x0400207F RID: 8319
		public const byte CannotBeCalledByAdventure = 16;

		// Token: 0x04002080 RID: 8320
		public const byte ForbiddenToExecuteAction = 7;
	}
}
