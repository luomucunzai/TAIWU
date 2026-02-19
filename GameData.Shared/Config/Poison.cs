using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Poison : ConfigData<PoisonItem, sbyte>
{
	public static Poison Instance = new Poison();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ProduceType", "TemplateId", "Name", "ShortName", "Desc", "FontColor", "Icon", "TipsIcon" };

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
		_dataArray.Add(new PoisonItem(0, 0, 1, 2, 3, 5, new byte[3] { 30, 15, 0 }, "hotpoison", "sp_icon_poison_0", "mousetip_duxing_0", 4, -1, -1));
		_dataArray.Add(new PoisonItem(1, 3, 4, 5, 2, 5, new byte[3] { 30, 15, 0 }, "gloomypoison", "sp_icon_poison_1", "mousetip_duxing_1", 40, -1, -1));
		_dataArray.Add(new PoisonItem(2, 6, 7, 8, 5, 10, new byte[3] { 20, 10, 0 }, "coldpoison", "sp_icon_poison_2", "mousetip_duxing_2", 75, -1, -1));
		_dataArray.Add(new PoisonItem(3, 9, 10, 11, 4, 10, new byte[3] { 20, 10, 0 }, "redpoison", "sp_icon_poison_3", "mousetip_duxing_3", 75, -1, -1));
		_dataArray.Add(new PoisonItem(4, 12, 13, 14, 0, 5, new byte[3] { 10, 5, 0 }, "rottenpoison", "sp_icon_poison_4", "mousetip_duxing_4", 600, 15, -1));
		_dataArray.Add(new PoisonItem(5, 15, 16, 17, 1, 5, new byte[3] { 10, 5, 0 }, "illusorypoison", "sp_icon_poison_5", "mousetip_duxing_5", 600, -1, 15));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<PoisonItem>(6);
		CreateItems0();
	}
}
