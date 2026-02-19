using System;
using Config.Common;

namespace Config;

[Serializable]
public class TeammateCommandItem : ConfigItem<TeammateCommandItem, sbyte>
{
	public readonly sbyte TemplateId;

	public readonly string Name;

	public readonly ETeammateCommandType Type;

	public readonly ETeammateCommandImplement Implement;

	public readonly ETeammateCommandOption Option;

	public readonly string Description;

	public readonly string BackCharEnterAni;

	public readonly string BackCharEnterSound;

	public readonly string BackCharPrepareAni;

	public readonly string BackCharPrepareSound;

	public readonly string BackCharExitAni;

	public readonly string BackCharParticle;

	public readonly string ForeCharAni1;

	public readonly string ForeCharAni2;

	public readonly string ForeCharAni3;

	public readonly bool ForeCharAniUseHit;

	public readonly short PrepareFrame;

	public readonly short AffectFrame;

	public readonly short CooldownFrame;

	public readonly int CdCount;

	public readonly bool IntoCombatField;

	public readonly bool DisableAi;

	public readonly bool NotCheckDoingOrReserve;

	public readonly short PosOffset;

	public readonly int IntArg;

	public readonly bool RequireTrick;

	public readonly bool RequireAttackSkill;

	public readonly bool RequireDefendSkill;

	public readonly int DefendSkillDurationPercent;

	public readonly sbyte MedalType;

	public readonly sbyte MedalCount;

	public readonly int UpgradeAuthorityCost;

	public readonly short[] FavorLimit;

	public readonly int[] AutoProgress;

	public readonly int AutoFrame;

	public readonly int AutoProb;

	public readonly string BubbleText;

	public readonly string[] EffectDisplayTextList;

	public readonly string[] EffectDisplayValueList;

	public readonly sbyte[] EffectDisplayPositiveList;

	public readonly sbyte EffectDisplayPreviewStyle;

