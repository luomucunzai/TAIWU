using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Global;

public class ArchiveInfo : ISerializableGameData
{
	public sbyte Status;

	public WorldInfo WorldInfo;

	public List<(long timestamp, WorldInfo worldInfo)> BackupWorldsInfo;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num++;
		num += 4;
		if (WorldInfo != null)
		{
			num += WorldInfo.GetSerializedSize();
		}
		int count = BackupWorldsInfo.Count;
		Tester.Assert(count <= 255);
		num++;
		for (int i = 0; i < count; i++)
		{
			WorldInfo item = BackupWorldsInfo[i].worldInfo;
			num += 8;
			if (item != null)
			{
				num += 4;
				num += item.GetSerializedSize();
			}
			else
			{
				num += 4;
			}
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
		*ptr = (byte)Status;
		ptr++;
		int num = WorldInfo?.GetSerializedSize() ?? 0;
		*(int*)ptr = num;
		ptr += 4;
		if (WorldInfo != null)
		{
			ptr += WorldInfo.Serialize(ptr);
		}
		int count = BackupWorldsInfo.Count;
		Tester.Assert(count <= 255);
		*ptr = (byte)count;
		ptr++;
		for (int i = 0; i < count; i++)
		{
			(long timestamp, WorldInfo worldInfo) tuple = BackupWorldsInfo[i];
			long item = tuple.timestamp;
			WorldInfo item2 = tuple.worldInfo;
			*(long*)ptr = item;
			ptr += 8;
			if (item2 != null)
			{
				int serializedSize = item2.GetSerializedSize();
				*(int*)ptr = serializedSize;
				ptr += 4;
				ptr += item2.Serialize(ptr);
			}
			else
			{
				*(int*)ptr = 0;
				ptr += 4;
			}
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
		Status = (sbyte)(*ptr);
		ptr++;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (WorldInfo == null)
			{
				WorldInfo = new WorldInfo();
			}
			ptr += WorldInfo.Deserialize(ptr);
		}
		else
		{
			WorldInfo = null;
		}
		byte b = *ptr;
		ptr++;
		if (BackupWorldsInfo == null)
		{
			BackupWorldsInfo = new List<(long, WorldInfo)>(b);
		}
		else
		{
			BackupWorldsInfo.Clear();
		}
		for (int i = 0; i < b; i++)
		{
			long item = *(long*)ptr;
			ptr += 8;
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				WorldInfo worldInfo = new WorldInfo();
				ptr += worldInfo.Deserialize(ptr);
				BackupWorldsInfo.Add((item, worldInfo));
			}
			else
			{
				BackupWorldsInfo.Add((item, null));
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
