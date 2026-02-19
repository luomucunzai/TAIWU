using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation.RelationTree;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class SpousesAndChildrenObsoleted : ISerializableGameData
{
	[SerializableGameDataField]
	public int CoreCharId;

	[SerializableGameDataField]
	public List<SpouseAndChildrenObsoleted> Spouses;

	public SpousesAndChildrenObsoleted(int coreCharId, List<SpouseAndChildrenObsoleted> spouses)
	{
		CoreCharId = coreCharId;
		Spouses = spouses;
	}

	public SpousesAndChildrenObsoleted()
	{
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		if (Spouses != null)
		{
			num += 2;
			int count = Spouses.Count;
			for (int i = 0; i < count; i++)
			{
				SpouseAndChildrenObsoleted spouseAndChildrenObsoleted = Spouses[i];
				num = ((spouseAndChildrenObsoleted == null) ? (num + 2) : (num + (2 + spouseAndChildrenObsoleted.GetSerializedSize())));
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
		*(int*)ptr = CoreCharId;
		ptr += 4;
		if (Spouses != null)
		{
			int count = Spouses.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				SpouseAndChildrenObsoleted spouseAndChildrenObsoleted = Spouses[i];
				if (spouseAndChildrenObsoleted != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = spouseAndChildrenObsoleted.Serialize(ptr);
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
		CoreCharId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Spouses == null)
			{
				Spouses = new List<SpouseAndChildrenObsoleted>(num);
			}
			else
			{
				Spouses.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				ushort num2 = *(ushort*)ptr;
				ptr += 2;
				if (num2 > 0)
				{
					SpouseAndChildrenObsoleted spouseAndChildrenObsoleted = new SpouseAndChildrenObsoleted();
					ptr += spouseAndChildrenObsoleted.Deserialize(ptr);
					Spouses.Add(spouseAndChildrenObsoleted);
				}
				else
				{
					Spouses.Add(null);
				}
			}
		}
		else
		{
			Spouses?.Clear();
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
