using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SolarTerm : ConfigData<SolarTermItem, sbyte>
{
	public static SolarTerm Instance = new SolarTerm();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"Month", "MaterialIdsOfFoodBuff", "PoisonBuffType", "DetoxBuffType", "TemplateId", "Name", "Type", "Desc", "Poem", "Image",
		"Sound", "FiveElementsTypesOfCombatSkillBuff"
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
		_dataArray.Add(new SolarTermItem(0, 0, 0, 1, 2, "TurnChange_3", "se_solarterm_0", 1, new List<byte> { 1, 4 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 5, 4, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(1, 3, 1, 4, 5, "TurnChange_4", "se_solarterm_1", 1, new List<byte> { 1 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 5, 4, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(2, 6, 0, 7, 8, "TurnChange_5", "se_solarterm_2", 2, new List<byte> { 1 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 1, 0, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(3, 9, 1, 10, 11, "TurnChange_6", "se_solarterm_3", 2, new List<byte> { 1, 4 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 1, 0, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(4, 12, 0, 13, 14, "TurnChange_7", "se_solarterm_4", 3, new List<byte> { 1 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 1, 0, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(5, 15, 1, 16, 17, "TurnChange_8", "se_solarterm_5", 3, new List<byte> { 1 }, new List<short> { 70, 71, 72, 73, 74, 75, 76 }, 1, 0, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: true, healthBuff: false));
		_dataArray.Add(new SolarTermItem(6, 18, 0, 19, 20, "TurnChange_9", "se_solarterm_6", 4, new List<byte> { 3, 4 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 0, 1, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(7, 21, 1, 22, 23, "TurnChange_10", "se_solarterm_7", 4, new List<byte> { 3 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 0, 1, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(8, 24, 0, 25, 26, "TurnChange_11", "se_solarterm_8", 5, new List<byte> { 3 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 0, 1, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(9, 27, 1, 28, 29, "TurnChange_12", "se_solarterm_9", 5, new List<byte> { 3, 4 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 0, 1, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(10, 30, 0, 31, 32, "TurnChange_13", "se_solarterm_10", 6, new List<byte> { 3 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 3, 2, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(11, 33, 1, 34, 35, "TurnChange_14", "se_solarterm_11", 6, new List<byte> { 3 }, new List<short> { 56, 57, 58, 59, 60, 61, 62 }, 3, 2, outerHealingBuff: false, innerHealingBuff: true, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(12, 36, 0, 37, 38, "TurnChange_15", "se_solarterm_12", 7, new List<byte> { 0, 4 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 3, 2, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(13, 39, 1, 40, 41, "TurnChange_16", "se_solarterm_13", 7, new List<byte> { 0 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 3, 2, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(14, 42, 0, 43, 44, "TurnChange_17", "se_solarterm_14", 8, new List<byte> { 0 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 4, 5, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(15, 45, 1, 46, 47, "TurnChange_18", "se_solarterm_15", 8, new List<byte> { 0, 4 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 4, 5, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(16, 48, 0, 49, 50, "TurnChange_19", "se_solarterm_16", 9, new List<byte> { 0 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 4, 5, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(17, 51, 1, 52, 53, "TurnChange_20", "se_solarterm_17", 9, new List<byte> { 0 }, new List<short> { 77, 78, 79, 80, 81, 82, 83 }, 4, 5, outerHealingBuff: true, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: false));
		_dataArray.Add(new SolarTermItem(18, 54, 0, 55, 56, "TurnChange_21", "se_solarterm_18", 10, new List<byte> { 2, 4 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 2, 3, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
		_dataArray.Add(new SolarTermItem(19, 57, 1, 58, 59, "TurnChange_22", "se_solarterm_19", 10, new List<byte> { 2 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 2, 3, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
		_dataArray.Add(new SolarTermItem(20, 60, 0, 61, 62, "TurnChange_23", "se_solarterm_20", 11, new List<byte> { 2 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 2, 3, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
		_dataArray.Add(new SolarTermItem(21, 63, 1, 64, 65, "TurnChange_24", "se_solarterm_21", 11, new List<byte> { 2, 4 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 2, 3, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
		_dataArray.Add(new SolarTermItem(22, 66, 0, 67, 68, "TurnChange_1", "se_solarterm_22", 0, new List<byte> { 2 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 5, 4, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
		_dataArray.Add(new SolarTermItem(23, 69, 1, 70, 71, "TurnChange_2", "se_solarterm_23", 0, new List<byte> { 2 }, new List<short> { 63, 64, 65, 66, 67, 68, 69 }, 5, 4, outerHealingBuff: false, innerHealingBuff: false, qiDisorderRecoveringBuff: false, healthBuff: true));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SolarTermItem>(24);
		CreateItems0();
	}
}
