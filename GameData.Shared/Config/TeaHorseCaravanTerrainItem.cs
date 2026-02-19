using System;
using Config.Common;

namespace Config;

[Serializable]
public class TeaHorseCaravanTerrainItem : ConfigItem<TeaHorseCaravanTerrainItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly string Desc;

	public readonly sbyte Weighted;

	public TeaHorseCaravanTerrainItem(short templateId, int name, int desc, sbyte weighted)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("TeaHorseCaravanTerrain_language", name);
		Desc = LocalStringManager.GetConfig("TeaHorseCaravanTerrain_language", desc);
		Weighted = weighted;
	}

	public TeaHorseCaravanTerrainItem()
	{
		TemplateId = 0;
		Name = null;
		Desc = null;
		Weighted = -1;
	}

	public TeaHorseCaravanTerrainItem(short templateId, TeaHorseCaravanTerrainItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		Desc = other.Desc;
		Weighted = other.Weighted;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TeaHorseCaravanTerrainItem Duplicate(int templateId)
	{
		return new TeaHorseCaravanTerrainItem((short)templateId, this);
	}
}
