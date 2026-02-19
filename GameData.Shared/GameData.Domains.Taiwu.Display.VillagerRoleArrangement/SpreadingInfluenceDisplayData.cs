using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class SpreadingInfluenceDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int SafetyOrCultureChange;

	[SerializableGameDataField]
	public bool IsIncreaseSafetyOrCulture;

	[SerializableGameDataField]
	public int AuthorityGain;

	[SerializableGameDataField]
	public int GatherOrBattleEnemyProbability;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 13;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = SafetyOrCultureChange;
		byte* num = pData + 4;
		*num = (IsIncreaseSafetyOrCulture ? ((byte)1) : ((byte)0));
		byte* num2 = num + 1;
		*(int*)num2 = AuthorityGain;
		byte* num3 = num2 + 4;
		*(int*)num3 = GatherOrBattleEnemyProbability;
		int num4 = (int)(num3 + 4 - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SafetyOrCultureChange = *(int*)ptr;
		ptr += 4;
		IsIncreaseSafetyOrCulture = *ptr != 0;
		ptr++;
		AuthorityGain = *(int*)ptr;
		ptr += 4;
		GatherOrBattleEnemyProbability = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
