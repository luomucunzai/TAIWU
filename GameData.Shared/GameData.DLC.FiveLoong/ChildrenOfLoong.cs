using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true)]
public class ChildrenOfLoong : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Key = 0;

		public const ushort NameId = 1;

		public const ushort Behavior = 2;

		public const ushort Properties = 3;

		public const ushort Id = 4;

		public const ushort JiaoTemplateId = 5;

		public const ushort LoongTemplateId = 6;

		public const ushort Gender = 7;

		public const ushort Count = 8;

		public static readonly string[] FieldId2FieldName = new string[8] { "Key", "NameId", "Behavior", "Properties", "Id", "JiaoTemplateId", "LoongTemplateId", "Gender" };
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField]
	public ItemKey Key;

	[SerializableGameDataField]
	public int NameId;

	[SerializableGameDataField]
	public sbyte Behavior;

	[SerializableGameDataField]
	public JiaoProperty Properties;

	[SerializableGameDataField]
	public short JiaoTemplateId;

	[SerializableGameDataField]
	public short LoongTemplateId;

	[SerializableGameDataField]
	public bool Gender;

	public ChildrenOfLoong()
	{
		Id = -1;
		Key = ItemKey.Invalid;
		NameId = -1;
		Behavior = -1;
		Properties = new JiaoProperty();
		JiaoTemplateId = 0;
		LoongTemplateId = 31;
		Gender = false;
	}

	public ChildrenOfLoong(int id, ItemKey key, sbyte behavior, short loongTemplateId, JiaoProperty properties)
	{
		Id = id;
		Key = key;
		NameId = -1;
		Behavior = behavior;
		Properties = properties;
		JiaoTemplateId = 0;
		LoongTemplateId = loongTemplateId;
		Gender = false;
	}

	public ChildrenOfLoong(Jiao jiao, ItemKey key, short loongTemplateId)
	{
		Id = jiao.Id;
		Key = key;
		NameId = jiao.NameId;
		Behavior = jiao.Behavior;
		Properties = new JiaoProperty();
		Properties.DeepCopy(jiao.Properties);
		JiaoTemplateId = jiao.TemplateId;
		LoongTemplateId = loongTemplateId;
		Gender = jiao.Gender;
	}

	public ChildrenOfLoong(ChildrenOfLoong childOfLoong, ItemKey key)
	{
		Id = key.Id;
		Key = key;
		NameId = childOfLoong.NameId;
		Behavior = childOfLoong.Behavior;
		Properties = new JiaoProperty();
		Properties.DeepCopy(childOfLoong.Properties);
		JiaoTemplateId = childOfLoong.JiaoTemplateId;
		LoongTemplateId = childOfLoong.LoongTemplateId;
		Gender = childOfLoong.Gender;
	}

	public string GetNameText()
	{
		return GetNameRelatedData().GetName();
	}

	public JiaoLoongNameRelatedData GetNameRelatedData()
	{
		return new JiaoLoongNameRelatedData
		{
			ItemType = Key.ItemType,
			ItemTemplateId = Key.TemplateId,
			NameId = NameId,
			CharTemplateId = -1
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 96;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 8;
		ptr += 2;
		ptr += Key.Serialize(ptr);
		*(int*)ptr = NameId;
		ptr += 4;
		*ptr = (byte)Behavior;
		ptr++;
		ptr += Properties.Serialize(ptr);
		*(int*)ptr = Id;
		ptr += 4;
		*(short*)ptr = JiaoTemplateId;
		ptr += 2;
		*(short*)ptr = LoongTemplateId;
		ptr += 2;
		*ptr = (Gender ? ((byte)1) : ((byte)0));
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
			ptr += Key.Deserialize(ptr);
		}
		if (num > 1)
		{
			NameId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			Behavior = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 3)
		{
			if (Properties == null)
			{
				Properties = new JiaoProperty();
			}
			ptr += Properties.Deserialize(ptr);
		}
		if (num > 4)
		{
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			JiaoTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 6)
		{
			LoongTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 7)
		{
			Gender = *ptr != 0;
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
