using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Character.Relation.RelationTree;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotRestrictCollectionSerializedSize = true)]
public class Genealogy : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort CoreCharId = 0;

		public const ushort BloodFatherId = 1;

		public const ushort BloodMotherId = 2;

		public const ushort GrandfatherId = 3;

		public const ushort GrandmotherId = 4;

		public const ushort MaternalGrandfatherId = 5;

		public const ushort MaternalGrandmotherId = 6;

		public const ushort Parents = 7;

		public const ushort BrothersAndSisters = 8;

		public const ushort Spouses = 9;

		public const ushort Count = 10;

		public static readonly string[] FieldId2FieldName = new string[10] { "CoreCharId", "BloodFatherId", "BloodMotherId", "GrandfatherId", "GrandmotherId", "MaternalGrandfatherId", "MaternalGrandmotherId", "Parents", "BrothersAndSisters", "Spouses" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public int CoreCharId;

	[SerializableGameDataField(FieldIndex = 1)]
	public int BloodFatherId;

	[SerializableGameDataField(FieldIndex = 2)]
	public int BloodMotherId;

	[SerializableGameDataField(FieldIndex = 3)]
	public int GrandfatherId;

	[SerializableGameDataField(FieldIndex = 4)]
	public int GrandmotherId;

	[SerializableGameDataField(FieldIndex = 5)]
	public int MaternalGrandfatherId;

	[SerializableGameDataField(FieldIndex = 6)]
	public int MaternalGrandmotherId;

	[SerializableGameDataField(FieldIndex = 7, CollectionMaxElementsCount = int.MaxValue)]
	public List<CharIdAndRelation> Parents;

	[SerializableGameDataField(FieldIndex = 8, CollectionMaxElementsCount = int.MaxValue)]
	public List<CharIdAndRelation> BrothersAndSisters;

	[SerializableGameDataField(FieldIndex = 9, CollectionMaxElementsCount = int.MaxValue)]
	public List<SpouseAndChildren> Spouses;

	public Genealogy()
	{
		CoreCharId = -1;
		BloodFatherId = -1;
		BloodMotherId = -1;
		GrandfatherId = -1;
		GrandmotherId = -1;
		MaternalGrandfatherId = -1;
		MaternalGrandmotherId = -1;
		Parents = new List<CharIdAndRelation>();
		BrothersAndSisters = new List<CharIdAndRelation>();
		Spouses = new List<SpouseAndChildren>();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 30;
		num = ((Parents == null) ? (num + 4) : (num + (4 + default(CharIdAndRelation).GetSerializedSize() * Parents.Count)));
		num = ((BrothersAndSisters == null) ? (num + 4) : (num + (4 + default(CharIdAndRelation).GetSerializedSize() * BrothersAndSisters.Count)));
		if (Spouses != null)
		{
			num += 4;
			for (int i = 0; i < Spouses.Count; i++)
			{
				num = ((Spouses[i] == null) ? (num + 2) : (num + (2 + Spouses[i].GetSerializedSize())));
			}
		}
		else
		{
			num += 4;
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
		*(short*)ptr = 10;
		ptr += 2;
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
			Tester.Assert(count <= int.MaxValue);
			*(int*)ptr = count;
			ptr += 4;
			for (int i = 0; i < count; i++)
			{
				ptr += Parents[i].Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (BrothersAndSisters != null)
		{
			int count2 = BrothersAndSisters.Count;
			Tester.Assert(count2 <= int.MaxValue);
			*(int*)ptr = count2;
			ptr += 4;
			for (int j = 0; j < count2; j++)
			{
				ptr += BrothersAndSisters[j].Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (Spouses != null)
		{
			int count3 = Spouses.Count;
			Tester.Assert(count3 <= int.MaxValue);
			*(int*)ptr = count3;
			ptr += 4;
			for (int k = 0; k < count3; k++)
			{
				if (Spouses[k] != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = Spouses[k].Serialize(ptr);
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
			*(int*)ptr = 0;
			ptr += 4;
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
			CoreCharId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			BloodFatherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			BloodMotherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			GrandfatherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			GrandmotherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			MaternalGrandfatherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 6)
		{
			MaternalGrandmotherId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 7)
		{
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				if (Parents == null)
				{
					Parents = new List<CharIdAndRelation>();
				}
				else
				{
					Parents.Clear();
				}
				for (int i = 0; i < num2; i++)
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
		}
		if (num > 8)
		{
			int num3 = *(int*)ptr;
			ptr += 4;
			if (num3 > 0)
			{
				if (BrothersAndSisters == null)
				{
					BrothersAndSisters = new List<CharIdAndRelation>();
				}
				else
				{
					BrothersAndSisters.Clear();
				}
				for (int j = 0; j < num3; j++)
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
		}
		if (num > 9)
		{
			int num4 = *(int*)ptr;
			ptr += 4;
			if (num4 > 0)
			{
				if (Spouses == null)
				{
					Spouses = new List<SpouseAndChildren>();
				}
				else
				{
					Spouses.Clear();
				}
				for (int k = 0; k < num4; k++)
				{
					ushort num5 = *(ushort*)ptr;
					ptr += 2;
					SpouseAndChildren spouseAndChildren;
					if (num5 > 0)
					{
						spouseAndChildren = new SpouseAndChildren();
						ptr += spouseAndChildren.Deserialize(ptr);
					}
					else
					{
						spouseAndChildren = null;
					}
					Spouses.Add(spouseAndChildren);
				}
			}
			else
			{
				Spouses?.Clear();
			}
		}
		int num6 = (int)(ptr - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}
}
