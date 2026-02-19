using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class BodyPart : ConfigData<BodyPartItem, sbyte>
{
	public static BodyPart Instance = new BodyPart();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "AcupointDesc", "AcupointParam", "OuterInjuryIcon", "InnerInjuryIcon" };

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new BodyPartItem(0, 0, 1, new int[3] { 20, 40, 60 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_0", "sp_combat_icon_neishang_0"));
		_dataArray.Add(new BodyPartItem(1, 2, 3, new int[3] { 20, 40, 60 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_1", "sp_combat_icon_neishang_1"));
		_dataArray.Add(new BodyPartItem(2, 4, 5, new int[3] { 4, 6, 8 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_2", "sp_combat_icon_neishang_2"));
		_dataArray.Add(new BodyPartItem(3, 6, 7, new int[3] { 50, 100, 150 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_3", "sp_combat_icon_neishang_3"));
		_dataArray.Add(new BodyPartItem(4, 8, 9, new int[3] { 50, 100, 150 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_4", "sp_combat_icon_neishang_4"));
		_dataArray.Add(new BodyPartItem(5, 10, 11, new int[3] { 50, 100, 150 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_5", "sp_combat_icon_neishang_5"));
		_dataArray.Add(new BodyPartItem(6, 12, 13, new int[3] { 50, 100, 150 }, new int[3] { 0, 50, 75 }, "sp_combat_icon_waishang_6", "sp_combat_icon_neishang_6"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<BodyPartItem>(7);
		CreateItems0();
	}
}
