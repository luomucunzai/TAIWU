using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.Profession.SkillsData;

[SerializableGameData(IsExtensible = true)]
public class HunterSkillsData : IProfessionSkillsData, ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort UsedCarrierAnimalAttackCount = 0;

		public const ushort AnimalCharIdToItemKey = 1;

		public const ushort AnimalItemKeyToGender = 2;

		public const ushort AnimalCharIdToAttraction = 3;

		public const ushort Count = 4;

		public static readonly string[] FieldId2FieldName = new string[4] { "UsedCarrierAnimalAttackCount", "AnimalCharIdToItemKey", "AnimalItemKeyToGender", "AnimalCharIdToAttraction" };
	}

	public const sbyte CarrierAnimalAttackCountPerMonth = 3;

	[SerializableGameDataField]
	public sbyte UsedCarrierAnimalAttackCount;

	[SerializableGameDataField]
	public Dictionary<int, ItemKey> AnimalCharIdToItemKey;

	[SerializableGameDataField]
	public Dictionary<ItemKey, sbyte> AnimalItemKeyToGender;

	[SerializableGameDataField]
	public Dictionary<ItemKey, short> AnimalCharIdToAttraction;

	public sbyte RemainCount => (sbyte)MathUtils.Clamp(3 - UsedCarrierAnimalAttackCount, 0, 3);

	public void Initialize()
	{
		UsedCarrierAnimalAttackCount = 0;
		AnimalCharIdToItemKey?.Clear();
		AnimalItemKeyToGender?.Clear();
		AnimalCharIdToAttraction?.Clear();
	}

	public void InheritFrom(IProfessionSkillsData sourceData)
	{
		if (sourceData is ObsoleteHunterSkillsData obsoleteHunterSkillsData)
		{
			UsedCarrierAnimalAttackCount = obsoleteHunterSkillsData.UsedCarrierAnimalAttackCount;
		}
	}

	public HunterSkillsData()
	{
	}

	public HunterSkillsData(HunterSkillsData other)
	{
		UsedCarrierAnimalAttackCount = other.UsedCarrierAnimalAttackCount;
		AnimalCharIdToItemKey = ((other.AnimalCharIdToItemKey == null) ? null : new Dictionary<int, ItemKey>(other.AnimalCharIdToItemKey));
		AnimalItemKeyToGender = ((other.AnimalItemKeyToGender == null) ? null : new Dictionary<ItemKey, sbyte>(other.AnimalItemKeyToGender));
		AnimalCharIdToAttraction = ((other.AnimalCharIdToAttraction == null) ? null : new Dictionary<ItemKey, short>(other.AnimalCharIdToAttraction));
	}

	public void Assign(HunterSkillsData other)
	{
		UsedCarrierAnimalAttackCount = other.UsedCarrierAnimalAttackCount;
		AnimalCharIdToItemKey = ((other.AnimalCharIdToItemKey == null) ? null : new Dictionary<int, ItemKey>(other.AnimalCharIdToItemKey));
		AnimalItemKeyToGender = ((other.AnimalItemKeyToGender == null) ? null : new Dictionary<ItemKey, sbyte>(other.AnimalItemKeyToGender));
		AnimalCharIdToAttraction = ((other.AnimalCharIdToAttraction == null) ? null : new Dictionary<ItemKey, short>(other.AnimalCharIdToAttraction));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 3;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<int, ItemKey>(AnimalCharIdToItemKey);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, sbyte>(AnimalItemKeyToGender);
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, short>(AnimalCharIdToAttraction);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(short*)pData = 4;
		byte* num = pData + 2;
		*num = (byte)UsedCarrierAnimalAttackCount;
		byte* num2 = num + 1;
		byte* num3 = num2 + DictionaryOfBasicTypeCustomTypePair.Serialize<int, ItemKey>(num2, ref AnimalCharIdToItemKey);
		byte* num4 = num3 + DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, sbyte>(num3, ref AnimalItemKeyToGender);
		int num5 = (int)(num4 + DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, short>(num4, ref AnimalCharIdToAttraction) - pData);
		if (num5 > 4)
		{
			return (num5 + 3) / 4 * 4;
		}
		return num5;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			UsedCarrierAnimalAttackCount = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 1)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<int, ItemKey>(ptr, ref AnimalCharIdToItemKey);
		}
		if (num > 2)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, sbyte>(ptr, ref AnimalItemKeyToGender);
		}
		if (num > 3)
		{
			ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, short>(ptr, ref AnimalCharIdToAttraction);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
