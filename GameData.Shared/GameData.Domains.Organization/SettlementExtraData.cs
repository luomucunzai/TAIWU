using System;
using System.Collections.Generic;
using GameData.Domains.Building;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Organization;

[SerializableGameData(IsExtensible = true)]
[Obsolete]
public class SettlementExtraData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort SupportedBlocks = 0;

		public const ushort DeactivateSupportedBlocks = 1;

		public const ushort DisabledSupportedBlocks = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "SupportedBlocks", "DeactivateSupportedBlocks", "DisabledSupportedBlocks" };
	}

	[SerializableGameDataField]
	public Dictionary<BuildingBlockKey, int> SupportedBlocks;

	[SerializableGameDataField]
	[Obsolete]
	public List<BuildingBlockKey> DeactivateSupportedBlocks;

	[SerializableGameDataField]
	public List<BuildingBlockKey> DisabledSupportedBlocks;

	public SettlementExtraData()
	{
		SupportedBlocks = new Dictionary<BuildingBlockKey, int>();
		DeactivateSupportedBlocks = new List<BuildingBlockKey>();
		DisabledSupportedBlocks = new List<BuildingBlockKey>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<BuildingBlockKey, int>(SupportedBlocks);
		num = ((DeactivateSupportedBlocks == null) ? (num + 2) : (num + (2 + 8 * DeactivateSupportedBlocks.Count)));
		num = ((DisabledSupportedBlocks == null) ? (num + 2) : (num + (2 + 8 * DisabledSupportedBlocks.Count)));
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
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<BuildingBlockKey, int>(ptr, ref SupportedBlocks);
		if (DeactivateSupportedBlocks != null)
		{
			int count = DeactivateSupportedBlocks.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += DeactivateSupportedBlocks[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (DisabledSupportedBlocks != null)
		{
			int count2 = DisabledSupportedBlocks.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += DisabledSupportedBlocks[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
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
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<BuildingBlockKey, int>(ptr, ref SupportedBlocks);
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (DeactivateSupportedBlocks == null)
				{
					DeactivateSupportedBlocks = new List<BuildingBlockKey>(num2);
				}
				else
				{
					DeactivateSupportedBlocks.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					BuildingBlockKey item = default(BuildingBlockKey);
					ptr += item.Deserialize(ptr);
					DeactivateSupportedBlocks.Add(item);
				}
			}
			else
			{
				DeactivateSupportedBlocks?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (DisabledSupportedBlocks == null)
				{
					DisabledSupportedBlocks = new List<BuildingBlockKey>(num3);
				}
				else
				{
					DisabledSupportedBlocks.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					BuildingBlockKey item2 = default(BuildingBlockKey);
					ptr += item2.Deserialize(ptr);
					DisabledSupportedBlocks.Add(item2);
				}
			}
			else
			{
				DisabledSupportedBlocks?.Clear();
			}
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
