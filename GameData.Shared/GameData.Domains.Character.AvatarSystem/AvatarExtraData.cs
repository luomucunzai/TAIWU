using System;
using GameData.Serializer;

namespace GameData.Domains.Character.AvatarSystem;

[Serializable]
[SerializableGameData(NoCopyConstructors = true)]
public class AvatarExtraData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Feature1MirrorType;

	[SerializableGameDataField]
	public sbyte Feature2MirrorType;

	private AvatarExtraData(sbyte feature1MirrorType, sbyte feature2MirrorType)
	{
		Feature1MirrorType = feature1MirrorType;
		Feature2MirrorType = feature2MirrorType;
	}

	public AvatarExtraData()
	{
	}

	public static AvatarExtraData GetInValidData()
	{
		return new AvatarExtraData(-1, -1);
	}

	public bool IsValid()
	{
		if (Feature1MirrorType == -1)
		{
			return Feature2MirrorType != -1;
		}
		return true;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)Feature1MirrorType;
		byte* num = pData + 1;
		*num = (byte)Feature2MirrorType;
		int num2 = (int)(num + 1 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Feature1MirrorType = (sbyte)(*ptr);
		ptr++;
		Feature2MirrorType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
