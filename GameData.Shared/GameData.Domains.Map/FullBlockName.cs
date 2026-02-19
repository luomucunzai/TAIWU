using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData]
public struct FullBlockName : ISerializableGameData
{
	[SerializableGameDataField]
	public MapBlockData BlockData;

	[SerializableGameDataField]
	public MapBlockData BelongBlockData;

	[SerializableGameDataField]
	public sbyte stateTemplateId;

	[SerializableGameDataField]
	public short areaTemplateId;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num = ((BlockData == null) ? (num + 2) : (num + (2 + BlockData.GetSerializedSize())));
		num = ((BelongBlockData == null) ? (num + 2) : (num + (2 + BelongBlockData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (BlockData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = BlockData.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BelongBlockData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = BelongBlockData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)stateTemplateId;
		ptr++;
		*(short*)ptr = areaTemplateId;
		ptr += 2;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (BlockData == null)
			{
				BlockData = new MapBlockData();
			}
			ptr += BlockData.Deserialize(ptr);
		}
		else
		{
			BlockData = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (BelongBlockData == null)
			{
				BelongBlockData = new MapBlockData();
			}
			ptr += BelongBlockData.Deserialize(ptr);
		}
		else
		{
			BelongBlockData = null;
		}
		stateTemplateId = (sbyte)(*ptr);
		ptr++;
		areaTemplateId = *(short*)ptr;
		ptr += 2;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
