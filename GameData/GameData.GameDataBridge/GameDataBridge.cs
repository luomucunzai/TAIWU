using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;
using Config.Common;
using GameData.ArchiveData;
using GameData.Common;
using GameData.DLC;
using GameData.Dependencies;
using GameData.DomainEvents;
using GameData.Domains;
using GameData.Domains.Mod;
using GameData.GameDataBridge.VnPipe;
using GameData.Serializer;
using GameData.Steamworks;
using GameData.Utilities;
using NLog;

namespace GameData.GameDataBridge;

public static class GameDataBridge
{
	public const int TimerResolution = 1;

	public const int FramesPerSecond = 60;

	public const int FrameRateDropWarningThreshold = 40;

	public static int ActualFramesPerSecond = 60;

	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	private static volatile bool _shouldDisconnect;

	private static volatile sbyte _gameDataModuleInitializationState;

	private static readonly DataMonitorManager DataMonitorManager = new DataMonitorManager();

	private static readonly Dictionary<uint, Operation> PassthroughOperations = new Dictionary<uint, Operation>();

	private static readonly Stopwatch Stopwatch = new Stopwatch();

	private static long _frameBeginTicks;

	private static long _averageSleepInterval = Stopwatch.Frequency / 1000;

	private static readonly long ExpectedTicksPerFrame = Stopwatch.Frequency / 60;

	private const int NotificationDataPoolDefaultCapacity = 65536;

	private const int InterProcessMessageBufferDefaultSize = 65536;

	private const int WritingThreadSleepInterval = 16;

	private const int ThreadJoinTimeout = 1000;

	private static Slaver _slaverPipe;

	private static Thread _readingThread;

	private static Thread _writingThread;

	private static readonly IncreasableBuffer IncomingMessageBuffer = new IncreasableBuffer(65536);

	private static readonly IncreasableBuffer OutgoingMessageBuffer = new IncreasableBuffer(65536);

	private static List<OperationCollection> _operationCollections = new List<OperationCollection>();

	private static readonly object OperationCollectionsLock = new object();

	private static List<OperationCollection> _processingOperationCollections = new List<OperationCollection>();

	private static List<NotificationCollection> _notificationCollections = new List<NotificationCollection>();

	private static readonly object NotificationCollectionsLock = new object();

	private static NotificationCollection _pendingNotifications = new NotificationCollection(65536);

	private static List<NotificationCollection> _writingNotificationCollections = new List<NotificationCollection>();

	private static readonly List<IConfigData> CachedConfigs = new List<IConfigData>();

	private static DlcInfoList _dlcInfos = DlcInfoList.Create();

	private static ModInfoList _modInfos = ModInfoList.Create();

	private static readonly List<string> ErrorMessages = new List<string>();

	private static readonly List<string> WarningMessages = new List<string>();

	public static void RunMainLoop()
	{
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		_shouldDisconnect = false;
		Stopwatch.Start();
		Logger.Info("Machine Frequency (Ticks/Second): " + Stopwatch.Frequency);
		_frameBeginTicks = Stopwatch.ElapsedTicks;
		timeBeginPeriod(1u);
		while (!_shouldDisconnect)
		{
			if (!CheckErrorMessages() || !GameData.ArchiveData.Common.CheckAsyncError())
			{
				return;
			}
			ArchiveDataManager.GetResponse();
			ArchiveDataManager.SendRequest();
			if (DomainManager.Global.GlobalDataLoaded)
			{
				break;
			}
			AdvanceFrame();
		}
		while (!_shouldDisconnect && CheckErrorMessages())
		{
			if (_gameDataModuleInitializationState == 1)
			{
				InitializeGameDataModule();
			}
			SteamManager.Update();
			if (IsGameDataModuleInitialized())
			{
				if (!GameData.ArchiveData.Common.CheckAsyncError())
				{
					break;
				}
				ArchiveDataManager.GetResponse();
				ProcessOperations(currentThreadDataContext);
				for (int i = 0; i < DomainManager.Domains.Length; i++)
				{
					DomainManager.Domains[i].OnUpdate(currentThreadDataContext);
				}
				currentThreadDataContext.ParallelModificationsRecorder.ApplyAll(currentThreadDataContext);
				DataMonitorManager.CheckMonitoredData();
				TransferPendingNotifications();
				Events.RaiseBeforeSendRequestToArchiveModule(currentThreadDataContext);
				ArchiveDataManager.SendRequest();
			}
			AdvanceFrame();
		}
		timeEndPeriod(1u);
	}

