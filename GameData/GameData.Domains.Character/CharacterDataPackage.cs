using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[SerializableGameData(NotForDisplayModule = true, NoCopyConstructors = true, IsExtensible = true)]
public class CharacterDataPackage : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Character = 0;

		public const ushort CombatSkills = 1;

		public const ushort CombatSkillProficiencies = 2;

		public const ushort ItemGroupPackage = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "Character", "CombatSkills", "CombatSkillProficiencies", "ItemGroupPackage" };
	}

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public Character Character;

	[SerializableGameDataField]
	public Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CombatSkills;

	[SerializableGameDataField]
	public Dictionary<short, int> CombatSkillProficiencies;

	[SerializableGameDataField(SubDataMaxCount = int.MaxValue)]
	public ItemGroupPackage ItemGroupPackage;

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 2;
		num = ((Character == null) ? (num + 4) : (num + (4 + Character.GetSerializedSize())));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, GameData.Domains.CombatSkill.CombatSkill>(CombatSkills);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)CombatSkillProficiencies);
		num = ((ItemGroupPackage == null) ? (num + 4) : (num + (4 + ItemGroupPackage.GetSerializedSize())));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 4;
		ptr += 2;
		if (Character != null)
		{
			byte* ptr2 = ptr;
			ptr += 4;
			int num = Character.Serialize(ptr);
			ptr += num;
			Tester.Assert(num <= int.MaxValue);
			*(int*)ptr2 = num;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, GameData.Domains.CombatSkill.CombatSkill>(ptr, ref CombatSkills);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref CombatSkillProficiencies);
		if (ItemGroupPackage != null)
		{
			byte* ptr3 = ptr;
			ptr += 4;
			int num2 = ItemGroupPackage.Serialize(ptr);
			ptr += num2;
			Tester.Assert(num2 <= int.MaxValue);
			*(int*)ptr3 = num2;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			int num2 = *(int*)ptr;
			ptr += 4;
			if (num2 > 0)
			{
				if (Character == null)
				{
					Character = new Character();
				}
				ptr += Character.Deserialize(ptr);
			}
			else
			{
				Character = null;
			}
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, GameData.Domains.CombatSkill.CombatSkill>(ptr, ref CombatSkills);
		}
		if (num > 2)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref CombatSkillProficiencies);
		}
		if (num > 3)
		{
			int num3 = *(int*)ptr;
			ptr += 4;
			if (num3 > 0)
			{
				if (ItemGroupPackage == null)
				{
					ItemGroupPackage = new ItemGroupPackage();
				}
				ptr += ItemGroupPackage.Deserialize(ptr);
			}
			else
			{
				ItemGroupPackage = null;
			}
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
