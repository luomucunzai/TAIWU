using System.Collections.Generic;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true, NoCopyConstructors = true)]
public class GearMate : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort Id = 0;

		public const ushort MainAttributeProgress = 1;

		public const ushort ConsummateLevelProgress = 2;

		public const ushort FeatureProgress = 3;

		public const ushort Neili = 4;

		public const ushort LifeSkillReadingProgress = 5;

		public const ushort CombatSkillReadingProgress = 6;

		public const ushort SkillBreakBonusDictObsolete = 7;

		public const ushort CombatSkillAttainmentProgress = 8;

		public const ushort LifeSkillAttainmentProgress = 9;

		public const ushort NeiliType = 10;

		public const ushort SkillBreakBonusDict = 11;

		public const ushort SkillBreakMaxPowerDict = 12;

		public const ushort Count = 13;

		public static readonly string[] FieldId2FieldName = new string[13]
		{
			"Id", "MainAttributeProgress", "ConsummateLevelProgress", "FeatureProgress", "Neili", "LifeSkillReadingProgress", "CombatSkillReadingProgress", "SkillBreakBonusDictObsolete", "CombatSkillAttainmentProgress", "LifeSkillAttainmentProgress",
			"NeiliType", "SkillBreakBonusDict", "SkillBreakMaxPowerDict"
		};
	}

	[SerializableGameDataField]
	public int Id;

	[SerializableGameDataField(ArrayElementsCount = 6)]
	public int[] MainAttributeProgress;

	[SerializableGameDataField]
	public int ConsummateLevelProgress;

	[SerializableGameDataField]
	public int FeatureProgress;

	[SerializableGameDataField]
	public int Neili;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuLifeSkill> LifeSkillReadingProgress;

	[SerializableGameDataField]
	public Dictionary<short, TaiwuCombatSkill> CombatSkillReadingProgress;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakPlateBonusList> SkillBreakBonusDict;

	[SerializableGameDataField]
	public Dictionary<short, int> SkillBreakMaxPowerDict;

	[SerializableGameDataField]
	public Dictionary<short, SkillBreakBonusCollection> SkillBreakBonusDictObsolete;

	[SerializableGameDataField(ArrayElementsCount = 14)]
	public int[] CombatSkillAttainmentProgress;

	[SerializableGameDataField(ArrayElementsCount = 16)]
	public int[] LifeSkillAttainmentProgress;

	[SerializableGameDataField]
	public int NeiliType;

	public GearMate()
	{
		MainAttributeProgress = new int[6];
		LifeSkillReadingProgress = new Dictionary<short, TaiwuLifeSkill>();
		CombatSkillReadingProgress = new Dictionary<short, TaiwuCombatSkill>();
		SkillBreakBonusDict = new Dictionary<short, SkillBreakPlateBonusList>();
		SkillBreakBonusDictObsolete = new Dictionary<short, SkillBreakBonusCollection>();
		CombatSkillAttainmentProgress = new int[14];
		LifeSkillAttainmentProgress = new int[16];
		NeiliType = 5;
		SkillBreakMaxPowerDict = new Dictionary<short, int>();
	}

	public GearMate(int charId)
	{
		Id = charId;
		MainAttributeProgress = new int[6];
		LifeSkillReadingProgress = new Dictionary<short, TaiwuLifeSkill>();
		CombatSkillReadingProgress = new Dictionary<short, TaiwuCombatSkill>();
		SkillBreakBonusDict = new Dictionary<short, SkillBreakPlateBonusList>();
		SkillBreakBonusDictObsolete = new Dictionary<short, SkillBreakBonusCollection>();
		CombatSkillAttainmentProgress = new int[14];
		LifeSkillAttainmentProgress = new int[16];
		NeiliType = 5;
		SkillBreakMaxPowerDict = new Dictionary<short, int>();
	}

	public bool IsCombatSkillBuffed(sbyte combatSkillType, sbyte grade)
	{
		return (CombatSkillAttainmentProgress[combatSkillType] & (1 << (int)grade)) != 0;
	}

	public bool IsLifeSkillBuffed(sbyte lifeSkillType, sbyte grade)
	{
		return (LifeSkillAttainmentProgress[lifeSkillType] & (1 << (int)grade)) != 0;
	}

	public void SetCombatSkillBuffed(sbyte combatSkillType, sbyte grade)
	{
		CombatSkillAttainmentProgress[combatSkillType] |= 1 << (int)grade;
	}

	public void SetLifeSkillBuffed(sbyte lifeSkillType, sbyte grade)
	{
		LifeSkillAttainmentProgress[lifeSkillType] |= 1 << (int)grade;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 166;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(LifeSkillReadingProgress);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(CombatSkillReadingProgress);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(SkillBreakBonusDictObsolete);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateBonusList>(SkillBreakBonusDict);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)SkillBreakMaxPowerDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 13;
		ptr += 2;
		*(int*)ptr = Id;
		ptr += 4;
		Tester.Assert(MainAttributeProgress.Length == 6);
		for (int i = 0; i < 6; i++)
		{
			((int*)ptr)[i] = MainAttributeProgress[i];
		}
		ptr += 24;
		*(int*)ptr = ConsummateLevelProgress;
		ptr += 4;
		*(int*)ptr = FeatureProgress;
		ptr += 4;
		*(int*)ptr = Neili;
		ptr += 4;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(ptr, ref LifeSkillReadingProgress);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(ptr, ref CombatSkillReadingProgress);
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(ptr, ref SkillBreakBonusDictObsolete);
		Tester.Assert(CombatSkillAttainmentProgress.Length == 14);
		for (int j = 0; j < 14; j++)
		{
			((int*)ptr)[j] = CombatSkillAttainmentProgress[j];
		}
		ptr += 56;
		Tester.Assert(LifeSkillAttainmentProgress.Length == 16);
		for (int k = 0; k < 16; k++)
		{
			((int*)ptr)[k] = LifeSkillAttainmentProgress[k];
		}
		ptr += 64;
		*(int*)ptr = NeiliType;
		ptr += 4;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateBonusList>(ptr, ref SkillBreakBonusDict);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref SkillBreakMaxPowerDict);
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
			Id = *(int*)ptr;
			ptr += 4;
		}
		if (num > 1)
		{
			if (MainAttributeProgress == null || MainAttributeProgress.Length != 6)
			{
				MainAttributeProgress = new int[6];
			}
			for (int i = 0; i < 6; i++)
			{
				MainAttributeProgress[i] = ((int*)ptr)[i];
			}
			ptr += 24;
		}
		if (num > 2)
		{
			ConsummateLevelProgress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			FeatureProgress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 4)
		{
			Neili = *(int*)ptr;
			ptr += 4;
		}
		if (num > 5)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(ptr, ref LifeSkillReadingProgress);
		}
		if (num > 6)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(ptr, ref CombatSkillReadingProgress);
		}
		if (num > 7)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(ptr, ref SkillBreakBonusDictObsolete);
		}
		if (num > 8)
		{
			if (CombatSkillAttainmentProgress == null || CombatSkillAttainmentProgress.Length != 14)
			{
				CombatSkillAttainmentProgress = new int[14];
			}
			for (int j = 0; j < 14; j++)
			{
				CombatSkillAttainmentProgress[j] = ((int*)ptr)[j];
			}
			ptr += 56;
		}
		if (num > 9)
		{
			if (LifeSkillAttainmentProgress == null || LifeSkillAttainmentProgress.Length != 16)
			{
				LifeSkillAttainmentProgress = new int[16];
			}
			for (int k = 0; k < 16; k++)
			{
				LifeSkillAttainmentProgress[k] = ((int*)ptr)[k];
			}
			ptr += 64;
		}
		if (num > 10)
		{
			NeiliType = *(int*)ptr;
			ptr += 4;
		}
		if (num > 11)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateBonusList>(ptr, ref SkillBreakBonusDict);
		}
		if (num > 12)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref SkillBreakMaxPowerDict);
		}
		int num2 = (int)(ptr - pData);
		if (num2 > 4)
		{
			return (num2 + 3) / 4 * 4;
		}
		return num2;
	}
}
