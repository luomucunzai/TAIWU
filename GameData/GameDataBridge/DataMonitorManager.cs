using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.Domains;
using GameData.Utilities;

namespace GameData.GameDataBridge
{
	// Token: 0x0200001D RID: 29
	public class DataMonitorManager
	{
		// Token: 0x06000CD7 RID: 3287 RVA: 0x000DBE70 File Offset: 0x000DA070
		public void MonitorData(DataUid uid)
		{
			this._monitoredData.Add(uid);
			this._initialMonitoring.Add(uid);
			BaseGameDataDomain domain = DomainManager.Domains[(int)uid.DomainId];
			domain.OnMonitorData(uid.DataId, uid.SubId0, uid.SubId1, true);
		}

		// Token: 0x06000CD8 RID: 3288 RVA: 0x000DBEC0 File Offset: 0x000DA0C0
		public void UnMonitorData(DataUid uid)
		{
			this._monitoredData.Remove(uid);
			this._initialMonitoring.Remove(uid);
			BaseGameDataDomain domain = DomainManager.Domains[(int)uid.DomainId];
			domain.OnMonitorData(uid.DataId, uid.SubId0, uid.SubId1, false);
		}

		// Token: 0x06000CD9 RID: 3289 RVA: 0x000DBF10 File Offset: 0x000DA110
		public void AddPostModificationHandler(DataUid uid, string handlerKey, Action<DataContext, DataUid> handler)
		{
			bool flag = !this._dataWithPostModificationHandlers.Contains(uid);
			if (flag)
			{
				this._dataWithInitialHandler.Add(uid);
			}
			this._dataWithAllHandlersRemoved.Remove(uid);
			BaseGameDataDomain domain = DomainManager.Domains[(int)uid.DomainId];
			domain.AddPostModificationHandler(uid, handlerKey, handler);
		}

		// Token: 0x06000CDA RID: 3290 RVA: 0x000DBF64 File Offset: 0x000DA164
		public void RemovePostModificationHandler(DataUid uid, string handlerKey)
		{
			BaseGameDataDomain domain = DomainManager.Domains[(int)uid.DomainId];
			bool flag = domain.RemovePostModificationHandler(uid, handlerKey);
			if (flag)
			{
				this._dataWithAllHandlersRemoved.Add(uid);
				this._dataWithInitialHandler.Remove(uid);
			}
		}

		// Token: 0x06000CDB RID: 3291 RVA: 0x000DBFA8 File Offset: 0x000DA1A8
		public void Clear()
		{
			foreach (DataUid uid in this._monitoredData)
			{
				BaseGameDataDomain domain = DomainManager.Domains[(int)uid.DomainId];
				domain.OnMonitorData(uid.DataId, uid.SubId0, uid.SubId1, false);
			}
			foreach (DataUid uid2 in this._dataWithPostModificationHandlers)
			{
				BaseGameDataDomain domain2 = DomainManager.Domains[(int)uid2.DomainId];
				domain2.RemovePostModificationHandlers(uid2);
			}
			foreach (DataUid uid3 in this._dataWithInitialHandler)
			{
				BaseGameDataDomain domain3 = DomainManager.Domains[(int)uid3.DomainId];
				domain3.RemovePostModificationHandlers(uid3);
			}
			this._monitoredData.Clear();
			this._initialMonitoring.Clear();
			this._dataWithPostModificationHandlers.Clear();
			this._dataWithInitialHandler.Clear();
			this._dataWithAllHandlersRemoved.Clear();
		}

