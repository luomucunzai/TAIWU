using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Character.Display;

[AutoGenerateSerializableGameData]
public class CharacterDisplayDataForSettlementPrisoner : ISerializableGameData
{
	[SerializableGameDataField]
	public SettlementPrisoner SettlementPrisoner;

	[SerializableGameDataField]
	public int Resistance;

	[SerializableGameDataField]
	public int EscapeRate;

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
	public short RandomNameId = -1;

	[SerializableGameDataField]
	public bool CompletelyInfected;

	[SerializableGameDataField]
	public short Happiness;

	[SerializableGameDataField]
	public short FavorabilityToTaiwu;

	[SerializableGameDataField]
	public short Health;

	[SerializableGameDataField]
	public short LeftMaxHealth;

	public PrisonType PrisonType
	{
		get
		{
			if (SettlementPrisoner.PunishmentSeverity == 0)
			{
				return PrisonType.Invalid;
			}
			if (SettlementPrisoner.PunishmentType == 39)
			{
				return PrisonType.Infected;
			}
			return (PrisonType)(OrgInfo.Grade / 3);
		}
	}

	public CharacterDisplayDataForSettlementPrisoner()
	{
	}

	public CharacterDisplayDataForSettlementPrisoner(CharacterDisplayDataForSettlementPrisoner other)
	{
		SettlementPrisoner = new SettlementPrisoner(other.SettlementPrisoner);
		Resistance = other.Resistance;
		EscapeRate = other.EscapeRate;
		NameRelatedData = other.NameRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		CurrAge = other.CurrAge;
		Gender = other.Gender;
		OrgInfo = other.OrgInfo;
		RandomNameId = other.RandomNameId;
		CompletelyInfected = other.CompletelyInfected;
		Happiness = other.Happiness;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		Health = other.Health;
		LeftMaxHealth = other.LeftMaxHealth;
	}

	public void Assign(CharacterDisplayDataForSettlementPrisoner other)
	{
		SettlementPrisoner = new SettlementPrisoner(other.SettlementPrisoner);
		Resistance = other.Resistance;
		EscapeRate = other.EscapeRate;
		NameRelatedData = other.NameRelatedData;
		AvatarRelatedData = new AvatarRelatedData(other.AvatarRelatedData);
		CurrAge = other.CurrAge;
		Gender = other.Gender;
		OrgInfo = other.OrgInfo;
		RandomNameId = other.RandomNameId;
		CompletelyInfected = other.CompletelyInfected;
		Happiness = other.Happiness;
		FavorabilityToTaiwu = other.FavorabilityToTaiwu;
		Health = other.Health;
		LeftMaxHealth = other.LeftMaxHealth;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		num = ((SettlementPrisoner == null) ? (num + 2) : (num + (2 + SettlementPrisoner.GetSerializedSize())));
		num += NameRelatedData.GetSerializedSize();
		num = ((AvatarRelatedData == null) ? (num + 2) : (num + (2 + AvatarRelatedData.GetSerializedSize())));
		num += OrgInfo.GetSerializedSize();
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (SettlementPrisoner != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = SettlementPrisoner.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(int*)ptr = Resistance;
		ptr += 4;
		*(int*)ptr = EscapeRate;
		ptr += 4;
		ptr += NameRelatedData.Serialize(ptr);
		ptr += AvatarRelatedData.Serialize(ptr);
		*(short*)ptr = CurrAge;
		ptr += 2;
		*ptr = (byte)Gender;
		ptr++;
		ptr += OrgInfo.Serialize(ptr);
		*(short*)ptr = RandomNameId;
		ptr += 2;
		*ptr = (CompletelyInfected ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = Happiness;
		ptr += 2;
		*(short*)ptr = FavorabilityToTaiwu;
		ptr += 2;
		*(short*)ptr = Health;
		ptr += 2;
		*(short*)ptr = LeftMaxHealth;
		ptr += 2;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			SettlementPrisoner = new SettlementPrisoner();
			ptr += SettlementPrisoner.Deserialize(ptr);
		}
		else
		{
			SettlementPrisoner = null;
		}
		Resistance = *(int*)ptr;
		ptr += 4;
		EscapeRate = *(int*)ptr;
		ptr += 4;
		ptr += NameRelatedData.Deserialize(ptr);
		AvatarRelatedData = new AvatarRelatedData();
		ptr += AvatarRelatedData.Deserialize(ptr);
		CurrAge = *(short*)ptr;
		ptr += 2;
		Gender = (sbyte)(*ptr);
		ptr++;
		ptr += OrgInfo.Deserialize(ptr);
		RandomNameId = *(short*)ptr;
		ptr += 2;
		CompletelyInfected = *ptr != 0;
		ptr++;
		Happiness = *(short*)ptr;
		ptr += 2;
		FavorabilityToTaiwu = *(short*)ptr;
		ptr += 2;
		Health = *(short*)ptr;
		ptr += 2;
		LeftMaxHealth = *(short*)ptr;
		ptr += 2;
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
