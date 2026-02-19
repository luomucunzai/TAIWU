using System.Collections.Generic;

namespace GameData.Domains.TutorialChapter;

public static class TutorialChapterDomainHelper
{
	public static class DataIds
	{
		public const ushort CurProgress = 0;

		public const ushort InGuiding = 1;

		public const ushort TutorialChapter = 2;

		public const ushort CanMove = 3;

		public const ushort CanOpenCharacterMenu = 4;

		public const ushort GuidVideoName = 5;

		public const ushort CanAdvanceMonth = 6;

		public const ushort NextForceLocation = 7;

		public const ushort NeiliAllocateFitChapter7 = 8;

		public const ushort HuanxinDying = 9;

		public const ushort HuanxinSurprised = 10;

		public const ushort CanShowEnterIndustry = 11;
	}

	public static class MethodIds
	{
		public const ushort CreateFixedWorld = 0;

		public const ushort StartChapter = 1;

		public const ushort GetNextForceMoveToLocation = 2;
	}

	public const ushort DataCount = 12;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "CurProgress", 0 },
		{ "InGuiding", 1 },
		{ "TutorialChapter", 2 },
		{ "CanMove", 3 },
		{ "CanOpenCharacterMenu", 4 },
		{ "GuidVideoName", 5 },
		{ "CanAdvanceMonth", 6 },
		{ "NextForceLocation", 7 },
		{ "NeiliAllocateFitChapter7", 8 },
		{ "HuanxinDying", 9 },
		{ "HuanxinSurprised", 10 },
		{ "CanShowEnterIndustry", 11 }
	};

	public static readonly string[] DataId2FieldName = new string[12]
	{
		"CurProgress", "InGuiding", "TutorialChapter", "CanMove", "CanOpenCharacterMenu", "GuidVideoName", "CanAdvanceMonth", "NextForceLocation", "NeiliAllocateFitChapter7", "HuanxinDying",
		"HuanxinSurprised", "CanShowEnterIndustry"
	};

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[12][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "CreateFixedWorld", 0 },
		{ "StartChapter", 1 },
		{ "GetNextForceMoveToLocation", 2 }
	};

	public static readonly string[] MethodId2MethodName = new string[3] { "CreateFixedWorld", "StartChapter", "GetNextForceMoveToLocation" };
}
