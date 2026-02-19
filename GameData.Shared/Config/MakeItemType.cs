using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class MakeItemType : ConfigData<MakeItemTypeItem, short>
{
	public static MakeItemType Instance = new MakeItemType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "ItemSubType", "MakeItemSubTypes", "TemplateId", "Name", "TypeName", "TypeBigIcon" };

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
		_dataArray.Add(new MakeItemTypeItem(0, 0, 1, 1, null, new List<short> { 0 }));
		_dataArray.Add(new MakeItemTypeItem(1, 2, 800, 2, null, new List<short> { 1 }));
		_dataArray.Add(new MakeItemTypeItem(2, 3, 14, 3, "building_icon_dushuang_0", new List<short> { 2 }));
		_dataArray.Add(new MakeItemTypeItem(3, 4, 15, 4, "building_icon_dusha_0", new List<short> { 3 }));
		_dataArray.Add(new MakeItemTypeItem(4, 5, 12, 6, "building_icon_jiguan_0", new List<short> { 4 }));
		_dataArray.Add(new MakeItemTypeItem(5, 7, 12, 6, "building_icon_jiguan_0", new List<short> { 5 }));
		_dataArray.Add(new MakeItemTypeItem(6, 8, 13, 9, "building_icon_lingfu_0", new List<short> { 6 }));
		_dataArray.Add(new MakeItemTypeItem(7, 10, 13, 9, "building_icon_lingfu_0", new List<short> { 7 }));
		_dataArray.Add(new MakeItemTypeItem(8, 11, 0, 12, "building_icon_zhenxia_0", new List<short> { 8 }));
		_dataArray.Add(new MakeItemTypeItem(9, 13, 0, 12, "building_icon_zhenxia_0", new List<short> { 9 }));
		_dataArray.Add(new MakeItemTypeItem(10, 14, 0, 12, "building_icon_zhenxia_0", new List<short> { 10 }));
		_dataArray.Add(new MakeItemTypeItem(11, 15, 0, 12, "building_icon_zhenxia_0", new List<short> { 11 }));
		_dataArray.Add(new MakeItemTypeItem(12, 16, 0, 12, "building_icon_zhenxia_0", new List<short> { 12 }));
		_dataArray.Add(new MakeItemTypeItem(13, 17, 0, 12, "building_icon_zhenxia_0", new List<short> { 13 }));
		_dataArray.Add(new MakeItemTypeItem(14, 18, 1, 19, "building_icon_duici_0", new List<short> { 14 }));
		_dataArray.Add(new MakeItemTypeItem(15, 20, 1, 19, "building_icon_duici_0", new List<short> { 15 }));
		_dataArray.Add(new MakeItemTypeItem(16, 21, 1, 19, "building_icon_duici_0", new List<short> { 16 }));
		_dataArray.Add(new MakeItemTypeItem(17, 22, 1, 19, "building_icon_duici_0", new List<short> { 17 }));
		_dataArray.Add(new MakeItemTypeItem(18, 23, 1, 19, "building_icon_duici_0", new List<short> { 18 }));
		_dataArray.Add(new MakeItemTypeItem(19, 24, 1, 19, "building_icon_duici_0", new List<short> { 19 }));
		_dataArray.Add(new MakeItemTypeItem(20, 25, 2, 26, "building_icon_anqi_0", new List<short> { 20 }));
		_dataArray.Add(new MakeItemTypeItem(21, 27, 2, 26, "building_icon_anqi_0", new List<short> { 21 }));
		_dataArray.Add(new MakeItemTypeItem(22, 28, 2, 26, "building_icon_anqi_0", new List<short> { 22 }));
		_dataArray.Add(new MakeItemTypeItem(23, 29, 2, 26, "building_icon_anqi_0", new List<short> { 23 }));
		_dataArray.Add(new MakeItemTypeItem(24, 30, 2, 26, "building_icon_anqi_0", new List<short> { 24 }));
		_dataArray.Add(new MakeItemTypeItem(25, 31, 2, 26, "building_icon_anqi_0", new List<short> { 25 }));
		_dataArray.Add(new MakeItemTypeItem(26, 32, 3, 33, "building_icon_xiaodi_0", new List<short> { 26 }));
		_dataArray.Add(new MakeItemTypeItem(27, 34, 3, 33, "building_icon_xiaodi_0", new List<short> { 27 }));
		_dataArray.Add(new MakeItemTypeItem(28, 35, 3, 33, "building_icon_xiaodi_0", new List<short> { 28 }));
		_dataArray.Add(new MakeItemTypeItem(29, 36, 3, 33, "building_icon_xiaodi_0", new List<short> { 29 }));
		_dataArray.Add(new MakeItemTypeItem(30, 37, 3, 33, "building_icon_xiaodi_0", new List<short> { 30 }));
		_dataArray.Add(new MakeItemTypeItem(31, 38, 3, 33, "building_icon_xiaodi_0", new List<short> { 31 }));
		_dataArray.Add(new MakeItemTypeItem(32, 39, 4, 40, "building_icon_quantao_0", new List<short> { 32, 33 }));
		_dataArray.Add(new MakeItemTypeItem(33, 41, 4, 40, "building_icon_quantao_0", new List<short> { 34 }));
		_dataArray.Add(new MakeItemTypeItem(34, 42, 4, 40, "building_icon_quantao_0", new List<short> { 35 }));
		_dataArray.Add(new MakeItemTypeItem(35, 43, 4, 40, "building_icon_quantao_0", new List<short> { 36, 37 }));
		_dataArray.Add(new MakeItemTypeItem(36, 44, 4, 40, "building_icon_quantao_0", new List<short> { 38, 39 }));
		_dataArray.Add(new MakeItemTypeItem(37, 45, 4, 40, "building_icon_quantao_0", new List<short> { 40 }));
		_dataArray.Add(new MakeItemTypeItem(38, 46, 4, 40, "building_icon_quantao_0", new List<short> { 41 }));
		_dataArray.Add(new MakeItemTypeItem(39, 47, 4, 40, "building_icon_quantao_0", new List<short> { 42, 43 }));
		_dataArray.Add(new MakeItemTypeItem(40, 48, 5, 49, "building_icon_duanchu_0", new List<short> { 44 }));
		_dataArray.Add(new MakeItemTypeItem(41, 50, 5, 49, "building_icon_duanchu_0", new List<short> { 45 }));
		_dataArray.Add(new MakeItemTypeItem(42, 51, 5, 49, "building_icon_duanchu_0", new List<short> { 46 }));
		_dataArray.Add(new MakeItemTypeItem(43, 52, 5, 49, "building_icon_duanchu_0", new List<short> { 47 }));
		_dataArray.Add(new MakeItemTypeItem(44, 53, 5, 49, "building_icon_duanchu_0", new List<short> { 48 }));
		_dataArray.Add(new MakeItemTypeItem(45, 54, 5, 49, "building_icon_duanchu_0", new List<short> { 49 }));
		_dataArray.Add(new MakeItemTypeItem(46, 55, 8, 56, "building_icon_jian_0", new List<short> { 50, 51, 52 }));
		_dataArray.Add(new MakeItemTypeItem(47, 57, 8, 56, "building_icon_jian_0", new List<short> { 53 }));
		_dataArray.Add(new MakeItemTypeItem(48, 58, 8, 56, "building_icon_jian_0", new List<short> { 54 }));
		_dataArray.Add(new MakeItemTypeItem(49, 59, 8, 56, "building_icon_jian_0", new List<short> { 55, 56, 57 }));
		_dataArray.Add(new MakeItemTypeItem(50, 60, 8, 56, "building_icon_jian_0", new List<short> { 58 }));
		_dataArray.Add(new MakeItemTypeItem(51, 61, 8, 56, "building_icon_jian_0", new List<short> { 59 }));
		_dataArray.Add(new MakeItemTypeItem(52, 62, 9, 63, "building_icon_dao_0", new List<short> { 60, 61, 62 }));
		_dataArray.Add(new MakeItemTypeItem(53, 64, 9, 63, "building_icon_dao_0", new List<short> { 63 }));
		_dataArray.Add(new MakeItemTypeItem(54, 65, 9, 63, "building_icon_dao_0", new List<short> { 64 }));
		_dataArray.Add(new MakeItemTypeItem(55, 66, 9, 63, "building_icon_dao_0", new List<short> { 65, 66, 67 }));
		_dataArray.Add(new MakeItemTypeItem(56, 67, 9, 63, "building_icon_dao_0", new List<short> { 68 }));
		_dataArray.Add(new MakeItemTypeItem(57, 68, 9, 63, "building_icon_dao_0", new List<short> { 69 }));
		_dataArray.Add(new MakeItemTypeItem(58, 69, 10, 70, "building_icon_changbing_0", new List<short> { 70, 71 }));
		_dataArray.Add(new MakeItemTypeItem(59, 71, 10, 70, "building_icon_changbing_0", new List<short> { 72, 73 }));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new MakeItemTypeItem(60, 72, 10, 70, "building_icon_changbing_0", new List<short> { 74 }));
		_dataArray.Add(new MakeItemTypeItem(61, 73, 10, 70, "building_icon_changbing_0", new List<short> { 75, 76 }));
		_dataArray.Add(new MakeItemTypeItem(62, 74, 10, 70, "building_icon_changbing_0", new List<short> { 77, 78 }));
		_dataArray.Add(new MakeItemTypeItem(63, 75, 10, 70, "building_icon_changbing_0", new List<short> { 79 }));
		_dataArray.Add(new MakeItemTypeItem(64, 76, 11, 77, "building_icon_yaoqin_0", new List<short> { 80 }));
		_dataArray.Add(new MakeItemTypeItem(65, 78, 11, 77, "building_icon_yaoqin_0", new List<short> { 81 }));
		_dataArray.Add(new MakeItemTypeItem(66, 79, 11, 77, "building_icon_yaoqin_0", new List<short> { 82 }));
		_dataArray.Add(new MakeItemTypeItem(67, 80, 11, 77, "building_icon_yaoqin_0", new List<short> { 83 }));
		_dataArray.Add(new MakeItemTypeItem(68, 81, 11, 77, "building_icon_yaoqin_0", new List<short> { 84 }));
		_dataArray.Add(new MakeItemTypeItem(69, 82, 11, 77, "building_icon_yaoqin_0", new List<short> { 85 }));
		_dataArray.Add(new MakeItemTypeItem(70, 83, 6, 84, "building_icon_fuchen_0", new List<short> { 86 }));
		_dataArray.Add(new MakeItemTypeItem(71, 85, 6, 84, "building_icon_fuchen_0", new List<short> { 87 }));
		_dataArray.Add(new MakeItemTypeItem(72, 86, 6, 84, "building_icon_fuchen_0", new List<short> { 88 }));
		_dataArray.Add(new MakeItemTypeItem(73, 87, 6, 84, "building_icon_fuchen_0", new List<short> { 89 }));
		_dataArray.Add(new MakeItemTypeItem(74, 88, 7, 89, "building_icon_changbian_0", new List<short> { 90 }));
		_dataArray.Add(new MakeItemTypeItem(75, 90, 7, 89, "building_icon_changbian_0", new List<short> { 91 }));
		_dataArray.Add(new MakeItemTypeItem(76, 91, 7, 89, "building_icon_changbian_0", new List<short> { 92 }));
		_dataArray.Add(new MakeItemTypeItem(77, 92, 7, 89, "building_icon_changbian_0", new List<short> { 93 }));
		_dataArray.Add(new MakeItemTypeItem(78, 93, 200, 94, "building_icon_baowu_0", new List<short> { 94 }));
		_dataArray.Add(new MakeItemTypeItem(79, 95, 200, 94, "building_icon_baowu_0", new List<short> { 95, 96, 97 }));
		_dataArray.Add(new MakeItemTypeItem(80, 96, 200, 94, "building_icon_baowu_0", new List<short> { 98, 99, 100, 101, 102, 103, 104 }));
		_dataArray.Add(new MakeItemTypeItem(81, 97, 200, 94, "building_icon_baowu_0", new List<short> { 105 }));
		_dataArray.Add(new MakeItemTypeItem(82, 98, 200, 94, "building_icon_baowu_0", new List<short> { 106 }));
		_dataArray.Add(new MakeItemTypeItem(83, 99, 200, 94, "building_icon_baowu_0", new List<short> { 107, 108, 109 }));
		_dataArray.Add(new MakeItemTypeItem(84, 100, 200, 94, "building_icon_baowu_0", new List<short> { 110, 111, 112, 113, 114, 115, 116 }));
		_dataArray.Add(new MakeItemTypeItem(85, 101, 200, 94, "building_icon_baowu_0", new List<short> { 117 }));
		_dataArray.Add(new MakeItemTypeItem(86, 102, 100, 103, "building_icon_guanshi_0", new List<short> { 118 }));
		_dataArray.Add(new MakeItemTypeItem(87, 104, 100, 103, "building_icon_guanshi_0", new List<short> { 119 }));
		_dataArray.Add(new MakeItemTypeItem(88, 105, 100, 103, "building_icon_guanshi_0", new List<short> { 120 }));
		_dataArray.Add(new MakeItemTypeItem(89, 106, 100, 103, "building_icon_guanshi_0", new List<short> { 121, 122, 123, 124 }));
		_dataArray.Add(new MakeItemTypeItem(90, 107, 100, 103, "building_icon_guanshi_0", new List<short> { 125 }));
		_dataArray.Add(new MakeItemTypeItem(91, 108, 100, 103, "building_icon_guanshi_0", new List<short> { 126 }));
		_dataArray.Add(new MakeItemTypeItem(92, 109, 100, 103, "building_icon_guanshi_0", new List<short> { 127 }));
		_dataArray.Add(new MakeItemTypeItem(93, 110, 100, 103, "building_icon_guanshi_0", new List<short> { 128, 129, 130, 131 }));
		_dataArray.Add(new MakeItemTypeItem(94, 111, 103, 112, "building_icon_huzu_0", new List<short> { 132 }));
		_dataArray.Add(new MakeItemTypeItem(95, 113, 103, 112, "building_icon_huzu_0", new List<short> { 133 }));
		_dataArray.Add(new MakeItemTypeItem(96, 114, 103, 112, "building_icon_huzu_0", new List<short> { 134 }));
		_dataArray.Add(new MakeItemTypeItem(97, 115, 103, 112, "building_icon_huzu_0", new List<short> { 135, 136, 137, 138 }));
		_dataArray.Add(new MakeItemTypeItem(98, 116, 103, 112, "building_icon_huzu_0", new List<short> { 139 }));
		_dataArray.Add(new MakeItemTypeItem(99, 117, 103, 112, "building_icon_huzu_0", new List<short> { 140 }));
		_dataArray.Add(new MakeItemTypeItem(100, 118, 103, 112, "building_icon_huzu_0", new List<short> { 141 }));
		_dataArray.Add(new MakeItemTypeItem(101, 119, 103, 112, "building_icon_huzu_0", new List<short> { 142, 143, 144, 145 }));
		_dataArray.Add(new MakeItemTypeItem(102, 120, 101, 121, "building_icon_hujia_0", new List<short> { 146, 147 }));
		_dataArray.Add(new MakeItemTypeItem(103, 122, 101, 121, "building_icon_hujia_0", new List<short> { 148 }));
		_dataArray.Add(new MakeItemTypeItem(104, 123, 101, 121, "building_icon_hujia_0", new List<short> { 149 }));
		_dataArray.Add(new MakeItemTypeItem(105, 124, 101, 121, "building_icon_hujia_0", new List<short> { 150, 151, 152, 153 }));
		_dataArray.Add(new MakeItemTypeItem(106, 125, 101, 121, "building_icon_hujia_0", new List<short> { 154, 155 }));
		_dataArray.Add(new MakeItemTypeItem(107, 126, 101, 121, "building_icon_hujia_0", new List<short> { 156 }));
		_dataArray.Add(new MakeItemTypeItem(108, 127, 101, 121, "building_icon_hujia_0", new List<short> { 157 }));
		_dataArray.Add(new MakeItemTypeItem(109, 128, 101, 121, "building_icon_hujia_0", new List<short> { 158, 159, 160, 161 }));
		_dataArray.Add(new MakeItemTypeItem(110, 129, 102, 130, "building_icon_hubi_0", new List<short> { 162 }));
		_dataArray.Add(new MakeItemTypeItem(111, 131, 102, 130, "building_icon_hubi_0", new List<short> { 163 }));
		_dataArray.Add(new MakeItemTypeItem(112, 132, 102, 130, "building_icon_hubi_0", new List<short> { 164 }));
		_dataArray.Add(new MakeItemTypeItem(113, 133, 102, 130, "building_icon_hubi_0", new List<short> { 165, 166, 167, 168 }));
		_dataArray.Add(new MakeItemTypeItem(114, 134, 102, 130, "building_icon_hubi_0", new List<short> { 169 }));
		_dataArray.Add(new MakeItemTypeItem(115, 135, 102, 130, "building_icon_hubi_0", new List<short> { 170 }));
		_dataArray.Add(new MakeItemTypeItem(116, 136, 102, 130, "building_icon_hubi_0", new List<short> { 171 }));
		_dataArray.Add(new MakeItemTypeItem(117, 137, 102, 130, "building_icon_hubi_0", new List<short> { 172, 173, 174, 175 }));
		_dataArray.Add(new MakeItemTypeItem(118, 138, 300, 139, "building_icon_yizhuang_0", new List<short> { 176 }));
		_dataArray.Add(new MakeItemTypeItem(119, 140, 300, 139, "building_icon_yizhuang_0", new List<short> { 177, 277 }));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new MakeItemTypeItem(120, 141, 400, 142, "building_icon_daibu_0", new List<short> { 178 }));
		_dataArray.Add(new MakeItemTypeItem(121, 143, 400, 142, "building_icon_daibu_0", new List<short> { 179 }));
		_dataArray.Add(new MakeItemTypeItem(122, 144, 200, 144, "building_icon_koudai_0", new List<short> { 180 }));
		_dataArray.Add(new MakeItemTypeItem(123, 145, 1206, 145, "building_icon_shengsuo_0", new List<short> { 181 }));
		_dataArray.Add(new MakeItemTypeItem(124, 146, 701, 147, "building_icon_sushi_0", new List<short> { 182 }));
		_dataArray.Add(new MakeItemTypeItem(125, 148, 701, 147, "building_icon_sushi_0", new List<short> { 183 }));
		_dataArray.Add(new MakeItemTypeItem(126, 149, 701, 147, "building_icon_sushi_0", new List<short> { 184 }));
		_dataArray.Add(new MakeItemTypeItem(127, 150, 701, 147, "building_icon_sushi_0", new List<short> { 185 }));
		_dataArray.Add(new MakeItemTypeItem(128, 151, 701, 147, "building_icon_sushi_0", new List<short> { 186 }));
		_dataArray.Add(new MakeItemTypeItem(129, 152, 701, 147, "building_icon_sushi_0", new List<short> { 187 }));
		_dataArray.Add(new MakeItemTypeItem(130, 153, 701, 147, "building_icon_sushi_0", new List<short> { 188 }));
		_dataArray.Add(new MakeItemTypeItem(131, 154, 701, 147, "building_icon_sushi_0", new List<short> { 189 }));
		_dataArray.Add(new MakeItemTypeItem(132, 155, 701, 147, "building_icon_sushi_0", new List<short> { 190 }));
		_dataArray.Add(new MakeItemTypeItem(133, 156, 701, 147, "building_icon_sushi_0", new List<short> { 191 }));
		_dataArray.Add(new MakeItemTypeItem(134, 157, 701, 147, "building_icon_sushi_0", new List<short> { 192 }));
		_dataArray.Add(new MakeItemTypeItem(135, 158, 701, 147, "building_icon_sushi_0", new List<short> { 193 }));
		_dataArray.Add(new MakeItemTypeItem(136, 159, 701, 147, "building_icon_sushi_0", new List<short> { 194 }));
		_dataArray.Add(new MakeItemTypeItem(137, 160, 701, 147, "building_icon_sushi_0", new List<short> { 195 }));
		_dataArray.Add(new MakeItemTypeItem(138, 161, 700, 147, "building_icon_sushi_0", new List<short> { 196 }));
		_dataArray.Add(new MakeItemTypeItem(139, 162, 700, 147, "building_icon_sushi_0", new List<short> { 197 }));
		_dataArray.Add(new MakeItemTypeItem(140, 163, 700, 147, "building_icon_sushi_0", new List<short> { 198 }));
		_dataArray.Add(new MakeItemTypeItem(141, 164, 700, 147, "building_icon_sushi_0", new List<short> { 199 }));
		_dataArray.Add(new MakeItemTypeItem(142, 165, 700, 147, "building_icon_sushi_0", new List<short> { 200 }));
		_dataArray.Add(new MakeItemTypeItem(143, 166, 700, 147, "building_icon_sushi_0", new List<short> { 201 }));
		_dataArray.Add(new MakeItemTypeItem(144, 167, 700, 147, "building_icon_sushi_0", new List<short> { 202 }));
		_dataArray.Add(new MakeItemTypeItem(145, 168, 701, 147, "building_icon_sushi_0", new List<short> { 203 }));
		_dataArray.Add(new MakeItemTypeItem(146, 169, 701, 147, "building_icon_sushi_0", new List<short> { 204 }));
		_dataArray.Add(new MakeItemTypeItem(147, 170, 701, 147, "building_icon_sushi_0", new List<short> { 205 }));
		_dataArray.Add(new MakeItemTypeItem(148, 171, 701, 147, "building_icon_sushi_0", new List<short> { 206 }));
		_dataArray.Add(new MakeItemTypeItem(149, 172, 701, 147, "building_icon_sushi_0", new List<short> { 207 }));
		_dataArray.Add(new MakeItemTypeItem(150, 173, 701, 147, "building_icon_sushi_0", new List<short> { 208 }));
		_dataArray.Add(new MakeItemTypeItem(151, 174, 701, 147, "building_icon_sushi_0", new List<short> { 209 }));
		_dataArray.Add(new MakeItemTypeItem(152, 175, 801, 176, "building_icon_duyao_0", new List<short> { 210 }));
		_dataArray.Add(new MakeItemTypeItem(153, 177, 801, 176, "building_icon_duyao_0", new List<short> { 211 }));
		_dataArray.Add(new MakeItemTypeItem(154, 178, 801, 176, "building_icon_duyao_0", new List<short> { 212 }));
		_dataArray.Add(new MakeItemTypeItem(155, 179, 801, 176, "building_icon_duyao_0", new List<short> { 213 }));
		_dataArray.Add(new MakeItemTypeItem(156, 180, 801, 176, "building_icon_duyao_0", new List<short> { 214 }));
		_dataArray.Add(new MakeItemTypeItem(157, 181, 801, 176, "building_icon_duyao_0", new List<short> { 215 }));
		_dataArray.Add(new MakeItemTypeItem(158, 182, 800, 183, "building_icon_danyao_0", new List<short> { 240, 216 }));
		_dataArray.Add(new MakeItemTypeItem(159, 184, 800, 183, "building_icon_danyao_0", new List<short> { 241, 217 }));
		_dataArray.Add(new MakeItemTypeItem(160, 185, 800, 183, "building_icon_danyao_0", new List<short> { 242, 218 }));
		_dataArray.Add(new MakeItemTypeItem(161, 186, 800, 183, "building_icon_danyao_0", new List<short> { 243, 219 }));
		_dataArray.Add(new MakeItemTypeItem(162, 187, 800, 183, "building_icon_danyao_0", new List<short> { 244, 220 }));
		_dataArray.Add(new MakeItemTypeItem(163, 188, 800, 183, "building_icon_danyao_0", new List<short> { 245, 221 }));
		_dataArray.Add(new MakeItemTypeItem(164, 189, 800, 183, "building_icon_danyao_0", new List<short> { 246, 222 }));
		_dataArray.Add(new MakeItemTypeItem(165, 190, 800, 183, "building_icon_danyao_0", new List<short> { 247, 223 }));
		_dataArray.Add(new MakeItemTypeItem(166, 191, 800, 183, "building_icon_danyao_0", new List<short> { 248, 224 }));
		_dataArray.Add(new MakeItemTypeItem(167, 192, 800, 183, "building_icon_danyao_0", new List<short> { 249, 225 }));
		_dataArray.Add(new MakeItemTypeItem(168, 193, 800, 183, "building_icon_danyao_0", new List<short> { 250, 226 }));
		_dataArray.Add(new MakeItemTypeItem(169, 194, 800, 183, "building_icon_danyao_0", new List<short> { 251, 227 }));
		_dataArray.Add(new MakeItemTypeItem(170, 195, 800, 183, "building_icon_danyao_0", new List<short> { 252, 228 }));
		_dataArray.Add(new MakeItemTypeItem(171, 196, 800, 183, "building_icon_danyao_0", new List<short> { 253, 229 }));
		_dataArray.Add(new MakeItemTypeItem(172, 197, 800, 183, "building_icon_danyao_0", new List<short> { 254, 230 }));
		_dataArray.Add(new MakeItemTypeItem(173, 198, 800, 183, "building_icon_danyao_0", new List<short> { 255, 231 }));
		_dataArray.Add(new MakeItemTypeItem(174, 199, 800, 183, "building_icon_danyao_0", new List<short> { 256, 232 }));
		_dataArray.Add(new MakeItemTypeItem(175, 200, 800, 183, "building_icon_danyao_0", new List<short> { 257, 233 }));
		_dataArray.Add(new MakeItemTypeItem(176, 201, 800, 183, "building_icon_danyao_0", new List<short> { 258, 234 }));
		_dataArray.Add(new MakeItemTypeItem(177, 202, 800, 183, "building_icon_danyao_0", new List<short> { 259, 235 }));
		_dataArray.Add(new MakeItemTypeItem(178, 203, 800, 183, "building_icon_danyao_0", new List<short> { 260, 236 }));
		_dataArray.Add(new MakeItemTypeItem(179, 204, 800, 183, "building_icon_danyao_0", new List<short> { 261, 237 }));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new MakeItemTypeItem(180, 205, 800, 183, "building_icon_danyao_0", new List<short> { 262, 238 }));
		_dataArray.Add(new MakeItemTypeItem(181, 206, 800, 183, "building_icon_danyao_0", new List<short> { 263, 239 }));
		_dataArray.Add(new MakeItemTypeItem(182, 207, 303, 139, null, new List<short> { 276 }));
		_dataArray.Add(new MakeItemTypeItem(183, 208, 302, 139, "building_icon_yizhuang_0", new List<short> { 277 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<MakeItemTypeItem>(184);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
	}
}
