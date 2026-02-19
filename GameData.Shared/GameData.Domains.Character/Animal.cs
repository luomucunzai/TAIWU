using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true)]
public class Animal : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort ItemKey = 1;

		public const ushort CharacterTemplateId = 2;

		public const ushort Location = 3;

		public const ushort Type = 4;

		public const ushort NoAccident = 5;

		public const ushort Count = 6;

		public static readonly string[] FieldId2FieldName = new string[6] { "Id", "ItemKey", "CharacterTemplateId", "Location", "Type", "NoAccident" };
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public ItemKey ItemKey;

	[SerializableGameDataField]
	public short CharacterTemplateId;

	[SerializableGameDataField]
	public Location Location;

	[SerializableGameDataField]
	public sbyte Type;

	[SerializableGameDataField]
	public bool NoAccident;

	public Animal()
	{
		Id = -1;
		ItemKey = ItemKey.Invalid;
		CharacterTemplateId = -1;
		Location = Location.Invalid;
		Type = 0;
		NoAccident = false;
	}

	public Animal(int id, short templateId)
	{
		Id = id;
		ItemKey = ItemKey.Invalid;
		CharacterTemplateId = templateId;
		Location = Location.Invalid;
		Type = 0;
		NoAccident = false;
	}

	public Animal(int id, ItemKey itemKey, short templateId, sbyte type)
	{
		Id = id;
		ItemKey = itemKey;
		CharacterTemplateId = templateId;
		Location = Location.Invalid;
		Type = type;
		NoAccident = true;
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
		*(short*)ptr = 6;
		ptr += 2;
		*(int*)ptr = Id;
		ptr += 4;
		ptr += ItemKey.Serialize(ptr);
		*(short*)ptr = CharacterTemplateId;
		ptr += 2;
		ptr += Location.Serialize(ptr);
		*ptr = (byte)Type;
		ptr++;
		*ptr = (NoAccident ? ((byte)1) : ((byte)0));
		ptr++;
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
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			ptr += ItemKey.Deserialize(ptr);
		}
		if (num > 2)
		{
			CharacterTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			ptr += Location.Deserialize(ptr);
		}
		if (num > 4)
		{
			Type = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			NoAccident = *ptr != 0;
			ptr++;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
