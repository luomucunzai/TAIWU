using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Organization;

[AutoGenerateSerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class SettlementMemberFeature : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort FeatureId = 0;

		public const ushort MinGrade = 1;

		public const ushort MaxGrade = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "FeatureId", "MinGrade", "MaxGrade" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public short FeatureId;

	[SerializableGameDataField(FieldIndex = 1)]
	public sbyte MinGrade;

	[SerializableGameDataField(FieldIndex = 2)]
	public sbyte MaxGrade;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 3;
		ptr += 2;
		*(short*)ptr = FeatureId;
		ptr += 2;
		*ptr = (byte)MinGrade;
		ptr++;
		*ptr = (byte)MaxGrade;
		ptr++;
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
			FeatureId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			MinGrade = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 2)
		{
			MaxGrade = (sbyte)(*ptr);
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
