using GameData.Serializer;

namespace GameData.Domains.Character.Display;

public class CharacterLoveAndHateItemInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public bool LovingItemRevealed;

	[SerializableGameDataField]
	public bool HatingItemRevealed;

	[SerializableGameDataField]
	public bool NeedShowFirstRevealLovingEffect;

	[SerializableGameDataField]
	public bool NeedShowFirstRevealHatingEffect;

	[SerializableGameDataField]
	public short LovingItemSubType;

	[SerializableGameDataField]
	public short HatingItemSubType;

	[SerializableGameDataField]
	public int HobbyExpirationDate;

	[SerializableGameDataField]
	public byte CreatingType;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = CharacterId;
		byte* num = pData + 4;
		*num = (LovingItemRevealed ? ((byte)1) : ((byte)0));
		byte* num2 = num + 1;
		*num2 = (HatingItemRevealed ? ((byte)1) : ((byte)0));
		byte* num3 = num2 + 1;
		*num3 = (NeedShowFirstRevealLovingEffect ? ((byte)1) : ((byte)0));
		byte* num4 = num3 + 1;
		*num4 = (NeedShowFirstRevealHatingEffect ? ((byte)1) : ((byte)0));
		byte* num5 = num4 + 1;
		*(short*)num5 = LovingItemSubType;
		byte* num6 = num5 + 2;
		*(short*)num6 = HatingItemSubType;
		byte* num7 = num6 + 2;
		*(int*)num7 = HobbyExpirationDate;
		byte* num8 = num7 + 4;
		*num8 = CreatingType;
		int num9 = (int)(num8 + 1 - pData);
		if (num9 > 4)
		{
			return (num9 + 3) / 4 * 4;
		}
		return num9;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		LovingItemRevealed = *ptr != 0;
		ptr++;
		HatingItemRevealed = *ptr != 0;
		ptr++;
		NeedShowFirstRevealLovingEffect = *ptr != 0;
		ptr++;
		NeedShowFirstRevealHatingEffect = *ptr != 0;
		ptr++;
		LovingItemSubType = *(short*)ptr;
		ptr += 2;
		HatingItemSubType = *(short*)ptr;
		ptr += 2;
		HobbyExpirationDate = *(int*)ptr;
		ptr += 4;
		CreatingType = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
