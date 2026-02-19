using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.DLC;

public struct DlcInfoList : ISerializableGameData
{
	[SerializableGameDataField]
	public List<DlcInfo> Items;

	public static DlcInfoList Create()
	{
		DlcInfoList result = default(DlcInfoList);
		result.Items = new List<DlcInfo>();
		return result;
	}

	public DlcInfoList(DlcInfoList other)
	{
		if (other.Items != null)
		{
			List<DlcInfo> items = other.Items;
			int count = items.Count;
			Items = new List<DlcInfo>(count);
			for (int i = 0; i < count; i++)
			{
				Items.Add(new DlcInfo(items[i]));
			}
		}
		else
		{
			Items = null;
		}
	}

	public void Assign(DlcInfoList other)
	{
		if (other.Items != null)
		{
			List<DlcInfo> items = other.Items;
			int count = items.Count;
			Items = new List<DlcInfo>(count);
			for (int i = 0; i < count; i++)
			{
				Items.Add(new DlcInfo(items[i]));
			}
		}
		else
		{
			Items = null;
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		if (Items != null)
		{
			num += 2;
			int count = Items.Count;
			for (int i = 0; i < count; i++)
			{
				DlcInfo dlcInfo = Items[i];
				num = ((dlcInfo == null) ? (num + 2) : (num + (2 + dlcInfo.GetSerializedSize())));
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
		if (Items != null)
		{
			int count = Items.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				DlcInfo dlcInfo = Items[i];
				if (dlcInfo != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = dlcInfo.Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Items == null)
			{
				Items = new List<DlcInfo>(num);
			}
			else
			{
				Items.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					DlcInfo dlcInfo = new DlcInfo();
					ptr += dlcInfo.Deserialize(ptr);
					Items.Add(dlcInfo);
				}
				else
				{
					Items.Add(null);
				}
			}
		}
		else
		{
			Items?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
