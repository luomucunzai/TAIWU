using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NoCopyConstructors = true)]
public class CharacterDisplayDataForSettlementBounty : ISerializableGameData
{
	[SerializableGameDataField]
	public SettlementBounty SettlementBounty;

	[SerializableGameDataField]
	public NameRelatedData NameRelatedData;

	[SerializableGameDataField]
	public AvatarRelatedData AvatarRelatedData;

	[SerializableGameDataField]
	public short CurrAge;

	[SerializableGameDataField]
	public sbyte Gender;

	[SerializableGameDataField]
	public OrganizationInfo OrgInfo;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short LeftMaxHealth;

	[SerializableGameDataField]
	public FullBlockName FullBlockName;

	[SerializableGameDataField]
	public short RandomNameId = -1;

	[SerializableGameDataField]
	public sbyte HunterState = -1;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 134;
		num = ((SettlementBounty == null) ? (num + 2) : (num + (2 + SettlementBounty.GetSerializedSize())));
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
		if (SettlementBounty != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = SettlementBounty.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += NameRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		ptr += OrgInfo.Serialize(ptr);
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = LeftMaxHealth;
		ptr += 2;
		int num2 = FullBlockName.Serialize(ptr);
		ptr += num2;
		Tester.Assert(num2 <= 65535);
		*(short*)ptr = RandomNameId;
		ptr += 2;
		*ptr = (byte)HunterState;
		ptr++;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (SettlementBounty == null)
			{
				SettlementBounty = new SettlementBounty();
			}
			ptr += SettlementBounty.Deserialize(ptr);
		}
		else
		{
			SettlementBounty = null;
		}
		ptr += NameRelatedData.Deserialize(ptr);
		if (AvatarRelatedData == null)
		{
			AvatarRelatedData = new AvatarRelatedData();
		}
		ptr += AvatarRelatedData.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		ptr += OrgInfo.Deserialize(ptr);
		Health = *(short*)ptr;
		ptr += 2;
		LeftMaxHealth = *(short*)ptr;
		ptr += 2;
		ptr += FullBlockName.Deserialize(ptr);
		RandomNameId = *(short*)ptr;
		ptr += 2;
		HunterState = (sbyte)(*ptr);
		ptr++;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
