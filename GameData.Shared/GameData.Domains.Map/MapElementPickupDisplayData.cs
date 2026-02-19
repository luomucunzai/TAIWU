using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public class MapElementPickupDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public MapPickup Pickup;

	[SerializableGameDataField]
	public uint BanReason;

	[SerializableGameDataField]
	public bool CanAutoBeatXiangshuMinion;

	[SerializableGameDataField]
	public short TaiwuLoopingNeigong;

	[SerializableGameDataField]
	public ItemKey TaiwuReadingBookKey;

	public bool CanAutoTrigger()
	{
		return BanReason == 0;
	}

	public bool NeedBattle()
	{
		if (Pickup.HasXiangshuMinion)
		{
			return !CanAutoBeatXiangshuMinion;
		}
		return false;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 15;
		num = ((Pickup == null) ? (num + 2) : (num + (2 + Pickup.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Pickup != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num = Pickup.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= 65535);
			*(ushort*)intPtr = (ushort)num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(uint*)ptr = BanReason;
		ptr += 4;
		*ptr = (CanAutoBeatXiangshuMinion ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = TaiwuLoopingNeigong;
		ptr += 2;
		ptr += TaiwuReadingBookKey.Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Pickup == null)
			{
				Pickup = new MapPickup();
			}
			ptr += Pickup.Deserialize(ptr);
		}
		else
		{
			Pickup = null;
		}
		BanReason = *(uint*)ptr;
		ptr += 4;
		CanAutoBeatXiangshuMinion = *ptr != 0;
		ptr++;
		TaiwuLoopingNeigong = *(short*)ptr;
		ptr += 2;
		ptr += TaiwuReadingBookKey.Deserialize(ptr);
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
