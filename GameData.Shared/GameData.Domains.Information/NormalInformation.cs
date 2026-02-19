using System;
using GameData.Serializer;

namespace GameData.Domains.Information;

public struct NormalInformation : ISerializableGameData
{
	[SerializableGameDataField]
	private short _templateId;

	[SerializableGameDataField]
	private sbyte _level;

	public const sbyte LevelMin = 0;

	public const sbyte LevelMax = 8;

	public short TemplateId => _templateId;

	public sbyte Level => _level;

	public bool IsValid()
	{
		if (TemplateId >= 0)
		{
			return Level >= 0;
		}
		return false;
	}

	public NormalInformation(short templateId, sbyte level)
	{
		_templateId = templateId;
		_level = level;
	}

	public void UpdateLevel(sbyte level)
	{
		_level = Math.Max(0, Math.Min(level, 8));
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
		*(short*)pData = _templateId;
		byte* num = pData + 2;
		*num = (byte)_level;
		int num2 = (int)(num + 1 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_templateId = *(short*)ptr;
		ptr += 2;
		_level = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