	private static void AdvanceFrame()
	{
		long num = Stopwatch.ElapsedTicks - _frameBeginTicks;
		long num2 = ExpectedTicksPerFrame - num;
		if (_averageSleepInterval == 0)
		{
			_averageSleepInterval = Stopwatch.Frequency / 1000;
		}
		long num3 = num2 / _averageSleepInterval * 8 / 10;
		if (num3 == 0)
		{
			num3++;
		}
		long elapsedTicks = Stopwatch.ElapsedTicks;
		int i;
		for (i = 0; i < num3; i++)
		{
			if (num >= ExpectedTicksPerFrame)
			{
				break;
			}
			Thread.Sleep(1);
			num = Stopwatch.ElapsedTicks - _frameBeginTicks;
		}
		if (i > 0)
		{
			_averageSleepInterval = (Stopwatch.ElapsedTicks - elapsedTicks) / i;
		}
		for (num = Stopwatch.ElapsedTicks - _frameBeginTicks; num < ExpectedTicksPerFrame; num = Stopwatch.ElapsedTicks - _frameBeginTicks)
		{
			Thread.Sleep(0);
		}
		ActualFramesPerSecond = (int)(Stopwatch.Frequency / num + 1);
		_frameBeginTicks = Stopwatch.ElapsedTicks;
	}

	private static void InitializeGameDataModule()
	{
		lock (CachedConfigs)
		{
			DomainManager.Mod.LoadAllMods(_modInfos);
			_modInfos.Items.Clear();
		}
		DataDependenciesInfo.Generate();
		BaseGameDataDomain[] domains = DomainManager.Domains;
		foreach (BaseGameDataDomain baseGameDataDomain in domains)
		{
			baseGameDataDomain.OnInitializeGameDataModule();
		}
		SetGameDataModuleInitializationState(2);
	}

	private static void ProcessOperations(DataContext context)
	{
		lock (OperationCollectionsLock)
		{
			if (_operationCollections.Count <= 0)
			{
				return;
			}
			List<OperationCollection> operationCollections = _operationCollections;
			List<OperationCollection> processingOperationCollections = _processingOperationCollections;
			_processingOperationCollections = operationCollections;
			_operationCollections = processingOperationCollections;
			_operationCollections.Clear();
		}
		int count = _processingOperationCollections.Count;
		for (int i = 0; i < count; i++)
		{
			OperationCollection operationCollection = _processingOperationCollections[i];
			RawDataPool dataPool = operationCollection.DataPool;
			int count2 = operationCollection.Operations.Count;
			for (int j = 0; j < count2; j++)
			{
				Operation operation = operationCollection.Operations[j];
				switch (operation.Type)
				{
				case 0:
					ProcessDataMonitor(operation);
					break;
				case 1:
					ProcessDataUnMonitor(operation);
					break;
				case 2:
					ProcessDataModification(operation, dataPool, context);
					break;
				case 3:
					ProcessMethodCall(operation, dataPool, context);
					break;
				default:
					throw new Exception($"Unsupported operation type: {operation.Type}");
				}
			}
		}
		_processingOperationCollections.Clear();
	}

	private static void ProcessDataMonitor(Operation operation)
	{
		DataUid uid = new DataUid(operation.DomainId, operation.DataId, operation.SubId0, operation.SubId1);
		DataMonitorManager.MonitorData(uid);
	}

