using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class NormalInteraction : ConfigData<NormalInteractionItem, short>
{
	public static class DefKey
	{
		public const short RequestPlayCombat = 0;

		public const short RequestNormalCombat = 1;

		public const short RequestLifeSkillBattle = 2;

		public const short RequestCricketBattle = 3;
	}

	public static class DefValue
	{
		public static NormalInteractionItem RequestPlayCombat => Instance[(short)0];

		public static NormalInteractionItem RequestNormalCombat => Instance[(short)1];

		public static NormalInteractionItem RequestLifeSkillBattle => Instance[(short)2];

		public static NormalInteractionItem RequestCricketBattle => Instance[(short)3];
	}

	public static NormalInteraction Instance = new NormalInteraction();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "HeadEvent", "AgreeAndSuccess", "AgreeAndFail", "Disagree" };

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
		_dataArray.Add(new NormalInteractionItem(0, 0, new int[5] { 1, 2, 3, 4, 5 }, new int[5] { 6, 7, 8, 9, 9 }, new int[5] { 10, 10, 11, 12, 12 }, new int[5] { 13, 13, 13, 13, 13 }));
		_dataArray.Add(new NormalInteractionItem(1, 14, new int[5] { 15, 16, 17, 18, 19 }, new int[5] { 20, 21, 22, 23, 23 }, new int[5] { 24, 24, 25, 24, 24 }, new int[5] { 13, 13, 13, 13, 13 }));
		_dataArray.Add(new NormalInteractionItem(2, 26, new int[5] { 27, 27, 28, 29, 30 }, new int[5] { 31, 31, 32, 33, 33 }, new int[5] { 34, 34, 11, 34, 34 }, new int[5] { 13, 13, 13, 13, 13 }));
		_dataArray.Add(new NormalInteractionItem(3, 35, new int[5] { 36, 36, 36, 37, 37 }, new int[5] { 38, 38, 38, 39, 39 }, new int[5] { 40, 40, 40, 40, 40 }, new int[5] { 13, 13, 13, 13, 13 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<NormalInteractionItem>(4);
		CreateItems0();
	}
}
