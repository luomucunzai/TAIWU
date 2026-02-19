using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class ProfessionTipDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int ProfessionId;

	[SerializableGameDataField]
	public sbyte WorkingSkillType;

	[SerializableGameDataField]
	public int AttainmentBonus;

	[SerializableGameDataField]
	public int ProfessionUpgrade;

	[SerializableGameDataField]
	public int ProfessionUpgradeBonus;

	[SerializableGameDataField]
	public bool IsWearingBonusClothing;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = ProfessionId;
		byte* num = pData + 4;
		*num = (byte)WorkingSkillType;
		byte* num2 = num + 1;
		*(int*)num2 = AttainmentBonus;
		byte* num3 = num2 + 4;
		*(int*)num3 = ProfessionUpgrade;
		byte* num4 = num3 + 4;
		*(int*)num4 = ProfessionUpgradeBonus;
		byte* num5 = num4 + 4;
		*num5 = (IsWearingBonusClothing ? ((byte)1) : ((byte)0));
		int num6 = (int)(num5 + 1 - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ProfessionId = *(int*)ptr;
		ptr += 4;
		WorkingSkillType = (sbyte)(*ptr);
		ptr++;
		AttainmentBonus = *(int*)ptr;
		ptr += 4;
		ProfessionUpgrade = *(int*)ptr;
		ptr += 4;
		ProfessionUpgradeBonus = *(int*)ptr;
		ptr += 4;
		IsWearingBonusClothing = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
