using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Adventure;

public class AdventureMapTrunk : ISerializableGameData
{
	[SerializableGameDataField]
	public int BranchIndex = -1;

	[SerializableGameDataField]
	public List<AdventureMapPoint> Points = new List<AdventureMapPoint>();

	[SerializableGameDataField]
	public List<AdventureMapConnect> Connects = new List<AdventureMapConnect>();

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((Points == null) ? (num + 2) : (num + (2 + 24 * Points.Count)));
		if (Connects != null)
		{
			num += 2;
			int count = Connects.Count;
			for (int i = 0; i < count; i++)
			{
				AdventureMapConnect adventureMapConnect = Connects[i];
				num = ((adventureMapConnect == null) ? (num + 2) : (num + (2 + adventureMapConnect.GetSerializedSize())));
			}
		}
		else
		{
			num += 2;
		}
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(int*)ptr = BranchIndex;
		ptr += 4;
		if (Points != null)
		{
			int count = Points.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Points[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Connects != null)
		{
			int count2 = Connects.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				AdventureMapConnect adventureMapConnect = Connects[j];
				if (adventureMapConnect != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = adventureMapConnect.Serialize(ptr);
					ptr += num;
					Tester.Assert(num <= 65535);
					*(ushort*)intPtr = (ushort)num;
				}
				else
				{
					*(short*)ptr = 0;
					ptr += 2;
				}
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		BranchIndex = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Points == null)
			{
				Points = new List<AdventureMapPoint>(num);
			}
			else
			{
				Points.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				AdventureMapPoint adventureMapPoint = new AdventureMapPoint();
				ptr += adventureMapPoint.Deserialize(ptr);
				Points.Add(adventureMapPoint);
			}
		}
		else
		{
			Points?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (Connects == null)
			{
				Connects = new List<AdventureMapConnect>(num2);
			}
			else
			{
				Connects.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ushort num3 = *(ushort*)ptr;
				ptr += 2;
				if (num3 > 0)
				{
					AdventureMapConnect adventureMapConnect = new AdventureMapConnect();
					ptr += adventureMapConnect.Deserialize(ptr);
					Connects.Add(adventureMapConnect);
				}
				else
				{
					Connects.Add(null);
				}
			}
		}
		else
		{
			Connects?.Clear();
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
