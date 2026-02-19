using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SecretInformationAppliedContent : ConfigData<SecretInformationAppliedContentItem, short>
{
	public static class DefKey
	{
		public const short Repeat = 0;
	}

	public static class DefValue
	{
		public static SecretInformationAppliedContentItem Repeat => Instance[(short)0];
	}

	public static SecretInformationAppliedContent Instance = new SecretInformationAppliedContent();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "LinkedResult", "TemplateId", "Texts" };

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
		_dataArray.Add(new SecretInformationAppliedContentItem(0, -1, new int[5]));
		_dataArray.Add(new SecretInformationAppliedContentItem(1, -1, new int[5] { 1, 1, 1, 1, 1 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(2, -1, new int[5] { 2, 2, 2, 2, 2 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(3, -1, new int[5] { 3, 4, 5, 6, 7 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(4, -1, new int[5] { 8, 9, 10, 11, 12 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(5, -1, new int[5] { 13, 14, 15, 16, 17 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(6, -1, new int[5] { 18, 19, 20, 21, 22 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(7, -1, new int[5] { 23, 24, 25, 26, 27 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(8, -1, new int[5] { 28, 29, 30, 31, 32 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(9, -1, new int[5] { 33, 34, 35, 36, 37 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(10, -1, new int[5] { 38, 39, 40, 41, 42 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(11, -1, new int[5] { 43, 44, 45, 46, 47 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(12, -1, new int[5] { 48, 49, 50, 51, 52 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(13, -1, new int[5] { 53, 54, 55, 56, 57 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(14, -1, new int[5] { 58, 59, 60, 61, 62 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(15, -1, new int[5] { 63, 64, 65, 66, 67 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(16, -1, new int[5] { 68, 69, 70, 71, 72 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(17, -1, new int[5] { 73, 74, 75, 76, 77 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(18, -1, new int[5] { 78, 79, 80, 81, 82 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(19, -1, new int[5] { 83, 84, 85, 86, 87 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(20, -1, new int[5] { 88, 89, 90, 91, 92 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(21, -1, new int[5] { 93, 94, 95, 96, 97 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(22, -1, new int[5] { 98, 99, 100, 101, 102 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(23, -1, new int[5] { 103, 104, 105, 106, 107 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(24, -1, new int[5] { 108, 109, 110, 111, 112 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(25, -1, new int[5] { 113, 114, 115, 116, 117 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(26, -1, new int[5] { 118, 119, 120, 121, 122 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(27, -1, new int[5] { 123, 124, 125, 126, 127 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(28, -1, new int[5] { 128, 129, 130, 131, 132 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(29, -1, new int[5] { 133, 134, 135, 136, 137 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(30, -1, new int[5] { 138, 139, 140, 141, 142 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(31, -1, new int[5] { 143, 144, 145, 146, 147 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(32, -1, new int[5] { 148, 149, 150, 151, 152 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(33, -1, new int[5] { 153, 154, 155, 156, 157 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(34, -1, new int[5] { 158, 159, 160, 161, 162 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(35, -1, new int[5] { 163, 164, 165, 166, 167 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(36, -1, new int[5] { 168, 169, 170, 171, 172 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(37, -1, new int[5] { 173, 174, 175, 176, 177 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(38, -1, new int[5] { 178, 179, 180, 181, 182 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(39, -1, new int[5] { 183, 184, 185, 186, 187 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(40, -1, new int[5] { 188, 189, 190, 191, 192 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(41, -1, new int[5] { 193, 194, 195, 196, 197 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(42, -1, new int[5] { 198, 199, 200, 201, 202 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(43, -1, new int[5] { 203, 204, 205, 206, 207 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(44, -1, new int[5] { 208, 209, 210, 211, 212 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(45, -1, new int[5] { 213, 214, 215, 216, 217 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(46, -1, new int[5] { 218, 218, 218, 218, 218 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(47, -1, new int[5] { 219, 220, 221, 222, 223 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(48, -1, new int[5] { 224, 225, 226, 227, 228 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(49, 231, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(50, -1, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(51, 231, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(52, 243, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(53, 243, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(54, 254, null));
		_dataArray.Add(new SecretInformationAppliedContentItem(55, -1, new int[5] { 229, 229, 229, 229, 229 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(56, -1, new int[5] { 230, 231, 232, 233, 234 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(57, -1, new int[5] { 235, 236, 237, 238, 239 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(58, -1, new int[5] { 240, 241, 242, 243, 244 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(59, -1, new int[5] { 245, 246, 247, 248, 249 }));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new SecretInformationAppliedContentItem(60, -1, new int[5] { 250, 251, 252, 253, 254 }));
		_dataArray.Add(new SecretInformationAppliedContentItem(61, -1, new int[5] { 255, 256, 257, 258, 259 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SecretInformationAppliedContentItem>(62);
		CreateItems0();
		CreateItems1();
	}
}
