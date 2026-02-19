using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AvatarHead : ConfigData<AvatarHeadItem, byte>
{
	public static class DefKey
	{
		public const byte ThinBoy = 6;

		public const byte ThinGirl = 7;

		public const byte FatBoy = 8;

		public const byte FatGirl = 9;

		public const byte MaleThinSkeleton = 10;

		public const byte FemaleThinSkeleton = 11;

		public const byte MaleNormalSkeleton = 12;

		public const byte FemaleNormalSkeleton = 13;

		public const byte MaleStrongSkeleton = 14;

		public const byte FemaleStrongSkeleton = 15;
	}

	public static class DefValue
	{
		public static AvatarHeadItem ThinBoy => Instance[(byte)6];

		public static AvatarHeadItem ThinGirl => Instance[(byte)7];

		public static AvatarHeadItem FatBoy => Instance[(byte)8];

		public static AvatarHeadItem FatGirl => Instance[(byte)9];

		public static AvatarHeadItem MaleThinSkeleton => Instance[(byte)10];

		public static AvatarHeadItem FemaleThinSkeleton => Instance[(byte)11];

		public static AvatarHeadItem MaleNormalSkeleton => Instance[(byte)12];

		public static AvatarHeadItem FemaleNormalSkeleton => Instance[(byte)13];

		public static AvatarHeadItem MaleStrongSkeleton => Instance[(byte)14];

		public static AvatarHeadItem FemaleStrongSkeleton => Instance[(byte)15];
	}

	public static AvatarHead Instance = new AvatarHead();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "RelativeExtraPart", "TemplateId", "HeadId", "DisplayDesc", "NameOrPath" };

	internal override int ToInt(byte value)
	{
		return value;
	}

	internal override byte ToTemplateId(int value)
	{
		return (byte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new AvatarHeadItem(0, 1, 1, 0, "avatar_1_head", 25, 15, 70, 36, 53, 10, 22, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(1, 1, 2, 1, "avatar_2_head", 26, 12, 66, 33, 50, 11, 22, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(2, 1, 3, 2, "avatar_3_head", 26, 17, 72, 42, 59, 12, 27, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(3, 1, 4, 3, "avatar_4_head", 26, 16, 73, 38, 54, 10, 25, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(4, 1, 5, 4, "avatar_5_head", 29, 18, 74, 43, 61, 13, 30, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(5, 1, 6, 5, "avatar_6_head", 28, 15, 73, 41, 58, 13, 28, -1, canRandom: true));
		_dataArray.Add(new AvatarHeadItem(6, 1, 251, 6, "avatar_251_head", 14, 16, 74, 33, 48, 9, 20, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(7, 1, 252, 7, "avatar_252_head", 10, 14, 76, 32, 48, 9, 19, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(8, 1, 253, 8, "avatar_253_head", 18, 19, 85, 36, 54, 13, 25, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(9, 1, 254, 9, "avatar_254_head", 12, 17, 79, 32, 47, 9, 19, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(10, byte.MaxValue, 1, 10, "avatar_1_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(11, byte.MaxValue, 2, 11, "avatar_2_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(12, byte.MaxValue, 3, 12, "avatar_3_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(13, byte.MaxValue, 4, 13, "avatar_4_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(14, byte.MaxValue, 5, 14, "avatar_5_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
		_dataArray.Add(new AvatarHeadItem(15, byte.MaxValue, 6, 15, "avatar_6_head_2", 0, 0, 0, 0, 0, 0, 0, -1, canRandom: false));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AvatarHeadItem>(16);
		CreateItems0();
	}
}
