using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains;
using GameData.Domains.Adventure.Modifications;
using GameData.Domains.Building;
using GameData.Domains.Character;
using GameData.Domains.Character.Ai;
using GameData.Domains.Character.ParallelModifications;
using GameData.Domains.CombatSkill;
using GameData.Domains.Extra;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Common;

public class ParallelModificationsRecorder
{
	private const int DataPoolInitialCapacity = 1048576;

	private const int ObjectsInitialCapacity = 16384;

	private readonly RawDataPool _dataPool;

	private readonly List<object> _objects;

	public ParallelModificationsRecorder()
	{
		_dataPool = new RawDataPool(1048576);
		_objects = new List<object>(16384);
	}

	public void RecordType(ParallelModificationType type)
	{
		_dataPool.AddUnmanaged(type);
	}

	public void RecordParameterUnmanaged<T>(T value) where T : unmanaged
	{
		_dataPool.AddUnmanaged(value);
	}

	public unsafe void RecordParameterNonTrivialStruct<T>(T value) where T : struct, ISerializableGameData
	{
		byte* ptr = default(byte*);
		_dataPool.Allocate(((ISerializableGameData)value/*cast due to .constrained prefix*/).GetSerializedSize(), &ptr);
		((ISerializableGameData)value/*cast due to .constrained prefix*/).Serialize(ptr);
	}

	public void RecordParameterClass<T>(T value) where T : class
	{
		int count = _objects.Count;
		_dataPool.AddUnmanaged(count);
		_objects.Add(value);
	}

	public unsafe void ApplyAll(DataContext context)
	{
		int rawDataSize = _dataPool.RawDataSize;
		if (rawDataSize > 0)
		{
			byte* ptr = _dataPool.GetPointer(0);
			byte* ptr2 = ptr + rawDataSize;
			while (ptr < ptr2)
			{
				ptr = Apply(context, ptr);
			}
			_dataPool.Clear();
			_objects.Clear();
		}
	}

