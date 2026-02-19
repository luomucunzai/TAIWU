using GameData.Domains.Character;
using GameData.Domains.Character.Display;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LegendaryBook;

[SerializableGameData(NoCopyConstructors = true)]
public class LegendaryBookCharacterRelatedData : ISerializableGameData
{
	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public short FeatureId;

	[SerializableGameDataField]
	public short Favorability;

	[SerializableGameDataField]
	public short Charm;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public sbyte ConsummateLevel;

	[SerializableGameDataField]
	public sbyte BehaviorType;

	[SerializableGameDataField]
	public sbyte HappinessType;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public sbyte HealthType;

	[SerializableGameDataField]
	public sbyte BookType;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public OrganizationInfo OrganizationInfo;

	[SerializableGameDataField]
	public FullBlockName FullBlockName;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 147;
		num += FullBlockName.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = CurrAge;
		ptr += 2;
		*(short*)ptr = FeatureId;
		ptr += 2;
		*(short*)ptr = Favorability;
		ptr += 2;
		*(short*)ptr = Charm;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		*ptr = (byte)ConsummateLevel;
		ptr++;
		*ptr = (byte)BehaviorType;
		ptr++;
		*ptr = (byte)HappinessType;
		ptr++;
		*ptr = (byte)FameType;
		ptr++;
		*ptr = (byte)HealthType;
		ptr++;
		*ptr = (byte)BookType;
		ptr++;
		ptr += Location.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		ptr += NameRelatedData.Serialize(ptr);
		ptr += OrganizationInfo.Serialize(ptr);
		int num = FullBlockName.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		CurrAge = *(short*)ptr;
		ptr += 2;
		FeatureId = *(short*)ptr;
		ptr += 2;
		Favorability = *(short*)ptr;
		ptr += 2;
		Charm = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		ConsummateLevel = (sbyte)(*ptr);
		ptr++;
		BehaviorType = (sbyte)(*ptr);
		ptr++;
		HappinessType = (sbyte)(*ptr);
		ptr++;
		FameType = (sbyte)(*ptr);
		ptr++;
		HealthType = (sbyte)(*ptr);
		ptr++;
		BookType = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		ptr += NameRelatedData.Deserialize(ptr);
		ptr += OrganizationInfo.Deserialize(ptr);
		ptr += FullBlockName.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
