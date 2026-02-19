using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class HealingDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int InteractTargetGrade;

	[SerializableGameDataField]
	public int HealXiangshuInfectionAmount;

	[SerializableGameDataField]
	public int GainSpiritualDebt;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = InteractTargetGrade;
		byte* num = pData + 4;
		*(int*)num = HealXiangshuInfectionAmount;
		byte* num2 = num + 4;
		*(int*)num2 = GainSpiritualDebt;
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
		InteractTargetGrade = *(int*)ptr;
		ptr += 4;
		HealXiangshuInfectionAmount = *(int*)ptr;
		ptr += 4;
		GainSpiritualDebt = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
