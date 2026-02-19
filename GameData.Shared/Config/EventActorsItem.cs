using System;
using Config.Common;

namespace Config;

[Serializable]
public class EventActorsItem : ConfigItem<EventActorsItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Texture;

	public readonly sbyte Gender;

	public readonly byte[] Age;

	public readonly short[] Attraction;

	public readonly short Clothing;

	public readonly bool IsMonk;

	public readonly sbyte PresetBodyType;

	public EventActorsItem(short templateId, int name, string texture, sbyte gender, byte[] age, short[] attraction, short clothing, bool isMonk, sbyte presetBodyType)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("EventActors_language", name);
		Texture = texture;
		Gender = gender;
		Age = age;
		Attraction = attraction;
		Clothing = clothing;
		IsMonk = isMonk;
		PresetBodyType = presetBodyType;
	}

	public EventActorsItem()
	{
		TemplateId = 0;
		Name = null;
		Texture = null;
		Gender = -1;
		Age = new byte[2] { 18, 60 };
		Attraction = new short[2] { 0, 900 };
		Clothing = 0;
		IsMonk = false;
		PresetBodyType = -1;
	}

	public EventActorsItem(short templateId, EventActorsItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Texture = other.Texture;
		Gender = other.Gender;
		Age = other.Age;
		Attraction = other.Attraction;
		Clothing = other.Clothing;
		IsMonk = other.IsMonk;
		PresetBodyType = other.PresetBodyType;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override EventActorsItem Duplicate(int templateId)
	{
		return new EventActorsItem((short)templateId, this);
	}
}
