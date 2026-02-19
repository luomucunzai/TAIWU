using System;
using System.Collections.Generic;
using CompDevLib.Interpreter;
using CompDevLib.Interpreter.Parse;
using Config;
using GameData.Domains.Adventure;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.Taiwu.Profession;
using GameData.Domains.TaiwuEvent.DisplayEvent;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.TaiwuEvent.FunctionDefinition;

public class EventConditions
{
	[EventFunction(148)]
	private static ValueInfo CheckCharacterCanTeachTaiwuProfession(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int id = anyValue.GetId();
		int characterTeachTaiwuProfessionDate = DomainManager.Extra.GetCharacterTeachTaiwuProfessionDate(id);
		bool flag = DomainManager.World.GetCurrDate() > characterTeachTaiwuProfessionDate + 12;
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(148, flag);
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(163)]
	private static ValueInfo CheckCharacterCanTeachTaiwuProfessionSkillUnlock(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int id = anyValue.GetId();
		int num = DomainManager.Extra.GetCharacterProfessionData(id, intValue)?.GetUnlockedSkillCount() ?? 0;
		bool flag = num > 0;
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(163, flag);
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(90)]
	private static ValueInfo CheckPreviousCombatResult(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int arg = 0;
		if (!runtime.ArgBox.Get("CombatResult", ref arg))
		{
			throw new Exception("CheckPreviousCombatResult can only be called after combat.");
		}
		int intValue = parameters[0].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(intValue == arg);
	}

