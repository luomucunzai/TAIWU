using System;
using Config.Common;

namespace Config;

[Serializable]
public class ChickenItem : ConfigItem<ChickenItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string Display;

	public readonly sbyte Grade;

	public readonly sbyte PersonalityType;

	public readonly int PersonalityValue;

	public readonly short FeatureId;

	public readonly short EventActorTemplateId;

	public readonly string EventDesc;

	public ChickenItem(short templateId, int name, int desc, string display, sbyte grade, sbyte personalityType, int personalityValue, short featureId, short eventActorTemplateId, int eventDesc)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("Chicken_language", name);
		Desc = LocalStringManager.GetConfig("Chicken_language", desc);
		Display = display;
		Grade = grade;
		PersonalityType = personalityType;
		PersonalityValue = personalityValue;
		FeatureId = featureId;
		EventActorTemplateId = eventActorTemplateId;
		EventDesc = LocalStringManager.GetConfig("Chicken_language", eventDesc);
	}

	public ChickenItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Display = null;
		Grade = 0;
		PersonalityType = 0;
		PersonalityValue = 0;
		FeatureId = 0;
		EventActorTemplateId = 0;
		EventDesc = null;
	}

	public ChickenItem(short templateId, ChickenItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Display = other.Display;
		Grade = other.Grade;
		PersonalityType = other.PersonalityType;
		PersonalityValue = other.PersonalityValue;
		FeatureId = other.FeatureId;
		EventActorTemplateId = other.EventActorTemplateId;
		EventDesc = other.EventDesc;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override ChickenItem Duplicate(int templateId)
	{
		return new ChickenItem((short)templateId, this);
	}
}
