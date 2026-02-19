using Config;
using GameData.Serializer;

namespace GameData.Domains.Organization.Display;

public struct SettlementNameRelatedData : ISerializableGameData
{
	public short RandomNameId;

	public short MapBlockTemplateId;

	public SettlementNameRelatedData(short randomNameId, short mapBlockTemplateId)
	{
		RandomNameId = randomNameId;
		MapBlockTemplateId = mapBlockTemplateId;
	}

	public string GetName()
	{
		if (RandomNameId != -1)
		{
			return LocalTownNames.Instance.TownNameCore[RandomNameId].Name;
		}
		if (MapBlockTemplateId == -1)
		{
			return Config.Organization.Instance[(sbyte)0].Name;
		}
		if (MapBlockTemplateId == 17 || MapBlockTemplateId == 18)
		{
			return MapArea.Instance[(short)136].Name;
		}
		return MapBlock.Instance[MapBlockTemplateId].Name;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 4;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = RandomNameId;
		((short*)pData)[1] = MapBlockTemplateId;
		return 4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		RandomNameId = *(short*)pData;
		MapBlockTemplateId = ((short*)pData)[1];
		return 4;
	}
}
