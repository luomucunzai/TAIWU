using System.Collections.Generic;
using System.Linq;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Relation.RelationTree;

[SerializableGameData(NotForDisplayModule = true)]
public class SpousesAndChildren : ISerializableGameData
{
	[SerializableGameDataField]
	public int CoreCharId;

	[SerializableGameDataField]
	public List<SpouseAndChildren> Spouses;

	public SpousesAndChildren(int coreCharId, List<SpouseAndChildren> spouses)
	{
		CoreCharId = coreCharId;
		Spouses = spouses;
	}

	public SpousesAndChildren(SpousesAndChildrenObsoleted obsoleted)
	{
		CoreCharId = obsoleted.CoreCharId;
		Spouses = obsoleted.Spouses?.Select((SpouseAndChildrenObsoleted u) => new SpouseAndChildren(u)).ToList();
	}

	public SpousesAndChildren()
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
				SpouseAndChildren spouseAndChildren = Spouses[i];
				num = ((spouseAndChildren == null) ? (num + 2) : (num + (2 + spouseAndChildren.GetSerializedSize())));
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
				SpouseAndChildren spouseAndChildren = Spouses[i];
				if (spouseAndChildren != null)
				{
					byte* intPtr = ptr;
					ptr += 2;
					int num = spouseAndChildren.Serialize(ptr);
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
				Spouses = new List<SpouseAndChildren>(num);
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
					SpouseAndChildren spouseAndChildren = new SpouseAndChildren();
					ptr += spouseAndChildren.Deserialize(ptr);
					Spouses.Add(spouseAndChildren);
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
