using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AvatarSkinColors : ConfigData<AvatarSkinColorsItem, byte>
{
	public static class DefKey
	{
		public const byte MostWhite = 0;
	}

	public static class DefValue
	{
		public static AvatarSkinColorsItem MostWhite => Instance[(byte)0];
	}

	public static AvatarSkinColors Instance = new AvatarSkinColors();

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
		_dataArray.Add(new AvatarSkinColorsItem(0, "ecd9d9", 8, 8, 8, 8, 0));
		_dataArray.Add(new AvatarSkinColorsItem(1, "e7caca", 16, 16, 16, 16, 0));
		_dataArray.Add(new AvatarSkinColorsItem(2, "d4b9b9", 8, 8, 8, 8, 1));
		_dataArray.Add(new AvatarSkinColorsItem(3, "ccaeae", 4, 4, 4, 4, 1));
		_dataArray.Add(new AvatarSkinColorsItem(4, "bc9e9e", 2, 2, 2, 2, 2));
		_dataArray.Add(new AvatarSkinColorsItem(5, "ab8f8c", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarSkinColorsItem(6, "f7d8d2", 16, 16, 16, 16, 0));
		_dataArray.Add(new AvatarSkinColorsItem(7, "f0c8c0", 32, 32, 32, 32, 0));
		_dataArray.Add(new AvatarSkinColorsItem(8, "deb3ab", 16, 16, 16, 16, 1));
		_dataArray.Add(new AvatarSkinColorsItem(9, "cfa69e", 8, 8, 8, 8, 1));
		_dataArray.Add(new AvatarSkinColorsItem(10, "bf9992", 4, 4, 4, 4, 2));
		_dataArray.Add(new AvatarSkinColorsItem(11, "b38d86", 2, 2, 2, 2, 2));
		_dataArray.Add(new AvatarSkinColorsItem(12, "f5ded4", 32, 32, 32, 32, 0));
		_dataArray.Add(new AvatarSkinColorsItem(13, "eacfc3", 64, 64, 64, 64, 0));
		_dataArray.Add(new AvatarSkinColorsItem(14, "e6c3b3", 32, 32, 32, 32, 1));
		_dataArray.Add(new AvatarSkinColorsItem(15, "dab4a3", 16, 16, 16, 16, 1));
		_dataArray.Add(new AvatarSkinColorsItem(16, "c39c8b", 8, 8, 8, 8, 2));
		_dataArray.Add(new AvatarSkinColorsItem(17, "aa8879", 4, 4, 4, 4, 2));
		_dataArray.Add(new AvatarSkinColorsItem(18, "f7d5c6", 64, 64, 64, 64, 0));
		_dataArray.Add(new AvatarSkinColorsItem(19, "f0ccbc", 128, 128, 128, 128, 0));
		_dataArray.Add(new AvatarSkinColorsItem(20, "ebc0ad", 64, 64, 64, 64, 1));
		_dataArray.Add(new AvatarSkinColorsItem(21, "d9b09e", 32, 32, 32, 32, 1));
		_dataArray.Add(new AvatarSkinColorsItem(22, "c49e8a", 16, 16, 16, 16, 2));
		_dataArray.Add(new AvatarSkinColorsItem(23, "b88f7b", 8, 8, 8, 8, 2));
		_dataArray.Add(new AvatarSkinColorsItem(24, "f0d9c6", 32, 32, 32, 32, 0));
		_dataArray.Add(new AvatarSkinColorsItem(25, "f0d4bb", 64, 64, 64, 64, 0));
		_dataArray.Add(new AvatarSkinColorsItem(26, "e9c8ab", 32, 32, 32, 32, 1));
		_dataArray.Add(new AvatarSkinColorsItem(27, "d9b697", 16, 16, 16, 16, 1));
		_dataArray.Add(new AvatarSkinColorsItem(28, "cca888", 8, 8, 8, 8, 2));
		_dataArray.Add(new AvatarSkinColorsItem(29, "bc9572", 4, 4, 4, 4, 2));
		_dataArray.Add(new AvatarSkinColorsItem(30, "e8d0b4", 16, 16, 16, 16, 0));
		_dataArray.Add(new AvatarSkinColorsItem(31, "e2c6a3", 32, 32, 32, 32, 0));
		_dataArray.Add(new AvatarSkinColorsItem(32, "d3b796", 16, 16, 16, 16, 1));
		_dataArray.Add(new AvatarSkinColorsItem(33, "c0a27c", 8, 8, 8, 8, 1));
		_dataArray.Add(new AvatarSkinColorsItem(34, "ae916e", 4, 4, 4, 4, 2));
		_dataArray.Add(new AvatarSkinColorsItem(35, "a28664", 2, 2, 2, 2, 2));
		_dataArray.Add(new AvatarSkinColorsItem(36, "f0e1d1", 8, 8, 8, 8, 0));
		_dataArray.Add(new AvatarSkinColorsItem(37, "e0d0b8", 16, 16, 16, 16, 0));
		_dataArray.Add(new AvatarSkinColorsItem(38, "cfbda4", 8, 8, 8, 8, 1));
		_dataArray.Add(new AvatarSkinColorsItem(39, "c4b095", 4, 4, 4, 4, 1));
		_dataArray.Add(new AvatarSkinColorsItem(40, "b09b7c", 2, 2, 2, 2, 2));
		_dataArray.Add(new AvatarSkinColorsItem(41, "89745d", 1, 1, 1, 1, 2));
		_dataArray.Add(new AvatarSkinColorsItem(42, "d7c1c1", 8, 8, 8, 8, 0));
		_dataArray.Add(new AvatarSkinColorsItem(43, "d4bcb1", 16, 16, 16, 16, 0));
		_dataArray.Add(new AvatarSkinColorsItem(44, "ceb5aa", 8, 8, 8, 8, 1));
		_dataArray.Add(new AvatarSkinColorsItem(45, "c9b5a6", 4, 4, 4, 4, 1));
		_dataArray.Add(new AvatarSkinColorsItem(46, "aa9e95", 2, 2, 2, 2, 2));
		_dataArray.Add(new AvatarSkinColorsItem(47, "82746f", 1, 1, 1, 1, 2));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AvatarSkinColorsItem>(48);
		CreateItems0();
	}
}
