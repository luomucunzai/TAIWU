using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class BuildingExceptionItem : ISerializableGameData
{
	[SerializableGameDataField]
	public List<sbyte> ExceptionTypeList = new List<sbyte>();

	public BuildingExceptionItem()
	{
	}

	public BuildingExceptionItem(BuildingExceptionItem other)
	{
		ExceptionTypeList = ((other.ExceptionTypeList == null) ? null : new List<sbyte>(other.ExceptionTypeList));
	}

	public void Assign(BuildingExceptionItem other)
	{
		ExceptionTypeList = ((other.ExceptionTypeList == null) ? null : new List<sbyte>(other.ExceptionTypeList));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((ExceptionTypeList == null) ? (num + 2) : (num + (2 + ExceptionTypeList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (ExceptionTypeList != null)
		{
			int count = ExceptionTypeList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr[i] = (byte)ExceptionTypeList[i];
			}
			ptr += count;
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
			if (ExceptionTypeList == null)
			{
				ExceptionTypeList = new List<sbyte>(num);
			}
			else
			{
				ExceptionTypeList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ExceptionTypeList.Add((sbyte)ptr[i]);
			}
			ptr += (int)num;
		}
		else
		{
			ExceptionTypeList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
