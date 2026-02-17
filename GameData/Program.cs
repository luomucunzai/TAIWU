using System;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
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
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace GameData
{
	// Token: 0x02000015 RID: 21
	internal static class Program
	{
		// Token: 0x17000018 RID: 24
		// (get) Token: 0x06000075 RID: 117 RVA: 0x00050400 File Offset: 0x0004E600
		// (set) Token: 0x06000076 RID: 118 RVA: 0x00050407 File Offset: 0x0004E607
		public static string BaseDataDir { get; private set; }

		// Token: 0x17000019 RID: 25
		// (get) Token: 0x06000077 RID: 119 RVA: 0x0005040F File Offset: 0x0004E60F
		// (set) Token: 0x06000078 RID: 120 RVA: 0x00050416 File Offset: 0x0004E616
		public static bool IsTestBranch { get; private set; }

		// Token: 0x06000079 RID: 121 RVA: 0x00050420 File Offset: 0x0004E620
		internal static void Main(string[] args)
		{
			try
			{
				Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.CurrentUICulture = CultureInfo.InvariantCulture;
				Thread.CurrentThread.Name = "Main";
				DataContextManager.RegisterMainThread();
				Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
				Program.InitializeCommandLineArgs(args);
				Program.BaseDataDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..");
				AppDomain.CurrentDomain.AssemblyResolve += ModDomain.ResolveTaiwuModdingLibPath;
				string loggingDir = Path.Combine(Program.BaseDataDir, "Logs");
				Program.InitializeLogging(loggingDir);
				Logger adaptableLogger = LogManager.GetLogger("AdaptableLog");
				AdaptableLog.Initialize(new Action<string>(adaptableLogger.Info), new Action<string>(adaptableLogger.Warn), new Action<string>(adaptableLogger.Error));
				ExternalDataBridge.Initialize(new GameContext());
				DomainManager.Global.ReloadAllConfigData();
				Common.Initialize(Program.BaseDataDir, loggingDir, Program.IsTestBranch);
				ObjectPoolManager.Initialize();
				CoordinateGenericInitializer.Initialize();
				Serializer.Initialize();
				WorkerThreadManager.Initialize();
				GameDataBridge.Initialize();
				SteamManager.Initialize();
				Program.Logger.Info("GameData module initialized.");
				DomainManager.Global.OnInitializeGameDataModule();
				DomainManager.Global.OnLoadWorld();
				LogHandle.OnAppendMessage += GameDataBridge.AppendWarningMessage;
				GameDataBridge.RunMainLoop();
			}
			catch (Exception ex)
			{
				Program.Logger.Error<Exception>(ex);
				Location location = DomainManager.Map.LastGetBlockDataPosition_Debug;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 2);
				defaultInterpolatedStringHandler.AppendLiteral("try get block data from frontend: [");
				defaultInterpolatedStringHandler.AppendFormatted<short>(location.AreaId);
				defaultInterpolatedStringHandler.AppendLiteral(", ");
				defaultInterpolatedStringHandler.AppendFormatted<short>(location.BlockId);
				defaultInterpolatedStringHandler.AppendLiteral("]");
				AdaptableLog.Info(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			finally
			{
				Program.Logger.Info("GameData module is about to exit.");
				GameDataBridge.UnInitialize();
				Common.UnInitialize();
				SteamManager.UnInitialize();
				LogManager.Shutdown();
			}
		}

		// Token: 0x0600007A RID: 122 RVA: 0x0005065C File Offset: 0x0004E85C
		private static void InitializeCommandLineArgs(string[] args)
		{
			foreach (string arg in args)
			{
				string text = arg;
				string a = text;
				if (a == "--test-branch")
				{
					Program.IsTestBranch = true;
				}
			}
		}

		// Token: 0x0600007B RID: 123 RVA: 0x000506A0 File Offset: 0x0004E8A0
		private static void InitializeLogging(string loggingDir)
		{
			ConfigurationItemFactory.Default = new ConfigurationItemFactory(new Assembly[]
			{
				typeof(ILogger).GetTypeInfo().Assembly
			});
			LoggingConfiguration config = new LoggingConfiguration();
			StringBuilder sbLayout = new StringBuilder("${longdate}");
			sbLayout.Append("|${level:uppercase=true}");
			sbLayout.Append("|${threadname}");
			sbLayout.Append("|${logger}");
			sbLayout.Append("|${message}");
			string layout = sbLayout.ToString();
			ColoredConsoleTarget logConsole = new ColoredConsoleTarget("console")
			{
				Layout = layout,
				Encoding = Encoding.UTF8,
				DetectConsoleAvailable = true
			};
			config.AddRule(LogLevel.Debug, LogLevel.Fatal, logConsole, "*");
			ErrorMessagesTarget logErrorMessages = new ErrorMessagesTarget("errorMessages")
			{
				Layout = layout
			};
			config.AddRule(LogLevel.Error, LogLevel.Fatal, logErrorMessages, "*");
			FileTarget logFile = new FileTarget("file")
			{
				Layout = layout,
				Encoding = Encoding.UTF8,
				ArchiveFileName = loggingDir + "/GameData_{#}.log",
				ArchiveNumbering = ArchiveNumberingMode.Date,
				ArchiveDateFormat = "yyyy-MM-dd_HH_mm_ss",
				ArchiveEvery = FileArchivePeriod.Year,
				MaxArchiveFiles = 9,
				FileName = loggingDir + "/GameData_${cached:${date:format=yyyy-MM-dd_HH_mm_ss}}.log",
				KeepFileOpen = true,
				OpenFileCacheTimeout = 30,
				ConcurrentWrites = false,
				CleanupFileName = false,
				AutoFlush = true
			};
			AsyncTargetWrapper logFileWrapper = new AsyncTargetWrapper
			{
				WrappedTarget = logFile,
				OverflowAction = AsyncTargetWrapperOverflowAction.Discard
			};
			config.AddRule(LogLevel.Debug, LogLevel.Fatal, logFileWrapper, "*");
			LogManager.Configuration = config;
		}

		// Token: 0x0400006E RID: 110
		private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
	}
}
