using GameData.Serializer;

namespace GameData.Domains.Map;

public struct SettlementInfo : ISerializableGameData
{
	public short SettlementId;

	public short BlockId;

	public sbyte OrgTemplateId;

	public short RandomNameId;

	public SettlementInfo(short settlementId, short blockId, sbyte orgTemplateId, short randomNameId)
	{
		SettlementId = settlementId;
		BlockId = blockId;
		OrgTemplateId = orgTemplateId;
		RandomNameId = randomNameId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 8;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = SettlementId;
		((short*)pData)[1] = BlockId;
		pData[4] = (byte)OrgTemplateId;
		((short*)pData)[3] = RandomNameId;
		return 8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		SettlementId = *(short*)pData;
		BlockId = ((short*)pData)[1];
		OrgTemplateId = (sbyte)pData[4];
		RandomNameId = ((short*)pData)[3];
		return 8;
	}
}
