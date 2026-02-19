using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true)]
public class PossessionData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort SoulCharId = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "SoulCharId" };
	}

	[SerializableGameDataField]
	public int SoulCharId;

	public PossessionData(int soulCharId)
	{
		SoulCharId = soulCharId;
	}

	public PossessionData()
	{
	}

	public PossessionData(PossessionData other)
	{
		SoulCharId = other.SoulCharId;
	}

	public void Assign(PossessionData other)
	{
		SoulCharId = other.SoulCharId;
	}

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
		*(int*)num = SoulCharId;
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
			SoulCharId = *(int*)ptr;
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
