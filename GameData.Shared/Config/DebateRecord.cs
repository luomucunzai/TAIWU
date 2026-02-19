using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DebateRecord : ConfigData<DebateRecordItem, short>
{
	public static class DefKey
	{
		public const short RoundStart = 0;

		public const short MakeMove = 1;

		public const short PawnReduceGamePoint = 2;

		public const short ShuffleCardGain = 3;

		public const short ShuffleCardLose = 4;

		public const short ResetStrategy = 68;

		public const short Comment = 5;

		public const short NodeEffectJustSpecial = 7;

		public const short NodeEffectEvenSpecial = 9;

		public const short PressureReduceGamePoint = 12;

		public const short PressureNoBasesRecover = 13;

		public const short PressureNoStrategyRecover = 14;

		public const short PressureUseBases = 15;

		public const short PressureUseStrategy = 16;

		public const short NpcUseStrategy = 17;

		public const short TaiwuUseTriggerStrategy = 18;

		public const short StrategyPoem3 = 27;

		public const short StrategyMedicine2Instant = 44;

		public const short StrategyMedicine2Special = 45;

		public const short StrategyJade1Special = 53;

		public const short StrategyJade2 = 54;

		public const short StrategyBuddhism3 = 61;
	}

	public static class DefValue
	{
		public static DebateRecordItem RoundStart => Instance[(short)0];

		public static DebateRecordItem MakeMove => Instance[(short)1];

		public static DebateRecordItem PawnReduceGamePoint => Instance[(short)2];

		public static DebateRecordItem ShuffleCardGain => Instance[(short)3];

		public static DebateRecordItem ShuffleCardLose => Instance[(short)4];

		public static DebateRecordItem ResetStrategy => Instance[(short)68];

		public static DebateRecordItem Comment => Instance[(short)5];

		public static DebateRecordItem NodeEffectJustSpecial => Instance[(short)7];

		public static DebateRecordItem NodeEffectEvenSpecial => Instance[(short)9];

		public static DebateRecordItem PressureReduceGamePoint => Instance[(short)12];

		public static DebateRecordItem PressureNoBasesRecover => Instance[(short)13];

		public static DebateRecordItem PressureNoStrategyRecover => Instance[(short)14];

		public static DebateRecordItem PressureUseBases => Instance[(short)15];

		public static DebateRecordItem PressureUseStrategy => Instance[(short)16];

		public static DebateRecordItem NpcUseStrategy => Instance[(short)17];

		public static DebateRecordItem TaiwuUseTriggerStrategy => Instance[(short)18];

		public static DebateRecordItem StrategyPoem3 => Instance[(short)27];

		public static DebateRecordItem StrategyMedicine2Instant => Instance[(short)44];

		public static DebateRecordItem StrategyMedicine2Special => Instance[(short)45];

		public static DebateRecordItem StrategyJade1Special => Instance[(short)53];

		public static DebateRecordItem StrategyJade2 => Instance[(short)54];

		public static DebateRecordItem StrategyBuddhism3 => Instance[(short)61];
	}

	public static DebateRecord Instance = new DebateRecord();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "Parameters", "TemplateId", "Desc" };

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
		_dataArray.Add(new DebateRecordItem(0, 0, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(1, 1, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(2, 2, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(3, 3, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(4, 4, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(5, 6, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.Character,
			EDebateRecordParamType.Comment,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(6, 7, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(7, 8, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(8, 9, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(9, 10, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(10, 11, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.Character,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(11, 12, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Spectator,
			EDebateRecordParamType.NodeEffect,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint
		}));
		_dataArray.Add(new DebateRecordItem(12, 13, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pressure,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(13, 14, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pressure,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(14, 14, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pressure,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(15, 15, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pressure,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(16, 16, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pressure,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(17, 17, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(18, 18, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(19, 19, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(20, 19, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(21, 19, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(22, 20, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(23, 21, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(24, 22, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(25, 23, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(26, 24, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(27, 25, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(28, 26, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(29, 26, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(30, 26, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(31, 27, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OwnedCards,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(32, 28, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(33, 29, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(34, 30, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(35, 31, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(36, 32, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(37, 33, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(38, 34, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(39, 35, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(40, 36, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(41, 37, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(42, 38, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(43, 39, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(44, 40, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(45, 41, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(46, 42, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(47, 43, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(48, 44, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(49, 26, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(50, 45, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(51, 46, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(52, 40, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(53, 41, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Bases,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(54, 47, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(55, 48, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.BottomNode,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.GamePoint,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(56, 49, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(57, 50, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.StrategyPoint,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(58, 27, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.UsedCards,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(59, 51, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.PawnCount,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
	}

	private void CreateItems1()
	{
		_dataArray.Add(new DebateRecordItem(60, 52, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.OpponentPawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(61, 53, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.SelfPawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(62, 54, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(63, 55, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(64, 56, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(65, 57, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(66, 58, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Pawn,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(67, 59, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.Strategy,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
		_dataArray.Add(new DebateRecordItem(68, 5, new EDebateRecordParamType[6]
		{
			EDebateRecordParamType.IntValue,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid,
			EDebateRecordParamType.Invalid
		}));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DebateRecordItem>(69);
		CreateItems0();
		CreateItems1();
	}
}
