using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains;
using GameData.Utilities;

namespace GameData.GameDataBridge;

public class DataMonitorManager
{
	private readonly HashSet<DataUid> _monitoredData = new HashSet<DataUid>();

	private readonly HashSet<DataUid> _initialMonitoring = new HashSet<DataUid>();

	private readonly HashSet<DataUid> _dataWithPostModificationHandlers = new HashSet<DataUid>();

	private readonly List<DataUid> _handledData = new List<DataUid>();

	private readonly HashSet<DataUid> _dataWithAllHandlersRemoved = new HashSet<DataUid>();

	private readonly HashSet<DataUid> _dataWithInitialHandler = new HashSet<DataUid>();

	public void MonitorData(DataUid uid)
	{
		_monitoredData.Add(uid);
		_initialMonitoring.Add(uid);
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[uid.DomainId];
		baseGameDataDomain.OnMonitorData(uid.DataId, uid.SubId0, uid.SubId1, monitoring: true);
	}

	public void UnMonitorData(DataUid uid)
	{
		_monitoredData.Remove(uid);
		_initialMonitoring.Remove(uid);
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[uid.DomainId];
		baseGameDataDomain.OnMonitorData(uid.DataId, uid.SubId0, uid.SubId1, monitoring: false);
	}

	public void AddPostModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
	{
		if (!_dataWithPostModificationHandlers.Contains(uid))
		{
			_dataWithInitialHandler.Add(uid);
		}
		_dataWithAllHandlersRemoved.Remove(uid);
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[uid.DomainId];
		baseGameDataDomain.AddPostModificationHandler(uid, handlerKey, handler);
	}

	public void RemovePostModificationHandler(DataUid uid, string handlerKey)
	{
		BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[uid.DomainId];
		if (baseGameDataDomain.RemovePostModificationHandler(uid, handlerKey))
		{
			_dataWithAllHandlersRemoved.Add(uid);
			_dataWithInitialHandler.Remove(uid);
		}
	}

	public void Clear()
	{
		foreach (DataUid monitoredDatum in _monitoredData)
		{
			BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[monitoredDatum.DomainId];
			baseGameDataDomain.OnMonitorData(monitoredDatum.DataId, monitoredDatum.SubId0, monitoredDatum.SubId1, monitoring: false);
		}
		foreach (DataUid dataWithPostModificationHandler in _dataWithPostModificationHandlers)
		{
			BaseGameDataDomain baseGameDataDomain2 = DomainManager.Domains[dataWithPostModificationHandler.DomainId];
			baseGameDataDomain2.RemovePostModificationHandlers(dataWithPostModificationHandler);
		}
		foreach (DataUid item in _dataWithInitialHandler)
		{
			BaseGameDataDomain baseGameDataDomain3 = DomainManager.Domains[item.DomainId];
			baseGameDataDomain3.RemovePostModificationHandlers(item);
		}
		_monitoredData.Clear();
		_initialMonitoring.Clear();
		_dataWithPostModificationHandlers.Clear();
		_dataWithInitialHandler.Clear();
		_dataWithAllHandlersRemoved.Clear();
	}

	public void CheckMonitoredData()
	{
		NotificationCollection pendingNotifications = GameDataBridge.GetPendingNotifications();
		RawDataPool dataPool = pendingNotifications.DataPool;
		List<Notification> notifications = pendingNotifications.Notifications;
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		_handledData.Clear();
		if (_dataWithAllHandlersRemoved.Count > 0)
		{
			foreach (DataUid item in _dataWithAllHandlersRemoved)
			{
				_dataWithPostModificationHandlers.Remove(item);
			}
			_dataWithAllHandlersRemoved.Clear();
		}
		if (_dataWithInitialHandler.Count > 0)
		{
			foreach (DataUid item2 in _dataWithInitialHandler)
			{
				_dataWithPostModificationHandlers.Add(item2);
			}
			_dataWithInitialHandler.Clear();
		}
		foreach (DataUid dataWithPostModificationHandler in _dataWithPostModificationHandlers)
		{
			BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataWithPostModificationHandler.DomainId];
			if (baseGameDataDomain.IsModifiedWrapper(dataWithPostModificationHandler.DataId, dataWithPostModificationHandler.SubId0, dataWithPostModificationHandler.SubId1))
			{
				_handledData.Add(dataWithPostModificationHandler);
			}
		}
		if (_initialMonitoring.Count <= 0)
		{
			foreach (DataUid monitoredDatum in _monitoredData)
			{
				BaseGameDataDomain baseGameDataDomain2 = DomainManager.Domains[monitoredDatum.DomainId];
				int num = baseGameDataDomain2.CheckModified(monitoredDatum.DataId, monitoredDatum.SubId0, monitoredDatum.SubId1, dataPool);
				if (num >= 0)
				{
					notifications.Add(Notification.CreateDataModification(monitoredDatum, num));
				}
			}
		}
		else
		{
			foreach (DataUid item3 in _initialMonitoring)
			{
				BaseGameDataDomain baseGameDataDomain3 = DomainManager.Domains[item3.DomainId];
				int data = baseGameDataDomain3.GetData(item3.DataId, item3.SubId0, item3.SubId1, dataPool, resetModified: true);
				if (data < 0)
				{
					AdaptableLog.TagWarning("CheckMonitoredData", $"Failed to get initial monitoring data {item3}.");
				}
				else
				{
					notifications.Add(Notification.CreateDataModification(item3, data));
				}
			}
			foreach (DataUid monitoredDatum2 in _monitoredData)
			{
				if (!_initialMonitoring.Contains(monitoredDatum2))
				{
					BaseGameDataDomain baseGameDataDomain4 = DomainManager.Domains[monitoredDatum2.DomainId];
					int num2 = baseGameDataDomain4.CheckModified(monitoredDatum2.DataId, monitoredDatum2.SubId0, monitoredDatum2.SubId1, dataPool);
					if (num2 >= 0)
					{
						notifications.Add(Notification.CreateDataModification(monitoredDatum2, num2));
					}
				}
			}
			_initialMonitoring.Clear();
		}
		foreach (DataUid handledDatum in _handledData)
		{
			BaseGameDataDomain baseGameDataDomain5 = DomainManager.Domains[handledDatum.DomainId];
			baseGameDataDomain5.ResetModifiedWrapper(handledDatum.DataId, handledDatum.SubId0, handledDatum.SubId1);
			baseGameDataDomain5.ExecutePostModificationHandlers(currentThreadDataContext, handledDatum);
		}
	}
}
