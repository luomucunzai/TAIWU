using GameData.Serializer;

namespace GameData.Domains.Adventure;

public class AdventureMapConnect : ISerializableGameData
{
	[SerializableGameDataField]
	public int PortA;

	[SerializableGameDataField]
	public int PortB;

	[SerializableGameDataField]
	public sbyte EnterLifeSkillType = -1;

	[SerializableGameDataField]
	public short EnterRequiredVal = -1;

	public AdventureMapConnect(int portA, int portB)
	{
		PortA = portA;
		PortB = portB;
	}

	public AdventureMapConnect()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = PortA;
		byte* num = pData + 4;
		*(int*)num = PortB;
		byte* num2 = num + 4;
		*num2 = (byte)EnterLifeSkillType;
		byte* num3 = num2 + 1;
		*(short*)num3 = EnterRequiredVal;
		int num4 = (int)(num3 + 2 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		PortA = *(int*)ptr;
		ptr += 4;
		PortB = *(int*)ptr;
		ptr += 4;
		EnterLifeSkillType = (sbyte)(*ptr);
		ptr++;
		EnterRequiredVal = *(short*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
