using System;
using System.Collections.Generic;
using System.Linq;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Merchant;

[Serializable]
public class CaravanPath : ISerializableGameData
{
	public const int CostDaysPerMove = 15;

	[SerializableGameDataField]
	public List<Location> FullPath = new List<Location>();

	[SerializableGameDataField]
	public List<int> MoveNodes = new List<int>();

	[SerializableGameDataField]
	public short MoveWaitDays;

	public CaravanPath(List<(Location location, short cost)> path)
	{
	}

	public Location GetCurrLocation()
	{
		int i;
		for (i = 0; MoveNodes[i] < 0; i++)
		{
		}
		return FullPath[MoveNodes[i]];
	}

	public Location GetNextLocation()
	{
		Location currLocation = GetCurrLocation();
		int index = FullPath.IndexOf(currLocation) + 1;
		if (!FullPath.CheckIndex(index))
		{
			return currLocation;
		}
		return FullPath[index];
	}

	public Location GetLastLocation()
	{
		Location currLocation = GetCurrLocation();
		int index = FullPath.IndexOf(currLocation) - 1;
		if (!FullPath.CheckIndex(index))
		{
			return currLocation;
		}
		return FullPath[index];
	}

	public Location GetDestLocation()
	{
		return FullPath.Last();
	}

	public Location GetSrcLocation()
	{
		return FullPath.First();
	}

	public CaravanPath GetRemainCaravanPathInCurrentArea()
	{
		CaravanPath caravanPath = new CaravanPath();
		caravanPath.MoveWaitDays = MoveWaitDays;
		Location currLocation = GetCurrLocation();
		Location nextLocation = GetNextLocation();
		Location lastLocation = GetLastLocation();
		int num = -1;
		for (int i = 0; i < FullPath.Count; i++)
		{
			if (currLocation == FullPath[i])
			{
				int index = i + 1;
				bool num2 = !FullPath.CheckIndex(index) || FullPath[index] == nextLocation;
				int index2 = i - 1;
				bool flag = !FullPath.CheckIndex(index2) || FullPath[index2] == lastLocation;
				if (num2 && flag)
				{
					num = i;
					break;
				}
			}
		}
		for (int j = num; j < FullPath.Count; j++)
		{
			Location item = FullPath[j];
			if (item.AreaId == currLocation.AreaId)
			{
				caravanPath.FullPath.Add(item);
			}
		}
		List<Location> list = (from index3 in MoveNodes
			where FullPath.CheckIndex(index3)
			select FullPath[index3]).ToList();
		for (int num3 = 0; num3 < caravanPath.FullPath.Count; num3++)
		{
			Location item2 = caravanPath.FullPath[num3];
			if (list.Contains(item2))
			{
				caravanPath.MoveNodes.Add(num3);
			}
		}
		return caravanPath;
	}

	public CaravanPath()
	{
	}

	public CaravanPath(CaravanPath other)
	{
		FullPath = new List<Location>(other.FullPath);
		MoveNodes = new List<int>(other.MoveNodes);
		MoveWaitDays = other.MoveWaitDays;
	}

	public void Assign(CaravanPath other)
	{
		FullPath = new List<Location>(other.FullPath);
		MoveNodes = new List<int>(other.MoveNodes);
		MoveWaitDays = other.MoveWaitDays;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((FullPath == null) ? (num + 2) : (num + (2 + 4 * FullPath.Count)));
		num = ((MoveNodes == null) ? (num + 2) : (num + (2 + 4 * MoveNodes.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (FullPath != null)
		{
			int count = FullPath.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += FullPath[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (MoveNodes != null)
		{
			int count2 = MoveNodes.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				((int*)ptr)[j] = MoveNodes[j];
			}
			ptr += 4 * count2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		*(short*)ptr = MoveWaitDays;
		ptr += 2;
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
			if (FullPath == null)
			{
				FullPath = new List<Location>(num);
			}
			else
			{
				FullPath.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Location item = default(Location);
				ptr += item.Deserialize(ptr);
				FullPath.Add(item);
			}
		}
		else
		{
			FullPath?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (MoveNodes == null)
			{
				MoveNodes = new List<int>(num2);
			}
			else
			{
				MoveNodes.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				MoveNodes.Add(((int*)ptr)[j]);
			}
			ptr += 4 * num2;
		}
		else
		{
			MoveNodes?.Clear();
		}
		MoveWaitDays = *(short*)ptr;
		ptr += 2;
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
