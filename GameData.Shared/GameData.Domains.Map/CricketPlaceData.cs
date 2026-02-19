using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Map;

public class CricketPlaceData : ISerializableGameData
{
	[SerializableGameDataField]
	public short[] CricketBlocks;

	[SerializableGameDataField]
	public bool[] CricketTriggered;

	[SerializableGameDataField]
	public byte[] RealCircketIdx;

	public CricketPlaceData()
	{
	}

	public CricketPlaceData(CricketPlaceData other)
	{
		short[] cricketBlocks = other.CricketBlocks;
		int num = cricketBlocks.Length;
		CricketBlocks = new short[num];
		for (int i = 0; i < num; i++)
		{
			CricketBlocks[i] = cricketBlocks[i];
		}
		bool[] cricketTriggered = other.CricketTriggered;
		int num2 = cricketTriggered.Length;
		CricketTriggered = new bool[num2];
		for (int j = 0; j < num2; j++)
		{
			CricketTriggered[j] = cricketTriggered[j];
		}
		byte[] realCircketIdx = other.RealCircketIdx;
		int num3 = realCircketIdx.Length;
		RealCircketIdx = new byte[num3];
		for (int k = 0; k < num3; k++)
		{
			RealCircketIdx[k] = realCircketIdx[k];
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((CricketBlocks == null) ? (num + 2) : (num + (2 + 2 * CricketBlocks.Length)));
		num = ((CricketTriggered == null) ? (num + 2) : (num + (2 + CricketTriggered.Length)));
		num = ((RealCircketIdx == null) ? (num + 2) : (num + (2 + RealCircketIdx.Length)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (CricketBlocks != null)
		{
			int num = CricketBlocks.Length;
			Tester.Assert(num <= 65535);
			*(ushort*)ptr = (ushort)num;
			ptr += 2;
			for (int i = 0; i < num; i++)
			{
				((short*)ptr)[i] = CricketBlocks[i];
			}
			ptr += 2 * num;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (CricketTriggered != null)
		{
			int num2 = CricketTriggered.Length;
			Tester.Assert(num2 <= 65535);
			*(ushort*)ptr = (ushort)num2;
			ptr += 2;
			for (int j = 0; j < num2; j++)
			{
				ptr[j] = (CricketTriggered[j] ? ((byte)1) : ((byte)0));
			}
			ptr += num2;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (RealCircketIdx != null)
		{
			int num3 = RealCircketIdx.Length;
			Tester.Assert(num3 <= 65535);
			*(ushort*)ptr = (ushort)num3;
			ptr += 2;
			for (int k = 0; k < num3; k++)
			{
				ptr[k] = RealCircketIdx[k];
			}
			ptr += num3;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CricketBlocks == null || CricketBlocks.Length != num)
			{
				CricketBlocks = new short[num];
			}
			for (int i = 0; i < num; i++)
			{
				CricketBlocks[i] = ((short*)ptr)[i];
			}
			ptr += 2 * num;
		}
		else
		{
			CricketBlocks = null;
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (CricketTriggered == null || CricketTriggered.Length != num2)
			{
				CricketTriggered = new bool[num2];
			}
			for (int j = 0; j < num2; j++)
			{
				CricketTriggered[j] = ptr[j] != 0;
			}
			ptr += (int)num2;
		}
		else
		{
			CricketTriggered = null;
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (RealCircketIdx == null || RealCircketIdx.Length != num3)
			{
				RealCircketIdx = new byte[num3];
			}
			for (int k = 0; k < num3; k++)
			{
				RealCircketIdx[k] = ptr[k];
			}
			ptr += (int)num3;
		}
		else
		{
			RealCircketIdx = null;
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
