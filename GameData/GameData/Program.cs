using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using GameData.ArchiveData;
using GameData.Common;
using GameData.Common.WorkerThread;
using GameData.Domains;
using GameData.Domains.Map;
using GameData.Domains.Mod;
using GameData.GameDataBridge;
using GameData.Serializer;
using GameData.Steamworks;
using GameData.Utilities;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace GameData;

internal static class Program
{
	private static readonly Logger Logger = LogManager.GetCurrentClassLogger();

	public static string BaseDataDir { get; private set; }

	public static bool IsTestBranch { get; private set; }

	internal static void Main(string[] args)
	{
		try
		{
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
			Thread.CurrentThread.Name = "Main";
			DataContextManager.RegisterMainThread();
			Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
			InitializeCommandLineArgs(args);
			BaseDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
			AppDomain.CurrentDomain.AssemblyResolve += ModDomain.ResolveTaiwuModdingLibPath;
			string loggingDir = Path.Combine(BaseDataDir, "Logs");
			InitializeLogging(loggingDir);
			Logger logger = LogManager.GetLogger("AdaptableLog");
			AdaptableLog.Initialize((Action<string>)logger.Info, (Action<string>)logger.Warn, (Action<string>)logger.Error);
			ExternalDataBridge.Initialize(new GameContext());
			DomainManager.Global.ReloadAllConfigData();
			GameData.ArchiveData.Common.Initialize(BaseDataDir, loggingDir, IsTestBranch);
			ObjectPoolManager.Initialize();
			CoordinateGenericInitializer.Initialize();
			GameData.Serializer.Serializer.Initialize();
			WorkerThreadManager.Initialize();
			GameData.GameDataBridge.GameDataBridge.Initialize();
			SteamManager.Initialize();
			Logger.Info("GameData module initialized.");
			DomainManager.Global.OnInitializeGameDataModule();
			DomainManager.Global.OnLoadWorld();
			LogHandle.OnAppendMessage += GameData.GameDataBridge.GameDataBridge.AppendWarningMessage;
			GameData.GameDataBridge.GameDataBridge.RunMainLoop();
		}
		catch (Exception ex)
		{
			Logger.Error<Exception>(ex);
			Location lastGetBlockDataPosition_Debug = DomainManager.Map.LastGetBlockDataPosition_Debug;
			AdaptableLog.Info($"try get block data from frontend: [{lastGetBlockDataPosition_Debug.AreaId}, {lastGetBlockDataPosition_Debug.BlockId}]");
		}
		finally
		{
			Logger.Info("GameData module is about to exit.");
			GameData.GameDataBridge.GameDataBridge.UnInitialize();
			GameData.ArchiveData.Common.UnInitialize();
			SteamManager.UnInitialize();
			LogManager.Shutdown();
		}
	}

	private static void InitializeCommandLineArgs(string[] args)
	{
		foreach (string text in args)
		{
			string text2 = text;
			string text3 = text2;
			if (text3 == "--test-branch")
			{
				IsTestBranch = true;
			}
		}
	}

	private static void InitializeLogging(string loggingDir)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Expected O, but got Unknown
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Expected O, but got Unknown
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Expected O, but got Unknown
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_011f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_016c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0174: Unknown result type (might be due to invalid IL or missing references)
		//IL_017c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Expected O, but got Unknown
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_018b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0194: Unknown result type (might be due to invalid IL or missing references)
		//IL_019e: Expected O, but got Unknown
		ConfigurationItemFactory.Default = new ConfigurationItemFactory(new Assembly[1] { typeof(ILogger).GetTypeInfo().Assembly });
		LoggingConfiguration val = new LoggingConfiguration();
		StringBuilder stringBuilder = new StringBuilder("${longdate}");
		stringBuilder.Append("|${level:uppercase=true}");
		stringBuilder.Append("|${threadname}");
		stringBuilder.Append("|${logger}");
		stringBuilder.Append("|${message}");
		string text = stringBuilder.ToString();
		ColoredConsoleTarget val2 = new ColoredConsoleTarget("console")
		{
			Layout = Layout.op_Implicit(text),
			Encoding = Encoding.UTF8,
			DetectConsoleAvailable = true
		};
		val.AddRule(LogLevel.Debug, LogLevel.Fatal, (Target)(object)val2, "*");
		ErrorMessagesTarget errorMessagesTarget = new ErrorMessagesTarget("errorMessages");
		((TargetWithLayout)errorMessagesTarget).Layout = Layout.op_Implicit(text);
		ErrorMessagesTarget errorMessagesTarget2 = errorMessagesTarget;
		val.AddRule(LogLevel.Error, LogLevel.Fatal, (Target)(object)errorMessagesTarget2, "*");
		FileTarget wrappedTarget = new FileTarget("file")
		{
			Layout = Layout.op_Implicit(text),
			Encoding = Encoding.UTF8,
			ArchiveFileName = Layout.op_Implicit(loggingDir + "/GameData_{#}.log"),
			ArchiveNumbering = (ArchiveNumberingMode)2,
			ArchiveDateFormat = "yyyy-MM-dd_HH_mm_ss",
			ArchiveEvery = (FileArchivePeriod)1,
			MaxArchiveFiles = 9,
			FileName = Layout.op_Implicit(loggingDir + "/GameData_${cached:${date:format=yyyy-MM-dd_HH_mm_ss}}.log"),
			KeepFileOpen = true,
			OpenFileCacheTimeout = 30,
			ConcurrentWrites = false,
			CleanupFileName = false,
			AutoFlush = true
		};
		AsyncTargetWrapper val3 = new AsyncTargetWrapper
		{
			WrappedTarget = (Target)(object)wrappedTarget,
			OverflowAction = (AsyncTargetWrapperOverflowAction)1
		};
		val.AddRule(LogLevel.Debug, LogLevel.Fatal, (Target)(object)val3, "*");
		LogManager.Configuration = val;
	}
}
