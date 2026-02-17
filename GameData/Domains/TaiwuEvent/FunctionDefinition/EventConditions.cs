using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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

namespace GameData.Domains.TaiwuEvent.FunctionDefinition
{
	// Token: 0x020000A4 RID: 164
	public class EventConditions
	{
		// Token: 0x06001ABA RID: 6842 RVA: 0x0017935C File Offset: 0x0017755C
		[EventFunction(148)]
		private static ValueInfo CheckCharacterCanTeachTaiwuProfession(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int charId = character.GetId();
			int date = DomainManager.Extra.GetCharacterTeachTaiwuProfessionDate(charId);
			bool result = DomainManager.World.GetCurrDate() > date + 12;
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				runtime.RecordConditionHint(148, result, Array.Empty<string>());
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ABB RID: 6843 RVA: 0x001793D0 File Offset: 0x001775D0
		[EventFunction(163)]
		private static ValueInfo CheckCharacterCanTeachTaiwuProfessionSkillUnlock(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int professionId = parameters[1].GetIntValue(evaluator);
			int charId = character.GetId();
			ProfessionData professionData = DomainManager.Extra.GetCharacterProfessionData(charId, professionId);
			int unlockedSkillCount = (professionData != null) ? professionData.GetUnlockedSkillCount() : 0;
			bool result = unlockedSkillCount > 0;
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				runtime.RecordConditionHint(163, result, Array.Empty<string>());
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ABC RID: 6844 RVA: 0x00179454 File Offset: 0x00177654
		[EventFunction(90)]
		private static ValueInfo CheckPreviousCombatResult(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int actualValue = 0;
			bool flag = !runtime.ArgBox.Get("CombatResult", ref actualValue);
			if (flag)
			{
				throw new Exception("CheckPreviousCombatResult can only be called after combat.");
			}
			int requiredValue = parameters[0].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult(requiredValue == actualValue);
		}

		// Token: 0x06001ABD RID: 6845 RVA: 0x001794AC File Offset: 0x001776AC
		public static bool PerformOperation(int operatorId, int currValue, int requiredValue)
		{
			if (!true)
			{
			}
			bool result;
			switch (operatorId)
			{
			case 0:
				result = (currValue == requiredValue);
				break;
			case 1:
				result = (currValue != requiredValue);
				break;
			case 2:
				result = (currValue > requiredValue);
				break;
			case 3:
				result = (currValue < requiredValue);
				break;
			case 4:
				result = (currValue >= requiredValue);
				break;
			case 5:
				result = (currValue <= requiredValue);
				break;
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(39, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Invalid operator ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(operatorId);
				defaultInterpolatedStringHandler.AppendLiteral(" for current condition");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x06001ABE RID: 6846 RVA: 0x00179554 File Offset: 0x00177754
		[EventFunction(190)]
		private static ValueInfo CheckSettlementApprovingRate(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			Settlement settlement = parameters[0].GetAnyValue<Settlement>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			short approvingRate = settlement.CalcApprovingRate();
			bool result = EventConditions.PerformOperation(operatorId, (int)approvingRate, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ABF RID: 6847 RVA: 0x001795A8 File Offset: 0x001777A8
		[EventFunction(193)]
		private static ValueInfo CheckStateHasSettlementType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			sbyte stateTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
			EOrganizationSettlementType settlementType = (EOrganizationSettlementType)parameters[1].GetIntValue(evaluator);
			sbyte stateId = DomainManager.Map.GetStateIdByStateTemplateId((short)stateTemplateId);
			List<short> settlementIds = ObjectPool<List<short>>.Instance.Get();
			DomainManager.Map.GetStateSettlementIds(stateId, settlementIds, true, true);
			bool flag = settlementType != EOrganizationSettlementType.Invalid;
			ValueInfo result;
			if (flag)
			{
				for (int i = settlementIds.Count - 1; i >= 0; i--)
				{
					short settlementId = settlementIds[i];
					Settlement settlement = DomainManager.Organization.GetSettlement(settlementId);
					bool flag2 = settlement.OrganizationConfig.SettlementType != settlementType;
					if (!flag2)
					{
						ObjectPool<List<short>>.Instance.Return(settlementIds);
						return evaluator.PushEvaluationResult(true);
					}
				}
				ObjectPool<List<short>>.Instance.Return(settlementIds);
				result = evaluator.PushEvaluationResult(false);
			}
			else
			{
				bool hasAny = settlementIds.Count > 0;
				ObjectPool<List<short>>.Instance.Return(settlementIds);
				result = evaluator.PushEvaluationResult(hasAny);
			}
			return result;
		}

		// Token: 0x06001AC0 RID: 6848 RVA: 0x001796B8 File Offset: 0x001778B8
		[EventFunction(202)]
		private static ValueInfo CheckAdventureTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int adventureTemplateIdA = parameters[0].GetIntValue(evaluator);
			int adventureTemplateIdB = parameters[1].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult(adventureTemplateIdA == adventureTemplateIdB);
		}

		// Token: 0x06001AC1 RID: 6849 RVA: 0x001796EF File Offset: 0x001778EF
		[EventFunction(169)]
		private static ValueInfo CheckAdventureParameterCount(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC2 RID: 6850 RVA: 0x001796F7 File Offset: 0x001778F7
		[EventFunction(170)]
		private static ValueInfo CheckMovePoint(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC3 RID: 6851 RVA: 0x001796FF File Offset: 0x001778FF
		[EventFunction(171)]
		private static ValueInfo CheckCurrMonth(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC4 RID: 6852 RVA: 0x00179707 File Offset: 0x00177907
		[EventFunction(172)]
		private static ValueInfo CheckCharacterKidnapSpecificGender(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC5 RID: 6853 RVA: 0x0017970F File Offset: 0x0017790F
		[EventFunction(173)]
		private static ValueInfo CheckCharacterKidnapSpecificAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC6 RID: 6854 RVA: 0x00179717 File Offset: 0x00177917
		[EventFunction(174)]
		private static ValueInfo CheckCharacterKidnapSpecificId(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC7 RID: 6855 RVA: 0x0017971F File Offset: 0x0017791F
		[EventFunction(175)]
		private static ValueInfo CheckCharacterTeammateSpecificIdGender(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC8 RID: 6856 RVA: 0x00179727 File Offset: 0x00177927
		[EventFunction(176)]
		private static ValueInfo CheckCharacterTeammateSpecificIdAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AC9 RID: 6857 RVA: 0x0017972F File Offset: 0x0017792F
		[EventFunction(177)]
		private static ValueInfo CheckCharacterTeammateSpecificIdId(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACA RID: 6858 RVA: 0x00179737 File Offset: 0x00177937
		[EventFunction(178)]
		private static ValueInfo CheckCharacterExp(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACB RID: 6859 RVA: 0x0017973F File Offset: 0x0017793F
		[EventFunction(179)]
		private static ValueInfo CheckCharacterReadCombatSkillPageCount(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACC RID: 6860 RVA: 0x00179747 File Offset: 0x00177947
		[EventFunction(180)]
		private static ValueInfo CheckCharacterCombatSkillBreakout(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACD RID: 6861 RVA: 0x0017974F File Offset: 0x0017794F
		[EventFunction(181)]
		private static ValueInfo CheckAdventurePerMoveCount(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACE RID: 6862 RVA: 0x00179757 File Offset: 0x00177957
		[EventFunction(182)]
		private static ValueInfo CheckAdventurePerCostMovePoint(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001ACF RID: 6863 RVA: 0x0017975F File Offset: 0x0017795F
		[EventFunction(183)]
		private static ValueInfo CheckAdventureElementVisible(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AD0 RID: 6864 RVA: 0x00179767 File Offset: 0x00177967
		[EventFunction(185)]
		private static ValueInfo CheckAdventureCharacterGroup(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AD1 RID: 6865 RVA: 0x0017976F File Offset: 0x0017796F
		[EventFunction(186)]
		private static ValueInfo CheckAdventureElementGroup(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			throw new NotImplementedException();
		}

		// Token: 0x06001AD2 RID: 6866 RVA: 0x00179778 File Offset: 0x00177978
		[EventFunction(71)]
		private static ValueInfo CheckExpression(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			bool value = parameters[0].GetBoolValue(evaluator);
			return evaluator.PushEvaluationResult(value);
		}

		// Token: 0x06001AD3 RID: 6867 RVA: 0x001797A4 File Offset: 0x001779A4
		[EventFunction(109)]
		private static ValueInfo CheckAnd(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return runtime.Evaluator.PushEvaluationResult(true);
		}

		// Token: 0x06001AD4 RID: 6868 RVA: 0x001797C4 File Offset: 0x001779C4
		[EventFunction(110)]
		private static ValueInfo CheckOr(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			return runtime.Evaluator.PushEvaluationResult(true);
		}

		// Token: 0x06001AD5 RID: 6869 RVA: 0x001797E4 File Offset: 0x001779E4
		[EventFunction(159)]
		private static ValueInfo CheckCharacterAlive(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			ValueInfo valueInfo = parameters[0].Evaluate(evaluator);
			EValueType valueType = valueInfo.ValueType;
			EValueType evalueType = valueType;
			ValueInfo result;
			if (evalueType != EValueType.Int)
			{
				if (evalueType != EValueType.Obj)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(55, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unrecognized argument type: Character expected, ");
					defaultInterpolatedStringHandler.AppendFormatted<EValueType>(valueInfo.ValueType);
					defaultInterpolatedStringHandler.AppendLiteral(" given.");
					throw new ArgumentException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				GameData.Domains.Character.Character character = evaluator.EvaluationStack.PopObject<GameData.Domains.Character.Character>();
				result = evaluator.PushEvaluationResult(character != null);
			}
			else
			{
				int charId = evaluator.EvaluationStack.PopUnmanaged<int>();
				result = evaluator.PushEvaluationResult(DomainManager.Character.IsCharacterAlive(charId));
			}
			return result;
		}

		// Token: 0x06001AD6 RID: 6870 RVA: 0x0017989C File Offset: 0x00177A9C
		[EventFunction(213)]
		private static ValueInfo CheckCharacterPassMatcher(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			ValueInfo arg0 = parameters[0].Evaluate(evaluator);
			EValueType valueType = arg0.ValueType;
			EValueType evalueType = valueType;
			GameData.Domains.Character.Character character;
			if (evalueType != EValueType.Int)
			{
				if (evalueType != EValueType.Obj)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unrecognized argument type ");
					defaultInterpolatedStringHandler.AppendFormatted<EValueType>(arg0.ValueType);
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				object obj = evaluator.EvaluationStack.PopObject<object>();
				bool flag = obj != null && !(obj is EventActorData) && !(obj is GameData.Domains.Character.Character);
				if (flag)
				{
					DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(28, 1);
					defaultInterpolatedStringHandler.AppendLiteral("Unrecognized argument type ");
					defaultInterpolatedStringHandler.AppendFormatted<Type>(obj.GetType());
					defaultInterpolatedStringHandler.AppendLiteral(".");
					throw new InvalidCastException(defaultInterpolatedStringHandler.ToStringAndClear());
				}
				character = (obj as GameData.Domains.Character.Character);
			}
			else
			{
				int charId = evaluator.EvaluationStack.PopUnmanaged<int>();
				DomainManager.Character.TryGetElement_Objects(charId, out character);
			}
			bool flag2 = character == null;
			ValueInfo result2;
			if (flag2)
			{
				result2 = evaluator.PushEvaluationResult(false);
			}
			else
			{
				int matcherId = parameters[1].GetIntValue(evaluator);
				CharacterMatcherItem matcher = CharacterMatcher.Instance[matcherId];
				bool result = matcher.Match(character);
				result2 = evaluator.PushEvaluationResult(result);
			}
			return result2;
		}

		// Token: 0x06001AD7 RID: 6871 RVA: 0x001799F8 File Offset: 0x00177BF8
		[EventFunction(76)]
		private static ValueInfo CheckCharacterCurrMainAttribute(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int mainAttributeType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetCurrMainAttribute((sbyte)mainAttributeType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AD8 RID: 6872 RVA: 0x00179A5C File Offset: 0x00177C5C
		[EventFunction(77)]
		private static ValueInfo CheckCharacterMainAttribute(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int mainAttributeType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetMaxMainAttribute((sbyte)mainAttributeType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AD9 RID: 6873 RVA: 0x00179AC0 File Offset: 0x00177CC0
		[EventFunction(78)]
		private static ValueInfo CheckCharacterLifeSkillQualification(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int lifeSkillType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetLifeSkillQualification((sbyte)lifeSkillType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ADA RID: 6874 RVA: 0x00179B24 File Offset: 0x00177D24
		[EventFunction(79)]
		private static ValueInfo CheckCharacterLifeSkillAttainment(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int lifeSkillType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetLifeSkillAttainment((sbyte)lifeSkillType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ADB RID: 6875 RVA: 0x00179B88 File Offset: 0x00177D88
		[EventFunction(80)]
		private static ValueInfo CheckCharacterCombatSkillQualification(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int combatSkillType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetCombatSkillQualification((sbyte)combatSkillType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ADC RID: 6876 RVA: 0x00179BEC File Offset: 0x00177DEC
		[EventFunction(81)]
		private static ValueInfo CheckCharacterCombatSkillAttainment(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int combatSkillType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = character.GetCombatSkillAttainment((sbyte)combatSkillType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ADD RID: 6877 RVA: 0x00179C50 File Offset: 0x00177E50
		[EventFunction(82)]
		private static ValueInfo CheckCharacterPersonality(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int personalityType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			sbyte currValue = character.GetPersonality((sbyte)personalityType);
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001ADE RID: 6878 RVA: 0x00179CB4 File Offset: 0x00177EB4
		[EventFunction(107)]
		private static ValueInfo CheckCharacterBehaviorType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int expectedBehaviorType = parameters[1].GetIntValue(evaluator);
			sbyte currBehaviorType = character.GetBehaviorType();
			return evaluator.PushEvaluationResult(expectedBehaviorType == (int)currBehaviorType);
		}

		// Token: 0x06001ADF RID: 6879 RVA: 0x00179CF4 File Offset: 0x00177EF4
		[EventFunction(108)]
		private static ValueInfo CheckMorality(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			short currValue = character.GetMorality();
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AE0 RID: 6880 RVA: 0x00179D48 File Offset: 0x00177F48
		[EventFunction(83)]
		private static ValueInfo CheckCharacterResource(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int resourceType = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			int currValue = character.GetResource((sbyte)resourceType);
			bool result = EventConditions.PerformOperation(operatorId, currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AE1 RID: 6881 RVA: 0x00179DAC File Offset: 0x00177FAC
		[EventFunction(85)]
		private static ValueInfo CheckCharacterInventoryByTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			TemplateKey templateKey = parameters[1].GetObjectValue<UnmanagedVariant<TemplateKey>>(evaluator).Value;
			bool ret = character.GetInventory().GetInventoryItemKey(templateKey.ItemType, templateKey.TemplateId).IsValid();
			return evaluator.PushEvaluationResult(ret);
		}

		// Token: 0x06001AE2 RID: 6882 RVA: 0x00179E08 File Offset: 0x00178008
		[EventFunction(84)]
		private static ValueInfo CheckCharacterFeature(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			short featureId = (short)parameters[1].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult(character.GetFeatureIds().Contains(featureId));
		}

		// Token: 0x06001AE3 RID: 6883 RVA: 0x00179E48 File Offset: 0x00178048
		[EventFunction(140)]
		private static ValueInfo CheckCharacterCurrentProfession(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int professionId = parameters[1].GetIntValue(evaluator);
			ProfessionData profession = DomainManager.Extra.GetCharacterCurrentProfession(character.GetId());
			int actualProfessionId = (profession != null) ? profession.TemplateId : -1;
			return evaluator.PushEvaluationResult(actualProfessionId == professionId);
		}

		// Token: 0x06001AE4 RID: 6884 RVA: 0x00179EA4 File Offset: 0x001780A4
		[EventFunction(146)]
		private static ValueInfo TryGetCharacterCurrentProfession(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int characterId = parameters[0].GetIntValue(evaluator);
			string tempArgKey = parameters[1].GetStringValue(evaluator);
			ProfessionData profession = DomainManager.Extra.GetCharacterCurrentProfession(characterId);
			bool flag = string.IsNullOrEmpty(tempArgKey);
			ValueInfo result;
			if (flag)
			{
				result = evaluator.PushEvaluationResult(profession != null);
			}
			else
			{
				runtime.ArgBox.Set(tempArgKey, (profession != null) ? profession.TemplateId : -1);
				result = evaluator.PushEvaluationResult(profession != null);
			}
			return result;
		}

		// Token: 0x06001AE5 RID: 6885 RVA: 0x00179F1C File Offset: 0x0017811C
		[EventFunction(141)]
		private static ValueInfo CheckCharacterSeniorityPercent(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int professionId = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			ProfessionData professionData = (professionId >= 0) ? DomainManager.Extra.GetCharacterProfessionData(character.GetId(), professionId) : DomainManager.Extra.GetCharacterCurrentProfession(character.GetId());
			int actualValue = (professionData != null) ? professionData.GetSeniorityPercent() : 0;
			bool result = EventConditions.PerformOperation(operatorId, actualValue, requiredValue);
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				runtime.RecordConditionHint(141, result, new string[]
				{
					string.Empty,
					(professionId >= 0) ? Profession.Instance[professionId].Name : string.Empty,
					EventConditionOperator.Instance[operatorId].Name,
					requiredValue.ToString()
				});
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AE6 RID: 6886 RVA: 0x0017A010 File Offset: 0x00178210
		[EventFunction(103)]
		private static ValueInfo CheckCharacterGrade(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			bool result = EventConditions.PerformOperation(operatorId, (int)character.GetOrganizationInfo().Grade, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AE7 RID: 6887 RVA: 0x0017A068 File Offset: 0x00178268
		[EventFunction(104)]
		private static ValueInfo CheckCharacterSettlement(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Settlement settlement = parameters[1].GetAnyValue<Settlement>(evaluator);
			bool flag = settlement == null;
			ValueInfo result;
			if (flag)
			{
				result = evaluator.PushEvaluationResult(character.GetOrganizationInfo().SettlementId < 0);
			}
			else
			{
				result = evaluator.PushEvaluationResult(character.GetOrganizationInfo().SettlementId == settlement.GetId());
			}
			return result;
		}

		// Token: 0x06001AE8 RID: 6888 RVA: 0x0017A0D0 File Offset: 0x001782D0
		[EventFunction(86)]
		private static ValueInfo CheckCharacterOnSettlementBlock(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Settlement settlement = parameters[1].GetAnyValue<Settlement>(evaluator);
			bool flag = settlement == null;
			ValueInfo result2;
			if (flag)
			{
				result2 = evaluator.PushEvaluationResult(false);
			}
			else
			{
				short settlementId = settlement.GetId();
				Location location = character.GetValidLocation();
				bool result = DomainManager.Map.IsLocationOnSettlementBlock(location, settlementId);
				result2 = evaluator.PushEvaluationResult(result);
			}
			return result2;
		}

		// Token: 0x06001AE9 RID: 6889 RVA: 0x0017A13C File Offset: 0x0017833C
		[EventFunction(87)]
		private static ValueInfo CheckCharacterInSettlementInfluenceRange(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Settlement settlement = parameters[1].GetAnyValue<Settlement>(evaluator);
			Location location = character.GetValidLocation();
			short settlementId = settlement.GetId();
			bool result = DomainManager.Map.IsLocationInSettlementInfluenceRange(location, settlementId);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AEA RID: 6890 RVA: 0x0017A194 File Offset: 0x00178394
		[EventFunction(168)]
		private static ValueInfo CheckCharacterInMapBlockRange(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Location characterLocation = character.GetLocation();
			bool flag = !character.GetLocation().IsValid();
			ValueInfo result2;
			if (flag)
			{
				result2 = evaluator.PushEvaluationResult(false);
			}
			else
			{
				MapBlockData targetBlock = parameters[1].GetAnyValue<MapBlockData>(evaluator);
				bool flag2 = targetBlock == null;
				if (flag2)
				{
					result2 = evaluator.PushEvaluationResult(false);
				}
				else
				{
					Location targetLocation = targetBlock.GetLocation();
					int distance = parameters[2].GetIntValue(evaluator);
					bool result = characterLocation.IsNearbyLocation(targetLocation, distance);
					result2 = evaluator.PushEvaluationResult(result);
				}
			}
			return result2;
		}

		// Token: 0x06001AEB RID: 6891 RVA: 0x0017A22C File Offset: 0x0017842C
		[EventFunction(95)]
		private static ValueInfo CheckCharacterInMapState(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int stateTemplateId = parameters[1].GetIntValue(evaluator);
			Location location = character.GetValidLocation();
			sbyte charStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
			bool result = stateTemplateId == (int)charStateTemplateId;
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AEC RID: 6892 RVA: 0x0017A284 File Offset: 0x00178484
		[EventFunction(96)]
		private static ValueInfo CheckCharacterInMapArea(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int areaTemplateId = parameters[1].GetIntValue(evaluator);
			Location location = character.GetValidLocation();
			MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
			bool result = (int)areaData.GetTemplateId() == areaTemplateId;
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AED RID: 6893 RVA: 0x0017A2E0 File Offset: 0x001784E0
		[EventFunction(97)]
		private static ValueInfo CheckCharacterOnAnySettlement(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Location location = character.GetValidLocation();
			MapBlockData rootBlock = DomainManager.Map.GetBlock(location).GetRootBlock();
			bool result = rootBlock.IsCityTown();
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AEE RID: 6894 RVA: 0x0017A330 File Offset: 0x00178530
		[EventFunction(98)]
		private static ValueInfo CheckCharacterInAnySettlementInfluenceRange(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Location location = character.GetValidLocation();
			MapBlockData belongSettlementBlock = DomainManager.Map.GetBelongSettlementBlock(location);
			bool result = belongSettlementBlock != null;
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AEF RID: 6895 RVA: 0x0017A378 File Offset: 0x00178578
		[EventFunction(194)]
		private static ValueInfo CheckCharacterInSettlementArea(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			Settlement settlement = parameters[1].GetAnyValue<Settlement>(evaluator);
			bool result = character.GetValidLocation().AreaId == settlement.GetLocation().AreaId;
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF0 RID: 6896 RVA: 0x0017A3C8 File Offset: 0x001785C8
		[EventFunction(88)]
		private static ValueInfo CheckCharacterFavorability(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			GameData.Domains.Character.Character relatedChar = parameters[1].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			short currValue = DomainManager.Character.GetFavorability(character.GetId(), relatedChar.GetId());
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF1 RID: 6897 RVA: 0x0017A438 File Offset: 0x00178638
		[EventFunction(142)]
		private static ValueInfo CheckCharacterFavorabilityType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			GameData.Domains.Character.Character relatedChar = parameters[1].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			sbyte currValue = DomainManager.Character.GetFavorabilityType(character.GetId(), relatedChar.GetId());
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				int funcId = 142;
				bool result2 = result;
				string[] array = new string[4];
				array[0] = string.Empty;
				array[1] = string.Empty;
				array[2] = EventConditionOperator.Instance[operatorId].Name;
				int num = 3;
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("<Language Key=LK_Favor_Type_");
				defaultInterpolatedStringHandler.AppendFormatted<int>(requiredValue - -6);
				defaultInterpolatedStringHandler.AppendLiteral("/>");
				array[num] = defaultInterpolatedStringHandler.ToStringAndClear();
				runtime.RecordConditionHint(funcId, result2, array);
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF2 RID: 6898 RVA: 0x0017A528 File Offset: 0x00178728
		[EventFunction(121)]
		private static ValueInfo CheckCharacterHasItem(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			UnmanagedVariant<TemplateKey> templateKey = parameters[1].GetAnyValue<UnmanagedVariant<TemplateKey>>(evaluator);
			Inventory inventory = character.GetInventory();
			return evaluator.PushEvaluationResult(inventory.GetInventoryItemKey(templateKey.Value.ItemType, templateKey.Value.TemplateId).IsValid());
		}

		// Token: 0x06001AF3 RID: 6899 RVA: 0x0017A58C File Offset: 0x0017878C
		[EventFunction(122)]
		private static ValueInfo CheckCharacterMerchantType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int merchantType = parameters[1].GetIntValue(evaluator);
			sbyte expectedMerchantType = DomainManager.Extra.GetMerchantCharToType(character.GetId());
			return evaluator.PushEvaluationResult(merchantType == (int)expectedMerchantType);
		}

		// Token: 0x06001AF4 RID: 6900 RVA: 0x0017A5D8 File Offset: 0x001787D8
		[EventFunction(157)]
		private static ValueInfo CheckCharacterConsummateLevel(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			sbyte actualValue = character.GetConsummateLevel();
			bool result = EventConditions.PerformOperation(operatorId, (int)actualValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF5 RID: 6901 RVA: 0x0017A62C File Offset: 0x0017882C
		[EventFunction(160)]
		private static ValueInfo CheckCharacterCurrAge(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			short actualValue = character.GetCurrAge();
			bool result = EventConditions.PerformOperation(operatorId, (int)actualValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF6 RID: 6902 RVA: 0x0017A680 File Offset: 0x00178880
		[EventFunction(161)]
		private static ValueInfo CheckCharacterActualAge(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			short actualValue = character.GetActualAge();
			bool result = EventConditions.PerformOperation(operatorId, (int)actualValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF7 RID: 6903 RVA: 0x0017A6D4 File Offset: 0x001788D4
		[EventFunction(162)]
		private static ValueInfo CheckCharacterAgeGroup(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			sbyte actualValue = character.GetAgeGroup();
			bool result = EventConditions.PerformOperation(operatorId, (int)actualValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF8 RID: 6904 RVA: 0x0017A728 File Offset: 0x00178928
		[EventFunction(203)]
		private static ValueInfo CheckCharacterGender(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int requiredValue = parameters[1].GetIntValue(evaluator);
			bool result = (int)character.GetGender() == requiredValue;
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AF9 RID: 6905 RVA: 0x0017A768 File Offset: 0x00178968
		[EventFunction(128)]
		private static ValueInfo CheckFixedCharacterTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int templateId = parameters[1].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult((int)character.GetTemplateId() == templateId);
		}

		// Token: 0x06001AFA RID: 6906 RVA: 0x0017A7A4 File Offset: 0x001789A4
		[EventFunction(131)]
		private static ValueInfo CheckCharacterReadLifeSkillPageCount(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int lifeSkillTemplateId = parameters[1].GetIntValue(evaluator);
			int operatorId = parameters[2].GetIntValue(evaluator);
			int requiredValue = parameters[3].GetIntValue(evaluator);
			List<GameData.Domains.Character.LifeSkillItem> learnedLifeSkills = character.GetLearnedLifeSkills();
			int lifeSkillIndex = character.FindLearnedLifeSkillIndex((short)lifeSkillTemplateId);
			int actualValue = (lifeSkillIndex >= 0) ? learnedLifeSkills[lifeSkillIndex].GetReadPagesCount() : 0;
			bool result = EventConditions.PerformOperation(operatorId, actualValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AFB RID: 6907 RVA: 0x0017A82C File Offset: 0x00178A2C
		[EventFunction(257)]
		private static ValueInfo CheckTaiwuHasFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int operatorId = parameters[0].GetIntValue(evaluator);
			int requiredValue = parameters[1].GetIntValue(evaluator);
			bool result = EventConditions.PerformOperation(operatorId, DomainManager.Extra.GetFuyuFaith(), requiredValue);
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				runtime.RecordConditionHint(257, result, new string[]
				{
					EventConditionOperator.Instance[operatorId].Name,
					requiredValue.ToString()
				});
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AFC RID: 6908 RVA: 0x0017A8B0 File Offset: 0x00178AB0
		[EventFunction(258)]
		private static ValueInfo CheckCharacterFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			bool result = EventConditions.PerformOperation(operatorId, character.GetDarkAshCounter().Tips3, requiredValue);
			bool recordingConditionHints = runtime.RecordingConditionHints;
			if (recordingConditionHints)
			{
				runtime.RecordConditionHint(258, result, new string[]
				{
					DomainManager.Character.GetName(character.GetId(), false),
					EventConditionOperator.Instance[operatorId].Name,
					requiredValue.ToString()
				});
			}
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001AFD RID: 6909 RVA: 0x0017A958 File Offset: 0x00178B58
		[EventFunction(256)]
		private static ValueInfo TryGetMaxAcceptableFuyuFaith(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			GameData.Domains.Character.Character character = parameters[0].GetAnyValue<GameData.Domains.Character.Character>(evaluator);
			string key = parameters[1].GetStringValue(evaluator);
			runtime.ArgBox.Set(key, EventHelper.GetMaxAcceptableFuyuFaith(character));
			return evaluator.PushEvaluationResult(true);
		}

		// Token: 0x06001AFE RID: 6910 RVA: 0x0017A9A0 File Offset: 0x00178BA0
		[EventFunction(136)]
		private static ValueInfo CheckItemType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			ItemKey itemKey = parameters[0].GetAnyValue<ItemKey>(evaluator);
			int expectedItemType = parameters[1].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult((int)itemKey.ItemType == expectedItemType);
		}

		// Token: 0x06001AFF RID: 6911 RVA: 0x0017A9DC File Offset: 0x00178BDC
		[EventFunction(137)]
		private static ValueInfo CheckItemSubType(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			ItemKey itemKey = parameters[0].GetAnyValue<ItemKey>(evaluator);
			short actualItemSubType = ItemTemplateHelper.GetItemSubType(itemKey.ItemType, itemKey.TemplateId);
			int expectedItemSubType = parameters[1].GetIntValue(evaluator);
			return evaluator.PushEvaluationResult(expectedItemSubType == (int)actualItemSubType);
		}

		// Token: 0x06001B00 RID: 6912 RVA: 0x0017AA28 File Offset: 0x00178C28
		[EventFunction(138)]
		private static ValueInfo CheckItemTemplate(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			ItemKey itemKey = parameters[0].GetAnyValue<ItemKey>(evaluator);
			TemplateKey itemTemplate = parameters[1].GetAnyValue<UnmanagedVariant<TemplateKey>>(evaluator).Value;
			return evaluator.PushEvaluationResult(itemTemplate.ItemType == itemKey.ItemType && itemTemplate.TemplateId == itemKey.TemplateId);
		}

		// Token: 0x06001B01 RID: 6913 RVA: 0x0017AA80 File Offset: 0x00178C80
		[EventFunction(106)]
		private static ValueInfo CheckSettlementInMapArea(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			Settlement settlement = parameters[0].GetAnyValue<Settlement>(evaluator);
			int areaTemplateId = parameters[1].GetIntValue(evaluator);
			bool flag = settlement == null;
			ValueInfo result2;
			if (flag)
			{
				result2 = evaluator.PushEvaluationResult(false);
			}
			else
			{
				Location location = settlement.GetLocation();
				MapAreaData areaData = DomainManager.Map.GetElement_Areas((int)location.AreaId);
				bool result = (int)areaData.GetTemplateId() == areaTemplateId;
				result2 = evaluator.PushEvaluationResult(result);
			}
			return result2;
		}

		// Token: 0x06001B02 RID: 6914 RVA: 0x0017AAF4 File Offset: 0x00178CF4
		[EventFunction(105)]
		private static ValueInfo CheckSettlementInMapState(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			Settlement settlement = parameters[0].GetAnyValue<Settlement>(evaluator);
			int stateTemplateId = parameters[1].GetIntValue(evaluator);
			bool flag = settlement == null;
			ValueInfo result2;
			if (flag)
			{
				result2 = evaluator.PushEvaluationResult(false);
			}
			else
			{
				Location location = settlement.GetLocation();
				sbyte settlementStateTemplateId = DomainManager.Map.GetStateTemplateIdByAreaId(location.AreaId);
				bool result = stateTemplateId == (int)settlementStateTemplateId;
				result2 = evaluator.PushEvaluationResult(result);
			}
			return result2;
		}

		// Token: 0x06001B03 RID: 6915 RVA: 0x0017AB60 File Offset: 0x00178D60
		[EventFunction(112)]
		private static ValueInfo CheckAreaSpiritualDebt(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int areaTemplateId = parameters[0].GetIntValue(evaluator);
			int operatorId = parameters[1].GetIntValue(evaluator);
			int requiredValue = parameters[2].GetIntValue(evaluator);
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)areaTemplateId);
			int spiritualDebt = DomainManager.Extra.GetAreaSpiritualDebt(areaId);
			bool result = EventConditions.PerformOperation(operatorId, spiritualDebt, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B04 RID: 6916 RVA: 0x0017ABC8 File Offset: 0x00178DC8
		[EventFunction(204)]
		private static ValueInfo CheckAreaHasAdventure(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int areaTemplateId = parameters[0].GetIntValue(evaluator);
			int adventureId = parameters[1].GetIntValue(evaluator);
			short areaId = DomainManager.Map.GetAreaIdByAreaTemplateId((short)areaTemplateId);
			AreaAdventureData adventuresInArea = DomainManager.Adventure.GetAdventuresInArea(areaId);
			foreach (AdventureSiteData adventureSite in adventuresInArea.AdventureSites.Values)
			{
				bool flag = (int)adventureSite.TemplateId == adventureId && adventureSite.SiteState >= 1;
				if (flag)
				{
					return evaluator.PushEvaluationResult(true);
				}
			}
			return evaluator.PushEvaluationResult(false);
		}

		// Token: 0x06001B05 RID: 6917 RVA: 0x0017AC8C File Offset: 0x00178E8C
		[EventFunction(158)]
		private static ValueInfo CheckIsDreamBack(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			return evaluator.PushEvaluationResult(DomainManager.Extra.GetIsDreamBack());
		}

		// Token: 0x06001B06 RID: 6918 RVA: 0x0017ACB8 File Offset: 0x00178EB8
		[EventFunction(72)]
		private static ValueInfo CheckMainStoryProgress(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int operatorId = parameters[0].GetIntValue(evaluator);
			int requiredValue = parameters[1].GetIntValue(evaluator);
			short currValue = DomainManager.World.GetMainStoryLineProgress();
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B07 RID: 6919 RVA: 0x0017AD04 File Offset: 0x00178F04
		[EventFunction(155)]
		private static ValueInfo CheckWorldFunctionStatus(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			byte worldFunctionType = (byte)parameters[0].GetIntValue(evaluator);
			bool result = DomainManager.World.GetWorldFunctionsStatus(worldFunctionType);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B08 RID: 6920 RVA: 0x0017AD3C File Offset: 0x00178F3C
		[EventFunction(73)]
		private static ValueInfo CheckTask(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int taskInfoId = parameters[0].GetIntValue(evaluator);
			bool result = DomainManager.Extra.IsExtraTaskInProgress(taskInfoId);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B09 RID: 6921 RVA: 0x0017AD74 File Offset: 0x00178F74
		[EventFunction(74)]
		private static ValueInfo CheckTaskChain(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int taskChainId = parameters[0].GetIntValue(evaluator);
			bool result = DomainManager.Extra.IsExtraTaskChainInProgress(taskChainId);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B0A RID: 6922 RVA: 0x0017ADAC File Offset: 0x00178FAC
		[EventFunction(75)]
		private static ValueInfo CheckXiangshuLevel(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			int operatorId = parameters[0].GetIntValue(evaluator);
			int requiredValue = parameters[0].GetIntValue(evaluator);
			sbyte currValue = DomainManager.World.GetXiangshuLevel();
			bool result = EventConditions.PerformOperation(operatorId, (int)currValue, requiredValue);
			return evaluator.PushEvaluationResult(result);
		}

		// Token: 0x06001B0B RID: 6923 RVA: 0x0017ADF8 File Offset: 0x00178FF8
		[EventFunction(123)]
		private static ValueInfo CheckSectMainStoryValueExists(EventScriptRuntime runtime, ASTNode[] parameters)
		{
			Evaluator evaluator = runtime.Evaluator;
			sbyte orgTemplateId = (sbyte)parameters[0].GetIntValue(evaluator);
			string sectMainStoryArgKey = parameters[1].GetStringValue(evaluator);
			string tempArgKey = parameters[2].GetStringValue(evaluator);
			EventArgBox argBox = DomainManager.Extra.GetSectMainStoryEventArgBox(orgTemplateId);
			ValueInfo valueInfo = argBox.SelectValue(evaluator, sectMainStoryArgKey);
			bool flag = valueInfo.ValueType == EValueType.Void;
			ValueInfo result;
			if (flag)
			{
				result = evaluator.PushEvaluationResult(false);
			}
			else
			{
				bool flag2 = string.IsNullOrEmpty(tempArgKey);
				if (flag2)
				{
					evaluator.RemoveTopValue(valueInfo);
					result = evaluator.PushEvaluationResult(true);
				}
				else
				{
					runtime.ArgBox.SetValueFromStack(evaluator.EvaluationStack, tempArgKey, valueInfo.ValueType);
					result = evaluator.PushEvaluationResult(true);
				}
			}
			return result;
		}
	}
}
