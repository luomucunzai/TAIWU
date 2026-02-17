using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Threading;
using Config.Common;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Dependencies;
using GameData.DLC;
using GameData.DomainEvents;
using GameData.Domains;
using GameData.Domains.Mod;
using GameData.GameDataBridge.VnPipe;
using GameData.Serializer;
using GameData.Steamworks;
using GameData.Utilities;
using NLog;

namespace GameData.GameDataBridge
{
	// Token: 0x0200001F RID: 31
	public static class GameDataBridge
	{
		// Token: 0x06000CE0 RID: 3296 RVA: 0x000DC5F0 File Offset: 0x000DA7F0
		public static void RunMainLoop()
		{
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			GameDataBridge._shouldDisconnect = false;
			GameDataBridge.Stopwatch.Start();
			GameDataBridge.Logger.Info("Machine Frequency (Ticks/Second): " + Stopwatch.Frequency.ToString());
			GameDataBridge._frameBeginTicks = GameDataBridge.Stopwatch.ElapsedTicks;
			GameDataBridge.timeBeginPeriod(1U);
			while (!GameDataBridge._shouldDisconnect)
			{
				bool flag = !GameDataBridge.CheckErrorMessages();
				if (!flag)
				{
					bool flag2 = !Common.CheckAsyncError();
					if (!flag2)
					{
						ArchiveDataManager.GetResponse();
						ArchiveDataManager.SendRequest();
						bool globalDataLoaded = DomainManager.Global.GlobalDataLoaded;
						if (globalDataLoaded)
						{
							break;
						}
						GameDataBridge.AdvanceFrame();
						continue;
					}
				}
				return;
			}
			while (!GameDataBridge._shouldDisconnect)
			{
				bool flag3 = !GameDataBridge.CheckErrorMessages();
				if (flag3)
				{
					break;
				}
				bool flag4 = GameDataBridge._gameDataModuleInitializationState == 1;
				if (flag4)
				{
					GameDataBridge.InitializeGameDataModule();
				}
				SteamManager.Update();
				bool flag5 = GameDataBridge.IsGameDataModuleInitialized();
				if (flag5)
				{
					bool flag6 = !Common.CheckAsyncError();
					if (flag6)
					{
						break;
					}
					ArchiveDataManager.GetResponse();
					GameDataBridge.ProcessOperations(context);
					for (int i = 0; i < DomainManager.Domains.Length; i++)
					{
						DomainManager.Domains[i].OnUpdate(context);
					}
					context.ParallelModificationsRecorder.ApplyAll(context);
					GameDataBridge.DataMonitorManager.CheckMonitoredData();
					GameDataBridge.TransferPendingNotifications();
					Events.RaiseBeforeSendRequestToArchiveModule(context);
					ArchiveDataManager.SendRequest();
				}
				GameDataBridge.AdvanceFrame();
			}
			GameDataBridge.timeEndPeriod(1U);
		}

		// Token: 0x06000CE1 RID: 3297 RVA: 0x000DC77C File Offset: 0x000DA97C
		private static void AdvanceFrame()
		{
			long currElapsedTicks = GameDataBridge.Stopwatch.ElapsedTicks - GameDataBridge._frameBeginTicks;
			long expectedSleepTicks = GameDataBridge.ExpectedTicksPerFrame - currElapsedTicks;
			bool flag = GameDataBridge._averageSleepInterval == 0L;
			if (flag)
			{
				GameDataBridge._averageSleepInterval = Stopwatch.Frequency / 1000L;
			}
			long expectedSleepCount = expectedSleepTicks / GameDataBridge._averageSleepInterval * 8L / 10L;
			bool flag2 = expectedSleepCount == 0L;
			if (flag2)
			{
				expectedSleepCount += 1L;
			}
			long sleepBeginTicks = GameDataBridge.Stopwatch.ElapsedTicks;
			int sleepCount = 0;
			while ((long)sleepCount < expectedSleepCount && currElapsedTicks < GameDataBridge.ExpectedTicksPerFrame)
			{
				Thread.Sleep(1);
				currElapsedTicks = GameDataBridge.Stopwatch.ElapsedTicks - GameDataBridge._frameBeginTicks;
				sleepCount++;
			}
			bool flag3 = sleepCount > 0;
			if (flag3)
			{
				GameDataBridge._averageSleepInterval = (GameDataBridge.Stopwatch.ElapsedTicks - sleepBeginTicks) / (long)sleepCount;
			}
			for (currElapsedTicks = GameDataBridge.Stopwatch.ElapsedTicks - GameDataBridge._frameBeginTicks; currElapsedTicks < GameDataBridge.ExpectedTicksPerFrame; currElapsedTicks = GameDataBridge.Stopwatch.ElapsedTicks - GameDataBridge._frameBeginTicks)
			{
				Thread.Sleep(0);
			}
			GameDataBridge.ActualFramesPerSecond = (int)(Stopwatch.Frequency / currElapsedTicks + 1L);
			GameDataBridge._frameBeginTicks = GameDataBridge.Stopwatch.ElapsedTicks;
		}

		// Token: 0x06000CE2 RID: 3298 RVA: 0x000DC8A4 File Offset: 0x000DAAA4
		private static void InitializeGameDataModule()
		{
			List<IConfigData> cachedConfigs = GameDataBridge.CachedConfigs;
			lock (cachedConfigs)
			{
				DomainManager.Mod.LoadAllMods(GameDataBridge._modInfos);
				GameDataBridge._modInfos.Items.Clear();
			}
			DataDependenciesInfo.Generate();
			foreach (BaseGameDataDomain domain in DomainManager.Domains)
			{
				domain.OnInitializeGameDataModule();
			}
			GameDataBridge.SetGameDataModuleInitializationState(2);
		}

