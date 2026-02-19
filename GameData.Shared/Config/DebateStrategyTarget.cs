using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DebateStrategyTarget : ConfigData<DebateStrategyTargetItem, short>
{
	public static class DefKey
	{
		public const short SelfRepeatablePawn = 0;

		public const short SelfDistinctPawn = 1;

		public const short OpponentRepeatablePawn = 2;

		public const short OpponentDistinctPawn = 3;

		public const short BothRepeatablePawn = 4;

		public const short BothDistinctPawn = 5;

		public const short NonOpponentBottomNode = 6;

		public const short NonSelfBottomNode = 7;

		public const short SelfNeighborNode = 8;

		public const short PawnGrade = 9;

		public const short StrategyCard = 10;

		public const short LoosePawn = 11;

		public const short BasesNotRevealedPawn = 12;

		public const short StrategiesNotRevealedPawn = 13;

		public const short RemovableStrategyPawn = 14;

		public const short OwnedCard = 15;

		public const short UsedCard = 16;

		public const short OpponentCard = 17;
	}

	public static class DefValue
	{
		public static DebateStrategyTargetItem SelfRepeatablePawn => Instance[(short)0];

		public static DebateStrategyTargetItem SelfDistinctPawn => Instance[(short)1];

		public static DebateStrategyTargetItem OpponentRepeatablePawn => Instance[(short)2];

		public static DebateStrategyTargetItem OpponentDistinctPawn => Instance[(short)3];

		public static DebateStrategyTargetItem BothRepeatablePawn => Instance[(short)4];

		public static DebateStrategyTargetItem BothDistinctPawn => Instance[(short)5];

		public static DebateStrategyTargetItem NonOpponentBottomNode => Instance[(short)6];

		public static DebateStrategyTargetItem NonSelfBottomNode => Instance[(short)7];

		public static DebateStrategyTargetItem SelfNeighborNode => Instance[(short)8];

		public static DebateStrategyTargetItem PawnGrade => Instance[(short)9];

		public static DebateStrategyTargetItem StrategyCard => Instance[(short)10];

		public static DebateStrategyTargetItem LoosePawn => Instance[(short)11];

		public static DebateStrategyTargetItem BasesNotRevealedPawn => Instance[(short)12];

		public static DebateStrategyTargetItem StrategiesNotRevealedPawn => Instance[(short)13];

		public static DebateStrategyTargetItem RemovableStrategyPawn => Instance[(short)14];

		public static DebateStrategyTargetItem OwnedCard => Instance[(short)15];

		public static DebateStrategyTargetItem UsedCard => Instance[(short)16];

		public static DebateStrategyTargetItem OpponentCard => Instance[(short)17];
	}

	public static DebateStrategyTarget Instance = new DebateStrategyTarget();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "TemplateId", "Name" };

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
		_dataArray.Add(new DebateStrategyTargetItem(0, 0, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(1, 1, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(2, 2, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(3, 3, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(4, 4, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(5, 5, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(6, 6, EDebateStrategyTargetObjectType.Node));
		_dataArray.Add(new DebateStrategyTargetItem(7, 7, EDebateStrategyTargetObjectType.Node));
		_dataArray.Add(new DebateStrategyTargetItem(8, 8, EDebateStrategyTargetObjectType.Node));
		_dataArray.Add(new DebateStrategyTargetItem(9, 9, EDebateStrategyTargetObjectType.PawnGrade));
		_dataArray.Add(new DebateStrategyTargetItem(10, 10, EDebateStrategyTargetObjectType.StrategyCard));
		_dataArray.Add(new DebateStrategyTargetItem(11, 11, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(12, 12, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(13, 13, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(14, 14, EDebateStrategyTargetObjectType.Pawn));
		_dataArray.Add(new DebateStrategyTargetItem(15, 15, EDebateStrategyTargetObjectType.StrategyCard));
		_dataArray.Add(new DebateStrategyTargetItem(16, 16, EDebateStrategyTargetObjectType.StrategyCard));
		_dataArray.Add(new DebateStrategyTargetItem(17, 17, EDebateStrategyTargetObjectType.StrategyCard));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DebateStrategyTargetItem>(18);
		CreateItems0();
	}
}
