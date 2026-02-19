using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TrickType : ConfigData<TrickTypeItem, sbyte>
{
	public static TrickType Instance = new TrickType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Icon", "FontColor", "BackIcon", "BigBackIcon", "AvoidBackIcon", "AvoidBigBackIcon" };

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
		_dataArray.Add(new TrickTypeItem(0, 0, "mousetip_shi_0", 0, new sbyte[1] { 40 }, new string[1] { "A_000_0" }, new string[1] { "Particle_A_000_0" }, new string[1] { "se_a_000_0" }, new sbyte[7] { 120, 120, 20, 40, 40, 40, 40 }, 12, "F3DB98", "combat_bottom_shi_0", "combat_bottom_shi_9", "combat_bottom_breakage_shi_0", "combat_bottom_breakage_shi_9", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst", "se_c_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(1, 1, "mousetip_shi_1", 2, new sbyte[1] { 40 }, new string[1] { "A_001_0" }, new string[1] { "Particle_A_001_0" }, new string[1] { "se_a_001_0" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 8, "F3DB98", "combat_bottom_shi_0", "combat_bottom_shi_9", "combat_bottom_breakage_shi_0", "combat_bottom_breakage_shi_9", new List<StringList>
		{
			new StringList("C_011_throat", "C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_throat", "Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_throat", "se_c_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(2, 2, "mousetip_shi_2", 1, new sbyte[1] { 40 }, new string[1] { "A_002_0" }, new string[1] { "Particle_A_002_0" }, new string[1] { "se_a_002_0" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 4, "F3DB98", "combat_bottom_shi_0", "combat_bottom_shi_9", "combat_bottom_breakage_shi_0", "combat_bottom_breakage_shi_9", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst", "se_c_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(3, 3, "mousetip_shi_3", 0, new sbyte[2] { 15, 25 }, new string[2] { "A_003_0", "A_014_0" }, new string[2] { "Particle_A_003_0", "Particle_A_014_0" }, new string[2] { "se_a_003_0", "se_a_003_0" }, new sbyte[7] { 120, 120, 20, 40, 40, 40, 40 }, 12, "C16927", "combat_bottom_shi_3", "combat_bottom_shi_12", "combat_bottom_breakage_shi_3", "combat_bottom_breakage_shi_12", new List<StringList>
		{
			new StringList("C_011_throat", "C_011_arm", "C_011_body", "C_011_head", "C_011_leg"),
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst", "C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_throat", "Particle_C_011_arm", "Particle_C_011_body", "Particle_C_011_head", "Particle_C_011_leg"),
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst", "Particle_C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_throat", "se_c_011_arm", "se_c_011_body", "se_c_011_head", "se_c_011_leg"),
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst", "se_c_011_head_burst", "se_c_011_leg_burst")
		}));
		_dataArray.Add(new TrickTypeItem(4, 4, "mousetip_shi_4", 2, new sbyte[2] { 20, 20 }, new string[2] { "A_004_0", "A_015_0" }, new string[2] { "Particle_A_004_0", "Particle_A_015_0" }, new string[2] { "se_a_004_0", "se_a_004_0" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 8, "C16927", "combat_bottom_shi_3", "combat_bottom_shi_12", "combat_bottom_breakage_shi_3", "combat_bottom_breakage_shi_12", new List<StringList>
		{
			new StringList("C_011_throat"),
			new StringList("C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_throat"),
			new StringList("Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_throat"),
			new StringList("se_c_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(5, 5, "mousetip_shi_5", 1, new sbyte[2] { 18, 20 }, new string[2] { "A_005_0", "A_016_0" }, new string[2] { "Particle_A_005_0", "Particle_A_016_0" }, new string[2] { "se_a_005_0", "se_a_005_0" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 4, "C16927", "combat_bottom_shi_3", "combat_bottom_shi_12", "combat_bottom_breakage_shi_3", "combat_bottom_breakage_shi_12", new List<StringList>
		{
			new StringList("C_011_throat", "C_011_arm", "C_011_body", "C_011_head"),
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst", "C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_throat", "Particle_C_011_arm", "Particle_C_011_body", "Particle_C_011_head"),
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst", "Particle_C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_throat", "se_c_011_arm", "se_c_011_body", "se_c_011_head"),
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst", "se_c_011_head_burst", "se_c_011_leg_burst")
		}));
		_dataArray.Add(new TrickTypeItem(6, 6, "mousetip_shi_6", 0, new sbyte[1] { 10 }, new string[1] { "A_006_0" }, new string[1] { "Particle_A_006_0" }, new string[1] { "se_a_006_0" }, new sbyte[7] { 120, 120, 20, 40, 40, 40, 40 }, 12, "73A9A1", "combat_bottom_shi_1", "combat_bottom_shi_10", "combat_bottom_breakage_shi_1", "combat_bottom_breakage_shi_10", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst")
		}));
		_dataArray.Add(new TrickTypeItem(7, 7, "mousetip_shi_7", 2, new sbyte[2] { 10, 10 }, new string[2] { "A_007_0", "A_007_0" }, new string[2] { "Particle_A_007_0", "Particle_A_007_0" }, new string[2] { "se_a_007_0", "se_a_007_0" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 8, "73A9A1", "combat_bottom_shi_1", "combat_bottom_shi_10", "combat_bottom_breakage_shi_1", "combat_bottom_breakage_shi_10", new List<StringList>
		{
			new StringList("C_011_throat", "C_011_head_burst"),
			new StringList("C_011_throat", "C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_throat", "Particle_C_011_head_burst"),
			new StringList("Particle_C_011_throat", "Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_throat", "se_c_011_head_burst"),
			new StringList("se_c_011_throat", "se_c_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(8, 8, "mousetip_shi_8", 1, new sbyte[2] { 10, 10 }, new string[2] { "A_008_0", "A_008_0" }, new string[2] { "Particle_A_008_0", "Particle_A_008_0" }, new string[2] { "se_a_008_0", "se_a_008_0" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 4, "73A9A1", "combat_bottom_shi_1", "combat_bottom_shi_10", "combat_bottom_breakage_shi_1", "combat_bottom_breakage_shi_10", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst"),
			new StringList("C_011_arm_burst", "C_011_body_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst"),
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst"),
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst")
		}));
		_dataArray.Add(new TrickTypeItem(9, 9, "mousetip_shi_9", 3, new sbyte[3] { 40, 40, 40 }, new string[3] { "A_009_0", "A_009_1", "A_017_0" }, new string[3] { "Particle_A_009_0", "Particle_A_009_1", "Particle_A_017_0" }, new string[3] { "se_a_009_0", "se_a_009_1", "se_a_017_0" }, new sbyte[7], 0, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_head_burst", "C_012"),
			new StringList("C_011_head_burst", "C_012"),
			new StringList("C_011_head_burst", "C_012")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_head_burst", ""),
			new StringList("Particle_C_011_head_burst", ""),
			new StringList("Particle_C_011_head_burst", "")
		}, new List<StringList>
		{
			new StringList("se_c_011_head_burst", ""),
			new StringList("se_c_011_head_burst", ""),
			new StringList("se_c_011_head_burst", "")
		}));
		_dataArray.Add(new TrickTypeItem(10, 10, "mousetip_shi_10", 2, new sbyte[1] { 30 }, new string[1] { "A_010_0" }, new string[1] { "Particle_A_010_0" }, new string[1] { "se_a_010_0" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 8, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_arm", "C_011_body", "C_011_head", "C_011_leg")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm", "Particle_C_011_body", "Particle_C_011_head", "Particle_C_011_leg")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm", "se_c_011_body", "se_c_011_head", "se_c_011_leg")
		}));
		_dataArray.Add(new TrickTypeItem(11, 11, "mousetip_shi_11", 3, new sbyte[1] { 40 }, new string[1] { "A_011_0" }, new string[1] { "Particle_A_011_0" }, new string[1] { "se_a_011_0" }, new sbyte[7], 0, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_head_burst", "C_012")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_head_burst", "")
		}, new List<StringList>
		{
			new StringList("se_c_011_head_burst", "")
		}));
		_dataArray.Add(new TrickTypeItem(12, 12, "mousetip_shi_12", 0, new sbyte[1] { 40 }, new string[1] { "A_012_0" }, new string[1] { "Particle_A_012_0" }, new string[1] { "se_a_012_0" }, new sbyte[7] { 120, 120, 20, 40, 40, 40, 40 }, 12, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst")
		}, new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst")
		}));
		_dataArray.Add(new TrickTypeItem(13, 13, "mousetip_shi_13", 1, new sbyte[2] { 20, 20 }, new string[2] { "A_013_1", "A_013_1" }, new string[2] { "Particle_A_013_1", "Particle_A_013_1" }, new string[2] { "se_a_013_1", "se_a_013_1" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 4, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011"),
			new StringList("C_011")
		}, new List<StringList>
		{
			new StringList("Particle_C_011"),
			new StringList("Particle_C_011")
		}, new List<StringList>
		{
			new StringList("se_c_011"),
			new StringList("se_c_011")
		}));
		_dataArray.Add(new TrickTypeItem(14, 14, "mousetip_shi_14", 2, new sbyte[2] { 20, 20 }, new string[2] { "A_013_0", "A_013_0" }, new string[2] { "Particle_A_013_0", "Particle_A_013_0" }, new string[2] { "se_a_013_0", "se_a_013_0" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 8, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_melt"),
			new StringList("C_011_melt")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_melt"),
			new StringList("Particle_C_011_melt")
		}, new List<StringList>
		{
			new StringList("se_c_011_melt"),
			new StringList("se_c_011_melt")
		}));
		_dataArray.Add(new TrickTypeItem(15, 15, "mousetip_shi_15", 1, new sbyte[1] { 20 }, new string[1] { "A_010_1" }, new string[1] { "Particle_A_010_1" }, new string[1] { "se_a_010_1" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 8, "B5DEDE", "combat_bottom_shi_6", "combat_bottom_shi_15", "combat_bottom_breakage_shi_6", "combat_bottom_breakage_shi_15", new List<StringList>
		{
			new StringList("C_011_arm_burst", "C_011_body_burst", "C_011_head_burst", "C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("Particle_C_011_arm_burst", "Particle_C_011_body_burst", "Particle_C_011_head_burst", "Particle_C_011_leg_burst")
		}, new List<StringList>
		{
			new StringList("se_c_011_arm_burst", "se_c_011_body_burst", "se_c_011_head_burst", "se_c_011_leg_burst")
		}));
		_dataArray.Add(new TrickTypeItem(16, 16, "mousetip_shi_16", 0, new sbyte[2], new string[5] { "A_001", "A_004", "A_001_a", "A_004_a", "A_004_b" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7] { 120, 120, 20, 40, 40, 40, 40 }, 12, "939393", "combat_bottom_shi_5", "combat_bottom_shi_14", "combat_bottom_breakage_shi_5", "combat_bottom_breakage_shi_14", new List<StringList>(), new List<StringList>(), new List<StringList>()));
		_dataArray.Add(new TrickTypeItem(17, 17, "mousetip_shi_17", 2, new sbyte[2], new string[1] { "A_002" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7] { 80, 80, 20, 60, 60, 60, 60 }, 8, "939393", "combat_bottom_shi_5", "combat_bottom_shi_14", "combat_bottom_breakage_shi_5", "combat_bottom_breakage_shi_14", new List<StringList>(), new List<StringList>(), new List<StringList>()));
		_dataArray.Add(new TrickTypeItem(18, 18, "mousetip_shi_18", 1, new sbyte[2], new string[1] { "A_003" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7] { 40, 40, 20, 80, 80, 80, 80 }, 4, "939393", "combat_bottom_shi_5", "combat_bottom_shi_14", "combat_bottom_breakage_shi_5", "combat_bottom_breakage_shi_14", new List<StringList>(), new List<StringList>(), new List<StringList>()));
		_dataArray.Add(new TrickTypeItem(19, 19, "mousetip_shi_19", -1, new sbyte[2], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7], 0, "F8E0CA", "combat_bottom_shi_4", "combat_bottom_shi_13", "combat_bottom_breakage_shi_4", "combat_bottom_breakage_shi_13", new List<StringList>(), new List<StringList>(), new List<StringList>()));
		_dataArray.Add(new TrickTypeItem(20, 20, "mousetip_shi_20", -1, new sbyte[2], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7], 0, "F8E0CA", "combat_bottom_shi_4", "combat_bottom_shi_13", "combat_bottom_breakage_shi_4", "combat_bottom_breakage_shi_13", new List<StringList>(), new List<StringList>(), new List<StringList>()));
		_dataArray.Add(new TrickTypeItem(21, 21, "mousetip_shi_21", 3, new sbyte[2], new string[1] { "" }, new string[1] { "" }, new string[1] { "" }, new sbyte[7] { 60, 60, 60, 60, 60, 60, 60 }, 0, "CCB8FF", "combat_bottom_shi_2", "combat_bottom_shi_11", "combat_bottom_breakage_shi_2", "combat_bottom_breakage_shi_11", new List<StringList>(), new List<StringList>(), new List<StringList>()));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<TrickTypeItem>(22);
		CreateItems0();
	}
}
