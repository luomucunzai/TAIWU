using GameData.Serializer;

namespace GameData.Domains.Item;

[SerializableGameData]
public struct MultiplyOperation : ISerializableGameData
{
	[SerializableGameDataField]
	public ItemKey Target;

	[SerializableGameDataField]
	public int Count;

	[SerializableGameDataField]
	public ItemKey Tool;

	[SerializableGameDataField]
	public sbyte TargetItemSourceType;

	[SerializableGameDataField]
	public sbyte ToolItemSourceType;

	public MultiplyOperation(ItemKey target, ItemKey tool, int count, sbyte targetItemSourceType, sbyte toolItemSourceType)
	{
		Target = target;
		Tool = tool;
		Count = count;
		TargetItemSourceType = targetItemSourceType;
		ToolItemSourceType = toolItemSourceType;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += Target.Serialize(ptr);
		*(int*)ptr = Count;
		ptr += 4;
		ptr += Tool.Serialize(ptr);
		*ptr = (byte)TargetItemSourceType;
		ptr++;
		*ptr = (byte)ToolItemSourceType;
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
		ptr += Target.Deserialize(ptr);
		Count = *(int*)ptr;
		ptr += 4;
		ptr += Tool.Deserialize(ptr);
		TargetItemSourceType = (sbyte)(*ptr);
		ptr++;
		ToolItemSourceType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
