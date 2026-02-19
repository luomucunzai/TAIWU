using System.Collections.Generic;
using GameData.Domains.Character.Ai;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu;

[SerializableGameData(IsExtensible = true)]
public class VillagerTreasuryNeed : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort PersonalNeeds = 0;

		public const ushort Count = 1;

		public static readonly string[] FieldId2FieldName = new string[1] { "PersonalNeeds" };
	}

	[SerializableGameDataField]
	public List<PersonalNeed> PersonalNeeds;

	public VillagerTreasuryNeed()
	{
	}

	public VillagerTreasuryNeed(VillagerTreasuryNeed other)
	{
		PersonalNeeds = ((other.PersonalNeeds == null) ? null : new List<PersonalNeed>(other.PersonalNeeds));
	}

	public void Assign(VillagerTreasuryNeed other)
	{
		PersonalNeeds = ((other.PersonalNeeds == null) ? null : new List<PersonalNeed>(other.PersonalNeeds));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((PersonalNeeds == null) ? (num + 2) : (num + (2 + 8 * PersonalNeeds.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 1;
		ptr += 2;
		if (PersonalNeeds != null)
		{
			int count = PersonalNeeds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += PersonalNeeds[i].Serialize(ptr);
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
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (PersonalNeeds == null)
				{
					PersonalNeeds = new List<PersonalNeed>(num2);
				}
				else
				{
					PersonalNeeds.Clear();
				}
				for (int i = 0; i < num2; i++)
				{
					PersonalNeed item = default(PersonalNeed);
					ptr += item.Deserialize(ptr);
					PersonalNeeds.Add(item);
				}
			}
			else
			{
				PersonalNeeds?.Clear();
			}
		}
		int num3 = (int)(ptr - pData);
		if (num3 > 4)
		{
			return (num3 + 3) / 4 * 4;
		}
		return num3;
	}
}
