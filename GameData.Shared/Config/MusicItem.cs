using System;
using Config.Common;

namespace Config;

[Serializable]
public class MusicItem : ConfigItem<MusicItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly short MapBlock;

	public readonly sbyte MapState;

	public readonly short HitRateMind;

	public readonly int AvoidRateMind;

	public readonly short TemporaryFeature;

	public readonly string Desc;

	public readonly string Evaluation;

	public MusicItem(short templateId, int name, short mapBlock, sbyte mapState, short hitRateMind, int avoidRateMind, short temporaryFeature, int desc, int evaluation)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("Music_language", name);
		MapBlock = mapBlock;
		MapState = mapState;
		HitRateMind = hitRateMind;
		AvoidRateMind = avoidRateMind;
		TemporaryFeature = temporaryFeature;
		Desc = LocalStringManager.GetConfig("Music_language", desc);
		Evaluation = LocalStringManager.GetConfig("Music_language", evaluation);
	}

	public MusicItem()
	{
		TemplateId = 0;
		Name = null;
		MapBlock = 0;
		MapState = 0;
		HitRateMind = 0;
		AvoidRateMind = 0;
		TemporaryFeature = 0;
		Desc = null;
		Evaluation = null;
	}

	public MusicItem(short templateId, MusicItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		MapBlock = other.MapBlock;
		MapState = other.MapState;
		HitRateMind = other.HitRateMind;
		AvoidRateMind = other.AvoidRateMind;
		TemporaryFeature = other.TemporaryFeature;
		Desc = other.Desc;
		Evaluation = other.Evaluation;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override MusicItem Duplicate(int templateId)
	{
		return new MusicItem((short)templateId, this);
	}

	public int GetCharacterPropertyBonusInt(ECharacterPropertyReferencedType key)
	{
		return key switch
		{
			ECharacterPropertyReferencedType.HitRateMind => HitRateMind, 
			ECharacterPropertyReferencedType.AvoidRateMind => AvoidRateMind, 
			_ => 0, 
		};
	}
}
