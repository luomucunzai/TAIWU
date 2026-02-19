using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class GuardingSwordTombDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte SwordTombId;

	[SerializableGameDataField]
	public sbyte EscapeState;

	[SerializableGameDataField]
	public int InformationGatheringSuccessRate;

	[SerializableGameDataField]
	public int InjuryProbability;

	[SerializableGameDataField]
	public int FeatureGainRateA;

	[SerializableGameDataField]
	public int FeatureGainRateB;

	[SerializableGameDataField]
	public int InfectionDecreaseRate;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)SwordTombId;
		byte* num = pData + 1;
		*num = (byte)EscapeState;
		byte* num2 = num + 1;
		*(int*)num2 = InformationGatheringSuccessRate;
		byte* num3 = num2 + 4;
		*(int*)num3 = InjuryProbability;
		byte* num4 = num3 + 4;
		*(int*)num4 = FeatureGainRateA;
		byte* num5 = num4 + 4;
		*(int*)num5 = FeatureGainRateB;
		byte* num6 = num5 + 4;
		*(int*)num6 = InfectionDecreaseRate;
		int num7 = (int)(num6 + 4 - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SwordTombId = (sbyte)(*ptr);
		ptr++;
		EscapeState = (sbyte)(*ptr);
		ptr++;
		InformationGatheringSuccessRate = *(int*)ptr;
		ptr += 4;
		InjuryProbability = *(int*)ptr;
		ptr += 4;
		FeatureGainRateA = *(int*)ptr;
		ptr += 4;
		FeatureGainRateB = *(int*)ptr;
		ptr += 4;
		InfectionDecreaseRate = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
