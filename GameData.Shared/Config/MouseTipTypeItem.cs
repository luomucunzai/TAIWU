using System;

namespace Config;

[Serializable]
public class MouseTipTypeItem
{
	public readonly short TemplateId;

	public readonly short TipsType;

	public MouseTipTypeItem(short arg0, short arg1)
	{
		TemplateId = arg0;
		TipsType = arg1;
	}

	public MouseTipTypeItem()
	{
		TemplateId = 0;
		TipsType = 0;
	}
}
