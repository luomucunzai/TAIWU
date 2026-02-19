using System;
using GameData.Serializer;

namespace GameData.Domains.Organization;

public struct MartialArtTournamentPreparationInfo : IComparable<MartialArtTournamentPreparationInfo>, ISerializableGameData
{
	[SerializableGameDataField]
	public short SettlementId;

	[SerializableGameDataField]
	public int TotalScore;

	[SerializableGameDataField]
	public int CombatPowerPreparation;

	[SerializableGameDataField]
	public int AuthorityPreparation;

	[SerializableGameDataField]
	public int ResourcePreparation;

	public int CompareTo(MartialArtTournamentPreparationInfo other)
	{
		if (TotalScore != other.TotalScore)
		{
			return TotalScore.CompareTo(other.TotalScore);
		}
		if (CombatPowerPreparation != other.CombatPowerPreparation)
		{
			return CombatPowerPreparation.CompareTo(other.CombatPowerPreparation);
		}
		if (AuthorityPreparation != other.AuthorityPreparation)
		{
			return AuthorityPreparation.CompareTo(other.AuthorityPreparation);
		}
		if (ResourcePreparation != other.ResourcePreparation)
		{
			return ResourcePreparation.CompareTo(other.ResourcePreparation);
		}
		return SettlementId.CompareTo(other.SettlementId);
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 18;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = SettlementId;
		byte* num = pData + 2;
		*(int*)num = TotalScore;
		byte* num2 = num + 4;
		*(int*)num2 = CombatPowerPreparation;
		byte* num3 = num2 + 4;
		*(int*)num3 = AuthorityPreparation;
		byte* num4 = num3 + 4;
		*(int*)num4 = ResourcePreparation;
		int num5 = (int)(num4 + 4 - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SettlementId = *(short*)ptr;
		ptr += 2;
		TotalScore = *(int*)ptr;
		ptr += 4;
		CombatPowerPreparation = *(int*)ptr;
		ptr += 4;
		AuthorityPreparation = *(int*)ptr;
		ptr += 4;
		ResourcePreparation = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
