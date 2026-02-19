using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AiNode : ConfigData<AiNodeItem, int>
{
	public static AiNode Instance = new AiNode();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Type", "Name", "Desc", "IsAction" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new AiNodeItem(0, EAiNodeType.Linear, 0, 1, isAction: false));
		_dataArray.Add(new AiNodeItem(1, EAiNodeType.Branch, 2, 3, isAction: false));
		_dataArray.Add(new AiNodeItem(2, EAiNodeType.Action, 4, 5, isAction: true));
		_dataArray.Add(new AiNodeItem(3, EAiNodeType.Relay, 6, 7, isAction: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AiNodeItem>(4);
		CreateItems0();
	}
}
