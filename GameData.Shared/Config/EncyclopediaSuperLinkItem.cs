using System;

namespace Config;

[Serializable]
public class EncyclopediaSuperLinkItem
{
	public readonly short TemplateId;

	public readonly string LinkId;

	public readonly string VolName;

	public readonly string PagePath;

	public readonly string TipType;

	public readonly string[] TipStringArgArray;

	public EncyclopediaSuperLinkItem(short arg0, string arg1, string arg2, string arg3, string arg4, string[] arg5)
	{
		TemplateId = arg0;
		LinkId = arg1;
		VolName = arg2;
		PagePath = arg3;
		TipType = arg4;
		TipStringArgArray = arg5;
	}

	public EncyclopediaSuperLinkItem()
	{
		TemplateId = 0;
		LinkId = null;
		VolName = null;
		PagePath = null;
		TipType = null;
		TipStringArgArray = null;
	}
}
