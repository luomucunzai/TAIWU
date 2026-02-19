using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Mod;

public struct ModInfoList : ISerializableGameData
{
	[SerializableGameDataField]
	public List<ModInfo> Items;

	public static ModInfoList Create()
	{
		ModInfoList result = default(ModInfoList);
		result.Items = new List<ModInfo>();
		return result;
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
				ModInfo modInfo = Items[i];
				num = ((modInfo == null) ? (num + 2) : (num + (2 + modInfo.GetSerializedSize())));
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
				ModInfo modInfo = Items[i];
				if (modInfo != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = modInfo.Serialize(ptr);
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
				Items = new List<ModInfo>(num);
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
					ModInfo modInfo = new ModInfo();
					ptr += modInfo.Deserialize(ptr);
					Items.Add(modInfo);
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
