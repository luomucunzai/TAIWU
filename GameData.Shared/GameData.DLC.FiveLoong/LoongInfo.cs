using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC.FiveLoong;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class LoongInfo : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort CharacterTemplateId = 0;

		public const ushort IsDisappear = 1;

		public const ushort TaiwuDebuffCount = 2;

		public const ushort LoongTerrainCenterLocation = 3;

		public const ushort LoongCurrentLocation = 4;

		public const ushort CoveredMapBlockTemplateId = 5;

		public const ushort DisappearDate = 6;

		public const ushort MinionLoongBlockList = 7;

		public const ushort CharacterDebuffCounts = 8;

		public const ushort MapBlockExtraItems = 9;

		public const ushort Count = 10;

		public static readonly string[] FieldId2FieldName = new string[10] { "CharacterTemplateId", "IsDisappear", "TaiwuDebuffCount", "LoongTerrainCenterLocation", "LoongCurrentLocation", "CoveredMapBlockTemplateId", "DisappearDate", "MinionLoongBlockList", "CharacterDebuffCounts", "MapBlockExtraItems" };
	}

	public const short LoongTerrainRange = 3;

	public const short LoongDisappearTime = 108;

	[SerializableGameDataField]
	public short CharacterTemplateId;

	[SerializableGameDataField]
	public bool IsDisappear;

	[SerializableGameDataField]
	public int DisappearDate;

	[Obsolete]
	[SerializableGameDataField]
	public ushort TaiwuDebuffCount;

	[SerializableGameDataField]
	public Location LoongTerrainCenterLocation;

	[SerializableGameDataField]
	public Location LoongCurrentLocation;

	[SerializableGameDataField]
	public Dictionary<short, short> CoveredMapBlockTemplateId;

	[SerializableGameDataField]
	public Dictionary<Location, Inventory> MapBlockExtraItems = new Dictionary<Location, Inventory>();

	[Obsolete]
	[SerializableGameDataField]
	public List<short> MinionLoongBlockList;

	[SerializableGameDataField]
	public Dictionary<int, ushort> CharacterDebuffCounts;

	public short LoongTemplateId => (short)(CharacterTemplateId - 686);

	public LoongItem ConfigData => Loong.Instance[LoongTemplateId];

	public void ChangeCharacterDebuffCount(int charId, int delta)
	{
		if (CharacterDebuffCounts == null)
		{
			CharacterDebuffCounts = new Dictionary<int, ushort>();
		}
		if (CharacterDebuffCounts.TryGetValue(charId, out var value))
		{
			value = (ushort)MathUtils.Clamp(value + delta, 0, GlobalConfig.Instance.FiveLoongDlcMaxDebuffCount);
			if (value == 0)
			{
				CharacterDebuffCounts.Remove(charId);
			}
			else
			{
				CharacterDebuffCounts[charId] = value;
			}
		}
		else if (delta > 0)
		{
			CharacterDebuffCounts.Add(charId, (ushort)delta);
		}
	}

	public ushort GetCharacterDebuffCount(int charId)
	{
		if (CharacterDebuffCounts == null)
		{
			return 0;
		}
		if (!CharacterDebuffCounts.TryGetValue(charId, out var value))
		{
			return 0;
		}
		return value;
	}

	public static short CharacterTemplateIdToLoongTemplateId(short charTemplateId)
	{
		return (short)(charTemplateId - 686);
	}

	public LoongInfo(short charTemplateId, Location initialLocation, Dictionary<short, short> coveredMapBlockTemplateId)
	{
		CharacterTemplateId = charTemplateId;
		LoongCurrentLocation = initialLocation;
		LoongTerrainCenterLocation = initialLocation;
		CoveredMapBlockTemplateId = coveredMapBlockTemplateId;
	}

	public LoongInfo()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 19;
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, short>((IReadOnlyDictionary<short, short>)CoveredMapBlockTemplateId);
		num = ((MinionLoongBlockList == null) ? (num + 2) : (num + (2 + 2 * MinionLoongBlockList.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<int, ushort>((IReadOnlyDictionary<int, ushort>)CharacterDebuffCounts);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<Location, Inventory>(MapBlockExtraItems);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 10;
		ptr += 2;
		*(short*)ptr = CharacterTemplateId;
		ptr += 2;
		*ptr = (IsDisappear ? ((byte)1) : ((byte)0));
		ptr++;
		*(ushort*)ptr = TaiwuDebuffCount;
		ptr += 2;
		ptr += LoongTerrainCenterLocation.Serialize(ptr);
		ptr += LoongCurrentLocation.Serialize(ptr);
		ptr += DictionaryOfBasicTypePair.Serialize<short, short>(ptr, ref CoveredMapBlockTemplateId);
		*(int*)ptr = DisappearDate;
		ptr += 4;
		if (MinionLoongBlockList != null)
		{
			int count = MinionLoongBlockList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = MinionLoongBlockList[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypePair.Serialize<int, ushort>(ptr, ref CharacterDebuffCounts);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<Location, Inventory>(ptr, ref MapBlockExtraItems);
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
			CharacterTemplateId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 1)
		{
			IsDisappear = *ptr != 0;
			ptr++;
		}
		if (num > 2)
		{
			TaiwuDebuffCount = *(ushort*)ptr;
			ptr += 2;
		}
		if (num > 3)
		{
			ptr += LoongTerrainCenterLocation.Deserialize(ptr);
		}
		if (num > 4)
		{
			ptr += LoongCurrentLocation.Deserialize(ptr);
		}
		if (num > 5)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, short>(ptr, ref CoveredMapBlockTemplateId);
		}
		if (num > 6)
		{
			DisappearDate = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (MinionLoongBlockList == null)
				{
					MinionLoongBlockList = new List<short>(num2);
				}
				else
				{
					MinionLoongBlockList.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					MinionLoongBlockList.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				MinionLoongBlockList?.Clear();
			}
		}
		if (num > 8)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<int, ushort>(ptr, ref CharacterDebuffCounts);
		}
		if (num > 9)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<Location, Inventory>(ptr, ref MapBlockExtraItems);
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
