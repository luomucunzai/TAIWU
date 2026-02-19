using System.Collections.Generic;
using System.Linq;
using GameData.Serializer;
using GameData.Utilities;
using SerializableGameDataSourceGenerator;

namespace GameData.Domains.Character.Relation.RelationTree;

[AutoGenerateSerializableGameData(IsExtensible = true, NoCopyConstructors = true, NotRestrictCollectionSerializedSize = true)]
public class SpouseAndChildren : ISerializableGameData
{
	public static class FieldIds
	{
		public const ushort SpouseCharId = 0;

		public const ushort Children = 1;

		public const ushort BloodChildrenSpouses = 2;

		public const ushort Count = 3;

		public static readonly string[] FieldId2FieldName = new string[3] { "SpouseCharId", "Children", "BloodChildrenSpouses" };
	}

	[SerializableGameDataField(FieldIndex = 0)]
	public int SpouseCharId;

	[SerializableGameDataField(FieldIndex = 1, CollectionMaxElementsCount = int.MaxValue)]
	public List<CharIdAndRelation> Children;

	[SerializableGameDataField(FieldIndex = 2, CollectionMaxElementsCount = int.MaxValue)]
	public List<SpousesAndChildren> BloodChildrenSpouses;

	public SpouseAndChildren()
	{
		SpouseCharId = -1;
	}

	public SpouseAndChildren(SpouseAndChildrenObsoleted obsoleted)
	{
		SpouseCharId = obsoleted.SpouseCharId;
		Children = obsoleted.Children?.ToList();
		BloodChildrenSpouses = obsoleted.BloodChildrenSpouses?.Select((SpousesAndChildrenObsoleted u) => new SpousesAndChildren
		{
			CoreCharId = u.CoreCharId,
			Spouses = u.Spouses?.Select((SpouseAndChildrenObsoleted obsoleted2) => new SpouseAndChildren(obsoleted2)).ToList()
		}).ToList();
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		num = ((Children == null) ? (num + 4) : (num + (4 + default(CharIdAndRelation).GetSerializedSize() * Children.Count)));
		if (BloodChildrenSpouses != null)
		{
			num += 4;
			for (int i = 0; i < BloodChildrenSpouses.Count; i++)
			{
				num = ((BloodChildrenSpouses[i] == null) ? (num + 2) : (num + (2 + BloodChildrenSpouses[i].GetSerializedSize())));
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
		*(short*)ptr = 3;
		ptr += 2;
		*(int*)ptr = SpouseCharId;
		ptr += 4;
		if (Children != null)
		{
			int count = Children.Count;
			Tester.Assert(count <= int.MaxValue);
			*(int*)ptr = count;
			ptr += 4;
			for (int i = 0; i < count; i++)
			{
				ptr += Children[i].Serialize(ptr);
			}
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		if (BloodChildrenSpouses != null)
		{
			int count2 = BloodChildrenSpouses.Count;
			Tester.Assert(count2 <= int.MaxValue);
			*(int*)ptr = count2;
			ptr += 4;
			for (int j = 0; j < count2; j++)
			{
				if (BloodChildrenSpouses[j] != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = BloodChildrenSpouses[j].Serialize(ptr);
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
			SpouseCharId = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				if (Children == null)
				{
					Children = new List<CharIdAndRelation>();
				}
				else
				{
					Children.Clear();
				}
				for (int i = 0; i < num2; i++)
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
		}
		if (num > 2)
		{
			int num3 = *(int*)ptr;
			ptr += 4;
			if (num3 > 0)
			{
				if (BloodChildrenSpouses == null)
				{
					BloodChildrenSpouses = new List<SpousesAndChildren>();
				}
				else
				{
					BloodChildrenSpouses.Clear();
				}
				for (int j = 0; j < num3; j++)
				{
					ushort num4 = *(ushort*)ptr;
					ptr += 2;
					SpousesAndChildren spousesAndChildren;
					if (num4 > 0)
					{
						spousesAndChildren = new SpousesAndChildren();
						ptr += spousesAndChildren.Deserialize(ptr);
					}
					else
					{
						spousesAndChildren = null;
					}
					BloodChildrenSpouses.Add(spousesAndChildren);
				}
			}
			else
			{
				BloodChildrenSpouses?.Clear();
			}
		}
		int num5 = (int)(ptr - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}
}
