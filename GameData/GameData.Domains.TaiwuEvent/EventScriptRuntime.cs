using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using Config;
using Config.EventConfig;
using GameData.Common;
using GameData.Domains.Map;
using GameData.Domains.TaiwuEvent.Decompiler;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.FunctionDefinition;
using GameData.Domains.TaiwuEvent.ObjectInitializer;
using GameData.Domains.TaiwuEvent.ValueSelector;
using GameData.Serializer;
using GameData.Utilities;
using NLog;

namespace GameData.Domains.TaiwuEvent;

public class EventScriptRuntime : IInterpreterContext<EventScriptRuntime>
{
	public readonly DataContext Context;

	public Instruction<EventScriptRuntime> ExecutingInstruction;

	private readonly Dictionary<ulong, IEventScriptDecompiler> _decompilers;

	private readonly Interpreter<EventScriptRuntime> _interpreter;

	private readonly Dictionary<int, string> _funcIdToNameMap;

	private readonly Stack<ScriptExecutionInstance> _executionInstances;

	private readonly Dictionary<string, EventScript> _globalScripts;

	private static readonly LocalObjectPool<ScriptExecutionInstance> ObjectPool = new LocalObjectPool<ScriptExecutionInstance>(4, 32);

	private EventArgBox _tmpArgBox;

	public bool MovingNext;

	public bool IsPaused;

	public EventScriptRuntimeSettings Settings;

	public readonly EventScriptDebugger Debugger;

	private readonly Logger _logger = LogManager.GetCurrentClassLogger();

	public bool RecordingConditionHints;

	private List<OptionAvailableConditionInfo> _recordedConditionInfos;

	public Evaluator Evaluator { get; }

	public ScriptExecutionInstance Current
	{
		get
		{
			ScriptExecutionInstance result;
			return _executionInstances.TryPeek(out result) ? result : null;
		}
	}

	public EventArgBox ArgBox => _tmpArgBox ?? Current?.ArgBox;

	public ICollection<int> ImplementedFunctionIds => _funcIdToNameMap.Keys;

