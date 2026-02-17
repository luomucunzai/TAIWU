using System;
using System.Runtime.CompilerServices;
using System.Text;
using NLog;
using Steamworks;

namespace GameData.Steamworks
{
	// Token: 0x0200001A RID: 26
	internal static class SteamManager
	{
		// Token: 0x060000AC RID: 172 RVA: 0x00051C90 File Offset: 0x0004FE90
		public static void Initialize()
		{
			SteamManager._logger = LogManager.GetCurrentClassLogger();
			bool flag = !Packsize.Test();
			if (flag)
			{
				SteamManager._logger.Error("[Steamworks.NET] Packsize Test returned false, the wrong version of Steamworks.NET is being run in this platform.");
			}
			bool flag2 = !DllCheck.Test();
			if (flag2)
			{
				SteamManager._logger.Error("[Steamworks.NET] DllCheck Test returned false, One or more of the Steamworks binaries seems to be the wrong version.");
			}
			SteamManager._initialized = SteamAPI.Init();
			bool flag3 = !SteamManager._initialized;
			if (flag3)
			{
				throw new Exception("Failed to initialize steam API.");
			}
			SteamManager._steamApiWarningMessageHook = new SteamAPIWarningMessageHook_t(SteamManager.LogWarn);
			SteamClient.SetWarningMessageHook(SteamManager._steamApiWarningMessageHook);
			SteamManager._steamId = SteamUser.GetSteamID();
			EUserHasLicenseForAppResult hasLicense = SteamUser.UserHasLicenseForApp(SteamManager._steamId, SteamManager._appId);
			bool flag4 = hasLicense == EUserHasLicenseForAppResult.k_EUserHasLicenseResultDoesNotHaveLicense;
			if (flag4)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(65, 3);
				defaultInterpolatedStringHandler.AppendLiteral("Current user ");
				defaultInterpolatedStringHandler.AppendFormatted<CSteamID>(SteamManager._steamId);
				defaultInterpolatedStringHandler.AppendLiteral(" does not have license for appid ");
				defaultInterpolatedStringHandler.AppendFormatted<AppId_t>(SteamManager._appId);
				defaultInterpolatedStringHandler.AppendLiteral(", license status: ");
				defaultInterpolatedStringHandler.AppendFormatted<EUserHasLicenseForAppResult>(hasLicense);
				defaultInterpolatedStringHandler.AppendLiteral(".");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			SteamManager._logger.Info("Verifying steam user successful! steam id " + SteamManager._steamId.ToString());
		}

		// Token: 0x060000AD RID: 173 RVA: 0x00051DD8 File Offset: 0x0004FFD8
		public static void Update()
		{
			bool flag = !SteamManager._initialized;
			if (!flag)
			{
				SteamAPI.RunCallbacks();
			}
		}

		// Token: 0x060000AE RID: 174 RVA: 0x00051DFA File Offset: 0x0004FFFA
		public static void AddAchievement(string achievementName)
		{
			SteamUserStats.SetAchievement(achievementName);
		}

		// Token: 0x060000AF RID: 175 RVA: 0x00051E04 File Offset: 0x00050004
		public static bool IsDlcInstalled(uint dlcAppId)
		{
			return SteamManager._initialized && SteamApps.BIsDlcInstalled(new AppId_t(dlcAppId));
		}

		// Token: 0x060000B0 RID: 176 RVA: 0x00051E2B File Offset: 0x0005002B
		public static void LogWarn(int nSeverity, StringBuilder pchDebugText)
		{
			SteamManager._logger.Warn<StringBuilder>(pchDebugText);
		}

		// Token: 0x060000B1 RID: 177 RVA: 0x00051E3C File Offset: 0x0005003C
		public static void UnInitialize()
		{
			bool flag = !SteamManager._initialized;
			if (!flag)
			{
				SteamAPI.Shutdown();
			}
		}

		// Token: 0x04000073 RID: 115
		private static readonly AppId_t _appId = new AppId_t(838350U);

		// Token: 0x04000074 RID: 116
		private static bool _initialized = false;

		// Token: 0x04000075 RID: 117
		private static CSteamID _steamId;

		// Token: 0x04000076 RID: 118
		private static SteamAPIWarningMessageHook_t _steamApiWarningMessageHook;

		// Token: 0x04000077 RID: 119
		private static Logger _logger;
	}
}
