using System;
using Config.Common;

namespace Config;

[Serializable]
public class WorldCreationItem : ConfigItem<WorldCreationItem, byte>
{
	public readonly byte TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly string[] Icons;

	public readonly string[] Options;

	public readonly short[] InfluenceFactors;

	public readonly short[] LegacyPointBonus;

	public readonly short[] SecondaryInfluenceFactors;

	public readonly bool ShowInLegacy;

	public readonly sbyte[] DifficultyPreset;

	public readonly string SaveFileKey;

	public WorldCreationItem(byte templateId, int name, int desc, string[] icons, int[] options, short[] influenceFactors, short[] legacyPointBonus, short[] secondaryInfluenceFactors, bool showInLegacy, sbyte[] difficultyPreset, string saveFileKey)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("WorldCreation_language", name);
		Desc = LocalStringManager.GetConfig("WorldCreation_language", desc);
		Icons = icons;
		Options = LocalStringManager.ConvertConfigList("WorldCreation_language", options);
		InfluenceFactors = influenceFactors;
		LegacyPointBonus = legacyPointBonus;
		SecondaryInfluenceFactors = secondaryInfluenceFactors;
		ShowInLegacy = showInLegacy;
		DifficultyPreset = difficultyPreset;
		SaveFileKey = saveFileKey;
	}

	public WorldCreationItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Icons = null;
		Options = null;
		InfluenceFactors = new short[0];
		LegacyPointBonus = new short[0];
		SecondaryInfluenceFactors = new short[0];
		ShowInLegacy = false;
		DifficultyPreset = null;
		SaveFileKey = null;
	}

	public WorldCreationItem(byte templateId, WorldCreationItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Icons = other.Icons;
		Options = other.Options;
		InfluenceFactors = other.InfluenceFactors;
		LegacyPointBonus = other.LegacyPointBonus;
		SecondaryInfluenceFactors = other.SecondaryInfluenceFactors;
		ShowInLegacy = other.ShowInLegacy;
		DifficultyPreset = other.DifficultyPreset;
		SaveFileKey = other.SaveFileKey;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override WorldCreationItem Duplicate(int templateId)
	{
		return new WorldCreationItem((byte)templateId, this);
	}
}
