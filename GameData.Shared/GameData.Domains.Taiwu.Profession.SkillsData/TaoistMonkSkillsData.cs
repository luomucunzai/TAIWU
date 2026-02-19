using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class TaoistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort SurvivedTribulationCount = 0;

		public const ushort LastAgeIncreaseDate = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "SurvivedTribulationCount", "LastAgeIncreaseDate" };
	}

	[SerializableGameDataField]
	public sbyte SurvivedTribulationCount;

	[SerializableGameDataField]
	public int LastAgeIncreaseDate;

	public bool IsTriggeringTribulation;

	public void Initialize()
	{
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (sourceData is ObsoleteTaoistMonkSkillsData obsoleteTaoistMonkSkillsData)
		{
			SurvivedTribulationCount = obsoleteTaoistMonkSkillsData.SurvivedTribulationCount;
		}
	}

	public bool HasSurvivedAllTribulation()
	{
		return SurvivedTribulationCount >= 4;
	}

	public bool ShouldIncreaseAge()
	{
		return ExternalDataBridge.Context.CurrDate - LastAgeIncreaseDate >= 36;
	}

	public TaoistMonkSkillsData()
	{
	}

	public TaoistMonkSkillsData(TaoistMonkSkillsData other)
	{
		SurvivedTribulationCount = other.SurvivedTribulationCount;
		LastAgeIncreaseDate = other.LastAgeIncreaseDate;
	}

	public void Assign(TaoistMonkSkillsData other)
	{
		SurvivedTribulationCount = other.SurvivedTribulationCount;
		LastAgeIncreaseDate = other.LastAgeIncreaseDate;
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
		*(short*)pData = 2;
		byte* num = pData + 2;
		*num = (byte)SurvivedTribulationCount;
		byte* num2 = num + 1;
		*(int*)num2 = LastAgeIncreaseDate;
		int num3 = (int)(num2 + 4 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			SurvivedTribulationCount = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 1)
		{
			LastAgeIncreaseDate = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
