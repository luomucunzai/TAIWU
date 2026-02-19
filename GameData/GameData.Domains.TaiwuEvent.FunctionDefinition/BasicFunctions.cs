using System;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class BasicFunctions
{
	[EventFunction(0)]
	private static ValueInfo If(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		bool boolValue = parameters[0].GetBoolValue(runtime.Evaluator);
		ScriptExecutionInstance current = runtime.Current;
		int currentIndentAmount = current.GetCurrentIndentAmount();
		current.ExecuteBranch(currentIndentAmount, boolValue);
		current.EnterScope(ScriptExecutionInstance.ScopeType.If);
		if (!boolValue)
		{
			current.ExitScope(currentIndentAmount + 1);
		}
		return ValueInfo.Void;
	}

	[EventFunction(1)]
	private static ValueInfo Else(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		ScriptExecutionInstance current = runtime.Current;
		int currentIndentAmount = current.GetCurrentIndentAmount();
		if (current.IsBranchEntered(currentIndentAmount))
		{
			current.ExitScope(currentIndentAmount + 1);
		}
		else
		{
			current.ExecuteBranch(currentIndentAmount, executed: true);
			current.EnterScope(ScriptExecutionInstance.ScopeType.If);
		}
		return ValueInfo.Void;
	}

	[EventFunction(2)]
	private static ValueInfo ElseIf(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		ScriptExecutionInstance current = runtime.Current;
		int currentIndentAmount = current.GetCurrentIndentAmount();
		if (current.IsBranchEntered(currentIndentAmount))
		{
			current.ExecuteBranch(currentIndentAmount, executed: true);
			current.ExitScope(currentIndentAmount + 1);
		}
		else
		{
			If(runtime, parameters);
		}
		return ValueInfo.Void;
	}

	[EventFunction(3)]
	private static ValueInfo Loop(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Invalid comparison between Unknown and I4
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Invalid comparison between Unknown and I4
		ScriptExecutionInstance current = runtime.Current;
		current.EnterScope(ScriptExecutionInstance.ScopeType.Loop);
		if (parameters.CheckIndex(0))
		{
			ValueInfo val = parameters[0].Evaluate(runtime.Evaluator);
			EValueType valueType = val.ValueType;
			EValueType val2 = valueType;
			if ((int)val2 != 1)
			{
				if ((int)val2 == 3)
				{
					bool condition = runtime.Evaluator.EvaluationStack.PopUnmanaged<bool>();
					current.CheckAndAdvanceIteration(condition);
				}
			}
			else
			{
				int maxCount = runtime.Evaluator.EvaluationStack.PopUnmanaged<int>();
				current.CheckAndAdvanceIteration(maxCount);
			}
		}
		return ValueInfo.Void;
	}

	[EventFunction(4)]
	private static ValueInfo Break(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		runtime.Current.BreakLoop();
		return ValueInfo.Void;
	}

	[EventFunction(6)]
	private static ValueInfo Continue(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		runtime.Current.GotoLoopHead();
		return ValueInfo.Void;
	}

	[EventFunction(7)]
	private static ValueInfo Label(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return ValueInfo.Void;
	}

	[EventFunction(8)]
	private static ValueInfo Jump(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		string stringValue = parameters[0].GetStringValue(runtime.Evaluator);
		runtime.Current.GotoLabel(stringValue);
		return ValueInfo.Void;
	}

	[EventFunction(11)]
	private static ValueInfo Random(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		int intValue = parameters[0].GetIntValue(runtime.Evaluator);
		int intValue2 = parameters[1].GetIntValue(runtime.Evaluator);
		int num = runtime.Context.Random.Next(intValue, intValue2);
		return runtime.Evaluator.PushEvaluationResult(num);
	}

	[EventFunction(12)]
	private static ValueInfo CheckProb(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		int intValue = parameters[0].GetIntValue(runtime.Evaluator);
		bool flag = runtime.Context.Random.CheckPercentProb(intValue);
		return runtime.Evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(215)]
	private static ValueInfo GetListLength(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		object anyValue = parameters[0].GetAnyValue(evaluator);
		if (1 == 0)
		{
		}
		int num = ((anyValue is ShortList shortList) ? (shortList.Items?.Count ?? 0) : ((anyValue is IntList intList) ? (intList.Items?.Count ?? 0) : 0));
		if (1 == 0)
		{
		}
		int num2 = num;
		return evaluator.PushEvaluationResult(num2);
	}

	[EventFunction(216)]
	private static ValueInfo GetListElement(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		object anyValue = parameters[0].GetAnyValue(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		object obj = anyValue;
		object obj2 = obj;
		if (!(obj2 is IntList intList))
		{
			if (obj2 is ShortList shortList)
			{
				return evaluator.PushEvaluationResult((int)shortList.Items[intValue]);
			}
			throw new InvalidCastException($"Unable to cast object {anyValue} to a list.");
		}
		return evaluator.PushEvaluationResult(intList.Items[intValue]);
	}

	[EventFunction(218)]
	private static ValueInfo CheckListElement(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_010e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00de: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		object anyValue = parameters[0].GetAnyValue(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		string stringValue = parameters[2].GetStringValue(evaluator);
		object obj = anyValue;
		object obj2 = obj;
		if (!(obj2 is IntList intList))
		{
			if (obj2 is ShortList shortList)
			{
				if (shortList.Items == null || shortList.Items.Count <= intValue)
				{
					return evaluator.PushEvaluationResult(false);
				}
				runtime.ArgBox.Set(stringValue, shortList.Items[intValue]);
				return evaluator.PushEvaluationResult(true);
			}
			return evaluator.PushEvaluationResult(false);
		}
		if (intList.Items == null || intList.Items.Count <= intValue)
		{
			return evaluator.PushEvaluationResult(false);
		}
		runtime.ArgBox.Set(stringValue, intList.Items[intValue]);
		return evaluator.PushEvaluationResult(true);
	}

	[EventFunction(91)]
	private static ValueInfo ExecuteGlobalScript(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		string stringValue = parameters[0].GetStringValue(runtime.Evaluator);
		EventArgBox argBox = runtime.Current.ArgBox;
		runtime.ExecuteGlobalScript(stringValue, argBox);
		return ValueInfo.Void;
	}

	[EventFunction(9)]
	private static ValueInfo Return(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		runtime.Current.ExitScript();
		return ValueInfo.Void;
	}

	[EventFunction(10)]
	private static ValueInfo Assign(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		return parameters[0].Evaluate(runtime.Evaluator);
	}

	[EventFunction(14)]
	private static ValueInfo Log(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		string anyValueAsString = parameters[0].GetAnyValueAsString(runtime.Evaluator);
		runtime.Log(anyValueAsString);
		return ValueInfo.Void;
	}

	[EventFunction(15)]
	private static ValueInfo Comment(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return ValueInfo.Void;
	}

	[EventFunction(5)]
	private static ValueInfo End(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		return ValueInfo.Void;
	}

	[EventFunction(13)]
	private static ValueInfo EventTransition(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		ScriptExecutionInstance current = runtime.Current;
		current.NextEvent = parameters[0].GetStringValue(runtime.Evaluator);
		current.ExitScript();
		return ValueInfo.Void;
	}

	[EventFunction(94)]
	private static ValueInfo OptionInjection(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		ScriptExecutionInstance current = runtime.Current;
		EventScriptId scriptId = current.ScriptId;
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		TaiwuEvent taiwuEvent = DomainManager.TaiwuEvent.GetEvent(stringValue);
		TaiwuEvent taiwuEvent2 = DomainManager.TaiwuEvent.GetEvent(scriptId.Guid);
		taiwuEvent.AddOption((scriptId.Guid, taiwuEvent2.EventConfig.EventOptions[intValue - 1].OptionKey));
		return ValueInfo.Void;
	}

	[EventFunction(101)]
	private static ValueInfo SaveSectMainStoryValue(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Expected I4, but got Unknown
		//IL_0206: Unknown result type (might be due to invalid IL or missing references)
		//IL_020b: Unknown result type (might be due to invalid IL or missing references)
		//IL_020f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b2: Expected O, but got Unknown
		Evaluator evaluator = runtime.Evaluator;
		sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		ValueInfo val = parameters[2].Evaluate(evaluator);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
		EValueType valueType = val.ValueType;
		EValueType val2 = valueType;
		switch (val2 - 1)
		{
		case 0:
		{
			int arg2 = evaluator.EvaluationStack.PopUnmanaged<int>();
			sectMainStoryEventArgBox.Set(stringValue, arg2);
			break;
		}
		case 1:
		{
			float arg3 = evaluator.EvaluationStack.PopUnmanaged<float>();
			sectMainStoryEventArgBox.Set(stringValue, arg3);
			break;
		}
		case 2:
		{
			bool arg4 = evaluator.EvaluationStack.PopUnmanaged<bool>();
			sectMainStoryEventArgBox.Set(stringValue, arg4);
			break;
		}
		case 3:
		{
			string arg = evaluator.EvaluationStack.PopObject<string>();
			sectMainStoryEventArgBox.Set(stringValue, arg);
			break;
		}
		case 4:
		{
			object obj = evaluator.EvaluationStack.PopObject<object>();
			object obj2 = obj;
			object obj3 = obj2;
			if (!(obj3 is GameData.Domains.Character.Character character))
			{
				if (!(obj3 is Settlement settlement))
				{
					if (!(obj3 is ItemKey itemKey))
					{
						if (obj3 is MapBlockData mapBlockData)
						{
							sectMainStoryEventArgBox.Set(stringValue, (ISerializableGameData)(object)mapBlockData.GetLocation());
							break;
						}
						if (!EventArgBox.SerializeObjectMap.ContainsKey(obj.GetType()))
						{
							throw new Exception($"Cannot save object {obj}.");
						}
						sectMainStoryEventArgBox.Set(stringValue, (ISerializableGameData)obj);
					}
					else
					{
						sectMainStoryEventArgBox.Set(stringValue, (ISerializableGameData)(object)itemKey);
					}
				}
				else
				{
					sectMainStoryEventArgBox.Set(stringValue, settlement.GetId());
				}
			}
			else
			{
				sectMainStoryEventArgBox.Set(stringValue, character.GetId());
			}
			break;
		}
		}
		DomainManager.Extra.SaveSectMainStoryEventArgumentBox(runtime.Context, orgTemplateId);
		return ValueInfo.Void;
	}

	[EventFunction(102)]
	private static ValueInfo ReadSectMainStoryValue(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
		return sectMainStoryEventArgBox.SelectValue(evaluator, stringValue);
	}

	[EventFunction(118)]
	private static void StartLifeSkillCombat(EventScriptRuntime runtime, GameData.Domains.Character.Character character, string onFinishEventId, bool normalBp, bool targetSelect, sbyte lifeSKillType)
	{
		if (character != null)
		{
			if (normalBp)
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartLifeSkillCombat(character.GetId(), 16, onFinishEventId, runtime.Current.ArgBox);
			}
			else if (targetSelect)
			{
				sbyte maxLifeSkillType = character.GetLifeSkillAttainments().GetMaxLifeSkillType();
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartLifeSkillCombat(character.GetId(), maxLifeSkillType, onFinishEventId, runtime.Current.ArgBox);
			}
			else
			{
				GameData.Domains.TaiwuEvent.EventHelper.EventHelper.StartLifeSkillCombat(character.GetId(), lifeSKillType, onFinishEventId, runtime.Current.ArgBox);
			}
		}
	}
}
