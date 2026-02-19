using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

[SerializableGameData]
public class JiaoEvolutionDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public int JiaoId;

	[SerializableGameDataField]
	public short SimulationResult;

	[SerializableGameDataField]
	public bool IsOwnedResult;

	[SerializableGameDataField]
	public sbyte Status;

	[SerializableGameDataField]
	public ItemDisplayData ItemDisplayData;

	public JiaoEvolutionDisplayData()
	{
		JiaoId = -1;
		SimulationResult = -1;
		IsOwnedResult = false;
		Status = -1;
		ItemDisplayData = null;
	}

	public JiaoEvolutionDisplayData(int jiaoId, short simulationResult, bool isOwnedResult, sbyte status, ItemDisplayData itemDisplayData)
	{
		JiaoId = jiaoId;
		SimulationResult = simulationResult;
		IsOwnedResult = isOwnedResult;
		Status = status;
		ItemDisplayData = itemDisplayData;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 8;
		num = ((ItemDisplayData == null) ? (num + 2) : (num + (2 + ItemDisplayData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = JiaoId;
		ptr += 4;
		*(short*)ptr = SimulationResult;
		ptr += 2;
		*ptr = (IsOwnedResult ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (byte)Status;
		ptr++;
		if (ItemDisplayData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = ItemDisplayData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
		JiaoId = *(int*)ptr;
		ptr += 4;
		SimulationResult = *(short*)ptr;
		ptr += 2;
		IsOwnedResult = *ptr != 0;
		ptr++;
		Status = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (ItemDisplayData == null)
			{
				ItemDisplayData = new ItemDisplayData();
			}
			ptr += ItemDisplayData.Deserialize(ptr);
		}
		else
		{
			ItemDisplayData = null;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
