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

namespace GameData.Common
{
	// Token: 0x020008FB RID: 2299
	public class ParallelModificationsRecorder
	{
		// Token: 0x0600824C RID: 33356 RVA: 0x004DA299 File Offset: 0x004D8499
		public ParallelModificationsRecorder()
		{
			this._dataPool = new RawDataPool(1048576);
			this._objects = new List<object>(16384);
		}

		// Token: 0x0600824D RID: 33357 RVA: 0x004DA2C3 File Offset: 0x004D84C3
		public void RecordType(ParallelModificationType type)
		{
			this._dataPool.AddUnmanaged<ParallelModificationType>(type);
		}

		// Token: 0x0600824E RID: 33358 RVA: 0x004DA2D3 File Offset: 0x004D84D3
		public void RecordParameterUnmanaged<[IsUnmanaged] T>(T value) where T : struct, ValueType
		{
			this._dataPool.AddUnmanaged<T>(value);
		}

		// Token: 0x0600824F RID: 33359 RVA: 0x004DA2E4 File Offset: 0x004D84E4
		public unsafe void RecordParameterNonTrivialStruct<T>(T value) where T : struct, ISerializableGameData
		{
			byte* pData;
			this._dataPool.Allocate(value.GetSerializedSize(), &pData);
			value.Serialize(pData);
		}

		// Token: 0x06008250 RID: 33360 RVA: 0x004DA320 File Offset: 0x004D8520
		public void RecordParameterClass<T>(T value) where T : class
		{
			int index = this._objects.Count;
			this._dataPool.AddUnmanaged<int>(index);
			this._objects.Add(value);
		}

		// Token: 0x06008251 RID: 33361 RVA: 0x004DA35C File Offset: 0x004D855C
		public unsafe void ApplyAll(DataContext context)
		{
			int dataSize = this._dataPool.RawDataSize;
			bool flag = dataSize <= 0;
			if (!flag)
			{
				byte* pData = this._dataPool.GetPointer(0);
				byte* pEnd = pData + dataSize;
				while (pData < pEnd)
				{
					pData = this.Apply(context, pData);
				}
				this._dataPool.Clear();
				this._objects.Clear();
			}
		}

