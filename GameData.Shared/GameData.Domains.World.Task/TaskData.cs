using Config;
using GameData.Serializer;

namespace GameData.Domains.World.Task;

public struct TaskData : ISerializableGameData
{
	[SerializableGameDataField]
	public int TaskInfoId;

	[SerializableGameDataField]
	public int TaskChainId;

	[SerializableGameDataField]
	public byte TaskStatus;

	public bool IsBlocked => TaskStatus == 1;

	public bool IsParallel => TaskChain.Instance[TaskChainId].Type == ETaskChainType.Parallel;

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(int*)pData = TaskInfoId;
		byte* num = pData + 4;
		*(int*)num = TaskChainId;
		byte* num2 = num + 4;
		*num2 = TaskStatus;
		int num3 = (int)(num2 + 1 - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		TaskInfoId = *(int*)ptr;
		ptr += 4;
		TaskChainId = *(int*)ptr;
		ptr += 4;
		TaskStatus = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
