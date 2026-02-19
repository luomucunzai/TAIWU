using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class LifeSkillCombatEffect : ConfigData<LifeSkillCombatEffectItem, sbyte>
{
	public static LifeSkillCombatEffect Instance = new LifeSkillCombatEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"BanCardList", "TemplateId", "Name", "Desc", "BaseAmount", "MaxAmount", "UsedCount", "Level", "Type", "Group",
		"BehaviorTypeAmounts", "SubEffect", "SubEffectParameters", "Imgae"
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
		_dataArray.Add(new LifeSkillCombatEffectItem(0, 0, 1, 3, 3, 2, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.FlexibleFall, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			1, 2, 18, 20, 19, 7, 15, 17, 16, 21,
			23
		}, new sbyte[5] { 0, 0, 3, 0, 0 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisLow, new sbyte[5] { -1, 1, 1, 0, 0 }, "lifeskillcombat_chahua_0"));
		_dataArray.Add(new LifeSkillCombatEffectItem(1, 2, 3, 3, 3, 2, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.FlexibleFall, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			0, 2, 18, 20, 19, 7, 15, 17, 16, 21,
			23
		}, new sbyte[5] { 3, 3, 0, 0, 0 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh, new sbyte[5] { 1, 127, 0, 0, 0 }, "lifeskillcombat_chahua_1"));
		_dataArray.Add(new LifeSkillCombatEffectItem(2, 4, 5, 3, 3, 2, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.FlexibleFall, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			1, 0, 18, 20, 19, 7, 15, 17, 16, 21,
			23
		}, new sbyte[5] { 0, 0, 0, 3, 3 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfExtraQuestionAroundHouseThesisBreakWhenAdversaryThesisHigh, new sbyte[5] { 1, 0, 127, 0, 0 }, "lifeskillcombat_chahua_2"));
		_dataArray.Add(new LifeSkillCombatEffectItem(3, 24, 25, 1, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.DissembleArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 13, 14, 12, 4, 5, 21, 23 }, new sbyte[5] { 0, 0, 3, 0, 0 }, new sbyte[5] { 30, 0, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfTrapedInCell, new sbyte[5] { 1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_3"));
		_dataArray.Add(new LifeSkillCombatEffectItem(4, 26, 27, 1, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.DissembleArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 13, 14, 12, 3, 5, 21, 23 }, new sbyte[5] { 0, 0, 3, 0, 0 }, new sbyte[5] { 30, 0, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion, new sbyte[5] { 1, 0, 0, 3, 0 }, "lifeskillcombat_chahua_4"));
		_dataArray.Add(new LifeSkillCombatEffectItem(5, 28, 29, 2, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.DissembleArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 13, 14, 12, 3, 4, 21, 23 }, new sbyte[5] { 0, 0, 3, 0, 0 }, new sbyte[5] { 30, 0, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion, new sbyte[5] { 1, 0, 0, -3, 0 }, "lifeskillcombat_chahua_5"));
		_dataArray.Add(new LifeSkillCombatEffectItem(6, 6, 7, 2, 3, 1, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.ReinforceArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: true, isDecideAddEffect: false, isGetPointAddEffect: true, new List<sbyte> { 8, 7, 10, 11, 9 }, new sbyte[5] { 3, 3, 0, 0, 0 }, new sbyte[5] { 0, 0, 0, 0, 30 }, ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam, new sbyte[5] { 1, 1, 1, 10, 0 }, "lifeskillcombat_chahua_6"));
		_dataArray.Add(new LifeSkillCombatEffectItem(7, 8, 9, 2, 3, 2, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.ReinforceArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			0, 1, 2, 18, 19, 15, 17, 6, 8, 10,
			11, 9
		}, new sbyte[5] { 3, 3, 0, 0, 0 }, new sbyte[5] { 0, 0, 0, 0, 30 }, ELifeSkillCombatEffectSubEffect.PointChange, new sbyte[5] { 0, 0, 0, 20, 0 }, "lifeskillcombat_chahua_7"));
		_dataArray.Add(new LifeSkillCombatEffectItem(8, 10, 11, 2, 3, 1, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.ReinforceArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: true, isDecideAddEffect: false, isGetPointAddEffect: true, new List<sbyte> { 6, 7, 10, 11, 9 }, new sbyte[5] { 3, 3, 0, 0, 0 }, new sbyte[5] { 0, 0, 0, 0, 30 }, ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam, new sbyte[5] { 1, 127, 0, 10, 0 }, "lifeskillcombat_chahua_8"));
		_dataArray.Add(new LifeSkillCombatEffectItem(9, 12, 13, 2, 3, 2, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.WeakenArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 6, 8, 7, 10, 11 }, new sbyte[5] { 0, 0, 0, 3, 3 }, new sbyte[5] { 0, 0, 0, 30, 0 }, ELifeSkillCombatEffectSubEffect.SelfTrapedInCell, new sbyte[5] { -1, 0, 0, -20, 0 }, "lifeskillcombat_chahua_9"));
		_dataArray.Add(new LifeSkillCombatEffectItem(10, 14, 15, 2, 3, 1, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.WeakenArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: true, isDecideAddEffect: false, isGetPointAddEffect: true, new List<sbyte> { 6, 8, 7, 11, 9 }, new sbyte[5] { 0, 0, 0, 3, 3 }, new sbyte[5] { 0, 0, 0, 30, 0 }, ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam, new sbyte[5] { -1, 1, 1, -10, 0 }, "lifeskillcombat_chahua_10"));
		_dataArray.Add(new LifeSkillCombatEffectItem(11, 16, 17, 2, 3, 1, 0, ELifeSkillCombatEffectType.BUFF, ELifeSkillCombatEffectGroup.WeakenArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: true, isDecideAddEffect: false, isGetPointAddEffect: true, new List<sbyte> { 6, 8, 7, 10, 9 }, new sbyte[5] { 0, 0, 0, 3, 3 }, new sbyte[5] { 0, 0, 0, 30, 0 }, ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam, new sbyte[5] { -1, 127, 0, -10, 0 }, "lifeskillcombat_chahua_11"));
		_dataArray.Add(new LifeSkillCombatEffectItem(12, 30, 31, 1, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.RemoveEffect, isInstant: true, isSelectGrid: true, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 13, 14, 3, 4, 5 }, new sbyte[5] { 0, 0, 2, 0, 0 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfEraseAroundHouseQuestionEffects, new sbyte[5] { -1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_12"));
		_dataArray.Add(new LifeSkillCombatEffectItem(13, 32, 33, 1, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.RemoveEffect, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 14, 12, 3, 4, 5, 21, 23 }, new sbyte[5] { 0, 0, 2, 0, 0 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow, new sbyte[5] { -1, 1, 1, 0, 0 }, "lifeskillcombat_chahua_13"));
		_dataArray.Add(new LifeSkillCombatEffectItem(14, 34, 35, 2, 2, 1, 1, ELifeSkillCombatEffectType.Strategy, ELifeSkillCombatEffectGroup.RemoveEffect, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 13, 12, 3, 4, 5, 21, 23 }, new sbyte[5] { 0, 0, 2, 0, 0 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow, new sbyte[5] { -1, 127, 0, 0, 0 }, "lifeskillcombat_chahua_14"));
		_dataArray.Add(new LifeSkillCombatEffectItem(15, 42, 43, 0, 1, 0, 2, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.ExtractCard, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 18, 20, 19, 17, 16, 21, 23 }, new sbyte[5] { 2, 2, 0, 0, 0 }, new sbyte[5] { 0, 30, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount, new sbyte[5] { 1, 1, 1, 0, 0 }, "lifeskillcombat_chahua_15"));
		_dataArray.Add(new LifeSkillCombatEffectItem(16, 44, 45, 1, 1, 1, 2, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.ExtractCard, isInstant: true, isSelectGrid: true, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 18, 20, 19, 15, 17 }, new sbyte[5] { 2, 2, 0, 0, 0 }, new sbyte[5] { 0, 30, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint, new sbyte[5] { 1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_16"));
		_dataArray.Add(new LifeSkillCombatEffectItem(17, 46, 47, 0, 1, 0, 2, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.ExtractCard, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 18, 20, 19, 15, 16, 21, 23 }, new sbyte[5] { 2, 2, 0, 0, 0 }, new sbyte[5] { 0, 30, 0, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount, new sbyte[5] { 1, 127, 0, 0, 0 }, "lifeskillcombat_chahua_17"));
		_dataArray.Add(new LifeSkillCombatEffectItem(18, 18, 19, 2, 3, 1, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.EliminateArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 20, 19, 15, 17, 16, 21, 23 }, new sbyte[5] { 0, 0, 0, 2, 2 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow, new sbyte[5] { -1, 1, 1, 0, 0 }, "lifeskillcombat_chahua_18"));
		_dataArray.Add(new LifeSkillCombatEffectItem(19, 20, 21, 2, 3, 2, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.EliminateArgument, isInstant: true, isSelectGrid: true, isSelectBook: false, isSaveCard: false, isDecideAddEffect: true, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 18, 20, 15, 17, 16 }, new sbyte[5] { 0, 0, 0, 2, 2 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfEraseAroundSelfThesisHouseQuestionThesis, new sbyte[5] { -1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_19"));
		_dataArray.Add(new LifeSkillCombatEffectItem(20, 22, 23, 2, 3, 1, 0, ELifeSkillCombatEffectType.Common, ELifeSkillCombatEffectGroup.EliminateArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 0, 1, 2, 18, 19, 15, 17, 16, 21, 23 }, new sbyte[5] { 0, 0, 0, 2, 2 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow, new sbyte[5] { -1, 127, 0, 0, 0 }, "lifeskillcombat_chahua_20"));
		_dataArray.Add(new LifeSkillCombatEffectItem(21, 48, 49, 1, 1, 1, 2, ELifeSkillCombatEffectType.Special, ELifeSkillCombatEffectGroup.RecycleCard, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			0, 1, 2, 18, 20, 15, 17, 7, 13, 14,
			3, 4, 5, 22, 23
		}, new sbyte[5] { 0, 0, 1, 0, 0 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndRecycleCardAndExchangeOperation, new sbyte[5] { 1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_21"));
		_dataArray.Add(new LifeSkillCombatEffectItem(22, 50, 51, 0, 1, 0, 2, ELifeSkillCombatEffectType.Special, ELifeSkillCombatEffectGroup.QuickArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 21, 23 }, new sbyte[5] { 1, 1, 0, 0, 0 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates, new sbyte[5], "lifeskillcombat_chahua_22"));
		_dataArray.Add(new LifeSkillCombatEffectItem(23, 52, 53, 1, 1, 1, 2, ELifeSkillCombatEffectType.Special, ELifeSkillCombatEffectGroup.CaptureArgument, isInstant: false, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte>
		{
			0, 1, 2, 18, 20, 15, 17, 7, 9, 13,
			14, 3, 4, 5, 21, 22
		}, new sbyte[5] { 0, 0, 0, 1, 1 }, new sbyte[5], ELifeSkillCombatEffectSubEffect.SelfExtraQuestionOnHouseThesisLowAndTransition, new sbyte[5] { -1, 0, 0, 0, 0 }, "lifeskillcombat_chahua_23"));
		_dataArray.Add(new LifeSkillCombatEffectItem(24, 36, 37, 1, 2, 1, 1, ELifeSkillCombatEffectType.Cooling, ELifeSkillCombatEffectGroup.ReduceCooling, isInstant: true, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 25, 26 }, new sbyte[5] { 0, 0, 3, 0, 0 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfChangeBookCd, new sbyte[5] { 1, 0, 0, 3, 1 }, "lifeskillcombat_chahua_24"));
		_dataArray.Add(new LifeSkillCombatEffectItem(25, 38, 39, 1, 2, 1, 1, ELifeSkillCombatEffectType.Cooling, ELifeSkillCombatEffectGroup.AddCooling, isInstant: true, isSelectGrid: false, isSelectBook: false, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 24, 26 }, new sbyte[5] { 3, 3, 0, 0, 0 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfChangeBookCd, new sbyte[5] { -1, 0, 0, 3, 1 }, "lifeskillcombat_chahua_25"));
		_dataArray.Add(new LifeSkillCombatEffectItem(26, 40, 41, 2, 2, 1, 1, ELifeSkillCombatEffectType.Cooling, ELifeSkillCombatEffectGroup.CompleteCooling, isInstant: true, isSelectGrid: false, isSelectBook: true, isSaveCard: false, isDecideAddEffect: false, isGetPointAddEffect: false, new List<sbyte> { 24, 25 }, new sbyte[5] { 0, 0, 0, 3, 3 }, new sbyte[5] { 0, 0, 30, 0, 0 }, ELifeSkillCombatEffectSubEffect.SelfChangeBookCd, new sbyte[5] { 1, 0, 0, 1, 127 }, "lifeskillcombat_chahua_26"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<LifeSkillCombatEffectItem>(27);
		CreateItems0();
	}
}
