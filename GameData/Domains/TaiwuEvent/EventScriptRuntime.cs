using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.TaiwuEvent
{
	// Token: 0x0200007D RID: 125
	public class EventScriptRuntime : IInterpreterContext<EventScriptRuntime>
	{
		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x060016D6 RID: 5846 RVA: 0x00153C90 File Offset: 0x00151E90
		public Evaluator Evaluator { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x060016D7 RID: 5847 RVA: 0x00153C98 File Offset: 0x00151E98
		public ScriptExecutionInstance Current
		{
			get
			{
				ScriptExecutionInstance instance;
				return this._executionInstances.TryPeek(out instance) ? instance : null;
			}
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x060016D8 RID: 5848 RVA: 0x00153CB8 File Offset: 0x00151EB8
		public EventArgBox ArgBox
		{
			get
			{
				EventArgBox result;
				if ((result = this._tmpArgBox) == null)
				{
					ScriptExecutionInstance scriptExecutionInstance = this.Current;
					result = ((scriptExecutionInstance != null) ? scriptExecutionInstance.ArgBox : null);
				}
				return result;
			}
		}

		// Token: 0x170000B6 RID: 182
		// (get) Token: 0x060016D9 RID: 5849 RVA: 0x00153CD6 File Offset: 0x00151ED6
		public ICollection<int> ImplementedFunctionIds
		{
			get
			{
				return this._funcIdToNameMap.Keys;
			}
		}

		// Token: 0x060016DA RID: 5850 RVA: 0x00153CE4 File Offset: 0x00151EE4
		public EventScriptRuntime(DataContext context, bool enableDebugging = false)
		{
			this.Context = context;
			this.Evaluator = new Evaluator();
			if (enableDebugging)
			{
				this.Debugger = new EventScriptDebugger(this);
			}
			this.Evaluator.RegisterValueSelector(new IValueSelector.SelectValueFunc(this.SelectValueFromArgBox));
			this.Evaluator.RegisterValueSelector(new GlobalValueSelector());
			this.Evaluator.RegisterValueSelector(typeof(MapBlockData), new MapBlockDataValueSelector());
			this.Evaluator.RegisterObjectInitializer("ItemTemplate", new ItemTemplateInitializer());
			this.RegisterValueConvertersFromType<ValueConverters>();
			this._decompilers = new Dictionary<ulong, IEventScriptDecompiler>();
			this.RegisterDecompilers();
			this._interpreter = new Interpreter<EventScriptRuntime>(true);
			this._funcIdToNameMap = new Dictionary<int, string>();
			this._executionInstances = new Stack<ScriptExecutionInstance>();
			this._globalScripts = new Dictionary<string, EventScript>();
			this.RegisterFunctionsFromType<EventConditions>();
			this.RegisterFunctionsFromType<BasicFunctions>();
			this.RegisterFunctionsFromType<CharacterFunctions>();
			this.RegisterFunctionsFromType<CombatSkillFunctions>();
			this.RegisterFunctionsFromType<ItemFunctions>();
			this.RegisterFunctionsFromType<MapFunctions>();
			this.RegisterFunctionsFromType<MerchantFunctions>();
			this.RegisterFunctionsFromType<OrganizationFunctions>();
			this.RegisterFunctionsFromType<TaiwuFunctions>();
			this.RegisterFunctionsFromType<WorldFunctions>();
			this.RegisterFunctionsFromType<AdventureFunctions>();
			this.RegisterFunctionsFromType<InterfaceFunctions>();
			this.RegisterFunctionsFromType<SectMainStoryInternalFunctions>();
		}

		// Token: 0x060016DB RID: 5851 RVA: 0x00153E24 File Offset: 0x00152024
		public void LoadSettings(string path)
		{
			this.Settings = null;
			bool flag = !File.Exists(path);
			if (!flag)
			{
				try
				{
					string content = File.ReadAllText(path);
					CommonObjectSerializer.Deserialize<EventScriptRuntimeSettings>(content, out this.Settings, CommonObjectSerializer.MarshalFormat.Json);
					bool flag2 = this.Settings.LogScriptTypes.Length != EventScriptType.Instance.Count;
					if (flag2)
					{
						Array.Resize<bool>(ref this.Settings.LogScriptTypes, EventScriptType.Instance.Count);
					}
				}
				catch (Exception e)
				{
					this.Settings = null;
					Logger logger = this._logger;
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unable to load runtime settings.\n");
					defaultInterpolatedStringHandler.AppendFormatted<Exception>(e);
					logger.Warn(defaultInterpolatedStringHandler.ToStringAndClear());
				}
			}
		}

		// Token: 0x060016DC RID: 5852 RVA: 0x00153EF0 File Offset: 0x001520F0
		private void RegisterDecompilers()
		{
			EventScriptDecompiler_0_0_2_0 decompiler = new EventScriptDecompiler_0_0_2_0();
			this._decompilers.Add(VersionUtils.VersionStringToUlong(decompiler.Version), decompiler);
		}

		// Token: 0x060016DD RID: 5853 RVA: 0x00153F1C File Offset: 0x0015211C
		private void RegisterValueConvertersFromType<T>()
		{
			Type type = typeof(T);
			Type attrType = typeof(ValueConverterAttribute);
			MethodInfo[] methods = type.GetMethods((BindingFlags)(-1));
			Type stackTopValueConverter = typeof(StackTopValueConverter.Conversion);
			Type objectValueConverter = typeof(ValueConverter.Conversion);
			foreach (MethodInfo method in methods)
			{
				object[] attributes = method.GetCustomAttributes(attrType, false);
				bool flag = attributes.Length == 0;
				if (!flag)
				{
					ValueConverterAttribute attribute = (ValueConverterAttribute)attributes[0];
					ParameterInfo[] parameters = method.GetParameters();
					bool flag2 = parameters.Length == 2 && parameters[0].ParameterType == typeof(Evaluator) && parameters[1].ParameterType == typeof(ValueInfo);
					if (flag2)
					{
						StackTopValueConverter.Conversion conversion = (StackTopValueConverter.Conversion)method.CreateDelegate(stackTopValueConverter);
						this.Evaluator.RegisterValueConversion(attribute.SrcType, attribute.DstType, conversion);
					}
					else
					{
						bool flag3 = parameters.Length == 1 && parameters[0].ParameterType == typeof(object);
						if (flag3)
						{
							ValueConverter.Conversion conversion2 = (ValueConverter.Conversion)method.CreateDelegate(objectValueConverter);
							this.Evaluator.RegisterValueConversion(attribute.SrcType, attribute.DstType, conversion2);
						}
					}
				}
			}
		}

		// Token: 0x060016DE RID: 5854 RVA: 0x0015407C File Offset: 0x0015227C
		public void RegisterFunctionsFromType<T>()
		{
			Type type = typeof(T);
			Type eventFuncAttr = typeof(EventFunctionAttribute);
			MethodInfo[] methods = type.GetMethods((BindingFlags)(-1));
			Type delegateType = typeof(StandardFunction<EventScriptRuntime>.Function);
			foreach (MethodInfo method in methods)
			{
				object[] attributes = method.GetCustomAttributes(eventFuncAttr, false);
				bool flag = attributes.Length == 0;
				if (!flag)
				{
					EventFunctionAttribute attribute = (EventFunctionAttribute)attributes[0];
					this._funcIdToNameMap.Add(attribute.Id, method.Name);
					ParameterInfo[] parameters = method.GetParameters();
					bool flag2 = parameters.Length == 2 && parameters[0].ParameterType == typeof(EventScriptRuntime) && parameters[1].ParameterType == typeof(ASTNode[]);
					if (flag2)
					{
						StandardFunction<EventScriptRuntime>.Function standardFunc = (StandardFunction<EventScriptRuntime>.Function)method.CreateDelegate(delegateType);
						this._interpreter.AddFunctionDefinition(method.Name, standardFunc);
					}
					else
					{
						this._interpreter.AddFunctionDefinition(method.Name, method);
					}
				}
			}
		}

		// Token: 0x060016DF RID: 5855 RVA: 0x001541A4 File Offset: 0x001523A4
		public void Execute(string instStr)
		{
			ValueInfo retVal = this._interpreter.Execute(this, instStr);
			this.Evaluator.RemoveTopValue(retVal);
		}

		// Token: 0x060016E0 RID: 5856 RVA: 0x001541CD File Offset: 0x001523CD
		public T Execute<T>(string instStr)
		{
			return this._interpreter.Execute<T>(this, instStr);
		}

		// Token: 0x060016E1 RID: 5857 RVA: 0x001541DC File Offset: 0x001523DC
		public T Evaluate<T>(string expr)
		{
			return this._interpreter.EvaluateExpression<T>(this, expr);
		}

		// Token: 0x060016E2 RID: 5858 RVA: 0x001541EC File Offset: 0x001523EC
		public EventInstruction CreateInst(int functionId, int indentAmount, string assignToVar, string args)
		{
			string funcName = this._funcIdToNameMap[functionId];
			EventInstruction result;
			try
			{
				Instruction<EventScriptRuntime> inst = this._interpreter.BuildInstruction(funcName, args, null);
				result = new EventInstruction(functionId, indentAmount, assignToVar, inst);
			}
			catch (Exception e)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to build instruction.\n\n ");
				defaultInterpolatedStringHandler.AppendFormatted(funcName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(args);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear(), e);
			}
			return result;
		}

		// Token: 0x060016E3 RID: 5859 RVA: 0x0015428C File Offset: 0x0015248C
		public EventInstruction CreateInst(int functionId, int indentAmount, string assignToVar, string[] args)
		{
			string funcName = this._funcIdToNameMap[functionId];
			EventInstruction result;
			try
			{
				Instruction<EventScriptRuntime> inst = this._interpreter.BuildInstruction(funcName, args);
				result = new EventInstruction(functionId, indentAmount, assignToVar, inst);
			}
			catch (Exception e)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to build instruction.\n\n ");
				defaultInterpolatedStringHandler.AppendFormatted(funcName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join(',', args));
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear(), e);
			}
			return result;
		}

		// Token: 0x060016E4 RID: 5860 RVA: 0x00154330 File Offset: 0x00152530
		public EventInstruction TestCreateInst(int indentAmount, string assignToVar, string instructionStr)
		{
			EventInstruction result;
			try
			{
				Instruction<EventScriptRuntime> inst = this._interpreter.BuildInstruction(instructionStr);
				result = new EventInstruction(-1, indentAmount, assignToVar, inst);
			}
			catch (Exception e)
			{
				throw new Exception("Failed to build instruction " + instructionStr + ".", e);
			}
			return result;
		}

		// Token: 0x060016E5 RID: 5861 RVA: 0x00154384 File Offset: 0x00152584
		public EventCondition CreateCondition(int functionId, int indent, bool reverse, string args)
		{
			string funcName = this._funcIdToNameMap[functionId];
			EventCondition result;
			try
			{
				Instruction<EventScriptRuntime> inst = this._interpreter.BuildInstruction(funcName, args, null);
				result = new EventCondition(functionId, inst, reverse, indent);
			}
			catch (Exception e)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to build instruction.\n\n ");
				defaultInterpolatedStringHandler.AppendFormatted(funcName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(args);
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear(), e);
			}
			return result;
		}

		// Token: 0x060016E6 RID: 5862 RVA: 0x00154424 File Offset: 0x00152624
		public EventCondition CreateCondition(int functionId, int indent, bool reverse, string[] args)
		{
			string funcName = this._funcIdToNameMap[functionId];
			EventCondition result;
			try
			{
				Instruction<EventScriptRuntime> inst = this._interpreter.BuildInstruction(funcName, args);
				result = new EventCondition(functionId, inst, reverse, indent);
			}
			catch (Exception e)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(34, 2);
				defaultInterpolatedStringHandler.AppendLiteral("Failed to build instruction.\n\n ");
				defaultInterpolatedStringHandler.AppendFormatted(funcName);
				defaultInterpolatedStringHandler.AppendLiteral(": ");
				defaultInterpolatedStringHandler.AppendFormatted(string.Join(',', args));
				defaultInterpolatedStringHandler.AppendLiteral("\n");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear(), e);
			}
			return result;
		}

		// Token: 0x060016E7 RID: 5863 RVA: 0x001544C8 File Offset: 0x001526C8
		public void LoadGlobalScripts(string path)
		{
			DirectoryInfo directoryInfo = new DirectoryInfo(path);
			this._globalScripts.Clear();
			bool flag = !directoryInfo.Exists;
			if (!flag)
			{
				foreach (FileInfo fileInfo in directoryInfo.GetFiles())
				{
					using (FileStream fileStream = File.OpenRead(fileInfo.FullName))
					{
						using (BinaryReader binaryReader = new BinaryReader(fileStream))
						{
							ulong version = binaryReader.ReadUInt64();
							IEventScriptDecompiler decompiler = this.GetDecompiler(version);
							EventScriptId id = decompiler.DecompileScriptId(this, binaryReader);
							EventScript script = decompiler.DecompileEventScript(this, id, binaryReader);
							this._globalScripts.Add(id.Guid, script);
						}
					}
				}
			}
		}

		// Token: 0x060016E8 RID: 5864 RVA: 0x001545B4 File Offset: 0x001527B4
		public void LoadPackageScripts(EventPackage package, string packageScriptPath)
		{
			bool flag = !File.Exists(packageScriptPath);
			if (!flag)
			{
				using (FileStream fileStream = File.OpenRead(packageScriptPath))
				{
					using (BinaryReader binaryReader = new BinaryReader(fileStream))
					{
						ulong version = binaryReader.ReadUInt64();
						IEventScriptDecompiler decompiler = this.GetDecompiler(version);
						bool flag2 = decompiler == null;
						if (flag2)
						{
							DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(53, 2);
							defaultInterpolatedStringHandler.AppendLiteral("Loading package with Invalid decompiler version ");
							defaultInterpolatedStringHandler.AppendFormatted(VersionUtils.VersionUlongToString(version));
							defaultInterpolatedStringHandler.AppendLiteral(" at ");
							defaultInterpolatedStringHandler.AppendFormatted(packageScriptPath);
							defaultInterpolatedStringHandler.AppendLiteral(".");
							throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
						}
						Dictionary<EventScriptId, EventScriptBase> scripts = decompiler.DecompileEventScriptPackage(this, binaryReader);
						List<TaiwuEventItem> events = package.GetAllEvents();
						foreach (TaiwuEventItem eventItem in events)
						{
							string eventGuid = eventItem.Guid.ToString();
							eventItem.Script = (EventScript)scripts.GetValueOrDefault(new EventScriptId(1, eventGuid, null));
							eventItem.Conditions = (EventConditionList)scripts.GetValueOrDefault(new EventScriptId(2, eventGuid, null));
							foreach (TaiwuEventOption option in eventItem.EventOptions)
							{
								string optionGuid = option.OptionGuid;
								option.Script = (EventScript)scripts.GetValueOrDefault(new EventScriptId(3, eventGuid, optionGuid));
								option.AvailableConditions = (EventConditionList)scripts.GetValueOrDefault(new EventScriptId(4, eventGuid, optionGuid));
								option.VisibleConditions = (EventConditionList)scripts.GetValueOrDefault(new EventScriptId(5, eventGuid, optionGuid));
							}
						}
					}
				}
			}
		}

		// Token: 0x060016E9 RID: 5865 RVA: 0x001547D4 File Offset: 0x001529D4
		private IEventScriptDecompiler GetDecompiler(ulong version)
		{
			return this._decompilers.GetValueOrDefault(version);
		}

		// Token: 0x060016EA RID: 5866 RVA: 0x001547F4 File Offset: 0x001529F4
		public void ExecuteScript(EventScript script, EventArgBox argBox)
		{
			ScriptExecutionInstance instance = EventScriptRuntime.ObjectPool.Get();
			this._executionInstances.Push(instance);
			instance.ExecuteScript(this, script, argBox, null);
		}

		// Token: 0x060016EB RID: 5867 RVA: 0x00154828 File Offset: 0x00152A28
		public void ExecuteGlobalScript(string scriptGuid, EventArgBox argBox)
		{
			EventScript script;
			bool flag = this._globalScripts.TryGetValue(scriptGuid, out script);
			if (flag)
			{
				this.ExecuteScript(script, argBox);
			}
		}

		// Token: 0x060016EC RID: 5868 RVA: 0x00154854 File Offset: 0x00152A54
		public bool CheckConditionList(EventConditionList conditionList, EventArgBox argBox)
		{
			bool flag = conditionList == null || conditionList.Conditions == null;
			bool result2;
			if (flag)
			{
				result2 = true;
			}
			else
			{
				this._tmpArgBox = argBox;
				bool logExecution = this.LogScriptExecution(conditionList.Id);
				bool flag2 = logExecution;
				if (flag2)
				{
					this.Debugger.LogScriptInfo(conditionList.Id);
				}
				int index = 0;
				bool result = this.CheckAndCondition(conditionList, ref index, logExecution);
				bool flag3 = logExecution;
				if (flag3)
				{
					this.Debugger.LogConditionListReturn(conditionList.Id, result);
				}
				this._tmpArgBox = null;
				result2 = result;
			}
			return result2;
		}

		// Token: 0x060016ED RID: 5869 RVA: 0x001548DC File Offset: 0x00152ADC
		private bool CheckCondition(EventConditionList conditionList, EventCondition condition, ref int currIndex, bool logExecution)
		{
			if (logExecution)
			{
				this.Debugger.LogInstructionWithArgs(currIndex, condition);
			}
			int initIndex = currIndex;
			int functionId = condition.FunctionId;
			int num = functionId;
			if (num != 5)
			{
				bool ret;
				if (num != 109)
				{
					if (num != 110)
					{
						ret = condition.Check(this);
					}
					else
					{
						currIndex++;
						ret = (condition.Reverse != this.CheckOrConditions(conditionList, ref currIndex, logExecution));
						while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent > condition.Indent)
						{
							currIndex++;
						}
					}
				}
				else
				{
					currIndex++;
					ret = (condition.Reverse != this.CheckAndCondition(conditionList, ref currIndex, logExecution));
					while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent > condition.Indent)
					{
						currIndex++;
					}
				}
				if (logExecution)
				{
					this.Debugger.LogConditionCheck(initIndex, condition, ret);
				}
				return ret;
			}
			throw new Exception("Cannot handle End here");
		}

		// Token: 0x060016EE RID: 5870 RVA: 0x00154A00 File Offset: 0x00152C00
		private bool CheckAndCondition(EventConditionList conditionList, ref int currIndex, bool logExecution)
		{
			bool ret = true;
			while (currIndex < conditionList.Conditions.Length)
			{
				EventCondition condition = conditionList.Conditions[currIndex];
				bool flag = condition.FunctionId == 5;
				if (!flag)
				{
					try
					{
						ret &= this.CheckCondition(conditionList, condition, ref currIndex, logExecution);
						bool flag2 = ret || this.RecordingConditionHints;
						if (!flag2)
						{
							while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent >= condition.Indent)
							{
								currIndex++;
							}
							return false;
						}
					}
					catch (Exception e)
					{
						throw new TaiwuEventConditionException("Failed to check condition", conditionList.Id, currIndex, condition, e);
					}
					currIndex++;
					continue;
				}
				return ret;
			}
			return ret;
		}

		// Token: 0x060016EF RID: 5871 RVA: 0x00154ADC File Offset: 0x00152CDC
		private bool CheckOrConditions(EventConditionList conditionList, ref int currIndex, bool logExecution)
		{
			bool ret = false;
			while (currIndex < conditionList.Conditions.Length)
			{
				EventCondition condition = conditionList.Conditions[currIndex];
				bool flag = condition.FunctionId == 5;
				if (!flag)
				{
					try
					{
						ret |= this.CheckCondition(conditionList, condition, ref currIndex, logExecution);
						bool flag2 = !ret || this.RecordingConditionHints;
						if (!flag2)
						{
							while (currIndex < conditionList.Conditions.Length && conditionList.Conditions[currIndex].Indent >= condition.Indent)
							{
								currIndex++;
							}
							return true;
						}
					}
					catch (Exception e)
					{
						throw new TaiwuEventConditionException("Failed to check condition", conditionList.Id, currIndex, condition, e);
					}
					currIndex++;
					continue;
				}
				return ret;
			}
			return ret;
		}

		// Token: 0x060016F0 RID: 5872 RVA: 0x00154BB8 File Offset: 0x00152DB8
		public bool LogScriptExecution(EventScriptId scriptId)
		{
			bool flag = this.Settings == null || this.Debugger == null;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = this.Settings.LogMonitoredScriptsOnly && !this.Settings.MonitoredScripts.Contains(scriptId);
				result = (!flag2 && this.Settings.LogScriptTypes[(int)scriptId.Type]);
			}
			return result;
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00154C24 File Offset: 0x00152E24
		public void Update()
		{
			for (;;)
			{
				ScriptExecutionInstance instance;
				bool flag = this._executionInstances.TryPeek(out instance);
				if (!flag)
				{
					break;
				}
				instance.Update(this);
				bool isRunning = instance.IsRunning;
				if (isRunning)
				{
					break;
				}
				this._executionInstances.Pop();
				EventScriptRuntime.ObjectPool.Return(instance);
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00154C74 File Offset: 0x00152E74
		private ValueInfo SelectValueFromArgBox(Evaluator env, string identifier)
		{
			bool flag = this._tmpArgBox != null;
			ValueInfo result;
			if (flag)
			{
				result = this._tmpArgBox.SelectValue(this.Evaluator, identifier);
			}
			else
			{
				bool flag2 = this.Current.ArgBox != null;
				if (flag2)
				{
					result = this.Current.ArgBox.SelectValue(this.Evaluator, identifier);
				}
				else
				{
					result = ValueInfo.Void;
				}
			}
			return result;
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x00154CD9 File Offset: 0x00152ED9
		public void Log(string message)
		{
			this._logger.Info(message);
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x00154CE9 File Offset: 0x00152EE9
		public void OnExecuteInstruction(Instruction<EventScriptRuntime> instruction, ASTNode[] parameters)
		{
			this.ExecutingInstruction = instruction;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00154CF3 File Offset: 0x00152EF3
		public void OnInstructionEvaluated(ValueInfo ret)
		{
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00154CF8 File Offset: 0x00152EF8
		public Instruction<EventScriptRuntime> GetExecutingInstruction()
		{
			return this.ExecutingInstruction;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00154D10 File Offset: 0x00152F10
		public void StartRecordConditionHints()
		{
			this.RecordingConditionHints = true;
			if (this._recordedConditionInfos == null)
			{
				this._recordedConditionInfos = new List<OptionAvailableConditionInfo>();
			}
			this._recordedConditionInfos.Clear();
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x00154D3C File Offset: 0x00152F3C
		public List<OptionAvailableConditionInfo> StopRecordConditionHints()
		{
			this.RecordingConditionHints = false;
			List<OptionAvailableConditionInfo> recordedConditionInfos2 = this._recordedConditionInfos;
			bool flag = recordedConditionInfos2 == null || recordedConditionInfos2.Count <= 0;
			List<OptionAvailableConditionInfo> result;
			if (flag)
			{
				result = null;
			}
			else
			{
				List<OptionAvailableConditionInfo> recordedConditionInfos = this._recordedConditionInfos;
				this._recordedConditionInfos = null;
				result = recordedConditionInfos;
			}
			return result;
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00154D88 File Offset: 0x00152F88
		public void RecordConditionHint(int funcId, bool result, params string[] args)
		{
			OptionAvailableConditionInfo conditionInfo = new OptionAvailableConditionInfo(funcId, result, args);
			this._recordedConditionInfos.Add(conditionInfo);
		}

		// Token: 0x0400048D RID: 1165
		public readonly DataContext Context;

		// Token: 0x0400048E RID: 1166
		public Instruction<EventScriptRuntime> ExecutingInstruction;

		// Token: 0x0400048F RID: 1167
		private readonly Dictionary<ulong, IEventScriptDecompiler> _decompilers;

		// Token: 0x04000490 RID: 1168
		private readonly Interpreter<EventScriptRuntime> _interpreter;

		// Token: 0x04000491 RID: 1169
		private readonly Dictionary<int, string> _funcIdToNameMap;

		// Token: 0x04000492 RID: 1170
		private readonly Stack<ScriptExecutionInstance> _executionInstances;

		// Token: 0x04000493 RID: 1171
		private readonly Dictionary<string, EventScript> _globalScripts;

		// Token: 0x04000494 RID: 1172
		private static readonly LocalObjectPool<ScriptExecutionInstance> ObjectPool = new LocalObjectPool<ScriptExecutionInstance>(4, 32);

		// Token: 0x04000495 RID: 1173
		private EventArgBox _tmpArgBox;

		// Token: 0x04000496 RID: 1174
		public bool MovingNext;

		// Token: 0x04000497 RID: 1175
		public bool IsPaused;

		// Token: 0x04000498 RID: 1176
		public EventScriptRuntimeSettings Settings;

		// Token: 0x04000499 RID: 1177
		public readonly EventScriptDebugger Debugger;

		// Token: 0x0400049A RID: 1178
		private readonly Logger _logger = LogManager.GetCurrentClassLogger();

		// Token: 0x0400049B RID: 1179
		public bool RecordingConditionHints;

		// Token: 0x0400049C RID: 1180
		private List<OptionAvailableConditionInfo> _recordedConditionInfos;
	}
}
