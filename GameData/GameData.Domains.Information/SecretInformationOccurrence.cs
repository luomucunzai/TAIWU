using GameData.Serializer;

namespace GameData.Domains.Information;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true)]
public class SecretInformationOccurrence : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Count = 0;

		public static readonly string[] FieldId2FieldName = new string[0];
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 0;
		ptr += 2;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
