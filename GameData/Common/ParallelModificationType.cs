using System;

namespace GameData.Common
{
	// Token: 0x020008FC RID: 2300
	public enum ParallelModificationType : ushort
	{
		// Token: 0x0400244F RID: 9295
		[ParallelModification(4, "ComplementCreateIntelligentCharacter")]
		CreateIntelligentCharacter,
		// Token: 0x04002450 RID: 9296
		[ParallelModification(4, "ComplementCreateNewbornChildren")]
		CreateNewbornChildren,
		// Token: 0x04002451 RID: 9297
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementSetInitialCombatSkillBreakouts")]
		SetInitialCombatSkillBreakouts,
		// Token: 0x04002452 RID: 9298
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementSetInitialCombatSkillAttainmentPanels")]
		SetInitialCombatSkillAttainmentPanels,
		// Token: 0x04002453 RID: 9299
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementPracticeAndBreakoutCombatSkill")]
		PracticeAndBreakoutCombatSkills,
		// Token: 0x04002454 RID: 9300
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementActivateCombatSkillPages")]
		ActivateCombatSkillPages,
		// Token: 0x04002455 RID: 9301
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementUpdateBreakPlateBonuses")]
		UpdateBreakPlateBonuses,
		// Token: 0x04002456 RID: 9302
		[ParallelModification("GameData.Domains.Character.Ai.Equipping", "ComplementSelectEquipments")]
		SelectEquipments,
		// Token: 0x04002457 RID: 9303
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_MixedPoisonEffect")]
		PeriAdvanceMonthMixedPoisonEffect,
		// Token: 0x04002458 RID: 9304
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_UpdateStatus")]
		PeriAdvanceMonthUpdateStatus,
		// Token: 0x04002459 RID: 9305
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_SelfImprovement")]
		PeriAdvanceMonthSelfImprovement,
		// Token: 0x0400245A RID: 9306
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_SelfImprovement_LearnNewSkills")]
		PeriAdvanceMonthSelfImprovementLearnNewSkills,
		// Token: 0x0400245B RID: 9307
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_ActivePreparation_GetSupply")]
		PeriAdvanceMonthActivePreparationGetSupply,
		// Token: 0x0400245C RID: 9308
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_ActivePreparation")]
		PeriAdvanceMonthActivePreparation,
		// Token: 0x0400245D RID: 9309
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_PassivePreparation")]
		PeriAdvanceMonthPassivePreparation,
		// Token: 0x0400245E RID: 9310
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_RelationsUpdate")]
		PeriAdvanceMonthRelationsUpdate,
		// Token: 0x0400245F RID: 9311
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_PersonalNeedsProcessing")]
		PeriAdvanceMonthPersonalNeedsProcessing,
		// Token: 0x04002460 RID: 9312
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_ExecutePrioritizedAction")]
		PeriAdvanceMonthExecutePrioritizedAction,
		// Token: 0x04002461 RID: 9313
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_ExecuteGeneralActions")]
		PeriAdvanceMonthExecuteGeneralActions,
		// Token: 0x04002462 RID: 9314
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPeriAdvanceMonth_ExecuteFixedActions")]
		PeriAdvanceMonthExecuteFixedActions,
		// Token: 0x04002463 RID: 9315
		[ParallelModification("GameData.Domains.Character.Character", "ComplementPostAdvanceMonth_PersonalNeedsUpdate")]
		PostAdvanceMonthPersonalNeedsUpdate,
		// Token: 0x04002464 RID: 9316
		[ParallelModification(2, "ComplementUpdateMapArea")]
		UpdateMapArea,
		// Token: 0x04002465 RID: 9317
		[ParallelModification(2, "SetBlockData")]
		UpdateBrokenArea,
		// Token: 0x04002466 RID: 9318
		[ParallelModification(9, "ComplementUpdateBuilding")]
		UpdateBuilding,
		// Token: 0x04002467 RID: 9319
		[ParallelModification(10, "ComplementPreAdvanceMonth_UpdateRandomEnemies")]
		PreAdvanceMonthUpdateRandomEnemies,
		// Token: 0x04002468 RID: 9320
		[ParallelModification(19, "ComplementPreAdvanceMonth_UpdateNpcTaming")]
		PreAdvanceMonthUpdateNpcTaming
	}
}
