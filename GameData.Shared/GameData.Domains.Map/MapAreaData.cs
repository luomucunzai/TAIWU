using System;
using System.Collections.Generic;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

public class MapAreaData : ISerializableGameData
{
	public const int RegularAreasCount = 45;

	public const int BrokenAreasCount = 90;

	public const int WorldAreasCount = 135;

	public const int SpecialAreasCount = 4;

	public const int TotalAreasCount = 139;

	public const int BrokenAreasPerState = 6;

	public const short BornAreaId = 135;

	public const short GuideAreaId = 136;

	public const short SecretVillageAreaId = 137;

	public const short BrokenPerformAreaId = 138;

	public const int MaxSettlementsCount = 3;

	[SerializableGameDataField]
	private short _templateId;

	[SerializableGameDataField]
	private short _areaIndex;

	[SerializableGameDataField(ArrayElementsCount = 3)]
	public SettlementInfo[] SettlementInfos;

	[SerializableGameDataField]
	public short StationBlockId;

	[SerializableGameDataField]
	public bool Discovered;

	[SerializableGameDataField]
	public bool StationUnlocked;

	[Obsolete("Use DomainManager.Extra._spiritualDebt instead")]
	[SerializableGameDataField]
	public short SpiritualDebt;

	[SerializableGameDataField]
	public HashSet<short> NeighborAreas;

	public static bool IsRegularArea(short areaId)
	{
		if (areaId < 45)
		{
			return areaId >= 0;
		}
		return false;
	}

	public static bool IsNormalArea(short areaId)
	{
		if (areaId >= 45)
		{
			return areaId >= 135;
		}
		return true;
	}

	public static bool IsBrokenArea(short areaId)
	{
		if (areaId >= 45)
		{
			return areaId < 135;
		}
		return false;
	}

	public MapAreaData()
	{
		SettlementInfos = new SettlementInfo[3];
		NeighborAreas = new HashSet<short>();
	}

	public void Init(short templateId, short areaIndex)
	{
		_templateId = templateId;
		_areaIndex = areaIndex;
		StationBlockId = -1;
		SpiritualDebt = 0;
		for (int i = 0; i < 3; i++)
		{
			SettlementInfos[i] = new SettlementInfo(-1, -1, -1, -1);
		}
	}

	public MapAreaData(MapAreaData other)
	{
		_templateId = other._templateId;
		_areaIndex = other._areaIndex;
		SettlementInfo[] settlementInfos = other.SettlementInfos;
		int num = settlementInfos.Length;
		SettlementInfos = new SettlementInfo[num];
		for (int i = 0; i < num; i++)
		{
			SettlementInfos[i] = settlementInfos[i];
		}
		StationBlockId = other.StationBlockId;
		Discovered = other.Discovered;
		StationUnlocked = other.StationUnlocked;
		SpiritualDebt = other.SpiritualDebt;
		NeighborAreas = new HashSet<short>(other.NeighborAreas);
	}

	public void Assign(MapAreaData other)
	{
		_templateId = other._templateId;
		_areaIndex = other._areaIndex;
		SettlementInfo[] settlementInfos = other.SettlementInfos;
		int num = settlementInfos.Length;
		SettlementInfos = new SettlementInfo[num];
		for (int i = 0; i < num; i++)
		{
			SettlementInfos[i] = settlementInfos[i];
		}
		StationBlockId = other.StationBlockId;
		Discovered = other.Discovered;
		StationUnlocked = other.StationUnlocked;
		SpiritualDebt = other.SpiritualDebt;
		NeighborAreas = new HashSet<short>(other.NeighborAreas);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 34;
		num = ((NeighborAreas == null) ? (num + 2) : (num + (2 + 2 * NeighborAreas.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = _templateId;
		ptr += 2;
		*(short*)ptr = _areaIndex;
		ptr += 2;
		Tester.Assert(SettlementInfos.Length == 3);
		for (int i = 0; i < 3; i++)
		{
			ptr += SettlementInfos[i].Serialize(ptr);
		}
		*(short*)ptr = StationBlockId;
		ptr += 2;
		*ptr = (Discovered ? ((byte)1) : ((byte)0));
		ptr++;
		*ptr = (StationUnlocked ? ((byte)1) : ((byte)0));
		ptr++;
		*(short*)ptr = SpiritualDebt;
		ptr += 2;
		if (NeighborAreas != null)
		{
			int count = NeighborAreas.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			foreach (short neighborArea in NeighborAreas)
			{
				*(short*)ptr = neighborArea;
				ptr += 2;
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
		_templateId = *(short*)ptr;
		ptr += 2;
		_areaIndex = *(short*)ptr;
		ptr += 2;
		if (SettlementInfos == null || SettlementInfos.Length != 3)
		{
			SettlementInfos = new SettlementInfo[3];
		}
		for (int i = 0; i < 3; i++)
		{
			SettlementInfo settlementInfo = default(SettlementInfo);
			ptr += settlementInfo.Deserialize(ptr);
			SettlementInfos[i] = settlementInfo;
		}
		StationBlockId = *(short*)ptr;
		ptr += 2;
		Discovered = *ptr != 0;
		ptr++;
		StationUnlocked = *ptr != 0;
		ptr++;
		SpiritualDebt = *(short*)ptr;
		ptr += 2;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (NeighborAreas == null)
			{
				NeighborAreas = new HashSet<short>();
			}
			else
			{
				NeighborAreas.Clear();
			}
			for (int j = 0; j < num; j++)
			{
				NeighborAreas.Add(((short*)ptr)[j]);
			}
			ptr += 2 * num;
		}
		else
		{
			NeighborAreas?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public short GetTemplateId()
	{
		return _templateId;
	}

	public short GetId()
	{
		return _areaIndex;
	}

	public MapAreaItem GetConfig()
	{
		return MapArea.Instance[_templateId];
	}

	public int GetSettlementIndex(short blockId)
	{
		int i = 0;
		for (int num = SettlementInfos.Length; i < num; i++)
		{
			if (SettlementInfos[i].BlockId == blockId)
			{
				return i;
			}
		}
		return -1;
	}

	public short GetAreaId()
	{
		return _areaIndex;
	}
}
