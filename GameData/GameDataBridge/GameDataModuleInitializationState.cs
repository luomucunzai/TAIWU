using System;

namespace GameData.GameDataBridge
{
	// Token: 0x02000020 RID: 32
	public static class GameDataModuleInitializationState
	{
		// Token: 0x06000D16 RID: 3350 RVA: 0x000DE228 File Offset: 0x000DC428
		public static bool CheckTransition(sbyte prevState, sbyte nextState)
		{
			bool flag = nextState == 1;
			bool result;
			if (flag)
			{
				bool flag2 = prevState == 0 || prevState == 3;
				result = flag2;
			}
			else
			{
				result = (nextState == prevState + 1);
			}
			return result;
		}

		// Token: 0x040000A4 RID: 164
		public const sbyte Uninitialized = 0;

		// Token: 0x040000A5 RID: 165
		public const sbyte ShouldInitialize = 1;

		// Token: 0x040000A6 RID: 166
		public const sbyte ShouldSendInitializedMessage = 2;

		// Token: 0x040000A7 RID: 167
		public const sbyte SentInitializedMessage = 3;
	}
}
