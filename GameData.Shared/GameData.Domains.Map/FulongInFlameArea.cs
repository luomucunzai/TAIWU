using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

[SerializableGameData(IsExtensible = true)]
public class FulongInFlameArea : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort LightedBlocks = 0;

		public const ushort ExtinguishedBlocks = 1;

		public const ushort MineBlocks = 2;

		public const ushort TriggeredMineBlocks = 3;

		public const ushort RewardGrade = 4;

		public const ushort EdgeBlocks = 5;

		public const ushort AreaId = 6;

		public const ushort MineCount = 7;

		public const ushort Count = 8;

		public static readonly string[] FieldId2FieldName = new string[8] { "LightedBlocks", "ExtinguishedBlocks", "MineBlocks", "TriggeredMineBlocks", "RewardGrade", "EdgeBlocks", "AreaId", "MineCount" };
	}

	[SerializableGameDataField]
	public Dictionary<short, int> LightedBlocks;

	[SerializableGameDataField]
	public List<short> ExtinguishedBlocks;

	[SerializableGameDataField]
	public List<short> MineBlocks;

	[SerializableGameDataField]
	public List<short> TriggeredMineBlocks;

	[SerializableGameDataField]
	public sbyte RewardGrade;

	[SerializableGameDataField]
	public Dictionary<short, sbyte> EdgeBlocks;

	[SerializableGameDataField]
	public short AreaId;

	[SerializableGameDataField]
	public int MineCount;

	public FulongInFlameArea()
	{
		LightedBlocks = new Dictionary<short, int>();
		MineBlocks = new List<short>();
		EdgeBlocks = new Dictionary<short, sbyte>();
		AreaId = -1;
		ExtinguishedBlocks = new List<short>();
		TriggeredMineBlocks = new List<short>();
		RewardGrade = -1;
		MineCount = -1;
	}

	public FulongInFlameArea(Dictionary<short, int> lightedBlocks, List<short> mineBlocks, Dictionary<short, sbyte> edgeBlocks, bool isBig, short areaId, int mineCount)
	{
		LightedBlocks = lightedBlocks;
		MineBlocks = mineBlocks;
		EdgeBlocks = edgeBlocks;
		AreaId = areaId;
		ExtinguishedBlocks = new List<short>();
		TriggeredMineBlocks = new List<short>();
		RewardGrade = (sbyte)(isBig ? 6 : 3);
		MineCount = mineCount;
	}

	public static bool IsAdjacent(MapBlockData a, MapBlockData b)
	{
		return a.GetBlockPos().GetManhattanDistance(b.GetBlockPos()) == 1;
	}

	public bool IsFullyExtinguished()
	{
		return ExtinguishedBlocks.Count + MineBlocks.Count >= LightedBlocks.Count;
	}

	public bool IsLocationInFlame(Location location)
	{
		if (location.AreaId == AreaId)
		{
			return LightedBlocks.ContainsKey(location.BlockId);
		}
		return false;
	}

	public bool IsLocationInActiveFlame(Location location)
	{
		if (IsLocationInFlame(location) && !ExtinguishedBlocks.Contains(location.BlockId))
		{
			return !TriggeredMineBlocks.Contains(location.BlockId);
		}
		return false;
	}

	public FulongInFlameArea(FulongInFlameArea other)
	{
		LightedBlocks = ((other.LightedBlocks == null) ? null : new Dictionary<short, int>(other.LightedBlocks));
		ExtinguishedBlocks = ((other.ExtinguishedBlocks == null) ? null : new List<short>(other.ExtinguishedBlocks));
		MineBlocks = ((other.MineBlocks == null) ? null : new List<short>(other.MineBlocks));
		TriggeredMineBlocks = ((other.TriggeredMineBlocks == null) ? null : new List<short>(other.TriggeredMineBlocks));
		RewardGrade = other.RewardGrade;
		EdgeBlocks = ((other.EdgeBlocks == null) ? null : new Dictionary<short, sbyte>(other.EdgeBlocks));
		AreaId = other.AreaId;
		MineCount = other.MineCount;
	}

	public void Assign(FulongInFlameArea other)
	{
		LightedBlocks = ((other.LightedBlocks == null) ? null : new Dictionary<short, int>(other.LightedBlocks));
		ExtinguishedBlocks = ((other.ExtinguishedBlocks == null) ? null : new List<short>(other.ExtinguishedBlocks));
		MineBlocks = ((other.MineBlocks == null) ? null : new List<short>(other.MineBlocks));
		TriggeredMineBlocks = ((other.TriggeredMineBlocks == null) ? null : new List<short>(other.TriggeredMineBlocks));
		RewardGrade = other.RewardGrade;
		EdgeBlocks = ((other.EdgeBlocks == null) ? null : new Dictionary<short, sbyte>(other.EdgeBlocks));
		AreaId = other.AreaId;
		MineCount = other.MineCount;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)LightedBlocks);
		num = ((ExtinguishedBlocks == null) ? (num + 2) : (num + (2 + 2 * ExtinguishedBlocks.Count)));
		num = ((MineBlocks == null) ? (num + 2) : (num + (2 + 2 * MineBlocks.Count)));
		num = ((TriggeredMineBlocks == null) ? (num + 2) : (num + (2 + 2 * TriggeredMineBlocks.Count)));
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, sbyte>((IReadOnlyDictionary<short, sbyte>)EdgeBlocks);
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
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref LightedBlocks);
		if (ExtinguishedBlocks != null)
		{
			int count = ExtinguishedBlocks.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((short*)ptr)[i] = ExtinguishedBlocks[i];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MineBlocks != null)
		{
			int count2 = MineBlocks.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((short*)ptr)[j] = MineBlocks[j];
			}
			ptr += 2 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (TriggeredMineBlocks != null)
		{
			int count3 = TriggeredMineBlocks.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = TriggeredMineBlocks[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*ptr = (byte)RewardGrade;
		ptr++;
		ptr += DictionaryOfBasicTypePair.Serialize<short, sbyte>(ptr, ref EdgeBlocks);
		*(short*)ptr = AreaId;
		ptr += 2;
		*(int*)ptr = MineCount;
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
			ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref LightedBlocks);
		}
		if (num > 1)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (ExtinguishedBlocks == null)
				{
					ExtinguishedBlocks = new List<short>(num2);
				}
				else
				{
					ExtinguishedBlocks.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					ExtinguishedBlocks.Add(((short*)ptr)[i]);
				}
				ptr += 2 * num2;
			}
			else
			{
				ExtinguishedBlocks?.Clear();
			}
		}
		if (num > 2)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (MineBlocks == null)
				{
					MineBlocks = new List<short>(num3);
				}
				else
				{
					MineBlocks.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					MineBlocks.Add(((short*)ptr)[j]);
				}
				ptr += 2 * num3;
			}
			else
			{
				MineBlocks?.Clear();
			}
		}
		if (num > 3)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (TriggeredMineBlocks == null)
				{
					TriggeredMineBlocks = new List<short>(num4);
				}
				else
				{
					TriggeredMineBlocks.Clear();
				}
				for (int k = 0; k < num4; k++)
				{
					TriggeredMineBlocks.Add(((short*)ptr)[k]);
				}
				ptr += 2 * num4;
			}
			else
			{
				TriggeredMineBlocks?.Clear();
			}
		}
		if (num > 4)
		{
			RewardGrade = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 5)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, sbyte>(ptr, ref EdgeBlocks);
		}
		if (num > 6)
		{
			AreaId = *(short*)ptr;
			ptr += 2;
		}
		if (num > 7)
		{
			MineCount = *(int*)ptr;
			ptr += 4;
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
