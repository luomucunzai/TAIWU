using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Debate;

public class StrategyTarget : ISerializableGameData
{
	[SerializableGameDataField]
	public int ObjectType;

	[SerializableGameDataField]
	public List<ulong> List;

	public EDebateStrategyTargetObjectType Type => (EDebateStrategyTargetObjectType)ObjectType;

	public StrategyTarget(EDebateStrategyTargetObjectType type, List<ulong> list)
	{
		ObjectType = (int)type;
		List = list;
	}

	public StrategyTarget()
	{
	}

	public StrategyTarget(StrategyTarget other)
	{
		ObjectType = other.ObjectType;
		List = ((other.List == null) ? null : new List<ulong>(other.List));
	}

	public void Assign(StrategyTarget other)
	{
		ObjectType = other.ObjectType;
		List = ((other.List == null) ? null : new List<ulong>(other.List));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((List == null) ? (num + 2) : (num + (2 + 8 * List.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = ObjectType;
		ptr += 4;
		if (List != null)
		{
			int count = List.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((long*)ptr)[i] = (long)List[i];
			}
			ptr += 8 * count;
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
		ObjectType = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (List == null)
			{
				List = new List<ulong>(num);
			}
			else
			{
				List.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				List.Add(((ulong*)ptr)[i]);
			}
			ptr += 8 * num;
		}
		else
		{
			List?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