		// Token: 0x06000CE3 RID: 3299 RVA: 0x000DC934 File Offset: 0x000DAB34
		private static void ProcessOperations(DataContext context)
		{
			object operationCollectionsLock = GameDataBridge.OperationCollectionsLock;
			lock (operationCollectionsLock)
			{
				bool flag2 = GameDataBridge._operationCollections.Count <= 0;
				if (flag2)
				{
					return;
				}
				List<OperationCollection> operationCollections = GameDataBridge._operationCollections;
				List<OperationCollection> processingOperationCollections = GameDataBridge._processingOperationCollections;
				GameDataBridge._processingOperationCollections = operationCollections;
				GameDataBridge._operationCollections = processingOperationCollections;
				GameDataBridge._operationCollections.Clear();
			}
			int collectionsCount = GameDataBridge._processingOperationCollections.Count;
			for (int i = 0; i < collectionsCount; i++)
			{
				OperationCollection collection = GameDataBridge._processingOperationCollections[i];
				RawDataPool dataPool = collection.DataPool;
				int operationsCount = collection.Operations.Count;
				for (int j = 0; j < operationsCount; j++)
				{
					Operation operation = collection.Operations[j];
					switch (operation.Type)
					{
					case 0:
						GameDataBridge.ProcessDataMonitor(operation);
						break;
					case 1:
						GameDataBridge.ProcessDataUnMonitor(operation);
						break;
					case 2:
						GameDataBridge.ProcessDataModification(operation, dataPool, context);
						break;
					case 3:
						GameDataBridge.ProcessMethodCall(operation, dataPool, context);
						break;
					default:
					{
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Unsupported operation type: ");
						defaultInterpolatedStringHandler.AppendFormatted<byte>(operation.Type);
						throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
					}
					}
				}
			}
			GameDataBridge._processingOperationCollections.Clear();
		}

		// Token: 0x06000CE4 RID: 3300 RVA: 0x000DCAC0 File Offset: 0x000DACC0
		private static void ProcessDataMonitor(Operation operation)
		{
			DataUid uid = new DataUid(operation.DomainId, operation.DataId, operation.SubId0, operation.SubId1);
			GameDataBridge.DataMonitorManager.MonitorData(uid);
		}

		// Token: 0x06000CE5 RID: 3301 RVA: 0x000DCAFC File Offset: 0x000DACFC
		private static void ProcessDataUnMonitor(Operation operation)
		{
			DataUid uid = new DataUid(operation.DomainId, operation.DataId, operation.SubId0, operation.SubId1);
			GameDataBridge.DataMonitorManager.UnMonitorData(uid);
		}

		// Token: 0x06000CE6 RID: 3302 RVA: 0x000DCB38 File Offset: 0x000DAD38
		private static void ProcessDataModification(Operation operation, RawDataPool dataPool, DataContext context)
		{
			BaseGameDataDomain domain = DomainManager.Domains[(int)operation.DomainId];
			domain.SetData(operation.DataId, operation.SubId0, operation.SubId1, operation.ValueOffset, dataPool, context);
		}

