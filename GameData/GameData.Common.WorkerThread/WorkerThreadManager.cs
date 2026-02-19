using System;
using System.Collections.Concurrent;
using System.Threading;
using GameData.GameDataBridge;
using GameData.Utilities;
using NLog;

namespace GameData.Common.WorkerThread;

public static class WorkerThreadManager
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private static readonly Thread[] Workers = new Thread[Math.Min(Environment.ProcessorCount, 32)];

	private static readonly DataContext[] Contexts = new DataContext[Math.Min(Environment.ProcessorCount, 32)];

	private static WorkerThreadTaskType _taskType;

	private static Action<DataContext, int> _workingMethod;

	private static ConcurrentQueue<int> _workIds;

	private static Semaphore _tasksCount;

	private static CountdownEvent _workingCount;

	public static void Initialize()
	{
		_workIds = new ConcurrentQueue<int>();
		_tasksCount = new Semaphore(0, Workers.Length);
		_workingCount = new CountdownEvent(0);
		CreateWorkerThreads();
	}

	public static void ReInitialize()
	{
		TerminateWorkerThreads();
		_tasksCount.Dispose();
		_workingCount.Dispose();
		Initialize();
	}

	public static void Run(Action<DataContext, int> workingMethod, int beginWorkId, int endWorkId, DataMonitorManager monitor = null, int monitorInterval = 0)
	{
		Tester.Assert(_workingMethod == null);
		Tester.Assert(_workIds.IsEmpty);
		Tester.Assert(_workingCount.CurrentCount == 0);
		AddTask(workingMethod, beginWorkId, endWorkId);
		if (monitor == null || monitorInterval <= 0)
		{
			_workingCount.Wait();
		}
		else
		{
			while (!_workingCount.Wait(monitorInterval))
			{
				monitor.CheckMonitoredData();
				GameData.GameDataBridge.GameDataBridge.TransferPendingNotifications();
			}
		}
		int i = 0;
		for (int num = Workers.Length; i < num; i++)
		{
			DataContext dataContext = Contexts[i];
			dataContext.ParallelModificationsRecorder.ApplyAll(dataContext);
		}
		_workingMethod = null;
		Tester.Assert(_workIds.IsEmpty);
		Tester.Assert(_workingCount.CurrentCount == 0);
	}

	public static void RunPostAction(Action<DataContext> postAction)
	{
		int i = 0;
		for (int num = Workers.Length; i < num; i++)
		{
			postAction(Contexts[i]);
		}
	}

	private static void CreateWorkerThreads()
	{
		int i = 0;
		for (int num = Workers.Length; i < num; i++)
		{
			Thread thread = new Thread(WorkerProc)
			{
				IsBackground = true,
				Name = $"Worker{i}",
				Priority = ThreadPriority.AboveNormal
			};
			Workers[i] = thread;
			thread.Start(i);
		}
	}

	private static void TerminateWorkerThreads()
	{
		_taskType = WorkerThreadTaskType.Exit;
		Tester.Assert(_workingMethod == null);
		Tester.Assert(_workIds.IsEmpty);
		_tasksCount.Release(Workers.Length);
		Tester.Assert(_workingCount.CurrentCount == 0);
		int i = 0;
		for (int num = Workers.Length; i < num; i++)
		{
			Workers[i].Join();
			Workers[i] = null;
			Contexts[i] = null;
		}
	}

	private static void WorkerProc(object index)
	{
		Logger.Info("Worker thread started.");
		try
		{
			DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
			Contexts[(int)index] = currentThreadDataContext;
			while (true)
			{
				_tasksCount.WaitOne();
				if (_taskType != WorkerThreadTaskType.Invoke)
				{
					break;
				}
				int result;
				while (_workIds.TryDequeue(out result))
				{
					_workingMethod(currentThreadDataContext, result);
				}
				_workingCount.Signal();
			}
			if (_taskType != WorkerThreadTaskType.Exit)
			{
				throw new Exception($"Unsupported WorkerThreadTaskType: {_taskType}");
			}
		}
		catch (Exception ex)
		{
			Logger.Error<Exception>(ex);
		}
		Logger.Info("Worker thread is about to exit.");
		LogManager.Flush();
	}

	private static void AddTask(Action<DataContext, int> workingMethod, int beginWorkId, int endWorkId)
	{
		_taskType = WorkerThreadTaskType.Invoke;
		_workingMethod = workingMethod;
		for (int i = beginWorkId; i < endWorkId; i++)
		{
			_workIds.Enqueue(i);
		}
		_workingCount.Reset(Workers.Length);
		_tasksCount.Release(Workers.Length);
	}
}
