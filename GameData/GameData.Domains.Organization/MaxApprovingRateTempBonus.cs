using GameData.Serializer;

namespace GameData.Domains.Organization;

[SerializableGameData(NotForDisplayModule = true)]
public struct MaxApprovingRateTempBonus : ISerializableGameData
{
	[SerializableGameDataField]
	public short SettlementId;

	[SerializableGameDataField]
	public short Bonus;

	[SerializableGameDataField]
	public int ExpireDate;

	public MaxApprovingRateTempBonus(short settlementId, short bonus, int expireDate)
	{
		SettlementId = settlementId;
		Bonus = bonus;
		ExpireDate = expireDate;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = SettlementId;
		ptr += 2;
		*(short*)ptr = Bonus;
		ptr += 2;
		*(int*)ptr = ExpireDate;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SettlementId = *(short*)ptr;
		ptr += 2;
		Bonus = *(short*)ptr;
		ptr += 2;
		ExpireDate = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
