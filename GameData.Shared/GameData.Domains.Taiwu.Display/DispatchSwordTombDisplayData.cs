using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true, NotForArchive = true, NoCopyConstructors = true)]
public class DispatchSwordTombDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Id;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public MapBlockData BlockData;

	[SerializableGameDataField]
	public MapBlockData RootBlockData;

	[SerializableGameDataField]
	public short RemainingMonths;

	[SerializableGameDataField]
	public sbyte EscapeState;

	[SerializableGameDataField]
	public short KeeperCount;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((BlockData == null) ? (num + 2) : (num + (2 + BlockData.GetSerializedSize())));
		num = ((RootBlockData == null) ? (num + 2) : (num + (2 + RootBlockData.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)Id;
		ptr++;
		ptr += Location.Serialize(ptr);
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
		if (RootBlockData != null)
		{
			byte* intPtr2 = ptr;
			ptr += 2;
			int num2 = RootBlockData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr2 = (ushort)num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = RemainingMonths;
		ptr += 2;
		*ptr = (byte)EscapeState;
		ptr++;
		*(short*)ptr = KeeperCount;
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
		Id = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
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
			if (RootBlockData == null)
			{
				RootBlockData = new MapBlockData();
			}
			ptr += RootBlockData.Deserialize(ptr);
		}
		else
		{
			RootBlockData = null;
		}
		RemainingMonths = *(short*)ptr;
		ptr += 2;
		EscapeState = (sbyte)(*ptr);
		ptr++;
		KeeperCount = *(short*)ptr;
		ptr += 2;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
