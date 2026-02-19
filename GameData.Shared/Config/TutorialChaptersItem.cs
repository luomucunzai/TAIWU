using System;
using Config.Common;

namespace Config;

[Serializable]
public class TutorialChaptersItem : ConfigItem<TutorialChaptersItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly short MainCharacter;

	public readonly Tuple<string, short>[] FixedCharacters;

	public readonly string MapAreaPresetKey;

	public readonly byte[] StartBlockCoordinate;

	public readonly short StartingMonth;

	public readonly string Head;

	public readonly string Tail;

	public TutorialChaptersItem(short templateId, int name, int desc, short mainCharacter, Tuple<string, short>[] fixedCharacters, string mapAreaPresetKey, byte[] startBlockCoordinate, short startingMonth, int head, int tail)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("TutorialChapters_language", name);
		Desc = LocalStringManager.GetConfig("TutorialChapters_language", desc);
		MainCharacter = mainCharacter;
		FixedCharacters = fixedCharacters;
		MapAreaPresetKey = mapAreaPresetKey;
		StartBlockCoordinate = startBlockCoordinate;
		StartingMonth = startingMonth;
		Head = LocalStringManager.GetConfig("TutorialChapters_language", head);
		Tail = LocalStringManager.GetConfig("TutorialChapters_language", tail);
	}

	public TutorialChaptersItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		MainCharacter = 0;
		FixedCharacters = new Tuple<string, short>[0];
		MapAreaPresetKey = null;
		StartBlockCoordinate = null;
		StartingMonth = -1;
		Head = null;
		Tail = null;
	}

	public TutorialChaptersItem(short templateId, TutorialChaptersItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		MainCharacter = other.MainCharacter;
		FixedCharacters = other.FixedCharacters;
		MapAreaPresetKey = other.MapAreaPresetKey;
		StartBlockCoordinate = other.StartBlockCoordinate;
		StartingMonth = other.StartingMonth;
		Head = other.Head;
		Tail = other.Tail;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TutorialChaptersItem Duplicate(int templateId)
	{
		return new TutorialChaptersItem((short)templateId, this);
	}
}
