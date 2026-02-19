using GameData.Serializer;

namespace GameData.Domains.Item;

[SerializableGameData(NotForArchive = true)]
public struct CricketCombatConfig : ISerializableGameData
{
	[SerializableGameDataField]
	public bool OnlyNoInjury;

	[SerializableGameDataField]
	public sbyte MinGrade;

	[SerializableGameDataField]
	public sbyte MaxGrade;

	public CricketCombatConfig()
	{
		OnlyNoInjury = false;
		MinGrade = 0;
		MaxGrade = 8;
	}

	public static implicit operator CricketCombatConfig((bool onlyNoInjury, sbyte minGrade, sbyte maxGrade) tp)
	{
		CricketCombatConfig result = new CricketCombatConfig();
		(result.OnlyNoInjury, result.MinGrade, result.MaxGrade) = tp;
		return result;
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
		*pData = (OnlyNoInjury ? ((byte)1) : ((byte)0));
		byte* num = pData + 1;
		*num = (byte)MinGrade;
		byte* num2 = num + 1;
		*num2 = (byte)MaxGrade;
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
		OnlyNoInjury = *ptr != 0;
		ptr++;
		MinGrade = (sbyte)(*ptr);
		ptr++;
		MaxGrade = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
