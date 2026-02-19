using System;
using Config.Common;

namespace Config;

[Serializable]
public class AiParamItem : ConfigItem<AiParamItem, int>
{
	public readonly int TemplateId;

	public readonly EAiParamType Type;

	public readonly string Name;

	public readonly string Desc;

	public readonly string[] PrintingAliases;

	public readonly string[] AnalysisAliases;

	public AiParamItem(int templateId, EAiParamType type, int name, int desc, int[] printingAliases, string[] analysisAliases)
	{
		TemplateId = templateId;
		Type = type;
		Name = LocalStringManager.GetConfig("AiParam_language", name);
		Desc = LocalStringManager.GetConfig("AiParam_language", desc);
		PrintingAliases = LocalStringManager.ConvertConfigList("AiParam_language", printingAliases);
		AnalysisAliases = analysisAliases;
	}

	public AiParamItem()
	{
		TemplateId = 0;
		Type = EAiParamType.Int;
		Name = null;
		Desc = null;
		PrintingAliases = LocalStringManager.ConvertConfigList("AiParam_language", null);
		AnalysisAliases = new string[1] { "" };
	}

	public AiParamItem(int templateId, AiParamItem other)
	{
		TemplateId = templateId;
		Type = other.Type;
		Name = other.Name;
		Desc = other.Desc;
		PrintingAliases = other.PrintingAliases;
		AnalysisAliases = other.AnalysisAliases;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override AiParamItem Duplicate(int templateId)
	{
		return new AiParamItem(templateId, this);
	}
}
