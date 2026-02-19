using GameData.Domains.Adventure;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true, NotForArchive = true, NoCopyConstructors = true)]
public class CharacterLocationDisplayData : ISerializableGameData
{
	public enum EDisplayType
	{
		Normal,
		Kidnapped,
		InAdventure,
		Buried
	}

	[SerializableGameDataField]
	public int CharacterId;

	[SerializableGameDataField]
	public sbyte DisplayType;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public FullBlockName FullBlockName;

	[SerializableGameDataField]
	public MapBlockData BlockData;

	[SerializableGameDataField]
	public MapBlockData RootBlockData;

	[SerializableGameDataField]
	public AdventureSiteData AdventureSite;

	[SerializableGameDataField]
	public CharacterDisplayData Kidnapper;

	[SerializableGameDataField]
	public bool IsCapturedInStoneRoom;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 22;
		num += FullBlockName.GetSerializedSize();
		num = ((BlockData == null) ? (num + 2) : (num + (2 + BlockData.GetSerializedSize())));
		num = ((RootBlockData == null) ? (num + 2) : (num + (2 + RootBlockData.GetSerializedSize())));
		num = ((Kidnapper == null) ? (num + 2) : (num + (2 + Kidnapper.GetSerializedSize())));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = CharacterId;
		ptr += 4;
		*ptr = (byte)DisplayType;
		ptr++;
		ptr += Location.Serialize(ptr);
		int num = FullBlockName.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		if (BlockData != null)
		{
			byte* intPtr = ptr;
			ptr += 2;
			int num2 = BlockData.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= 65535);
			*(ushort*)intPtr = (ushort)num2;
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
			int num3 = RootBlockData.Serialize(ptr);
			ptr += num3;
			Tester.Assert(num3 <= 65535);
			*(ushort*)intPtr2 = (ushort)num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += AdventureSite.Serialize(ptr);
		if (Kidnapper != null)
		{
			byte* intPtr3 = ptr;
			ptr += 2;
			int num4 = Kidnapper.Serialize(ptr);
			ptr += num4;
			Tester.Assert(num4 <= 65535);
			*(ushort*)intPtr3 = (ushort)num4;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (IsCapturedInStoneRoom ? ((byte)1) : ((byte)0));
		ptr++;
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		CharacterId = *(int*)ptr;
		ptr += 4;
		DisplayType = (sbyte)(*ptr);
		ptr++;
		ptr += Location.Deserialize(ptr);
		ptr += FullBlockName.Deserialize(ptr);
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
		if (AdventureSite == null)
		{
			AdventureSite = new AdventureSiteData();
		}
		ptr += AdventureSite.Deserialize(ptr);
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (Kidnapper == null)
			{
				Kidnapper = new CharacterDisplayData();
			}
			ptr += Kidnapper.Deserialize(ptr);
		}
		else
		{
			Kidnapper = null;
		}
		IsCapturedInStoneRoom = *ptr != 0;
		ptr++;
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
