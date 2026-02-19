using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GameData.Common;
using GameData.DLC.FiveLoong;
using GameData.Domains;
using GameData.Domains.Global;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.MonthlyEvent;
using NLog;

namespace GameData.DLC;

public static class DlcManager
{
	private static List<DlcInfo> _dlcInfoList;

	private static List<DlcId> _enabledDlcIds;

	private static Dictionary<ulong, DlcEntryWrapper> _dlcEntries;

	internal static void OnLoadedArchiveData(Dictionary<ulong, DlcEntryWrapper> dlcEntries)
	{
		_dlcEntries = dlcEntries;
		DataContext currentThreadDataContext = DataContextManager.GetCurrentThreadDataContext();
		foreach (DlcId enabledDlcId in _enabledDlcIds)
		{
			DlcEntryWrapper value;
			IDlcEntry dlcEntry = (_dlcEntries.TryGetValue(enabledDlcId.AppId, out value) ? value.GetDlcEntry() : CreateDlcEntry(enabledDlcId));
			dlcEntry?.OnLoadedArchiveData();
			DomainManager.Extra.SetDlcEntry(currentThreadDataContext, enabledDlcId, dlcEntry);
		}
	}

	internal static void InitializeOnEnterNewWorld(Dictionary<ulong, DlcEntryWrapper> dlcEntries)
	{
		_dlcEntries = dlcEntries;
		foreach (DlcId enabledDlcId in _enabledDlcIds)
		{
			IDlcEntry dlcEntry = CreateDlcEntry(enabledDlcId);
			dlcEntry?.OnEnterNewWorld();
			_dlcEntries.Add(enabledDlcId.AppId, new DlcEntryWrapper(enabledDlcId, dlcEntry));
		}
	}

	internal static void FixAbnormalArchiveData(DataContext context)
	{
		foreach (DlcId enabledDlcId in _enabledDlcIds)
		{
			IDlcEntry dlcEntry = _dlcEntries[enabledDlcId.AppId].GetDlcEntry();
			dlcEntry?.FixAbnormalArchiveData(context);
			DomainManager.Extra.SetDlcEntry(context, enabledDlcId, dlcEntry);
		}
	}

	internal static void OnPostAdvanceMonth(DataContext context)
	{
		foreach (DlcId enabledDlcId in _enabledDlcIds)
		{
			IDlcEntry dlcEntry = _dlcEntries[enabledDlcId.AppId].GetDlcEntry();
			dlcEntry?.OnPostAdvanceMonth(context);
			DomainManager.Extra.SetDlcEntry(context, enabledDlcId, dlcEntry);
		}
	}

	internal static void AddMonthlyEventHappyNewYear2026()
	{
		if (IsDlcInstalled(4395170uL))
		{
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddDLCYearOfHorseCloth();
		}
	}

	internal static void OnCrossArchive(DataContext context, CrossArchiveGameData crossArchiveGameData)
	{
		foreach (DlcId enabledDlcId in _enabledDlcIds)
		{
			IDlcEntry dlcEntry = _dlcEntries[enabledDlcId.AppId].GetDlcEntry();
			IDlcEntry dlcEntry2 = crossArchiveGameData.DlcEntries[enabledDlcId.AppId].GetDlcEntry();
			dlcEntry?.OnCrossArchive(context, dlcEntry2);
			DomainManager.Extra.SetDlcEntry(context, enabledDlcId, dlcEntry);
		}
		crossArchiveGameData.DlcEntries = null;
	}

	public static IDlcEntry CreateDlcEntry(DlcId dlcId)
	{
		ulong appId = dlcId.AppId;
		ulong num = appId;
		if (num == 2764950)
		{
			return new FiveLoongDlcEntry();
		}
		return null;
	}

	public static bool CheckDlcEntryType(DlcId dlcId, Type type)
	{
		return true;
	}

	public static void SetDldInfoList(List<DlcInfo> dlcInfoList)
	{
		_dlcInfoList = new List<DlcInfo>();
		_enabledDlcIds = new List<DlcId>();
		foreach (DlcInfo dlcInfo in dlcInfoList)
		{
			_dlcInfoList.Add(new DlcInfo(dlcInfo.DlcId.AppId, dlcInfo.DlcId.Version, dlcInfo.IsInstalled, dlcInfo.EventDirectory));
			if (dlcInfo.IsInstalled)
			{
				_enabledDlcIds.Add(dlcInfo.DlcId);
			}
		}
	}

	public static bool IsDlcInstalled(ulong appId)
	{
		foreach (DlcInfo dlcInfo in _dlcInfoList)
		{
			if (dlcInfo.DlcId.AppId == appId && dlcInfo.IsInstalled)
			{
				return true;
			}
		}
		return false;
	}

	public static List<DlcId> GetAllInstalledDlcIds()
	{
		return _enabledDlcIds.ToList();
	}

	public static void LoadAllEventPackages()
	{
		LogManager.GetCurrentClassLogger().Info("Start loading Dlc Events:");
		foreach (DlcInfo dlcInfo in _dlcInfoList)
		{
			LogManager.GetCurrentClassLogger().Info($"DLC: {dlcInfo.DlcId}, Path: {dlcInfo.EventDirectory}");
			if (Directory.Exists(dlcInfo.EventDirectory))
			{
				EventPackagePathInfo pathInfo = new EventPackagePathInfo(dlcInfo.EventDirectory);
				string[] files = Directory.GetFiles(pathInfo.DllDirPath);
				foreach (string path in files)
				{
					string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(path);
					DomainManager.TaiwuEvent.LoadEventPackageFromAssembly(fileNameWithoutExtension, pathInfo, dlcInfo.DlcId.ToString());
				}
			}
		}
	}
}
