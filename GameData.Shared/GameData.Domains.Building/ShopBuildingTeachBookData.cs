using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

[SerializableGameData(IsExtensible = true)]
public class ShopBuildingTeachBookData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TeachBookResult = 0;

		public const ushort TeachBookInfo = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "TeachBookResult", "TeachBookInfo" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public sbyte TeachBookResult;

	[SerializableGameDataField(FieldIndex = 1)]
	public List<(short skillBookTemplateId, byte pageId, sbyte pageDirect)> TeachBookInfo;

	public static ShopBuildingTeachBookData CreateDefault()
	{
		return new ShopBuildingTeachBookData
		{
			TeachBookResult = 0,
			TeachBookInfo = new List<(short, byte, sbyte)>()
		};
	}

	public ShopBuildingTeachBookData()
	{
	}

	public ShopBuildingTeachBookData(ShopBuildingTeachBookData other)
	{
		TeachBookResult = other.TeachBookResult;
		TeachBookInfo = ((other.TeachBookInfo == null) ? null : new List<(short, byte, sbyte)>(other.TeachBookInfo));
	}

	public void Assign(ShopBuildingTeachBookData other)
	{
		TeachBookResult = other.TeachBookResult;
		TeachBookInfo = ((other.TeachBookInfo == null) ? null : new List<(short, byte, sbyte)>(other.TeachBookInfo));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		if (TeachBookInfo != null)
		{
			num += 2;
			int count = TeachBookInfo.Count;
			for (int i = 0; i < count; i++)
			{
				(short, byte, sbyte) tuple = TeachBookInfo[i];
				num += SerializationHelper.GetSerializedSize<short, byte, sbyte>(tuple);
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		*ptr = (byte)TeachBookResult;
		ptr++;
		if (TeachBookInfo != null)
		{
			int count = TeachBookInfo.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				(short, byte, sbyte) tuple = TeachBookInfo[i];
				ptr += SerializationHelper.Serialize<short, byte, sbyte>(ptr, tuple);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			TeachBookResult = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (TeachBookInfo == null)
				{
					TeachBookInfo = new List<(short, byte, sbyte)>(num2);
				}
				else
				{
					TeachBookInfo.Clear();
				}
				(short, byte, sbyte) item = default((short, byte, sbyte));
				for (int i = 0; i < num2; i++)
				{
					ptr += SerializationHelper.Deserialize<short, byte, sbyte>(ptr, ref item);
					TeachBookInfo.Add(item);
				}
			}
			else
			{
				TeachBookInfo?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
