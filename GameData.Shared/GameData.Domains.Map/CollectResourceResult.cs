using GameData.Domains.Item.Display;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

public struct CollectResourceResult : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte ResourceType;

	[SerializableGameDataField]
	public short ResourceCount;

	[SerializableGameDataField]
	public ItemDisplayData ItemDisplayData;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
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
		*ptr = (byte)ResourceType;
		ptr++;
		*(short*)ptr = ResourceCount;
		ptr += 2;
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
		ResourceType = (sbyte)(*ptr);
		ptr++;
		ResourceCount = *(short*)ptr;
		ptr += 2;
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
