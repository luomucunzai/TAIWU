using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class CombatConfigItem : ConfigItem<CombatConfigItem, short>
{
	public readonly short TemplateId;

	public readonly sbyte CombatType;

	public readonly bool IsVicious;

	public readonly byte MinDistance;

	public readonly byte MaxDistance;

	public readonly sbyte InitDistance;

	public readonly byte FleeDistance;

	public readonly byte FleeInterruptDistance;

	public readonly bool HideDistance;

	public readonly bool EnemyAnonymous;

	public readonly bool IsOutBoss;

	public readonly int StayPercent;

	public readonly bool SelfCanFlee;

	public readonly bool EnemyCanFlee;

	public readonly bool EnemyOnlyFlee;

	public readonly bool IsGroupMemberLeave;

	public readonly bool AllowShowMercy;

	public readonly bool AllowGroupMember;

	public readonly bool AllowRandomFavorability;

	public readonly bool AllowPrepare;

	public readonly bool AllowVitalDemon;

	public readonly bool AllowVitalDemonBetray;

	public readonly bool AffectTemporaryCharacter;

	public readonly List<List<sbyte>> SpecialTeammateCommands;

	public readonly string[] SpecialTeammateCommandBubbleTexts;

	public readonly List<sbyte> FiveElementsOfSkill;

	public readonly List<sbyte> CombatSkillType;

	public readonly sbyte Sect;

	public readonly bool DropResource;

	public readonly bool AllowDropItem;

	public readonly short LootItemRate;

	public readonly bool LootAllInventory;

	public readonly short CaptureRate;

	public readonly short CaptureRequireRope;

	public readonly bool CaptureNoCarrier;

	public readonly bool EnemyHealDamage;

	public readonly bool SelfFatalDamageReduceHealth;

	public readonly bool EnemyFatalDamageReduceHealth;

	public readonly uint SelfForceDefeatFrame;

	public readonly uint EnemyForceDefeatFrame;

	public readonly string[] Bgm;

	public readonly short Scene;

	public readonly int EnemyAi;

	public readonly bool StartInSecondPhase;

	public CombatConfigItem(short templateId, sbyte combatType, bool isVicious, byte minDistance, byte maxDistance, sbyte initDistance, byte fleeDistance, byte fleeInterruptDistance, bool hideDistance, bool enemyAnonymous, bool isOutBoss, int stayPercent, bool selfCanFlee, bool enemyCanFlee, bool enemyOnlyFlee, bool isGroupMemberLeave, bool allowShowMercy, bool allowGroupMember, bool allowRandomFavorability, bool allowPrepare, bool allowVitalDemon, bool allowVitalDemonBetray, bool affectTemporaryCharacter, List<List<sbyte>> specialTeammateCommands, int[] specialTeammateCommandBubbleTexts, List<sbyte> fiveElementsOfSkill, List<sbyte> combatSkillType, sbyte sect, bool dropResource, bool allowDropItem, short lootItemRate, bool lootAllInventory, short captureRate, short captureRequireRope, bool captureNoCarrier, bool enemyHealDamage, bool selfFatalDamageReduceHealth, bool enemyFatalDamageReduceHealth, uint selfForceDefeatFrame, uint enemyForceDefeatFrame, string[] bgm, short scene, int enemyAi, bool startInSecondPhase)
	{
		TemplateId = templateId;
		CombatType = combatType;
		IsVicious = isVicious;
		MinDistance = minDistance;
		MaxDistance = maxDistance;
		InitDistance = initDistance;
		FleeDistance = fleeDistance;
		FleeInterruptDistance = fleeInterruptDistance;
		HideDistance = hideDistance;
		EnemyAnonymous = enemyAnonymous;
		IsOutBoss = isOutBoss;
		StayPercent = stayPercent;
		SelfCanFlee = selfCanFlee;
		EnemyCanFlee = enemyCanFlee;
		EnemyOnlyFlee = enemyOnlyFlee;
		IsGroupMemberLeave = isGroupMemberLeave;
		AllowShowMercy = allowShowMercy;
		AllowGroupMember = allowGroupMember;
		AllowRandomFavorability = allowRandomFavorability;
		AllowPrepare = allowPrepare;
		AllowVitalDemon = allowVitalDemon;
		AllowVitalDemonBetray = allowVitalDemonBetray;
		AffectTemporaryCharacter = affectTemporaryCharacter;
		SpecialTeammateCommands = specialTeammateCommands;
		SpecialTeammateCommandBubbleTexts = LocalStringManager.ConvertConfigList("CombatConfig_language", specialTeammateCommandBubbleTexts);
		FiveElementsOfSkill = fiveElementsOfSkill;
		CombatSkillType = combatSkillType;
		Sect = sect;
		DropResource = dropResource;
		AllowDropItem = allowDropItem;
		LootItemRate = lootItemRate;
		LootAllInventory = lootAllInventory;
		CaptureRate = captureRate;
		CaptureRequireRope = captureRequireRope;
		CaptureNoCarrier = captureNoCarrier;
		EnemyHealDamage = enemyHealDamage;
		SelfFatalDamageReduceHealth = selfFatalDamageReduceHealth;
		EnemyFatalDamageReduceHealth = enemyFatalDamageReduceHealth;
		SelfForceDefeatFrame = selfForceDefeatFrame;
		EnemyForceDefeatFrame = enemyForceDefeatFrame;
		Bgm = bgm;
		Scene = scene;
		EnemyAi = enemyAi;
		StartInSecondPhase = startInSecondPhase;
	}

	public CombatConfigItem()
	{
		TemplateId = 0;
		CombatType = -1;
		IsVicious = true;
		MinDistance = 20;
		MaxDistance = 120;
		InitDistance = -1;
		FleeDistance = 100;
		FleeInterruptDistance = 40;
		HideDistance = false;
		EnemyAnonymous = false;
		IsOutBoss = false;
		StayPercent = 50;
		SelfCanFlee = true;
		EnemyCanFlee = true;
		EnemyOnlyFlee = false;
		IsGroupMemberLeave = true;
		AllowShowMercy = true;
		AllowGroupMember = true;
		AllowRandomFavorability = true;
		AllowPrepare = true;
		AllowVitalDemon = true;
		AllowVitalDemonBetray = true;
		AffectTemporaryCharacter = false;
		SpecialTeammateCommands = new List<List<sbyte>>();
		SpecialTeammateCommandBubbleTexts = LocalStringManager.ConvertConfigList("CombatConfig_language", null);
		FiveElementsOfSkill = new List<sbyte>();
		CombatSkillType = new List<sbyte>();
		Sect = 0;
		DropResource = true;
		AllowDropItem = true;
		LootItemRate = 100;
		LootAllInventory = false;
		CaptureRate = 100;
		CaptureRequireRope = 0;
		CaptureNoCarrier = false;
		EnemyHealDamage = false;
		SelfFatalDamageReduceHealth = true;
		EnemyFatalDamageReduceHealth = true;
		SelfForceDefeatFrame = 0u;
		EnemyForceDefeatFrame = 0u;
		Bgm = null;
		Scene = 0;
		EnemyAi = 0;
		StartInSecondPhase = false;
	}

	public CombatConfigItem(short templateId, CombatConfigItem other)
	{
		TemplateId = templateId;
		CombatType = other.CombatType;
		IsVicious = other.IsVicious;
		MinDistance = other.MinDistance;
		MaxDistance = other.MaxDistance;
		InitDistance = other.InitDistance;
		FleeDistance = other.FleeDistance;
		FleeInterruptDistance = other.FleeInterruptDistance;
		HideDistance = other.HideDistance;
		EnemyAnonymous = other.EnemyAnonymous;
		IsOutBoss = other.IsOutBoss;
		StayPercent = other.StayPercent;
		SelfCanFlee = other.SelfCanFlee;
		EnemyCanFlee = other.EnemyCanFlee;
		EnemyOnlyFlee = other.EnemyOnlyFlee;
		IsGroupMemberLeave = other.IsGroupMemberLeave;
		AllowShowMercy = other.AllowShowMercy;
		AllowGroupMember = other.AllowGroupMember;
		AllowRandomFavorability = other.AllowRandomFavorability;
		AllowPrepare = other.AllowPrepare;
		AllowVitalDemon = other.AllowVitalDemon;
		AllowVitalDemonBetray = other.AllowVitalDemonBetray;
		AffectTemporaryCharacter = other.AffectTemporaryCharacter;
		SpecialTeammateCommands = other.SpecialTeammateCommands;
		SpecialTeammateCommandBubbleTexts = other.SpecialTeammateCommandBubbleTexts;
		FiveElementsOfSkill = other.FiveElementsOfSkill;
		CombatSkillType = other.CombatSkillType;
		Sect = other.Sect;
		DropResource = other.DropResource;
		AllowDropItem = other.AllowDropItem;
		LootItemRate = other.LootItemRate;
		LootAllInventory = other.LootAllInventory;
		CaptureRate = other.CaptureRate;
		CaptureRequireRope = other.CaptureRequireRope;
		CaptureNoCarrier = other.CaptureNoCarrier;
		EnemyHealDamage = other.EnemyHealDamage;
		SelfFatalDamageReduceHealth = other.SelfFatalDamageReduceHealth;
		EnemyFatalDamageReduceHealth = other.EnemyFatalDamageReduceHealth;
		SelfForceDefeatFrame = other.SelfForceDefeatFrame;
		EnemyForceDefeatFrame = other.EnemyForceDefeatFrame;
		Bgm = other.Bgm;
		Scene = other.Scene;
		EnemyAi = other.EnemyAi;
		StartInSecondPhase = other.StartInSecondPhase;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override CombatConfigItem Duplicate(int templateId)
	{
		return new CombatConfigItem((short)templateId, this);
	}
}
