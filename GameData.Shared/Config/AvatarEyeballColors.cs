using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AvatarEyeballColors : ConfigData<AvatarEyeballColorsItem, byte>
{
	public static AvatarEyeballColors Instance = new AvatarEyeballColors();

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
		_dataArray.Add(new AvatarEyeballColorsItem(0, "726666", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(1, "6f5050", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(2, "512020", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(3, "3d1212", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(4, "220b0b", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(5, "c66565", 1, 1, 1, 1, 0));
		_dataArray.Add(new AvatarEyeballColorsItem(6, "726966", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(7, "6f5950", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(8, "512e20", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(9, "3d1e12", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(10, "22110b", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(11, "c68165", 1, 1, 1, 1, 1));
		_dataArray.Add(new AvatarEyeballColorsItem(12, "726e66", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(13, "6f6450", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(14, "514020", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(15, "3d2e12", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(16, "221a0b", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(17, "c6a465", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarEyeballColorsItem(18, "6a7266", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(19, "5a6f50", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(20, "2f5120", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(21, "203d12", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(22, "12220b", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(23, "84c665", 1, 1, 1, 1, 3));
		_dataArray.Add(new AvatarEyeballColorsItem(24, "667072", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(25, "506a6f", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(26, "204851", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(27, "12363d", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(28, "0b1e22", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(29, "65b5c6", 1, 1, 1, 1, 4));
		_dataArray.Add(new AvatarEyeballColorsItem(30, "666872", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(31, "50576f", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(32, "202a51", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(33, "121b3d", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(34, "0b1022", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(35, "657ac6", 1, 1, 1, 1, 5));
		_dataArray.Add(new AvatarEyeballColorsItem(36, "716672", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(37, "6e506f", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(38, "4f2051", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(39, "3b123d", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(40, "220b22", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(41, "c165c6", 1, 1, 1, 1, 6));
		_dataArray.Add(new AvatarEyeballColorsItem(42, "676767", 1, 1, 1, 1, 7));
		_dataArray.Add(new AvatarEyeballColorsItem(43, "535353", 1, 1, 1, 1, 7));
		_dataArray.Add(new AvatarEyeballColorsItem(44, "2a2a2a", 1, 1, 1, 1, 8));
		_dataArray.Add(new AvatarEyeballColorsItem(45, "1c1c1c", 1, 1, 1, 1, 8));
		_dataArray.Add(new AvatarEyeballColorsItem(46, "101010", 1, 1, 1, 1, 9));
		_dataArray.Add(new AvatarEyeballColorsItem(47, "797979", 1, 1, 1, 1, 9));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AvatarEyeballColorsItem>(48);
		CreateItems0();
	}
}