	public static bool PerformOperation(int operatorId, int currValue, int requiredValue)
	{
		if (1 == 0)
		{
		}
		bool result = operatorId switch
		{
			0 => currValue == requiredValue, 
			1 => currValue != requiredValue, 
			2 => currValue > requiredValue, 
			3 => currValue < requiredValue, 
			4 => currValue >= requiredValue, 
			5 => currValue <= requiredValue, 
			_ => throw new Exception($"Invalid operator {operatorId} for current condition"), 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	[EventFunction(190)]
	private static ValueInfo CheckSettlementApprovingRate(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		Settlement anyValue = parameters[0].GetAnyValue<Settlement>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		short currValue = anyValue.CalcApprovingRate();
		bool flag = PerformOperation(intValue, currValue, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(193)]
	private static ValueInfo CheckStateHasSettlementType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_00f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		sbyte stateTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
		EOrganizationSettlementType intValue = (EOrganizationSettlementType)parameters[1].GetIntValue(evaluator);
		sbyte stateIdByStateTemplateId = DomainManager.Map.GetStateIdByStateTemplateId(stateTemplateId);
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		DomainManager.Map.GetStateSettlementIds(stateIdByStateTemplateId, list, containsMainCity: true, containsSect: true);
		if (intValue != EOrganizationSettlementType.Invalid)
		{
			for (int num = list.Count - 1; num >= 0; num--)
			{
				short settlementId = list[num];
				Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
				if (settlement.OrganizationConfig.SettlementType == intValue)
				{
					ObjectPool<List<short>>.Instance.Return(list);
					return evaluator.PushEvaluationResult(true);
				}
			}
			ObjectPool<List<short>>.Instance.Return(list);
			return evaluator.PushEvaluationResult(false);
		}
		bool flag = list.Count > 0;
		ObjectPool<List<short>>.Instance.Return(list);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(202)]
	private static ValueInfo CheckAdventureTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[1].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(intValue == intValue2);
	}

	[EventFunction(169)]
	private static ValueInfo CheckAdventureParameterCount(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(170)]
	private static ValueInfo CheckMovePoint(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(171)]
	private static ValueInfo CheckCurrMonth(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(172)]
	private static ValueInfo CheckCharacterKidnapSpecificGender(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(173)]
	private static ValueInfo CheckCharacterKidnapSpecificAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(174)]
	private static ValueInfo CheckCharacterKidnapSpecificId(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(175)]
	private static ValueInfo CheckCharacterTeammateSpecificIdGender(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(176)]
	private static ValueInfo CheckCharacterTeammateSpecificIdAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(177)]
	private static ValueInfo CheckCharacterTeammateSpecificIdId(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(178)]
	private static ValueInfo CheckCharacterExp(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(179)]
	private static ValueInfo CheckCharacterReadCombatSkillPageCount(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(180)]
	private static ValueInfo CheckCharacterCombatSkillBreakout(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(181)]
	private static ValueInfo CheckAdventurePerMoveCount(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(182)]
	private static ValueInfo CheckAdventurePerCostMovePoint(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(183)]
	private static ValueInfo CheckAdventureElementVisible(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(185)]
	private static ValueInfo CheckAdventureCharacterGroup(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(186)]
	private static ValueInfo CheckAdventureElementGroup(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		throw new NotImplementedException();
	}

	[EventFunction(71)]
	private static ValueInfo CheckExpression(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		bool boolValue = parameters[0].GetBoolValue(evaluator);
		return evaluator.PushEvaluationResult(boolValue);
	}

	[EventFunction(109)]
	private static ValueInfo CheckAnd(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return runtime.Evaluator.PushEvaluationResult(true);
	}

	[EventFunction(110)]
	private static ValueInfo CheckOr(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return runtime.Evaluator.PushEvaluationResult(true);
	}

	[EventFunction(159)]
	private static ValueInfo CheckCharacterAlive(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Invalid comparison between Unknown and I4
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Invalid comparison between Unknown and I4
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		ValueInfo val = parameters[0].Evaluate(evaluator);
		EValueType valueType = val.ValueType;
		EValueType val2 = valueType;
		if ((int)val2 != 1)
		{
			if ((int)val2 == 5)
			{
				GameData.Domains.Character.Character character = evaluator.EvaluationStack.PopObject<GameData.Domains.Character.Character>();
				return evaluator.PushEvaluationResult(character != null);
			}
			throw new ArgumentException($"Unrecognized argument type: Character expected, {val.ValueType} given.");
		}
		int charId = evaluator.EvaluationStack.PopUnmanaged<int>();
		return evaluator.PushEvaluationResult(DomainManager.Character.IsCharacterAlive(charId));
	}

	[EventFunction(213)]
	private static ValueInfo CheckCharacterPassMatcher(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Invalid comparison between Unknown and I4
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Invalid comparison between Unknown and I4
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_0117: Unknown result type (might be due to invalid IL or missing references)
		//IL_011c: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		ValueInfo val = parameters[0].Evaluate(evaluator);
		EValueType valueType = val.ValueType;
		EValueType val2 = valueType;
		GameData.Domains.Character.Character element;
		if ((int)val2 != 1)
		{
			if ((int)val2 != 5)
			{
				throw new InvalidCastException($"Unrecognized argument type {val.ValueType}.");
			}
			object obj = evaluator.EvaluationStack.PopObject<object>();
			if (obj != null && !(obj is EventActorData) && !(obj is GameData.Domains.Character.Character))
			{
				throw new InvalidCastException($"Unrecognized argument type {obj.GetType()}.");
			}
			element = obj as GameData.Domains.Character.Character;
		}
		else
		{
			int objectId = evaluator.EvaluationStack.PopUnmanaged<int>();
			DomainManager.Character.TryGetElement_Objects(objectId, out element);
		}
		if (element == null)
		{
			return evaluator.PushEvaluationResult(false);
		}
		int intValue = parameters[1].GetIntValue(evaluator);
		CharacterMatcherItem matcherItem = CharacterMatcher.Instance[intValue];
		bool flag = matcherItem.Match(element);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(76)]
	private static ValueInfo CheckCharacterCurrMainAttribute(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short currMainAttribute = anyValue.GetCurrMainAttribute((sbyte)intValue);
		bool flag = PerformOperation(intValue2, currMainAttribute, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(77)]
	private static ValueInfo CheckCharacterMainAttribute(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short maxMainAttribute = anyValue.GetMaxMainAttribute((sbyte)intValue);
		bool flag = PerformOperation(intValue2, maxMainAttribute, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(78)]
	private static ValueInfo CheckCharacterLifeSkillQualification(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short lifeSkillQualification = anyValue.GetLifeSkillQualification((sbyte)intValue);
		bool flag = PerformOperation(intValue2, lifeSkillQualification, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(79)]
	private static ValueInfo CheckCharacterLifeSkillAttainment(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short lifeSkillAttainment = anyValue.GetLifeSkillAttainment((sbyte)intValue);
		bool flag = PerformOperation(intValue2, lifeSkillAttainment, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(80)]
	private static ValueInfo CheckCharacterCombatSkillQualification(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short combatSkillQualification = anyValue.GetCombatSkillQualification((sbyte)intValue);
		bool flag = PerformOperation(intValue2, combatSkillQualification, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(81)]
	private static ValueInfo CheckCharacterCombatSkillAttainment(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		short combatSkillAttainment = anyValue.GetCombatSkillAttainment((sbyte)intValue);
		bool flag = PerformOperation(intValue2, combatSkillAttainment, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(82)]
	private static ValueInfo CheckCharacterPersonality(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		sbyte personality = anyValue.GetPersonality((sbyte)intValue);
		bool flag = PerformOperation(intValue2, personality, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(107)]
	private static ValueInfo CheckCharacterBehaviorType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		sbyte behaviorType = anyValue.GetBehaviorType();
		return evaluator.PushEvaluationResult(intValue == behaviorType);
	}

	[EventFunction(108)]
	private static ValueInfo CheckMorality(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		short morality = anyValue.GetMorality();
		bool flag = PerformOperation(intValue, morality, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(83)]
	private static ValueInfo CheckCharacterResource(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		int resource = anyValue.GetResource((sbyte)intValue);
		bool flag = PerformOperation(intValue2, resource, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(85)]
	private static ValueInfo CheckCharacterInventoryByTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		TemplateKey value = ((Variant<TemplateKey>)(object)parameters[1].GetObjectValue<UnmanagedVariant<TemplateKey>>(evaluator)).Value;
		bool flag = anyValue.GetInventory().GetInventoryItemKey(value.ItemType, value.TemplateId).IsValid();
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(84)]
	private static ValueInfo CheckCharacterFeature(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		short item = (short)parameters[1].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(anyValue.GetFeatureIds().Contains(item));
	}

	[EventFunction(140)]
	private static ValueInfo CheckCharacterCurrentProfession(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int num = DomainManager.Extra.GetCharacterCurrentProfession(anyValue.GetId())?.TemplateId ?? (-1);
		return evaluator.PushEvaluationResult(num == intValue);
	}

	[EventFunction(146)]
	private static ValueInfo TryGetCharacterCurrentProfession(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		ProfessionData characterCurrentProfession = DomainManager.Extra.GetCharacterCurrentProfession(intValue);
		if (string.IsNullOrEmpty(stringValue))
		{
			return evaluator.PushEvaluationResult(characterCurrentProfession != null);
		}
		runtime.ArgBox.Set(stringValue, characterCurrentProfession?.TemplateId ?? (-1));
		return evaluator.PushEvaluationResult(characterCurrentProfession != null);
	}

	[EventFunction(141)]
	private static ValueInfo CheckCharacterSeniorityPercent(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_00dc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		int currValue = ((intValue >= 0) ? DomainManager.Extra.GetCharacterProfessionData(anyValue.GetId(), intValue) : DomainManager.Extra.GetCharacterCurrentProfession(anyValue.GetId()))?.GetSeniorityPercent() ?? 0;
		bool flag = PerformOperation(intValue2, currValue, intValue3);
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(141, flag, string.Empty, (intValue >= 0) ? Profession.Instance[intValue].Name : string.Empty, EventConditionOperator.Instance[intValue2].Name, intValue3.ToString());
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(103)]
	private static ValueInfo CheckCharacterGrade(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		bool flag = PerformOperation(intValue, anyValue.GetOrganizationInfo().Grade, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(104)]
	private static ValueInfo CheckCharacterSettlement(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Settlement anyValue2 = parameters[1].GetAnyValue<Settlement>(evaluator);
		if (anyValue2 == null)
		{
			return evaluator.PushEvaluationResult(anyValue.GetOrganizationInfo().SettlementId < 0);
		}
		return evaluator.PushEvaluationResult(anyValue.GetOrganizationInfo().SettlementId == anyValue2.GetId());
	}

	[EventFunction(86)]
	private static ValueInfo CheckCharacterOnSettlementBlock(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Settlement anyValue2 = parameters[1].GetAnyValue<Settlement>(evaluator);
		if (anyValue2 == null)
		{
			return evaluator.PushEvaluationResult(false);
		}
		short id = anyValue2.GetId();
		Location validLocation = anyValue.GetValidLocation();
		bool flag = DomainManager.Map.IsLocationOnSettlementBlock(validLocation, id);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(87)]
	private static ValueInfo CheckCharacterInSettlementInfluenceRange(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Settlement anyValue2 = parameters[1].GetAnyValue<Settlement>(evaluator);
		Location validLocation = anyValue.GetValidLocation();
		short id = anyValue2.GetId();
		bool flag = DomainManager.Map.IsLocationInSettlementInfluenceRange(validLocation, id);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(168)]
	private static ValueInfo CheckCharacterInMapBlockRange(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Location location = anyValue.GetLocation();
		if (!anyValue.GetLocation().IsValid())
		{
			return evaluator.PushEvaluationResult(false);
		}
		MapBlockData anyValue2 = parameters[1].GetAnyValue<MapBlockData>(evaluator);
		if (anyValue2 == null)
		{
			return evaluator.PushEvaluationResult(false);
		}
		Location location2 = anyValue2.GetLocation();
		int intValue = parameters[2].GetIntValue(evaluator);
		bool flag = location.IsNearbyLocation(location2, intValue);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(95)]
	private static ValueInfo CheckCharacterInMapState(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		Location validLocation = anyValue.GetValidLocation();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(validLocation.AreaId);
		bool flag = intValue == stateTemplateIdByAreaId;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(96)]
	private static ValueInfo CheckCharacterInMapArea(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		Location validLocation = anyValue.GetValidLocation();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(validLocation.AreaId);
		bool flag = element_Areas.GetTemplateId() == intValue;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(97)]
	private static ValueInfo CheckCharacterOnAnySettlement(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Location validLocation = anyValue.GetValidLocation();
		MapBlockData rootBlock = DomainManager.Map.GetBlock(validLocation).GetRootBlock();
		bool flag = rootBlock.IsCityTown();
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(98)]
	private static ValueInfo CheckCharacterInAnySettlementInfluenceRange(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Location validLocation = anyValue.GetValidLocation();
		MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(validLocation);
		bool flag = belongSettlementBlock != null;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(194)]
	private static ValueInfo CheckCharacterInSettlementArea(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		Settlement anyValue2 = parameters[1].GetAnyValue<Settlement>(evaluator);
		bool flag = anyValue.GetValidLocation().AreaId == anyValue2.GetLocation().AreaId;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(88)]
	private static ValueInfo CheckCharacterFavorability(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		GameData.Domains.Character.Character anyValue2 = parameters[1].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[2].GetIntValue(evaluator);
		int intValue2 = parameters[3].GetIntValue(evaluator);
		short favorability = DomainManager.Character.GetFavorability(anyValue.GetId(), anyValue2.GetId());
		bool flag = PerformOperation(intValue, favorability, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(142)]
	private static ValueInfo CheckCharacterFavorabilityType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		GameData.Domains.Character.Character anyValue2 = parameters[1].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[2].GetIntValue(evaluator);
		int intValue2 = parameters[3].GetIntValue(evaluator);
		sbyte favorabilityType = DomainManager.Character.GetFavorabilityType(anyValue.GetId(), anyValue2.GetId());
		bool flag = PerformOperation(intValue, favorabilityType, intValue2);
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(142, flag, string.Empty, string.Empty, EventConditionOperator.Instance[intValue].Name, $"<Language Key=LK_Favor_Type_{intValue2 - -6}/>");
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(121)]
	private static ValueInfo CheckCharacterHasItem(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		UnmanagedVariant<TemplateKey> anyValue2 = parameters[1].GetAnyValue<UnmanagedVariant<TemplateKey>>(evaluator);
		Inventory inventory = anyValue.GetInventory();
		return evaluator.PushEvaluationResult(inventory.GetInventoryItemKey(((Variant<TemplateKey>)(object)anyValue2).Value.ItemType, ((Variant<TemplateKey>)(object)anyValue2).Value.TemplateId).IsValid());
	}

	[EventFunction(122)]
	private static ValueInfo CheckCharacterMerchantType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		sbyte merchantCharToType = DomainManager.Extra.GetMerchantCharToType(anyValue.GetId());
		return evaluator.PushEvaluationResult(intValue == merchantCharToType);
	}

	[EventFunction(157)]
	private static ValueInfo CheckCharacterConsummateLevel(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		sbyte consummateLevel = anyValue.GetConsummateLevel();
		bool flag = PerformOperation(intValue, consummateLevel, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(160)]
	private static ValueInfo CheckCharacterCurrAge(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		short currAge = anyValue.GetCurrAge();
		bool flag = PerformOperation(intValue, currAge, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(161)]
	private static ValueInfo CheckCharacterActualAge(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		short actualAge = anyValue.GetActualAge();
		bool flag = PerformOperation(intValue, actualAge, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(162)]
	private static ValueInfo CheckCharacterAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		sbyte ageGroup = anyValue.GetAgeGroup();
		bool flag = PerformOperation(intValue, ageGroup, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(203)]
	private static ValueInfo CheckCharacterGender(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		bool flag = anyValue.GetGender() == intValue;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(128)]
	private static ValueInfo CheckFixedCharacterTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(anyValue.GetTemplateId() == intValue);
	}

	[EventFunction(131)]
	private static ValueInfo CheckCharacterReadLifeSkillPageCount(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		int intValue3 = parameters[3].GetIntValue(evaluator);
		List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = anyValue.GetLearnedLifeSkills();
		int num = anyValue.FindLearnedLifeSkillIndex((short)intValue);
		int currValue = ((num >= 0) ? learnedLifeSkills[num].GetReadPagesCount() : 0);
		bool flag = PerformOperation(intValue2, currValue, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(257)]
	private static ValueInfo CheckTaiwuHasFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[1].GetIntValue(evaluator);
		bool flag = PerformOperation(intValue, DomainManager.Extra.GetFuyuFaith(), intValue2);
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(257, flag, EventConditionOperator.Instance[intValue].Name, intValue2.ToString());
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(258)]
	private static ValueInfo CheckCharacterFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		int intValue2 = parameters[2].GetIntValue(evaluator);
		bool flag = PerformOperation(intValue, anyValue.GetDarkAshCounter().Tips3, intValue2);
		if (runtime.RecordingConditionHints)
		{
			runtime.RecordConditionHint(258, flag, DomainManager.Character.GetName(anyValue.GetId()), EventConditionOperator.Instance[intValue].Name, intValue2.ToString());
		}
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(256)]
	private static ValueInfo TryGetMaxAcceptableFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		GameData.Domains.Character.Character anyValue = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		runtime.ArgBox.Set(stringValue, GameData.Domains.TaiwuEvent.EventHelper.EventHelper.GetMaxAcceptableFuyuFaith(anyValue));
		return evaluator.PushEvaluationResult(true);
	}

	[EventFunction(136)]
	private static ValueInfo CheckItemType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		ItemKey anyValue = parameters[0].GetAnyValue<ItemKey>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(anyValue.ItemType == intValue);
	}

	[EventFunction(137)]
	private static ValueInfo CheckItemSubType(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		ItemKey anyValue = parameters[0].GetAnyValue<ItemKey>(evaluator);
		short itemSubType = ItemTemplateHelper.GetItemSubType(anyValue.ItemType, anyValue.TemplateId);
		int intValue = parameters[1].GetIntValue(evaluator);
		return evaluator.PushEvaluationResult(intValue == itemSubType);
	}

	[EventFunction(138)]
	private static ValueInfo CheckItemTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		ItemKey anyValue = parameters[0].GetAnyValue<ItemKey>(evaluator);
		TemplateKey value = ((Variant<TemplateKey>)(object)parameters[1].GetAnyValue<UnmanagedVariant<TemplateKey>>(evaluator)).Value;
		return evaluator.PushEvaluationResult(value.ItemType == anyValue.ItemType && value.TemplateId == anyValue.TemplateId);
	}

	[EventFunction(106)]
	private static ValueInfo CheckSettlementInMapArea(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		Settlement anyValue = parameters[0].GetAnyValue<Settlement>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		if (anyValue == null)
		{
			return evaluator.PushEvaluationResult(false);
		}
		Location location = anyValue.GetLocation();
		MapAreaData element_Areas = DomainManager.Map.GetElement_Areas(location.AreaId);
		bool flag = element_Areas.GetTemplateId() == intValue;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(105)]
	private static ValueInfo CheckSettlementInMapState(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		Settlement anyValue = parameters[0].GetAnyValue<Settlement>(evaluator);
		int intValue = parameters[1].GetIntValue(evaluator);
		if (anyValue == null)
		{
			return evaluator.PushEvaluationResult(false);
		}
		Location location = anyValue.GetLocation();
		sbyte stateTemplateIdByAreaId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
		bool flag = intValue == stateTemplateIdByAreaId;
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(112)]
	private static ValueInfo CheckAreaSpiritualDebt(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[1].GetIntValue(evaluator);
		int intValue3 = parameters[2].GetIntValue(evaluator);
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)intValue);
		int areaSpiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(areaIdByAreaTemplateId);
		bool flag = PerformOperation(intValue2, areaSpiritualDebt, intValue3);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(204)]
	private static ValueInfo CheckAreaHasAdventure(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[1].GetIntValue(evaluator);
		short areaIdByAreaTemplateId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)intValue);
		AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaIdByAreaTemplateId);
		foreach (AdventureSiteData value in adventuresInArea.AdventureSites.Values)
		{
			if (value.TemplateId == intValue2 && value.SiteState >= 1)
			{
				return evaluator.PushEvaluationResult(true);
			}
		}
		return evaluator.PushEvaluationResult(false);
	}

	[EventFunction(158)]
	private static ValueInfo CheckIsDreamBack(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		return evaluator.PushEvaluationResult(DomainManager.Extra.GetIsDreamBack());
	}

	[EventFunction(72)]
	private static ValueInfo CheckMainStoryProgress(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[1].GetIntValue(evaluator);
		short mainStoryLineProgress = DomainManager.World.GetMainStoryLineProgress();
		bool flag = PerformOperation(intValue, mainStoryLineProgress, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(155)]
	private static ValueInfo CheckWorldFunctionStatus(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		byte worldFunctionType = (byte)parameters[0].GetIntValue(evaluator);
		bool worldFunctionsStatus = DomainManager.World.GetWorldFunctionsStatus(worldFunctionType);
		return evaluator.PushEvaluationResult(worldFunctionsStatus);
	}

	[EventFunction(73)]
	private static ValueInfo CheckTask(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		bool flag = DomainManager.Extra.IsExtraTaskInProgress(intValue);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(74)]
	private static ValueInfo CheckTaskChain(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		bool flag = DomainManager.Extra.IsExtraTaskChainInProgress(intValue);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(75)]
	private static ValueInfo CheckXiangshuLevel(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		int intValue = parameters[0].GetIntValue(evaluator);
		int intValue2 = parameters[0].GetIntValue(evaluator);
		sbyte xiangshuLevel = DomainManager.World.GetXiangshuLevel();
		bool flag = PerformOperation(intValue, xiangshuLevel, intValue2);
		return evaluator.PushEvaluationResult(flag);
	}

	[EventFunction(123)]
	private static ValueInfo CheckSectMainStoryValueExists(EventScriptRuntime runtime, ASTNode[] parameters)
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Invalid comparison between Unknown and I4
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		Evaluator evaluator = runtime.Evaluator;
		sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
		string stringValue = parameters[1].GetStringValue(evaluator);
		string stringValue2 = parameters[2].GetStringValue(evaluator);
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
		ValueInfo val = sectMainStoryEventArgBox.SelectValue(evaluator, stringValue);
		if ((int)val.ValueType == 0)
		{
			return evaluator.PushEvaluationResult(false);
		}
		if (string.IsNullOrEmpty(stringValue2))
		{
			evaluator.RemoveTopValue(val);
			return evaluator.PushEvaluationResult(true);
		}
		runtime.ArgBox.SetValueFromStack(evaluator.EvaluationStack, stringValue2, val.ValueType);
		return evaluator.PushEvaluationResult(true);
	}
}