		// Token: 0x06000CDC RID: 3292 RVA: 0x000DC110 File Offset: 0x000DA310
		public void CheckMonitoredData()
		{
			NotificationCollection pendingNotifications = GameDataBridge.GetPendingNotifications();
			RawDataPool dataPool = pendingNotifications.DataPool;
			List<Notification> notifications = pendingNotifications.Notifications;
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			this._handledData.Clear();
			bool flag = this._dataWithAllHandlersRemoved.Count > 0;
			if (flag)
			{
				foreach (DataUid uid in this._dataWithAllHandlersRemoved)
				{
					this._dataWithPostModificationHandlers.Remove(uid);
				}
				this._dataWithAllHandlersRemoved.Clear();
			}
			bool flag2 = this._dataWithInitialHandler.Count > 0;
			if (flag2)
			{
				foreach (DataUid uid2 in this._dataWithInitialHandler)
				{
					this._dataWithPostModificationHandlers.Add(uid2);
				}
				this._dataWithInitialHandler.Clear();
			}
			foreach (DataUid uid3 in this._dataWithPostModificationHandlers)
			{
				BaseGameDataDomain domain = DomainManager.Domains[(int)uid3.DomainId];
				bool flag3 = !domain.IsModifiedWrapper(uid3.DataId, uid3.SubId0, uid3.SubId1);
				if (!flag3)
				{
					this._handledData.Add(uid3);
				}
			}
			bool flag4 = this._initialMonitoring.Count <= 0;
			if (flag4)
			{
				foreach (DataUid uid4 in this._monitoredData)
				{
					BaseGameDataDomain domain2 = DomainManager.Domains[(int)uid4.DomainId];
					int offset = domain2.CheckModified(uid4.DataId, uid4.SubId0, uid4.SubId1, dataPool);
					bool flag5 = offset >= 0;
					if (flag5)
					{
						notifications.Add(Notification.CreateDataModification(uid4, offset));
					}
				}
			}
			else
			{
				foreach (DataUid uid5 in this._initialMonitoring)
				{
					BaseGameDataDomain domain3 = DomainManager.Domains[(int)uid5.DomainId];
					int offset2 = domain3.GetData(uid5.DataId, uid5.SubId0, uid5.SubId1, dataPool, true);
					bool flag6 = offset2 < 0;
					if (flag6)
					{
						string tag = "CheckMonitoredData";
						DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
						defaultInterpolatedStringHandler.AppendLiteral("Failed to get initial monitoring data ");
						defaultInterpolatedStringHandler.AppendFormatted<DataUid>(uid5);
						defaultInterpolatedStringHandler.AppendLiteral(".");
						AdaptableLog.TagWarning(tag, defaultInterpolatedStringHandler.ToStringAndClear(), false);
					}
					else
					{
						notifications.Add(Notification.CreateDataModification(uid5, offset2));
					}
				}
				foreach (DataUid uid6 in this._monitoredData)
				{
					bool flag7 = this._initialMonitoring.Contains(uid6);
					if (!flag7)
					{
						BaseGameDataDomain domain4 = DomainManager.Domains[(int)uid6.DomainId];
						int offset3 = domain4.CheckModified(uid6.DataId, uid6.SubId0, uid6.SubId1, dataPool);
						bool flag8 = offset3 >= 0;
						if (flag8)
						{
							notifications.Add(Notification.CreateDataModification(uid6, offset3));
						}
					}
				}
				this._initialMonitoring.Clear();
			}
			foreach (DataUid uid7 in this._handledData)
			{
				BaseGameDataDomain domain5 = DomainManager.Domains[(int)uid7.DomainId];
				domain5.ResetModifiedWrapper(uid7.DataId, uid7.SubId0, uid7.SubId1);
				domain5.ExecutePostModificationHandlers(context, uid7);
			}
		}

		// Token: 0x0400007C RID: 124
		private readonly HashSet<DataUid> _monitoredData = new HashSet<DataUid>();

		// Token: 0x0400007D RID: 125
		private readonly HashSet<DataUid> _initialMonitoring = new HashSet<DataUid>();

		// Token: 0x0400007E RID: 126
		private readonly HashSet<DataUid> _dataWithPostModificationHandlers = new HashSet<DataUid>();

		// Token: 0x0400007F RID: 127
		private readonly List<DataUid> _handledData = new List<DataUid>();

		// Token: 0x04000080 RID: 128
		private readonly HashSet<DataUid> _dataWithAllHandlersRemoved = new HashSet<DataUid>();

		// Token: 0x04000081 RID: 129
		private readonly HashSet<DataUid> _dataWithInitialHandler = new HashSet<DataUid>();
	}
}
