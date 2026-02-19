using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation.RelationTree;

[Obsolete]
[SerializableGameData(NotForDisplayModule = true)]
public class GenealogyObsoleted : ISerializableGameData
{
	[SerializableGameDataField]
	public int CoreCharId;

	[SerializableGameDataField]
	public int BloodFatherId;

	[SerializableGameDataField]
	public int BloodMotherId;

	[SerializableGameDataField]
	public int GrandfatherId;

	[SerializableGameDataField]
	public int GrandmotherId;

	[SerializableGameDataField]
	public int MaternalGrandfatherId;

	[SerializableGameDataField]
	public int MaternalGrandmotherId;

	[SerializableGameDataField]
	public List<CharIdAndRelation> Parents;

	[SerializableGameDataField]
	public List<CharIdAndRelation> BrothersAndSisters;

	[SerializableGameDataField]
	public List<SpouseAndChildrenObsoleted> Spouses;

	public GenealogyObsoleted()
	{
		CoreCharId = -1;
		BloodFatherId = -1;
		BloodMotherId = -1;
		GrandfatherId = -1;
		GrandmotherId = -1;
		MaternalGrandfatherId = -1;
		MaternalGrandmotherId = -1;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 28;
		num = ((Parents == null) ? (num + 2) : (num + (2 + 8 * Parents.Count)));
		num = ((BrothersAndSisters == null) ? (num + 2) : (num + (2 + 8 * BrothersAndSisters.Count)));
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
		*(int*)ptr = BloodFatherId;
		ptr += 4;
		*(int*)ptr = BloodMotherId;
		ptr += 4;
		*(int*)ptr = GrandfatherId;
		ptr += 4;
		*(int*)ptr = GrandmotherId;
		ptr += 4;
		*(int*)ptr = MaternalGrandfatherId;
		ptr += 4;
		*(int*)ptr = MaternalGrandmotherId;
		ptr += 4;
		if (Parents != null)
		{
			int count = Parents.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += Parents[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (BrothersAndSisters != null)
		{
			int count2 = BrothersAndSisters.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += BrothersAndSisters[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (Spouses != null)
		{
			int count3 = Spouses.Count;
			Tester.Assert(count3 <= 65535);
			*(ushort*)ptr = (ushort)count3;
			ptr += 2;
			for (int k = 0; k < count3; k++)
			{
				SpouseAndChildrenObsoleted spouseAndChildrenObsoleted = Spouses[k];
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
		BloodFatherId = *(int*)ptr;
		ptr += 4;
		BloodMotherId = *(int*)ptr;
		ptr += 4;
		GrandfatherId = *(int*)ptr;
		ptr += 4;
		GrandmotherId = *(int*)ptr;
		ptr += 4;
		MaternalGrandfatherId = *(int*)ptr;
		ptr += 4;
		MaternalGrandmotherId = *(int*)ptr;
		ptr += 4;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (Parents == null)
			{
				Parents = new List<CharIdAndRelation>(num);
			}
			else
			{
				Parents.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				CharIdAndRelation item = default(CharIdAndRelation);
				ptr += item.Deserialize(ptr);
				Parents.Add(item);
			}
		}
		else
		{
			Parents?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (BrothersAndSisters == null)
			{
				BrothersAndSisters = new List<CharIdAndRelation>(num2);
			}
			else
			{
				BrothersAndSisters.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				CharIdAndRelation item2 = default(CharIdAndRelation);
				ptr += item2.Deserialize(ptr);
				BrothersAndSisters.Add(item2);
			}
		}
		else
		{
			BrothersAndSisters?.Clear();
		}
		ushort num3 = *(ushort*)ptr;
		ptr += 2;
		if (num3 > 0)
		{
			if (Spouses == null)
			{
				Spouses = new List<SpouseAndChildrenObsoleted>(num3);
			}
			else
			{
				Spouses.Clear();
			}
			for (int k = 0; k < num3; k++)
			{
				ushort num4 = *(ushort*)ptr;
				ptr += 2;
				if (num4 > 0)
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
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
