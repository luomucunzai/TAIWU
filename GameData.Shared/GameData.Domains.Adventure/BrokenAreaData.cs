using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Adventure;

public class BrokenAreaData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Level;

	[SerializableGameDataField]
	public List<MapTemplateEnemyInfo> RandomEnemies;

	public short BaseXiangshuMinionTemplateId => (short)Math.Clamp(298 + Level - 1, 298, 306);

	public BrokenAreaData()
	{
		RandomEnemies = new List<MapTemplateEnemyInfo>();
	}

	public BrokenAreaData(BrokenAreaData other)
	{
		Level = other.Level;
		RandomEnemies = new List<MapTemplateEnemyInfo>(other.RandomEnemies);
	}

	public void Assign(BrokenAreaData other)
	{
		Level = other.Level;
		RandomEnemies = new List<MapTemplateEnemyInfo>(other.RandomEnemies);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 1;
		num = ((RandomEnemies == null) ? (num + 2) : (num + (2 + 8 * RandomEnemies.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)Level;
		ptr++;
		if (RandomEnemies != null)
		{
			int count = RandomEnemies.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += RandomEnemies[i].Serialize(ptr);
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
		Level = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RandomEnemies == null)
			{
				RandomEnemies = new List<MapTemplateEnemyInfo>(num);
			}
			else
			{
				RandomEnemies.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				MapTemplateEnemyInfo item = default(MapTemplateEnemyInfo);
				ptr += item.Deserialize(ptr);
				RandomEnemies.Add(item);
			}
		}
		else
		{
			RandomEnemies?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
