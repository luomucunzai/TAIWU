using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LocalZangNames : IConfigData
{
	public static LocalZangNames Instance = new LocalZangNames();

	public short SoloStart = 1;

	public short SoloEnd = 15;

	public short MixStart = 100;

	public short MixEnd = 168;

	public short MaleStart = 300;

	public short MaleEnd = 319;

	public short FemaleStart = 400;

	public short FemaleEnd = 423;

	public ZangNameItem[] ZangNameCore;

	public IReadOnlyDictionary<string, int> RefNameMap
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public int GetItemId(string refName)
	{
		throw new NotImplementedException();
	}

	public int AddExtraItem(string identifier, string refName, object configItem)
	{
		throw new NotImplementedException();
	}

	public void Init()
	{
		ZangNameItem[] array = new ZangNameItem[424];
		array[1] = new ZangNameItem(1, "尼玛");
		array[2] = new ZangNameItem(2, "达瓦");
		array[3] = new ZangNameItem(3, "米玛");
		array[4] = new ZangNameItem(4, "拉巴");
		array[5] = new ZangNameItem(5, "普布");
		array[6] = new ZangNameItem(6, "巴桑");
		array[7] = new ZangNameItem(7, "边巴");
		array[8] = new ZangNameItem(8, "次吉");
		array[9] = new ZangNameItem(9, "次尼");
		array[10] = new ZangNameItem(10, "次松");
		array[11] = new ZangNameItem(11, "次西");
		array[12] = new ZangNameItem(12, "次珠");
		array[13] = new ZangNameItem(13, "次顿");
		array[14] = new ZangNameItem(14, "次杰");
		array[15] = new ZangNameItem(15, "次穷");
		array[100] = new ZangNameItem(100, "次仁");
		array[101] = new ZangNameItem(101, "次旦");
		array[102] = new ZangNameItem(102, "才让");
		array[103] = new ZangNameItem(103, "扎西");
		array[104] = new ZangNameItem(104, "洛登");
		array[105] = new ZangNameItem(105, "拉泽");
		array[106] = new ZangNameItem(106, "桑珠");
		array[107] = new ZangNameItem(107, "群佩");
		array[108] = new ZangNameItem(108, "曲吉");
		array[109] = new ZangNameItem(109, "曲礼");
		array[110] = new ZangNameItem(110, "南卡");
		array[111] = new ZangNameItem(111, "嘉措");
		array[112] = new ZangNameItem(112, "噶玛");
		array[113] = new ZangNameItem(113, "嘉央");
		array[114] = new ZangNameItem(114, "措姆");
		array[115] = new ZangNameItem(115, "玛琼");
		array[116] = new ZangNameItem(116, "那日");
		array[117] = new ZangNameItem(117, "那森");
		array[118] = new ZangNameItem(118, "白巴");
		array[119] = new ZangNameItem(119, "达吉");
		array[120] = new ZangNameItem(120, "达娃");
		array[121] = new ZangNameItem(121, "格桑");
		array[122] = new ZangNameItem(122, "格勒");
		array[123] = new ZangNameItem(123, "琼达");
		array[124] = new ZangNameItem(124, "德吉");
		array[125] = new ZangNameItem(125, "平措");
		array[126] = new ZangNameItem(126, "加西");
		array[127] = new ZangNameItem(127, "加南");
		array[128] = new ZangNameItem(128, "甲央");
		array[129] = new ZangNameItem(129, "甲热");
		array[130] = new ZangNameItem(130, "多仁");
		array[131] = new ZangNameItem(131, "格利");
		array[132] = new ZangNameItem(132, "曲扎");
		array[133] = new ZangNameItem(133, "翁丁");
		array[134] = new ZangNameItem(134, "苏朗");
		array[135] = new ZangNameItem(135, "呷绒");
		array[136] = new ZangNameItem(136, "旺杰");
		array[137] = new ZangNameItem(137, "向巴");
		array[138] = new ZangNameItem(138, "强巴");
		array[139] = new ZangNameItem(139, "郎地");
		array[140] = new ZangNameItem(140, "朗日");
		array[141] = new ZangNameItem(141, "朗杰");
		array[142] = new ZangNameItem(142, "泽旺");
		array[143] = new ZangNameItem(143, "帕加");
		array[144] = new ZangNameItem(144, "其加");
		array[145] = new ZangNameItem(145, "白宗");
		array[146] = new ZangNameItem(146, "甲黑");
		array[147] = new ZangNameItem(147, "让祖");
		array[148] = new ZangNameItem(148, "黑八");
		array[149] = new ZangNameItem(149, "帕绸");
		array[150] = new ZangNameItem(150, "仁增");
		array[151] = new ZangNameItem(151, "伦珠");
		array[152] = new ZangNameItem(152, "洛桑");
		array[153] = new ZangNameItem(153, "土登");
		array[154] = new ZangNameItem(154, "丹巴");
		array[155] = new ZangNameItem(155, "益西");
		array[156] = new ZangNameItem(156, "德勒");
		array[157] = new ZangNameItem(157, "欧珠");
		array[158] = new ZangNameItem(158, "杰布");
		array[159] = new ZangNameItem(159, "阿旺");
		array[160] = new ZangNameItem(160, "坚赞");
		array[161] = new ZangNameItem(161, "绛曲");
		array[162] = new ZangNameItem(162, "仁青");
		array[163] = new ZangNameItem(163, "奥色");
		array[164] = new ZangNameItem(164, "曲珍");
		array[165] = new ZangNameItem(165, "金巴");
		array[166] = new ZangNameItem(166, "让穷");
		array[167] = new ZangNameItem(167, "洛卓");
		array[168] = new ZangNameItem(168, "耶喜");
		array[300] = new ZangNameItem(300, "贡布");
		array[301] = new ZangNameItem(301, "帕卓");
		array[302] = new ZangNameItem(302, "顿珠");
		array[303] = new ZangNameItem(303, "丹珠");
		array[304] = new ZangNameItem(304, "多吉");
		array[305] = new ZangNameItem(305, "多杰");
		array[306] = new ZangNameItem(306, "多金");
		array[307] = new ZangNameItem(307, "晋美");
		array[308] = new ZangNameItem(308, "旺堆");
		array[309] = new ZangNameItem(309, "珠杰");
		array[310] = new ZangNameItem(310, "罗追");
		array[311] = new ZangNameItem(311, "占堆");
		array[312] = new ZangNameItem(312, "桑杰");
		array[313] = new ZangNameItem(313, "丹增");
		array[314] = new ZangNameItem(314, "索朗");
		array[315] = new ZangNameItem(315, "森格");
		array[316] = new ZangNameItem(316, "巴沃");
		array[317] = new ZangNameItem(317, "慈诚");
		array[318] = new ZangNameItem(318, "成来");
		array[319] = new ZangNameItem(319, "宗哲");
		array[400] = new ZangNameItem(400, "美朵");
		array[401] = new ZangNameItem(401, "卓玛");
		array[402] = new ZangNameItem(402, "卓嘎");
		array[403] = new ZangNameItem(403, "白玛");
		array[404] = new ZangNameItem(404, "梅朵");
		array[405] = new ZangNameItem(405, "布赤");
		array[406] = new ZangNameItem(406, "贡桑");
		array[407] = new ZangNameItem(407, "拉珍");
		array[408] = new ZangNameItem(408, "拉姆");
		array[409] = new ZangNameItem(409, "旺姆");
		array[410] = new ZangNameItem(410, "桑姆");
		array[411] = new ZangNameItem(411, "措姆");
		array[412] = new ZangNameItem(412, "央金");
		array[413] = new ZangNameItem(413, "央拉");
		array[414] = new ZangNameItem(414, "央宗");
		array[415] = new ZangNameItem(415, "曲珍");
		array[416] = new ZangNameItem(416, "阿加");
		array[417] = new ZangNameItem(417, "拉吉");
		array[418] = new ZangNameItem(418, "才吉");
		array[419] = new ZangNameItem(419, "仓觉");
		array[420] = new ZangNameItem(420, "仓拉");
		array[421] = new ZangNameItem(421, "罗布");
		array[422] = new ZangNameItem(422, "索娜");
		array[423] = new ZangNameItem(423, "贝玛");
		ZangNameCore = array;
	}
}
