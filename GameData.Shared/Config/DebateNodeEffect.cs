using System;
using System.Collections.Generic;
using Config.Common;
using GameData.Utilities;

namespace Config;

[Serializable]
public class DebateNodeEffect : ConfigData<DebateNodeEffectItem, short>
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
		public static DebateNodeEffectItem Just => Instance[(short)0];

		public static DebateNodeEffectItem Kind => Instance[(short)1];

		public static DebateNodeEffectItem Even => Instance[(short)2];

		public static DebateNodeEffectItem Rebel => Instance[(short)3];

		public static DebateNodeEffectItem Egoistic => Instance[(short)4];
	}

	public static DebateNodeEffect Instance = new DebateNodeEffect();

	private readonly HashSet<string> RequiredFields = new HashSet<string>
	{
		"BehaviorType", "DebateRecord", "InstantEffectList", "TriggerEffectList", "SpecialEffectList", "RemoveType", "TemplateId", "Name", "Desc", "BubbleContent",
		"LoopSound", "TriggerSound", "ExtraTriggerSound"
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
		_dataArray.Add(new DebateNodeEffectItem(0, 0, 1, 2, 0, 6, new List<IntPair>
		{
			new IntPair(20, 1),
			new IntPair(32, 1)
		}, new List<IntPair>
		{
			new IntPair(20, 1),
			new IntPair(32, 1)
		}, new List<IntPair>
		{
			new IntPair(20, 1),
			new IntPair(32, 1),
			new IntPair(44, 1)
		}, 3, 3, new List<EDebateNodeEffectRemoveType> { EDebateNodeEffectRemoveType.Special }, "art_GridLoop_1", "art_GridSkill_1", null));
		_dataArray.Add(new DebateNodeEffectItem(1, 3, 4, 5, 1, 8, new List<IntPair>
		{
			new IntPair(9, 20)
		}, new List<IntPair>
		{
			new IntPair(9, 20)
		}, new List<IntPair>
		{
			new IntPair(23, 1)
		}, 3, 3, new List<EDebateNodeEffectRemoveType>
		{
			EDebateNodeEffectRemoveType.Instant,
			EDebateNodeEffectRemoveType.Trigger
		}, "art_GridLoop_2", "art_GridSkill_2", "art_add"));
		_dataArray.Add(new DebateNodeEffectItem(2, 6, 7, 8, 2, 9, new List<IntPair>(), new List<IntPair>(), new List<IntPair>
		{
			new IntPair(41, 1)
		}, 3, 3, new List<EDebateNodeEffectRemoveType> { EDebateNodeEffectRemoveType.Special }, "art_GridLoop_3", "art_GridSkill_3", null));
		_dataArray.Add(new DebateNodeEffectItem(3, 9, 10, 11, 3, 10, new List<IntPair>
		{
			new IntPair(42, 1)
		}, new List<IntPair>
		{
			new IntPair(42, 1)
		}, new List<IntPair>(), 3, 3, new List<EDebateNodeEffectRemoveType>
		{
			EDebateNodeEffectRemoveType.Instant,
			EDebateNodeEffectRemoveType.Trigger
		}, "art_GridLoop_4", "art_GridSkill_4", null));
		_dataArray.Add(new DebateNodeEffectItem(4, 12, 13, 14, 4, 11, new List<IntPair>(), new List<IntPair>
		{
			new IntPair(43, 1)
		}, new List<IntPair>(), 3, 3, new List<EDebateNodeEffectRemoveType> { EDebateNodeEffectRemoveType.Trigger }, "art_GridLoop_5", "art_GridSkill_5", "art_LightningHurt"));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DebateNodeEffectItem>(5);
		CreateItems0();
	}
}
