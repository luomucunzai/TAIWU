using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class ResourceBlockExtraData : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort BuildingBlockKey = 0;

		public const ushort Progress = 1;

		public const ushort Cooldown = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "BuildingBlockKey", "Progress", "Cooldown" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public BuildingBlockKey BuildingBlockKey;

	[SerializableGameDataField(FieldIndex = 1)]
	public int Progress;

	[SerializableGameDataField(FieldIndex = 2)]
	public int Cooldown;

	public ResourceBlockExtraData()
	{
		BuildingBlockKey = BuildingBlockKey.Invalid;
		Progress = 0;
		Cooldown = 0;
	}

	public ResourceBlockExtraData(BuildingBlockKey blockKey)
	{
		BuildingBlockKey = blockKey;
		Progress = 0;
		Cooldown = 0;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num += BuildingBlockKey.GetSerializedSize();
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
		ptr += BuildingBlockKey.Serialize(ptr);
		*(int*)ptr = Progress;
		ptr += 4;
		*(int*)ptr = Cooldown;
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
			ptr += BuildingBlockKey.Deserialize(ptr);
		}
		if (num > 1)
		{
			Progress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			Cooldown = *(int*)ptr;
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
