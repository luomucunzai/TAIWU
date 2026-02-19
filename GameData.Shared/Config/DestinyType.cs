using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DestinyType : ConfigData<DestinyTypeItem, sbyte>
{
	public static DestinyType Instance = new DestinyType();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"Feature", "MotherLifeRecord", "SectList", "TemplateId", "Name", "Desc", "MoralityRange", "OrganizationGradeRange", "RecordColor", "UnlockResourceTypeIcon",
		"UnlockCost", "LockedIcon", "UnlockedIcon"
	};

	internal override int ToInt(sbyte value)
	{
		return value;
	}

	internal override sbyte ToTemplateId(int value)
	{
		return (sbyte)value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new DestinyTypeItem(0, 0, 1, 215, new short[2] { -500, -125 }, 401, new List<sbyte> { 1, 2, 3, 4, 5 }, new sbyte[2] { 0, 2 }, "lightred", 2, new ushort[6] { 50000, 0, 0, 0, 0, 0 }, "building_liudao_lock_1_2", "building_liudao_1_2"));
		_dataArray.Add(new DestinyTypeItem(1, 3, 4, 214, new short[2] { -500, -125 }, 402, new List<sbyte> { 6, 7, 8, 9, 10 }, new sbyte[2] { 1, 3 }, "lightred", 5, new ushort[6] { 0, 0, 0, 0, 0, 50000 }, "building_liudao_lock_1_1", "building_liudao_1_1"));
		_dataArray.Add(new DestinyTypeItem(2, 6, 7, 213, new short[2] { -500, 124 }, 403, new List<sbyte> { 11, 12, 13, 14, 15 }, new sbyte[2] { 2, 4 }, "lightred", 8, new ushort[6] { 0, 0, 50000, 0, 0, 0 }, "building_liudao_lock_1_0", "building_liudao_1_0"));
		_dataArray.Add(new DestinyTypeItem(3, 9, 10, 212, new short[2] { -124, 500 }, 404, new List<sbyte> { 11, 12, 13, 14, 15 }, new sbyte[2] { 4, 6 }, "lightblue", 11, new ushort[6] { 0, 0, 0, 0, 50000, 0 }, "building_liudao_lock_0_2", "building_liudao_0_2"));
		_dataArray.Add(new DestinyTypeItem(4, 12, 13, 211, new short[2] { 125, 500 }, 405, new List<sbyte> { 6, 7, 8, 9, 10 }, new sbyte[2] { 5, 7 }, "lightblue", 14, new ushort[6] { 0, 50000, 0, 0, 0, 0 }, "building_liudao_lock_0_1", "building_liudao_0_1"));
		_dataArray.Add(new DestinyTypeItem(5, 15, 16, 210, new short[2] { 125, 500 }, 406, new List<sbyte> { 1, 2, 3, 4, 5 }, new sbyte[2] { 6, 8 }, "lightblue", 17, new ushort[6] { 0, 0, 0, 50000, 0, 0 }, "building_liudao_lock_0_0", "building_liudao_0_0"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DestinyTypeItem>(6);
		CreateItems0();
	}
}
