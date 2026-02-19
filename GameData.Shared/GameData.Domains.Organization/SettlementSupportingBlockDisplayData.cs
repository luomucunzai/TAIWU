using System;
using GameData.Domains.Building;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization;

[SerializableGameData]
[Obsolete]
public struct SettlementSupportingBlockDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int SettlementId;

	[SerializableGameDataField]
	public BuildingBlockKey Key;

	[SerializableGameDataField]
	public bool CanUpgrade;

	[SerializableGameDataField]
	public bool CanDownGrade;

	[SerializableGameDataField]
	public int Safety;

	[SerializableGameDataField]
	public int SafetyMax;

	[SerializableGameDataField]
	public int SupportLevel;

	[SerializableGameDataField]
	public int SupportLevelMax;

	[SerializableGameDataField]
	public IntPair SettlementInfo;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 38;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = SettlementId;
		ptr += 4;
		ptr += Key.Serialize(ptr);
		*ptr = (CanUpgrade ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (CanDownGrade ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = Safety;
		ptr += 4;
		*(int*)ptr = SafetyMax;
		ptr += 4;
		*(int*)ptr = SupportLevel;
		ptr += 4;
		*(int*)ptr = SupportLevelMax;
		ptr += 4;
		ptr += SettlementInfo.Serialize(ptr);
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
		SettlementId = *(int*)ptr;
		ptr += 4;
		ptr += Key.Deserialize(ptr);
		CanUpgrade = *ptr != 0;
		ptr++;
		CanDownGrade = *ptr != 0;
		ptr++;
		Safety = *(int*)ptr;
		ptr += 4;
		SafetyMax = *(int*)ptr;
		ptr += 4;
		SupportLevel = *(int*)ptr;
		ptr += 4;
		SupportLevelMax = *(int*)ptr;
		ptr += 4;
		ptr += SettlementInfo.Deserialize(ptr);
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
