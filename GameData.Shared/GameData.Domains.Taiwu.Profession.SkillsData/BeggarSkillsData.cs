using System;
using System.Collections.Generic;
using System.Text;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Domains.Map;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class BeggarSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort LookingForCharName = 0;

		public const ushort AlreadyFoundCharacters = 1;

		public const ushort ForbiddenLocations = 2;

		public const ushort EatenItems = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "LookingForCharName", "AlreadyFoundCharacters", "ForbiddenLocations", "EatenItems" };
	}

	[SerializableGameDataField]
	public string LookingForCharName;

	[SerializableGameDataField]
	public CharacterSet AlreadyFoundCharacters;

	[SerializableGameDataField]
	public List<Location> ForbiddenLocations;

	[Obsolete]
	[SerializableGameDataField]
	public List<ItemKey> EatenItems;

	public bool FoundMoreAlive;

	public bool FoundMoreDead;

	public void Initialize()
	{
		ClearData();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (sourceData is ObsoleteBeggarSkillsData obsoleteBeggarSkillsData)
		{
			LookingForCharName = obsoleteBeggarSkillsData.LookingForCharName;
			FoundMoreAlive = obsoleteBeggarSkillsData.FoundMoreAlive;
			FoundMoreDead = obsoleteBeggarSkillsData.FoundMoreDead;
			AlreadyFoundCharacters = obsoleteBeggarSkillsData.AlreadyFoundCharacters;
			if (ForbiddenLocations == null)
			{
				ForbiddenLocations = new List<Location>();
			}
		}
	}

	public void ClearData()
	{
		LookingForCharName = null;
		AlreadyFoundCharacters.Clear();
		FoundMoreDead = false;
		FoundMoreAlive = false;
		if (ForbiddenLocations != null)
		{
			ForbiddenLocations.Clear();
		}
		else
		{
			ForbiddenLocations = new List<Location>();
		}
		if (EatenItems != null)
		{
			EatenItems.Clear();
		}
		else
		{
			EatenItems = new List<ItemKey>();
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((LookingForCharName == null) ? (num + 2) : (num + (2 + 2 * LookingForCharName.Length)));
		num += AlreadyFoundCharacters.GetSerializedSize();
		num = ((ForbiddenLocations == null) ? (num + 2) : (num + (2 + 4 * ForbiddenLocations.Count)));
		num = ((EatenItems == null) ? (num + 2) : (num + (2 + 8 * EatenItems.Count)));
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		if (LookingForCharName != null)
		{
			int length = LookingForCharName.Length;
			Tester.Assert(length <= 65535);
			*(ushort*)ptr = (ushort)length;
			ptr += 2;
			fixed (char* lookingForCharName = LookingForCharName)
			{
				for (int i = 0; i < length; i++)
				{
					((short*)ptr)[i] = (short)lookingForCharName[i];
				}
			}
			ptr += 2 * length;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = AlreadyFoundCharacters.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		if (ForbiddenLocations != null)
		{
			int count = ForbiddenLocations.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int j = 0; j < count; j++)
			{
				ptr += ForbiddenLocations[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (EatenItems != null)
		{
			int count2 = EatenItems.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int k = 0; k < count2; k++)
			{
				ptr += EatenItems[k].Serialize(ptr);
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
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				int num3 = 2 * num2;
				LookingForCharName = Encoding.Unicode.GetString(ptr, num3);
				ptr += num3;
			}
			else
			{
				LookingForCharName = null;
			}
		}
		if (num > 1)
		{
			ptr += AlreadyFoundCharacters.Deserialize(ptr);
		}
		if (num > 2)
		{
			ushort num4 = *(ushort*)ptr;
			ptr += 2;
			if (num4 > 0)
			{
				if (ForbiddenLocations == null)
				{
					ForbiddenLocations = new List<Location>(num4);
				}
				else
				{
					ForbiddenLocations.Clear();
				}
				for (int i = 0; i < num4; i++)
				{
					Location item = default(Location);
					ptr += item.Deserialize(ptr);
					ForbiddenLocations.Add(item);
				}
			}
			else
			{
				ForbiddenLocations?.Clear();
			}
		}
		if (num > 3)
		{
			ushort num5 = *(ushort*)ptr;
			ptr += 2;
			if (num5 > 0)
			{
				if (EatenItems == null)
				{
					EatenItems = new List<ItemKey>(num5);
				}
				else
				{
					EatenItems.Clear();
				}
				for (int j = 0; j < num5; j++)
				{
					ItemKey item2 = default(ItemKey);
					ptr += item2.Deserialize(ptr);
					EatenItems.Add(item2);
				}
			}
			else
			{
				EatenItems?.Clear();
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
