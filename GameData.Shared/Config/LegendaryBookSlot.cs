using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LegendaryBookSlot : ConfigData<LegendaryBookSlotItem, short>
{
	public static LegendaryBookSlot Instance = new LegendaryBookSlot();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "ClassName" };

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
		_dataArray.Add(new LegendaryBookSlotItem(0, 0, 1, null));
		_dataArray.Add(new LegendaryBookSlotItem(1, 2, 3, "LegendaryBook.Neigong.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(2, 4, 5, "LegendaryBook.Neigong.YongJi"));
		_dataArray.Add(new LegendaryBookSlotItem(3, 6, 7, "LegendaryBook.Neigong.DaYing"));
		_dataArray.Add(new LegendaryBookSlotItem(4, 8, 9, "LegendaryBook.Neigong.DaCheng"));
		_dataArray.Add(new LegendaryBookSlotItem(5, 10, 11, "LegendaryBook.Neigong.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(6, 12, 13, "LegendaryBook.Neigong.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(7, 14, 15, null));
		_dataArray.Add(new LegendaryBookSlotItem(8, 16, 17, null));
		_dataArray.Add(new LegendaryBookSlotItem(9, 18, 19, null));
		_dataArray.Add(new LegendaryBookSlotItem(10, 20, 21, null));
		_dataArray.Add(new LegendaryBookSlotItem(11, 2, 3, "LegendaryBook.Posing.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(12, 22, 23, "LegendaryBook.Posing.FengShen"));
		_dataArray.Add(new LegendaryBookSlotItem(13, 24, 25, "LegendaryBook.Posing.YunYong"));
		_dataArray.Add(new LegendaryBookSlotItem(14, 26, 27, "LegendaryBook.Posing.JingShui"));
		_dataArray.Add(new LegendaryBookSlotItem(15, 10, 11, "LegendaryBook.Posing.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(16, 12, 13, "LegendaryBook.Posing.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(17, 28, 29, null));
		_dataArray.Add(new LegendaryBookSlotItem(18, 30, 31, null));
		_dataArray.Add(new LegendaryBookSlotItem(19, 32, 33, null));
		_dataArray.Add(new LegendaryBookSlotItem(20, 34, 35, null));
		_dataArray.Add(new LegendaryBookSlotItem(21, 2, 3, "LegendaryBook.Stunt.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(22, 36, 37, "LegendaryBook.Stunt.YuanLiu"));
		_dataArray.Add(new LegendaryBookSlotItem(23, 38, 39, "LegendaryBook.Stunt.ChaNa"));
		_dataArray.Add(new LegendaryBookSlotItem(24, 40, 41, "LegendaryBook.Stunt.ZhouQuan"));
		_dataArray.Add(new LegendaryBookSlotItem(25, 10, 11, "LegendaryBook.Stunt.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(26, 12, 13, "LegendaryBook.Stunt.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(27, 42, 43, null));
		_dataArray.Add(new LegendaryBookSlotItem(28, 44, 45, null));
		_dataArray.Add(new LegendaryBookSlotItem(29, 46, 47, null));
		_dataArray.Add(new LegendaryBookSlotItem(30, 48, 49, "LegendaryBook.FistAndPalm.KeDi"));
		_dataArray.Add(new LegendaryBookSlotItem(31, 2, 3, "LegendaryBook.FistAndPalm.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(32, 50, 51, "LegendaryBook.FistAndPalm.JinSha"));
		_dataArray.Add(new LegendaryBookSlotItem(33, 52, 53, "LegendaryBook.FistAndPalm.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(34, 10, 11, "LegendaryBook.FistAndPalm.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(35, 12, 13, "LegendaryBook.FistAndPalm.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(36, 54, 55, null));
		_dataArray.Add(new LegendaryBookSlotItem(37, 56, 57, null));
		_dataArray.Add(new LegendaryBookSlotItem(38, 58, 59, null));
		_dataArray.Add(new LegendaryBookSlotItem(39, 60, 61, null));
		_dataArray.Add(new LegendaryBookSlotItem(40, 62, 63, "LegendaryBook.Finger.DuanXue"));
		_dataArray.Add(new LegendaryBookSlotItem(41, 2, 3, "LegendaryBook.Finger.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(42, 64, 65, "LegendaryBook.Finger.SiXue"));
		_dataArray.Add(new LegendaryBookSlotItem(43, 52, 66, "LegendaryBook.Finger.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(44, 10, 11, "LegendaryBook.Finger.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(45, 12, 13, "LegendaryBook.Finger.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(46, 67, 68, null));
		_dataArray.Add(new LegendaryBookSlotItem(47, 69, 70, null));
		_dataArray.Add(new LegendaryBookSlotItem(48, 71, 72, null));
		_dataArray.Add(new LegendaryBookSlotItem(49, 73, 74, null));
		_dataArray.Add(new LegendaryBookSlotItem(50, 75, 76, "LegendaryBook.Leg.BingZu"));
		_dataArray.Add(new LegendaryBookSlotItem(51, 2, 3, "LegendaryBook.Leg.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(52, 77, 78, "LegendaryBook.Leg.XianSha"));
		_dataArray.Add(new LegendaryBookSlotItem(53, 52, 79, "LegendaryBook.Leg.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(54, 10, 11, "LegendaryBook.Leg.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(55, 12, 13, "LegendaryBook.Leg.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(56, 80, 81, null));
		_dataArray.Add(new LegendaryBookSlotItem(57, 82, 83, null));
		_dataArray.Add(new LegendaryBookSlotItem(58, 84, 85, null));
		_dataArray.Add(new LegendaryBookSlotItem(59, 86, 87, null));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new LegendaryBookSlotItem(60, 88, 89, "LegendaryBook.Throw.AnSha"));
		_dataArray.Add(new LegendaryBookSlotItem(61, 2, 3, "LegendaryBook.Throw.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(62, 90, 91, "LegendaryBook.Throw.YuanSha"));
		_dataArray.Add(new LegendaryBookSlotItem(63, 52, 92, "LegendaryBook.Throw.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(64, 10, 11, "LegendaryBook.Throw.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(65, 12, 13, "LegendaryBook.Throw.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(66, 93, 94, null));
		_dataArray.Add(new LegendaryBookSlotItem(67, 95, 96, null));
		_dataArray.Add(new LegendaryBookSlotItem(68, 97, 98, null));
		_dataArray.Add(new LegendaryBookSlotItem(69, 99, 100, null));
		_dataArray.Add(new LegendaryBookSlotItem(70, 101, 102, "LegendaryBook.Sword.HuaXi"));
		_dataArray.Add(new LegendaryBookSlotItem(71, 2, 3, "LegendaryBook.Sword.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(72, 103, 104, "LegendaryBook.Sword.ShiSha"));
		_dataArray.Add(new LegendaryBookSlotItem(73, 52, 105, "LegendaryBook.Sword.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(74, 10, 11, "LegendaryBook.Sword.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(75, 12, 13, "LegendaryBook.Sword.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(76, 106, 107, null));
		_dataArray.Add(new LegendaryBookSlotItem(77, 108, 109, null));
		_dataArray.Add(new LegendaryBookSlotItem(78, 110, 111, null));
		_dataArray.Add(new LegendaryBookSlotItem(79, 112, 113, null));
		_dataArray.Add(new LegendaryBookSlotItem(80, 114, 115, "LegendaryBook.Blade.ZhenLie"));
		_dataArray.Add(new LegendaryBookSlotItem(81, 2, 3, "LegendaryBook.Blade.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(82, 116, 117, "LegendaryBook.Blade.PoSha"));
		_dataArray.Add(new LegendaryBookSlotItem(83, 52, 118, "LegendaryBook.Blade.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(84, 10, 11, "LegendaryBook.Blade.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(85, 12, 13, "LegendaryBook.Blade.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(86, 119, 120, null));
		_dataArray.Add(new LegendaryBookSlotItem(87, 121, 122, null));
		_dataArray.Add(new LegendaryBookSlotItem(88, 123, 124, null));
		_dataArray.Add(new LegendaryBookSlotItem(89, 125, 126, null));
		_dataArray.Add(new LegendaryBookSlotItem(90, 127, 128, "LegendaryBook.Polearm.ChuangZhen"));
		_dataArray.Add(new LegendaryBookSlotItem(91, 2, 3, "LegendaryBook.Polearm.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(92, 129, 130, "LegendaryBook.Polearm.JueDou"));
		_dataArray.Add(new LegendaryBookSlotItem(93, 52, 131, "LegendaryBook.Polearm.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(94, 10, 11, "LegendaryBook.Polearm.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(95, 12, 13, "LegendaryBook.Polearm.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(96, 132, 133, null));
		_dataArray.Add(new LegendaryBookSlotItem(97, 134, 135, null));
		_dataArray.Add(new LegendaryBookSlotItem(98, 136, 137, null));
		_dataArray.Add(new LegendaryBookSlotItem(99, 138, 139, null));
		_dataArray.Add(new LegendaryBookSlotItem(100, 140, 141, "LegendaryBook.Special.GuiJi"));
		_dataArray.Add(new LegendaryBookSlotItem(101, 2, 3, "LegendaryBook.Special.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(102, 142, 143, "LegendaryBook.Special.QiShi"));
		_dataArray.Add(new LegendaryBookSlotItem(103, 52, 144, "LegendaryBook.Special.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(104, 10, 11, "LegendaryBook.Special.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(105, 12, 13, "LegendaryBook.Special.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(106, 145, 146, null));
		_dataArray.Add(new LegendaryBookSlotItem(107, 147, 148, null));
		_dataArray.Add(new LegendaryBookSlotItem(108, 149, 150, null));
		_dataArray.Add(new LegendaryBookSlotItem(109, 151, 152, null));
		_dataArray.Add(new LegendaryBookSlotItem(110, 153, 154, "LegendaryBook.Whip.YanSheng"));
		_dataArray.Add(new LegendaryBookSlotItem(111, 2, 3, "LegendaryBook.Whip.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(112, 155, 156, "LegendaryBook.Whip.JieQi"));
		_dataArray.Add(new LegendaryBookSlotItem(113, 52, 157, "LegendaryBook.Whip.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(114, 10, 11, "LegendaryBook.Whip.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(115, 12, 13, "LegendaryBook.Whip.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(116, 158, 159, null));
		_dataArray.Add(new LegendaryBookSlotItem(117, 160, 161, null));
		_dataArray.Add(new LegendaryBookSlotItem(118, 162, 163, null));
		_dataArray.Add(new LegendaryBookSlotItem(119, 164, 165, null));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new LegendaryBookSlotItem(120, 166, 167, "LegendaryBook.ControllableShot.QiBian"));
		_dataArray.Add(new LegendaryBookSlotItem(121, 2, 3, "LegendaryBook.ControllableShot.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(122, 168, 169, "LegendaryBook.ControllableShot.BianJie"));
		_dataArray.Add(new LegendaryBookSlotItem(123, 52, 170, "LegendaryBook.ControllableShot.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(124, 10, 11, "LegendaryBook.ControllableShot.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(125, 12, 13, "LegendaryBook.ControllableShot.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(126, 171, 172, null));
		_dataArray.Add(new LegendaryBookSlotItem(127, 173, 174, null));
		_dataArray.Add(new LegendaryBookSlotItem(128, 175, 176, null));
		_dataArray.Add(new LegendaryBookSlotItem(129, 177, 178, null));
		_dataArray.Add(new LegendaryBookSlotItem(130, 179, 180, "LegendaryBook.CombatMusic.LuanXin"));
		_dataArray.Add(new LegendaryBookSlotItem(131, 2, 3, "LegendaryBook.CombatMusic.ZhuanJie"));
		_dataArray.Add(new LegendaryBookSlotItem(132, 181, 182, "LegendaryBook.CombatMusic.KuangSheng"));
		_dataArray.Add(new LegendaryBookSlotItem(133, 52, 183, "LegendaryBook.CombatMusic.JiePo"));
		_dataArray.Add(new LegendaryBookSlotItem(134, 10, 11, "LegendaryBook.CombatMusic.JueZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(135, 12, 13, "LegendaryBook.CombatMusic.ShouZhi"));
		_dataArray.Add(new LegendaryBookSlotItem(136, 184, 185, null));
		_dataArray.Add(new LegendaryBookSlotItem(137, 186, 187, null));
		_dataArray.Add(new LegendaryBookSlotItem(138, 188, 189, null));
		_dataArray.Add(new LegendaryBookSlotItem(139, 190, 191, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LegendaryBookSlotItem>(140);
		CreateItems0();
		CreateItems1();
		CreateItems2();
	}
}
