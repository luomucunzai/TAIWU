using System;
using System.Collections.Generic;
using Config.Common;
using Config.ConfigCells.Character;
using GameData.Domains.Character;

namespace Config;

[Serializable]
public class Armor : ConfigData<ArmorItem, short>
{
	public static Armor Instance = new Armor();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"ItemSubType", "GroupId", "ResourceType", "MakeItemSubType", "EquipmentEffectId", "RequiredCharacterProperties", "RelatedWeapon", "TemplateId", "Name", "Grade",
		"Icon", "Desc", "MaxDurability", "BaseWeight", "BaseHappinessChange", "DropRate", "EquipmentType", "BaseEquipmentAttack", "BaseEquipmentDefense", "SkeletonSlotAndAttachment"
	};

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
		_dataArray.Add(new ArmorItem(0, 0, 1, 100, 0, 0, "icon_Armor_toutuotiegu", 1, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 210, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 118, 1, -1, 625, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_131" }, 100));
		_dataArray.Add(new ArmorItem(1, 2, 1, 100, 1, 0, "icon_Armor_toutuotiegu", 3, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 275, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 118, 1, -1, 700, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_131" }, 100));
		_dataArray.Add(new ArmorItem(2, 4, 1, 100, 2, 0, "icon_Armor_toutuotiegu", 5, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 345, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 118, 1, -1, 790, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_131" }, 100));
		_dataArray.Add(new ArmorItem(3, 6, 1, 100, 3, 0, "icon_Armor_jiangjunwuyoudou", 7, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 410, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 118, 1, -1, 880, 880, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_135" }, 100));
		_dataArray.Add(new ArmorItem(4, 8, 1, 100, 4, 0, "icon_Armor_jiangjunwuyoudou", 9, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 480, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 118, 1, -1, 975, 975, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_135" }, 100));
		_dataArray.Add(new ArmorItem(5, 10, 1, 100, 5, 0, "icon_Armor_jiangjunwuyoudou", 11, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 118, 1, -1, 1070, 1070, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_135" }, 100));
		_dataArray.Add(new ArmorItem(6, 12, 1, 100, 6, 0, "icon_Armor_tiewolong", 13, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 615, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 118, 1, -1, 1180, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_139" }, 100));
		_dataArray.Add(new ArmorItem(7, 14, 1, 100, 7, 0, "icon_Armor_daaonilindou", 15, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 680, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 118, 1, -1, 1285, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_139" }, 100));
		_dataArray.Add(new ArmorItem(8, 16, 1, 100, 8, 0, "icon_Armor_xuantieli", 17, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 750, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 118, 1, -1, 1400, 1400, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 0), new OuterAndInnerShorts(35, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_139" }, 100));
		_dataArray.Add(new ArmorItem(9, 18, 1, 100, 0, 9, "icon_Armor_tongdou", 19, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 180, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 125, 1, -1, 490, 730, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_131" }, 100));
		_dataArray.Add(new ArmorItem(10, 20, 1, 100, 1, 9, "icon_Armor_tongdou", 21, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 230, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 125, 1, -1, 555, 815, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_131" }, 100));
		_dataArray.Add(new ArmorItem(11, 22, 1, 100, 2, 9, "icon_Armor_tongdou", 23, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 285, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 125, 1, -1, 625, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_131" }, 100));
		_dataArray.Add(new ArmorItem(12, 24, 1, 100, 3, 9, "icon_Armor_baishejindian", 25, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 335, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 125, 1, -1, 690, 990, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 50), new OuterAndInnerShorts(10, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_135" }, 100));
		_dataArray.Add(new ArmorItem(13, 26, 1, 100, 4, 9, "icon_Armor_baishejindian", 27, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 390, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 125, 1, -1, 770, 1090, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(10, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_135" }, 100));
		_dataArray.Add(new ArmorItem(14, 28, 1, 100, 5, 9, "icon_Armor_baishejindian", 29, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 440, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 125, 1, -1, 840, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 70), new OuterAndInnerShorts(15, 15), -1, new List<string> { "headwear/headwear", "headwear/headwear_135" }, 100));
		_dataArray.Add(new ArmorItem(15, 30, 1, 100, 6, 9, "icon_Armor_jinjingshouliandou", 31, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 495, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 125, 1, -1, 925, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), -1, new List<string> { "headwear/headwear", "headwear/headwear_139" }, 100));
		_dataArray.Add(new ArmorItem(16, 32, 1, 100, 7, 9, "icon_Armor_tunxiabaodian", 33, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 125, 1, -1, 1005, 1385, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 90), new OuterAndInnerShorts(20, 20), -1, new List<string> { "headwear/headwear", "headwear/headwear_139" }, 100));
		_dataArray.Add(new ArmorItem(17, 34, 1, 100, 8, 9, "icon_Armor_jiutoujiao", 35, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 600, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 125, 1, -1, 1100, 1500, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 100), new OuterAndInnerShorts(25, 25), -1, new List<string> { "headwear/headwear", "headwear/headwear_139" }, 100));
		_dataArray.Add(new ArmorItem(18, 36, 1, 100, 0, 18, "icon_Armor_baimuzan", 37, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 119, 1, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_211" }, 100));
		_dataArray.Add(new ArmorItem(19, 38, 1, 100, 1, 18, "icon_Armor_baimuzan", 39, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 100, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 119, 1, -1, 245, 635, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 15), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_211" }, 100));
		_dataArray.Add(new ArmorItem(20, 40, 1, 100, 2, 18, "icon_Armor_baimuzan", 41, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 130, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 119, 1, -1, 285, 705, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_211" }, 100));
		_dataArray.Add(new ArmorItem(21, 42, 1, 100, 3, 18, "icon_Armor_pomoyuanyang", 43, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 160, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 119, 1, -1, 325, 775, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 25), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_215" }, 100));
		_dataArray.Add(new ArmorItem(22, 44, 1, 100, 4, 18, "icon_Armor_pomoyuanyang", 45, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 119, 1, -1, 370, 850, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 30), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_215" }, 100));
		_dataArray.Add(new ArmorItem(23, 46, 1, 100, 5, 18, "icon_Armor_pomoyuanyang", 47, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 119, 1, -1, 410, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 35), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_215" }, 100));
		_dataArray.Add(new ArmorItem(24, 48, 1, 100, 6, 18, "icon_Armor_fulongyinxiangzan", 49, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 119, 1, -1, 460, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 40), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_219" }, 100));
		_dataArray.Add(new ArmorItem(25, 50, 1, 100, 7, 18, "icon_Armor_huanglongbaoguan", 51, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 119, 1, -1, 505, 1075, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 45), new OuterAndInnerShorts(25, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_219" }, 100));
		_dataArray.Add(new ArmorItem(26, 52, 1, 100, 8, 18, "icon_Armor_zhaochenshu", 53, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 310, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 119, 1, -1, 560, 1160, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 50), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_219" }, 100));
		_dataArray.Add(new ArmorItem(27, 54, 1, 100, 0, 27, "icon_Armor_qingzhuzan", 55, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 45, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 126, 1, -1, 265, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_231" }, 100));
		_dataArray.Add(new ArmorItem(28, 56, 1, 100, 1, 27, "icon_Armor_qingzhuzan", 57, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 126, 1, -1, 300, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_231" }, 100));
		_dataArray.Add(new ArmorItem(29, 58, 1, 100, 2, 27, "icon_Armor_qingzhuzan", 59, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 95, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 126, 1, -1, 335, 615, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 40), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_231" }, 100));
		_dataArray.Add(new ArmorItem(30, 60, 1, 100, 3, 27, "icon_Armor_qiushouzanbi", 61, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 120, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 126, 1, -1, 375, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 50), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_235" }, 100));
		_dataArray.Add(new ArmorItem(31, 62, 1, 100, 4, 27, "icon_Armor_qiushouzanbi", 63, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 126, 1, -1, 415, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 60), new OuterAndInnerShorts(0, 15), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_235" }, 100));
		_dataArray.Add(new ArmorItem(32, 64, 1, 100, 5, 27, "icon_Armor_qiushouzanbi", 65, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 165, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 126, 1, -1, 460, 800, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 70), new OuterAndInnerShorts(0, 20), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_235" }, 100));
		_dataArray.Add(new ArmorItem(33, 66, 1, 100, 6, 27, "icon_Armor_wujikou", 67, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 126, 1, -1, 505, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 80), new OuterAndInnerShorts(0, 20), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_239" }, 100));
		_dataArray.Add(new ArmorItem(34, 68, 1, 100, 7, 27, "icon_Armor_shenmufazan", 69, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 215, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 126, 1, -1, 550, 930, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 90), new OuterAndInnerShorts(0, 25), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_239" }, 100));
		_dataArray.Add(new ArmorItem(35, 70, 1, 100, 8, 27, "icon_Armor_ziyuanyin", 71, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 240, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 126, 1, -1, 600, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 100), new OuterAndInnerShorts(0, 30), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_239" }, 100));
		_dataArray.Add(new ArmorItem(36, 72, 1, 100, 0, 36, "icon_Armor_chize", 73, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 121, 1, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_311" }, 100));
		_dataArray.Add(new ArmorItem(37, 74, 1, 100, 1, 36, "icon_Armor_chize", 75, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 65, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 121, 1, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_311" }, 100));
		_dataArray.Add(new ArmorItem(38, 76, 1, 100, 2, 36, "icon_Armor_chize", 77, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 70, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 121, 1, -1, 190, 470, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_311" }, 100));
		_dataArray.Add(new ArmorItem(39, 78, 1, 100, 3, 36, "icon_Armor_bailujin", 79, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 75, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 121, 1, -1, 205, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_315" }, 100));
		_dataArray.Add(new ArmorItem(40, 80, 1, 100, 4, 36, "icon_Armor_bailujin", 81, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 121, 1, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_315" }, 100));
		_dataArray.Add(new ArmorItem(41, 82, 1, 100, 5, 36, "icon_Armor_bailujin", 83, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 121, 1, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_315" }, 100));
		_dataArray.Add(new ArmorItem(42, 84, 1, 100, 6, 36, "icon_Armor_ruyihunyuanjin", 85, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 121, 1, -1, 260, 620, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_319" }, 100));
		_dataArray.Add(new ArmorItem(43, 86, 1, 100, 7, 36, "icon_Armor_tianyanjintong", 87, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 121, 1, -1, 275, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_319" }, 100));
		_dataArray.Add(new ArmorItem(44, 88, 1, 100, 8, 36, "icon_Armor_chankebaoshu", 89, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 121, 1, -1, 300, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_319" }, 100));
		_dataArray.Add(new ArmorItem(45, 90, 1, 100, 0, 45, "icon_Armor_hupidou", 91, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 122, 1, -1, 180, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_331" }, 100));
		_dataArray.Add(new ArmorItem(46, 92, 1, 100, 1, 45, "icon_Armor_hupidou", 93, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 122, 1, -1, 195, 455, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_331" }, 100));
		_dataArray.Add(new ArmorItem(47, 94, 1, 100, 2, 45, "icon_Armor_hupidou", 95, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 122, 1, -1, 215, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_331" }, 100));
		_dataArray.Add(new ArmorItem(48, 96, 1, 100, 3, 45, "icon_Armor_baishoudou", 97, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 122, 1, -1, 235, 535, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_335" }, 100));
		_dataArray.Add(new ArmorItem(49, 98, 1, 100, 4, 45, "icon_Armor_baishoudou", 99, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 122, 1, -1, 255, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_335" }, 100));
		_dataArray.Add(new ArmorItem(50, 100, 1, 100, 5, 45, "icon_Armor_baishoudou", 101, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 125, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 122, 1, -1, 270, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_335" }, 100));
		_dataArray.Add(new ArmorItem(51, 102, 1, 100, 6, 45, "icon_Armor_jiufengzhuling", 103, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 122, 1, -1, 295, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_339" }, 100));
		_dataArray.Add(new ArmorItem(52, 104, 1, 100, 7, 45, "icon_Armor_yanwangmian", 105, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 135, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 122, 1, -1, 315, 695, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_339" }, 100));
		_dataArray.Add(new ArmorItem(53, 106, 1, 100, 8, 45, "icon_Armor_daluotianxianguan", 107, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 122, 1, -1, 340, 740, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_339" }, 100));
		_dataArray.Add(new ArmorItem(54, 108, 1, 100, 0, 54, "icon_Armor_zhanmao", 109, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 123, 1, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_311" }, 100));
		_dataArray.Add(new ArmorItem(55, 110, 1, 100, 1, 54, "icon_Armor_zhanmao", 111, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 123, 1, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_311" }, 100));
		_dataArray.Add(new ArmorItem(56, 112, 1, 100, 2, 54, "icon_Armor_zhanmao", 113, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 123, 1, -1, 205, 485, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_311" }, 100));
		_dataArray.Add(new ArmorItem(57, 114, 1, 100, 3, 54, "icon_Armor_baguajin", 115, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 123, 1, -1, 220, 520, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_315" }, 100));
		_dataArray.Add(new ArmorItem(58, 116, 1, 100, 4, 54, "icon_Armor_baguajin", 117, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 123, 1, -1, 240, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_315" }, 100));
		_dataArray.Add(new ArmorItem(59, 118, 1, 100, 5, 54, "icon_Armor_baguajin", 119, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 123, 1, -1, 255, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_315" }, 100));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new ArmorItem(60, 120, 1, 100, 6, 54, "icon_Armor_chizhafengleiguan", 121, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 123, 1, -1, 280, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_319" }, 100));
		_dataArray.Add(new ArmorItem(61, 122, 1, 100, 7, 54, "icon_Armor_chongtianguan", 123, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 123, 1, -1, 295, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_319" }, 100));
		_dataArray.Add(new ArmorItem(62, 124, 1, 100, 8, 54, "icon_Armor_jinxiaguan", 125, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 123, 1, -1, 320, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_hel/equip_hel", "equip_hel/equip_hel_319" }, 100));
		_dataArray.Add(new ArmorItem(63, 126, 1, 100, 0, 63, "icon_Armor_wurenbian", 127, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 124, 1, -1, 180, 540, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_331" }, 100));
		_dataArray.Add(new ArmorItem(64, 128, 1, 100, 1, 63, "icon_Armor_wurenbian", 129, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 124, 1, -1, 200, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_331" }, 100));
		_dataArray.Add(new ArmorItem(65, 130, 1, 100, 2, 63, "icon_Armor_wurenbian", 131, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 124, 1, -1, 225, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_331" }, 100));
		_dataArray.Add(new ArmorItem(66, 132, 1, 100, 3, 63, "icon_Armor_fuhuguan", 133, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 124, 1, -1, 250, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_335" }, 100));
		_dataArray.Add(new ArmorItem(67, 134, 1, 100, 4, 63, "icon_Armor_fuhuguan", 135, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 124, 1, -1, 270, 750, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_335" }, 100));
		_dataArray.Add(new ArmorItem(68, 136, 1, 100, 5, 63, "icon_Armor_fuhuguan", 137, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 124, 1, -1, 300, 810, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_335" }, 100));
		_dataArray.Add(new ArmorItem(69, 138, 1, 100, 6, 63, "icon_Armor_zhuangjinjiulongguan", 139, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 124, 1, -1, 325, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_339" }, 100));
		_dataArray.Add(new ArmorItem(70, 140, 1, 100, 7, 63, "icon_Armor_jiuxiaojin", 141, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 170, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 124, 1, -1, 350, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(55, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_339" }, 100));
		_dataArray.Add(new ArmorItem(71, 142, 1, 100, 8, 63, "icon_Armor_xuanyuanmian", 143, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 124, 1, -1, 380, 980, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(35, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_339" }, 100));
		_dataArray.Add(new ArmorItem(72, 144, 1, 100, 0, 72, "icon_Armor_huangze", 145, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 129, 1, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_311" }, 100));
		_dataArray.Add(new ArmorItem(73, 146, 1, 100, 1, 72, "icon_Armor_huangze", 147, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 129, 1, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_311" }, 100));
		_dataArray.Add(new ArmorItem(74, 148, 1, 100, 2, 72, "icon_Armor_huangze", 149, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 129, 1, -1, 195, 475, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_311" }, 100));
		_dataArray.Add(new ArmorItem(75, 150, 1, 100, 3, 72, "icon_Armor_jiuliangjin", 151, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 129, 1, -1, 210, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_315" }, 100));
		_dataArray.Add(new ArmorItem(76, 152, 1, 100, 4, 72, "icon_Armor_jiuliangjin", 153, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 129, 1, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_315" }, 100));
		_dataArray.Add(new ArmorItem(77, 154, 1, 100, 5, 72, "icon_Armor_jiuliangjin", 155, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 129, 1, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_315" }, 100));
		_dataArray.Add(new ArmorItem(78, 156, 1, 100, 6, 72, "icon_Armor_tiansheguan", 157, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 129, 1, -1, 250, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_319" }, 100));
		_dataArray.Add(new ArmorItem(79, 158, 1, 100, 7, 72, "icon_Armor_taijizhenyuanguan", 159, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 129, 1, -1, 265, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_319" }, 100));
		_dataArray.Add(new ArmorItem(80, 160, 1, 100, 8, 72, "icon_Armor_hundunxuanjin", 161, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 129, 1, -1, 280, 680, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_319" }, 100));
		_dataArray.Add(new ArmorItem(81, 162, 1, 100, 0, 81, "icon_Armor_malongguan", 163, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 130, 1, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_351" }, 100));
		_dataArray.Add(new ArmorItem(82, 164, 1, 100, 1, 81, "icon_Armor_malongguan", 165, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 130, 1, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_351" }, 100));
		_dataArray.Add(new ArmorItem(83, 166, 1, 100, 2, 81, "icon_Armor_malongguan", 167, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 130, 1, -1, 180, 460, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_351" }, 100));
		_dataArray.Add(new ArmorItem(84, 168, 1, 100, 3, 81, "icon_Armor_pilumao", 169, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 130, 1, -1, 195, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_355" }, 100));
		_dataArray.Add(new ArmorItem(85, 170, 1, 100, 4, 81, "icon_Armor_pilumao", 171, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 130, 1, -1, 210, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_355" }, 100));
		_dataArray.Add(new ArmorItem(86, 172, 1, 100, 5, 81, "icon_Armor_pilumao", 173, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 130, 1, -1, 220, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_355" }, 100));
		_dataArray.Add(new ArmorItem(87, 174, 1, 100, 6, 81, "icon_Armor_liuyabaoxiangguan", 175, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 130, 1, -1, 235, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_359" }, 100));
		_dataArray.Add(new ArmorItem(88, 176, 1, 100, 7, 81, "icon_Armor_huohuanbingluo", 177, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 130, 1, -1, 245, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_359" }, 100));
		_dataArray.Add(new ArmorItem(89, 178, 1, 100, 8, 81, "icon_Armor_tiancanshu", 179, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 130, 1, -1, 260, 660, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_359" }, 100));
		_dataArray.Add(new ArmorItem(90, 180, 1, 100, 0, 90, "icon_Armor_shushengjin", 181, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 131, 1, -1, 170, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 10), -1, new List<string> { "hair/hair_hat", "hair/hair_321" }, 100));
		_dataArray.Add(new ArmorItem(91, 182, 1, 100, 1, 90, "icon_Armor_shushengjin", 183, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 131, 1, -1, 190, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 25), new OuterAndInnerShorts(0, 10), -1, new List<string> { "hair/hair_hat", "hair/hair_321" }, 100));
		_dataArray.Add(new ArmorItem(92, 184, 1, 100, 2, 90, "icon_Armor_shushengjin", 185, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 131, 1, -1, 210, 630, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 15), -1, new List<string> { "hair/hair_hat", "hair/hair_321" }, 100));
		_dataArray.Add(new ArmorItem(93, 186, 1, 100, 3, 90, "icon_Armor_sishibaihuashu", 187, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 131, 1, -1, 235, 685, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 35), new OuterAndInnerShorts(0, 20), -1, new List<string> { "hair/hair_hat", "hair/hair_325" }, 100));
		_dataArray.Add(new ArmorItem(94, 188, 1, 100, 4, 90, "icon_Armor_sishibaihuashu", 189, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 131, 1, -1, 255, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 20), -1, new List<string> { "hair/hair_hat", "hair/hair_325" }, 100));
		_dataArray.Add(new ArmorItem(95, 190, 1, 100, 5, 90, "icon_Armor_sishibaihuashu", 191, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 131, 1, -1, 280, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 45), new OuterAndInnerShorts(0, 25), -1, new List<string> { "hair/hair_hat", "hair/hair_325" }, 100));
		_dataArray.Add(new ArmorItem(96, 192, 1, 100, 6, 90, "icon_Armor_xuantianguan", 193, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 131, 1, -1, 305, 845, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 30), -1, new List<string> { "hair/hair_hat", "hair/hair_329" }, 100));
		_dataArray.Add(new ArmorItem(97, 194, 1, 100, 7, 90, "icon_Armor_jiusexieying", 195, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 131, 1, -1, 335, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 55), new OuterAndInnerShorts(0, 30), -1, new List<string> { "hair/hair_hat", "hair/hair_329" }, 100));
		_dataArray.Add(new ArmorItem(98, 196, 1, 100, 8, 90, "icon_Armor_mingmingjin", 197, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 131, 1, -1, 360, 960, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 35), -1, new List<string> { "hair/hair_hat", "hair/hair_329" }, 100));
		_dataArray.Add(new ArmorItem(99, 198, 1, 100, 0, 99, "icon_Armor_wansujin", 199, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 128, 1, -1, 145, 385, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_331" }, 100));
		_dataArray.Add(new ArmorItem(100, 200, 1, 100, 1, 99, "icon_Armor_wansujin", 201, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 128, 1, -1, 155, 415, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_331" }, 100));
		_dataArray.Add(new ArmorItem(101, 202, 1, 100, 2, 99, "icon_Armor_wansujin", 203, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 128, 1, -1, 170, 450, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_331" }, 100));
		_dataArray.Add(new ArmorItem(102, 204, 1, 100, 3, 99, "icon_Armor_baxianguan", 205, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 128, 1, -1, 180, 480, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_335" }, 100));
		_dataArray.Add(new ArmorItem(103, 206, 1, 100, 4, 99, "icon_Armor_baxianguan", 207, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 128, 1, -1, 190, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_335" }, 100));
		_dataArray.Add(new ArmorItem(104, 208, 1, 100, 5, 99, "icon_Armor_baxianguan", 209, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 128, 1, -1, 205, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_335" }, 100));
		_dataArray.Add(new ArmorItem(105, 210, 1, 100, 6, 99, "icon_Armor_qibaoyinhezhi", 211, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 128, 1, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_339" }, 100));
		_dataArray.Add(new ArmorItem(106, 212, 1, 100, 7, 99, "icon_Armor_longxubaoshouguan", 213, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 128, 1, -1, 230, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_339" }, 100));
		_dataArray.Add(new ArmorItem(107, 214, 1, 100, 8, 99, "icon_Armor_taishicangshu", 215, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 128, 1, -1, 240, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "hair/hair_hat", "hair/hair_339" }, 100));
		_dataArray.Add(new ArmorItem(108, 216, 1, 100, 0, 108, "icon_Armor_heishizan", 217, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 120, 1, -1, 480, 480, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 5), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_431" }, 100));
		_dataArray.Add(new ArmorItem(109, 218, 1, 100, 1, 108, "icon_Armor_heishizan", 219, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 175, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 120, 1, -1, 535, 535, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_431" }, 100));
		_dataArray.Add(new ArmorItem(110, 220, 1, 100, 2, 108, "icon_Armor_heishizan", 221, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 210, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 120, 1, -1, 595, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 15), new OuterAndInnerShorts(10, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_431" }, 100));
		_dataArray.Add(new ArmorItem(111, 222, 1, 100, 3, 108, "icon_Armor_tongxiazan", 223, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 245, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 120, 1, -1, 655, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_435" }, 100));
		_dataArray.Add(new ArmorItem(112, 224, 1, 100, 4, 108, "icon_Armor_tongxiazan", 225, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 120, 1, -1, 720, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 25), new OuterAndInnerShorts(15, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_435" }, 100));
		_dataArray.Add(new ArmorItem(113, 226, 1, 100, 5, 108, "icon_Armor_tongxiazan", 227, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 315, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 120, 1, -1, 780, 780, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 30), new OuterAndInnerShorts(20, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_435" }, 100));
		_dataArray.Add(new ArmorItem(114, 228, 1, 100, 6, 108, "icon_Armor_chihuzan", 229, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 350, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 120, 1, -1, 855, 855, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 35), new OuterAndInnerShorts(20, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_439" }, 100));
		_dataArray.Add(new ArmorItem(115, 230, 1, 100, 7, 108, "icon_Armor_biyanxianyan", 231, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 385, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 120, 1, -1, 920, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 40), new OuterAndInnerShorts(25, 15), -1, new List<string> { "headwear/headwear", "headwear/headwear_439" }, 100));
		_dataArray.Add(new ArmorItem(116, 232, 1, 100, 8, 108, "icon_Armor_chiyoujiao", 233, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 420, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 120, 1, -1, 1000, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 45), new OuterAndInnerShorts(30, 20), -1, new List<string> { "headwear/headwear", "headwear/headwear_439" }, 100));
		_dataArray.Add(new ArmorItem(117, 234, 1, 100, 0, 117, "icon_Armor_jingyuzan", 235, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 127, 1, -1, 515, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(5, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_411" }, 100));
		_dataArray.Add(new ArmorItem(118, 236, 1, 100, 1, 117, "icon_Armor_jingyuzan", 237, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 127, 1, -1, 570, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "headwear/headwear", "headwear/headwear_411" }, 100));
		_dataArray.Add(new ArmorItem(119, 238, 1, 100, 2, 117, "icon_Armor_jingyuzan", 239, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 127, 1, -1, 630, 490, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 60), new OuterAndInnerShorts(0, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_411" }, 100));
	}

	private void CreateItems2()
	{
		_dataArray.Add(new ArmorItem(120, 240, 1, 100, 3, 117, "icon_Armor_liushuangguan", 241, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 165, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 127, 1, -1, 690, 540, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 70), new OuterAndInnerShorts(0, 10), -1, new List<string> { "headwear/headwear", "headwear/headwear_415" }, 100));
		_dataArray.Add(new ArmorItem(121, 242, 1, 100, 4, 117, "icon_Armor_liushuangguan", 243, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 195, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 127, 1, -1, 750, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 80), new OuterAndInnerShorts(0, 15), -1, new List<string> { "headwear/headwear", "headwear/headwear_415" }, 100));
		_dataArray.Add(new ArmorItem(122, 244, 1, 100, 5, 117, "icon_Armor_liushuangguan", 245, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 127, 1, -1, 815, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 90), new OuterAndInnerShorts(10, 20), -1, new List<string> { "headwear/headwear", "headwear/headwear_415" }, 100));
		_dataArray.Add(new ArmorItem(123, 246, 1, 100, 6, 117, "icon_Armor_jiuhuazhuxinzan", 247, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 127, 1, -1, 880, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 100), new OuterAndInnerShorts(10, 20), -1, new List<string> { "headwear/headwear", "headwear/headwear_419" }, 100));
		_dataArray.Add(new ArmorItem(124, 248, 1, 100, 7, 117, "icon_Armor_lirenzan", 249, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 127, 1, -1, 950, 760, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 110), new OuterAndInnerShorts(15, 25), -1, new List<string> { "headwear/headwear", "headwear/headwear_419" }, 100));
		_dataArray.Add(new ArmorItem(125, 250, 1, 100, 8, 117, "icon_Armor_kongmingguan", 251, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 305, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 127, 1, -1, 1020, 820, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 120), new OuterAndInnerShorts(20, 30), -1, new List<string> { "headwear/headwear", "headwear/headwear_419" }, 100));
		_dataArray.Add(new ArmorItem(126, 252, 1, 103, 0, 126, "icon_Armor_jiaoliao", 253, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 210, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 132, 5, -1, 625, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(10, 0), 381, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_111", "equip_shank_r", "equip_shank_r/equip_shank_r_111" }, 100));
		_dataArray.Add(new ArmorItem(127, 254, 1, 103, 1, 126, "icon_Armor_jiaoliao", 255, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 275, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 132, 5, -1, 700, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(10, 0), 382, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_111", "equip_shank_r", "equip_shank_r/equip_shank_r_111" }, 100));
		_dataArray.Add(new ArmorItem(128, 256, 1, 103, 2, 126, "icon_Armor_jiaoliao", 257, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 345, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 132, 5, -1, 790, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(15, 0), 383, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_111", "equip_shank_r", "equip_shank_r/equip_shank_r_111" }, 100));
		_dataArray.Add(new ArmorItem(129, 258, 1, 103, 3, 126, "icon_Armor_baoliantiexue", 259, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 410, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 132, 5, -1, 880, 880, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(20, 0), 384, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_115", "equip_shank_r", "equip_shank_r/equip_shank_r_115" }, 100));
		_dataArray.Add(new ArmorItem(130, 260, 1, 103, 4, 126, "icon_Armor_baoliantiexue", 261, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 480, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 132, 5, -1, 975, 975, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(20, 0), 385, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_115", "equip_shank_r", "equip_shank_r/equip_shank_r_115" }, 100));
		_dataArray.Add(new ArmorItem(131, 262, 1, 103, 5, 126, "icon_Armor_baoliantiexue", 263, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 132, 5, -1, 1070, 1070, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(25, 0), 386, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_115", "equip_shank_r", "equip_shank_r/equip_shank_r_115" }, 100));
		_dataArray.Add(new ArmorItem(132, 264, 1, 103, 6, 126, "icon_Armor_jingangzuhuan", 265, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 615, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 132, 5, -1, 1180, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(30, 0), 387, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_119", "equip_shank_r", "equip_shank_r/equip_shank_r_119" }, 100));
		_dataArray.Add(new ArmorItem(133, 266, 1, 103, 7, 126, "icon_Armor_longxiangbaoxue", 267, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 680, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 132, 5, -1, 1285, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 0), new OuterAndInnerShorts(30, 0), 388, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_119", "equip_shank_r", "equip_shank_r/equip_shank_r_119" }, 100));
		_dataArray.Add(new ArmorItem(134, 268, 1, 103, 8, 126, "icon_Armor_qiushensuo", 269, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 750, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 132, 5, -1, 1400, 1400, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 0), new OuterAndInnerShorts(35, 0), 389, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_119", "equip_shank_r", "equip_shank_r/equip_shank_r_119" }, 100));
		_dataArray.Add(new ArmorItem(135, 270, 1, 103, 0, 135, "icon_Armor_tongzuhuan", 271, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 180, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 139, 5, -1, 490, 730, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 20), new OuterAndInnerShorts(0, 0), 390, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_131", "equip_shank_r", "equip_shank_r/equip_shank_r_131" }, 100));
		_dataArray.Add(new ArmorItem(136, 272, 1, 103, 1, 135, "icon_Armor_tongzuhuan", 273, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 230, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 139, 5, -1, 555, 815, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 30), new OuterAndInnerShorts(0, 0), 391, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_131", "equip_shank_r", "equip_shank_r/equip_shank_r_131" }, 100));
		_dataArray.Add(new ArmorItem(137, 274, 1, 103, 2, 135, "icon_Armor_tongzuhuan", 275, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 285, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 139, 5, -1, 625, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(0, 0), 392, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_131", "equip_shank_r", "equip_shank_r/equip_shank_r_131" }, 100));
		_dataArray.Add(new ArmorItem(138, 276, 1, 103, 3, 135, "icon_Armor_tujinxiongtouxue", 277, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 335, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 139, 5, -1, 690, 990, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 50), new OuterAndInnerShorts(10, 10), 393, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_135", "equip_shank_r", "equip_shank_r/equip_shank_r_135" }, 100));
		_dataArray.Add(new ArmorItem(139, 278, 1, 103, 4, 135, "icon_Armor_tujinxiongtouxue", 279, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 390, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 139, 5, -1, 770, 1090, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(10, 10), 394, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_135", "equip_shank_r", "equip_shank_r/equip_shank_r_135" }, 100));
		_dataArray.Add(new ArmorItem(140, 280, 1, 103, 5, 135, "icon_Armor_tujinxiongtouxue", 281, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 440, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 139, 5, -1, 840, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 70), new OuterAndInnerShorts(15, 15), 395, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_135", "equip_shank_r", "equip_shank_r/equip_shank_r_135" }, 100));
		_dataArray.Add(new ArmorItem(141, 282, 1, 103, 6, 135, "icon_Armor_jiangmohumulun", 283, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 495, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 139, 5, -1, 925, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), 396, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_139", "equip_shank_r", "equip_shank_r/equip_shank_r_139" }, 100));
		_dataArray.Add(new ArmorItem(142, 284, 1, 103, 7, 135, "icon_Armor_shenquelv", 285, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 139, 5, -1, 1005, 1385, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 90), new OuterAndInnerShorts(20, 20), 397, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_139", "equip_shank_r", "equip_shank_r/equip_shank_r_139" }, 100));
		_dataArray.Add(new ArmorItem(143, 286, 1, 103, 8, 135, "icon_Armor_jinjiazu", 287, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 600, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 139, 5, -1, 1100, 1500, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 100), new OuterAndInnerShorts(25, 25), 398, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_139", "equip_shank_r", "equip_shank_r/equip_shank_r_139" }, 100));
		_dataArray.Add(new ArmorItem(144, 288, 1, 103, 0, 144, "icon_Armor_muji", 289, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 133, 5, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 10), new OuterAndInnerShorts(0, 0), 327, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_211", "equip_shank_r", "equip_shank_r/equip_shank_r_211" }, 100));
		_dataArray.Add(new ArmorItem(145, 290, 1, 103, 1, 144, "icon_Armor_muji", 291, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 100, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 133, 5, -1, 245, 635, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 15), new OuterAndInnerShorts(0, 0), 328, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_211", "equip_shank_r", "equip_shank_r/equip_shank_r_211" }, 100));
		_dataArray.Add(new ArmorItem(146, 292, 1, 103, 2, 144, "icon_Armor_muji", 293, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 130, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 133, 5, -1, 285, 705, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 20), new OuterAndInnerShorts(10, 0), 329, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_211", "equip_shank_r", "equip_shank_r/equip_shank_r_211" }, 100));
		_dataArray.Add(new ArmorItem(147, 294, 1, 103, 3, 144, "icon_Armor_xiegongchiji", 295, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 160, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 133, 5, -1, 325, 775, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 25), new OuterAndInnerShorts(10, 0), 330, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_215", "equip_shank_r", "equip_shank_r/equip_shank_r_215" }, 100));
		_dataArray.Add(new ArmorItem(148, 296, 1, 103, 4, 144, "icon_Armor_xiegongchiji", 297, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 133, 5, -1, 370, 850, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 30), new OuterAndInnerShorts(15, 0), 331, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_215", "equip_shank_r", "equip_shank_r/equip_shank_r_215" }, 100));
		_dataArray.Add(new ArmorItem(149, 298, 1, 103, 5, 144, "icon_Armor_xiegongchiji", 299, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 133, 5, -1, 410, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 35), new OuterAndInnerShorts(20, 0), 332, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_215", "equip_shank_r", "equip_shank_r/equip_shank_r_215" }, 100));
		_dataArray.Add(new ArmorItem(150, 300, 1, 103, 6, 144, "icon_Armor_yinyangshunnilv", 301, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 133, 5, -1, 460, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 40), new OuterAndInnerShorts(20, 0), 333, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_219", "equip_shank_r", "equip_shank_r/equip_shank_r_219" }, 100));
		_dataArray.Add(new ArmorItem(151, 302, 1, 103, 7, 144, "icon_Armor_juediwuhuangji", 303, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 133, 5, -1, 505, 1075, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 45), new OuterAndInnerShorts(25, 0), 334, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_219", "equip_shank_r", "equip_shank_r/equip_shank_r_219" }, 100));
		_dataArray.Add(new ArmorItem(152, 304, 1, 103, 8, 144, "icon_Armor_kuilongji", 305, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 310, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 133, 5, -1, 560, 1160, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 50), new OuterAndInnerShorts(30, 0), 335, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_219", "equip_shank_r", "equip_shank_r/equip_shank_r_219" }, 100));
		_dataArray.Add(new ArmorItem(153, 306, 1, 103, 0, 153, "icon_Armor_zhubangtui", 307, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 45, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 140, 5, -1, 265, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 20), new OuterAndInnerShorts(0, 0), 336, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_231", "equip_shank_r", "equip_shank_r/equip_shank_r_231" }, 100));
		_dataArray.Add(new ArmorItem(154, 308, 1, 103, 1, 153, "icon_Armor_zhubangtui", 309, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 140, 5, -1, 300, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 30), new OuterAndInnerShorts(0, 0), 337, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_231", "equip_shank_r", "equip_shank_r/equip_shank_r_231" }, 100));
		_dataArray.Add(new ArmorItem(155, 310, 1, 103, 2, 153, "icon_Armor_zhubangtui", 311, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 95, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 140, 5, -1, 335, 615, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 40), new OuterAndInnerShorts(0, 10), 338, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_231", "equip_shank_r", "equip_shank_r/equip_shank_r_231" }, 100));
		_dataArray.Add(new ArmorItem(156, 312, 1, 103, 3, 153, "icon_Armor_fenshuimuji", 313, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 120, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 140, 5, -1, 375, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 50), new OuterAndInnerShorts(0, 10), 339, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_235", "equip_shank_r", "equip_shank_r/equip_shank_r_235" }, 100));
		_dataArray.Add(new ArmorItem(157, 314, 1, 103, 4, 153, "icon_Armor_fenshuimuji", 315, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 140, 5, -1, 415, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 60), new OuterAndInnerShorts(0, 15), 340, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_235", "equip_shank_r", "equip_shank_r/equip_shank_r_235" }, 100));
		_dataArray.Add(new ArmorItem(158, 316, 1, 103, 5, 153, "icon_Armor_fenshuimuji", 317, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 165, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 140, 5, -1, 460, 800, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 70), new OuterAndInnerShorts(0, 20), 341, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_235", "equip_shank_r", "equip_shank_r/equip_shank_r_235" }, 100));
		_dataArray.Add(new ArmorItem(159, 318, 1, 103, 6, 153, "icon_Armor_jialanbaoxue", 319, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 140, 5, -1, 505, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 80), new OuterAndInnerShorts(0, 20), 342, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_239", "equip_shank_r", "equip_shank_r/equip_shank_r_239" }, 100));
		_dataArray.Add(new ArmorItem(160, 320, 1, 103, 7, 153, "icon_Armor_liurendunjiaji", 321, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 215, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 140, 5, -1, 550, 930, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 90), new OuterAndInnerShorts(0, 25), 343, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_239", "equip_shank_r", "equip_shank_r/equip_shank_r_239" }, 100));
		_dataArray.Add(new ArmorItem(161, 322, 1, 103, 8, 153, "icon_Armor_kurongyouta", 323, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 240, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 140, 5, -1, 600, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 100), new OuterAndInnerShorts(0, 30), 344, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_239", "equip_shank_r", "equip_shank_r/equip_shank_r_239" }, 100));
		_dataArray.Add(new ArmorItem(162, 324, 1, 103, 0, 162, "icon_Armor_sulv", 325, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 135, 5, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), 345, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(163, 326, 1, 103, 1, 162, "icon_Armor_sulv", 327, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 135, 5, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), 346, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(164, 328, 1, 103, 2, 162, "icon_Armor_sulv", 329, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 135, 5, -1, 180, 460, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), 347, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(165, 330, 1, 103, 3, 162, "icon_Armor_dianguanglv", 331, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 135, 5, -1, 195, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), 348, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(166, 332, 1, 103, 4, 162, "icon_Armor_dianguanglv", 333, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 135, 5, -1, 210, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), 349, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(167, 334, 1, 103, 5, 162, "icon_Armor_dianguanglv", 335, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 135, 5, -1, 220, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), 350, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(168, 336, 1, 103, 6, 162, "icon_Armor_jinghuawu", 337, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 135, 5, -1, 235, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), 351, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(169, 338, 1, 103, 7, 162, "icon_Armor_piaomiaoluowa", 339, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 135, 5, -1, 245, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), 352, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(170, 340, 1, 103, 8, 162, "icon_Armor_chankexue", 341, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 135, 5, -1, 260, 660, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), 353, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(171, 342, 1, 103, 0, 171, "icon_Armor_zhifengxue", 343, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 137, 5, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), 354, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(172, 344, 1, 103, 1, 171, "icon_Armor_zhifengxue", 345, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 65, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 137, 5, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), 355, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(173, 346, 1, 103, 2, 171, "icon_Armor_zhifengxue", 347, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 70, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 137, 5, -1, 190, 470, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), 356, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(174, 348, 1, 103, 3, 171, "icon_Armor_wuyangxue", 349, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 75, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 137, 5, -1, 205, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), 357, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(175, 350, 1, 103, 4, 171, "icon_Armor_wuyangxue", 351, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 137, 5, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), 358, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(176, 352, 1, 103, 5, 171, "icon_Armor_wuyangxue", 353, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 137, 5, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), 359, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(177, 354, 1, 103, 6, 171, "icon_Armor_guizhishenxue", 355, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 137, 5, -1, 260, 620, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), 360, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(178, 356, 1, 103, 7, 171, "icon_Armor_qilinzu", 357, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 137, 5, -1, 275, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), 361, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(179, 358, 1, 103, 8, 171, "icon_Armor_jingangdou", 359, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 137, 5, -1, 300, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), 362, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
	}

	private void CreateItems3()
	{
		_dataArray.Add(new ArmorItem(180, 360, 1, 103, 0, 180, "icon_Armor_zhanxue", 361, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 136, 5, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), 399, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(181, 362, 1, 103, 1, 180, "icon_Armor_zhanxue", 363, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 136, 5, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), 400, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(182, 364, 1, 103, 2, 180, "icon_Armor_zhanxue", 365, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 136, 5, -1, 205, 485, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), 401, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(183, 366, 1, 103, 3, 180, "icon_Armor_gunlongxue", 367, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 136, 5, -1, 220, 520, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), 402, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(184, 368, 1, 103, 4, 180, "icon_Armor_gunlongxue", 369, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 136, 5, -1, 240, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), 403, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(185, 370, 1, 103, 5, 180, "icon_Armor_gunlongxue", 371, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 136, 5, -1, 255, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), 404, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(186, 372, 1, 103, 6, 180, "icon_Armor_zhuri", 373, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 136, 5, -1, 280, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), 405, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(187, 374, 1, 103, 7, 180, "icon_Armor_zhijiudian", 375, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 136, 5, -1, 295, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), 406, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(188, 376, 1, 103, 8, 180, "icon_Armor_pidishenxing", 377, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 136, 5, -1, 320, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), 407, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(189, 378, 1, 103, 0, 189, "icon_Armor_cubuguozu", 379, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 138, 5, -1, 180, 540, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(10, 0), 417, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(190, 380, 1, 103, 1, 189, "icon_Armor_cubuguozu", 381, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 138, 5, -1, 200, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 0), new OuterAndInnerShorts(10, 0), 418, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(191, 382, 1, 103, 2, 189, "icon_Armor_cubuguozu", 383, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 138, 5, -1, 225, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(15, 0), 419, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(192, 384, 1, 103, 3, 189, "icon_Armor_shifanglv", 385, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 138, 5, -1, 250, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 0), new OuterAndInnerShorts(20, 0), 420, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(193, 386, 1, 103, 4, 189, "icon_Armor_shifanglv", 387, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 138, 5, -1, 270, 750, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(20, 0), 421, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(194, 388, 1, 103, 5, 189, "icon_Armor_shifanglv", 389, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 138, 5, -1, 300, 810, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 0), new OuterAndInnerShorts(25, 0), 422, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(195, 390, 1, 103, 6, 189, "icon_Armor_longjuxue", 391, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 138, 5, -1, 325, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(30, 0), 423, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(196, 392, 1, 103, 7, 189, "icon_Armor_tongtianxue", 393, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 170, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 138, 5, -1, 350, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(55, 0), new OuterAndInnerShorts(30, 0), 424, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(197, 394, 1, 103, 8, 189, "icon_Armor_wuzhuosuopo", 395, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 138, 5, -1, 380, 980, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(35, 0), 425, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(198, 396, 1, 103, 0, 198, "icon_Armor_penghaoxie", 397, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 143, 5, -1, 180, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), 408, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(199, 398, 1, 103, 1, 198, "icon_Armor_penghaoxie", 399, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 143, 5, -1, 195, 455, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), 409, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(200, 400, 1, 103, 2, 198, "icon_Armor_penghaoxie", 401, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 143, 5, -1, 215, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), 410, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_321", "equip_shank_r", "equip_shank_r/equip_shank_r_321" }, 100));
		_dataArray.Add(new ArmorItem(201, 402, 1, 103, 3, 198, "icon_Armor_suifuxie", 403, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 143, 5, -1, 235, 535, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), 411, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(202, 404, 1, 103, 4, 198, "icon_Armor_suifuxie", 405, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 143, 5, -1, 255, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), 412, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(203, 406, 1, 103, 5, 198, "icon_Armor_suifuxie", 407, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 125, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 143, 5, -1, 270, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), 413, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_325", "equip_shank_r", "equip_shank_r/equip_shank_r_325" }, 100));
		_dataArray.Add(new ArmorItem(204, 408, 1, 103, 6, 198, "icon_Armor_xiejiansizu", 409, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 143, 5, -1, 295, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), 414, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(205, 410, 1, 103, 7, 198, "icon_Armor_longshalv", 411, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 135, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 143, 5, -1, 315, 695, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), 415, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(206, 412, 1, 103, 8, 198, "icon_Armor_taichongshenxue", 413, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 143, 5, -1, 340, 740, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), 416, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_329", "equip_shank_r", "equip_shank_r/equip_shank_r_329" }, 100));
		_dataArray.Add(new ArmorItem(207, 414, 1, 103, 0, 207, "icon_Armor_zongmaxie", 415, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 142, 5, -1, 145, 385, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), 363, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(208, 416, 1, 103, 1, 207, "icon_Armor_zongmaxie", 417, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 142, 5, -1, 155, 415, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), 364, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(209, 418, 1, 103, 2, 207, "icon_Armor_zongmaxie", 419, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 142, 5, -1, 170, 450, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), 365, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(210, 420, 1, 103, 3, 207, "icon_Armor_wuyinxue", 421, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 142, 5, -1, 180, 480, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), 366, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(211, 422, 1, 103, 4, 207, "icon_Armor_wuyinxue", 423, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 142, 5, -1, 190, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), 367, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(212, 424, 1, 103, 5, 207, "icon_Armor_wuyinxue", 425, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 142, 5, -1, 205, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), 368, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(213, 426, 1, 103, 6, 207, "icon_Armor_hunyuanxue", 427, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 142, 5, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), 369, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(214, 428, 1, 103, 7, 207, "icon_Armor_baoxiangshenglian", 429, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 142, 5, -1, 230, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), 370, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(215, 430, 1, 103, 8, 207, "icon_Armor_lingboxianlv", 431, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 142, 5, -1, 240, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), 371, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(216, 432, 1, 103, 0, 216, "icon_Armor_maxianguozu", 433, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 145, 5, -1, 170, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 10), 426, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(217, 434, 1, 103, 1, 216, "icon_Armor_maxianguozu", 435, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 145, 5, -1, 190, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 25), new OuterAndInnerShorts(0, 10), 427, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(218, 436, 1, 103, 2, 216, "icon_Armor_maxianguozu", 437, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 145, 5, -1, 210, 630, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 15), 428, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(219, 438, 1, 103, 3, 216, "icon_Armor_bishuixue", 439, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 145, 5, -1, 235, 685, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 35), new OuterAndInnerShorts(0, 20), 429, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(220, 440, 1, 103, 4, 216, "icon_Armor_bishuixue", 441, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 145, 5, -1, 255, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 20), 430, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(221, 442, 1, 103, 5, 216, "icon_Armor_bishuixue", 443, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 145, 5, -1, 280, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 45), new OuterAndInnerShorts(0, 25), 431, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(222, 444, 1, 103, 6, 216, "icon_Armor_chongxiao", 445, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 145, 5, -1, 305, 845, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 30), 432, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(223, 446, 1, 103, 7, 216, "icon_Armor_juechenlv", 447, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 145, 5, -1, 335, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 55), new OuterAndInnerShorts(0, 30), 433, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(224, 448, 1, 103, 8, 216, "icon_Armor_shenezu", 449, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 145, 5, -1, 360, 960, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 35), 434, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(225, 450, 1, 103, 0, 225, "icon_Armor_buboxie", 451, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 144, 5, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), 372, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(226, 452, 1, 103, 1, 225, "icon_Armor_buboxie", 453, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 144, 5, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), 373, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(227, 454, 1, 103, 2, 225, "icon_Armor_buboxie", 455, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 144, 5, -1, 195, 475, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), 374, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_311", "equip_shank_r", "equip_shank_r/equip_shank_r_311" }, 100));
		_dataArray.Add(new ArmorItem(228, 456, 1, 103, 3, 225, "icon_Armor_baguaxie", 457, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 144, 5, -1, 210, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), 375, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(229, 458, 1, 103, 4, 225, "icon_Armor_baguaxie", 459, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 144, 5, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), 376, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(230, 460, 1, 103, 5, 225, "icon_Armor_baguaxie", 461, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 144, 5, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), 377, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_315", "equip_shank_r", "equip_shank_r/equip_shank_r_315" }, 100));
		_dataArray.Add(new ArmorItem(231, 462, 1, 103, 6, 225, "icon_Armor_taqiankun", 463, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 144, 5, -1, 250, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), 378, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(232, 464, 1, 103, 7, 225, "icon_Armor_xianfulv", 465, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 144, 5, -1, 265, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), 379, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(233, 466, 1, 103, 8, 225, "icon_Armor_xuandoubaoxue", 467, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 144, 5, -1, 280, 680, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), 380, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_319", "equip_shank_r", "equip_shank_r/equip_shank_r_319" }, 100));
		_dataArray.Add(new ArmorItem(234, 468, 1, 103, 0, 234, "icon_Armor_wuliangzudang", 469, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 134, 5, -1, 480, 480, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 5), new OuterAndInnerShorts(0, 0), 309, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_411", "equip_shank_r", "equip_shank_r/equip_shank_r_411" }, 100));
		_dataArray.Add(new ArmorItem(235, 470, 1, 103, 1, 234, "icon_Armor_wuliangzudang", 471, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 175, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 134, 5, -1, 535, 535, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 10), new OuterAndInnerShorts(0, 0), 310, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_411", "equip_shank_r", "equip_shank_r/equip_shank_r_411" }, 100));
		_dataArray.Add(new ArmorItem(236, 472, 1, 103, 2, 234, "icon_Armor_wuliangzudang", 473, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 210, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 134, 5, -1, 595, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 15), new OuterAndInnerShorts(10, 0), 311, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_411", "equip_shank_r", "equip_shank_r/equip_shank_r_411" }, 100));
		_dataArray.Add(new ArmorItem(237, 474, 1, 103, 3, 234, "icon_Armor_luochazudang", 475, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 245, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 134, 5, -1, 655, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 20), new OuterAndInnerShorts(10, 0), 312, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_415", "equip_shank_r", "equip_shank_r/equip_shank_r_415" }, 100));
		_dataArray.Add(new ArmorItem(238, 476, 1, 103, 4, 234, "icon_Armor_luochazudang", 477, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 134, 5, -1, 720, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 25), new OuterAndInnerShorts(15, 0), 313, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_415", "equip_shank_r", "equip_shank_r/equip_shank_r_415" }, 100));
		_dataArray.Add(new ArmorItem(239, 478, 1, 103, 5, 234, "icon_Armor_luochazudang", 479, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 315, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 134, 5, -1, 780, 780, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 30), new OuterAndInnerShorts(20, 10), 314, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_415", "equip_shank_r", "equip_shank_r/equip_shank_r_415" }, 100));
	}

	private void CreateItems4()
	{
		_dataArray.Add(new ArmorItem(240, 480, 1, 103, 6, 234, "icon_Armor_longjingzudang", 481, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 350, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 134, 5, -1, 855, 855, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 35), new OuterAndInnerShorts(20, 10), 315, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_419", "equip_shank_r", "equip_shank_r/equip_shank_r_419" }, 100));
		_dataArray.Add(new ArmorItem(241, 482, 1, 103, 7, 234, "icon_Armor_sanbaodian", 483, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 385, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 134, 5, -1, 920, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 40), new OuterAndInnerShorts(25, 15), 316, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_419", "equip_shank_r", "equip_shank_r/equip_shank_r_419" }, 100));
		_dataArray.Add(new ArmorItem(242, 484, 1, 103, 8, 234, "icon_Armor_jiuxinglv", 485, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 420, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 134, 5, -1, 1000, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 45), new OuterAndInnerShorts(30, 20), 317, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_419", "equip_shank_r", "equip_shank_r/equip_shank_r_419" }, 100));
		_dataArray.Add(new ArmorItem(243, 486, 1, 103, 0, 243, "icon_Armor_shuiyuzuhuan", 487, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 141, 5, -1, 515, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(5, 40), new OuterAndInnerShorts(0, 0), 318, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_431", "equip_shank_r", "equip_shank_r/equip_shank_r_431" }, 100));
		_dataArray.Add(new ArmorItem(244, 488, 1, 103, 1, 243, "icon_Armor_shuiyuzuhuan", 489, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 141, 5, -1, 570, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 50), new OuterAndInnerShorts(0, 0), 319, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_431", "equip_shank_r", "equip_shank_r/equip_shank_r_431" }, 100));
		_dataArray.Add(new ArmorItem(245, 490, 1, 103, 2, 243, "icon_Armor_shuiyuzuhuan", 491, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 141, 5, -1, 630, 490, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 60), new OuterAndInnerShorts(0, 10), 320, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_431", "equip_shank_r", "equip_shank_r/equip_shank_r_431" }, 100));
		_dataArray.Add(new ArmorItem(246, 492, 1, 103, 3, 243, "icon_Armor_lingchiqingjing", 493, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 165, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 141, 5, -1, 690, 540, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 70), new OuterAndInnerShorts(0, 10), 321, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_435", "equip_shank_r", "equip_shank_r/equip_shank_r_435" }, 100));
		_dataArray.Add(new ArmorItem(247, 494, 1, 103, 4, 243, "icon_Armor_lingchiqingjing", 495, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 195, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 141, 5, -1, 750, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 80), new OuterAndInnerShorts(0, 15), 322, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_435", "equip_shank_r", "equip_shank_r/equip_shank_r_435" }, 100));
		_dataArray.Add(new ArmorItem(248, 496, 1, 103, 5, 243, "icon_Armor_lingchiqingjing", 497, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 141, 5, -1, 815, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 90), new OuterAndInnerShorts(10, 20), 323, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_435", "equip_shank_r", "equip_shank_r/equip_shank_r_435" }, 100));
		_dataArray.Add(new ArmorItem(249, 498, 1, 103, 6, 243, "icon_Armor_liantaibingzu", 499, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 141, 5, -1, 880, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 100), new OuterAndInnerShorts(10, 20), 324, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_439", "equip_shank_r", "equip_shank_r/equip_shank_r_439" }, 100));
		_dataArray.Add(new ArmorItem(250, 500, 1, 103, 7, 243, "icon_Armor_panlixue", 501, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 141, 5, -1, 950, 760, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 110), new OuterAndInnerShorts(15, 25), 325, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_439", "equip_shank_r", "equip_shank_r/equip_shank_r_439" }, 100));
		_dataArray.Add(new ArmorItem(251, 502, 1, 103, 8, 243, "icon_Armor_kunlunzu", 503, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 305, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 141, 5, -1, 1020, 820, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 120), new OuterAndInnerShorts(20, 30), 326, new List<string> { "equip_shank_l", "equip_shank_l/equip_shank_l_439", "equip_shank_r", "equip_shank_r/equip_shank_r_439" }, 100));
		_dataArray.Add(new ArmorItem(252, 504, 1, 101, 0, 252, "icon_Armor_tiezhajia", 505, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1050, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 146, 3, -1, 1080, 840, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(253, 506, 1, 101, 1, 252, "icon_Armor_tiezhajia", 507, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1200, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 146, 3, -1, 1235, 975, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(254, 508, 1, 101, 2, 252, "icon_Armor_tiezhajia", 509, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1350, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 146, 3, -1, 1400, 1120, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(255, 510, 1, 101, 3, 252, "icon_Armor_tongxiukai", 511, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1500, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 146, 3, -1, 1575, 1275, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(256, 512, 1, 101, 4, 252, "icon_Armor_tongxiukai", 513, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1650, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 146, 3, -1, 1760, 1440, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(257, 514, 1, 101, 5, 252, "icon_Armor_tongxiukai", 515, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1800, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 146, 3, -1, 1955, 1615, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 105)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(258, 516, 1, 101, 6, 252, "icon_Armor_bawangkai", 517, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1950, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 146, 3, -1, 2160, 1800, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 110)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(30, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(259, 518, 1, 101, 7, 252, "icon_Armor_baihuangwusekai", 519, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 2100, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 146, 3, -1, 2375, 1995, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 115)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 0), new OuterAndInnerShorts(30, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(260, 520, 1, 101, 8, 252, "icon_Armor_qingxuanbaokai", 521, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 2250, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 146, 3, -1, 2600, 2200, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 120)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 0), new OuterAndInnerShorts(35, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(261, 522, 1, 101, 0, 261, "icon_Armor_huxinduankai", 523, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 945, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 147, 3, -1, 920, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_111", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_111", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_111", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_111" }, 100));
		_dataArray.Add(new ArmorItem(262, 524, 1, 101, 1, 261, "icon_Armor_huxinduankai", 525, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1080, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 147, 3, -1, 1055, 1055, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 25), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_111", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_111", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_111", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_111" }, 100));
		_dataArray.Add(new ArmorItem(263, 526, 1, 101, 2, 261, "icon_Armor_huxinduankai", 527, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1215, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 147, 3, -1, 1195, 1195, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 30), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_111", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_111", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_111", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_111" }, 100));
		_dataArray.Add(new ArmorItem(264, 528, 1, 101, 3, 261, "icon_Armor_yanlingjia", 529, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1350, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 147, 3, -1, 1350, 1350, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 35), new OuterAndInnerShorts(10, 0), -1, new List<string> { "collar/collar", "collar/collar_115", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_115", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_115", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_115", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_115" }, 100));
		_dataArray.Add(new ArmorItem(265, 530, 1, 101, 4, 261, "icon_Armor_yanlingjia", 531, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1485, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 147, 3, -1, 1510, 1510, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 40), new OuterAndInnerShorts(15, 0), -1, new List<string> { "collar/collar", "collar/collar_115", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_115", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_115", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_115", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_115" }, 100));
		_dataArray.Add(new ArmorItem(266, 532, 1, 101, 5, 261, "icon_Armor_yanlingjia", 533, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1620, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 147, 3, -1, 1685, 1685, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 45), new OuterAndInnerShorts(20, 10), -1, new List<string> { "collar/collar", "collar/collar_115", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_115", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_115", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_115", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_115" }, 100));
		_dataArray.Add(new ArmorItem(267, 534, 1, 101, 6, 261, "icon_Armor_tianwangjia", 535, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1755, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 147, 3, -1, 1865, 1865, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 50), new OuterAndInnerShorts(20, 10), -1, new List<string>
		{
			"collar/collar", "collar/collar_119", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_119", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_119", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_119", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_119",
			"equip_waist/equip_waist", "equip_waist/equip_waist_119", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_119", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_119"
		}, 100));
		_dataArray.Add(new ArmorItem(268, 536, 1, 101, 7, 261, "icon_Armor_guibeituolongjia", 537, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1890, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 147, 3, -1, 2050, 2050, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 105)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 55), new OuterAndInnerShorts(25, 15), -1, new List<string>
		{
			"collar/collar", "collar/collar_119", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_119", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_119", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_119", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_119",
			"equip_waist/equip_waist", "equip_waist/equip_waist_119", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_119", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_119"
		}, 100));
		_dataArray.Add(new ArmorItem(269, 538, 1, 101, 8, 261, "icon_Armor_niniujia", 539, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 2025, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 147, 3, -1, 2250, 2250, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 110)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 60), new OuterAndInnerShorts(30, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_119", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_119", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_119", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_119", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_119",
			"equip_waist/equip_waist", "equip_waist/equip_waist_119", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_119", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_119"
		}, 100));
		_dataArray.Add(new ArmorItem(270, 540, 1, 101, 0, 270, "icon_Armor_yuantongyi", 541, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 840, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 154, 3, -1, 755, 995, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(271, 542, 1, 101, 1, 270, "icon_Armor_yuantongyi", 543, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 960, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 154, 3, -1, 870, 1130, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(272, 544, 1, 101, 2, 270, "icon_Armor_yuantongyi", 545, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1080, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 154, 3, -1, 995, 1275, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_131", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_131", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_131" }, 100));
		_dataArray.Add(new ArmorItem(273, 546, 1, 101, 3, 270, "icon_Armor_juanyunhuangjia", 547, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1200, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 154, 3, -1, 1125, 1425, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 50), new OuterAndInnerShorts(10, 10), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(274, 548, 1, 101, 4, 270, "icon_Armor_juanyunhuangjia", 549, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1320, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 154, 3, -1, 1265, 1585, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(10, 10), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(275, 550, 1, 101, 5, 270, "icon_Armor_juanyunhuangjia", 551, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1440, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 154, 3, -1, 1410, 1750, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 105)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 70), new OuterAndInnerShorts(15, 15), -1, new List<string> { "collar/collar", "collar/collar_135", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_135", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_135", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_135", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_135" }, 100));
		_dataArray.Add(new ArmorItem(276, 552, 1, 101, 6, 270, "icon_Armor_qiufuzhou", 553, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1560, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 154, 3, -1, 1565, 1925, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 110)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(277, 554, 1, 101, 7, 270, "icon_Armor_xuanzhupao", 555, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1680, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 154, 3, -1, 1730, 2110, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 115)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 90), new OuterAndInnerShorts(20, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(278, 556, 1, 101, 8, 270, "icon_Armor_jinjianbaojia", 557, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1800, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 154, 3, -1, 1900, 2300, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 120)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 100), new OuterAndInnerShorts(25, 25), -1, new List<string>
		{
			"collar/collar", "collar/collar_139", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_139", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_139", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_139", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_139",
			"equip_waist/equip_waist", "equip_waist/equip_waist_139", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_139", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_139"
		}, 100));
		_dataArray.Add(new ArmorItem(279, 558, 1, 101, 0, 279, "icon_Armor_tongsuojia", 559, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 735, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 155, 3, -1, 655, 1015, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_121", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_121", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_121" }, 100));
		_dataArray.Add(new ArmorItem(280, 560, 1, 101, 1, 279, "icon_Armor_tongsuojia", 561, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 840, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 155, 3, -1, 755, 1145, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_121", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_121", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_121" }, 100));
		_dataArray.Add(new ArmorItem(281, 562, 1, 101, 2, 279, "icon_Armor_tongsuojia", 563, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 945, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 155, 3, -1, 860, 1280, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 60), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_121", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_121", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_121" }, 100));
		_dataArray.Add(new ArmorItem(282, 564, 1, 101, 3, 279, "icon_Armor_liuhuoshanwenjia", 565, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1050, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 155, 3, -1, 975, 1425, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 70), new OuterAndInnerShorts(0, 10), -1, new List<string> { "collar/collar", "collar/collar_125", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_125", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_125", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_125", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_125" }, 100));
		_dataArray.Add(new ArmorItem(283, 566, 1, 101, 4, 279, "icon_Armor_liuhuoshanwenjia", 567, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1155, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 155, 3, -1, 1095, 1575, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 80), new OuterAndInnerShorts(0, 15), -1, new List<string> { "collar/collar", "collar/collar_125", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_125", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_125", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_125", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_125" }, 100));
		_dataArray.Add(new ArmorItem(284, 568, 1, 101, 5, 279, "icon_Armor_liuhuoshanwenjia", 569, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1260, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 155, 3, -1, 1225, 1735, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 90), new OuterAndInnerShorts(10, 20), -1, new List<string> { "collar/collar", "collar/collar_125", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_125", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_125", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_125", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_125" }, 100));
		_dataArray.Add(new ArmorItem(285, 570, 1, 101, 6, 279, "icon_Armor_shengyuanyi", 571, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1365, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 155, 3, -1, 1360, 1900, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 100), new OuterAndInnerShorts(10, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_129", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_129", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_129", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_129", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_129",
			"equip_waist/equip_waist", "equip_waist/equip_waist_129", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_129", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_129"
		}, 100));
		_dataArray.Add(new ArmorItem(286, 572, 1, 101, 7, 279, "icon_Armor_xuanwukai", 573, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1470, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 155, 3, -1, 1500, 2070, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 105)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(55, 110), new OuterAndInnerShorts(15, 25), -1, new List<string>
		{
			"collar/collar", "collar/collar_129", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_129", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_129", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_129", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_129",
			"equip_waist/equip_waist", "equip_waist/equip_waist_129", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_129", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_129"
		}, 100));
		_dataArray.Add(new ArmorItem(287, 574, 1, 101, 8, 279, "icon_Armor_fenguangbaoyi", 575, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 1575, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 155, 3, -1, 1650, 2250, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 110)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 120), new OuterAndInnerShorts(20, 30), -1, new List<string>
		{
			"collar/collar", "collar/collar_129", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_129", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_129", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_129", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_129",
			"equip_waist/equip_waist", "equip_waist/equip_waist_129", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_129", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_129"
		}, 100));
		_dataArray.Add(new ArmorItem(288, 576, 1, 101, 0, 288, "icon_Armor_muzhajia", 577, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 360, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 148, 3, -1, 360, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_231", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_231", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_231" }, 100));
		_dataArray.Add(new ArmorItem(289, 578, 1, 101, 1, 288, "icon_Armor_muzhajia", 579, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 430, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 148, 3, -1, 430, 820, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 15), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_231", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_231", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_231" }, 100));
		_dataArray.Add(new ArmorItem(290, 580, 1, 101, 2, 288, "icon_Armor_muzhajia", 581, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 500, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 148, 3, -1, 505, 925, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_231", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_231", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_231" }, 100));
		_dataArray.Add(new ArmorItem(291, 582, 1, 101, 3, 288, "icon_Armor_chixiaoyi", 583, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 575, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 148, 3, -1, 585, 1035, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 25), new OuterAndInnerShorts(10, 0), -1, new List<string> { "collar/collar", "collar/collar_235", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_235", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_235", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_235", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_235" }, 100));
		_dataArray.Add(new ArmorItem(292, 584, 1, 101, 4, 288, "icon_Armor_chixiaoyi", 585, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 645, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 148, 3, -1, 670, 1150, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 30), new OuterAndInnerShorts(15, 0), -1, new List<string> { "collar/collar", "collar/collar_235", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_235", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_235", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_235", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_235" }, 100));
		_dataArray.Add(new ArmorItem(293, 586, 1, 101, 5, 288, "icon_Armor_chixiaoyi", 587, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 720, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 148, 3, -1, 765, 1275, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 35), new OuterAndInnerShorts(20, 0), -1, new List<string> { "collar/collar", "collar/collar_235", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_235", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_235", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_235", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_235" }, 100));
		_dataArray.Add(new ArmorItem(294, 588, 1, 101, 6, 288, "icon_Armor_baiqiaolinglongjia", 589, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 790, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 148, 3, -1, 865, 1405, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 40), new OuterAndInnerShorts(20, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_239", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_239", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_239", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_239", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_239",
			"equip_waist/equip_waist", "equip_waist/equip_waist_239", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_239", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_239"
		}, 100));
		_dataArray.Add(new ArmorItem(295, 590, 1, 101, 7, 288, "icon_Armor_kuhuanjia", 591, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 860, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 148, 3, -1, 970, 1540, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 45), new OuterAndInnerShorts(25, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_239", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_239", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_239", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_239", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_239",
			"equip_waist/equip_waist", "equip_waist/equip_waist_239", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_239", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_239"
		}, 100));
		_dataArray.Add(new ArmorItem(296, 592, 1, 101, 8, 288, "icon_Armor_xuanhuangmujia", 593, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 935, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 148, 3, -1, 1080, 1680, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 50), new OuterAndInnerShorts(30, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_239", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_239", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_239", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_239", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_239",
			"equip_waist/equip_waist", "equip_waist/equip_waist_239", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_239", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_239"
		}, 100));
		_dataArray.Add(new ArmorItem(297, 594, 1, 101, 0, 297, "icon_Armor_qingzhuyi", 595, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 285, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 156, 3, -1, 385, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_211", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_211", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_211" }, 100));
		_dataArray.Add(new ArmorItem(298, 596, 1, 101, 1, 297, "icon_Armor_qingzhuyi", 597, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 340, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 156, 3, -1, 440, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_211", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_211", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_211" }, 100));
		_dataArray.Add(new ArmorItem(299, 598, 1, 101, 2, 297, "icon_Armor_qingzhuyi", 599, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 395, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 156, 3, -1, 510, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 40), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_211", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_211", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_211" }, 100));
	}

	private void CreateItems5()
	{
		_dataArray.Add(new ArmorItem(300, 600, 1, 101, 3, 297, "icon_Armor_guishezhou", 601, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 450, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 156, 3, -1, 580, 880, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 50), new OuterAndInnerShorts(0, 10), -1, new List<string> { "collar/collar", "collar/collar_215", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_215", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_215", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_215", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_215" }, 100));
		_dataArray.Add(new ArmorItem(301, 602, 1, 101, 4, 297, "icon_Armor_guishezhou", 603, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 500, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 156, 3, -1, 655, 975, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 60), new OuterAndInnerShorts(0, 15), -1, new List<string> { "collar/collar", "collar/collar_215", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_215", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_215", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_215", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_215" }, 100));
		_dataArray.Add(new ArmorItem(302, 604, 1, 101, 5, 297, "icon_Armor_guishezhou", 605, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 555, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 156, 3, -1, 730, 1070, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 70), new OuterAndInnerShorts(0, 20), -1, new List<string> { "collar/collar", "collar/collar_215", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_215", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_215", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_215", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_215" }, 100));
		_dataArray.Add(new ArmorItem(303, 606, 1, 101, 6, 297, "icon_Armor_ziweixuanmujia", 607, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 610, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 156, 3, -1, 820, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 80), new OuterAndInnerShorts(0, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_219", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_219", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_219", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_219", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_219",
			"equip_waist/equip_waist", "equip_waist/equip_waist_219", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_219", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_219"
		}, 100));
		_dataArray.Add(new ArmorItem(304, 608, 1, 101, 7, 297, "icon_Armor_minglingbaozhou", 609, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 665, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 156, 3, -1, 905, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 90), new OuterAndInnerShorts(0, 25), -1, new List<string>
		{
			"collar/collar", "collar/collar_219", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_219", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_219", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_219", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_219",
			"equip_waist/equip_waist", "equip_waist/equip_waist_219", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_219", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_219"
		}, 100));
		_dataArray.Add(new ArmorItem(305, 610, 1, 101, 8, 297, "icon_Armor_daixingyi", 611, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 720, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 156, 3, -1, 1000, 1400, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 100), new OuterAndInnerShorts(0, 30), -1, new List<string>
		{
			"collar/collar", "collar/collar_219", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_219", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_219", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_219", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_219",
			"equip_waist/equip_waist", "equip_waist/equip_waist_219", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_219", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_219"
		}, 100));
		_dataArray.Add(new ArmorItem(306, 612, 1, 101, 0, 306, "icon_Armor_hupibeixin", 613, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 151, 3, -1, 230, 470, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(307, 614, 1, 101, 1, 306, "icon_Armor_hupibeixin", 615, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 210, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 151, 3, -1, 265, 525, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(308, 616, 1, 101, 2, 306, "icon_Armor_hupibeixin", 617, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 240, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 151, 3, -1, 310, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(309, 618, 1, 101, 3, 306, "icon_Armor_shouwangpigua", 619, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 270, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 151, 3, -1, 355, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(310, 620, 1, 101, 4, 306, "icon_Armor_shouwangpigua", 621, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 300, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 151, 3, -1, 400, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(311, 622, 1, 101, 5, 306, "icon_Armor_shouwangpigua", 623, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 330, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 151, 3, -1, 450, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(312, 624, 1, 101, 6, 306, "icon_Armor_fenghuangzhijin", 625, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 360, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 151, 3, -1, 505, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(313, 626, 1, 101, 7, 306, "icon_Armor_shenxiuyi", 627, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 390, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 151, 3, -1, 560, 940, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(314, 628, 1, 101, 8, 306, "icon_Armor_tianmingjia", 629, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 420, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 151, 3, -1, 620, 1020, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(315, 630, 1, 101, 0, 315, "icon_Armor_huangwenpigua", 631, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 152, 3, -1, 205, 445, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(316, 632, 1, 101, 1, 315, "icon_Armor_huangwenpigua", 633, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 152, 3, -1, 235, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(317, 634, 1, 101, 2, 315, "icon_Armor_huangwenpigua", 635, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 152, 3, -1, 265, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(318, 636, 1, 101, 3, 315, "icon_Armor_biyuechangshan", 637, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 200, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 152, 3, -1, 300, 600, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(319, 638, 1, 101, 4, 315, "icon_Armor_biyuechangshan", 639, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 220, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 152, 3, -1, 335, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(320, 640, 1, 101, 5, 315, "icon_Armor_biyuechangshan", 641, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 240, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 152, 3, -1, 375, 715, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(321, 642, 1, 101, 6, 315, "icon_Armor_mingmiwuse", 643, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 260, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 152, 3, -1, 415, 775, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(322, 644, 1, 101, 7, 315, "icon_Armor_baicai", 645, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 152, 3, -1, 455, 835, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65),
			new PropertyAndValue(5, 65)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(323, 646, 1, 101, 8, 315, "icon_Armor_jinchanbaoyi", 647, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 300, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 152, 3, -1, 500, 900, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70),
			new PropertyAndValue(5, 70)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(324, 648, 1, 101, 0, 324, "icon_Armor_shoulieduanshan", 649, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 150, 3, -1, 190, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(325, 650, 1, 101, 1, 324, "icon_Armor_shoulieduanshan", 651, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 135, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 150, 3, -1, 215, 475, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(326, 652, 1, 101, 2, 324, "icon_Armor_shoulieduanshan", 653, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 150, 3, -1, 245, 525, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(327, 654, 1, 101, 3, 324, "icon_Armor_baguahechang", 655, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 165, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 150, 3, -1, 270, 570, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(328, 656, 1, 101, 4, 324, "icon_Armor_baguahechang", 657, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 150, 3, -1, 305, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(329, 658, 1, 101, 5, 324, "icon_Armor_baguahechang", 659, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 195, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 150, 3, -1, 330, 670, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(330, 660, 1, 101, 6, 324, "icon_Armor_liuxupigua", 661, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 210, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 150, 3, -1, 370, 730, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(331, 662, 1, 101, 7, 324, "icon_Armor_biyixianmei", 663, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 225, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 150, 3, -1, 400, 780, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(332, 664, 1, 101, 8, 324, "icon_Armor_shenguanglihe", 665, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 240, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 150, 3, -1, 440, 840, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(333, 666, 1, 101, 0, 333, "icon_Armor_lianjia", 667, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 220, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 153, 3, -1, 250, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(334, 668, 1, 101, 1, 333, "icon_Armor_lianjia", 669, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 260, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 153, 3, -1, 300, 690, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(335, 670, 1, 101, 2, 333, "icon_Armor_lianjia", 671, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 300, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 153, 3, -1, 350, 770, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_311", "peplum/peplum", "peplum/peplum_311", "skirt/skirt", "skirt/skirt_311", "skirt_back/skirt_back", "skirt_back/skirt_back_311" }, 100));
		_dataArray.Add(new ArmorItem(336, 672, 1, 101, 3, 333, "icon_Armor_shuangshijinluoqiu", 673, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 340, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 153, 3, -1, 405, 855, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(337, 674, 1, 101, 4, 333, "icon_Armor_shuangshijinluoqiu", 675, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 380, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 153, 3, -1, 465, 945, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(338, 676, 1, 101, 5, 333, "icon_Armor_shuangshijinluoqiu", 677, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 420, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 153, 3, -1, 525, 1035, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_315", "peplum/peplum", "peplum/peplum_315", "skirt/skirt", "skirt/skirt_315", "skirt_back/skirt_back", "skirt_back/skirt_back_315" }, 100));
		_dataArray.Add(new ArmorItem(339, 678, 1, 101, 6, 333, "icon_Armor_kunpengjia", 679, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 460, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 153, 3, -1, 595, 1135, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(340, 680, 1, 101, 7, 333, "icon_Armor_jiaoxiaobaoyi", 681, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 500, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 153, 3, -1, 665, 1235, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(55, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(341, 682, 1, 101, 8, 333, "icon_Armor_gualonglin", 683, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 540, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 153, 3, -1, 740, 1340, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(35, 0), -1, new List<string> { "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_319", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_319", "peplum/peplum", "peplum/peplum_319", "skirt/skirt", "skirt/skirt_319", "skirt_back/skirt_back", "skirt_back/skirt_back_319" }, 100));
		_dataArray.Add(new ArmorItem(342, 684, 1, 101, 0, 342, "icon_Armor_mabuyi", 685, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 158, 3, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(343, 686, 1, 101, 1, 342, "icon_Armor_mabuyi", 687, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 158, 3, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(344, 688, 1, 101, 2, 342, "icon_Armor_mabuyi", 689, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 158, 3, -1, 205, 485, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(345, 690, 1, 101, 3, 342, "icon_Armor_taigegangyi", 691, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 158, 3, -1, 220, 520, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(346, 692, 1, 101, 4, 342, "icon_Armor_taigegangyi", 693, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 158, 3, -1, 240, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(347, 694, 1, 101, 5, 342, "icon_Armor_taigegangyi", 695, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 158, 3, -1, 255, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(348, 696, 1, 101, 6, 342, "icon_Armor_jiangxuelinghanyi", 697, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 158, 3, -1, 280, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(349, 698, 1, 101, 7, 342, "icon_Armor_bingjibaoyi", 699, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 158, 3, -1, 295, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(350, 700, 1, 101, 8, 342, "icon_Armor_jiuhuayi", 701, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 158, 3, -1, 320, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(351, 702, 1, 101, 0, 351, "icon_Armor_juanmajia", 703, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 160, 3, -1, 180, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(352, 704, 1, 101, 1, 351, "icon_Armor_juanmajia", 705, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 160, 3, -1, 200, 460, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(353, 706, 1, 101, 2, 351, "icon_Armor_juanmajia", 707, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 160, 3, -1, 225, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(354, 708, 1, 101, 3, 351, "icon_Armor_liemingxuanchang", 709, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 160, 3, -1, 250, 550, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(355, 710, 1, 101, 4, 351, "icon_Armor_liemingxuanchang", 711, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 160, 3, -1, 270, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(356, 712, 1, 101, 5, 351, "icon_Armor_liemingxuanchang", 713, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 160, 3, -1, 300, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(357, 714, 1, 101, 6, 351, "icon_Armor_baoxiangtianyi", 715, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 160, 3, -1, 325, 685, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(358, 716, 1, 101, 7, 351, "icon_Armor_xiehuansha", 717, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 170, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 160, 3, -1, 350, 730, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 65),
			new PropertyAndValue(5, 65)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(359, 718, 1, 101, 8, 351, "icon_Armor_tiancanbaoyi", 719, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 160, 3, -1, 380, 780, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 70),
			new PropertyAndValue(5, 70)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
	}

	private void CreateItems6()
	{
		_dataArray.Add(new ArmorItem(360, 720, 1, 101, 0, 360, "icon_Armor_kuxingyi", 721, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 200, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 161, 3, -1, 240, 600, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 10), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(361, 722, 1, 101, 1, 360, "icon_Armor_kuxingyi", 723, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 235, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 161, 3, -1, 280, 670, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 25), new OuterAndInnerShorts(0, 10), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(362, 724, 1, 101, 2, 360, "icon_Armor_kuxingyi", 725, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 270, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 161, 3, -1, 330, 750, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 15), -1, new List<string>
		{
			"collar/collar", "collar/collar_321", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_321", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_321", "peplum/peplum", "peplum/peplum_321", "skirt/skirt", "skirt/skirt_321",
			"skirt_back/skirt_back", "skirt_back/skirt_back_321"
		}, 100));
		_dataArray.Add(new ArmorItem(363, 726, 1, 101, 3, 360, "icon_Armor_cuimuyi", 727, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 305, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 161, 3, -1, 375, 825, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 35), new OuterAndInnerShorts(0, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(364, 728, 1, 101, 4, 360, "icon_Armor_cuimuyi", 729, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 340, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 161, 3, -1, 430, 910, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(365, 730, 1, 101, 5, 360, "icon_Armor_cuimuyi", 731, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 375, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 161, 3, -1, 485, 995, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 45), new OuterAndInnerShorts(0, 25), -1, new List<string>
		{
			"collar/collar", "collar/collar_325", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_325", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_325", "peplum/peplum", "peplum/peplum_325", "skirt/skirt", "skirt/skirt_325",
			"skirt_back/skirt_back", "skirt_back/skirt_back_325"
		}, 100));
		_dataArray.Add(new ArmorItem(366, 732, 1, 101, 6, 360, "icon_Armor_kongqueluo", 733, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 410, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 161, 3, -1, 550, 1090, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 30), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(367, 734, 1, 101, 7, 360, "icon_Armor_zishoubaoyi", 735, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 445, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 161, 3, -1, 610, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 55), new OuterAndInnerShorts(0, 30), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(368, 736, 1, 101, 8, 360, "icon_Armor_nayuanshan", 737, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 480, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 161, 3, -1, 680, 1280, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 35), -1, new List<string>
		{
			"collar/collar", "collar/collar_329", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_329", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_329", "peplum/peplum", "peplum/peplum_329", "skirt/skirt", "skirt/skirt_329",
			"skirt_back/skirt_back", "skirt_back/skirt_back_329"
		}, 100));
		_dataArray.Add(new ArmorItem(369, 738, 1, 101, 0, 369, "icon_Armor_guantouyi", 739, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 159, 3, -1, 215, 455, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(370, 740, 1, 101, 1, 369, "icon_Armor_guantouyi", 741, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 185, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 159, 3, -1, 245, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(371, 742, 1, 101, 2, 369, "icon_Armor_guantouyi", 743, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 210, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 159, 3, -1, 285, 565, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_331", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_331", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_331", "peplum/peplum", "peplum/peplum_331", "skirt/skirt", "skirt/skirt_331",
			"skirt_back/skirt_back", "skirt_back/skirt_back_331"
		}, 100));
		_dataArray.Add(new ArmorItem(372, 744, 1, 101, 3, 369, "icon_Armor_baxianchang", 745, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 235, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 159, 3, -1, 325, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(373, 746, 1, 101, 4, 369, "icon_Armor_baxianchang", 747, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 260, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 159, 3, -1, 370, 690, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(374, 748, 1, 101, 5, 369, "icon_Armor_baxianchang", 749, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 285, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 159, 3, -1, 410, 750, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_335", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_335", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_335", "peplum/peplum", "peplum/peplum_335", "skirt/skirt", "skirt/skirt_335",
			"skirt_back/skirt_back", "skirt_back/skirt_back_335"
		}, 100));
		_dataArray.Add(new ArmorItem(375, 750, 1, 101, 6, 369, "icon_Armor_jiuyouditingpao", 751, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 310, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 159, 3, -1, 460, 820, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(376, 752, 1, 101, 7, 369, "icon_Armor_yunnibaoshan", 753, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 335, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 159, 3, -1, 505, 885, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(377, 754, 1, 101, 8, 369, "icon_Armor_xuandoutianyi", 755, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 360, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 159, 3, -1, 560, 960, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string>
		{
			"collar/collar", "collar/collar_339", "sleeve_l/sleeve_l", "sleeve_l/sleeve_l_339", "sleeve_r/sleeve_r", "sleeve_r/sleeve_r_339", "peplum/peplum", "peplum/peplum_339", "skirt/skirt", "skirt/skirt_339",
			"skirt_back/skirt_back", "skirt_back/skirt_back_339"
		}, 100));
		_dataArray.Add(new ArmorItem(378, 756, 1, 101, 0, 378, "icon_Armor_fanyijia", 757, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 585, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 149, 3, -1, 730, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 5), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_421", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_421", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_421" }, 100));
		_dataArray.Add(new ArmorItem(379, 758, 1, 101, 1, 378, "icon_Armor_fanyijia", 759, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 670, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 149, 3, -1, 830, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_421", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_421", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_421" }, 100));
		_dataArray.Add(new ArmorItem(380, 760, 1, 101, 2, 378, "icon_Armor_fanyijia", 761, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 755, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 149, 3, -1, 940, 800, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 15), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_421", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_421", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_421" }, 100));
		_dataArray.Add(new ArmorItem(381, 762, 1, 101, 3, 378, "icon_Armor_heibazhang", 763, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 840, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 149, 3, -1, 1050, 900, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "collar/collar", "collar/collar_425", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_425", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_425", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_425", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_425" }, 100));
		_dataArray.Add(new ArmorItem(382, 764, 1, 101, 4, 378, "icon_Armor_heibazhang", 765, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 920, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 149, 3, -1, 1170, 1010, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 25), new OuterAndInnerShorts(15, 0), -1, new List<string> { "collar/collar", "collar/collar_425", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_425", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_425", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_425", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_425" }, 100));
		_dataArray.Add(new ArmorItem(383, 766, 1, 101, 5, 378, "icon_Armor_heibazhang", 767, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 1005, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 149, 3, -1, 1290, 1120, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 30), new OuterAndInnerShorts(20, 10), -1, new List<string> { "collar/collar", "collar/collar_425", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_425", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_425", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_425", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_425" }, 100));
		_dataArray.Add(new ArmorItem(384, 768, 1, 101, 6, 378, "icon_Armor_baiyaoyi", 769, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 1090, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 149, 3, -1, 1420, 1240, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 35), new OuterAndInnerShorts(20, 10), -1, new List<string>
		{
			"collar/collar", "collar/collar_429", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_429", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_429", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_429", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_429",
			"equip_waist/equip_waist", "equip_waist/equip_waist_429", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_429", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_429"
		}, 100));
		_dataArray.Add(new ArmorItem(385, 770, 1, 101, 7, 378, "icon_Armor_qinglongbaozhou", 771, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 1175, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 149, 3, -1, 1560, 1370, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 75),
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 40), new OuterAndInnerShorts(25, 15), -1, new List<string>
		{
			"collar/collar", "collar/collar_429", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_429", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_429", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_429", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_429",
			"equip_waist/equip_waist", "equip_waist/equip_waist_429", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_429", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_429"
		}, 100));
		_dataArray.Add(new ArmorItem(386, 772, 1, 101, 8, 378, "icon_Armor_haosuyi", 773, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 1260, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 149, 3, -1, 1700, 1500, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 80),
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 45), new OuterAndInnerShorts(30, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_429", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_429", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_429", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_429", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_429",
			"equip_waist/equip_waist", "equip_waist/equip_waist_429", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_429", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_429"
		}, 100));
		_dataArray.Add(new ArmorItem(387, 774, 1, 101, 0, 387, "icon_Armor_yubeixin", 775, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 530, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 157, 3, -1, 770, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(5, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_411", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_411", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_411" }, 100));
		_dataArray.Add(new ArmorItem(388, 776, 1, 101, 1, 387, "icon_Armor_yubeixin", 777, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 580, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 157, 3, -1, 850, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_411", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_411", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_411" }, 100));
		_dataArray.Add(new ArmorItem(389, 778, 1, 101, 2, 387, "icon_Armor_yubeixin", 779, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 630, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 157, 3, -1, 945, 665, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 60), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_411", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_411", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_411" }, 100));
		_dataArray.Add(new ArmorItem(390, 780, 1, 101, 3, 387, "icon_Armor_lengxunchangyi", 781, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 675, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 157, 3, -1, 1035, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 70), new OuterAndInnerShorts(0, 10), -1, new List<string> { "collar/collar", "collar/collar_415", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_415", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_415", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_415", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_415" }, 100));
		_dataArray.Add(new ArmorItem(391, 782, 1, 101, 4, 387, "icon_Armor_lengxunchangyi", 783, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 725, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 157, 3, -1, 1135, 815, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 80), new OuterAndInnerShorts(0, 15), -1, new List<string> { "collar/collar", "collar/collar_415", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_415", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_415", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_415", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_415" }, 100));
		_dataArray.Add(new ArmorItem(392, 784, 1, 101, 5, 387, "icon_Armor_lengxunchangyi", 785, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 775, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 157, 3, -1, 1235, 895, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 90), new OuterAndInnerShorts(10, 20), -1, new List<string> { "collar/collar", "collar/collar_415", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_415", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_415", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_415", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_415" }, 100));
		_dataArray.Add(new ArmorItem(393, 786, 1, 101, 6, 387, "icon_Armor_cailvyuyi", 787, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 825, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 157, 3, -1, 1340, 980, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 100), new OuterAndInnerShorts(10, 20), -1, new List<string>
		{
			"collar/collar", "collar/collar_419", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_419", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_419", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_419", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_419",
			"equip_waist/equip_waist", "equip_waist/equip_waist_419", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_419", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_419"
		}, 100));
		_dataArray.Add(new ArmorItem(394, 788, 1, 101, 7, 387, "icon_Armor_xuelibaojia", 789, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 875, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 157, 3, -1, 1445, 1065, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 75),
			new PropertyAndValue(4, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 110), new OuterAndInnerShorts(15, 25), -1, new List<string>
		{
			"collar/collar", "collar/collar_419", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_419", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_419", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_419", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_419",
			"equip_waist/equip_waist", "equip_waist/equip_waist_419", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_419", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_419"
		}, 100));
		_dataArray.Add(new ArmorItem(395, 790, 1, 101, 8, 387, "icon_Armor_sumingbaolianyi", 791, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 920, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 157, 3, -1, 1560, 1160, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 80),
			new PropertyAndValue(4, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 120), new OuterAndInnerShorts(20, 30), -1, new List<string>
		{
			"collar/collar", "collar/collar_419", "equip_arm_l/equip_arm_l", "equip_arm_l/equip_arm_l_419", "equip_arm_r/equip_arm_r", "equip_arm_r/equip_arm_r_419", "equip_bw_l/equip_bw_l", "equip_bw_l/equip_bw_l_419", "equip_bw_r/equip_bw_r", "equip_bw_r/equip_bw_r_419",
			"equip_waist/equip_waist", "equip_waist/equip_waist_419", "equip_leg_l/equip_leg_l", "equip_leg_l/equip_leg_l_419", "equip_leg_r/equip_leg_r", "equip_leg_r/equip_leg_r_419"
		}, 100));
		_dataArray.Add(new ArmorItem(396, 792, 1, 102, 0, 396, "icon_Armor_heitiehubi", 793, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 210, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 162, 4, -1, 625, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_131", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_131" }, 100));
		_dataArray.Add(new ArmorItem(397, 794, 1, 102, 1, 396, "icon_Armor_heitiehubi", 795, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 275, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 162, 4, -1, 700, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_131", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_131" }, 100));
		_dataArray.Add(new ArmorItem(398, 796, 1, 102, 2, 396, "icon_Armor_heitiehubi", 797, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 345, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 162, 4, -1, 790, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_131", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_131" }, 100));
		_dataArray.Add(new ArmorItem(399, 798, 1, 102, 3, 396, "icon_Armor_jingganghuanbei", 799, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 410, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 162, 4, -1, 880, 880, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_135", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_135" }, 100));
		_dataArray.Add(new ArmorItem(400, 800, 1, 102, 4, 396, "icon_Armor_jingganghuanbei", 801, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 480, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 162, 4, -1, 975, 975, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_135", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_135" }, 100));
		_dataArray.Add(new ArmorItem(401, 802, 1, 102, 5, 396, "icon_Armor_jingganghuanbei", 803, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 162, 4, -1, 1070, 1070, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_135", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_135" }, 100));
		_dataArray.Add(new ArmorItem(402, 804, 1, 102, 6, 396, "icon_Armor_chihubeijia", 805, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 615, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 162, 4, -1, 1180, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_139", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_139" }, 100));
		_dataArray.Add(new ArmorItem(403, 806, 1, 102, 7, 396, "icon_Armor_ningchuanzhihai", 807, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 680, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 162, 4, -1, 1285, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_139", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_139" }, 100));
		_dataArray.Add(new ArmorItem(404, 808, 1, 102, 8, 396, "icon_Armor_xuantiebi", 809, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 750, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 162, 4, -1, 1400, 1400, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 0), new OuterAndInnerShorts(35, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_139", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_139" }, 100));
		_dataArray.Add(new ArmorItem(405, 810, 1, 102, 0, 405, "icon_Armor_tonghubi", 811, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 180, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 2, 36, 169, 4, -1, 490, 730, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_111", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_111" }, 100));
		_dataArray.Add(new ArmorItem(406, 812, 1, 102, 1, 405, "icon_Armor_tonghubi", 813, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 230, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 2, 36, 169, 4, -1, 555, 815, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_111", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_111" }, 100));
		_dataArray.Add(new ArmorItem(407, 814, 1, 102, 2, 405, "icon_Armor_tonghubi", 815, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 285, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 2, 36, 169, 4, -1, 625, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_111", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_111" }, 100));
		_dataArray.Add(new ArmorItem(408, 816, 1, 102, 3, 405, "icon_Armor_shanwenhubi", 817, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 335, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 2, 36, 169, 4, -1, 690, 990, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 50), new OuterAndInnerShorts(10, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_115", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_115" }, 100));
		_dataArray.Add(new ArmorItem(409, 818, 1, 102, 4, 405, "icon_Armor_shanwenhubi", 819, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 390, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 2, 36, 169, 4, -1, 770, 1090, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(10, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_115", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_115" }, 100));
		_dataArray.Add(new ArmorItem(410, 820, 1, 102, 5, 405, "icon_Armor_shanwenhubi", 821, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 440, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 2, 36, 169, 4, -1, 840, 1180, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 70), new OuterAndInnerShorts(15, 15), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_115", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_115" }, 100));
		_dataArray.Add(new ArmorItem(411, 822, 1, 102, 6, 405, "icon_Armor_liucaijinlou", 823, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 495, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 2, 36, 169, 4, -1, 925, 1285, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_119", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_119" }, 100));
		_dataArray.Add(new ArmorItem(412, 824, 1, 102, 7, 405, "icon_Armor_qingyunsuolei", 825, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 545, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 2, 36, 169, 4, -1, 1005, 1385, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 95)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 90), new OuterAndInnerShorts(20, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_119", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_119" }, 100));
		_dataArray.Add(new ArmorItem(413, 826, 1, 102, 8, 405, "icon_Armor_jinchanbaozhuo", 827, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 100, 600, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 2, 36, 169, 4, -1, 1100, 1500, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 100)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 100), new OuterAndInnerShorts(25, 25), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_119", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_119" }, 100));
		_dataArray.Add(new ArmorItem(414, 828, 1, 102, 0, 414, "icon_Armor_muhuwan", 829, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 163, 4, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_231", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_231" }, 100));
		_dataArray.Add(new ArmorItem(415, 830, 1, 102, 1, 414, "icon_Armor_muhuwan", 831, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 100, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 163, 4, -1, 245, 635, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 15), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_231", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_231" }, 100));
		_dataArray.Add(new ArmorItem(416, 832, 1, 102, 2, 414, "icon_Armor_muhuwan", 833, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 130, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 163, 4, -1, 285, 705, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_231", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_231" }, 100));
		_dataArray.Add(new ArmorItem(417, 834, 1, 102, 3, 414, "icon_Armor_xiangdiaobeihuan", 835, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 160, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 163, 4, -1, 325, 775, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 25), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_235", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_235" }, 100));
		_dataArray.Add(new ArmorItem(418, 836, 1, 102, 4, 414, "icon_Armor_xiangdiaobeihuan", 837, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 163, 4, -1, 370, 850, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 30), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_235", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_235" }, 100));
		_dataArray.Add(new ArmorItem(419, 838, 1, 102, 5, 414, "icon_Armor_xiangdiaobeihuan", 839, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 163, 4, -1, 410, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 35), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_235", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_235" }, 100));
	}

	private void CreateItems7()
	{
		_dataArray.Add(new ArmorItem(420, 840, 1, 102, 6, 414, "icon_Armor_dingyijiangmohuan", 841, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 163, 4, -1, 460, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 40), new OuterAndInnerShorts(20, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_239", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_239" }, 100));
		_dataArray.Add(new ArmorItem(421, 842, 1, 102, 7, 414, "icon_Armor_wuzhifoguang", 843, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 163, 4, -1, 505, 1075, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 45), new OuterAndInnerShorts(25, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_239", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_239" }, 100));
		_dataArray.Add(new ArmorItem(422, 844, 1, 102, 8, 414, "icon_Armor_yuanjuehuan", 845, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 310, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 163, 4, -1, 560, 1160, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 50), new OuterAndInnerShorts(30, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_239", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_239" }, 100));
		_dataArray.Add(new ArmorItem(423, 846, 1, 102, 0, 423, "icon_Armor_zhutiaotuo", 847, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 45, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 1, 36, 170, 4, -1, 265, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_211", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_211" }, 100));
		_dataArray.Add(new ArmorItem(424, 848, 1, 102, 1, 423, "icon_Armor_zhutiaotuo", 849, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 70, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 1, 36, 170, 4, -1, 300, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_211", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_211" }, 100));
		_dataArray.Add(new ArmorItem(425, 850, 1, 102, 2, 423, "icon_Armor_zhutiaotuo", 851, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 95, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 1, 36, 170, 4, -1, 335, 615, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 40), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_211", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_211" }, 100));
		_dataArray.Add(new ArmorItem(426, 852, 1, 102, 3, 423, "icon_Armor_hewenhushou", 853, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 120, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 1, 36, 170, 4, -1, 375, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 50), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_215", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_215" }, 100));
		_dataArray.Add(new ArmorItem(427, 854, 1, 102, 4, 423, "icon_Armor_hewenhushou", 855, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 1, 36, 170, 4, -1, 415, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 60), new OuterAndInnerShorts(0, 15), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_215", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_215" }, 100));
		_dataArray.Add(new ArmorItem(428, 856, 1, 102, 5, 423, "icon_Armor_hewenhushou", 857, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 165, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 1, 36, 170, 4, -1, 460, 800, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 75)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 70), new OuterAndInnerShorts(0, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_215", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_215" }, 100));
		_dataArray.Add(new ArmorItem(429, 858, 1, 102, 6, 423, "icon_Armor_sandiwanhuan", 859, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 190, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 1, 36, 170, 4, -1, 505, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 80)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 80), new OuterAndInnerShorts(0, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_219", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_219" }, 100));
		_dataArray.Add(new ArmorItem(430, 860, 1, 102, 7, 423, "icon_Armor_longhuachuan", 861, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 215, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 1, 36, 170, 4, -1, 550, 930, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 85)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 90), new OuterAndInnerShorts(0, 25), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_219", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_219" }, 100));
		_dataArray.Add(new ArmorItem(431, 862, 1, 102, 8, 423, "icon_Armor_wuzhuxin", 863, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 60, 240, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 1, 36, 170, 4, -1, 600, 1000, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 90)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 100), new OuterAndInnerShorts(0, 30), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_219", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_219" }, 100));
		_dataArray.Add(new ArmorItem(432, 864, 1, 102, 0, 432, "icon_Armor_shoupipibo", 865, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 166, 4, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(433, 866, 1, 102, 1, 432, "icon_Armor_shoupipibo", 867, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 65, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 166, 4, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(434, 868, 1, 102, 2, 432, "icon_Armor_shoupipibo", 869, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 70, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 166, 4, -1, 190, 470, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(435, 870, 1, 102, 3, 432, "icon_Armor_fengweibeijia", 871, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 75, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 166, 4, -1, 205, 505, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(436, 872, 1, 102, 4, 432, "icon_Armor_fengweibeijia", 873, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 166, 4, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(437, 874, 1, 102, 5, 432, "icon_Armor_fengweibeijia", 875, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 166, 4, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(438, 876, 1, 102, 6, 432, "icon_Armor_longweihushou", 877, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 166, 4, -1, 260, 620, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(439, 878, 1, 102, 7, 432, "icon_Armor_xuanfengxiu", 879, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 166, 4, -1, 275, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(440, 880, 1, 102, 8, 432, "icon_Armor_jiexingbeijia", 881, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 166, 4, -1, 300, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(441, 882, 1, 102, 0, 441, "icon_Armor_shupichanbi", 883, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 168, 4, -1, 180, 540, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(442, 884, 1, 102, 1, 441, "icon_Armor_shupichanbi", 885, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 168, 4, -1, 200, 590, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 0), new OuterAndInnerShorts(10, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(443, 886, 1, 102, 2, 441, "icon_Armor_shupichanbi", 887, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 168, 4, -1, 225, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(15, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(444, 888, 1, 102, 3, 441, "icon_Armor_mangwenyanbei", 889, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 168, 4, -1, 250, 700, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(445, 890, 1, 102, 4, 441, "icon_Armor_mangwenyanbei", 891, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 168, 4, -1, 270, 750, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(20, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(446, 892, 1, 102, 5, 441, "icon_Armor_mangwenyanbei", 893, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 168, 4, -1, 300, 810, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 0), new OuterAndInnerShorts(25, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(447, 894, 1, 102, 6, 441, "icon_Armor_kuiniubeijia", 895, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 168, 4, -1, 325, 865, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(448, 896, 1, 102, 7, 441, "icon_Armor_fuyunhubi", 897, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 170, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 168, 4, -1, 350, 920, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(55, 0), new OuterAndInnerShorts(30, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(449, 898, 1, 102, 8, 441, "icon_Armor_xinghehuaying", 899, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 180, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 168, 4, -1, 380, 980, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(35, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(450, 900, 1, 102, 0, 450, "icon_Armor_ruanpibeitao", 901, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 167, 4, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(451, 902, 1, 102, 1, 450, "icon_Armor_ruanpibeitao", 903, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 85, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 167, 4, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(452, 904, 1, 102, 2, 450, "icon_Armor_ruanpibeitao", 905, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 167, 4, -1, 205, 485, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(453, 906, 1, 102, 3, 450, "icon_Armor_baiyegebei", 907, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 95, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 167, 4, -1, 220, 520, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(454, 908, 1, 102, 4, 450, "icon_Armor_baiyegebei", 909, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 167, 4, -1, 240, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(455, 910, 1, 102, 5, 450, "icon_Armor_baiyegebei", 911, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 167, 4, -1, 255, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(456, 912, 1, 102, 6, 450, "icon_Armor_xuanlubaopibo", 913, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 167, 4, -1, 280, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(457, 914, 1, 102, 7, 450, "icon_Armor_duanyanxia", 915, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 167, 4, -1, 295, 675, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(458, 916, 1, 102, 8, 450, "icon_Armor_qifowanbei", 917, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 167, 4, -1, 320, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(0, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(459, 918, 1, 102, 0, 459, "icon_Armor_gedaihuwan", 919, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 165, 4, -1, 155, 395, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(20, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(460, 920, 1, 102, 1, 459, "icon_Armor_gedaihuwan", 921, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 165, 4, -1, 170, 430, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(30, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(461, 922, 1, 102, 2, 459, "icon_Armor_gedaihuwan", 923, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 165, 4, -1, 180, 460, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(40, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(462, 924, 1, 102, 3, 459, "icon_Armor_suiyunbeitao", 925, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 165, 4, -1, 195, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(50, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(463, 926, 1, 102, 4, 459, "icon_Armor_suiyunbeitao", 927, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 165, 4, -1, 210, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(60, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(464, 928, 1, 102, 5, 459, "icon_Armor_suiyunbeitao", 929, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 165, 4, -1, 220, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(70, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(465, 930, 1, 102, 6, 459, "icon_Armor_wusejianlingxiu", 931, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 165, 4, -1, 235, 595, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(80, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(466, 932, 1, 102, 7, 459, "icon_Armor_fuguangpowang", 933, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 165, 4, -1, 245, 625, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(90, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(467, 934, 1, 102, 8, 459, "icon_Armor_wanjianchanbi", 935, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 60, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 165, 4, -1, 260, 660, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(100, 0), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(468, 936, 1, 102, 0, 468, "icon_Armor_huangmahuwan", 937, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 173, 4, -1, 170, 410, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(469, 938, 1, 102, 1, 468, "icon_Armor_huangmahuwan", 939, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 173, 4, -1, 180, 440, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(470, 940, 1, 102, 2, 468, "icon_Armor_huangmahuwan", 941, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 173, 4, -1, 195, 475, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(20, 0, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(471, 942, 1, 102, 3, 468, "icon_Armor_cuozonghuwan", 943, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 173, 4, -1, 210, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(472, 944, 1, 102, 4, 468, "icon_Armor_cuozonghuwan", 945, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 173, 4, -1, 225, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(473, 946, 1, 102, 5, 468, "icon_Armor_cuozonghuwan", 947, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 173, 4, -1, 240, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(25, 0, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(474, 948, 1, 102, 6, 468, "icon_Armor_jiangxiechong", 949, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 173, 4, -1, 250, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(475, 950, 1, 102, 7, 468, "icon_Armor_qingxuanwan", 951, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 173, 4, -1, 265, 645, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(476, 952, 1, 102, 8, 468, "icon_Armor_juechenhuwan", 953, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 173, 4, -1, 280, 680, new List<PropertyAndValue>
		{
			new PropertyAndValue(3, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(30, 0, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(477, 954, 1, 102, 0, 477, "icon_Armor_sumaguoshou", 955, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 172, 4, -1, 145, 385, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(3, 20)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(478, 956, 1, 102, 1, 477, "icon_Armor_sumaguoshou", 957, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 172, 4, -1, 155, 415, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(3, 25)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
		_dataArray.Add(new ArmorItem(479, 958, 1, 102, 2, 477, "icon_Armor_sumaguoshou", 959, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 172, 4, -1, 170, 450, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(0, 0, 20, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_111", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_111" }, 100));
	}

	private void CreateItems8()
	{
		_dataArray.Add(new ArmorItem(480, 960, 1, 102, 3, 477, "icon_Armor_diwenduanxiu", 961, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 172, 4, -1, 180, 480, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(481, 962, 1, 102, 4, 477, "icon_Armor_diwenduanxiu", 963, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 172, 4, -1, 190, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(482, 964, 1, 102, 5, 477, "icon_Armor_diwenduanxiu", 965, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 172, 4, -1, 205, 545, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(0, 0, 25, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_115", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_115" }, 100));
		_dataArray.Add(new ArmorItem(483, 966, 1, 102, 6, 477, "icon_Armor_luhuaxixinxiu", 967, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 172, 4, -1, 215, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(484, 968, 1, 102, 7, 477, "icon_Armor_wuyaopibo", 969, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 172, 4, -1, 230, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(485, 970, 1, 102, 8, 477, "icon_Armor_chongxulun", 971, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 40, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 172, 4, -1, 240, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(0, 0, 30, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_119", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_119" }, 100));
		_dataArray.Add(new ArmorItem(486, 972, 1, 102, 0, 486, "icon_Armor_cumazhixiu", 973, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 174, 4, -1, 180, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 20),
			new PropertyAndValue(5, 20)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(487, 974, 1, 102, 1, 486, "icon_Armor_cumazhixiu", 975, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 105, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 174, 4, -1, 195, 455, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 25),
			new PropertyAndValue(5, 25)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(488, 976, 1, 102, 2, 486, "icon_Armor_cumazhixiu", 977, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 174, 4, -1, 215, 495, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 30),
			new PropertyAndValue(5, 30)
		}, new HitOrAvoidShorts(0, 20, 0, 0), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_121", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_121" }, 100));
		_dataArray.Add(new ArmorItem(489, 978, 1, 102, 3, 486, "icon_Armor_baicaolianhuwan", 979, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 115, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 174, 4, -1, 235, 535, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 35),
			new PropertyAndValue(5, 35)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(490, 980, 1, 102, 4, 486, "icon_Armor_baicaolianhuwan", 981, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 174, 4, -1, 255, 575, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 40),
			new PropertyAndValue(5, 40)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(491, 982, 1, 102, 5, 486, "icon_Armor_baicaolianhuwan", 983, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 125, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 174, 4, -1, 270, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 45),
			new PropertyAndValue(5, 45)
		}, new HitOrAvoidShorts(0, 25, 0, 0), new OuterAndInnerShorts(0, 70), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_125", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_125" }, 100));
		_dataArray.Add(new ArmorItem(492, 984, 1, 102, 6, 486, "icon_Armor_bingwenxianzaowan", 985, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 174, 4, -1, 295, 655, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 50),
			new PropertyAndValue(5, 50)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 80), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(493, 986, 1, 102, 7, 486, "icon_Armor_jiehailou", 987, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 135, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 174, 4, -1, 315, 695, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 55),
			new PropertyAndValue(5, 55)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 90), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(494, 988, 1, 102, 8, 486, "icon_Armor_xuanyuanwusebei", 989, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 174, 4, -1, 340, 740, new List<PropertyAndValue>
		{
			new PropertyAndValue(4, 60),
			new PropertyAndValue(5, 60)
		}, new HitOrAvoidShorts(0, 30, 0, 0), new OuterAndInnerShorts(0, 100), new OuterAndInnerShorts(0, 0), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_129", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_129" }, 100));
		_dataArray.Add(new ArmorItem(495, 990, 1, 102, 0, 495, "icon_Armor_huangmashuxiu", 991, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 4, 36, 175, 4, -1, 170, 530, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 20),
			new PropertyAndValue(4, 20)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 20), new OuterAndInnerShorts(0, 10), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(496, 992, 1, 102, 1, 495, "icon_Armor_huangmashuxiu", 993, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 90, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 4, 36, 175, 4, -1, 190, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 25),
			new PropertyAndValue(4, 25)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 25), new OuterAndInnerShorts(0, 10), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(497, 994, 1, 102, 2, 495, "icon_Armor_huangmashuxiu", 995, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 100, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 4, 36, 175, 4, -1, 210, 630, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 30), new OuterAndInnerShorts(0, 15), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_131", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_131" }, 100));
		_dataArray.Add(new ArmorItem(498, 996, 1, 102, 3, 495, "icon_Armor_xiangluohuwan", 997, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 110, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 4, 36, 175, 4, -1, 235, 685, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 35), new OuterAndInnerShorts(0, 20), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(499, 998, 1, 102, 4, 495, "icon_Armor_xiangluohuwan", 999, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 120, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 4, 36, 175, 4, -1, 255, 735, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 40), new OuterAndInnerShorts(0, 20), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(500, 1000, 1, 102, 5, 495, "icon_Armor_xiangluohuwan", 1001, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 130, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 4, 36, 175, 4, -1, 280, 790, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 45), new OuterAndInnerShorts(0, 25), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_135", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_135" }, 100));
		_dataArray.Add(new ArmorItem(501, 1002, 1, 102, 6, 495, "icon_Armor_shanyunwuxinxiu", 1003, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 140, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 4, 36, 175, 4, -1, 305, 845, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 50), new OuterAndInnerShorts(0, 30), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(502, 1004, 1, 102, 7, 495, "icon_Armor_wugehuaqi", 1005, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 150, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 4, 36, 175, 4, -1, 335, 905, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 55), new OuterAndInnerShorts(0, 30), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(503, 1006, 1, 102, 8, 495, "icon_Armor_tiannvwenxiu", 1007, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 40, 160, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 4, 36, 175, 4, -1, 360, 960, new List<PropertyAndValue>
		{
			new PropertyAndValue(1, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(0, 60), new OuterAndInnerShorts(0, 35), -1, new List<string> { "sleeve_l/sleeve_l_weapon", "sleeve_l/sleeve_l_139", "sleeve_r/sleeve_r_weapon", "sleeve_r/sleeve_r_139" }, 100));
		_dataArray.Add(new ArmorItem(504, 1008, 1, 102, 0, 504, "icon_Armor_manaohuwan", 1009, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 164, 4, -1, 540, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(3, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 5), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_431", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_431" }, 100));
		_dataArray.Add(new ArmorItem(505, 1010, 1, 102, 1, 504, "icon_Armor_manaohuwan", 1011, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 175, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 164, 4, -1, 600, 470, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(3, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(50, 10), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_431", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_431" }, 100));
		_dataArray.Add(new ArmorItem(506, 1012, 1, 102, 2, 504, "icon_Armor_manaohuwan", 1013, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 210, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 164, 4, -1, 665, 525, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(3, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 15), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_431", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_431" }, 100));
		_dataArray.Add(new ArmorItem(507, 1014, 1, 102, 3, 504, "icon_Armor_yuchanbeishu", 1015, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 245, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 164, 4, -1, 730, 580, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(3, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(70, 20), new OuterAndInnerShorts(10, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_435", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_435" }, 100));
		_dataArray.Add(new ArmorItem(508, 1016, 1, 102, 4, 504, "icon_Armor_yuchanbeishu", 1017, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 164, 4, -1, 800, 640, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(3, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 25), new OuterAndInnerShorts(15, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_435", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_435" }, 100));
		_dataArray.Add(new ArmorItem(509, 1018, 1, 102, 5, 504, "icon_Armor_yuchanbeishu", 1019, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 315, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 164, 4, -1, 865, 695, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(3, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(90, 30), new OuterAndInnerShorts(20, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_435", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_435" }, 100));
		_dataArray.Add(new ArmorItem(510, 1020, 1, 102, 6, 504, "icon_Armor_shichibuduo", 1021, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 350, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 164, 4, -1, 945, 765, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(3, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(100, 35), new OuterAndInnerShorts(20, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_439", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_439" }, 100));
		_dataArray.Add(new ArmorItem(511, 1022, 1, 102, 7, 504, "icon_Armor_yinyangzhidehuan", 1023, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 385, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 164, 4, -1, 1015, 825, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(3, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(110, 40), new OuterAndInnerShorts(25, 15), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_439", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_439" }, 100));
		_dataArray.Add(new ArmorItem(512, 1024, 1, 102, 8, 504, "icon_Armor_qiandengyijing", 1025, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 420, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 164, 4, -1, 1100, 900, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(3, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 45), new OuterAndInnerShorts(30, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_439", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_439" }, 100));
		_dataArray.Add(new ArmorItem(513, 1026, 1, 102, 0, 513, "icon_Armor_yuguoshou", 1027, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 80, 150, 300, 0, 2, 600, 3, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 45, isSpecial: false, 3, 36, 171, 4, -1, 575, 335, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 30),
			new PropertyAndValue(4, 30)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(5, 40), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_411", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_411" }, 100));
		_dataArray.Add(new ArmorItem(514, 1028, 1, 102, 1, 513, "icon_Armor_yuguoshou", 1029, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 110, 300, 600, 0, 4, 1200, 4, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 40, isSpecial: false, 3, 36, 171, 4, -1, 635, 375, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 35),
			new PropertyAndValue(4, 35)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(10, 50), new OuterAndInnerShorts(0, 0), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_411", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_411" }, 100));
		_dataArray.Add(new ArmorItem(515, 1030, 1, 102, 2, 513, "icon_Armor_yuguoshou", 1031, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 140, 900, 1800, 1, 6, 1800, 5, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 35, isSpecial: false, 3, 36, 171, 4, -1, 700, 420, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 40),
			new PropertyAndValue(4, 40)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(15, 60), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_411", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_411" }, 100));
		_dataArray.Add(new ArmorItem(516, 1032, 1, 102, 3, 513, "icon_Armor_jingyunbeidang", 1033, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 165, 2250, 4500, 2, 8, 3000, 6, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 30, isSpecial: false, 3, 36, 171, 4, -1, 765, 465, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 45),
			new PropertyAndValue(4, 45)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(20, 70), new OuterAndInnerShorts(0, 10), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_415", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_415" }, 100));
		_dataArray.Add(new ArmorItem(517, 1034, 1, 102, 4, 513, "icon_Armor_jingyunbeidang", 1035, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 195, 4650, 9300, 3, 10, 4200, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 25, isSpecial: false, 3, 36, 171, 4, -1, 830, 510, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 50),
			new PropertyAndValue(4, 50)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(25, 80), new OuterAndInnerShorts(0, 15), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_415", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_415" }, 100));
		_dataArray.Add(new ArmorItem(518, 1036, 1, 102, 5, 513, "icon_Armor_jingyunbeidang", 1037, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 220, 8400, 16800, 4, 12, 5400, 7, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 20, isSpecial: false, 3, 36, 171, 4, -1, 900, 560, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 55),
			new PropertyAndValue(4, 55)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(30, 90), new OuterAndInnerShorts(10, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_415", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_415" }, 100));
		_dataArray.Add(new ArmorItem(519, 1038, 1, 102, 6, 513, "icon_Armor_wuzhenhuan", 1039, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 250, 13800, 27600, 5, 14, 7200, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 15, isSpecial: false, 3, 36, 171, 4, -1, 970, 610, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 60),
			new PropertyAndValue(4, 60)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(35, 100), new OuterAndInnerShorts(10, 20), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_419", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_419" }, 100));
		_dataArray.Add(new ArmorItem(520, 1040, 1, 102, 7, 513, "icon_Armor_jinggouhuan", 1041, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 280, 21150, 42300, 6, 16, 9000, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 10, isSpecial: false, 3, 36, 171, 4, -1, 1045, 665, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 65),
			new PropertyAndValue(4, 65)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(40, 110), new OuterAndInnerShorts(15, 25), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_419", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_419" }, 100));
		_dataArray.Add(new ArmorItem(521, 1042, 1, 102, 8, 513, "icon_Armor_xiyihuan", 1043, transferable: true, stackable: false, wagerable: true, refinable: true, poisonable: true, repairable: true, inheritable: true, detachable: true, 80, 305, 30750, 61500, 6, 18, 10800, 8, allowRandomCreate: true, allowRawCreate: true, allowCrippledCreate: true, 5, isSpecial: false, 3, 36, 171, 4, -1, 1120, 720, new List<PropertyAndValue>
		{
			new PropertyAndValue(2, 70),
			new PropertyAndValue(4, 70)
		}, new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(45, 120), new OuterAndInnerShorts(20, 30), -1, new List<string> { "equip_elbow_l", "equip_elbow_l/equip_elbow_l_419", "equip_elbow_r", "equip_elbow_r/equip_elbow_r_419" }, 100));
		_dataArray.Add(new ArmorItem(522, 1044, 1, 104, 3, -1, "icon_Armor_houpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 300, 400, new List<PropertyAndValue>(), new HitOrAvoidShorts(40, 0, 0, 0), new OuterAndInnerShorts(50, 20), new OuterAndInnerShorts(15, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(523, 1044, 1, 104, 3, -1, "icon_Armor_houpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 300, 400, new List<PropertyAndValue>(), new HitOrAvoidShorts(40, 0, 0, 0), new OuterAndInnerShorts(50, 20), new OuterAndInnerShorts(15, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(524, 1044, 1, 104, 3, -1, "icon_Armor_houpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 300, 400, new List<PropertyAndValue>(), new HitOrAvoidShorts(40, 0, 0, 0), new OuterAndInnerShorts(50, 20), new OuterAndInnerShorts(15, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(525, 1044, 1, 104, 3, -1, "icon_Armor_houpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 300, 400, new List<PropertyAndValue>(), new HitOrAvoidShorts(40, 0, 0, 0), new OuterAndInnerShorts(50, 20), new OuterAndInnerShorts(15, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(526, 1046, 1, 104, 6, -1, "icon_Armor_gangpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 100, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 600, 800, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(100, 40), new OuterAndInnerShorts(30, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(527, 1046, 1, 104, 6, -1, "icon_Armor_gangpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 100, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 600, 800, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(100, 40), new OuterAndInnerShorts(30, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(528, 1046, 1, 104, 6, -1, "icon_Armor_gangpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 100, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 600, 800, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(100, 40), new OuterAndInnerShorts(30, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(529, 1046, 1, 104, 6, -1, "icon_Armor_gangpi", 1045, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 100, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 600, 800, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(100, 40), new OuterAndInnerShorts(30, 0), -1, null, 100));
		_dataArray.Add(new ArmorItem(530, 1047, 1, 104, 3, -1, "icon_Armor_houyu", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 40, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 200, 300, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 40, 0), new OuterAndInnerShorts(20, 50), new OuterAndInnerShorts(0, 15), -1, null, 100));
		_dataArray.Add(new ArmorItem(531, 1047, 1, 104, 3, -1, "icon_Armor_houyu", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 40, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 200, 300, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 40, 0), new OuterAndInnerShorts(20, 50), new OuterAndInnerShorts(0, 15), -1, null, 100));
		_dataArray.Add(new ArmorItem(532, 1047, 1, 104, 3, -1, "icon_Armor_houyu", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 40, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 200, 300, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 40, 0), new OuterAndInnerShorts(20, 50), new OuterAndInnerShorts(0, 15), -1, null, 100));
		_dataArray.Add(new ArmorItem(533, 1047, 1, 104, 3, -1, "icon_Armor_houyu", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 40, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 200, 300, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 40, 0), new OuterAndInnerShorts(20, 50), new OuterAndInnerShorts(0, 15), -1, null, 100));
		_dataArray.Add(new ArmorItem(534, 1049, 1, 104, 6, -1, "icon_Armor_jinling", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 400, 600, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(40, 100), new OuterAndInnerShorts(0, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(535, 1049, 1, 104, 6, -1, "icon_Armor_jinling", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 400, 600, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(40, 100), new OuterAndInnerShorts(0, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(536, 1049, 1, 104, 6, -1, "icon_Armor_jinling", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 400, 600, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(40, 100), new OuterAndInnerShorts(0, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(537, 1049, 1, 104, 6, -1, "icon_Armor_jinling", 1048, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 400, 600, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(40, 100), new OuterAndInnerShorts(0, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(538, 1050, 1, 104, 3, -1, "icon_Armor_houlin", 1051, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 400, 350, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 40, 0, 0), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(10, 10), -1, null, 100));
		_dataArray.Add(new ArmorItem(539, 1050, 1, 104, 3, -1, "icon_Armor_houlin", 1051, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 60, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 400, 350, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 40, 0, 0), new OuterAndInnerShorts(40, 40), new OuterAndInnerShorts(10, 10), -1, null, 100));
	}

	private void CreateItems9()
	{
		_dataArray.Add(new ArmorItem(540, 1052, 1, 104, 6, -1, "icon_Armor_xuanlin", 1051, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 800, 700, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 60, 0, 0), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), -1, null, 100));
		_dataArray.Add(new ArmorItem(541, 1052, 1, 104, 6, -1, "icon_Armor_xuanlin", 1051, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 800, 700, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 60, 0, 0), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(20, 20), -1, null, 100));
		_dataArray.Add(new ArmorItem(542, 1053, 1, 104, 8, -1, "icon_Armor_linglintou", 1054, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(543, 1053, 1, 104, 8, -1, "icon_Armor_linglin", 1055, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(60, 0, 0, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(544, 1053, 1, 104, 8, -1, "icon_Armor_linglintou", 1054, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 60, 0, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(545, 1053, 1, 104, 8, -1, "icon_Armor_linglin", 1055, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 60, 0, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(546, 1053, 1, 104, 8, -1, "icon_Armor_linglintou", 1054, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(547, 1053, 1, 104, 8, -1, "icon_Armor_linglin", 1055, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 60, 0), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(548, 1053, 1, 104, 8, -1, "icon_Armor_linglintou", 1054, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 0, 60), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(549, 1053, 1, 104, 8, -1, "icon_Armor_linglin", 1055, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1600, 1400, new List<PropertyAndValue>(), new HitOrAvoidShorts(0, 0, 0, 60), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(550, 1056, 1, 104, 8, -1, "icon_Armor_longlintou", 1057, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 240, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 2200, 2200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(35, 35), -1, null, 100));
		_dataArray.Add(new ArmorItem(551, 1056, 1, 104, 8, -1, "icon_Armor_longlinqu", 1058, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 240, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 2200, 2200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(35, 35), -1, null, 100));
		_dataArray.Add(new ArmorItem(552, 1056, 1, 104, 8, -1, "icon_Armor_longlinshou", 1059, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 240, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 2200, 2200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(35, 35), -1, null, 100));
		_dataArray.Add(new ArmorItem(553, 1056, 1, 104, 8, -1, "icon_Armor_longlinzu", 1060, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 240, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 2200, 2200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(80, 80), new OuterAndInnerShorts(35, 35), -1, null, 100));
		_dataArray.Add(new ArmorItem(554, 1061, 1, 104, 8, -1, "icon_Armor_xiaolongtou", 1062, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 120, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1800, 1800, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(555, 1061, 1, 104, 8, -1, "icon_Armor_xiaolongqu", 1063, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 120, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1800, 1800, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(556, 1061, 1, 104, 8, -1, "icon_Armor_xiaolongshou", 1064, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 120, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 4, -1, 1800, 1800, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(557, 1061, 1, 104, 8, -1, "icon_Armor_xiaolongzu", 1065, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 120, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 5, -1, 1800, 1800, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(30, 30), -1, null, 100));
		_dataArray.Add(new ArmorItem(558, 1066, 1, 104, 8, -1, "icon_Armor_jiaoyitou", 1067, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 1, -1, 1200, 1200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(20, 20), -1, null, 100));
		_dataArray.Add(new ArmorItem(559, 1066, 1, 104, 8, -1, "icon_Armor_jiaoyiqu", 1068, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 80, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 1200, 1200, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(60, 60), new OuterAndInnerShorts(20, 20), -1, null, 100));
		_dataArray.Add(new ArmorItem(560, 1069, 1, 104, 8, -1, "icon_Armor_xuanke", 1070, transferable: false, stackable: false, wagerable: false, refinable: false, poisonable: false, repairable: false, inheritable: true, detachable: false, 160, 0, 0, 0, 0, 0, 0, 8, allowRandomCreate: false, allowRawCreate: false, allowCrippledCreate: false, 0, isSpecial: true, -1, 12, -1, 3, -1, 2600, 2600, new List<PropertyAndValue>(), new HitOrAvoidShorts(default(short), default(short), default(short), default(short)), new OuterAndInnerShorts(120, 120), new OuterAndInnerShorts(30, 30), -1, null, 100));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<ArmorItem>(561);
		CreateItems0();
		CreateItems1();
		CreateItems2();
		CreateItems3();
		CreateItems4();
		CreateItems5();
		CreateItems6();
		CreateItems7();
		CreateItems8();
		CreateItems9();
	}
}
