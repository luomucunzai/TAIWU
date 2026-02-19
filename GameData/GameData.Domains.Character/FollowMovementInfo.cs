using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class FollowMovementInfo : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort TargetCharId = 0;

		public const ushort Distance = 1;

		public const ushort Count = 2;

		public static readonly string[] FieldId2FieldName = new string[2] { "TargetCharId", "Distance" };
	}

	[SerializableGameDataField]
	public int TargetCharId;

	[SerializableGameDataField]
	public int Distance;

	public FollowMovementInfo(int charId, int distance)
	{
		TargetCharId = charId;
		Distance = distance;
	}

	public FollowMovementInfo()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 2;
		ptr += 2;
		*(int*)ptr = TargetCharId;
		ptr += 4;
		*(int*)ptr = Distance;
		ptr += 4;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			TargetCharId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			Distance = *(int*)ptr;
			ptr += 4;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
