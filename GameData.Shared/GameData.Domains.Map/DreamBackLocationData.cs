using GameData.Serializer;

namespace GameData.Domains.Map;

[SerializableGameData(IsExtensible = true)]
public struct DreamBackLocationData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Location = 0;

		public const ushort InternalType = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "Location", "InternalType" };
	}

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	private sbyte _internalType;

	public EDreamBackLocationType Type => (EDreamBackLocationType)_internalType;

	public static DreamBackLocationData Create(Location location, EDreamBackLocationType locationType)
	{
		return new DreamBackLocationData
		{
			Location = location,
			_internalType = (sbyte)locationType
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 7;
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
		ptr += Location.Serialize(ptr);
		*ptr = (byte)_internalType;
		ptr++;
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
			ptr += Location.Deserialize(ptr);
		}
		if (num > 1)
		{
			_internalType = (sbyte)(*ptr);
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
