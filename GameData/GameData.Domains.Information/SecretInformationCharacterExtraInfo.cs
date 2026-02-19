using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(NotForDisplayModule = true)]
public struct SecretInformationCharacterExtraInfo : ISerializableGameData
{
	[SerializableGameDataField]
	public OrganizationInfo OrgInfo;

	[SerializableGameDataField]
	public sbyte FameType;

	[SerializableGameDataField]
	public byte MonkType;

	[SerializableGameDataField]
	public sbyte AliveState;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 11;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += OrgInfo.Serialize(ptr);
		*ptr = (byte)FameType;
		ptr++;
		*ptr = MonkType;
		ptr++;
		*ptr = (byte)AliveState;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += OrgInfo.Deserialize(ptr);
		FameType = (sbyte)(*ptr);
		ptr++;
		MonkType = *ptr;
		ptr++;
		AliveState = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
