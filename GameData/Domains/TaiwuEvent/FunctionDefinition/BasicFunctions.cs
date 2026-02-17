using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A1 RID: 161
	public class BasicFunctions
	{
		// Token: 0x06001A62 RID: 6754 RVA: 0x00177768 File Offset: 0x00175968
		[EventFunction(0)]
		private static ValueInfo If(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			bool condition = parameters[0].GetBoolValue(runtime.Evaluator);
			ScriptExecutionInstance instance = runtime.Current;
			int indent = instance.GetCurrentIndentAmount();
			instance.ExecuteBranch(indent, condition);
			instance.EnterScope(ScriptExecutionInstance.ScopeType.If);
			bool flag = !condition;
			if (flag)
			{
				instance.ExitScope(indent + 1);
			}
			return ValueInfo.Void;
		}

		// Token: 0x06001A63 RID: 6755 RVA: 0x001777C4 File Offset: 0x001759C4
		[EventFunction(1)]
		private static ValueInfo Else(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			ScriptExecutionInstance instance = runtime.Current;
			int indent = instance.GetCurrentIndentAmount();
			bool flag = instance.IsBranchEntered(indent);
			if (flag)
			{
				instance.ExitScope(indent + 1);
			}
			else
			{
				instance.ExecuteBranch(indent, true);
				instance.EnterScope(ScriptExecutionInstance.ScopeType.If);
			}
			return ValueInfo.Void;
		}

		// Token: 0x06001A64 RID: 6756 RVA: 0x00177814 File Offset: 0x00175A14
		[EventFunction(2)]
		private static ValueInfo ElseIf(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			ScriptExecutionInstance instance = runtime.Current;
			int indent = instance.GetCurrentIndentAmount();
			bool flag = instance.IsBranchEntered(indent);
			if (flag)
			{
				instance.ExecuteBranch(indent, true);
				instance.ExitScope(indent + 1);
			}
			else
			{
				BasicFunctions.If(runtime, parameters);
			}
			return ValueInfo.Void;
		}

		// Token: 0x06001A65 RID: 6757 RVA: 0x00177864 File Offset: 0x00175A64
		[EventFunction(3)]
		private static ValueInfo Loop(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			ScriptExecutionInstance instance = runtime.Current;
			instance.EnterScope(ScriptExecutionInstance.ScopeType.Loop);
			bool flag = parameters.CheckIndex(0);
			if (flag)
			{
				ValueInfo arg0 = parameters[0].Evaluate(runtime.Evaluator);
				EValueType valueType = arg0.ValueType;
				EValueType evalueType = valueType;
				if (evalueType != EValueType.Int)
				{
					if (evalueType == EValueType.Bool)
					{
						bool condition = runtime.Evaluator.EvaluationStack.PopUnmanaged<bool>();
						instance.CheckAndAdvanceIteration(condition);
					}
				}
				else
				{
					int maxCount = runtime.Evaluator.EvaluationStack.PopUnmanaged<int>();
					instance.CheckAndAdvanceIteration(maxCount);
				}
			}
			return ValueInfo.Void;
		}

		// Token: 0x06001A66 RID: 6758 RVA: 0x001778FC File Offset: 0x00175AFC
		[EventFunction(4)]
		private static ValueInfo Break(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			runtime.Current.BreakLoop();
			return ValueInfo.Void;
		}

		// Token: 0x06001A67 RID: 6759 RVA: 0x00177920 File Offset: 0x00175B20
		[EventFunction(6)]
		private static ValueInfo Continue(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			runtime.Current.GotoLoopHead();
			return ValueInfo.Void;
		}

		// Token: 0x06001A68 RID: 6760 RVA: 0x00177944 File Offset: 0x00175B44
		[EventFunction(7)]
		private static ValueInfo Label(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return ValueInfo.Void;
		}

		// Token: 0x06001A69 RID: 6761 RVA: 0x0017795C File Offset: 0x00175B5C
		[EventFunction(8)]
		private static ValueInfo Jump(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			string label = parameters[0].GetStringValue(runtime.Evaluator);
			runtime.Current.GotoLabel(label);
			return ValueInfo.Void;
		}

		// Token: 0x06001A6A RID: 6762 RVA: 0x00177990 File Offset: 0x00175B90
		[EventFunction(11)]
		private static ValueInfo Random(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			int min = parameters[0].GetIntValue(runtime.Evaluator);
			int max = parameters[1].GetIntValue(runtime.Evaluator);
			int retVal = runtime.Context.Random.Next(min, max);
			return runtime.Evaluator.PushEvaluationResult(retVal);
		}

		// Token: 0x06001A6B RID: 6763 RVA: 0x001779E0 File Offset: 0x00175BE0
		[EventFunction(12)]
		private static ValueInfo CheckProb(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			int chance = parameters[0].GetIntValue(runtime.Evaluator);
			bool retVal = runtime.Context.Random.CheckPercentProb(chance);
			return runtime.Evaluator.PushEvaluationResult(retVal);
		}

		// Token: 0x06001A6C RID: 6764 RVA: 0x00177A20 File Offset: 0x00175C20
		[EventFunction(215)]
		private static ValueInfo GetListLength(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			object array = parameters[0].GetAnyValue(evaluator);
			if (!true)
			{
			}
			int num;
			if (array is ShortList)
			{
				ShortList shortList = (ShortList)array;
				List<short> items = shortList.Items;
				num = ((items != null) ? items.Count : 0);
			}
			else if (array is IntList)
			{
				IntList intList = (IntList)array;
				List<int> items2 = intList.Items;
				num = ((items2 != null) ? items2.Count : 0);
			}
			else
			{
				num = 0;
			}
			if (!true)
			{
			}
			int length = num;
			return evaluator.PushEvaluationResult(length);
		}

		// Token: 0x06001A6D RID: 6765 RVA: 0x00177AB0 File Offset: 0x00175CB0
		[EventFunction(216)]
		private static ValueInfo GetListElement(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			object obj = parameters[0].GetAnyValue(evaluator);
			int index = parameters[1].GetIntValue(evaluator);
			object obj2 = obj;
			object obj3 = obj2;
			ValueInfo result;
			if (obj3 is IntList)
			{
				IntList intList = (IntList)obj3;
				result = evaluator.PushEvaluationResult(intList.Items[index]);
			}
			else
			{
				if (!(obj3 is ShortList))
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(33, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unable to cast object ");
					defaultInterpolatedStringHandler.AppendFormatted<object>(obj);
					defaultInterpolatedStringHandler.AppendLiteral(" to a list.");
					throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				ShortList shortList = (ShortList)obj3;
				result = evaluator.PushEvaluationResult((int)shortList.Items[index]);
			}
			return result;
		}

		// Token: 0x06001A6E RID: 6766 RVA: 0x00177B74 File Offset: 0x00175D74
		[EventFunction(218)]
		private static ValueInfo CheckListElement(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			object obj = parameters[0].GetAnyValue(evaluator);
			int index = parameters[1].GetIntValue(evaluator);
			string key = parameters[2].GetStringValue(evaluator);
			object obj2 = obj;
			object obj3 = obj2;
			ValueInfo result;
			if (obj3 is IntList)
			{
				IntList intList = (IntList)obj3;
				bool flag = intList.Items == null || intList.Items.Count <= index;
				if (flag)
				{
					result = evaluator.PushEvaluationResult(false);
				}
				else
				{
					runtime.ArgBox.Set(key, intList.Items[index]);
					result = evaluator.PushEvaluationResult(true);
				}
			}
			else if (obj3 is ShortList)
			{
				ShortList shortList = (ShortList)obj3;
				bool flag2 = shortList.Items == null || shortList.Items.Count <= index;
				if (flag2)
				{
					result = evaluator.PushEvaluationResult(false);
				}
				else
				{
					runtime.ArgBox.Set(key, shortList.Items[index]);
					result = evaluator.PushEvaluationResult(true);
				}
			}
			else
			{
				result = evaluator.PushEvaluationResult(false);
			}
			return result;
		}

		// Token: 0x06001A6F RID: 6767 RVA: 0x00177C98 File Offset: 0x00175E98
		[EventFunction(91)]
		private static ValueInfo ExecuteGlobalScript(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			string scriptGuid = parameters[0].GetStringValue(runtime.Evaluator);
			EventArgBox argBox = runtime.Current.ArgBox;
			runtime.ExecuteGlobalScript(scriptGuid, argBox);
			return ValueInfo.Void;
		}

		// Token: 0x06001A70 RID: 6768 RVA: 0x00177CD4 File Offset: 0x00175ED4
		[EventFunction(9)]
		private static ValueInfo Return(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			runtime.Current.ExitScript();
			return ValueInfo.Void;
		}

		// Token: 0x06001A71 RID: 6769 RVA: 0x00177CF8 File Offset: 0x00175EF8
		[EventFunction(10)]
		private static ValueInfo Assign(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return parameters[0].Evaluate(runtime.Evaluator);
		}

		// Token: 0x06001A72 RID: 6770 RVA: 0x00177D18 File Offset: 0x00175F18
		[EventFunction(14)]
		private static ValueInfo Log(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			string text = parameters[0].GetAnyValueAsString(runtime.Evaluator);
			runtime.Log(text);
			return ValueInfo.Void;
		}

		// Token: 0x06001A73 RID: 6771 RVA: 0x00177D48 File Offset: 0x00175F48
		[EventFunction(15)]
		private static ValueInfo Comment(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return ValueInfo.Void;
		}

		// Token: 0x06001A74 RID: 6772 RVA: 0x00177D60 File Offset: 0x00175F60
		[EventFunction(5)]
		private static ValueInfo End(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return ValueInfo.Void;
		}

		// Token: 0x06001A75 RID: 6773 RVA: 0x00177D78 File Offset: 0x00175F78
		[EventFunction(13)]
		private static ValueInfo EventTransition(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			ScriptExecutionInstance current = runtime.Current;
			current.NextEvent = parameters[0].GetStringValue(runtime.Evaluator);
			current.ExitScript();
			return ValueInfo.Void;
		}

		// Token: 0x06001A76 RID: 6774 RVA: 0x00177DB4 File Offset: 0x00175FB4
		[EventFunction(94)]
		private static ValueInfo OptionInjection(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			ScriptExecutionInstance current = runtime.Current;
			EventScriptId scriptId = current.ScriptId;
			Evaluator evaluator = runtime.Evaluator;
			int optionId = parameters[0].GetIntValue(evaluator);
			string targetEventGuid = parameters[1].GetStringValue(evaluator);
			TaiwuEvent targetEvent = DomainManager.TaiwuEvent.GetEvent(targetEventGuid);
			TaiwuEvent srcEvent = DomainManager.TaiwuEvent.GetEvent(scriptId.Guid);
			targetEvent.AddOption(new ValueTuple<string, string>(scriptId.Guid, srcEvent.EventConfig.EventOptions[optionId - 1].OptionKey));
			return ValueInfo.Void;
		}

		// Token: 0x06001A77 RID: 6775 RVA: 0x00177E40 File Offset: 0x00176040
		[EventFunction(101)]
		private static ValueInfo SaveSectMainStoryValue(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
			string key = parameters[1].GetStringValue(evaluator);
			ValueInfo valueInfo = parameters[2].Evaluate(evaluator);
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
			switch (valueInfo.ValueType)
			{
			case EValueType.Int:
			{
				int value = evaluator.EvaluationStack.PopUnmanaged<int>();
				argBox.Set(key, value);
				break;
			}
			case EValueType.Float:
			{
				float value2 = evaluator.EvaluationStack.PopUnmanaged<float>();
				argBox.Set(key, value2);
				break;
			}
			case EValueType.Bool:
			{
				bool value3 = evaluator.EvaluationStack.PopUnmanaged<bool>();
				argBox.Set(key, value3);
				break;
			}
			case EValueType.Str:
			{
				string value4 = evaluator.EvaluationStack.PopObject<string>();
				argBox.Set(key, value4);
				break;
			}
			case EValueType.Obj:
			{
				object value5 = evaluator.EvaluationStack.PopObject<object>();
				object obj = value5;
				object obj2 = obj;
				Character character = obj2 as Character;
				if (character == null)
				{
					Settlement settlement = obj2 as Settlement;
					if (settlement == null)
					{
						if (obj2 is ItemKey)
						{
							ItemKey itemKey = (ItemKey)obj2;
							argBox.Set(key, itemKey);
						}
						else
						{
							MapBlockData mapBlock = obj2 as MapBlockData;
							if (mapBlock == null)
							{
								bool flag = EventArgBox.SerializeObjectMap.ContainsKey(value5.GetType());
								if (!flag)
								{
									DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(20, 1);
									defaultInterpolatedStringHandler.AppendLiteral("Cannot save object ");
									defaultInterpolatedStringHandler.AppendFormatted<object>(value5);
									defaultInterpolatedStringHandler.AppendLiteral(".");
									throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
								}
								argBox.Set(key, (ISerializableGameData)value5);
							}
							else
							{
								argBox.Set(key, mapBlock.GetLocation());
							}
						}
					}
					else
					{
						argBox.Set(key, settlement.GetId());
					}
				}
				else
				{
					argBox.Set(key, character.GetId());
				}
				break;
			}
			}
			DomainManager.Extra.SaveSectMainStoryEventArgumentBox(runtime.Context, orgTemplateId);
			return ValueInfo.Void;
		}

		// Token: 0x06001A78 RID: 6776 RVA: 0x00178060 File Offset: 0x00176260
		[EventFunction(102)]
		private static ValueInfo ReadSectMainStoryValue(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
			string key = parameters[1].GetStringValue(evaluator);
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
			return argBox.SelectValue(evaluator, key);
		}

		// Token: 0x06001A79 RID: 6777 RVA: 0x001780A4 File Offset: 0x001762A4
		[EventFunction(118)]
		private static void StartLifeSkillCombat(EventScriptRuntime runtime, Character character, string onFinishEventId, bool normalBp, bool targetSelect, sbyte lifeSKillType)
		{
			bool flag = character != null;
			if (flag)
			{
				if (normalBp)
				{
					EventHelper.StartLifeSkillCombat(character.GetId(), 16, onFinishEventId, runtime.Current.ArgBox);
				}
				else if (targetSelect)
				{
					sbyte type = character.GetLifeSkillAttainments().GetMaxLifeSkillType();
					EventHelper.StartLifeSkillCombat(character.GetId(), type, onFinishEventId, runtime.Current.ArgBox);
				}
				else
				{
					EventHelper.StartLifeSkillCombat(character.GetId(), lifeSKillType, onFinishEventId, runtime.Current.ArgBox);
				}
			}
		}
	}
}
