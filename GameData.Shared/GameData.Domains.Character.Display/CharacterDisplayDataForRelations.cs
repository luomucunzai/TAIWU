using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public struct CharacterDisplayDataForRelations : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public NameAndLifeRelatedData NameAndLifeRelatedData;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public byte CreatingType;

	[SerializableGameDataField]
	public ushort RelationType;

	public CharacterDisplayDataForRelations(CharacterDisplayDataForRelations other)
	{
		CharacterId = other.CharacterId;
		NameAndLifeRelatedData = other.NameAndLifeRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		Location = other.Location;
		CreatingType = other.CreatingType;
		RelationType = other.RelationType;
	}

	public void Assign(CharacterDisplayDataForRelations other)
	{
		CharacterId = other.CharacterId;
		NameAndLifeRelatedData = other.NameAndLifeRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		Location = other.Location;
		CreatingType = other.CreatingType;
		RelationType = other.RelationType;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 131;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		ptr += NameAndLifeRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		ptr += Location.Serialize(ptr);
		*ptr = CreatingType;
		ptr++;
		*(ushort*)ptr = RelationType;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		ptr += NameAndLifeRelatedData.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		ptr += Location.Deserialize(ptr);
		CreatingType = *ptr;
		ptr++;
		RelationType = *(ushort*)ptr;
		ptr += 2;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