		// Token: 0x06000CE7 RID: 3303 RVA: 0x000DCB74 File Offset: 0x000DAD74
		private static void ProcessMethodCall(Operation operation, RawDataPool argDataPool, DataContext context)
		{
			BaseGameDataDomain domain = DomainManager.Domains[(int)operation.DomainId];
			int returnOffset = domain.CallMethod(operation, argDataPool, GameDataBridge._pendingNotifications.DataPool, context);
			bool flag = returnOffset >= 0;
			if (flag)
			{
				GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, returnOffset));
			}
		}

		// Token: 0x06000CE8 RID: 3304 RVA: 0x000DCBD8 File Offset: 0x000DADD8
		public static NotificationCollection GetPendingNotifications()
		{
			return GameDataBridge._pendingNotifications;
		}

		// Token: 0x06000CE9 RID: 3305 RVA: 0x000DCBF0 File Offset: 0x000DADF0
		public static void TransferPendingNotifications()
		{
			bool flag = GameDataBridge._pendingNotifications.Notifications.Count <= 0;
			if (!flag)
			{
				object notificationCollectionsLock = GameDataBridge.NotificationCollectionsLock;
				lock (notificationCollectionsLock)
				{
					GameDataBridge._notificationCollections.Add(GameDataBridge._pendingNotifications);
					GameDataBridge._pendingNotifications = new NotificationCollection(65536);
				}
			}
		}

		// Token: 0x06000CEA RID: 3306 RVA: 0x000DCC68 File Offset: 0x000DAE68
		public static ValueTuple<DataMonitorManager, NotificationCollection> StartSemiBlockingTask()
		{
			DataMonitorManager monitor = new DataMonitorManager();
			NotificationCollection oriPendingNotifications = GameDataBridge._pendingNotifications;
			GameDataBridge._pendingNotifications = new NotificationCollection(65536);
			return new ValueTuple<DataMonitorManager, NotificationCollection>(monitor, oriPendingNotifications);
		}

		// Token: 0x06000CEB RID: 3307 RVA: 0x000DCC9C File Offset: 0x000DAE9C
		public static void StopSemiBlockingTask(DataMonitorManager monitor, NotificationCollection oriPendingNotifications)
		{
			monitor.CheckMonitoredData();
			monitor.Clear();
			GameDataBridge.TransferPendingNotifications();
			GameDataBridge._pendingNotifications = oriPendingNotifications;
		}

		// Token: 0x06000CEC RID: 3308 RVA: 0x000DCCBC File Offset: 0x000DAEBC
		public static void AppendErrorMessage(string message)
		{
			List<string> errorMessages = GameDataBridge.ErrorMessages;
			lock (errorMessages)
			{
				GameDataBridge.ErrorMessages.Add(DomainManager.Global.GetGameVersion() + " " + message);
			}
		}

		// Token: 0x06000CED RID: 3309 RVA: 0x000DCD1C File Offset: 0x000DAF1C
		public static void AppendWarningMessage(string message)
		{
			List<string> warningMessages = GameDataBridge.WarningMessages;
			lock (warningMessages)
			{
				GameDataBridge.WarningMessages.Add(message);
			}
		}

		// Token: 0x06000CEE RID: 3310 RVA: 0x000DCD68 File Offset: 0x000DAF68
		public static void ClearMonitoredData()
		{
			GameDataBridge.DataMonitorManager.Clear();
		}

		// Token: 0x06000CEF RID: 3311 RVA: 0x000DCD78 File Offset: 0x000DAF78
		private static bool CheckErrorMessages()
		{
			List<string> errorMessages = GameDataBridge.ErrorMessages;
			bool result;
			lock (errorMessages)
			{
				result = (GameDataBridge.ErrorMessages.Count <= 0);
			}
			return result;
		}

		// Token: 0x06000CF0 RID: 3312 RVA: 0x000DCDC8 File Offset: 0x000DAFC8
		private static bool CheckWarningMessages()
		{
			List<string> warningMessages = GameDataBridge.WarningMessages;
			bool result;
			lock (warningMessages)
			{
				result = (GameDataBridge.WarningMessages.Count <= 0);
			}
			return result;
		}

		// Token: 0x06000CF1 RID: 3313 RVA: 0x000DCE18 File Offset: 0x000DB018
		public static uint RecordPassthroughMethod(Operation displayOperation)
		{
			uint archiveOperationId = ArchiveDataManager.GetNextOperationId();
			GameDataBridge.PassthroughOperations.Add(archiveOperationId, displayOperation);
			return archiveOperationId;
		}

		// Token: 0x06000CF2 RID: 3314 RVA: 0x000DCE40 File Offset: 0x000DB040
		public static void TryReturnPassthroughMethod<T>(uint archiveOperationId, T returnValue)
		{
			Operation displayOperation;
			bool flag = GameDataBridge.PassthroughOperations.TryGetValue(archiveOperationId, out displayOperation);
			if (flag)
			{
				int returnOffset = SerializerHolder<T>.Serialize(returnValue, GameDataBridge._pendingNotifications.DataPool);
				GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateMethodReturn(displayOperation.ListenerId, displayOperation.DomainId, displayOperation.MethodId, returnOffset));
				GameDataBridge.PassthroughOperations.Remove(archiveOperationId);
			}
		}

		// Token: 0x06000CF3 RID: 3315 RVA: 0x000DCEA8 File Offset: 0x000DB0A8
		private static void SetGameDataModuleInitializationState(sbyte state)
		{
			bool flag = !GameDataModuleInitializationState.CheckTransition(GameDataBridge._gameDataModuleInitializationState, state);
			DefaultInterpolatedStringHandler defaultInterpolatedStringHandler;
			if (flag)
			{
				defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(24, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid transition: ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(GameDataBridge._gameDataModuleInitializationState);
				defaultInterpolatedStringHandler.AppendLiteral(" -> ");
				defaultInterpolatedStringHandler.AppendFormatted<sbyte>(state);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			Logger logger = GameDataBridge.Logger;
			defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(16, 2);
			defaultInterpolatedStringHandler.AppendLiteral("Transition: ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(GameDataBridge._gameDataModuleInitializationState);
			defaultInterpolatedStringHandler.AppendLiteral(" -> ");
			defaultInterpolatedStringHandler.AppendFormatted<sbyte>(state);
			logger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			GameDataBridge._gameDataModuleInitializationState = state;
		}

		// Token: 0x06000CF4 RID: 3316 RVA: 0x000DCF6C File Offset: 0x000DB16C
		public static bool IsGameDataModuleInitialized()
		{
			sbyte gameDataModuleInitializationState = GameDataBridge._gameDataModuleInitializationState;
			return gameDataModuleInitializationState - 2 <= 1;
		}

		// Token: 0x06000CF5 RID: 3317 RVA: 0x000DCF95 File Offset: 0x000DB195
		public static void AddPostDataModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
		{
			GameDataBridge.DataMonitorManager.AddPostModificationHandler(uid, handlerKey, handler);
		}

		// Token: 0x06000CF6 RID: 3318 RVA: 0x000DCFA6 File Offset: 0x000DB1A6
		public static void RemovePostDataModificationHandler(DataUid uid, string handlerKey)
		{
			GameDataBridge.DataMonitorManager.RemovePostModificationHandler(uid, handlerKey);
		}

		// Token: 0x06000CF7 RID: 3319
		[DllImport("winmm.dll")]
		internal static extern uint timeBeginPeriod(uint period);

		// Token: 0x06000CF8 RID: 3320
		[DllImport("winmm.dll")]
		internal static extern uint timeEndPeriod(uint period);

		// Token: 0x06000CF9 RID: 3321 RVA: 0x000DCFB6 File Offset: 0x000DB1B6
		public static void AddDisplayEvent(DisplayEventType type)
		{
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, -1));
		}

		// Token: 0x06000CFA RID: 3322 RVA: 0x000DCFD0 File Offset: 0x000DB1D0
		public static void AddDisplayEvent<T1>(DisplayEventType type, T1 arg1)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000CFB RID: 3323 RVA: 0x000DD008 File Offset: 0x000DB208
		public static void AddDisplayEvent<T1, T2>(DisplayEventType type, T1 arg1, T2 arg2)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000CFC RID: 3324 RVA: 0x000DD048 File Offset: 0x000DB248
		public static void AddDisplayEvent<T1, T2, T3>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000CFD RID: 3325 RVA: 0x000DD090 File Offset: 0x000DB290
		public static void AddDisplayEvent<T1, T2, T3, T4>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			SerializerHolder<T4>.Serialize(arg4, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000CFE RID: 3326 RVA: 0x000DD0E4 File Offset: 0x000DB2E4
		public static void AddDisplayEvent<T1, T2, T3, T4, T5>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			SerializerHolder<T4>.Serialize(arg4, dataPool);
			SerializerHolder<T5>.Serialize(arg5, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000CFF RID: 3327 RVA: 0x000DD140 File Offset: 0x000DB340
		public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			SerializerHolder<T4>.Serialize(arg4, dataPool);
			SerializerHolder<T5>.Serialize(arg5, dataPool);
			SerializerHolder<T6>.Serialize(arg6, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000D00 RID: 3328 RVA: 0x000DD1A4 File Offset: 0x000DB3A4
		public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6, T7>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			SerializerHolder<T4>.Serialize(arg4, dataPool);
			SerializerHolder<T5>.Serialize(arg5, dataPool);
			SerializerHolder<T6>.Serialize(arg6, dataPool);
			SerializerHolder<T7>.Serialize(arg7, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000D01 RID: 3329 RVA: 0x000DD210 File Offset: 0x000DB410
		public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6, T7, T8>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
		{
			RawDataPool dataPool = GameDataBridge._pendingNotifications.DataPool;
			int offset = SerializerHolder<T1>.Serialize(arg1, dataPool);
			SerializerHolder<T2>.Serialize(arg2, dataPool);
			SerializerHolder<T3>.Serialize(arg3, dataPool);
			SerializerHolder<T4>.Serialize(arg4, dataPool);
			SerializerHolder<T5>.Serialize(arg5, dataPool);
			SerializerHolder<T6>.Serialize(arg6, dataPool);
			SerializerHolder<T7>.Serialize(arg7, dataPool);
			SerializerHolder<T8>.Serialize(arg8, dataPool);
			GameDataBridge._pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, offset));
		}

		// Token: 0x06000D02 RID: 3330 RVA: 0x000DD288 File Offset: 0x000DB488
		public static void Initialize()
		{
			GameDataBridge._gameDataModuleInitializationState = 0;
			GameDataBridge._slaverPipe = Slaver.Connect("taiwu");
			bool flag = GameDataBridge._slaverPipe == null;
			if (flag)
			{
				throw new Exception("pipe connect failed");
			}
			GameDataBridge._readingThread = new Thread(new ThreadStart(GameDataBridge.ReadInterProcessMessages))
			{
				IsBackground = false,
				Name = "ReadInterProcessMessages"
			};
			GameDataBridge._readingThread.Start();
			GameDataBridge._writingThread = new Thread(new ThreadStart(GameDataBridge.WriteInterProcessMessages))
			{
				IsBackground = false,
				Name = "WriteInterProcessMessages"
			};
			GameDataBridge._writingThread.Start();
		}

		// Token: 0x06000D03 RID: 3331 RVA: 0x000DD330 File Offset: 0x000DB530
		public static void UnInitialize()
		{
			GameDataBridge._shouldDisconnect = true;
			bool flag = GameDataBridge._readingThread != null;
			if (flag)
			{
				bool flag2 = GameDataBridge._readingThread.ThreadState != ThreadState.Unstarted && !GameDataBridge._readingThread.Join(1000);
				if (flag2)
				{
					GameDataBridge.Logger.Warn("Failed to wait for _readingThread to terminate.");
				}
				GameDataBridge._readingThread = null;
			}
			bool flag3 = GameDataBridge._writingThread != null;
			if (flag3)
			{
				bool flag4 = GameDataBridge._writingThread.ThreadState != ThreadState.Unstarted && !GameDataBridge._writingThread.Join(1000);
				if (flag4)
				{
					GameDataBridge.Logger.Warn("Failed to wait for _writingThread to terminate.");
				}
				GameDataBridge._writingThread = null;
			}
			bool flag5 = GameDataBridge._slaverPipe != null;
			if (flag5)
			{
				GameDataBridge._slaverPipe.Dispose();
				GameDataBridge._slaverPipe = null;
			}
		}

		// Token: 0x06000D04 RID: 3332 RVA: 0x000DD3FC File Offset: 0x000DB5FC
		private static void ReadInterProcessMessages()
		{
			GameDataBridge.Logger.Info("ReadInterProcessMessages thread started.");
			try
			{
				bool shouldExit = false;
				while (!shouldExit)
				{
					shouldExit = GameDataBridge.ReadInterProcessMessage();
				}
			}
			catch (Exception ex)
			{
				GameDataBridge.Logger.Error<Exception>(ex);
			}
			GameDataBridge.Logger.Info("ReadInterProcessMessages thread is about to exit.");
			LogManager.Flush();
			GameDataBridge._shouldDisconnect = true;
		}

		// Token: 0x06000D05 RID: 3333 RVA: 0x000DD474 File Offset: 0x000DB674
		private static bool ReadInterProcessMessage()
		{
			byte messageType;
			int contentLength;
			GameDataBridge.GetUnmanagedValuesFromSocket<byte, int>(out messageType, out contentLength);
			bool result;
			switch (messageType)
			{
			case 0:
				result = GameDataBridge.ReadInterProcessMessageInitialize(contentLength);
				break;
			case 1:
				result = GameDataBridge.ReadInterProcessMessageOperations(contentLength);
				break;
			case 2:
				result = GameDataBridge.ReadInterProcessMessageDisconnect(contentLength);
				break;
			default:
				throw new Exception("Unknown message type: " + messageType.ToString());
			}
			return result;
		}

		// Token: 0x06000D06 RID: 3334 RVA: 0x000DD4DC File Offset: 0x000DB6DC
		private static bool ReadInterProcessMessageInitialize(int contentLength)
		{
			GameDataBridge.Logger.Info("Incoming message: Initialize");
			RawDataPool dataPool = new RawDataPool(GameDataBridge._slaverPipe, contentLength);
			int offset = 0;
			string gameVersion = "";
			List<IConfigData> cachedConfigs = GameDataBridge.CachedConfigs;
			lock (cachedConfigs)
			{
				int gameVersionSize = SerializationHelper.Deserialize(dataPool.GetPointer(offset), out gameVersion);
				offset += gameVersionSize;
				int dlcInfosSize = (int)dataPool.GetUnmanaged<uint>(offset);
				offset += 4;
				int readSize = GameDataBridge._dlcInfos.Deserialize(dataPool.GetPointer(offset));
				bool flag2 = dlcInfosSize != readSize;
				if (flag2)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Serialized size of dlc data: expected ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(dlcInfosSize);
					defaultInterpolatedStringHandler.AppendLiteral(", actual ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(readSize);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				offset += readSize;
				int modInfosSize = (int)dataPool.GetUnmanaged<uint>(offset);
				offset += 4;
				readSize = GameDataBridge._modInfos.Deserialize(dataPool.GetPointer(offset));
				bool flag3 = modInfosSize != readSize;
				if (flag3)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(47, 2);
					defaultInterpolatedStringHandler.AppendLiteral("Serialized size of mod data: expected ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(modInfosSize);
					defaultInterpolatedStringHandler.AppendLiteral(", actual ");
					defaultInterpolatedStringHandler.AppendFormatted<int>(readSize);
					throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				offset += readSize;
			}
			bool flag4 = contentLength != offset;
			if (flag4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Content length: expected ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(contentLength);
				defaultInterpolatedStringHandler.AppendLiteral(", actual ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(offset);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			DomainManager.Global.SetGameVersion(gameVersion);
			LogManager.GetCurrentClassLogger().Info(DomainManager.Global.GetGameVersion());
			DlcManager.SetDldInfoList(GameDataBridge._dlcInfos.Items);
			GameDataBridge.SetGameDataModuleInitializationState(1);
			return false;
		}

		// Token: 0x06000D07 RID: 3335 RVA: 0x000DD6E4 File Offset: 0x000DB8E4
		private unsafe static bool ReadInterProcessMessageOperations(int contentLength)
		{
			uint operationsCount;
			GameDataBridge.GetUnmanagedValuesFromSocket<uint>(out operationsCount);
			int operationsContentLength = sizeof(Operation) * (int)operationsCount;
			byte[] buffer = GameDataBridge.GetRawDataFromSocket(operationsContentLength);
			List<Operation> operations = new List<Operation>();
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			byte* pCurrData = pBuffer;
			byte* pEnd = pBuffer + operationsContentLength;
			while (pCurrData < pEnd)
			{
				operations.Add(*(Operation*)pCurrData);
				pCurrData += sizeof(Operation);
			}
			array = null;
			uint dataPoolSize;
			GameDataBridge.GetUnmanagedValuesFromSocket<uint>(out dataPoolSize);
			RawDataPool dataPool = new RawDataPool(GameDataBridge._slaverPipe, (int)dataPoolSize);
			OperationCollection operationCollection = new OperationCollection(operations, dataPool);
			object operationCollectionsLock = GameDataBridge.OperationCollectionsLock;
			lock (operationCollectionsLock)
			{
				GameDataBridge._operationCollections.Add(operationCollection);
			}
			long readSize = (long)(4 + operationsContentLength + 4) + (long)((ulong)dataPoolSize);
			bool flag2 = (long)contentLength != readSize;
			if (flag2)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Content length: expected ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(contentLength);
				defaultInterpolatedStringHandler.AppendLiteral(", actual ");
				defaultInterpolatedStringHandler.AppendFormatted<long>(readSize);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			return false;
		}

		// Token: 0x06000D08 RID: 3336 RVA: 0x000DD82C File Offset: 0x000DBA2C
		private static bool ReadInterProcessMessageDisconnect(int contentLength)
		{
			GameDataBridge.Logger.Info("Incoming message: Disconnect");
			bool flag = contentLength != 0;
			if (flag)
			{
				throw new Exception("Content length of Disconnect message must be zero: " + contentLength.ToString());
			}
			GameDataBridge._shouldDisconnect = true;
			return true;
		}

		// Token: 0x06000D09 RID: 3337 RVA: 0x000DD878 File Offset: 0x000DBA78
		private static void WriteInterProcessMessages()
		{
			GameDataBridge.Logger.Info("WriteInterProcessMessages thread started.");
			for (;;)
			{
				try
				{
					GameDataBridge.WriteInterProcessMessagesWarningMessages();
					GameDataBridge.WriteInterProcessMessagesErrorMessages();
					bool flag = GameDataBridge.WriteInterProcessMessagesDisconnect();
					if (flag)
					{
						break;
					}
					GameDataBridge.WriteInterProcessMessagesGameModuleInitialized();
					GameDataBridge.WriteInterProcessMessagesNotifications();
					Thread.Sleep(16);
				}
				catch (Exception ex)
				{
					GameDataBridge.Logger.Error<Exception>(ex);
				}
			}
			GameDataBridge.Logger.Info("WriteInterProcessMessages thread is about to exit.");
			LogManager.Flush();
		}

		// Token: 0x06000D0A RID: 3338 RVA: 0x000DD904 File Offset: 0x000DBB04
		private unsafe static void WriteInterProcessMessagesGameModuleInitialized()
		{
			bool flag = GameDataBridge._gameDataModuleInitializationState != 2;
			if (!flag)
			{
				GameDataBridge.Logger.Info("Outgoing message: GameModuleInitialized");
				byte[] buffer = GameDataBridge.OutgoingMessageBuffer.Get(5);
				byte[] array;
				byte* pBuffer;
				if ((array = buffer) == null || array.Length == 0)
				{
					pBuffer = null;
				}
				else
				{
					pBuffer = &array[0];
				}
				*pBuffer = 0;
				*(int*)(pBuffer + 1) = 0;
				array = null;
				GameDataBridge._slaverPipe.Write(buffer, 0, 5);
				GameDataBridge.SetGameDataModuleInitializationState(3);
			}
		}

		// Token: 0x06000D0B RID: 3339 RVA: 0x000DD97C File Offset: 0x000DBB7C
		private static void WriteInterProcessMessagesNotifications()
		{
			object notificationCollectionsLock = GameDataBridge.NotificationCollectionsLock;
			lock (notificationCollectionsLock)
			{
				bool flag2 = GameDataBridge._notificationCollections.Count <= 0;
				if (flag2)
				{
					return;
				}
				List<NotificationCollection> notificationCollections = GameDataBridge._notificationCollections;
				List<NotificationCollection> writingNotificationCollections = GameDataBridge._writingNotificationCollections;
				GameDataBridge._writingNotificationCollections = notificationCollections;
				GameDataBridge._notificationCollections = writingNotificationCollections;
				GameDataBridge._notificationCollections.Clear();
			}
			foreach (NotificationCollection collection in GameDataBridge._writingNotificationCollections)
			{
				int dataSize;
				byte[] buffer = GameDataBridge.CreateNotificationCollectionData(collection, out dataSize);
				GameDataBridge._slaverPipe.Write(buffer, 0, dataSize);
				collection.DataPool.CopyTo(GameDataBridge._slaverPipe);
			}
			GameDataBridge._writingNotificationCollections.Clear();
		}

		// Token: 0x06000D0C RID: 3340 RVA: 0x000DDA6C File Offset: 0x000DBC6C
		private unsafe static byte[] CreateNotificationCollectionData(NotificationCollection collection, out int dataSize)
		{
			int notificationsCount = collection.Notifications.Count;
			int contentLengthWithoutDataPool = 4 + sizeof(Notification) * notificationsCount + 4;
			int contentLength = contentLengthWithoutDataPool + collection.DataPool.RawDataSize;
			dataSize = 5 + contentLengthWithoutDataPool;
			byte[] buffer = GameDataBridge.OutgoingMessageBuffer.Get(dataSize);
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			byte* pCurrData = pBuffer;
			*pCurrData = 1;
			pCurrData++;
			*(int*)pCurrData = contentLength;
			pCurrData += 4;
			*(int*)pCurrData = notificationsCount;
			pCurrData += 4;
			for (int i = 0; i < notificationsCount; i++)
			{
				*(Notification*)pCurrData = collection.Notifications[i];
				pCurrData += sizeof(Notification);
			}
			*(int*)pCurrData = collection.DataPool.RawDataSize;
			array = null;
			return buffer;
		}

		// Token: 0x06000D0D RID: 3341 RVA: 0x000DDB44 File Offset: 0x000DBD44
		private static void WriteInterProcessMessagesErrorMessages()
		{
			List<string> errorMessages = GameDataBridge.ErrorMessages;
			int dataSize;
			byte[] buffer;
			lock (errorMessages)
			{
				bool flag2 = GameDataBridge.ErrorMessages.Count <= 0;
				if (flag2)
				{
					return;
				}
				buffer = GameDataBridge.CreateErrorMessagesData(out dataSize);
				GameDataBridge.ErrorMessages.Clear();
			}
			GameDataBridge.Logger.Info("Outgoing message: ErrorMessages");
			Slaver slaverPipe = GameDataBridge._slaverPipe;
			if (slaverPipe != null)
			{
				slaverPipe.Write(buffer, 0, dataSize);
			}
		}

		// Token: 0x06000D0E RID: 3342 RVA: 0x000DDBD4 File Offset: 0x000DBDD4
		private static void WriteInterProcessMessagesWarningMessages()
		{
			List<string> warningMessages = GameDataBridge.WarningMessages;
			int dataSize;
			byte[] buffer;
			lock (warningMessages)
			{
				bool flag2 = GameDataBridge.WarningMessages.Count <= 0;
				if (flag2)
				{
					return;
				}
				buffer = GameDataBridge.CreateWarningMessagesData(out dataSize);
				GameDataBridge.WarningMessages.Clear();
			}
			GameDataBridge.Logger.Info("Outgoing message: WarningMessage");
			Slaver slaverPipe = GameDataBridge._slaverPipe;
			if (slaverPipe != null)
			{
				slaverPipe.Write(buffer, 0, dataSize);
			}
		}

		// Token: 0x06000D0F RID: 3343 RVA: 0x000DDC64 File Offset: 0x000DBE64
		private unsafe static byte[] CreateErrorMessagesData(out int dataSize)
		{
			int errorMessagesCount = GameDataBridge.ErrorMessages.Count;
			int contentLength = 0;
			for (int i = 0; i < errorMessagesCount; i++)
			{
				int elementSize = 2 * GameDataBridge.ErrorMessages[i].Length;
				bool flag = elementSize > 65000;
				if (flag)
				{
					elementSize = 65000;
				}
				contentLength += 2 + elementSize;
			}
			dataSize = 5 + contentLength;
			byte[] buffer = GameDataBridge.OutgoingMessageBuffer.Get(dataSize);
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			byte* pCurrData = pBuffer;
			*pCurrData = 2;
			pCurrData++;
			*(int*)pCurrData = contentLength;
			pCurrData += 4;
			for (int j = 0; j < errorMessagesCount; j++)
			{
				string element = GameDataBridge.ErrorMessages[j];
				int charsCount = element.Length;
				int elementSize2 = 2 * charsCount;
				bool flag2 = elementSize2 > 65000;
				if (flag2)
				{
					elementSize2 = 65000;
					charsCount = 32500;
				}
				*(short*)pCurrData = (short)((ushort)elementSize2);
				pCurrData += 2;
				char* ptr;
				if (element == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = element.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pErrorMessage = ptr;
				for (int k = 0; k < charsCount; k++)
				{
					*(short*)(pCurrData + (IntPtr)k * 2) = (short)pErrorMessage[k];
				}
				char* ptr2 = null;
				pCurrData += elementSize2;
			}
			array = null;
			return buffer;
		}

		// Token: 0x06000D10 RID: 3344 RVA: 0x000DDDCC File Offset: 0x000DBFCC
		private unsafe static byte[] CreateWarningMessagesData(out int dataSize)
		{
			int errorMessagesCount = GameDataBridge.WarningMessages.Count;
			int contentLength = 0;
			for (int i = 0; i < errorMessagesCount; i++)
			{
				int elementSize = 2 * GameDataBridge.WarningMessages[i].Length;
				bool flag = elementSize > 65536;
				if (flag)
				{
					elementSize = 65536;
				}
				contentLength += 2 + elementSize;
			}
			dataSize = 5 + contentLength;
			byte[] buffer = GameDataBridge.OutgoingMessageBuffer.Get(dataSize);
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			byte* pCurrData = pBuffer;
			*pCurrData = 3;
			pCurrData++;
			*(int*)pCurrData = contentLength;
			pCurrData += 4;
			for (int j = 0; j < errorMessagesCount; j++)
			{
				string element = GameDataBridge.WarningMessages[j];
				int charsCount = element.Length;
				int elementSize2 = 2 * charsCount;
				bool flag2 = elementSize2 > 65536;
				if (flag2)
				{
					elementSize2 = 65536;
					charsCount = 32768;
				}
				*(short*)pCurrData = (short)((ushort)elementSize2);
				pCurrData += 2;
				char* ptr;
				if (element == null)
				{
					ptr = null;
				}
				else
				{
					fixed (char* ptr2 = element.GetPinnableReference())
					{
						ptr = ptr2;
					}
				}
				char* pWarningMessage = ptr;
				for (int k = 0; k < charsCount; k++)
				{
					*(short*)(pCurrData + (IntPtr)k * 2) = (short)pWarningMessage[k];
				}
				char* ptr2 = null;
				pCurrData += elementSize2;
			}
			array = null;
			return buffer;
		}

		// Token: 0x06000D11 RID: 3345 RVA: 0x000DDF34 File Offset: 0x000DC134
		private unsafe static bool WriteInterProcessMessagesDisconnect()
		{
			bool flag = !GameDataBridge._shouldDisconnect;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				GameDataBridge.Logger.Info("Outgoing message: Disconnect");
				try
				{
					byte[] buffer = GameDataBridge.OutgoingMessageBuffer.Get(5);
					try
					{
						byte[] array;
						byte* pBuffer;
						if ((array = buffer) == null || array.Length == 0)
						{
							pBuffer = null;
						}
						else
						{
							pBuffer = &array[0];
						}
						*pBuffer = 4;
						*(int*)(pBuffer + 1) = 0;
					}
					finally
					{
						byte[] array = null;
					}
					Slaver slaverPipe = GameDataBridge._slaverPipe;
					if (slaverPipe != null)
					{
						slaverPipe.Write(buffer, 0, 5);
					}
				}
				catch (Exception ex)
				{
					GameDataBridge.Logger.Error<Exception>(ex);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000D12 RID: 3346 RVA: 0x000DDFE8 File Offset: 0x000DC1E8
		private static byte[] GetRawDataFromSocket(int size)
		{
			byte[] buffer = GameDataBridge.IncomingMessageBuffer.Get(size);
			int received;
			for (int totalReceived = 0; totalReceived < size; totalReceived += received)
			{
				received = GameDataBridge._slaverPipe.Read(buffer, totalReceived, size - totalReceived);
			}
			return buffer;
		}

		// Token: 0x06000D13 RID: 3347 RVA: 0x000DE02C File Offset: 0x000DC22C
		private unsafe static void GetUnmanagedValuesFromSocket<[IsUnmanaged] T1>(out T1 item1) where T1 : struct, ValueType
		{
			int size = sizeof(T1);
			byte[] buffer = GameDataBridge.IncomingMessageBuffer.Get(size);
			int received;
			for (int totalReceived = 0; totalReceived < size; totalReceived += received)
			{
				received = GameDataBridge._slaverPipe.Read(buffer, totalReceived, size - totalReceived);
			}
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			item1 = *(T1*)pBuffer;
			array = null;
		}

		// Token: 0x06000D14 RID: 3348 RVA: 0x000DE0A0 File Offset: 0x000DC2A0
		private unsafe static void GetUnmanagedValuesFromSocket<[IsUnmanaged] T1, [IsUnmanaged] T2>(out T1 item1, out T2 item2) where T1 : struct, ValueType where T2 : struct, ValueType
		{
			int size = sizeof(T1) + sizeof(T2);
			byte[] buffer = GameDataBridge.IncomingMessageBuffer.Get(size);
			int received;
			for (int totalReceived = 0; totalReceived < size; totalReceived += received)
			{
				received = GameDataBridge._slaverPipe.Read(buffer, totalReceived, size - totalReceived);
			}
			byte[] array;
			byte* pBuffer;
			if ((array = buffer) == null || array.Length == 0)
			{
				pBuffer = null;
			}
			else
			{
				pBuffer = &array[0];
			}
			item1 = *(T1*)pBuffer;
			item2 = *(T2*)(pBuffer + sizeof(T1));
			array = null;
		}

		// Token: 0x04000082 RID: 130
		public const int TimerResolution = 1;

		// Token: 0x04000083 RID: 131
		public const int FramesPerSecond = 60;

		// Token: 0x04000084 RID: 132
		public const int FrameRateDropWarningThreshold = 40;

		// Token: 0x04000085 RID: 133
		public static int ActualFramesPerSecond = 60;

		// Token: 0x04000086 RID: 134
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

		// Token: 0x04000087 RID: 135
		private static volatile bool _shouldDisconnect;

		// Token: 0x04000088 RID: 136
		private static volatile sbyte _gameDataModuleInitializationState;

		// Token: 0x04000089 RID: 137
		private static readonly DataMonitorManager DataMonitorManager = new DataMonitorManager();

		// Token: 0x0400008A RID: 138
		private static readonly Dictionary<uint, Operation> PassthroughOperations = new Dictionary<uint, Operation>();

		// Token: 0x0400008B RID: 139
		private static readonly Stopwatch Stopwatch = new Stopwatch();

		// Token: 0x0400008C RID: 140
		private static long _frameBeginTicks;

		// Token: 0x0400008D RID: 141
		private static long _averageSleepInterval = Stopwatch.Frequency / 1000L;

		// Token: 0x0400008E RID: 142
		private static readonly long ExpectedTicksPerFrame = Stopwatch.Frequency / 60L;

		// Token: 0x0400008F RID: 143
		private const int NotificationDataPoolDefaultCapacity = 65536;

		// Token: 0x04000090 RID: 144
		private const int InterProcessMessageBufferDefaultSize = 65536;

		// Token: 0x04000091 RID: 145
		private const int WritingThreadSleepInterval = 16;

		// Token: 0x04000092 RID: 146
		private const int ThreadJoinTimeout = 1000;

		// Token: 0x04000093 RID: 147
		private static Slaver _slaverPipe;

		// Token: 0x04000094 RID: 148
		private static Thread _readingThread;

		// Token: 0x04000095 RID: 149
		private static Thread _writingThread;

		// Token: 0x04000096 RID: 150
		private static readonly IncreasableBuffer IncomingMessageBuffer = new IncreasableBuffer(65536);

		// Token: 0x04000097 RID: 151
		private static readonly IncreasableBuffer OutgoingMessageBuffer = new IncreasableBuffer(65536);

		// Token: 0x04000098 RID: 152
		private static List<OperationCollection> _operationCollections = new List<OperationCollection>();

		// Token: 0x04000099 RID: 153
		private static readonly object OperationCollectionsLock = new object();

		// Token: 0x0400009A RID: 154
		private static List<OperationCollection> _processingOperationCollections = new List<OperationCollection>();

		// Token: 0x0400009B RID: 155
		private static List<NotificationCollection> _notificationCollections = new List<NotificationCollection>();

		// Token: 0x0400009C RID: 156
		private static readonly object NotificationCollectionsLock = new object();

		// Token: 0x0400009D RID: 157
		private static NotificationCollection _pendingNotifications = new NotificationCollection(65536);

		// Token: 0x0400009E RID: 158
		private static List<NotificationCollection> _writingNotificationCollections = new List<NotificationCollection>();

		// Token: 0x0400009F RID: 159
		private static readonly List<IConfigData> CachedConfigs = new List<IConfigData>();

		// Token: 0x040000A0 RID: 160
		private static DlcInfoList _dlcInfos = DlcInfoList.Create();

		// Token: 0x040000A1 RID: 161
		private static ModInfoList _modInfos = ModInfoList.Create();

		// Token: 0x040000A2 RID: 162
		private static readonly List<string> ErrorMessages = new List<string>();

		// Token: 0x040000A3 RID: 163
		private static readonly List<string> WarningMessages = new List<string>();
	}
}
