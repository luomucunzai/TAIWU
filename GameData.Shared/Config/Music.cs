using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class Music : ConfigData<MusicItem, short>
{
	public static Music Instance = new Music();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "MapBlock", "MapState", "TemporaryFeature", "TemplateId", "Name", "Desc", "Evaluation" };

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
		_dataArray.Add(new MusicItem(0, 0, 0, -1, 30, 30, 416, 1, 2));
		_dataArray.Add(new MusicItem(1, 3, -1, 0, 30, 30, 417, 4, 5));
		_dataArray.Add(new MusicItem(2, 6, 19, -1, 20, 0, 418, 7, 8));
		_dataArray.Add(new MusicItem(3, 9, 20, -1, 20, 0, 419, 10, 11));
		_dataArray.Add(new MusicItem(4, 12, 21, -1, 20, 0, 420, 13, 14));
		_dataArray.Add(new MusicItem(5, 15, 22, -1, 20, 0, 421, 16, 17));
		_dataArray.Add(new MusicItem(6, 18, 23, -1, 20, 0, 422, 19, 20));
		_dataArray.Add(new MusicItem(7, 21, 24, -1, 20, 0, 423, 22, 23));
		_dataArray.Add(new MusicItem(8, 24, 25, -1, 20, 0, 424, 25, 26));
		_dataArray.Add(new MusicItem(9, 27, 26, -1, 20, 0, 425, 28, 29));
		_dataArray.Add(new MusicItem(10, 30, 27, -1, 20, 0, 426, 31, 32));
		_dataArray.Add(new MusicItem(11, 33, 28, -1, 20, 0, 427, 34, 35));
		_dataArray.Add(new MusicItem(12, 36, 29, -1, 20, 0, 428, 37, 38));
		_dataArray.Add(new MusicItem(13, 39, 30, -1, 20, 0, 429, 40, 41));
		_dataArray.Add(new MusicItem(14, 42, 31, -1, 20, 0, 430, 43, 44));
		_dataArray.Add(new MusicItem(15, 45, 32, -1, 20, 0, 431, 46, 47));
		_dataArray.Add(new MusicItem(16, 48, 33, -1, 20, 0, 432, 49, 50));
		_dataArray.Add(new MusicItem(17, 51, 1, -1, 0, 20, 433, 52, 53));
		_dataArray.Add(new MusicItem(18, 54, 2, -1, 0, 20, 434, 55, 56));
		_dataArray.Add(new MusicItem(19, 57, 3, -1, 0, 20, 435, 58, 59));
		_dataArray.Add(new MusicItem(20, 60, 4, -1, 0, 20, 436, 61, 62));
		_dataArray.Add(new MusicItem(21, 63, 5, -1, 0, 20, 437, 64, 65));
		_dataArray.Add(new MusicItem(22, 66, 6, -1, 0, 20, 438, 67, 68));
		_dataArray.Add(new MusicItem(23, 69, 7, -1, 0, 20, 439, 70, 71));
		_dataArray.Add(new MusicItem(24, 72, 8, -1, 0, 20, 440, 73, 74));
		_dataArray.Add(new MusicItem(25, 75, 9, -1, 0, 20, 441, 76, 77));
		_dataArray.Add(new MusicItem(26, 78, 10, -1, 0, 20, 442, 79, 80));
		_dataArray.Add(new MusicItem(27, 81, 11, -1, 0, 20, 443, 82, 83));
		_dataArray.Add(new MusicItem(28, 84, 12, -1, 0, 20, 444, 85, 86));
		_dataArray.Add(new MusicItem(29, 87, 13, -1, 0, 20, 445, 88, 89));
		_dataArray.Add(new MusicItem(30, 90, 14, -1, 0, 20, 446, 91, 92));
		_dataArray.Add(new MusicItem(31, 93, 15, -1, 0, 20, 447, 94, 95));
		_dataArray.Add(new MusicItem(32, 96, -1, 1, 10, 10, 448, 97, 98));
		_dataArray.Add(new MusicItem(33, 99, -1, 2, 10, 10, 449, 100, 101));
		_dataArray.Add(new MusicItem(34, 102, -1, 3, 10, 10, 450, 103, 104));
		_dataArray.Add(new MusicItem(35, 105, -1, 4, 10, 10, 451, 106, 107));
		_dataArray.Add(new MusicItem(36, 108, -1, 5, 10, 10, 452, 109, 110));
		_dataArray.Add(new MusicItem(37, 111, -1, 6, 10, 10, 453, 112, 113));
		_dataArray.Add(new MusicItem(38, 114, -1, 7, 10, 10, 454, 115, 116));
		_dataArray.Add(new MusicItem(39, 117, -1, 8, 10, 10, 455, 118, 119));
		_dataArray.Add(new MusicItem(40, 120, -1, 9, 10, 10, 456, 121, 122));
		_dataArray.Add(new MusicItem(41, 123, -1, 10, 10, 10, 457, 124, 125));
		_dataArray.Add(new MusicItem(42, 126, -1, 11, 10, 10, 458, 127, 128));
		_dataArray.Add(new MusicItem(43, 129, -1, 12, 10, 10, 459, 130, 131));
		_dataArray.Add(new MusicItem(44, 132, -1, 13, 10, 10, 460, 133, 134));
		_dataArray.Add(new MusicItem(45, 135, -1, 14, 10, 10, 461, 136, 137));
		_dataArray.Add(new MusicItem(46, 138, -1, 15, 10, 10, 462, 139, 140));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MusicItem>(47);
		CreateItems0();
	}

	public static int GetCharacterPropertyBonus(int key, ECharacterPropertyReferencedType property)
	{
		return Instance._dataArray[key].GetCharacterPropertyBonusInt(property);
	}

	public static int GetCharacterPropertyBonus(short[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<short> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(int[] keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int num2 = keys.Length; i < num2; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}

	public static int GetCharacterPropertyBonus(List<int> keys, ECharacterPropertyReferencedType property)
	{
		int num = 0;
		int i = 0;
		for (int count = keys.Count; i < count; i++)
		{
			num += Instance._dataArray[keys[i]].GetCharacterPropertyBonusInt(property);
		}
		return num;
	}
}
