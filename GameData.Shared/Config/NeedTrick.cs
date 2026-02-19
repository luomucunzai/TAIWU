using System;
using GameData.Serializer;

namespace Config;

[Serializable]
public struct NeedTrick : ISerializableGameData
{
	public sbyte TrickType;

	public byte NeedCount;

	public NeedTrick(sbyte type, byte count)
	{
		TrickType = type;
		NeedCount = count;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 4;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)TrickType;
		byte* num = pData + 1;
		*num = NeedCount;
		return (int)(num + 1 + 2 - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TrickType = (sbyte)(*ptr);
		ptr++;
		NeedCount = *ptr;
		ptr++;
		ptr += 2;
		return (int)(ptr - pData);
	}
}
