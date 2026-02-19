using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class BehaviorType : ConfigData<BehaviorTypeItem, short>
{
	public static class DefKey
	{
		public const short Just = 0;

		public const short Kind = 1;

		public const short Even = 2;

		public const short Rebel = 3;

		public const short Egoistic = 4;
	}

	public static class DefValue
	{
		public static BehaviorTypeItem Just => Instance[(short)0];

		public static BehaviorTypeItem Kind => Instance[(short)1];

		public static BehaviorTypeItem Even => Instance[(short)2];

		public static BehaviorTypeItem Rebel => Instance[(short)3];

		public static BehaviorTypeItem Egoistic => Instance[(short)4];
	}

	public static BehaviorType Instance = new BehaviorType();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name", "Desc", "ExchangeBook", "Icon", "BetrayTips" };

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
		_dataArray.Add(new BehaviorTypeItem(0, 0, 1, 4, "sp_icon_behavior_just", new int[3] { 2, 3, 4 }));
		_dataArray.Add(new BehaviorTypeItem(1, 5, 6, 4, "sp_icon_behavior_kind", new int[3] { 7, 8, 9 }));
		_dataArray.Add(new BehaviorTypeItem(2, 10, 11, 4, "sp_icon_behavior_even", new int[3] { 12, 13, 14 }));
		_dataArray.Add(new BehaviorTypeItem(3, 15, 16, 4, "sp_icon_behavior_rebel", new int[3] { 17, 18, 19 }));
		_dataArray.Add(new BehaviorTypeItem(4, 20, 21, 4, "sp_icon_behavior_egoistic", new int[3] { 22, 23, 24 }));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<BehaviorTypeItem>(5);
		CreateItems0();
	}
}
