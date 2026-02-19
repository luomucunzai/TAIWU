using System;
using Config.Common;

namespace Config;

[Serializable]
public class SpecialEffectItem : ConfigItem<SpecialEffectItem, short>
{
	public readonly short TemplateId;

	public readonly sbyte EffectActiveType;

	public readonly short MinEffectCount;

	public readonly short MaxEffectCount;

	public readonly sbyte RequireAttackPower;

	public readonly sbyte AiCostNeiliAllocationChanceDelayFrame;

	public readonly ESpecialEffectAiCostNeiliAllocationType AiCostNeiliAllocationType;

	public readonly int TransferProportion;

	public readonly int[] AffectRequirePower;

	public readonly int AddUnlockValue;

	public readonly short AddUnlockValueItemSubType;

	public readonly short RawCreateEffect;

	public readonly short RawCreateTips;

	public readonly ESpecialEffectRawCreateType RawCreateType;

	public readonly int RawCreateRequireMaterialCount;

	public readonly bool ShowUsingItemButtonEffect;

	public readonly string Name;

	public readonly short SkillTemplateId;

	public readonly string[] ShortDesc;

	public readonly string[] Desc;

	public readonly string[] PlayerCastBossSkillDesc;

	public readonly string ClassName;

	public SpecialEffectItem(short templateId, sbyte effectActiveType, short minEffectCount, short maxEffectCount, sbyte requireAttackPower, sbyte aiCostNeiliAllocationChanceDelayFrame, ESpecialEffectAiCostNeiliAllocationType aiCostNeiliAllocationType, int transferProportion, int[] affectRequirePower, int addUnlockValue, short addUnlockValueItemSubType, short rawCreateEffect, short rawCreateTips, ESpecialEffectRawCreateType rawCreateType, int rawCreateRequireMaterialCount, bool showUsingItemButtonEffect, int name, short skillTemplateId, int[] shortDesc, int[] desc, int[] playerCastBossSkillDesc, string className)
	{
		TemplateId = templateId;
		EffectActiveType = effectActiveType;
		MinEffectCount = minEffectCount;
		MaxEffectCount = maxEffectCount;
		RequireAttackPower = requireAttackPower;
		AiCostNeiliAllocationChanceDelayFrame = aiCostNeiliAllocationChanceDelayFrame;
		AiCostNeiliAllocationType = aiCostNeiliAllocationType;
		TransferProportion = transferProportion;
		AffectRequirePower = affectRequirePower;
		AddUnlockValue = addUnlockValue;
		AddUnlockValueItemSubType = addUnlockValueItemSubType;
		RawCreateEffect = rawCreateEffect;
		RawCreateTips = rawCreateTips;
		RawCreateType = rawCreateType;
		RawCreateRequireMaterialCount = rawCreateRequireMaterialCount;
		ShowUsingItemButtonEffect = showUsingItemButtonEffect;
		Name = LocalStringManager.GetConfig("SpecialEffect_language", name);
		SkillTemplateId = skillTemplateId;
		ShortDesc = LocalStringManager.ConvertConfigList("SpecialEffect_language", shortDesc);
		Desc = LocalStringManager.ConvertConfigList("SpecialEffect_language", desc);
		PlayerCastBossSkillDesc = LocalStringManager.ConvertConfigList("SpecialEffect_language", playerCastBossSkillDesc);
		ClassName = className;
	}

	public SpecialEffectItem()
	{
		TemplateId = 0;
		EffectActiveType = -1;
		MinEffectCount = 1;
		MaxEffectCount = -1;
		RequireAttackPower = -1;
		AiCostNeiliAllocationChanceDelayFrame = -1;
		AiCostNeiliAllocationType = ESpecialEffectAiCostNeiliAllocationType.None;
		TransferProportion = 0;
		AffectRequirePower = new int[0];
		AddUnlockValue = 0;
		AddUnlockValueItemSubType = 0;
		RawCreateEffect = 0;
		RawCreateTips = 0;
		RawCreateType = ESpecialEffectRawCreateType.None;
		RawCreateRequireMaterialCount = 0;
		ShowUsingItemButtonEffect = false;
		Name = null;
		SkillTemplateId = 880;
		ShortDesc = LocalStringManager.ConvertConfigList("SpecialEffect_language", null);
		Desc = LocalStringManager.ConvertConfigList("SpecialEffect_language", null);
		PlayerCastBossSkillDesc = LocalStringManager.ConvertConfigList("SpecialEffect_language", null);
		ClassName = null;
	}

	public SpecialEffectItem(short templateId, SpecialEffectItem other)
	{
		TemplateId = templateId;
		EffectActiveType = other.EffectActiveType;
		MinEffectCount = other.MinEffectCount;
		MaxEffectCount = other.MaxEffectCount;
		RequireAttackPower = other.RequireAttackPower;
		AiCostNeiliAllocationChanceDelayFrame = other.AiCostNeiliAllocationChanceDelayFrame;
		AiCostNeiliAllocationType = other.AiCostNeiliAllocationType;
		TransferProportion = other.TransferProportion;
		AffectRequirePower = other.AffectRequirePower;
		AddUnlockValue = other.AddUnlockValue;
		AddUnlockValueItemSubType = other.AddUnlockValueItemSubType;
		RawCreateEffect = other.RawCreateEffect;
		RawCreateTips = other.RawCreateTips;
		RawCreateType = other.RawCreateType;
		RawCreateRequireMaterialCount = other.RawCreateRequireMaterialCount;
		ShowUsingItemButtonEffect = other.ShowUsingItemButtonEffect;
		Name = other.Name;
		SkillTemplateId = other.SkillTemplateId;
		ShortDesc = other.ShortDesc;
		Desc = other.Desc;
		PlayerCastBossSkillDesc = other.PlayerCastBossSkillDesc;
		ClassName = other.ClassName;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override SpecialEffectItem Duplicate(int templateId)
	{
		return new SpecialEffectItem((short)templateId, this);
	}
}
