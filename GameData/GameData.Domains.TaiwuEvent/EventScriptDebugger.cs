using System.IO;
using System.Text;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using Config;
using GameData.GameDataBridge;
using NLog;
using NLog.Config;
using NLog.Layouts;
using NLog.Targets;
using NLog.Targets.Wrappers;

namespace GameData.Domains.TaiwuEvent;

public class EventScriptDebugger
{
	private StringBuilder _logStringBuilder = new StringBuilder();

	private readonly EventScriptRuntime _runtime;

	private Logger _logger;

	public EventScriptDebugger(EventScriptRuntime runtime)
	{
		_runtime = runtime;
		string outputPath = Path.Combine(Program.BaseDataDir, "Logs", "EventScripts");
		InitLogger(outputPath);
	}

	private void InitLogger(string outputPath)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Expected O, but got Unknown
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Expected O, but got Unknown
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Expected O, but got Unknown
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0114: Unknown result type (might be due to invalid IL or missing references)
		//IL_012b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0133: Unknown result type (might be due to invalid IL or missing references)
		//IL_013c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Expected O, but got Unknown
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_015b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0164: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Expected O, but got Unknown
		LogFactory val = new LogFactory();
		LoggingConfiguration val2 = new LoggingConfiguration(val);
		StringBuilder stringBuilder = new StringBuilder();
		stringBuilder.Append("${date}");
		stringBuilder.Append("|${logger}");
		stringBuilder.Append("|${message}");
		string text = stringBuilder.ToString();
		ColoredConsoleTarget val3 = new ColoredConsoleTarget("console")
		{
			Layout = Layout.op_Implicit(text),
			Encoding = Encoding.UTF8,
			DetectConsoleAvailable = true
		};
		val2.AddRule(LogLevel.Debug, LogLevel.Fatal, (Target)(object)val3, "*");
		ErrorMessagesTarget errorMessagesTarget = new ErrorMessagesTarget("errorMessages");
		((TargetWithLayout)errorMessagesTarget).Layout = Layout.op_Implicit(text);
		ErrorMessagesTarget errorMessagesTarget2 = errorMessagesTarget;
		val2.AddRule(LogLevel.Error, LogLevel.Fatal, (Target)(object)errorMessagesTarget2, "*");
		FileTarget wrappedTarget = new FileTarget("file")
		{
			Layout = Layout.op_Implicit(text),
			Encoding = Encoding.UTF8,
			ArchiveFileName = Layout.op_Implicit(outputPath + "/EventScript_{#}.log"),
			ArchiveNumbering = (ArchiveNumberingMode)2,
			ArchiveDateFormat = "yyyy-MM-dd_HH_mm_ss",
			ArchiveEvery = (FileArchivePeriod)1,
			MaxArchiveFiles = 9,
			FileName = Layout.op_Implicit(outputPath + "/EventScript_${cached:${date:format=yyyy-MM-dd_HH_mm_ss}}.log"),
			KeepFileOpen = true,
			OpenFileCacheTimeout = 30,
			ConcurrentWrites = false,
			CleanupFileName = false,
			AutoFlush = true
		};
		AsyncTargetWrapper val4 = new AsyncTargetWrapper
		{
			WrappedTarget = (Target)(object)wrappedTarget,
			OverflowAction = (AsyncTargetWrapperOverflowAction)1
		};
		val2.AddRule(LogLevel.Debug, LogLevel.Fatal, (Target)(object)val4, "*");
		val.Configuration = val2;
		_logger = val.GetLogger("EventScriptDebugger");
	}

	public void LogError(string message)
	{
		_logger.Warn(message);
	}

	public void LogMessage(string message)
	{
		_logger.Info(message);
	}

	public void LogScriptInfo(EventScriptId scriptId)
	{
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[{0}]", scriptId.ToString());
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogConditionListReturn(EventScriptId scriptId, bool value)
	{
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[Return {0}] {1}", scriptId.ToString(), value.ToString());
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogInstruction(int index, int funcId)
	{
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[Line ({0})]", index.ToString());
		BuildFunctionNameText(_logStringBuilder, funcId);
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogArguments(int index, ASTNode[] args)
	{
		_logStringBuilder.Clear();
		BuildArgumentOutputText(_logStringBuilder, args);
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogInstructionWithArgs(int index, EventInstructionBase instruction)
	{
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[Line ({0})]", index.ToString());
		BuildFunctionNameText(_logStringBuilder, instruction.FunctionId);
		_logStringBuilder.Append(':');
		_logStringBuilder.Append(' ');
		BuildArgumentOutputText(_logStringBuilder, instruction.Instruction.Parameters);
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogConditionCheck(int index, EventInstructionBase instruction, bool value)
	{
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[Return ({0})] {1}", index.ToString(), value.ToString());
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	public void LogInstructionReturn(string assignToVar, ValueInfo valueInfo)
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		_logStringBuilder.Clear();
		_logStringBuilder.AppendFormat("[Return]");
		BuildReturnValueText(_logStringBuilder, assignToVar, valueInfo);
		_logger.Info<StringBuilder>(_logStringBuilder);
	}

	private void BuildFunctionNameText(StringBuilder builder, int funcId)
	{
		string name = EventFunction.Instance[funcId].Name;
		builder.Append(name);
	}

	private void BuildArgumentOutputText(StringBuilder builder, ASTNode[] args)
	{
		if (args == null)
		{
			return;
		}
		for (int i = 0; i < args.Length; i++)
		{
			string anyValueAsString = args[i].GetAnyValueAsString(_runtime.Evaluator);
			builder.Append(anyValueAsString);
			if (i + 1 < args.Length)
			{
				builder.Append(',');
				builder.Append(' ');
			}
		}
	}

	private void BuildReturnValueText(StringBuilder builder, string assignToVar, ValueInfo valueInfo)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		string valueAsString = _runtime.Evaluator.GetValueAsString(valueInfo);
		builder.Append(assignToVar);
		builder.Append(" = ");
		builder.Append(valueAsString);
	}
}
