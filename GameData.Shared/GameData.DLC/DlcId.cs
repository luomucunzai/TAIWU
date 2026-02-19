using System;
using GameData.Serializer;

namespace GameData.DLC;

public struct DlcId : ISerializableGameData, IEquatable<DlcId>
{
	[SerializableGameDataField]
	public ulong AppId;

	[SerializableGameDataField]
	public ulong Version;

	public DlcId(ulong appId, ulong version)
	{
		AppId = appId;
		Version = version;
	}

	public bool Equals(DlcId other)
	{
		if (AppId == other.AppId)
		{
			return Version == other.Version;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (AppId.GetHashCode() * 397) ^ Version.GetHashCode();
	}

	public override string ToString()
	{
		return $"{AppId}_{Version}";
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(ulong*)pData = AppId;
		byte* num = pData + 8;
		*(ulong*)num = Version;
		int num2 = (int)(num + 8 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		AppId = *(ulong*)ptr;
		ptr += 8;
		Version = *(ulong*)ptr;
		ptr += 8;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
