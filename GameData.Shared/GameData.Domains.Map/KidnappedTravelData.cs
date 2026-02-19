using GameData.Serializer;

namespace GameData.Domains.Map;

[SerializableGameData(IsExtensible = true)]
public class KidnappedTravelData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Target = 0;

		public const ushort HunterCharId = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "Target", "HunterCharId" };
	}

	public static readonly KidnappedTravelData Invalid = new KidnappedTravelData();

	[SerializableGameDataField]
	public Location Target = Location.Invalid;

	[SerializableGameDataField]
	public int HunterCharId = -1;

	public bool Valid
	{
		get
		{
			if (Target.IsValid())
			{
				return HunterCharId >= 0;
			}
			return false;
		}
	}

	public KidnappedTravelData()
	{
	}

	public KidnappedTravelData(KidnappedTravelData other)
	{
		Target = other.Target;
		HunterCharId = other.HunterCharId;
	}

	public void Assign(KidnappedTravelData other)
	{
		Target = other.Target;
		HunterCharId = other.HunterCharId;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		ptr += Target.Serialize(ptr);
		*(int*)ptr = HunterCharId;
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
			ptr += Target.Deserialize(ptr);
		}
		if (num > 1)
		{
			HunterCharId = *(int*)ptr;
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
