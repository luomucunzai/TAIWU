using GameData.Serializer;

namespace GameData.Domains.Taiwu.Display.VillagerRoleArrangement;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class EntertainingDisplayData : IVillagerRoleArrangementDisplayData, ISerializableGameData
{
	[SerializableGameDataField]
	public int ActionEffectCount;

	[SerializableGameDataField]
	public int ActionEffectValue;

	[SerializableGameDataField]
	public int ExtraPeopleCount;

	[SerializableGameDataField]
	public int RelationChange;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 16;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = ActionEffectCount;
		byte* num = pData + 4;
		*(int*)num = ActionEffectValue;
		byte* num2 = num + 4;
		*(int*)num2 = ExtraPeopleCount;
		byte* num3 = num2 + 4;
		*(int*)num3 = RelationChange;
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
		ActionEffectCount = *(int*)ptr;
		ptr += 4;
		ActionEffectValue = *(int*)ptr;
		ptr += 4;
		ExtraPeopleCount = *(int*)ptr;
		ptr += 4;
		RelationChange = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
