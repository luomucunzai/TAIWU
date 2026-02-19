using System;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;
using Steamworks;

namespace GameData.Steamworks;

internal static class SteamManager
{
	private static readonly AppId_t _appId = new AppId_t(838350u);

	private static bool _initialized = false;

	private static CSteamID _steamId;

	private static SteamAPIWarningMessageHook_t _steamApiWarningMessageHook;

	private static Logger _logger;

	public static void Initialize()
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Expected O, but got Unknown
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Invalid comparison between Unknown and I4
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		_logger = LogManager.GetCurrentClassLogger();
		if (!Packsize.Test())
		{
			_logger.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
		}
		if (!DllCheck.Test())
		{
			_logger.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
		}
		_initialized = SteamAPI.Init();
		if (!_initialized)
		{
			throw new Exception("Failed to initialize steam API.");
		}
		_steamApiWarningMessageHook = new SteamAPIWarningMessageHook_t(LogWarn);
		SteamClient.SetWarningMessageHook(_steamApiWarningMessageHook);
		_steamId = SteamUser.GetSteamID();
		EUserHasLicenseForAppResult val = SteamUser.UserHasLicenseForApp(_steamId, _appId);
		if ((int)val == 1)
		{
			throw new Exception($"Current user {_steamId} does not have license for appid {_appId}, license status: {val}.");
		}
		_logger.Info("Verifying steam user successful! steam id " + ((object)Unsafe.As<CSteamID, CSteamID>(ref _steamId)/*cast due to .constrained prefix*/).ToString());
	}

	public static void Update()
	{
		if (_initialized)
		{
			SteamAPI.RunCallbacks();
		}
	}

	public static void AddAchievement(string achievementName)
	{
		SteamUserStats.SetAchievement(achievementName);
	}

	public static bool IsDlcInstalled(uint dlcAppId)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return _initialized && SteamApps.BIsDlcInstalled(new AppId_t(dlcAppId));
	}

	public static void LogWarn(int nSeverity, StringBuilder pchDebugText)
	{
		_logger.Warn<StringBuilder>(pchDebugText);
	}

	public static void UnInitialize()
	{
		if (_initialized)
		{
			SteamAPI.Shutdown();
		}
	}
}
