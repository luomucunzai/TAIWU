using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class AiParam : ConfigData<AiParamItem, int>
{
	public static AiParam Instance = new AiParam();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Type", "Name", "Desc" };

	internal override int ToInt(int value)
	{
		return value;
	}

	internal override int ToTemplateId(int value)
	{
		return value;
	}

	private void CreateItems0()
	{
		_dataArray.Add(new AiParamItem(0, EAiParamType.Int, 0, 1, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(1, EAiParamType.Bool, 2, 3, new int[2] { 4, 5 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(2, EAiParamType.CombatSkill, 6, 7, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(3, EAiParamType.IsAlly, 8, 9, new int[2] { 10, 11 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(4, EAiParamType.String, 12, 13, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(5, EAiParamType.CombatDifficulty, 14, 15, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(6, EAiParamType.TeammateCommand, 16, 17, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(7, EAiParamType.ProactiveSkillType, 18, 19, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(8, EAiParamType.OtherActionType, 20, 21, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(9, EAiParamType.IsForward, 22, 23, new int[2] { 24, 25 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(10, EAiParamType.BodyPartType, 26, 27, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(11, EAiParamType.Expression, 28, 29, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(12, EAiParamType.IsDirect, 30, 31, new int[2] { 32, 33 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(13, EAiParamType.IsInner, 34, 35, new int[2] { 36, 37 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(14, EAiParamType.PoisonType, 38, 39, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(15, EAiParamType.IsNotOnlyInCombat, 40, 41, new int[2] { 42, 43 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(16, EAiParamType.IsGood, 44, 45, new int[2] { 46, 47 }, new string[2] { "True", "False" }));
		_dataArray.Add(new AiParamItem(17, EAiParamType.WugType, 48, 49, new int[8] { 50, 51, 52, 53, 54, 55, 56, 57 }, new string[8] { "0", "1", "2", "3", "4", "5", "6", "7" }));
		_dataArray.Add(new AiParamItem(18, EAiParamType.NeiliAllocationType, 58, 59, new int[4] { 60, 61, 62, 63 }, new string[4] { "0", "1", "2", "3" }));
		_dataArray.Add(new AiParamItem(19, EAiParamType.TrickType, 64, 65, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(20, EAiParamType.Weapon, 66, 67, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(21, EAiParamType.WeaponSubType, 68, 69, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(22, EAiParamType.FiveElementsType, 70, 71, new int[6] { 72, 73, 74, 75, 76, 77 }, new string[6] { "0", "1", "2", "3", "4", "5" }));
		_dataArray.Add(new AiParamItem(23, EAiParamType.CombatType, 78, 79, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(24, EAiParamType.CombatStateName, 80, 81, new int[0], new string[1] { "" }));
		_dataArray.Add(new AiParamItem(25, EAiParamType.Misc, 82, 83, new int[0], new string[1] { "" }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<AiParamItem>(26);
		CreateItems0();
	}
}
