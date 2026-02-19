using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Building;

public struct MakeConditionArguments : ISerializableGameData
{
	[SerializableGameDataField]
	public int CharId;

	[SerializableGameDataField]
	public BuildingBlockKey BuildingBlockKey;

	[SerializableGameDataField]
	public ItemKey ToolKey;

	[SerializableGameDataField]
	public ItemKey MaterialKey;

	[SerializableGameDataField]
	public short MakeCount;

	[SerializableGameDataField]
	public ResourceInts ResourceCount;

	[SerializableGameDataField]
	public short MakeItemTypeId;

	[SerializableGameDataField]
	public short MakeItemSubTypeId;

	[SerializableGameDataField]
	public bool IsManual;

	[SerializableGameDataField]
	public bool IsPerfect;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 68;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharId;
		ptr += 4;
		ptr += BuildingBlockKey.Serialize(ptr);
		ptr += ToolKey.Serialize(ptr);
		ptr += MaterialKey.Serialize(ptr);
		*(short*)ptr = MakeCount;
		ptr += 2;
		ptr += ResourceCount.Serialize(ptr);
		*(short*)ptr = MakeItemTypeId;
		ptr += 2;
		*(short*)ptr = MakeItemSubTypeId;
		ptr += 2;
		*ptr = (IsManual ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (IsPerfect ? ((byte)1) : ((byte)0));
		ptr++;
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
		CharId = *(int*)ptr;
		ptr += 4;
		ptr += BuildingBlockKey.Deserialize(ptr);
		ptr += ToolKey.Deserialize(ptr);
		ptr += MaterialKey.Deserialize(ptr);
		MakeCount = *(short*)ptr;
		ptr += 2;
		ptr += ResourceCount.Deserialize(ptr);
		MakeItemTypeId = *(short*)ptr;
		ptr += 2;
		MakeItemSubTypeId = *(short*)ptr;
		ptr += 2;
		IsManual = *ptr != 0;
		ptr++;
		IsPerfect = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
