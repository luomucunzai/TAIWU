using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Loong : ConfigData<LoongItem, short>
{
	public static class DefKey
	{
		public const short While = 0;

		public const short Black = 1;

		public const short Green = 2;

		public const short Red = 3;

		public const short Yellow = 4;
	}

	public static class DefValue
	{
		public static LoongItem While => Instance[(short)0];

		public static LoongItem Black => Instance[(short)1];

		public static LoongItem Green => Instance[(short)2];

		public static LoongItem Red => Instance[(short)3];

		public static LoongItem Yellow => Instance[(short)4];
	}

	public static Loong Instance = new Loong();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"CharTemplateId", "MinionCharTemplateId", "MapBlock", "ClothingTemplateId", "WorldState", "DebuffCountIncNotification", "DebuffCountDecNotification", "Jiao", "Task", "TemplateId",
		"PersonalityType", "PersonalityRequirement", "BlockEffectTip", "EnterCombatEffect", "EnterCombatSound", "DebuffMarkOnChar"
	};

	internal override int ToInt(short value)
	{
		return value;
	}

	internal override short ToTemplateId(int value)
	{
		return (short)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new LoongItem(0, 686, 691, 137, 0, 50, 75, 39, 0, 120, 125, "debuff_leisj", "Ambience_map_dragon_jin_appear", 0, 268, "fiveloong_mark_0"));
		_dataArray.Add(new LoongItem(1, 687, 692, 138, 1, 50, 76, 40, 1, 121, 126, "debuff_shuisj", "Ambience_map_dragon_shui_appear", 1, 269, "fiveloong_mark_2"));
		_dataArray.Add(new LoongItem(2, 688, 693, 139, 2, 50, 77, 41, 2, 123, 128, "debuff_fengsj", "Ambience_map_dragon_mu_appear", 2, 270, "fiveloong_mark_1"));
		_dataArray.Add(new LoongItem(3, 689, 694, 140, 3, 50, 78, 42, 3, 122, 127, "debuff_huosj", "Ambience_map_dragon_huo_appear", 3, 271, "fiveloong_mark_3"));
		_dataArray.Add(new LoongItem(4, 690, 695, 141, 4, 50, 79, 43, 4, 124, 129, "debuff_shasj", "Ambience_map_dragon_tu_appear", 4, 272, "fiveloong_mark_4"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LoongItem>(5);
		CreateItems0();
	}
}
