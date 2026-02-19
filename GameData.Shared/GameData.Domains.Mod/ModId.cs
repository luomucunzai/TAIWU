using System;
using GameData.Serializer;

namespace GameData.Domains.Mod;

public struct ModId : ISerializableGameData, IEquatable<ModId>
{
	[SerializableGameDataField]
	public ulong FileId;

	[SerializableGameDataField]
	public ulong Version;

	[SerializableGameDataField]
	public byte Source;

	public bool IsValid
	{
		get
		{
			if (FileId != 0)
			{
				return FileId < ulong.MaxValue;
			}
			return false;
		}
	}

	public ModId(ulong fileId, ulong version, byte source)
	{
		FileId = fileId;
		Version = version;
		Source = source;
	}

	public bool Equals(ModId other)
	{
		if (FileId == other.FileId && Source == other.Source)
		{
			return Version == other.Version;
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (((FileId.GetHashCode() * 397) ^ Source.GetHashCode()) * 397) ^ Version.GetHashCode();
	}

	public override string ToString()
	{
		return $"{Source}_{FileId}";
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(ulong*)pData = FileId;
		byte* num = pData + 8;
		*(ulong*)num = Version;
		byte* num2 = num + 8;
		*num2 = Source;
		int num3 = (int)(num2 + 1 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		FileId = *(ulong*)ptr;
		ptr += 8;
		Version = *(ulong*)ptr;
		ptr += 8;
		Source = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
