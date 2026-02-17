using System;
using System.Threading;

namespace GameData.Common
{
	// Token: 0x020008F1 RID: 2289
	public static class DataContextManager
	{
		// Token: 0x0600821D RID: 33309 RVA: 0x004D9AE8 File Offset: 0x004D7CE8
		public static DataContext GetCurrentThreadDataContext()
		{
			return DataContextManager.Context.Value;
		}

		// Token: 0x0600821E RID: 33310 RVA: 0x004D9B04 File Offset: 0x004D7D04
		public static void RegisterMainThread()
		{
			DataContextManager._mainThreadId = Thread.CurrentThread.ManagedThreadId;
		}

		// Token: 0x0600821F RID: 33311 RVA: 0x004D9B18 File Offset: 0x004D7D18
		public static bool IsMainThread(int threadId)
		{
			return threadId == DataContextManager._mainThreadId;
		}

		// Token: 0x04002426 RID: 9254
		private static readonly ThreadLocal<DataContext> Context = new ThreadLocal<DataContext>(() => new DataContext(Thread.CurrentThread.ManagedThreadId), true);

		// Token: 0x04002427 RID: 9255
		private static int _mainThreadId = -1;
	}
}
