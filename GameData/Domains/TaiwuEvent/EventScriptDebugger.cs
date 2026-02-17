using System;
using System.IO;
using System.Text;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using Config;
using GameData.GameDataBridge;
using NLog;
using NLog.Config;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007C RID: 124
	public class EventScriptDebugger
	{
		// Token: 0x060016C8 RID: 5832 RVA: 0x00153778 File Offset: 0x00151978
		public EventScriptDebugger(EventScriptRuntime runtime)
		{
			this._runtime = runtime;
			string outputPath = Path.Combine(Program.BaseDataDir, "Logs", "EventScripts");
			this.InitLogger(outputPath);
		}

		// Token: 0x060016C9 RID: 5833 RVA: 0x001537BC File Offset: 0x001519BC
		private void InitLogger(string outputPath)
		{
			LogFactory factory = new LogFactory();
			LoggingConfiguration config = new LoggingConfiguration(factory);
			StringBuilder sbLayout = new StringBuilder();
			sbLayout.Append("${date}");
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
				ArchiveFileName = outputPath + "/EventScript_{#}.log",
				ArchiveNumbering = ArchiveNumberingMode.Date,
				ArchiveDateFormat = "yyyy-MM-dd_HH_mm_ss",
				ArchiveEvery = FileArchivePeriod.Year,
				MaxArchiveFiles = 9,
				FileName = outputPath + "/EventScript_${cached:${date:format=yyyy-MM-dd_HH_mm_ss}}.log",
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
			factory.Configuration = config;
			this._logger = factory.GetLogger("EventScriptDebugger");
		}

		// Token: 0x060016CA RID: 5834 RVA: 0x00153968 File Offset: 0x00151B68
		public void LogError(string message)
		{
			this._logger.Warn(message);
		}

		// Token: 0x060016CB RID: 5835 RVA: 0x00153978 File Offset: 0x00151B78
		public void LogMessage(string message)
		{
			this._logger.Info(message);
		}

		// Token: 0x060016CC RID: 5836 RVA: 0x00153988 File Offset: 0x00151B88
		public void LogScriptInfo(EventScriptId scriptId)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[{0}]", scriptId.ToString());
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016CD RID: 5837 RVA: 0x001539C8 File Offset: 0x00151BC8
		public void LogConditionListReturn(EventScriptId scriptId, bool value)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[Return {0}] {1}", scriptId.ToString(), value.ToString());
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016CE RID: 5838 RVA: 0x00153A1C File Offset: 0x00151C1C
		public void LogInstruction(int index, int funcId)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[Line ({0})]", index.ToString());
			this.BuildFunctionNameText(this._logStringBuilder, funcId);
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016CF RID: 5839 RVA: 0x00153A6E File Offset: 0x00151C6E
		public void LogArguments(int index, ASTNode[] args)
		{
			this._logStringBuilder.Clear();
			this.BuildArgumentOutputText(this._logStringBuilder, args);
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016D0 RID: 5840 RVA: 0x00153AA0 File Offset: 0x00151CA0
		public void LogInstructionWithArgs(int index, EventInstructionBase instruction)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[Line ({0})]", index.ToString());
			this.BuildFunctionNameText(this._logStringBuilder, instruction.FunctionId);
			this._logStringBuilder.Append(':');
			this._logStringBuilder.Append(' ');
			this.BuildArgumentOutputText(this._logStringBuilder, instruction.Instruction.Parameters);
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016D1 RID: 5841 RVA: 0x00153B2B File Offset: 0x00151D2B
		public void LogConditionCheck(int index, EventInstructionBase instruction, bool value)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[Return ({0})] {1}", index.ToString(), value.ToString());
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016D2 RID: 5842 RVA: 0x00153B6C File Offset: 0x00151D6C
		public void LogInstructionReturn(string assignToVar, ValueInfo valueInfo)
		{
			this._logStringBuilder.Clear();
			this._logStringBuilder.AppendFormat("[Return]", Array.Empty<object>());
			this.BuildReturnValueText(this._logStringBuilder, assignToVar, valueInfo);
			this._logger.Info<StringBuilder>(this._logStringBuilder);
		}

		// Token: 0x060016D3 RID: 5843 RVA: 0x00153BC0 File Offset: 0x00151DC0
		private void BuildFunctionNameText(StringBuilder builder, int funcId)
		{
			string funcName = EventFunction.Instance[funcId].Name;
			builder.Append(funcName);
		}

		// Token: 0x060016D4 RID: 5844 RVA: 0x00153BE8 File Offset: 0x00151DE8
		private void BuildArgumentOutputText(StringBuilder builder, ASTNode[] args)
		{
			bool flag = args == null;
			if (!flag)
			{
				for (int i = 0; i < args.Length; i++)
				{
					string value = args[i].GetAnyValueAsString(this._runtime.Evaluator);
					builder.Append(value);
					bool flag2 = i + 1 < args.Length;
					if (flag2)
					{
						builder.Append(',');
						builder.Append(' ');
					}
				}
			}
		}

		// Token: 0x060016D5 RID: 5845 RVA: 0x00153C54 File Offset: 0x00151E54
		private void BuildReturnValueText(StringBuilder builder, string assignToVar, ValueInfo valueInfo)
		{
			string value = this._runtime.Evaluator.GetValueAsString(valueInfo);
			builder.Append(assignToVar);
			builder.Append(" = ");
			builder.Append(value);
		}

		// Token: 0x04000489 RID: 1161
		private StringBuilder _logStringBuilder = new StringBuilder();

		// Token: 0x0400048A RID: 1162
		private readonly EventScriptRuntime _runtime;

		// Token: 0x0400048B RID: 1163
		private Logger _logger;
	}
}
