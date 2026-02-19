using System;
using Config.Common;

namespace Config;

[Serializable]
public class InformationInfoItem : ConfigItem<InformationInfoItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly sbyte Grade;

	public readonly sbyte Oraganization;

	public readonly sbyte LifeSkillType;

	public readonly short WesternRegionId;

	public readonly EInformationInfoSwordInformationType SwordInformationType;

	public readonly sbyte SwordTombTemplateId;

	public readonly sbyte Profession;

	public readonly bool Consume;

	public readonly string Desc;

	public readonly string EffectiveAnswer;

	public readonly string NormalAnswer;

	public readonly string InvalidAnswer;

	public readonly string[] BehaviorTypePlaceHolders;

	public readonly string SwordTombPlaceHolder;

	public InformationInfoItem(short templateId, int name, sbyte grade, sbyte oraganization, sbyte lifeSkillType, short westernRegionId, EInformationInfoSwordInformationType swordInformationType, sbyte swordTombTemplateId, sbyte profession, bool consume, int desc, int effectiveAnswer, int normalAnswer, int invalidAnswer, int[] behaviorTypePlaceHolders, int swordTombPlaceHolder)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("InformationInfo_language", name);
		Grade = grade;
		Oraganization = oraganization;
		LifeSkillType = lifeSkillType;
		WesternRegionId = westernRegionId;
		SwordInformationType = swordInformationType;
		SwordTombTemplateId = swordTombTemplateId;
		Profession = profession;
		Consume = consume;
		Desc = LocalStringManager.GetConfig("InformationInfo_language", desc);
		EffectiveAnswer = LocalStringManager.GetConfig("InformationInfo_language", effectiveAnswer);
		NormalAnswer = LocalStringManager.GetConfig("InformationInfo_language", normalAnswer);
		InvalidAnswer = LocalStringManager.GetConfig("InformationInfo_language", invalidAnswer);
		BehaviorTypePlaceHolders = LocalStringManager.ConvertConfigList("InformationInfo_language", behaviorTypePlaceHolders);
		SwordTombPlaceHolder = LocalStringManager.GetConfig("InformationInfo_language", swordTombPlaceHolder);
	}

	public InformationInfoItem()
	{
		TemplateId = 0;
		Name = null;
		Grade = 0;
		Oraganization = 0;
		LifeSkillType = 0;
		WesternRegionId = 0;
		SwordInformationType = EInformationInfoSwordInformationType.Invalid;
		SwordTombTemplateId = 0;
		Profession = 0;
		Consume = true;
		Desc = null;
		EffectiveAnswer = null;
		NormalAnswer = null;
		InvalidAnswer = null;
		BehaviorTypePlaceHolders = null;
		SwordTombPlaceHolder = null;
	}

	public InformationInfoItem(short templateId, InformationInfoItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Grade = other.Grade;
		Oraganization = other.Oraganization;
		LifeSkillType = other.LifeSkillType;
		WesternRegionId = other.WesternRegionId;
		SwordInformationType = other.SwordInformationType;
		SwordTombTemplateId = other.SwordTombTemplateId;
		Profession = other.Profession;
		Consume = other.Consume;
		Desc = other.Desc;
		EffectiveAnswer = other.EffectiveAnswer;
		NormalAnswer = other.NormalAnswer;
		InvalidAnswer = other.InvalidAnswer;
		BehaviorTypePlaceHolders = other.BehaviorTypePlaceHolders;
		SwordTombPlaceHolder = other.SwordTombPlaceHolder;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override InformationInfoItem Duplicate(int templateId)
	{
		return new InformationInfoItem((short)templateId, this);
	}
}