		// Token: 0x06008252 RID: 33362 RVA: 0x004DA3C4 File Offset: 0x004D85C4
		private unsafe byte* Apply(DataContext context, byte* pData)
		{
			ParallelModificationType type = (ParallelModificationType)(*(ushort*)pData);
			pData += 2;
			byte* result;
			switch (type)
			{
			case ParallelModificationType.CreateIntelligentCharacter:
			{
				CreateIntelligentCharacterModification mod = (CreateIntelligentCharacterModification)this._objects[*(int*)pData];
				pData += 4;
				bool autoComplement = *pData != 0;
				pData++;
				DomainManager.Character.ComplementCreateIntelligentCharacter(context, mod, autoComplement);
				result = pData;
				break;
			}
			case ParallelModificationType.CreateNewbornChildren:
			{
				CreateNewBornChildrenModification mod2 = (CreateNewBornChildrenModification)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Character.ComplementCreateNewbornChildren(context, mod2);
				result = pData;
				break;
			}
			case ParallelModificationType.SetInitialCombatSkillBreakouts:
			{
				List<CombatSkillInitialBreakoutData> brokenOutSkills = (List<CombatSkillInitialBreakoutData>)this._objects[*(int*)pData];
				pData += 4;
				Character character = (Character)this._objects[*(int*)pData];
				pData += 4;
				NeiliProportionOfFiveElements neiliProportion = *(NeiliProportionOfFiveElements*)pData;
				pData += sizeof(NeiliProportionOfFiveElements);
				int[] extraNeiliAllocationProgress = (int[])this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementSetInitialCombatSkillBreakouts(context, brokenOutSkills, character, neiliProportion, extraNeiliAllocationProgress);
				result = pData;
				break;
			}
			case ParallelModificationType.SetInitialCombatSkillAttainmentPanels:
			{
				Character character2 = (Character)this._objects[*(int*)pData];
				pData += 4;
				short[] panels = (short[])this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementSetInitialCombatSkillAttainmentPanels(context, character2, panels);
				result = pData;
				break;
			}
			case ParallelModificationType.PracticeAndBreakoutCombatSkills:
			{
				PracticeAndBreakoutModification mod3 = (PracticeAndBreakoutModification)this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementPracticeAndBreakoutCombatSkill(context, mod3);
				result = pData;
				break;
			}
			case ParallelModificationType.ActivateCombatSkillPages:
			{
				List<ValueTuple<CombatSkill, ushort>> newlyActivatedCombatSkills = (List<ValueTuple<CombatSkill, ushort>>)this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementActivateCombatSkillPages(context, newlyActivatedCombatSkills);
				result = pData;
				break;
			}
			case ParallelModificationType.UpdateBreakPlateBonuses:
			{
				UpdateBreakPlateBonusesModification mod4 = (UpdateBreakPlateBonusesModification)this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementUpdateBreakPlateBonuses(context, mod4);
				result = pData;
				break;
			}
			case ParallelModificationType.SelectEquipments:
			{
				SelectEquipmentsModification mod5 = (SelectEquipmentsModification)this._objects[*(int*)pData];
				pData += 4;
				Equipping.ComplementSelectEquipments(context, mod5);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthMixedPoisonEffect:
			{
				Character character3 = (Character)this._objects[*(int*)pData];
				pData += 4;
				List<ValueTuple<sbyte, int>> mixedPoisonInfoList = (List<ValueTuple<sbyte, int>>)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_MixedPoisonEffect(context, character3, mixedPoisonInfoList);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthUpdateStatus:
			{
				PeriAdvanceMonthUpdateStatusModification mod6 = (PeriAdvanceMonthUpdateStatusModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_UpdateStatus(context, mod6);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthSelfImprovement:
			{
				PeriAdvanceMonthSelfImprovementModification mod7 = (PeriAdvanceMonthSelfImprovementModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_SelfImprovement(context, mod7);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthSelfImprovementLearnNewSkills:
			{
				Character character4 = (Character)this._objects[*(int*)pData];
				pData += 4;
				ValueTuple<short, short, byte> combatSkillToLearn = *(ValueTuple<short, short, byte>*)pData;
				pData += sizeof(ValueTuple<short, short, byte>);
				ValueTuple<short, short, byte> lifeSkillToLearn = *(ValueTuple<short, short, byte>*)pData;
				pData += sizeof(ValueTuple<short, short, byte>);
				Character.ComplementPeriAdvanceMonth_SelfImprovement_LearnNewSkills(context, character4, combatSkillToLearn, lifeSkillToLearn);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthActivePreparationGetSupply:
			{
				PeriAdvanceMonthGetSupplyModification mod8 = (PeriAdvanceMonthGetSupplyModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_ActivePreparation_GetSupply(context, mod8);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthActivePreparation:
			{
				PeriAdvanceMonthActivePreparationModification mod9 = (PeriAdvanceMonthActivePreparationModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_ActivePreparation(context, mod9);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthPassivePreparation:
			{
				PeriAdvanceMonthPassivePreparationModification mod10 = (PeriAdvanceMonthPassivePreparationModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_PassivePreparation(context, mod10);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthRelationsUpdate:
			{
				PeriAdvanceMonthRelationsUpdateModification mod11 = (PeriAdvanceMonthRelationsUpdateModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_RelationsUpdate(context, mod11);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthPersonalNeedsProcessing:
			{
				Character character5 = (Character)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_PersonalNeedsProcessing(context, character5);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthExecutePrioritizedAction:
			{
				PrioritizedActionModification mod12 = (PrioritizedActionModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_ExecutePrioritizedAction(context, mod12);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthExecuteGeneralActions:
			{
				PeriAdvanceMonthGeneralActionModification mod13 = (PeriAdvanceMonthGeneralActionModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_ExecuteGeneralActions(context, mod13);
				result = pData;
				break;
			}
			case ParallelModificationType.PeriAdvanceMonthExecuteFixedActions:
			{
				PeriAdvanceMonthFixedActionModification mod14 = (PeriAdvanceMonthFixedActionModification)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPeriAdvanceMonth_ExecuteFixedActions(context, mod14);
				result = pData;
				break;
			}
			case ParallelModificationType.PostAdvanceMonthPersonalNeedsUpdate:
			{
				Character character6 = (Character)this._objects[*(int*)pData];
				pData += 4;
				Character.ComplementPostAdvanceMonth_PersonalNeedsUpdate(context, character6);
				result = pData;
				break;
			}
			case ParallelModificationType.UpdateMapArea:
			{
				ParallelMapAreaModification mod15 = (ParallelMapAreaModification)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Map.ComplementUpdateMapArea(context, mod15);
				result = pData;
				break;
			}
			case ParallelModificationType.UpdateBrokenArea:
			{
				MapBlockData block = (MapBlockData)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Map.SetBlockData(context, block);
				result = pData;
				break;
			}
			case ParallelModificationType.UpdateBuilding:
			{
				ParallelBuildingModification modification = (ParallelBuildingModification)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Building.ComplementUpdateBuilding(context, modification);
				result = pData;
				break;
			}
			case ParallelModificationType.PreAdvanceMonthUpdateRandomEnemies:
			{
				PreAdvanceMonthRandomEnemiesModification mod16 = (PreAdvanceMonthRandomEnemiesModification)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Adventure.ComplementPreAdvanceMonth_UpdateRandomEnemies(context, mod16);
				result = pData;
				break;
			}
			case ParallelModificationType.PreAdvanceMonthUpdateNpcTaming:
			{
				PreAdvanceMonthNpcTamingModification mod17 = (PreAdvanceMonthNpcTamingModification)this._objects[*(int*)pData];
				pData += 4;
				DomainManager.Extra.ComplementPreAdvanceMonth_UpdateNpcTaming(context, mod17);
				result = pData;
				break;
			}
			default:
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(38, 1);
				defaultInterpolatedStringHandler.AppendLiteral("Unsupported ParallelModificationType: ");
				defaultInterpolatedStringHandler.AppendFormatted<ParallelModificationType>(type);
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			}
			return result;
		}

		// Token: 0x0400244A RID: 9290
		private const int DataPoolInitialCapacity = 1048576;

		// Token: 0x0400244B RID: 9291
		private const int ObjectsInitialCapacity = 16384;

		// Token: 0x0400244C RID: 9292
		private readonly RawDataPool _dataPool;

		// Token: 0x0400244D RID: 9293
		private readonly List<object> _objects;
	}
}