	private unsafe byte* Apply(DataContext context, byte* pData)
	{
		ParallelModificationType parallelModificationType = *(ParallelModificationType*)pData;
		pData += 2;
		switch (parallelModificationType)
		{
		case ParallelModificationType.CreateIntelligentCharacter:
		{
			CreateIntelligentCharacterModification mod17 = (CreateIntelligentCharacterModification)_objects[*(int*)pData];
			pData += 4;
			bool autoComplement = *pData != 0;
			pData++;
			DomainManager.Character.ComplementCreateIntelligentCharacter(context, mod17, autoComplement);
			return pData;
		}
		case ParallelModificationType.CreateNewbornChildren:
		{
			CreateNewBornChildrenModification mod16 = (CreateNewBornChildrenModification)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Character.ComplementCreateNewbornChildren(context, mod16);
			return pData;
		}
		case ParallelModificationType.SetInitialCombatSkillBreakouts:
		{
			List<CombatSkillInitialBreakoutData> brokenOutSkills = (List<CombatSkillInitialBreakoutData>)_objects[*(int*)pData];
			pData += 4;
			Character character6 = (Character)_objects[*(int*)pData];
			pData += 4;
			NeiliProportionOfFiveElements neiliProportion = *(NeiliProportionOfFiveElements*)pData;
			pData += sizeof(NeiliProportionOfFiveElements);
			int[] extraNeiliAllocationProgress = (int[])_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementSetInitialCombatSkillBreakouts(context, brokenOutSkills, character6, neiliProportion, extraNeiliAllocationProgress);
			return pData;
		}
		case ParallelModificationType.SetInitialCombatSkillAttainmentPanels:
		{
			Character character5 = (Character)_objects[*(int*)pData];
			pData += 4;
			short[] panels = (short[])_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementSetInitialCombatSkillAttainmentPanels(context, character5, panels);
			return pData;
		}
		case ParallelModificationType.PracticeAndBreakoutCombatSkills:
		{
			PracticeAndBreakoutModification mod15 = (PracticeAndBreakoutModification)_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementPracticeAndBreakoutCombatSkill(context, mod15);
			return pData;
		}
		case ParallelModificationType.ActivateCombatSkillPages:
		{
			List<(CombatSkill, ushort)> newlyActivatedCombatSkills = (List<(CombatSkill, ushort)>)_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementActivateCombatSkillPages(context, newlyActivatedCombatSkills);
			return pData;
		}
		case ParallelModificationType.UpdateBreakPlateBonuses:
		{
			UpdateBreakPlateBonusesModification mod14 = (UpdateBreakPlateBonusesModification)_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementUpdateBreakPlateBonuses(context, mod14);
			return pData;
		}
		case ParallelModificationType.SelectEquipments:
		{
			SelectEquipmentsModification mod13 = (SelectEquipmentsModification)_objects[*(int*)pData];
			pData += 4;
			Equipping.ComplementSelectEquipments(context, mod13);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthMixedPoisonEffect:
		{
			Character character4 = (Character)_objects[*(int*)pData];
			pData += 4;
			List<(sbyte, int)> mixedPoisonInfoList = (List<(sbyte, int)>)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_MixedPoisonEffect(context, character4, mixedPoisonInfoList);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthUpdateStatus:
		{
			PeriAdvanceMonthUpdateStatusModification mod12 = (PeriAdvanceMonthUpdateStatusModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_UpdateStatus(context, mod12);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthSelfImprovement:
		{
			PeriAdvanceMonthSelfImprovementModification mod11 = (PeriAdvanceMonthSelfImprovementModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_SelfImprovement(context, mod11);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthSelfImprovementLearnNewSkills:
		{
			Character character3 = (Character)_objects[*(int*)pData];
			pData += 4;
			(short, short, byte) combatSkillToLearn = Unsafe.Read<(short, short, byte)>(pData);
			pData += Unsafe.SizeOf<(short, short, byte)>();
			(short, short, byte) lifeSkillToLearn = Unsafe.Read<(short, short, byte)>(pData);
			pData += Unsafe.SizeOf<(short, short, byte)>();
			Character.ComplementPeriAdvanceMonth_SelfImprovement_LearnNewSkills(context, character3, combatSkillToLearn, lifeSkillToLearn);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthActivePreparationGetSupply:
		{
			PeriAdvanceMonthGetSupplyModification mod10 = (PeriAdvanceMonthGetSupplyModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_ActivePreparation_GetSupply(context, mod10);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthActivePreparation:
		{
			PeriAdvanceMonthActivePreparationModification mod9 = (PeriAdvanceMonthActivePreparationModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_ActivePreparation(context, mod9);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthPassivePreparation:
		{
			PeriAdvanceMonthPassivePreparationModification mod8 = (PeriAdvanceMonthPassivePreparationModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_PassivePreparation(context, mod8);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthRelationsUpdate:
		{
			PeriAdvanceMonthRelationsUpdateModification mod7 = (PeriAdvanceMonthRelationsUpdateModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_RelationsUpdate(context, mod7);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthPersonalNeedsProcessing:
		{
			Character character2 = (Character)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_PersonalNeedsProcessing(context, character2);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthExecutePrioritizedAction:
		{
			PrioritizedActionModification mod6 = (PrioritizedActionModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_ExecutePrioritizedAction(context, mod6);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthExecuteGeneralActions:
		{
			PeriAdvanceMonthGeneralActionModification mod5 = (PeriAdvanceMonthGeneralActionModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_ExecuteGeneralActions(context, mod5);
			return pData;
		}
		case ParallelModificationType.PeriAdvanceMonthExecuteFixedActions:
		{
			PeriAdvanceMonthFixedActionModification mod4 = (PeriAdvanceMonthFixedActionModification)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPeriAdvanceMonth_ExecuteFixedActions(context, mod4);
			return pData;
		}
		case ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate:
		{
			Character character = (Character)_objects[*(int*)pData];
			pData += 4;
			Character.ComplementPostAdvanceMonth_PersonalNeedsUpdate(context, character);
			return pData;
		}
		case ParallelModificationType.UpdateMapArea:
		{
			ParallelMapAreaModification mod3 = (ParallelMapAreaModification)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Map.ComplementUpdateMapArea(context, mod3);
			return pData;
		}
		case ParallelModificationType.UpdateBrokenArea:
		{
			MapBlockData block = (MapBlockData)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Map.SetBlockData(context, block);
			return pData;
		}
		case ParallelModificationType.UpdateBuilding:
		{
			ParallelBuildingModification modification = (ParallelBuildingModification)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Building.ComplementUpdateBuilding(context, modification);
			return pData;
		}
		case ParallelModificationType.PreAdvanceMonthUpdateRandomEnemies:
		{
			PreAdvanceMonthRandomEnemiesModification mod2 = (PreAdvanceMonthRandomEnemiesModification)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Adventure.ComplementPreAdvanceMonth_UpdateRandomEnemies(context, mod2);
			return pData;
		}
		case ParallelModificationType.PreAdvanceMonthUpdateNpcTaming:
		{
			PreAdvanceMonthNpcTamingModification mod = (PreAdvanceMonthNpcTamingModification)_objects[*(int*)pData];
			pData += 4;
			DomainManager.Extra.ComplementPreAdvanceMonth_UpdateNpcTaming(context, mod);
			return pData;
		}
		default:
			throw new Exception($"Unsupported ParallelModificationType: {parallelModificationType}");
		}
	}
}
