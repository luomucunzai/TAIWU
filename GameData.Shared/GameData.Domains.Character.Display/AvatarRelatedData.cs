using System;
using Config;
using GameData.Domains.Building;
using GameData.Domains.Character.AvatarSystem;
using GameData.Serializer;

namespace GameData.Domains.Character.Display;

[Serializable]
[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class AvatarRelatedData : ISerializableGameData
{
	[SerializableGameDataField]
	public AvatarData AvatarData;

	[SerializableGameDataField]
	public short DisplayAge;

	[SerializableGameDataField]
	public short ClothingDisplayId;

	[SerializableGameDataField]
	public bool HasNewGoods;

	public AvatarRelatedData(RecruitCharacterData recruitChar)
	{
		AvatarData = new AvatarData(recruitChar.AvatarData);
		DisplayAge = recruitChar.Age;
		ClothingDisplayId = Clothing.Instance[recruitChar.ClothingTemplateId].DisplayId;
	}

	public AvatarRelatedData()
	{
	}

	public AvatarRelatedData(AvatarRelatedData other)
	{
		AvatarData = new AvatarData(other.AvatarData);
		DisplayAge = other.DisplayAge;
		ClothingDisplayId = other.ClothingDisplayId;
		HasNewGoods = other.HasNewGoods;
	}

	public void Assign(AvatarRelatedData other)
	{
		AvatarData = new AvatarData(other.AvatarData);
		DisplayAge = other.DisplayAge;
		ClothingDisplayId = other.ClothingDisplayId;
		HasNewGoods = other.HasNewGoods;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 81;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += AvatarData.Serialize(ptr);
		*(short*)ptr = DisplayAge;
		ptr += 2;
		*(short*)ptr = ClothingDisplayId;
		ptr += 2;
		*ptr = (HasNewGoods ? ((byte)1) : ((byte)0));
		ptr++;
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
		if (AvatarData == null)
		{
			AvatarData = new AvatarData();
		}
		ptr += AvatarData.Deserialize(ptr);
		DisplayAge = *(short*)ptr;
		ptr += 2;
		ClothingDisplayId = *(short*)ptr;
		ptr += 2;
		HasNewGoods = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
