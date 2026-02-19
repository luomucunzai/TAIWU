using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Display;

[SerializableGameData(NoCopyConstructors = true, NotForArchive = true)]
public class VillagerRoleManageDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public short RoleTemplateId;

	[Obsolete]
	[SerializableGameDataField]
	public int AvailableSeats;

	[SerializableGameDataField]
	public List<int> CharacterIds;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((CharacterIds == null) ? (num + 2) : (num + (2 + 4 * CharacterIds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = RoleTemplateId;
		ptr += 2;
		*(int*)ptr = AvailableSeats;
		ptr += 4;
		if (CharacterIds != null)
		{
			int count = CharacterIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				((int*)ptr)[i] = CharacterIds[i];
			}
			ptr += 4 * count;
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
		RoleTemplateId = *(short*)ptr;
		ptr += 2;
		AvailableSeats = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CharacterIds == null)
			{
				CharacterIds = new List<int>(num);
			}
			else
			{
				CharacterIds.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharacterIds.Add(((int*)ptr)[i]);
			}
			ptr += 4 * num;
		}
		else
		{
			CharacterIds?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
