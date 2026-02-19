namespace GameData.ArchiveData;

public static class OperationConstants
{
	public static class OperationType
	{
		public const byte Read = 0;

		public const byte Write = 1;

		public const byte Control = 2;
	}

	public static class FixedSingleValueMethodIds
	{
		public const byte Set = 0;

		public const byte Get = 1;
	}

	public static class DynamicSingleValueMethodIds
	{
		public const byte Set = 0;

		public const byte Get = 1;
	}

	public static class FixedSingleValueCollectionMethodIds
	{
		public const byte Add = 0;

		public const byte Set = 1;

		public const byte Get = 2;

		public const byte Remove = 3;

		public const byte Clear = 4;

		public const byte GetAll = 5;
	}

	public static class DynamicSingleValueCollectionMethodIds
	{
		public const byte Add = 0;

		public const byte Set = 1;

		public const byte Get = 2;

		public const byte Remove = 3;

		public const byte Clear = 4;

		public const byte GetAll = 5;
	}

	public static class FixedElementListMethodIds
	{
		public const byte GetCount = 0;

		public const byte Set = 1;

		public const byte Get = 2;

		public const byte Insert = 3;

		public const byte InsertRange = 4;

		public const byte RemoveAt = 5;

		public const byte RemoveRange = 6;

		public const byte Clear = 7;

		public const byte GetAll = 8;
	}

	public static class DynamicElementListMethodIds
	{
		public const byte GetCount = 0;

		public const byte Set = 1;

		public const byte Get = 2;

		public const byte Insert = 3;

		public const byte InsertRange = 4;

		public const byte RemoveAt = 5;

		public const byte RemoveRange = 6;

		public const byte Clear = 7;

		public const byte GetAll = 8;
	}

	public static class FixedObjectCollectionMethodIds
	{
		public const byte Add = 0;

		public const byte Get = 1;

		public const byte GetList = 2;

		public const byte Remove = 3;

		public const byte Clear = 4;

		public const byte SetFixedField = 5;

		public const byte GetFixedField = 6;

		public const byte GetAllIds = 7;

		public const byte GetAllObjects = 8;
	}

	public static class DynamicObjectCollectionMethodIds
	{
		public const byte Add = 0;

		public const byte Get = 1;

		public const byte GetList = 2;

		public const byte Remove = 3;

		public const byte Clear = 4;

		public const byte SetFixedField = 5;

		public const byte GetFixedField = 6;

		public const byte SetDynamicField = 7;

		public const byte GetDynamicField = 8;

		public const byte GetAllIds = 9;

		public const byte GetAllObjects = 10;
	}

	public static class BinaryMethodIds
	{
		public const byte Insert = 0;

		public const byte Write = 1;

		public const byte Remove = 2;

		public const byte SetMetadata = 3;

		public const byte Get = 4;
	}

	public static class GlobalMethodIds
	{
		public const byte FreeMemory = 0;

		public const byte EnterNewWorld = 1;

		public const byte LoadWorld = 2;

		public const byte SaveWorld = 3;

		public const byte LeaveWorld = 4;

		public const byte GetArchivesInfo = 5;

		public const byte DeleteArchive = 6;

		public const byte SaveEnding = 7;

		public const byte LoadEnding = 8;

		public const byte GetEndingArchiveInfo = 9;
	}

	public static class LifeRecordMethodIds
	{
		public const byte Add = 0;

		public const byte Get = 1;

		public const byte GetByDate = 2;

		public const byte GetLast = 3;

		public const byte Search = 4;

		public const byte Remove = 5;

		public const byte GenerateDead = 6;

		public const byte GetDead = 7;

		public const byte RemoveDead = 8;

		public const byte GetAllByChar = 11;
	}
}
