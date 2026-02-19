using System.Collections.Generic;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.LifeRecord.GeneralRecord;

[SerializableGameData(NotForArchive = true, NoCopyConstructors = true)]
public struct RecordArgumentsRequest : ISerializableGameData
{
	[SerializableGameDataField]
	public List<int> Characters;

	[SerializableGameDataField]
	public List<Location> Locations;

	[SerializableGameDataField]
	public List<short> Settlements;

	[SerializableGameDataField]
	public List<int> JiaoLoongs;

	public RecordArgumentsRequest(ArgumentCollection argumentCollection)
	{
		Characters = argumentCollection.Characters;
		Locations = argumentCollection.Locations;
		Settlements = argumentCollection.Settlements;
		JiaoLoongs = argumentCollection.JiaoLoongs;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((Characters == null) ? (num + 2) : (num + (2 + 4 * Characters.Count)));
		num = ((Locations == null) ? (num + 2) : (num + (2 + 4 * Locations.Count)));
		num = ((Settlements == null) ? (num + 2) : (num + (2 + 2 * Settlements.Count)));
		num = ((JiaoLoongs == null) ? (num + 2) : (num + (2 + 4 * JiaoLoongs.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (Characters != null)
		{
			int count = Characters.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = Characters[i];
			}
			ptr += 4 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Locations != null)
		{
			int count2 = Locations.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += Locations[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Settlements != null)
		{
			int count3 = Settlements.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				((short*)ptr)[k] = Settlements[k];
			}
			ptr += 2 * count3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (JiaoLoongs != null)
		{
			int count4 = JiaoLoongs.Count;
			Tester.Assert(count4 <= 65535);
			*(ushort*)ptr = (ushort)count4;
			ptr += 2;
			for (int l = 0; l < count4; l++)
			{
				((int*)ptr)[l] = JiaoLoongs[l];
			}
			ptr += 4 * count4;
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
			if (Characters == null)
			{
				Characters = new List<int>(num);
			}
			else
			{
				Characters.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				Characters.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			Characters?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (Locations == null)
			{
				Locations = new List<Location>(num2);
			}
			else
			{
				Locations.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				Location item = default(Location);
				ptr += item.Deserialize(ptr);
				Locations.Add(item);
			}
		}
		else
		{
			Locations?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (Settlements == null)
			{
				Settlements = new List<short>(num3);
			}
			else
			{
				Settlements.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				Settlements.Add(((short*)ptr)[k]);
			}
			ptr += 2 * num3;
		}
		else
		{
			Settlements?.Clear();
		}
		ushort num4 = *(ushort*)ptr;
		ptr += 2;
		if (num4 > 0)
		{
			if (JiaoLoongs == null)
			{
				JiaoLoongs = new List<int>(num4);
			}
			else
			{
				JiaoLoongs.Clear();
			}
			for (int l = 0; l < num4; l++)
			{
				JiaoLoongs.Add(((int*)ptr)[l]);
			}
			ptr += 4 * num4;
		}
		else
		{
			JiaoLoongs?.Clear();
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