	private static void ProcessDataUnMonitor(Operation operation)
	{
		DataUid uid = new DataUid(operation.DomainId, operation.DataId, operation.SubId0, operation.SubId1);
		DataMonitorManager.UnMonitorData(uid);
	}

	private static void ProcessDataModification(Operation operation, RawDataPool dataPool, DataContext context)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[operation.DomainId];
		baseGameDataDomain.SetData(operation.DataId, operation.SubId0, operation.SubId1, operation.ValueOffset, dataPool, context);
	}

	private static void ProcessMethodCall(Operation operation, RawDataPool argDataPool, DataContext context)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[operation.DomainId];
		int num = baseGameDataDomain.CallMethod(operation, argDataPool, _pendingNotifications.DataPool, context);
		if (num >= 0)
		{
			_pendingNotifications.Notifications.Add(Notification.CreateMethodReturn(operation.ListenerId, operation.DomainId, operation.MethodId, num));
		}
	}

	public static NotificationCollection GetPendingNotifications()
	{
		return _pendingNotifications;
	}

	public static void TransferPendingNotifications()
	{
		if (_pendingNotifications.Notifications.Count <= 0)
		{
			return;
		}
		lock (NotificationCollectionsLock)
		{
			_notificationCollections.Add(_pendingNotifications);
			_pendingNotifications = new NotificationCollection(65536);
		}
	}

	public static (DataMonitorManager, NotificationCollection) StartSemiBlockingTask()
	{
		DataMonitorManager item = new DataMonitorManager();
		NotificationCollection pendingNotifications = _pendingNotifications;
		_pendingNotifications = new NotificationCollection(65536);
		return (item, pendingNotifications);
	}

	public static void StopSemiBlockingTask(DataMonitorManager monitor, NotificationCollection oriPendingNotifications)
	{
		monitor.CheckMonitoredData();
		monitor.Clear();
		TransferPendingNotifications();
		_pendingNotifications = oriPendingNotifications;
	}

	public static void AppendErrorMessage(string message)
	{
		lock (ErrorMessages)
		{
			ErrorMessages.Add(DomainManager.Global.GetGameVersion() + " " + message);
		}
	}

	public static void AppendWarningMessage(string message)
	{
		lock (WarningMessages)
		{
			WarningMessages.Add(message);
		}
	}

	public static void ClearMonitoredData()
	{
		DataMonitorManager.Clear();
	}

	private static bool CheckErrorMessages()
	{
		lock (ErrorMessages)
		{
			return ErrorMessages.Count <= 0;
		}
	}

	private static bool CheckWarningMessages()
	{
		lock (WarningMessages)
		{
			return WarningMessages.Count <= 0;
		}
	}

	public static uint RecordPassthroughMethod(Operation displayOperation)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		PassthroughOperations.Add(nextOperationId, displayOperation);
		return nextOperationId;
	}

	public static void TryReturnPassthroughMethod<T>(uint archiveOperationId, T returnValue)
	{
		if (PassthroughOperations.TryGetValue(archiveOperationId, out var value))
		{
			int returnValueOffset = SerializerHolder<T>.Serialize(returnValue, _pendingNotifications.DataPool);
			_pendingNotifications.Notifications.Add(Notification.CreateMethodReturn(value.ListenerId, value.DomainId, value.MethodId, returnValueOffset));
			PassthroughOperations.Remove(archiveOperationId);
		}
	}

	private static void SetGameDataModuleInitializationState(sbyte state)
	{
		if (!GameDataModuleInitializationState.CheckTransition(_gameDataModuleInitializationState, state))
		{
			throw new Exception($"Invalid transition: {_gameDataModuleInitializationState} -> {state}");
		}
		Logger.Info($"Transition: {_gameDataModuleInitializationState} -> {state}");
		_gameDataModuleInitializationState = state;
	}

	public static bool IsGameDataModuleInitialized()
	{
		sbyte gameDataModuleInitializationState = _gameDataModuleInitializationState;
		if ((uint)(gameDataModuleInitializationState - 2) <= 1u)
		{
			return true;
		}
		return false;
	}

	public static void AddPostDataModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
	{
		DataMonitorManager.AddPostModificationHandler(uid, handlerKey, handler);
	}

	public static void RemovePostDataModificationHandler(DataUid uid, string handlerKey)
	{
		DataMonitorManager.RemovePostModificationHandler(uid, handlerKey);
	}

	[DllImport("winmm.dll")]
	internal static extern uint timeBeginPeriod(uint period);

	[DllImport("winmm.dll")]
	internal static extern uint timeEndPeriod(uint period);

	public static void AddDisplayEvent(DisplayEventType type)
	{
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, -1));
	}

	public static void AddDisplayEvent<T1>(DisplayEventType type, T1 arg1)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2>(DisplayEventType type, T1 arg1, T2 arg2)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3, T4>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		SerializerHolder<T4>.Serialize(arg4, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3, T4, T5>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		SerializerHolder<T4>.Serialize(arg4, dataPool);
		SerializerHolder<T5>.Serialize(arg5, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		SerializerHolder<T4>.Serialize(arg4, dataPool);
		SerializerHolder<T5>.Serialize(arg5, dataPool);
		SerializerHolder<T6>.Serialize(arg6, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6, T7>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		SerializerHolder<T4>.Serialize(arg4, dataPool);
		SerializerHolder<T5>.Serialize(arg5, dataPool);
		SerializerHolder<T6>.Serialize(arg6, dataPool);
		SerializerHolder<T7>.Serialize(arg7, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void AddDisplayEvent<T1, T2, T3, T4, T5, T6, T7, T8>(DisplayEventType type, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
	{
		RawDataPool dataPool = _pendingNotifications.DataPool;
		int valueOffset = SerializerHolder<T1>.Serialize(arg1, dataPool);
		SerializerHolder<T2>.Serialize(arg2, dataPool);
		SerializerHolder<T3>.Serialize(arg3, dataPool);
		SerializerHolder<T4>.Serialize(arg4, dataPool);
		SerializerHolder<T5>.Serialize(arg5, dataPool);
		SerializerHolder<T6>.Serialize(arg6, dataPool);
		SerializerHolder<T7>.Serialize(arg7, dataPool);
		SerializerHolder<T8>.Serialize(arg8, dataPool);
		_pendingNotifications.Notifications.Add(Notification.CreateDisplayEvent(type, valueOffset));
	}

	public static void Initialize()
	{
		_gameDataModuleInitializationState = 0;
		_slaverPipe = Slaver.Connect("taiwu");
		if (_slaverPipe == null)
		{
			throw new Exception("pipe connect failed");
		}
		_readingThread = new Thread(ReadInterProcessMessages)
		{
			IsBackground = false,
			Name = "ReadInterProcessMessages"
		};
		_readingThread.Start();
		_writingThread = new Thread(WriteInterProcessMessages)
		{
			IsBackground = false,
			Name = "WriteInterProcessMessages"
		};
		_writingThread.Start();
	}

	public static void UnInitialize()
	{
		_shouldDisconnect = true;
		if (_readingThread != null)
		{
			if (_readingThread.ThreadState != ThreadState.Unstarted && !_readingThread.Join(1000))
			{
				Logger.Warn("Failed to wait for _readingThread to terminate.");
			}
			_readingThread = null;
		}
		if (_writingThread != null)
		{
			if (_writingThread.ThreadState != ThreadState.Unstarted && !_writingThread.Join(1000))
			{
				Logger.Warn("Failed to wait for _writingThread to terminate.");
			}
			_writingThread = null;
		}
		if (_slaverPipe != null)
		{
			_slaverPipe.Dispose();
			_slaverPipe = null;
		}
	}

	private static void ReadInterProcessMessages()
	{
		Logger.Info("ReadInterProcessMessages thread started.");
		try
		{
			bool flag = false;
			while (!flag)
			{
				flag = ReadInterProcessMessage();
			}
		}
		catch (Exception ex)
		{
			Logger.Error<Exception>(ex);
		}
		Logger.Info("ReadInterProcessMessages thread is about to exit.");
		LogManager.Flush();
		_shouldDisconnect = true;
	}

	private static bool ReadInterProcessMessage()
	{
		GetUnmanagedValuesFromSocket<byte, int>(out var item, out var item2);
		return item switch
		{
			0 => ReadInterProcessMessageInitialize(item2), 
			1 => ReadInterProcessMessageOperations(item2), 
			2 => ReadInterProcessMessageDisconnect(item2), 
			_ => throw new Exception("Unknown message type: " + item), 
		};
	}

	private unsafe static bool ReadInterProcessMessageInitialize(int contentLength)
	{
		Logger.Info("Incoming message: Initialize");
		RawDataPool rawDataPool = new RawDataPool((IPipe)_slaverPipe, contentLength);
		int num = 0;
		string gameVersion = "";
		lock (CachedConfigs)
		{
			int num2 = SerializationHelper.Deserialize(rawDataPool.GetPointer(num), ref gameVersion);
			num += num2;
			int unmanaged = (int)rawDataPool.GetUnmanaged<uint>(num);
			num += 4;
			int num3 = _dlcInfos.Deserialize(rawDataPool.GetPointer(num));
			if (unmanaged != num3)
			{
				throw new Exception($"Serialized size of dlc data: expected {unmanaged}, actual {num3}");
			}
			num += num3;
			int unmanaged2 = (int)rawDataPool.GetUnmanaged<uint>(num);
			num += 4;
			num3 = _modInfos.Deserialize(rawDataPool.GetPointer(num));
			if (unmanaged2 != num3)
			{
				throw new Exception($"Serialized size of mod data: expected {unmanaged2}, actual {num3}");
			}
			num += num3;
		}
		if (contentLength != num)
		{
			throw new Exception($"Content length: expected {contentLength}, actual {num}");
		}
		DomainManager.Global.SetGameVersion(gameVersion);
		LogManager.GetCurrentClassLogger().Info(DomainManager.Global.GetGameVersion());
		DlcManager.SetDldInfoList(_dlcInfos.Items);
		SetGameDataModuleInitializationState(1);
		return false;
	}

	private unsafe static bool ReadInterProcessMessageOperations(int contentLength)
	{
		GetUnmanagedValuesFromSocket<uint>(out var item);
		int num = sizeof(Operation) * (int)item;
		byte[] rawDataFromSocket = GetRawDataFromSocket(num);
		List<Operation> list = new List<Operation>();
		fixed (byte* ptr = rawDataFromSocket)
		{
			byte* ptr2 = ptr;
			for (byte* ptr3 = ptr + num; ptr2 < ptr3; ptr2 += sizeof(Operation))
			{
				list.Add(*(Operation*)ptr2);
			}
		}
		GetUnmanagedValuesFromSocket<uint>(out var item2);
		RawDataPool dataPool = new RawDataPool((IPipe)_slaverPipe, (int)item2);
		OperationCollection item3 = new OperationCollection(list, dataPool);
		lock (OperationCollectionsLock)
		{
			_operationCollections.Add(item3);
		}
		long num2 = 4 + num + 4 + item2;
		if (contentLength != num2)
		{
			throw new Exception($"Content length: expected {contentLength}, actual {num2}");
		}
		return false;
	}

	private static bool ReadInterProcessMessageDisconnect(int contentLength)
	{
		Logger.Info("Incoming message: Disconnect");
		if (contentLength != 0)
		{
			throw new Exception("Content length of Disconnect message must be zero: " + contentLength);
		}
		_shouldDisconnect = true;
		return true;
	}

	private static void WriteInterProcessMessages()
	{
		Logger.Info("WriteInterProcessMessages thread started.");
		while (true)
		{
			try
			{
				WriteInterProcessMessagesWarningMessages();
				WriteInterProcessMessagesErrorMessages();
				if (WriteInterProcessMessagesDisconnect())
				{
					break;
				}
				WriteInterProcessMessagesGameModuleInitialized();
				WriteInterProcessMessagesNotifications();
				Thread.Sleep(16);
				continue;
			}
			catch (Exception ex)
			{
				Logger.Error<Exception>(ex);
				continue;
			}
		}
		Logger.Info("WriteInterProcessMessages thread is about to exit.");
		LogManager.Flush();
	}

	private unsafe static void WriteInterProcessMessagesGameModuleInitialized()
	{
		if (_gameDataModuleInitializationState == 2)
		{
			Logger.Info("Outgoing message: GameModuleInitialized");
			byte[] array = OutgoingMessageBuffer.Get(5);
			fixed (byte* ptr = array)
			{
				*ptr = 0;
				*(int*)(ptr + 1) = 0;
			}
			_slaverPipe.Write(array, 0, 5);
			SetGameDataModuleInitializationState(3);
		}
	}

	private static void WriteInterProcessMessagesNotifications()
	{
		lock (NotificationCollectionsLock)
		{
			if (_notificationCollections.Count <= 0)
			{
				return;
			}
			List<NotificationCollection> notificationCollections = _notificationCollections;
			List<NotificationCollection> writingNotificationCollections = _writingNotificationCollections;
			_writingNotificationCollections = notificationCollections;
			_notificationCollections = writingNotificationCollections;
			_notificationCollections.Clear();
		}
		foreach (NotificationCollection writingNotificationCollection in _writingNotificationCollections)
		{
			int dataSize;
			byte[] buf = CreateNotificationCollectionData(writingNotificationCollection, out dataSize);
			_slaverPipe.Write(buf, 0, dataSize);
			writingNotificationCollection.DataPool.CopyTo((IPipe)_slaverPipe);
		}
		_writingNotificationCollections.Clear();
	}

	private unsafe static byte[] CreateNotificationCollectionData(NotificationCollection collection, out int dataSize)
	{
		int count = collection.Notifications.Count;
		int num = 4 + sizeof(Notification) * count + 4;
		int num2 = num + collection.DataPool.RawDataSize;
		dataSize = 5 + num;
		byte[] array = OutgoingMessageBuffer.Get(dataSize);
		fixed (byte* ptr = array)
		{
			byte* ptr2 = ptr;
			*ptr2 = 1;
			ptr2++;
			*(int*)ptr2 = num2;
			ptr2 += 4;
			*(int*)ptr2 = count;
			ptr2 += 4;
			for (int i = 0; i < count; i++)
			{
				*(Notification*)ptr2 = collection.Notifications[i];
				ptr2 += sizeof(Notification);
			}
			*(int*)ptr2 = collection.DataPool.RawDataSize;
		}
		return array;
	}

	private static void WriteInterProcessMessagesErrorMessages()
	{
		byte[] buf;
		int dataSize;
		lock (ErrorMessages)
		{
			if (ErrorMessages.Count <= 0)
			{
				return;
			}
			buf = CreateErrorMessagesData(out dataSize);
			ErrorMessages.Clear();
		}
		Logger.Info("Outgoing message: ErrorMessages");
		_slaverPipe?.Write(buf, 0, dataSize);
	}

	private static void WriteInterProcessMessagesWarningMessages()
	{
		byte[] buf;
		int dataSize;
		lock (WarningMessages)
		{
			if (WarningMessages.Count <= 0)
			{
				return;
			}
			buf = CreateWarningMessagesData(out dataSize);
			WarningMessages.Clear();
		}
		Logger.Info("Outgoing message: WarningMessage");
		_slaverPipe?.Write(buf, 0, dataSize);
	}

	private unsafe static byte[] CreateErrorMessagesData(out int dataSize)
	{
		int count = ErrorMessages.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			int num2 = 2 * ErrorMessages[i].Length;
			if (num2 > 65000)
			{
				num2 = 65000;
			}
			num += 2 + num2;
		}
		dataSize = 5 + num;
		byte[] array = OutgoingMessageBuffer.Get(dataSize);
		fixed (byte* ptr = array)
		{
			byte* ptr2 = ptr;
			*ptr2 = 2;
			ptr2++;
			*(int*)ptr2 = num;
			ptr2 += 4;
			for (int j = 0; j < count; j++)
			{
				string text = ErrorMessages[j];
				int num3 = text.Length;
				int num4 = 2 * num3;
				if (num4 > 65000)
				{
					num4 = 65000;
					num3 = 32500;
				}
				*(ushort*)ptr2 = (ushort)num4;
				ptr2 += 2;
				fixed (char* ptr3 = text)
				{
					for (int k = 0; k < num3; k++)
					{
						((short*)ptr2)[k] = (short)ptr3[k];
					}
				}
				ptr2 += num4;
			}
		}
		return array;
	}

	private unsafe static byte[] CreateWarningMessagesData(out int dataSize)
	{
		int count = WarningMessages.Count;
		int num = 0;
		for (int i = 0; i < count; i++)
		{
			int num2 = 2 * WarningMessages[i].Length;
			if (num2 > 65536)
			{
				num2 = 65536;
			}
			num += 2 + num2;
		}
		dataSize = 5 + num;
		byte[] array = OutgoingMessageBuffer.Get(dataSize);
		fixed (byte* ptr = array)
		{
			byte* ptr2 = ptr;
			*ptr2 = 3;
			ptr2++;
			*(int*)ptr2 = num;
			ptr2 += 4;
			for (int j = 0; j < count; j++)
			{
				string text = WarningMessages[j];
				int num3 = text.Length;
				int num4 = 2 * num3;
				if (num4 > 65536)
				{
					num4 = 65536;
					num3 = 32768;
				}
				*(ushort*)ptr2 = (ushort)num4;
				ptr2 += 2;
				fixed (char* ptr3 = text)
				{
					for (int k = 0; k < num3; k++)
					{
						((short*)ptr2)[k] = (short)ptr3[k];
					}
				}
				ptr2 += num4;
			}
		}
		return array;
	}

	private unsafe static bool WriteInterProcessMessagesDisconnect()
	{
		if (!_shouldDisconnect)
		{
			return false;
		}
		Logger.Info("Outgoing message: Disconnect");
		try
		{
			byte[] array = OutgoingMessageBuffer.Get(5);
			fixed (byte* ptr = array)
			{
				*ptr = 4;
				*(int*)(ptr + 1) = 0;
			}
			_slaverPipe?.Write(array, 0, 5);
		}
		catch (Exception ex)
		{
			Logger.Error<Exception>(ex);
		}
		return true;
	}

	private static byte[] GetRawDataFromSocket(int size)
	{
		byte[] array = IncomingMessageBuffer.Get(size);
		int num;
		for (int i = 0; i < size; i += num)
		{
			num = _slaverPipe.Read(array, i, size - i);
		}
		return array;
	}

	private unsafe static void GetUnmanagedValuesFromSocket<T1>(out T1 item1) where T1 : unmanaged
	{
		int num = sizeof(T1);
		byte[] array = IncomingMessageBuffer.Get(num);
		int num2;
		for (int i = 0; i < num; i += num2)
		{
			num2 = _slaverPipe.Read(array, i, num - i);
		}
		fixed (byte* ptr = array)
		{
			item1 = *(T1*)ptr;
		}
	}

	private unsafe static void GetUnmanagedValuesFromSocket<T1, T2>(out T1 item1, out T2 item2) where T1 : unmanaged where T2 : unmanaged
	{
		int num = sizeof(T1) + sizeof(T2);
		byte[] array = IncomingMessageBuffer.Get(num);
		int num2;
		for (int i = 0; i < num; i += num2)
		{
			num2 = _slaverPipe.Read(array, i, num - i);
		}
		fixed (byte* ptr = array)
		{
			item1 = *(T1*)ptr;
			item2 = *(T2*)(ptr + sizeof(T1));
		}
	}
}
