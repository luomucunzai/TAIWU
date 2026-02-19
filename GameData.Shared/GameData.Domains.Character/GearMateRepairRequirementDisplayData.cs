using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class GearMateRepairRequirementDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int GearMateId;

	[SerializableGameDataField]
	public sbyte RepairType;

	[SerializableGameDataField]
	public sbyte ResourceType;

	[SerializableGameDataField]
	public int ResourceCost;

	[SerializableGameDataField]
	public sbyte LifeSkillType;

	[SerializableGameDataField]
	public int AttainmentCount;

	[SerializableGameDataField]
	public sbyte ItemGrade;

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
		*(int*)pData = GearMateId;
		byte* num = pData + 4;
		*num = (byte)RepairType;
		byte* num2 = num + 1;
		*num2 = (byte)ResourceType;
		byte* num3 = num2 + 1;
		*(int*)num3 = ResourceCost;
		byte* num4 = num3 + 4;
		*num4 = (byte)LifeSkillType;
		byte* num5 = num4 + 1;
		*(int*)num5 = AttainmentCount;
		byte* num6 = num5 + 4;
		*num6 = (byte)ItemGrade;
		int num7 = (int)(num6 + 1 - pData);
		if (num7 > 4)
		{
			return (num7 + 3) / 4 * 4;
		}
		return num7;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		GearMateId = *(int*)ptr;
		ptr += 4;
		RepairType = (sbyte)(*ptr);
		ptr++;
		ResourceType = (sbyte)(*ptr);
		ptr++;
		ResourceCost = *(int*)ptr;
		ptr += 4;
		LifeSkillType = (sbyte)(*ptr);
		ptr++;
		AttainmentCount = *(int*)ptr;
		ptr += 4;
		ItemGrade = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
