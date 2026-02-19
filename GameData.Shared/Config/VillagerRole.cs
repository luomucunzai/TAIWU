using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class VillagerRole : ConfigData<VillagerRoleItem, short>
{
	public static class DefKey
	{
		public const short Farmer = 0;

		public const short Craftsman = 1;

		public const short Doctor = 2;

		public const short Merchant = 3;

		public const short Literati = 4;

		public const short SwordTombKeeper = 5;

		public const short VillageHead = 6;
	}

	public static class DefValue
	{
		public static VillagerRoleItem Farmer => Instance[(short)0];

		public static VillagerRoleItem Craftsman => Instance[(short)1];

		public static VillagerRoleItem Doctor => Instance[(short)2];

		public static VillagerRoleItem Merchant => Instance[(short)3];

		public static VillagerRoleItem Literati => Instance[(short)4];

		public static VillagerRoleItem SwordTombKeeper => Instance[(short)5];

		public static VillagerRoleItem VillageHead => Instance[(short)6];
	}

	public static VillagerRole Instance = new VillagerRole();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "OrganizationMember", "PersonalityType", "NeedPersonalityList", "Clothing", "FeatureId", "LearnableLifeSkillTypes", "LearnableCombatSkillTypes", "AutoActions", "TemplateId", "IdleIcon" };

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
		_dataArray.Add(new VillagerRoleItem(0, 17, 6, new NeedPersonality[1]
		{
			new NeedPersonality(6, 3)
		}, new int[3] { 0, 1, 2 }, new int[3] { 3, 3, 4 }, new List<float[]>
		{
			new float[1] { 0.2f },
			new float[1] { 0.1f },
			new float[1] { 0.05f }
		}, new int[1] { 1 }, 85, "sp_icon_gongzuozhuangtai_1", 734, new sbyte[1] { 14 }, new sbyte[0], int.MaxValue, 100, new short[1]));
		_dataArray.Add(new VillagerRoleItem(1, 16, 4, new NeedPersonality[1]
		{
			new NeedPersonality(4, 3)
		}, new int[3] { 5, 6, 7 }, new int[3] { 4, 3, 3 }, new List<float[]>
		{
			new float[1] { 0.05f },
			new float[1] { 0.1f },
			new float[1] { 0.1f }
		}, new int[1] { 2 }, 86, "sp_icon_gongzuozhuangtai_3", 735, new sbyte[4] { 6, 7, 10, 11 }, new sbyte[0], int.MaxValue, 250, new short[3] { 1, 2, 3 }));
		_dataArray.Add(new VillagerRoleItem(2, 15, 0, new NeedPersonality[1]
		{
			new NeedPersonality(0, 3)
		}, new int[2] { 8, 9 }, new int[2] { 10, 11 }, new List<float[]>
		{
			new float[1] { 0.0677f },
			new float[1] { 1f }
		}, new int[1] { 1 }, 87, "sp_icon_gongzuozhuangtai_4", 736, new sbyte[2] { 8, 9 }, new sbyte[0], int.MaxValue, 500, new short[1] { 4 }));
		_dataArray.Add(new VillagerRoleItem(3, 14, 2, new NeedPersonality[1]
		{
			new NeedPersonality(2, 3)
		}, new int[2] { 12, 13 }, new int[2] { 10, 11 }, new List<float[]>
		{
			new float[1] { 0.0677f },
			new float[1] { 1f }
		}, new int[1] { 1 }, 88, "sp_icon_gongzuozhuangtai_6", 737, new sbyte[1] { 15 }, new sbyte[0], int.MaxValue, 750, new short[1] { 5 }));
		_dataArray.Add(new VillagerRoleItem(4, 13, 1, new NeedPersonality[1]
		{
			new NeedPersonality(1, 3)
		}, new int[3] { 14, 15, 16 }, new int[3] { 4, 17, 17 }, new List<float[]>
		{
			new float[1] { 0.05f },
			new float[1] { 0.05f },
			new float[1] { 0.1f }
		}, new int[1] { 1 }, 89, "sp_icon_gongzuozhuangtai_11", 738, new sbyte[5] { 0, 1, 2, 3, 5 }, new sbyte[0], int.MaxValue, 1000, new short[1] { 6 }));
		_dataArray.Add(new VillagerRoleItem(5, 12, 3, new NeedPersonality[1]
		{
			new NeedPersonality(3, 3)
		}, new int[5] { 18, 19, 20, 21, 22 }, new int[5] { 23, 23, 23, 11, 4 }, new List<float[]>
		{
			new float[1] { 1f },
			new float[1] { 1f },
			new float[1] { 1f },
			new float[1] { 1f },
			new float[1] { 0.05f }
		}, new int[1] { 3 }, 90, "sp_icon_gongzuozhuangtai_16", 739, new sbyte[2] { 13, 12 }, new sbyte[14]
		{
			0, 1, 2, 3, 4, 5, 6, 7, 8, 9,
			10, 11, 12, 13
		}, int.MaxValue, 1500, new short[1] { 7 }));
		_dataArray.Add(new VillagerRoleItem(6, 11, 5, new NeedPersonality[1]
		{
			new NeedPersonality(5, 3)
		}, new int[3] { 24, 25, 26 }, new int[3] { 27, 28, 3 }, new List<float[]>
		{
			new float[1] { 0.05f },
			new float[1] { 0.05f },
			new float[1] { 0.5f }
		}, new int[1] { 2 }, 91, "sp_icon_gongzuozhuangtai_21", 740, new sbyte[1] { 4 }, new sbyte[0], int.MaxValue, 2500, new short[1] { 8 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<VillagerRoleItem>(7);
		CreateItems0();
	}
}
