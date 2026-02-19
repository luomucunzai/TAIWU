using System.Collections.Generic;

namespace GameData.Domains.LegendaryBook;

public static class LegendaryBookDomainHelper
{
	public static class DataIds
	{
		public const ushort BookOwners = 0;

		public const ushort LegendaryBookOwnerData = 1;
	}

	public static class MethodIds
	{
		public const ushort GmCmd_GetAllLegendaryBookStates = 0;

		public const ushort GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc = 1;

		public const ushort GetLegendaryBookIncrementData = 2;

		public const ushort GetAllLegendaryBooksOwningState = 3;
	}

	public const ushort DataCount = 2;

	public static readonly Dictionary<string, ushort> FieldName2DataId = new Dictionary<string, ushort>
	{
		{ "BookOwners", 0 },
		{ "LegendaryBookOwnerData", 1 }
	};

	public static readonly string[] DataId2FieldName = new string[2] { "BookOwners", "LegendaryBookOwnerData" };

	public static readonly string[][] DataId2ObjectFieldId2FieldName = new string[2][];

	public static readonly Dictionary<string, ushort> MethodName2MethodId = new Dictionary<string, ushort>
	{
		{ "GmCmd_GetAllLegendaryBookStates", 0 },
		{ "GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc", 1 },
		{ "GetLegendaryBookIncrementData", 2 },
		{ "GetAllLegendaryBooksOwningState", 3 }
	};

	public static readonly string[] MethodId2MethodName = new string[4] { "GmCmd_GetAllLegendaryBookStates", "GmCmd_GiveAllTaiwuLegendaryBookToRandomNpc", "GetLegendaryBookIncrementData", "GetAllLegendaryBooksOwningState" };
}
