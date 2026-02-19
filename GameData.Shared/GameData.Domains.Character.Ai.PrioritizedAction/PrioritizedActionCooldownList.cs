using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData]
public class PrioritizedActionCooldownList : ISerializableGameData
{
	[SerializableGameDataField]
	public List<PrioritizedActionCooldown> DataList = new List<PrioritizedActionCooldown>();

	public void Add(PrioritizedActionCooldown cooldown)
	{
		if (Contains(cooldown))
		{
			DataList.Remove(cooldown);
		}
		DataList.Add(cooldown);
	}

	public void Remove(PrioritizedActionCooldown cooldown)
	{
		if (Contains(cooldown))
		{
			DataList.Remove(cooldown);
		}
	}

	public int Get(short templateId)
	{
		foreach (PrioritizedActionCooldown data in DataList)
		{
			if (templateId == data.TemplateId)
			{
				return data.Cooldown;
			}
		}
		return 0;
	}

	public bool Contains(PrioritizedActionCooldown cooldown)
	{
		foreach (PrioritizedActionCooldown data in DataList)
		{
			if (cooldown == data)
			{
				return true;
			}
		}
		return false;
	}

	public bool Contains(short templateId)
	{
		foreach (PrioritizedActionCooldown data in DataList)
		{
			if (templateId == data.TemplateId)
			{
				return true;
			}
		}
		return false;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num = ((DataList == null) ? (num + 2) : (num + (2 + 6 * DataList.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		if (DataList != null)
		{
			int count = DataList.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += DataList[i].Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (DataList == null)
			{
				DataList = new List<PrioritizedActionCooldown>(num);
			}
			else
			{
				DataList.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				PrioritizedActionCooldown item = default(PrioritizedActionCooldown);
				ptr += item.Deserialize(ptr);
				DataList.Add(item);
			}
		}
		else
		{
			DataList?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