	public TeammateCommandItem(sbyte templateId, int name, ETeammateCommandType type, ETeammateCommandImplement implement, ETeammateCommandOption option, int description, string backCharEnterAni, string backCharEnterSound, string backCharPrepareAni, string backCharPrepareSound, string backCharExitAni, string backCharParticle, string foreCharAni1, string foreCharAni2, string foreCharAni3, bool foreCharAniUseHit, short prepareFrame, short affectFrame, short cooldownFrame, int cdCount, bool intoCombatField, bool disableAi, bool notCheckDoingOrReserve, short posOffset, int intArg, bool requireTrick, bool requireAttackSkill, bool requireDefendSkill, int defendSkillDurationPercent, sbyte medalType, sbyte medalCount, int upgradeAuthorityCost, short[] favorLimit, int[] autoProgress, int autoFrame, int autoProb, int bubbleText, int[] effectDisplayTextList, string[] effectDisplayValueList, sbyte[] effectDisplayPositiveList, sbyte effectDisplayPreviewStyle)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("TeammateCommand_language", name);
		Type = type;
		Implement = implement;
		Option = option;
		Description = LocalStringManager.GetConfig("TeammateCommand_language", description);
		BackCharEnterAni = backCharEnterAni;
		BackCharEnterSound = backCharEnterSound;
		BackCharPrepareAni = backCharPrepareAni;
		BackCharPrepareSound = backCharPrepareSound;
		BackCharExitAni = backCharExitAni;
		BackCharParticle = backCharParticle;
		ForeCharAni1 = foreCharAni1;
		ForeCharAni2 = foreCharAni2;
		ForeCharAni3 = foreCharAni3;
		ForeCharAniUseHit = foreCharAniUseHit;
		PrepareFrame = prepareFrame;
		AffectFrame = affectFrame;
		CooldownFrame = cooldownFrame;
		CdCount = cdCount;
		IntoCombatField = intoCombatField;
		DisableAi = disableAi;
		NotCheckDoingOrReserve = notCheckDoingOrReserve;
		PosOffset = posOffset;
		IntArg = intArg;
		RequireTrick = requireTrick;
		RequireAttackSkill = requireAttackSkill;
		RequireDefendSkill = requireDefendSkill;
		DefendSkillDurationPercent = defendSkillDurationPercent;
		MedalType = medalType;
		MedalCount = medalCount;
		UpgradeAuthorityCost = upgradeAuthorityCost;
		FavorLimit = favorLimit;
		AutoProgress = autoProgress;
		AutoFrame = autoFrame;
		AutoProb = autoProb;
		BubbleText = LocalStringManager.GetConfig("TeammateCommand_language", bubbleText);
		EffectDisplayTextList = LocalStringManager.ConvertConfigList("TeammateCommand_language", effectDisplayTextList);
		EffectDisplayValueList = effectDisplayValueList;
		EffectDisplayPositiveList = effectDisplayPositiveList;
		EffectDisplayPreviewStyle = effectDisplayPreviewStyle;
	}

	public TeammateCommandItem()
	{
		TemplateId = 0;
		Name = null;
		Type = ETeammateCommandType.Normal;
		Implement = ETeammateCommandImplement.Invalid;
		Option = ETeammateCommandOption.Invalid;
		Description = null;
		BackCharEnterAni = null;
		BackCharEnterSound = null;
		BackCharPrepareAni = null;
		BackCharPrepareSound = null;
		BackCharExitAni = null;
		BackCharParticle = null;
		ForeCharAni1 = null;
		ForeCharAni2 = null;
		ForeCharAni3 = null;
		ForeCharAniUseHit = false;
		PrepareFrame = -1;
		AffectFrame = -1;
		CooldownFrame = 0;
		CdCount = -1;
		IntoCombatField = true;
		DisableAi = false;
		NotCheckDoingOrReserve = false;
		PosOffset = 0;
		IntArg = 0;
		RequireTrick = false;
		RequireAttackSkill = false;
		RequireDefendSkill = false;
		DefendSkillDurationPercent = 100;
		MedalType = -1;
		MedalCount = 0;
		UpgradeAuthorityCost = 0;
		FavorLimit = new short[2] { -30000, 30000 };
		AutoProgress = new int[0];
		AutoFrame = 0;
		AutoProb = 0;
		BubbleText = null;
		EffectDisplayTextList = LocalStringManager.ConvertConfigList("TeammateCommand_language", null);
		EffectDisplayValueList = new string[1] { "" };
		EffectDisplayPositiveList = new sbyte[0];
		EffectDisplayPreviewStyle = -1;
	}

	public TeammateCommandItem(sbyte templateId, TeammateCommandItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Type = other.Type;
		Implement = other.Implement;
		Option = other.Option;
		Description = other.Description;
		BackCharEnterAni = other.BackCharEnterAni;
		BackCharEnterSound = other.BackCharEnterSound;
		BackCharPrepareAni = other.BackCharPrepareAni;
		BackCharPrepareSound = other.BackCharPrepareSound;
		BackCharExitAni = other.BackCharExitAni;
		BackCharParticle = other.BackCharParticle;
		ForeCharAni1 = other.ForeCharAni1;
		ForeCharAni2 = other.ForeCharAni2;
		ForeCharAni3 = other.ForeCharAni3;
		ForeCharAniUseHit = other.ForeCharAniUseHit;
		PrepareFrame = other.PrepareFrame;
		AffectFrame = other.AffectFrame;
		CooldownFrame = other.CooldownFrame;
		CdCount = other.CdCount;
		IntoCombatField = other.IntoCombatField;
		DisableAi = other.DisableAi;
		NotCheckDoingOrReserve = other.NotCheckDoingOrReserve;
		PosOffset = other.PosOffset;
		IntArg = other.IntArg;
		RequireTrick = other.RequireTrick;
		RequireAttackSkill = other.RequireAttackSkill;
		RequireDefendSkill = other.RequireDefendSkill;
		DefendSkillDurationPercent = other.DefendSkillDurationPercent;
		MedalType = other.MedalType;
		MedalCount = other.MedalCount;
		UpgradeAuthorityCost = other.UpgradeAuthorityCost;
		FavorLimit = other.FavorLimit;
		AutoProgress = other.AutoProgress;
		AutoFrame = other.AutoFrame;
		AutoProb = other.AutoProb;
		BubbleText = other.BubbleText;
		EffectDisplayTextList = other.EffectDisplayTextList;
		EffectDisplayValueList = other.EffectDisplayValueList;
		EffectDisplayPositiveList = other.EffectDisplayPositiveList;
		EffectDisplayPreviewStyle = other.EffectDisplayPreviewStyle;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TeammateCommandItem Duplicate(int templateId)
	{
		return new TeammateCommandItem((sbyte)templateId, this);
	}
}
