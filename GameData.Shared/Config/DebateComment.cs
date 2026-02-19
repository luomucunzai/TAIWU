using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class DebateComment : ConfigData<DebateCommentItem, short>
{
	public static class DefKey
	{
		public const short PositiveComment0 = 0;

		public const short NegativeComment0 = 1;

		public const short PositiveComment1 = 2;

		public const short NegativeComment1 = 3;

		public const short PositiveComment2 = 4;

		public const short NegativeComment2 = 5;

		public const short PositiveComment3 = 6;

		public const short NegativeComment3 = 7;

		public const short PositiveComment4 = 8;

		public const short NegativeComment4 = 9;
	}

	public static class DefValue
	{
		public static DebateCommentItem PositiveComment0 => Instance[(short)0];

		public static DebateCommentItem NegativeComment0 => Instance[(short)1];

		public static DebateCommentItem PositiveComment1 => Instance[(short)2];

		public static DebateCommentItem NegativeComment1 => Instance[(short)3];

		public static DebateCommentItem PositiveComment2 => Instance[(short)4];

		public static DebateCommentItem NegativeComment2 => Instance[(short)5];

		public static DebateCommentItem PositiveComment3 => Instance[(short)6];

		public static DebateCommentItem NegativeComment3 => Instance[(short)7];

		public static DebateCommentItem PositiveComment4 => Instance[(short)8];

		public static DebateCommentItem NegativeComment4 => Instance[(short)9];
	}

	public static DebateComment Instance = new DebateComment();

	private readonly HashSet<string> RequiredFields = new HashSet<string> { "BehaviorType", "Negation", "TemplateId", "Name", "Desc", "ResultTip", "BubbleContent", "Favor", "IsPositive", "CheckValue" };

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
		_dataArray.Add(new DebateCommentItem(0, 0, 1, 2, 3, 0, new short[5] { 3, 1, 1, 0, 1 }, 600, isPositive: true, 1, 0));
		_dataArray.Add(new DebateCommentItem(1, 4, 5, 2, 6, 3, new short[5] { 0, -1, -1, -3, -1 }, -300, isPositive: false, 0, 0));
		_dataArray.Add(new DebateCommentItem(2, 7, 8, 9, 10, 2, new short[5] { 0, 1, 3, 1, 1 }, 600, isPositive: true, 3, 2));
		_dataArray.Add(new DebateCommentItem(3, 11, 12, 9, 13, 0, new short[5] { -3, -1, 0, -1, -1 }, -300, isPositive: false, 2, 2));
		_dataArray.Add(new DebateCommentItem(4, 14, 15, 16, 17, 1, new short[5] { 1, 3, 1, 1, 0 }, 600, isPositive: true, 5, 5));
		_dataArray.Add(new DebateCommentItem(5, 18, 19, 16, 20, 4, new short[5] { -1, 0, -1, -1, -3 }, -300, isPositive: false, 4, 5));
		_dataArray.Add(new DebateCommentItem(6, 21, 22, 23, 24, 4, new short[5] { 1, 0, 1, 1, 3 }, 600, isPositive: true, 7, 2));
		_dataArray.Add(new DebateCommentItem(7, 25, 26, 23, 27, 1, new short[5] { -1, -3, -1, -1, 0 }, -300, isPositive: false, 6, 2));
		_dataArray.Add(new DebateCommentItem(8, 28, 29, 30, 31, 3, new short[5] { 1, 1, 0, 3, 1 }, 600, isPositive: true, 9, 0));
		_dataArray.Add(new DebateCommentItem(9, 32, 33, 30, 34, 2, new short[5] { -1, -1, -3, 0, -1 }, -300, isPositive: false, 8, 0));
	}

	public override void Init()
	{
		base.Init();
		_dataArray = new List<DebateCommentItem>(10);
		CreateItems0();
	}
}
