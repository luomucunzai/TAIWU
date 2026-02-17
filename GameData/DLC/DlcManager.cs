using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.Domains.TaiwuEvent;
using NLog;

namespace GameData.DLC
{
	// Token: 0x020008D8 RID: 2264
	public static class DlcManager
	{
		// Token: 0x0600813E RID: 33086 RVA: 0x004D069C File Offset: 0x004CE89C
		internal static void OnLoadedArchiveData(Dictionary<ulong, DlcEntryWrapper> dlcEntries)
		{
			DlcManager._dlcEntries = dlcEntries;
			DataContext context = DataContextManager.GetCurrentThreadDataContext();
			foreach (DlcId dlcId in DlcManager._enabledDlcIds)
			{
				DlcEntryWrapper entryWrapper;
				IDlcEntry dlcEntry = DlcManager._dlcEntries.TryGetValue(dlcId.AppId, out entryWrapper) ? entryWrapper.GetDlcEntry() : DlcManager.CreateDlcEntry(dlcId);
				if (dlcEntry != null)
				{
					dlcEntry.OnLoadedArchiveData();
				}
				DomainManager.Extra.SetDlcEntry(context, dlcId, dlcEntry);
			}
		}

		// Token: 0x0600813F RID: 33087 RVA: 0x004D0738 File Offset: 0x004CE938
		internal static void InitializeOnEnterNewWorld(Dictionary<ulong, DlcEntryWrapper> dlcEntries)
		{
			DlcManager._dlcEntries = dlcEntries;
			foreach (DlcId dlcId in DlcManager._enabledDlcIds)
			{
				IDlcEntry entry = DlcManager.CreateDlcEntry(dlcId);
				if (entry != null)
				{
					entry.OnEnterNewWorld();
				}
				DlcManager._dlcEntries.Add(dlcId.AppId, new DlcEntryWrapper(dlcId, entry));
			}
		}

		// Token: 0x06008140 RID: 33088 RVA: 0x004D07BC File Offset: 0x004CE9BC
		internal static void FixAbnormalArchiveData(DataContext context)
		{
			foreach (DlcId dlcId in DlcManager._enabledDlcIds)
			{
				IDlcEntry entry = DlcManager._dlcEntries[dlcId.AppId].GetDlcEntry();
				if (entry != null)
				{
					entry.FixAbnormalArchiveData(context);
				}
				DomainManager.Extra.SetDlcEntry(context, dlcId, entry);
			}
		}

		// Token: 0x06008141 RID: 33089 RVA: 0x004D0840 File Offset: 0x004CEA40
		internal static void OnPostAdvanceMonth(DataContext context)
		{
			foreach (DlcId dlcId in DlcManager._enabledDlcIds)
			{
				IDlcEntry entry = DlcManager._dlcEntries[dlcId.AppId].GetDlcEntry();
				if (entry != null)
				{
					entry.OnPostAdvanceMonth(context);
				}
				DomainManager.Extra.SetDlcEntry(context, dlcId, entry);
			}
		}

		// Token: 0x06008142 RID: 33090 RVA: 0x004D08C4 File Offset: 0x004CEAC4
		internal static void OnCrossArchive(DataContext context, CrossArchiveGameData crossArchiveGameData)
		{
			foreach (DlcId dlcId in DlcManager._enabledDlcIds)
			{
				IDlcEntry entry = DlcManager._dlcEntries[dlcId.AppId].GetDlcEntry();
				IDlcEntry entryBeforeCrossArchive = crossArchiveGameData.DlcEntries[dlcId.AppId].GetDlcEntry();
				if (entry != null)
				{
					entry.OnCrossArchive(context, entryBeforeCrossArchive);
				}
				DomainManager.Extra.SetDlcEntry(context, dlcId, entry);
			}
			crossArchiveGameData.DlcEntries = null;
		}

