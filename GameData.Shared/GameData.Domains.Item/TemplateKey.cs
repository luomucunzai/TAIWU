using System;
using GameData.Serializer;

namespace GameData.Domains.Item;

public struct TemplateKey : ISerializableGameData, IEquatable<TemplateKey>
{
	public static readonly TemplateKey Invalid = new TemplateKey(-1, -1);

	[SerializableGameDataField]
	public sbyte ItemType;

	[SerializableGameDataField]
	public short TemplateId;

	public TemplateKey(sbyte itemType, short templateId)
	{
		ItemType = itemType;
		TemplateId = templateId;
	}

	public static explicit operator uint(TemplateKey value)
	{
		return (uint)(((ushort)value.TemplateId << 16) + (byte)value.ItemType);
	}

	public static explicit operator TemplateKey(uint value)
	{
		return new TemplateKey((sbyte)value, (short)(value >> 16));
	}

	public bool Equals(TemplateKey other)
	{
		if (ItemType == other.ItemType)
		{
			return TemplateId == other.TemplateId;
		}
		return false;
	}

	public override bool Equals(object obj)
	{
		if (obj is TemplateKey other)
		{
			return Equals(other);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return (ItemType.GetHashCode() * 397) ^ TemplateId.GetHashCode();
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)ItemType;
		byte* num = pData + 1;
		*(short*)num = TemplateId;
		int num2 = (int)(num + 2 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ItemType = (sbyte)(*ptr);
		ptr++;
		TemplateId = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
