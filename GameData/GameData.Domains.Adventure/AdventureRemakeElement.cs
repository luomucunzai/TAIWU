using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Adventure;

[AutoGenerateSerializableGameData(IsExtensible = true)]
public class AdventureRemakeElement : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort Location = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "Location" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public Location Location;

	public AdventureRemakeElement()
	{
	}

	public AdventureRemakeElement(AdventureRemakeElement other)
	{
		Location = other.Location;
	}

	public void Assign(AdventureRemakeElement other)
	{
		Location = other.Location;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += Location.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		int num = Location.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ptr += Location.Deserialize(ptr);
		}
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}
}
