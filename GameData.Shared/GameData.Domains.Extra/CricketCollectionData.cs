using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Extra;

[SerializableGameData(IsExtensible = true)]
public class CricketCollectionData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CollectionCrickets = 0;

		public const ushort CollectionCricketJars = 1;

		public const ushort CollectionCricketRegen = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "Cricket", "CricketJar", "CricketRegen" };
	}

	public const int CricketCollectionCapacity = 17;

	[SerializableGameDataField]
	public ItemKey Cricket;

	[SerializableGameDataField]
	public ItemKey CricketJar;

	[SerializableGameDataField]
	public int CricketRegen;

	public CricketCollectionData(ItemKey crickets, ItemKey cricketJar, int cricketRegen)
	{
		Cricket = crickets;
		CricketJar = cricketJar;
		CricketRegen = cricketRegen;
	}

	public CricketCollectionData()
	{
	}

	public CricketCollectionData(CricketCollectionData other)
	{
		Cricket = other.Cricket;
		CricketJar = other.CricketJar;
		CricketRegen = other.CricketRegen;
	}

	public void Assign(CricketCollectionData other)
	{
		Cricket = other.Cricket;
		CricketJar = other.CricketJar;
		CricketRegen = other.CricketRegen;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
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
		*(short*)ptr = 3;
		ptr += 2;
		ptr += Cricket.Serialize(ptr);
		ptr += CricketJar.Serialize(ptr);
		*(int*)ptr = CricketRegen;
		ptr += 4;
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += Cricket.Deserialize(ptr);
		}
		if (num > 1)
		{
			ptr += CricketJar.Deserialize(ptr);
		}
		if (num > 2)
		{
			CricketRegen = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
