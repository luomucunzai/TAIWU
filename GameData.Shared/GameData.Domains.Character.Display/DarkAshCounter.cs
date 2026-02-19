using System;
using GameData.Serializer;

namespace GameData.Domains.Character.Display;

[SerializableGameData(IsExtensible = true, NotForArchive = true)]
public struct DarkAshCounter : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Tips1 = 0;

		public const ushort Tips2 = 1;

		public const ushort Tips3 = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "Tips1", "Tips2", "Tips3" };
	}

	[SerializableGameDataField]
	public int Tips1;

	[SerializableGameDataField]
	public int Tips2;

	[SerializableGameDataField]
	public int Tips3;

	public int Total => Tips1 + Tips2 + Tips3;

	public DarkAshCounter(int expiredDate, int currDate)
	{
		Tips1 = Math.Max(expiredDate - currDate, 0);
		Tips2 = (Tips3 = 0);
	}

	public DarkAshCounter(int expiredDate, int currDate, DarkAshCounterData data)
	{
		Tips3 = Math.Max(data.ExpiredDate3 - currDate, 0);
		Tips2 = Math.Max(data.ExpiredDate2 - currDate - Tips3, 0);
		Tips1 = Math.Max(expiredDate - currDate - Tips2 - Tips3, 0);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 14;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 3;
		byte* num = pData + 2;
		*(int*)num = Tips1;
		byte* num2 = num + 4;
		*(int*)num2 = Tips2;
		byte* num3 = num2 + 4;
		*(int*)num3 = Tips3;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			Tips1 = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			Tips2 = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			Tips3 = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
