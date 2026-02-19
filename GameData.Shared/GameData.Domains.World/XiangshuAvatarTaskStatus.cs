using GameData.Serializer;

namespace GameData.Domains.World;

public struct XiangshuAvatarTaskStatus : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte SwordTombStatus;

	[SerializableGameDataField]
	public sbyte JuniorXiangshuTaskStatus;

	[SerializableGameDataField]
	public int JuniorXiangshuCharId;

	public XiangshuAvatarTaskStatus(sbyte swordTombStatus, sbyte juniorXiangshuTaskStatus, int juniorXiangshuCharId)
	{
		SwordTombStatus = swordTombStatus;
		JuniorXiangshuTaskStatus = juniorXiangshuTaskStatus;
		JuniorXiangshuCharId = juniorXiangshuCharId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)SwordTombStatus;
		byte* num = pData + 1;
		*num = (byte)JuniorXiangshuTaskStatus;
		byte* num2 = num + 1;
		*(int*)num2 = JuniorXiangshuCharId;
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
		SwordTombStatus = (sbyte)(*ptr);
		ptr++;
		JuniorXiangshuTaskStatus = (sbyte)(*ptr);
		ptr++;
		JuniorXiangshuCharId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
