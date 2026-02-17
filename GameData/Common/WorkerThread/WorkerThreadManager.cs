using System;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Threading;
using GameData.GameDataBridge;
using GameData.Utilities;
using NLog;

namespace GameData.Common.WorkerThread
{
	// Token: 0x020008FD RID: 2301
	public static class WorkerThreadManager
	{
		// Token: 0x06008253 RID: 33363 RVA: 0x004DA9AC File Offset: 0x004D8BAC
		public static void Initialize()
		{
			WorkerThreadManager._workIds = new ConcurrentQueue<int>();
			WorkerThreadManager._tasksCount = new Semaphore(0, WorkerThreadManager.Workers.Length);
			WorkerThreadManager._workingCount = new CountdownEvent(0);
			WorkerThreadManager.CreateWorkerThreads();
		}

		// Token: 0x06008254 RID: 33364 RVA: 0x004DA9DC File Offset: 0x004D8BDC
		public static void ReInitialize()
		{
			WorkerThreadManager.TerminateWorkerThreads();
			WorkerThreadManager._tasksCount.Dispose();
			WorkerThreadManager._workingCount.Dispose();
			WorkerThreadManager.Initialize();
		}

		// Token: 0x06008255 RID: 33365 RVA: 0x004DAA04 File Offset: 0x004D8C04
		public static void Run(Action<DataContext, int> workingMethod, int beginWorkId, int endWorkId, DataMonitorManager monitor = null, int monitorInterval = 0)
		{
			Tester.Assert(WorkerThreadManager._workingMethod == null, "");
			Tester.Assert(WorkerThreadManager._workIds.IsEmpty, "");
			Tester.Assert(WorkerThreadManager._workingCount.CurrentCount == 0, "");
			WorkerThreadManager.AddTask(workingMethod, beginWorkId, endWorkId);
			bool flag = monitor == null || monitorInterval <= 0;
			if (flag)
			{
				WorkerThreadManager._workingCount.Wait();
			}
			else
			{
				while (!WorkerThreadManager._workingCount.Wait(monitorInterval))
				{
					monitor.CheckMonitoredData();
					GameDataBridge.TransferPendingNotifications();
				}
			}
			int i = 0;
			int count = WorkerThreadManager.Workers.Length;
			while (i < count)
			{
				DataContext context = WorkerThreadManager.Contexts[i];
				context.ParallelModificationsRecorder.ApplyAll(context);
				i++;
			}
			WorkerThreadManager._workingMethod = null;
			Tester.Assert(WorkerThreadManager._workIds.IsEmpty, "");
			Tester.Assert(WorkerThreadManager._workingCount.CurrentCount == 0, "");
		}

		// Token: 0x06008256 RID: 33366 RVA: 0x004DAB08 File Offset: 0x004D8D08
		public static void RunPostAction(Action<DataContext> postAction)
		{
			int i = 0;
			int count = WorkerThreadManager.Workers.Length;
			while (i < count)
			{
				postAction(WorkerThreadManager.Contexts[i]);
				i++;
			}
		}

		// Token: 0x06008257 RID: 33367 RVA: 0x004DAB3C File Offset: 0x004D8D3C
		private static void CreateWorkerThreads()
		{
			int i = 0;
			int count = WorkerThreadManager.Workers.Length;
			while (i < count)
			{
				Thread thread = new Thread(new ParameterizedThreadStart(WorkerThreadManager.WorkerProc));
				thread.IsBackground = true;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(6, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Worker");
				defaultInterpolatedStringHandler.AppendFormatted<int>(i);
				thread.Name = defaultInterpolatedStringHandler.ToStringAndClear();
				thread.Priority = ThreadPriority.AboveNormal;
				Thread worker = thread;
				WorkerThreadManager.Workers[i] = worker;
				worker.Start(i);
				i++;
			}
		}

		// Token: 0x06008258 RID: 33368 RVA: 0x004DABCC File Offset: 0x004D8DCC
		private static void TerminateWorkerThreads()
		{
			WorkerThreadManager._taskType = WorkerThreadTaskType.Exit;
			Tester.Assert(WorkerThreadManager._workingMethod == null, "");
			Tester.Assert(WorkerThreadManager._workIds.IsEmpty, "");
			WorkerThreadManager._tasksCount.Release(WorkerThreadManager.Workers.Length);
			Tester.Assert(WorkerThreadManager._workingCount.CurrentCount == 0, "");
			int i = 0;
			int count = WorkerThreadManager.Workers.Length;
			while (i < count)
			{
				WorkerThreadManager.Workers[i].Join();
				WorkerThreadManager.Workers[i] = null;
				WorkerThreadManager.Contexts[i] = null;
				i++;
			}
		}

		// Token: 0x06008259 RID: 33369 RVA: 0x004DAC6C File Offset: 0x004D8E6C
		private static void WorkerProc(object index)
		{
			WorkerThreadManager.Logger.Info("Worker thread started.");
			try
			{
				DataContext context = DataContextManager.GetCurrentThreadDataContext();
				WorkerThreadManager.Contexts[(int)index] = context;
				for (;;)
				{
					WorkerThreadManager._tasksCount.WaitOne();
					bool flag = WorkerThreadManager._taskType == WorkerThreadTaskType.Invoke;
					if (!flag)
					{
						break;
					}
					for (;;)
					{
						int workId;
						bool flag2 = WorkerThreadManager._workIds.TryDequeue(out workId);
						if (!flag2)
						{
							break;
						}
						WorkerThreadManager._workingMethod(context, workId);
					}
					WorkerThreadManager._workingCount.Signal();
				}
				bool flag3 = WorkerThreadManager._taskType == WorkerThreadTaskType.Exit;
				if (!flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unsupported WorkerThreadTaskType: ");
					defaultInterpolatedStringHandler.AppendFormatted<WorkerThreadTaskType>(WorkerThreadManager._taskType);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
			catch (Exception ex)
			{
				WorkerThreadManager.Logger.Error<Exception>(ex);
			}
			WorkerThreadManager.Logger.Info("Worker thread is about to exit.");
			LogManager.Flush();
		}

		// Token: 0x0600825A RID: 33370 RVA: 0x004DAD70 File Offset: 0x004D8F70
		private static void AddTask(Action<DataContext, int> workingMethod, int beginWorkId, int endWorkId)
		{
			WorkerThreadManager._taskType = WorkerThreadTaskType.Invoke;
			WorkerThreadManager._workingMethod = workingMethod;
			for (int i = beginWorkId; i < endWorkId; i++)
			{
				WorkerThreadManager._workIds.Enqueue(i);
			}
			WorkerThreadManager._workingCount.Reset(WorkerThreadManager.Workers.Length);
			WorkerThreadManager._tasksCount.Release(WorkerThreadManager.Workers.Length);
		}

		// Token: 0x04002469 RID: 9321
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400246A RID: 9322
		private static readonly Thread[] Workers = new Thread[Math.Min(Environment.ProcessorCount, 32)];

		// Token: 0x0400246B RID: 9323
		private static readonly DataContext[] Contexts = new DataContext[Math.Min(Environment.ProcessorCount, 32)];

		// Token: 0x0400246C RID: 9324
		private static WorkerThreadTaskType _taskType;

		// Token: 0x0400246D RID: 9325
		private static Action<DataContext, int> _workingMethod;

		// Token: 0x0400246E RID: 9326
		private static ConcurrentQueue<int> _workIds;

		// Token: 0x0400246F RID: 9327
		private static Semaphore _tasksCount;

		// Token: 0x04002470 RID: 9328
		private static CountdownEvent _workingCount;
	}
}
