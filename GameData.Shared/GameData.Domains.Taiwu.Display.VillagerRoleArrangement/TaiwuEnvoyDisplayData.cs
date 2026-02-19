using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[AutoGenerateSerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class TaiwuEnvoyDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int SpecialRuleCount;

	[SerializableGameDataField]
	public int MonthlyAuthorityCost;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = SpecialRuleCount;
		byte* num = pData + 4;
		*(int*)num = MonthlyAuthorityCost;
		int num2 = (int)(num + 4 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SpecialRuleCount = *(int*)ptr;
		ptr += 4;
		MonthlyAuthorityCost = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
