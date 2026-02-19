using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

public class TravelRoute : ISerializableGameData
{
	public readonly List<ByteCoordinate> PosList = new List<ByteCoordinate>();

	public readonly List<short> AreaList = new List<short>();

	public readonly List<short> CostList = new List<short>();

	public TravelRoute()
	{
	}

	public TravelRoute(TravelRoute other)
	{
		PosList.Clear();
		PosList.AddRange(other.PosList);
		AreaList.Clear();
		AreaList.AddRange(other.AreaList);
		CostList.Clear();
		CostList.AddRange(other.CostList);
	}

	public short GetTotalTimeCost()
	{
		int num = 0;
		for (int i = 0; i < CostList.Count; i++)
		{
			num += CostList[i];
		}
		return (short)num;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return 2 + 2 * PosList.Count + 2 + 2 * AreaList.Count + 2 + 2 * CostList.Count;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		int count = PosList.Count;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int i = 0; i < PosList.Count; i++)
		{
			ByteCoordinate byteCoordinate = PosList[i];
			*ptr = byteCoordinate.X;
			ptr++;
			*ptr = byteCoordinate.Y;
			ptr++;
		}
		count = AreaList.Count;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int j = 0; j < AreaList.Count; j++)
		{
			*(short*)ptr = AreaList[j];
			ptr += 2;
		}
		count = CostList.Count;
		*(ushort*)ptr = (ushort)count;
		ptr += 2;
		for (int k = 0; k < CostList.Count; k++)
		{
			*(short*)ptr = CostList[k];
			ptr += 2;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		PosList.Clear();
		PosList.Capacity = num;
		ByteCoordinate item = default(ByteCoordinate);
		for (int i = 0; i < num; i++)
		{
			item.X = *ptr;
			ptr++;
			item.Y = *ptr;
			ptr++;
			PosList.Add(item);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		AreaList.Clear();
		AreaList.Capacity = num;
		for (int j = 0; j < num; j++)
		{
			short item2 = *(short*)ptr;
			ptr += 2;
			AreaList.Add(item2);
		}
		num = *(ushort*)ptr;
		ptr += 2;
		CostList.Clear();
		CostList.Capacity = num;
		for (int k = 0; k < num; k++)
		{
			short item3 = *(short*)ptr;
			ptr += 2;
			CostList.Add(item3);
		}
		return (int)(ptr - pData);
	}
}
