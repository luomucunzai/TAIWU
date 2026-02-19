using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Adventure;

public class EnemyNestSiteExtraData : ISerializableGameData
{
	[SerializableGameDataField]
	public sbyte InitialNestTemplateId;

	[SerializableGameDataField]
	public short UpgradeChance;

	[SerializableGameDataField]
	public (sbyte Type, short TemplateId, int Amount) Tribute;

	[SerializableGameDataField]
	public List<MapTemplateEnemyInfo> RandomEnemies;

	[Obsolete]
	public short UpgradeCount => UpgradeChance;

	public EnemyNestSiteExtraData(sbyte initialNestTemplateId)
	{
		InitialNestTemplateId = initialNestTemplateId;
		UpgradeChance = 0;
		RandomEnemies = new List<MapTemplateEnemyInfo>();
		Tribute = (Type: -1, TemplateId: -1, Amount: -1);
	}

	public EnemyNestSiteExtraData()
	{
		RandomEnemies = new List<MapTemplateEnemyInfo>();
	}

	public EnemyNestSiteExtraData(EnemyNestSiteExtraData other)
	{
		InitialNestTemplateId = other.InitialNestTemplateId;
		UpgradeChance = other.UpgradeChance;
		Tribute = other.Tribute;
		RandomEnemies = new List<MapTemplateEnemyInfo>(other.RandomEnemies);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 10;
		num = ((RandomEnemies == null) ? (num + 2) : (num + (2 + 8 * RandomEnemies.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)InitialNestTemplateId;
		ptr++;
		*(short*)ptr = UpgradeChance;
		ptr += 2;
		*ptr = (byte)Tribute.Type;
		ptr++;
		*(short*)ptr = Tribute.TemplateId;
		ptr += 2;
		*(int*)ptr = Tribute.Amount;
		ptr += 4;
		if (RandomEnemies != null)
		{
			int count = RandomEnemies.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += RandomEnemies[i].Serialize(ptr);
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
		InitialNestTemplateId = (sbyte)(*ptr);
		ptr++;
		UpgradeChance = *(short*)ptr;
		ptr += 2;
		Tribute.Type = (sbyte)(*ptr);
		ptr++;
		Tribute.TemplateId = *(short*)ptr;
		ptr += 2;
		Tribute.Amount = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (RandomEnemies == null)
			{
				RandomEnemies = new List<MapTemplateEnemyInfo>(num);
			}
			else
			{
				RandomEnemies.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				MapTemplateEnemyInfo item = default(MapTemplateEnemyInfo);
				ptr += item.Deserialize(ptr);
				RandomEnemies.Add(item);
			}
		}
		else
		{
			RandomEnemies?.Clear();
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
