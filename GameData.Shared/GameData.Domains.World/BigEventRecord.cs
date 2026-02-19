using GameData.Serializer;

namespace GameData.Domains.World;

[SerializableGameData(NoCopyConstructors = true, IsExtensible = true)]
public class BigEventRecord : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort OccurDate = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "OccurDate" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public int OccurDate;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 1;
		byte* num = pData + 2;
		*(int*)num = OccurDate;
		int num2 = (int)(num + 4 - pData);
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
			OccurDate = *(int*)ptr;
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
