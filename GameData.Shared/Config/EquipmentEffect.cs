using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Domains.Character;

namespace Config;

[Serializable]
public class EquipmentEffect : ConfigData<EquipmentEffectItem, short>
{
	public static class DefKey
	{
		public const short MadeByAdoptiveFather = 54;

		public const short ZhujianTest = 55;
	}

	public static class DefValue
	{
		public static EquipmentEffectItem MadeByAdoptiveFather => Instance[(short)54];

		public static EquipmentEffectItem ZhujianTest => Instance[(short)55];
	}

	public static EquipmentEffect Instance = new EquipmentEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "EffectClassName" };

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
		_dataArray.Add(new EquipmentEffectItem(0, 0, 1, special: false, isTotalPercent: false, 1, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoZhang"));
		_dataArray.Add(new EquipmentEffectItem(1, 2, 1, special: false, isTotalPercent: false, 3, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoJian"));
		_dataArray.Add(new EquipmentEffectItem(2, 4, 1, special: false, isTotalPercent: false, 5, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoDao"));
		_dataArray.Add(new EquipmentEffectItem(3, 6, 1, special: false, isTotalPercent: false, 7, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoDu"));
		_dataArray.Add(new EquipmentEffectItem(4, 8, 1, special: false, isTotalPercent: false, 9, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoChangBing"));
		_dataArray.Add(new EquipmentEffectItem(5, 10, 1, special: false, isTotalPercent: false, 11, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoRuanBing"));
		_dataArray.Add(new EquipmentEffectItem(6, 12, 1, special: false, isTotalPercent: false, 13, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoAnQi"));
		_dataArray.Add(new EquipmentEffectItem(7, 14, 1, special: false, isTotalPercent: false, 15, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoQiMen"));
		_dataArray.Add(new EquipmentEffectItem(8, 16, 1, special: false, isTotalPercent: false, 17, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceAvoid.PoMoYin"));
		_dataArray.Add(new EquipmentEffectItem(9, 18, 1, special: false, isTotalPercent: false, 19, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReducePenetrateResist.PoJin"));
		_dataArray.Add(new EquipmentEffectItem(10, 20, 1, special: false, isTotalPercent: false, 21, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReducePenetrateResist.PoMu"));
		_dataArray.Add(new EquipmentEffectItem(11, 22, 1, special: false, isTotalPercent: false, 23, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReducePenetrateResist.PoYu"));
		_dataArray.Add(new EquipmentEffectItem(12, 24, 1, special: false, isTotalPercent: false, 25, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReducePenetrateResist.PoJin2"));
		_dataArray.Add(new EquipmentEffectItem(13, 26, 1, special: false, isTotalPercent: false, 27, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 20, 0, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(14, 28, 1, special: false, isTotalPercent: false, 29, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 20, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(15, 30, 1, special: false, isTotalPercent: false, 31, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 50, -50, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(16, 32, 1, special: false, isTotalPercent: false, 33, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), -50, 50, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(17, 34, 1, special: false, isTotalPercent: false, 35, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.DuCi"));
		_dataArray.Add(new EquipmentEffectItem(18, 36, 1, special: false, isTotalPercent: false, 37, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, -20, null));
		_dataArray.Add(new EquipmentEffectItem(19, 38, 1, special: false, isTotalPercent: false, 39, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.BreakAddInjury.XueSha"));
		_dataArray.Add(new EquipmentEffectItem(20, 40, 1, special: false, isTotalPercent: false, 41, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.BreakAddInjury.QiSha"));
		_dataArray.Add(new EquipmentEffectItem(21, 42, 1, special: false, isTotalPercent: false, 43, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceBounceDamage.HuaXue"));
		_dataArray.Add(new EquipmentEffectItem(22, 44, 1, special: false, isTotalPercent: false, 45, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Weapon.ReduceBounceDamage.HuaQi"));
		_dataArray.Add(new EquipmentEffectItem(23, 46, 2, special: false, isTotalPercent: false, 47, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiZhang"));
		_dataArray.Add(new EquipmentEffectItem(24, 48, 2, special: false, isTotalPercent: false, 49, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiJian"));
		_dataArray.Add(new EquipmentEffectItem(25, 50, 2, special: false, isTotalPercent: false, 51, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiDao"));
		_dataArray.Add(new EquipmentEffectItem(26, 52, 2, special: false, isTotalPercent: false, 53, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiDu"));
		_dataArray.Add(new EquipmentEffectItem(27, 54, 2, special: false, isTotalPercent: false, 55, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiChangBing"));
		_dataArray.Add(new EquipmentEffectItem(28, 56, 2, special: false, isTotalPercent: false, 57, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiRuanBing"));
		_dataArray.Add(new EquipmentEffectItem(29, 58, 2, special: false, isTotalPercent: false, 59, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiAnQi"));
		_dataArray.Add(new EquipmentEffectItem(30, 60, 2, special: false, isTotalPercent: false, 61, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiQiMen"));
		_dataArray.Add(new EquipmentEffectItem(31, 62, 2, special: false, isTotalPercent: false, 63, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReduceHit.PiMoYin"));
		_dataArray.Add(new EquipmentEffectItem(32, 64, 2, special: false, isTotalPercent: false, 65, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReducePenetrate.PiJin"));
		_dataArray.Add(new EquipmentEffectItem(33, 66, 2, special: false, isTotalPercent: false, 67, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReducePenetrate.PiMu"));
		_dataArray.Add(new EquipmentEffectItem(34, 68, 2, special: false, isTotalPercent: false, 69, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReducePenetrate.PiYu"));
		_dataArray.Add(new EquipmentEffectItem(35, 70, 2, special: false, isTotalPercent: false, 71, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.ReducePenetrate.PiJin2"));
		_dataArray.Add(new EquipmentEffectItem(36, 72, 2, special: false, isTotalPercent: false, 73, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 20, 0, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(37, 74, 2, special: false, isTotalPercent: false, 75, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 20, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(38, 76, 2, special: false, isTotalPercent: false, 77, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 50, -50, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(39, 78, 2, special: false, isTotalPercent: false, 79, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), -50, 50, 0, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(40, 34, 2, special: false, isTotalPercent: false, 80, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.DuCi"));
		_dataArray.Add(new EquipmentEffectItem(41, 36, 2, special: false, isTotalPercent: false, 81, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, -20, null));
		_dataArray.Add(new EquipmentEffectItem(42, 82, 2, special: false, isTotalPercent: false, 83, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.BreakReduceInjury.HuXue"));
		_dataArray.Add(new EquipmentEffectItem(43, 84, 2, special: false, isTotalPercent: false, 85, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.BreakReduceInjury.HuQi"));
		_dataArray.Add(new EquipmentEffectItem(44, 86, 2, special: false, isTotalPercent: false, 87, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.AddBounceDamage.ZhenXue"));
		_dataArray.Add(new EquipmentEffectItem(45, 88, 2, special: false, isTotalPercent: false, 89, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.Armor.AddBounceDamage.ZhenQi"));
		_dataArray.Add(new EquipmentEffectItem(46, 90, 0, special: false, isTotalPercent: false, 91, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 20, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(47, 92, 0, special: false, isTotalPercent: false, 93, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, -20, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(48, 94, 0, special: false, isTotalPercent: false, 95, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 15, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(49, 96, 0, special: false, isTotalPercent: false, 97, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 30, 0, null));
		_dataArray.Add(new EquipmentEffectItem(50, 98, 0, special: false, isTotalPercent: false, 99, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 50, 50, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(51, 100, 0, special: false, isTotalPercent: false, 101, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, -50, -50, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(52, 102, 0, special: false, isTotalPercent: false, 103, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 40, -75, 0, null));
		_dataArray.Add(new EquipmentEffectItem(53, 104, 0, special: false, isTotalPercent: false, 105, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, -40, 75, 0, null));
		_dataArray.Add(new EquipmentEffectItem(54, 106, 1, special: true, isTotalPercent: false, 107, new short[4] { 20, 20, 20, 0 }, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 100, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(55, 108, 1, special: true, isTotalPercent: false, 109, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, -50, -50, 0, 100, null));
		_dataArray.Add(new EquipmentEffectItem(56, 110, 2, special: true, isTotalPercent: true, 111, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.YanXiaShenZhu"));
		_dataArray.Add(new EquipmentEffectItem(57, 112, 1, special: true, isTotalPercent: true, 113, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.ShanHeShenJie"));
		_dataArray.Add(new EquipmentEffectItem(58, 114, 0, special: true, isTotalPercent: true, 115, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.GuiYanLingZhuo"));
		_dataArray.Add(new EquipmentEffectItem(59, 116, 1, special: true, isTotalPercent: true, 117, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.XuanHuShenJie"));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new EquipmentEffectItem(60, 118, 1, special: true, isTotalPercent: true, 119, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, 0, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.XianYuanShenJie"));
		_dataArray.Add(new EquipmentEffectItem(61, 120, 0, special: true, isTotalPercent: true, 121, new short[4], new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 0), new OuterAndInnerShorts(0, 0), 0, -80, 100, 0, 0, 0, 0, null));
		_dataArray.Add(new EquipmentEffectItem(62, 122, 2, special: true, isTotalPercent: true, 123, new short[4], new HitOrAvoidShorts(50, 50, 50, 50), new OuterAndInnerShorts(50, 50), new OuterAndInnerShorts(25, 25), 100, 100, 0, 0, 0, 0, 0, "EquipmentEffect.RawCreate.HunYuanShenZhu"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<EquipmentEffectItem>(63);
		CreateItems0();
		CreateItems1();
	}
}
