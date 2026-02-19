using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Taiwu;

public class VillagerWorkData : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharacterId = -1;

	[SerializableGameDataField]
	public sbyte WorkType = -1;

	[SerializableGameDataField]
	public short AreaId = -1;

	[SerializableGameDataField]
	public short BlockId = -1;

	[SerializableGameDataField]
	public short BlockTemplateId = -1;

	[SerializableGameDataField]
	public short BuildingBlockIndex = -1;

	[SerializableGameDataField]
	public sbyte WorkerIndex = -1;

	[SerializableGameDataField]
	public sbyte ResourceType = -1;

	[SerializableGameDataField]
	public int GraveId = -1;

	public Location Location => new Location(AreaId, BlockId);

	public VillagerWorkData(int characterId, sbyte workType, short areaId, short blockId)
	{
		CharacterId = characterId;
		WorkType = workType;
		AreaId = areaId;
		BlockId = blockId;
		BuildingBlockIndex = 255;
		BlockTemplateId = (short)((blockId < 0) ? (-1) : (ExternalDataBridge.Context.GetBlockData(Location)?.TemplateId ?? (-1)));
		WorkerIndex = -1;
		ResourceType = -1;
	}

	public VillagerWorkData()
	{
	}

	public VillagerWorkData(VillagerWorkData other)
	{
		CharacterId = other.CharacterId;
		WorkType = other.WorkType;
		AreaId = other.AreaId;
		BlockId = other.BlockId;
		BlockTemplateId = other.BlockTemplateId;
		BuildingBlockIndex = other.BuildingBlockIndex;
		WorkerIndex = other.WorkerIndex;
		ResourceType = other.ResourceType;
		GraveId = other.GraveId;
	}

	public void Assign(VillagerWorkData other)
	{
		CharacterId = other.CharacterId;
		WorkType = other.WorkType;
		AreaId = other.AreaId;
		BlockId = other.BlockId;
		BlockTemplateId = other.BlockTemplateId;
		BuildingBlockIndex = other.BuildingBlockIndex;
		WorkerIndex = other.WorkerIndex;
		ResourceType = other.ResourceType;
		GraveId = other.GraveId;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 19;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = CharacterId;
		byte* num = pData + 4;
		*num = (byte)WorkType;
		byte* num2 = num + 1;
		*(short*)num2 = AreaId;
		byte* num3 = num2 + 2;
		*(short*)num3 = BlockId;
		byte* num4 = num3 + 2;
		*(short*)num4 = BlockTemplateId;
		byte* num5 = num4 + 2;
		*(short*)num5 = BuildingBlockIndex;
		byte* num6 = num5 + 2;
		*num6 = (byte)WorkerIndex;
		byte* num7 = num6 + 1;
		*num7 = (byte)ResourceType;
		byte* num8 = num7 + 1;
		*(int*)num8 = GraveId;
		int num9 = (int)(num8 + 4 - pData);
		if (num9 > 4)
		{
			return (num9 + 3) / 4 * 4;
		}
		return num9;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		WorkType = (sbyte)(*ptr);
		ptr++;
		AreaId = *(short*)ptr;
		ptr += 2;
		BlockId = *(short*)ptr;
		ptr += 2;
		BlockTemplateId = *(short*)ptr;
		ptr += 2;
		BuildingBlockIndex = *(short*)ptr;
		ptr += 2;
		WorkerIndex = (sbyte)(*ptr);
		ptr++;
		ResourceType = (sbyte)(*ptr);
		ptr++;
		GraveId = *(int*)ptr;
		ptr += 4;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
