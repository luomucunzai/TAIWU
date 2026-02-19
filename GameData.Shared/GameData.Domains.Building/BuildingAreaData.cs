using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Building;

public class BuildingAreaData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte Width;

	[SerializableGameDataField]
	public sbyte LandFormType;

	public BuildingAreaData(sbyte width, sbyte landFormType)
	{
		Width = width;
		LandFormType = landFormType;
	}

	public BuildingAreaData()
	{
	}

	public BuildingAreaData(BuildingAreaData other)
	{
		Width = other.Width;
		LandFormType = other.LandFormType;
	}

	public void Assign(BuildingAreaData other)
	{
		Width = other.Width;
		LandFormType = other.LandFormType;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (byte)Width;
		byte* num = pData + 1;
		*num = (byte)LandFormType;
		int num2 = (int)(num + 1 - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		Width = (sbyte)(*ptr);
		ptr++;
		LandFormType = (sbyte)(*ptr);
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public short GetCenterBlockIndex()
	{
		int num = Width / 2;
		if (Width % 2 == 0)
		{
			num--;
		}
		return (short)(num * Width + num);
	}

	public (int x, int y) GetBlockPos(short index)
	{
		return (x: index % Width, y: index / Width);
	}

	public void GetNeighborBlocks(short blockIndex, sbyte blockWidth, List<short> neighborList, List<int> neighborDistanceList = null, int range = 1)
	{
		int num = blockIndex % Width;
		int num2 = blockIndex / Width;
		neighborList.Clear();
		neighborDistanceList?.Clear();
		for (int i = Math.Max(num - range, 0); i < Math.Min(num + blockWidth + range, Width); i++)
		{
			for (int j = Math.Max(num2 - range, 0); j < Math.Min(num2 + blockWidth + range, Width); j++)
			{
				int manhattanDistance = MathUtils.GetManhattanDistance(num, num2, i, j, blockWidth);
				if (manhattanDistance <= range && manhattanDistance > 0)
				{
					short num3 = (short)(j * Width + i);
					if (num3 != blockIndex && !neighborList.Contains(num3))
					{
						neighborList.Add(num3);
						neighborDistanceList?.Add(manhattanDistance);
					}
				}
			}
		}
	}
}
