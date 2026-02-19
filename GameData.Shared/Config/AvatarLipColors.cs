using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AvatarLipColors : ConfigData<AvatarLipColorsItem, byte>
{
	public static AvatarLipColors Instance = new AvatarLipColors();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "ColorHex", "DisplayDesc" };

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
		_dataArray.Add(new AvatarLipColorsItem(0, "fcc0bf", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarLipColorsItem(1, "fc8d94", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarLipColorsItem(2, "fd5f6a", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarLipColorsItem(3, "c5404c", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarLipColorsItem(4, "9b1212", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarLipColorsItem(5, "7d2023", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarLipColorsItem(6, "ebbbcb", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarLipColorsItem(7, "e491a5", 1, 1, 1, 1, 7));
		_dataArray.Add(new AvatarLipColorsItem(8, "d8577d", 1, 1, 1, 1, 8));
		_dataArray.Add(new AvatarLipColorsItem(9, "be0139", 1, 1, 1, 1, 9));
		_dataArray.Add(new AvatarLipColorsItem(10, "a01d2b", 1, 1, 1, 1, 10));
		_dataArray.Add(new AvatarLipColorsItem(11, "640f19", 1, 1, 1, 1, 11));
		_dataArray.Add(new AvatarLipColorsItem(12, "e2aa9f", 1, 1, 1, 1, 12));
		_dataArray.Add(new AvatarLipColorsItem(13, "e07e71", 1, 1, 1, 1, 13));
		_dataArray.Add(new AvatarLipColorsItem(14, "de5657", 1, 1, 1, 1, 14));
		_dataArray.Add(new AvatarLipColorsItem(15, "c8382f", 1, 1, 1, 1, 15));
		_dataArray.Add(new AvatarLipColorsItem(16, "ac332d", 1, 1, 1, 1, 16));
		_dataArray.Add(new AvatarLipColorsItem(17, "6c2825", 1, 1, 1, 1, 17));
		_dataArray.Add(new AvatarLipColorsItem(18, "fdc3a6", 1, 1, 1, 1, 18));
		_dataArray.Add(new AvatarLipColorsItem(19, "ff9a77", 1, 1, 1, 1, 19));
		_dataArray.Add(new AvatarLipColorsItem(20, "eb7551", 1, 1, 1, 1, 20));
		_dataArray.Add(new AvatarLipColorsItem(21, "ba5b1d", 1, 1, 1, 1, 21));
		_dataArray.Add(new AvatarLipColorsItem(22, "bf6421", 1, 1, 1, 1, 22));
		_dataArray.Add(new AvatarLipColorsItem(23, "81481e", 1, 1, 1, 1, 23));
		_dataArray.Add(new AvatarLipColorsItem(24, "d7a2ba", 1, 1, 1, 1, 24));
		_dataArray.Add(new AvatarLipColorsItem(25, "cb88a6", 1, 1, 1, 1, 25));
		_dataArray.Add(new AvatarLipColorsItem(26, "bc618a", 1, 1, 1, 1, 26));
		_dataArray.Add(new AvatarLipColorsItem(27, "9f4172", 1, 1, 1, 1, 27));
		_dataArray.Add(new AvatarLipColorsItem(28, "bc2372", 1, 1, 1, 1, 28));
		_dataArray.Add(new AvatarLipColorsItem(29, "7a1749", 1, 1, 1, 1, 29));
		_dataArray.Add(new AvatarLipColorsItem(30, "d9b3ad", 1, 1, 1, 1, 30));
		_dataArray.Add(new AvatarLipColorsItem(31, "c9a193", 1, 1, 1, 1, 31));
		_dataArray.Add(new AvatarLipColorsItem(32, "a87964", 1, 1, 1, 1, 32));
		_dataArray.Add(new AvatarLipColorsItem(33, "906852", 1, 1, 1, 1, 33));
		_dataArray.Add(new AvatarLipColorsItem(34, "7b654c", 1, 1, 1, 1, 34));
		_dataArray.Add(new AvatarLipColorsItem(35, "714e49", 1, 1, 1, 1, 35));
		_dataArray.Add(new AvatarLipColorsItem(36, "527243", 1, 1, 1, 1, 36));
		_dataArray.Add(new AvatarLipColorsItem(37, "4a8073", 1, 1, 1, 1, 37));
		_dataArray.Add(new AvatarLipColorsItem(38, "586d7c", 1, 1, 1, 1, 38));
		_dataArray.Add(new AvatarLipColorsItem(39, "405373", 1, 1, 1, 1, 39));
		_dataArray.Add(new AvatarLipColorsItem(40, "4b395a", 1, 1, 1, 1, 40));
		_dataArray.Add(new AvatarLipColorsItem(41, "5c3360", 1, 1, 1, 1, 41));
		_dataArray.Add(new AvatarLipColorsItem(42, "ffffff", 1, 1, 1, 1, 42));
		_dataArray.Add(new AvatarLipColorsItem(43, "f2e9e0", 1, 1, 1, 1, 43));
		_dataArray.Add(new AvatarLipColorsItem(44, "c3bdb7", 1, 1, 1, 1, 44));
		_dataArray.Add(new AvatarLipColorsItem(45, "696663", 1, 1, 1, 1, 45));
		_dataArray.Add(new AvatarLipColorsItem(46, "343434", 1, 1, 1, 1, 46));
		_dataArray.Add(new AvatarLipColorsItem(47, "000000", 1, 1, 1, 1, 47));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AvatarLipColorsItem>(48);
		CreateItems0();
	}
}
