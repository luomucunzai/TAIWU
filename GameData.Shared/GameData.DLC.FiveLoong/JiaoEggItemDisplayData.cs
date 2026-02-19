using GameData.Serializer;

namespace GameData.DLC.FiveLoong;

public struct JiaoEggItemDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short TemplateId;

	[SerializableGameDataField]
	public sbyte ColorCount;

	[SerializableGameDataField]
	public bool Gender;

	[SerializableGameDataField]
	public int Generation;

	[SerializableGameDataField]
	public sbyte Behavior;

	public bool IsValid => ColorCount > 0;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = TemplateId;
		byte* num = pData + 2;
		*num = (byte)ColorCount;
		byte* num2 = num + 1;
		*num2 = (Gender ? ((byte)1) : ((byte)0));
		byte* num3 = num2 + 1;
		*(int*)num3 = Generation;
		byte* num4 = num3 + 4;
		*num4 = (byte)Behavior;
		int num5 = (int)(num4 + 1 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TemplateId = *(short*)ptr;
		ptr += 2;
		ColorCount = (sbyte)(*ptr);
		ptr++;
		Gender = *ptr != 0;
		ptr++;
		Generation = *(int*)ptr;
		ptr += 4;
		Behavior = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
