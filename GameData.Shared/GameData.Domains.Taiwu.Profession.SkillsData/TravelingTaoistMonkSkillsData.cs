using GameData.Serializer;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class TravelingTaoistMonkSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort BonusMaxHealth = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "BonusMaxHealth" };
	}

	[SerializableGameDataField]
	public short BonusMaxHealth;

	public void Initialize()
	{
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (sourceData is ObsoleteTravelingTaoistMonkSkillsData obsoleteTravelingTaoistMonkSkillsData)
		{
			BonusMaxHealth = obsoleteTravelingTaoistMonkSkillsData.BonusMaxHealth;
		}
	}

	public TravelingTaoistMonkSkillsData()
	{
	}

	public TravelingTaoistMonkSkillsData(TravelingTaoistMonkSkillsData other)
	{
		BonusMaxHealth = other.BonusMaxHealth;
	}

	public void Assign(TravelingTaoistMonkSkillsData other)
	{
		BonusMaxHealth = other.BonusMaxHealth;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 1;
		byte* num = pData + 2;
		*(short*)num = BonusMaxHealth;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			BonusMaxHealth = *(short*)ptr;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