	public EventScriptRuntime(DataContext context, bool enableDebugging = false)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Expected O, but got Unknown
		Context = context;
		Evaluator = new Evaluator();
		if (enableDebugging)
		{
			Debugger = new EventScriptDebugger(this);
		}
		Evaluator.RegisterValueSelector(new SelectValueFunc(SelectValueFromArgBox));
		Evaluator.RegisterValueSelector((IValueSelector)(object)new GlobalValueSelector());
		Evaluator.RegisterValueSelector(typeof(MapBlockData), (IFieldValueSelector)(object)new MapBlockDataValueSelector());
		Evaluator.RegisterObjectInitializer("ItemTemplate", (IObjectInitializer)(object)new ItemTemplateInitializer());
		RegisterValueConvertersFromType<ValueConverters>();
		_decompilers = new Dictionary<ulong, IEventScriptDecompiler>();
		RegisterDecompilers();
		_interpreter = new Interpreter<EventScriptRuntime>(true);
		_funcIdToNameMap = new Dictionary<int, string>();
		_executionInstances = new Stack<ScriptExecutionInstance>();
		_globalScripts = new Dictionary<string, EventScript>();
		RegisterFunctionsFromType<EventConditions>();
		RegisterFunctionsFromType<BasicFunctions>();
		RegisterFunctionsFromType<CharacterFunctions>();
		RegisterFunctionsFromType<CombatSkillFunctions>();
		RegisterFunctionsFromType<ItemFunctions>();
		RegisterFunctionsFromType<MapFunctions>();
		RegisterFunctionsFromType<MerchantFunctions>();
		RegisterFunctionsFromType<OrganizationFunctions>();
		RegisterFunctionsFromType<TaiwuFunctions>();
		RegisterFunctionsFromType<WorldFunctions>();
		RegisterFunctionsFromType<AdventureFunctions>();
		RegisterFunctionsFromType<InterfaceFunctions>();
		RegisterFunctionsFromType<SectMainStoryInternalFunctions>();
	}

	public void LoadSettings(string path)
	{
		Settings = null;
		if (!File.Exists(path))
		{
			return;
		}
		try
		{
			string text = File.ReadAllText(path);
			CommonObjectSerializer.Deserialize<EventScriptRuntimeSettings>(text, ref Settings, (MarshalFormat)2);
			if (Settings.LogScriptTypes.Length != EventScriptType.Instance.Count)
			{
				Array.Resize(ref Settings.LogScriptTypes, EventScriptType.Instance.Count);
			}
		}
		catch (Exception value)
		{
			Settings = null;
			_logger.Warn($"Unable to load runtime settings.\n{value}");
		}
	}

	private void RegisterDecompilers()
	{
		EventScriptDecompiler_0_0_2_0 eventScriptDecompiler_0_0_2_ = new EventScriptDecompiler_0_0_2_0();
		_decompilers.Add(VersionUtils.VersionStringToUlong(eventScriptDecompiler_0_0_2_.Version), eventScriptDecompiler_0_0_2_);
	}

	private void RegisterValueConvertersFromType<T>()
	{
		//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cc: Expected O, but got Unknown
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Expected O, but got Unknown
		Type typeFromHandle = typeof(T);
		Type typeFromHandle2 = typeof(ValueConverterAttribute);
		MethodInfo[] methods = typeFromHandle.GetMethods((BindingFlags)(-1));
		Type typeFromHandle3 = typeof(Conversion);
		Type typeFromHandle4 = typeof(Conversion);
		MethodInfo[] array = methods;
		foreach (MethodInfo methodInfo in array)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeFromHandle2, inherit: false);
			if (customAttributes.Length != 0)
			{
				ValueConverterAttribute valueConverterAttribute = (ValueConverterAttribute)customAttributes[0];
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == 2 && parameters[0].ParameterType == typeof(Evaluator) && parameters[1].ParameterType == typeof(ValueInfo))
				{
					Conversion val = (Conversion)methodInfo.CreateDelegate(typeFromHandle3);
					Evaluator.RegisterValueConversion(valueConverterAttribute.SrcType, valueConverterAttribute.DstType, val);
				}
				else if (parameters.Length == 1 && parameters[0].ParameterType == typeof(object))
				{
					Conversion val2 = (Conversion)methodInfo.CreateDelegate(typeFromHandle4);
					Evaluator.RegisterValueConversion(valueConverterAttribute.SrcType, valueConverterAttribute.DstType, val2);
				}
			}
		}
	}

	public void RegisterFunctionsFromType<T>()
	{
		Type typeFromHandle = typeof(T);
		Type typeFromHandle2 = typeof(EventFunctionAttribute);
		MethodInfo[] methods = typeFromHandle.GetMethods((BindingFlags)(-1));
		Type typeFromHandle3 = typeof(Function<EventScriptRuntime>);
		MethodInfo[] array = methods;
		foreach (MethodInfo methodInfo in array)
		{
			object[] customAttributes = methodInfo.GetCustomAttributes(typeFromHandle2, inherit: false);
			if (customAttributes.Length != 0)
			{
				EventFunctionAttribute eventFunctionAttribute = (EventFunctionAttribute)customAttributes[0];
				_funcIdToNameMap.Add(eventFunctionAttribute.Id, methodInfo.Name);
				ParameterInfo[] parameters = methodInfo.GetParameters();
				if (parameters.Length == 2 && parameters[0].ParameterType == typeof(EventScriptRuntime) && parameters[1].ParameterType == typeof(ASTNode[]))
				{
					Function<EventScriptRuntime> val = (Function<EventScriptRuntime>)(object)methodInfo.CreateDelegate(typeFromHandle3);
					_interpreter.AddFunctionDefinition(methodInfo.Name, val);
				}
				else
				{
					_interpreter.AddFunctionDefinition(methodInfo.Name, methodInfo);
				}
			}
		}
	}

	public void Execute(string instStr)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		ValueInfo val = _interpreter.Execute(this, instStr);
		Evaluator.RemoveTopValue(val);
	}

	public T Execute<T>(string instStr)
	{
		return _interpreter.Execute<T>(this, instStr);
	}

	public T Evaluate<T>(string expr)
	{
		return _interpreter.EvaluateExpression<T>(this, expr);
	}

	public EventInstruction CreateInst(int functionId, int indentAmount, string assignToVar, string args)
	{
		string text = _funcIdToNameMap[functionId];
		try
		{
			Instruction<EventScriptRuntime> instruction = _interpreter.BuildInstruction(text, args, (string)null);
			return new EventInstruction(functionId, indentAmount, assignToVar, instruction);
		}
		catch (Exception innerException)
		{
			throw new Exception($"Failed to build instruction.\n\n {text}: {args}\n", innerException);
		}
	}

	public EventInstruction CreateInst(int functionId, int indentAmount, string assignToVar, string[] args)
	{
		string text = _funcIdToNameMap[functionId];
		try
		{
			Instruction<EventScriptRuntime> instruction = _interpreter.BuildInstruction(text, args);
			return new EventInstruction(functionId, indentAmount, assignToVar, instruction);
		}
		catch (Exception innerException)
		{
			throw new Exception($"Failed to build instruction.\n\n {text}: {string.Join(',', args)}\n", innerException);
		}
	}

	public EventInstruction TestCreateInst(int indentAmount, string assignToVar, string instructionStr)
	{
		try
		{
			Instruction<EventScriptRuntime> instruction = _interpreter.BuildInstruction(instructionStr);
			return new EventInstruction(-1, indentAmount, assignToVar, instruction);
		}
		catch (Exception innerException)
		{
			throw new Exception("Failed to build instruction " + instructionStr + ".", innerException);
		}
	}

	public EventCondition CreateCondition(int functionId, int indent, bool reverse, string args)
	{
		string text = _funcIdToNameMap[functionId];
		try
		{
			Instruction<EventScriptRuntime> instruction = _interpreter.BuildInstruction(text, args, (string)null);
			return new EventCondition(functionId, instruction, reverse, indent);
		}
		catch (Exception innerException)
		{
			throw new Exception($"Failed to build instruction.\n\n {text}: {args}\n", innerException);
		}
	}

	public EventCondition CreateCondition(int functionId, int indent, bool reverse, string[] args)
	{
		string text = _funcIdToNameMap[functionId];
		try
		{
			Instruction<EventScriptRuntime> instruction = _interpreter.BuildInstruction(text, args);
			return new EventCondition(functionId, instruction, reverse, indent);
		}
		catch (Exception innerException)
		{
			throw new Exception($"Failed to build instruction.\n\n {text}: {string.Join(',', args)}\n", innerException);
		}
	}

	public void LoadGlobalScripts(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		_globalScripts.Clear();
		if (!directoryInfo.Exists)
		{
			return;
		}
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			using FileStream input = File.OpenRead(fileInfo.FullName);
			using BinaryReader binaryReader = new BinaryReader(input);
			ulong version = binaryReader.ReadUInt64();
			IEventScriptDecompiler decompiler = GetDecompiler(version);
			EventScriptId id = decompiler.DecompileScriptId(this, binaryReader);
			EventScript value = decompiler.DecompileEventScript(this, id, binaryReader);
			_globalScripts.Add(id.Guid, value);
		}
	}

	public void LoadPackageScripts(EventPackage package, string packageScriptPath)
	{
		if (!File.Exists(packageScriptPath))
		{
			return;
		}
		using FileStream input = File.OpenRead(packageScriptPath);
		using BinaryReader binaryReader = new BinaryReader(input);
		ulong num = binaryReader.ReadUInt64();
		IEventScriptDecompiler decompiler = GetDecompiler(num);
		if (decompiler == null)
		{
			throw new Exception($"Loading package with Invalid decompiler version {VersionUtils.VersionUlongToString(num)} at {packageScriptPath}.");
		}
		Dictionary<EventScriptId, EventScriptBase> dictionary = decompiler.DecompileEventScriptPackage(this, binaryReader);
		List<TaiwuEventItem> allEvents = package.GetAllEvents();
		foreach (TaiwuEventItem item in allEvents)
		{
			string guid = item.Guid.ToString();
			item.Script = (EventScript)dictionary.GetValueOrDefault(new EventScriptId(1, guid));
			item.Conditions = (EventConditionList)dictionary.GetValueOrDefault(new EventScriptId(2, guid));
			TaiwuEventOption[] eventOptions = item.EventOptions;
			foreach (TaiwuEventOption taiwuEventOption in eventOptions)
			{
				string optionGuid = taiwuEventOption.OptionGuid;
				taiwuEventOption.Script = (EventScript)dictionary.GetValueOrDefault(new EventScriptId(3, guid, optionGuid));
				taiwuEventOption.AvailableConditions = (EventConditionList)dictionary.GetValueOrDefault(new EventScriptId(4, guid, optionGuid));
				taiwuEventOption.VisibleConditions = (EventConditionList)dictionary.GetValueOrDefault(new EventScriptId(5, guid, optionGuid));
			}
		}
	}

	private IEventScriptDecompiler GetDecompiler(ulong version)
	{
		return _decompilers.GetValueOrDefault(version);
	}

	public void ExecuteScript(EventScript script, EventArgBox argBox)
	{
		ScriptExecutionInstance scriptExecutionInstance = ObjectPool.Get();
		_executionInstances.Push(scriptExecutionInstance);
		scriptExecutionInstance.ExecuteScript(this, script, argBox);
	}

	public void ExecuteGlobalScript(string scriptGuid, EventArgBox argBox)
	{
		if (_globalScripts.TryGetValue(scriptGuid, out var value))
		{
			ExecuteScript(value, argBox);
		}
	}

	public bool CheckConditionList(EventConditionList conditionList, EventArgBox argBox)
	{
		if (conditionList == null || conditionList.Conditions == null)
		{
			return true;
		}
		_tmpArgBox = argBox;
		bool flag = LogScriptExecution(conditionList.Id);
		if (flag)
		{
			Debugger.LogScriptInfo(conditionList.Id);
		}
		int currIndex = 0;
		bool flag2 = CheckAndCondition(conditionList, ref currIndex, flag);
		if (flag)
		{
			Debugger.LogConditionListReturn(conditionList.Id, flag2);
		}
		_tmpArgBox = null;
		return flag2;
	}

	private bool CheckCondition(EventConditionList conditionList, EventCondition condition, ref int currIndex, bool logExecution)
	{
		if (logExecution)
		{
			Debugger.LogInstructionWithArgs(currIndex, condition);
		}
		int index = currIndex;
		bool flag;
		switch (condition.FunctionId)
		{
		case 109:
			currIndex++;
			flag = condition.Reverse != CheckAndCondition(conditionList, ref currIndex, logExecution);
			while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent > condition.Indent)
			{
				currIndex++;
			}
			break;
		case 110:
			currIndex++;
			flag = condition.Reverse != CheckOrConditions(conditionList, ref currIndex, logExecution);
			while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent > condition.Indent)
			{
				currIndex++;
			}
			break;
		case 5:
			throw new Exception("Cannot handle End here");
		default:
			flag = condition.Check(this);
			break;
		}
		if (logExecution)
		{
			Debugger.LogConditionCheck(index, condition, flag);
		}
		return flag;
	}

	private bool CheckAndCondition(EventConditionList conditionList, ref int currIndex, bool logExecution)
	{
		bool flag = true;
		while (currIndex < conditionList.Conditions.Length)
		{
			EventCondition eventCondition = conditionList.Conditions[currIndex];
			if (eventCondition.FunctionId == 5)
			{
				return flag;
			}
			try
			{
				flag &= CheckCondition(conditionList, eventCondition, ref currIndex, logExecution);
				if (!flag && !RecordingConditionHints)
				{
					while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent >= eventCondition.Indent)
					{
						currIndex++;
					}
					return false;
				}
			}
			catch (Exception innerException)
			{
				throw new TaiwuEventConditionException("Failed to check condition", conditionList.Id, currIndex, eventCondition, innerException);
			}
			currIndex++;
		}
		return flag;
	}

	private bool CheckOrConditions(EventConditionList conditionList, ref int currIndex, bool logExecution)
	{
		bool flag = false;
		while (currIndex < conditionList.Conditions.Length)
		{
			EventCondition eventCondition = conditionList.Conditions[currIndex];
			if (eventCondition.FunctionId == 5)
			{
				return flag;
			}
			try
			{
				flag |= CheckCondition(conditionList, eventCondition, ref currIndex, logExecution);
				if (flag && !RecordingConditionHints)
				{
					while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent >= eventCondition.Indent)
					{
						currIndex++;
					}
					return true;
				}
			}
			catch (Exception innerException)
			{
				throw new TaiwuEventConditionException("Failed to check condition", conditionList.Id, currIndex, eventCondition, innerException);
			}
			currIndex++;
		}
		return flag;
	}

	public bool LogScriptExecution(EventScriptId scriptId)
	{
		if (Settings == null || Debugger == null)
		{
			return false;
		}
		if (Settings.LogMonitoredScriptsOnly && !Settings.MonitoredScripts.Contains(scriptId))
		{
			return false;
		}
		return Settings.LogScriptTypes[scriptId.Type];
	}

	public void Update()
	{
		ScriptExecutionInstance result;
		while (_executionInstances.TryPeek(out result))
		{
			result.Update(this);
			if (result.IsRunning)
			{
				break;
			}
			_executionInstances.Pop();
			ObjectPool.Return(result);
		}
	}

	private ValueInfo SelectValueFromArgBox(Evaluator env, string identifier)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (_tmpArgBox != null)
		{
			return _tmpArgBox.SelectValue(Evaluator, identifier);
		}
		if (Current.ArgBox != null)
		{
			return Current.ArgBox.SelectValue(Evaluator, identifier);
		}
		return ValueInfo.Void;
	}

	public void Log(string message)
	{
		_logger.Info(message);
	}

	public void OnExecuteInstruction(Instruction<EventScriptRuntime> instruction, ASTNode[] parameters)
	{
		ExecutingInstruction = instruction;
	}

	public void OnInstructionEvaluated(ValueInfo ret)
	{
	}

	public Instruction<EventScriptRuntime> GetExecutingInstruction()
	{
		return ExecutingInstruction;
	}

	public void StartRecordConditionHints()
	{
		RecordingConditionHints = true;
		if (_recordedConditionInfos == null)
		{
			_recordedConditionInfos = new List<OptionAvailableConditionInfo>();
		}
		_recordedConditionInfos.Clear();
	}

	public List<OptionAvailableConditionInfo> StopRecordConditionHints()
	{
		RecordingConditionHints = false;
		List<OptionAvailableConditionInfo> recordedConditionInfos = _recordedConditionInfos;
		if (recordedConditionInfos == null || recordedConditionInfos.Count <= 0)
		{
			return null;
		}
		List<OptionAvailableConditionInfo> recordedConditionInfos2 = _recordedConditionInfos;
		_recordedConditionInfos = null;
		return recordedConditionInfos2;
	}

	public void RecordConditionHint(int funcId, bool result, params string[] args)
	{
		OptionAvailableConditionInfo item = new OptionAvailableConditionInfo(funcId, result, args);
		_recordedConditionInfos.Add(item);
	}
}
