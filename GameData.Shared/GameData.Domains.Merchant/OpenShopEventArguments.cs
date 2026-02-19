using GameData.Serializer;

namespace GameData.Domains.Merchant;

public class OpenShopEventArguments : ISerializableGameData
{
	public enum EMerchantSourceType
	{
		None = -1,
		NormalCharacter,
		MerchantHeadBuilding,
		MerchantBranchBuilding,
		SettlementTreasury,
		SpecialBuilding,
		NormalCaravan,
		AdventureCaravan,
		ProfessionSkillCaravan,
		SpecifiedOnBuildingMerchantType
	}

	[SerializableGameDataField]
	public int Id = -1;

	[SerializableGameDataField]
	public sbyte BuildingMerchantType = -1;

	[SerializableGameDataField]
	public bool Refresh;

	[SerializableGameDataField]
	public bool IgnoreWorldProgress;

	[SerializableGameDataField]
	public bool IgnoreFavorability;

	[SerializableGameDataField]
	public short SettlementId = -1;

	[SerializableGameDataField]
	public sbyte MerchantSourceType = -1;

	[SerializableGameDataField]
	public sbyte CurrPage;

	public EMerchantSourceType MerchantSourceTypeEnum => (EMerchantSourceType)MerchantSourceType;

	public bool IsSettlementTreasury => MerchantSourceTypeEnum == EMerchantSourceType.SettlementTreasury;

	public bool IsFromBuilding
	{
		get
		{
			EMerchantSourceType merchantSourceTypeEnum = MerchantSourceTypeEnum;
			if ((uint)(merchantSourceTypeEnum - 1) <= 1u)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsHeadBuildingMerchant => MerchantSourceTypeEnum == EMerchantSourceType.MerchantHeadBuilding;

	public bool IsCaravan
	{
		get
		{
			EMerchantSourceType merchantSourceTypeEnum = MerchantSourceTypeEnum;
			if ((uint)(merchantSourceTypeEnum - 5) <= 2u)
			{
				return true;
			}
			return false;
		}
	}

	public bool IsSpecialBuilding => MerchantSourceTypeEnum == EMerchantSourceType.SpecialBuilding;

	public OpenShopEventArguments()
	{
	}

	public OpenShopEventArguments(OpenShopEventArguments other)
	{
		Id = other.Id;
		BuildingMerchantType = other.BuildingMerchantType;
		Refresh = other.Refresh;
		IgnoreWorldProgress = other.IgnoreWorldProgress;
		IgnoreFavorability = other.IgnoreFavorability;
		SettlementId = other.SettlementId;
		MerchantSourceType = other.MerchantSourceType;
		CurrPage = other.CurrPage;
	}

	public void Assign(OpenShopEventArguments other)
	{
		Id = other.Id;
		BuildingMerchantType = other.BuildingMerchantType;
		Refresh = other.Refresh;
		IgnoreWorldProgress = other.IgnoreWorldProgress;
		IgnoreFavorability = other.IgnoreFavorability;
		SettlementId = other.SettlementId;
		MerchantSourceType = other.MerchantSourceType;
		CurrPage = other.CurrPage;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 12;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = Id;
		byte* num = pData + 4;
		*num = (byte)BuildingMerchantType;
		byte* num2 = num + 1;
		*num2 = (Refresh ? ((byte)1) : ((byte)0));
		byte* num3 = num2 + 1;
		*num3 = (IgnoreWorldProgress ? ((byte)1) : ((byte)0));
		byte* num4 = num3 + 1;
		*num4 = (IgnoreFavorability ? ((byte)1) : ((byte)0));
		byte* num5 = num4 + 1;
		*(short*)num5 = SettlementId;
		byte* num6 = num5 + 2;
		*num6 = (byte)MerchantSourceType;
		byte* num7 = num6 + 1;
		*num7 = (byte)CurrPage;
		int num8 = (int)(num7 + 1 - pData);
		if (num8 > 4)
		{
			return (num8 + 3) / 4 * 4;
		}
		return num8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Id = *(int*)ptr;
		ptr += 4;
		BuildingMerchantType = (sbyte)(*ptr);
		ptr++;
		Refresh = *ptr != 0;
		ptr++;
		IgnoreWorldProgress = *ptr != 0;
		ptr++;
		IgnoreFavorability = *ptr != 0;
		ptr++;
		SettlementId = *(short*)ptr;
		ptr += 2;
		MerchantSourceType = (sbyte)(*ptr);
		ptr++;
		CurrPage = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
