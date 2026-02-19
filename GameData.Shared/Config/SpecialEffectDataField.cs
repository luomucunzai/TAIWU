using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class SpecialEffectDataField : ConfigData<SpecialEffectDataFieldItem, short>
{
	public static SpecialEffectDataField Instance = new SpecialEffectDataField();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "FieldName", "DisplayFormat" };

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
		_dataArray.Add(new SpecialEffectDataFieldItem(0, 0, "HitStrength", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(1, 1, "HitTechnique", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(2, 2, "HitSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(3, 3, "HitMind", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(4, 4, "PenetrateOuter", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(5, 5, "PenetrateInner", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(6, 6, "AvoidStrength", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(7, 7, "AvoidTechnique", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(8, 8, "AvoidSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(9, 9, "AvoidMind", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(10, 10, "PenetrateResistOuter", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(11, 11, "PenetrateResistInner", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(12, 12, "RecoveryOfStance", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(13, 13, "RecoveryOfBreath", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(14, 14, "MoveSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(15, 15, "RecoveryOfFlaw", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(16, 16, "CastSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(17, 17, "RecoveryOfBlockedAcupoint", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(18, 18, "WeaponSwitchSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(19, 19, "AttackSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(20, 20, "InnerRatio", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(21, 21, "RecoveryOfQiDisorder", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(22, 22, "ResistOfHotPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(23, 23, "ResistOfGloomyPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(24, 24, "ResistOfColdPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(25, 25, "ResistOfRedPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(26, 26, "ResistOfRottenPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(27, 27, "ResistOfIllusoryPoison", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(28, 28, "MakeDirectDamage", new int[3] { 0, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(29, 29, "MakeDirectDamage", new int[3] { 1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(30, 30, "Happiness", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(31, 31, "AddNeiliAllocation", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(32, 32, "MobilityRecoverSpeed", new int[3] { -1, -1, -1 }, -1, null));
		_dataArray.Add(new SpecialEffectDataFieldItem(33, 33, "AttackRangeForward", new int[3] { -1, -1, -1 }, 10, "F1"));
		_dataArray.Add(new SpecialEffectDataFieldItem(34, 34, "AttackRangeBackward", new int[3] { -1, -1, -1 }, 10, "F1"));
		_dataArray.Add(new SpecialEffectDataFieldItem(35, 35, "PersonalitiesAll", new int[3] { -1, -1, -1 }, -1, null));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<SpecialEffectDataFieldItem>(36);
		CreateItems0();
	}
}
