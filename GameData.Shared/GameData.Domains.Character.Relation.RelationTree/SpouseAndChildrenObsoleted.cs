using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation.RelationTree;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class SpouseAndChildrenObsoleted : ISerializableGameData
{
	[SerializableGameDataField]
	public int SpouseCharId;

	[SerializableGameDataField]
	public List<CharIdAndRelation> Children;

	[SerializableGameDataField]
	public List<SpousesAndChildrenObsoleted> BloodChildrenSpouses;

	public SpouseAndChildrenObsoleted()
	{
		SpouseCharId = -1;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((Children == null) ? (num + 2) : (num + (2 + 8 * Children.Count)));
		if (BloodChildrenSpouses != null)
		{
			num += 2;
			int count = BloodChildrenSpouses.Count;
			for (int i = 0; i < count; i++)
			{
				SpousesAndChildrenObsoleted spousesAndChildrenObsoleted = BloodChildrenSpouses[i];
				num = ((spousesAndChildrenObsoleted == null) ? (num + 2) : (num + (2 + spousesAndChildrenObsoleted.GetSerializedSize())));
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
		*(int*)ptr = SpouseCharId;
		ptr += 4;
		if (Children != null)
		{
			int count = Children.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Children[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BloodChildrenSpouses != null)
		{
			int count2 = BloodChildrenSpouses.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				SpousesAndChildrenObsoleted spousesAndChildrenObsoleted = BloodChildrenSpouses[j];
				if (spousesAndChildrenObsoleted != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = spousesAndChildrenObsoleted.Serialize(ptr);
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
		SpouseCharId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Children == null)
			{
				Children = new List<CharIdAndRelation>(num);
			}
			else
			{
				Children.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharIdAndRelation item = default(CharIdAndRelation);
				ptr += item.Deserialize(ptr);
				Children.Add(item);
			}
		}
		else
		{
			Children?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (BloodChildrenSpouses == null)
			{
				BloodChildrenSpouses = new List<SpousesAndChildrenObsoleted>(num2);
			}
			else
			{
				BloodChildrenSpouses.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				ushort num3 = *(ushort*)ptr;
				ptr += 2;
				if (num3 > 0)
				{
					SpousesAndChildrenObsoleted spousesAndChildrenObsoleted = new SpousesAndChildrenObsoleted();
					ptr += spousesAndChildrenObsoleted.Deserialize(ptr);
					BloodChildrenSpouses.Add(spousesAndChildrenObsoleted);
				}
				else
				{
					BloodChildrenSpouses.Add(null);
				}
			}
		}
		else
		{
			BloodChildrenSpouses?.Clear();
		}
		int num4 = (int)(ptr - pData);
		if (num4 > 4)
		{
			return (num4 + 3) / 4 * 4;
		}
		return num4;
	}
}