		// Token: 0x06008143 RID: 33091 RVA: 0x004D0964 File Offset: 0x004CEB64
		public static IDlcEntry CreateDlcEntry(DlcId dlcId)
		{
			ulong appId = dlcId.AppId;
			ulong num = appId;
			IDlcEntry result;
			if (num != 2764950UL)
			{
				result = null;
			}
			else
			{
				result = new FiveLoongDlcEntry();
			}
			return result;
		}

		// Token: 0x06008144 RID: 33092 RVA: 0x004D0994 File Offset: 0x004CEB94
		public static bool CheckDlcEntryType(DlcId dlcId, Type type)
		{
			return true;
		}

		// Token: 0x06008145 RID: 33093 RVA: 0x004D09A8 File Offset: 0x004CEBA8
		public static void SetDldInfoList(List<DlcInfo> dlcInfoList)
		{
			DlcManager._dlcInfoList = new List<DlcInfo>();
			DlcManager._enabledDlcIds = new List<DlcId>();
			foreach (DlcInfo dlcInfo in dlcInfoList)
			{
				DlcManager._dlcInfoList.Add(new DlcInfo(dlcInfo.DlcId.AppId, dlcInfo.DlcId.Version, dlcInfo.IsInstalled, dlcInfo.EventDirectory));
				bool isInstalled = dlcInfo.IsInstalled;
				if (isInstalled)
				{
					DlcManager._enabledDlcIds.Add(dlcInfo.DlcId);
				}
			}
		}

		// Token: 0x06008146 RID: 33094 RVA: 0x004D0A58 File Offset: 0x004CEC58
		public static bool IsDlcInstalled(ulong appId)
		{
			foreach (DlcInfo dlcInfo in DlcManager._dlcInfoList)
			{
				bool flag = dlcInfo.DlcId.AppId == appId && dlcInfo.IsInstalled;
				if (flag)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06008147 RID: 33095 RVA: 0x004D0ACC File Offset: 0x004CECCC
		public static List<DlcId> GetAllInstalledDlcIds()
		{
			return DlcManager._enabledDlcIds.ToList<DlcId>();
		}

		// Token: 0x06008148 RID: 33096 RVA: 0x004D0AE8 File Offset: 0x004CECE8
		public static void LoadAllEventPackages()
		{
			LogManager.GetCurrentClassLogger().Info("Start loading Dlc Events:");
			foreach (DlcInfo dlcInfo in DlcManager._dlcInfoList)
			{
				Logger currentClassLogger = LogManager.GetCurrentClassLogger();
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(13, 2);
				defaultInterpolatedStringHandler.AppendLiteral("DLC: ");
				defaultInterpolatedStringHandler.AppendFormatted<DlcId>(dlcInfo.DlcId);
				defaultInterpolatedStringHandler.AppendLiteral(", Path: ");
				defaultInterpolatedStringHandler.AppendFormatted(dlcInfo.EventDirectory);
				currentClassLogger.Info(defaultInterpolatedStringHandler.ToStringAndClear());
				bool flag = !Directory.Exists(dlcInfo.EventDirectory);
				if (!flag)
				{
					EventPackagePathInfo pathInfo = new EventPackagePathInfo(dlcInfo.EventDirectory);
					foreach (string eventDll in Directory.GetFiles(pathInfo.DllDirPath))
					{
						string packageName = Path.GetFileNameWithoutExtension(eventDll);
						DomainManager.TaiwuEvent.LoadEventPackageFromAssembly(packageName, pathInfo, dlcInfo.DlcId.ToString(), null);
					}
				}
			}
		}

		// Token: 0x04002386 RID: 9094
		private static List<DlcInfo> _dlcInfoList;

		// Token: 0x04002387 RID: 9095
		private static List<DlcId> _enabledDlcIds;

		// Token: 0x04002388 RID: 9096
		private static Dictionary<ulong, DlcEntryWrapper> _dlcEntries;
	}
}
