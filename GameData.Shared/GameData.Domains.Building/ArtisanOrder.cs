using System.Collections.Generic;
using GameData.Serializer;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Building;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class ArtisanOrder : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort BuildingBlockKey = 0;

		public const ushort ArtisanId = 1;

		public const ushort SubscriberId = 2;

		public const ushort ItemSubType = 3;

		public const ushort LifeSkillType = 4;

		public const ushort Progress = 5;

		public const ushort StorageType = 6;

		public const ushort ProductionWeight = 7;

		public const ushort IsDebateWon = 8;

		public const ushort ProgressDelta = 9;

		public const ushort DebateCount = 10;

		public const ushort ProgressBaseDelta = 11;

		public const ushort Count = 12;

		public static readonly string[] FieldId2FieldName = new string[12]
		{
			"BuildingBlockKey", "ArtisanId", "SubscriberId", "ItemSubType", "LifeSkillType", "Progress", "StorageType", "ProductionWeight", "IsDebateWon", "ProgressDelta",
			"DebateCount", "ProgressBaseDelta"
		};
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public BuildingBlockKey BuildingBlockKey;

	[SerializableGameDataField(FieldIndex = 1)]
	public int ArtisanId;

	[SerializableGameDataField(FieldIndex = 2)]
	public int SubscriberId;

	[SerializableGameDataField(FieldIndex = 3)]
	public short ItemSubType;

	[SerializableGameDataField(FieldIndex = 4)]
	public sbyte LifeSkillType;

	[SerializableGameDataField(FieldIndex = 5)]
	public int Progress;

	[SerializableGameDataField(FieldIndex = 6)]
	public int StorageType;

	[SerializableGameDataField(FieldIndex = 7)]
	public Dictionary<Production, int> ProductionWeight;

	[SerializableGameDataField(FieldIndex = 8)]
	public bool IsDebateWon;

	[SerializableGameDataField(FieldIndex = 9)]
	public int ProgressDelta;

	[SerializableGameDataField(FieldIndex = 10)]
	public int DebateCount;

	[SerializableGameDataField(FieldIndex = 11)]
	public int ProgressBaseDelta;

	public ArtisanOrder()
	{
		BuildingBlockKey = BuildingBlockKey.Invalid;
		ArtisanId = -1;
		SubscriberId = -1;
		ItemSubType = -1;
		Progress = 0;
		StorageType = 2;
		ProductionWeight = new Dictionary<Production, int>();
		IsDebateWon = false;
		ProgressDelta = 0;
		ProgressBaseDelta = 0;
		DebateCount = 0;
	}

	public ArtisanOrder(int artisanId, int subscriberId, sbyte lifeSkillType, int progressDelta)
	{
		BuildingBlockKey = BuildingBlockKey.Invalid;
		ArtisanId = artisanId;
		SubscriberId = subscriberId;
		ItemSubType = -1;
		LifeSkillType = lifeSkillType;
		Progress = 0;
		StorageType = 2;
		ProductionWeight = new Dictionary<Production, int>();
		IsDebateWon = false;
		ProgressDelta = progressDelta;
		DebateCount = 0;
	}

	public ArtisanOrder(BuildingBlockKey buildingBlockKey, int artisanId, int subscriberId, sbyte lifeSkillType, int progressDelta, int progressBaseDelta, short itemSubType)
	{
		BuildingBlockKey = buildingBlockKey;
		ArtisanId = artisanId;
		SubscriberId = subscriberId;
		ItemSubType = itemSubType;
		LifeSkillType = lifeSkillType;
		Progress = 0;
		StorageType = 2;
		ProductionWeight = new Dictionary<Production, int>();
		IsDebateWon = false;
		ProgressDelta = progressDelta;
		ProgressBaseDelta = progressBaseDelta;
		DebateCount = 0;
	}

	public bool IsArtisanOrder()
	{
		return BuildingBlockKey.Equals(BuildingBlockKey.Invalid);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 34;
		num += BuildingBlockKey.GetSerializedSize();
		num += 4;
		if (ProductionWeight != null)
		{
			foreach (KeyValuePair<Production, int> item in ProductionWeight)
			{
				num += item.Key.GetSerializedSize();
				num += 4;
			}
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 12;
		ptr += 2;
		ptr += BuildingBlockKey.Serialize(ptr);
		*(int*)ptr = ArtisanId;
		ptr += 4;
		*(int*)ptr = SubscriberId;
		ptr += 4;
		*(short*)ptr = ItemSubType;
		ptr += 2;
		*ptr = (byte)LifeSkillType;
		ptr++;
		*(int*)ptr = Progress;
		ptr += 4;
		*(int*)ptr = StorageType;
		ptr += 4;
		if (ProductionWeight != null)
		{
			*(int*)ptr = ProductionWeight.Count;
			ptr += 4;
			foreach (KeyValuePair<Production, int> item in ProductionWeight)
			{
				ptr += item.Key.Serialize(ptr);
				*(int*)ptr = item.Value;
				ptr += 4;
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		*ptr = (IsDebateWon ? ((byte)1) : ((byte)0));
		ptr++;
		*(int*)ptr = ProgressDelta;
		ptr += 4;
		*(int*)ptr = DebateCount;
		ptr += 4;
		*(int*)ptr = ProgressBaseDelta;
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
			ArtisanId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			SubscriberId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			ItemSubType = *(short*)ptr;
			ptr += 2;
		}
		if (num > 4)
		{
			LifeSkillType = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			Progress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			StorageType = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				if (ProductionWeight == null)
				{
					ProductionWeight = new Dictionary<Production, int>();
				}
				else
				{
					ProductionWeight.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					Production key = default(Production);
					ptr += key.Deserialize(ptr);
					int value = *(int*)ptr;
					ptr += 4;
					ProductionWeight.Add(key, value);
				}
			}
			else
			{
				ProductionWeight?.Clear();
			}
		}
		if (num > 8)
		{
			IsDebateWon = *ptr != 0;
			ptr++;
		}
		if (num > 9)
		{
			ProgressDelta = *(int*)ptr;
			ptr += 4;
		}
		if (num > 10)
		{
			DebateCount = *(int*)ptr;
			ptr += 4;
		}
		if (num > 11)
		{
			ProgressBaseDelta = *(int*)ptr;
			ptr += 4;
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
