using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character;

[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
public class GearMateDreamBackData : ISerializableGameData
{
	private static class FieldIds
	{
		public const ushort MainAttributeProgress = 0;

		public const ushort ConsummateLevelProgress = 1;

		public const ushort FeatureProgress = 2;

		public const ushort LifeSkillReadingProgress = 3;

		public const ushort CombatSkillReadingProgress = 4;

		public const ushort SkillBreakBonusDictObsolete = 5;

		public const ushort CombatSkillAttainmentProgress = 6;

		public const ushort LifeSkillAttainmentProgress = 7;

		public const ushort NeiliType = 8;

		public const ushort MainAttributes = 9;

		public const ushort ConsummateLevel = 10;

		public const ushort FeatureIds = 11;

		public const ushort CombatSkills = 12;

		public const ushort BaseCombatSkillQualifications = 13;

		public const ushort LifeSkills = 14;

		public const ushort BaseLifeSkillQualifications = 15;

		public const ushort ExtraNeiliAllocationProgress = 16;

		public const ushort SkillBreakBonusDict = 17;

		public const ushort SkillBreakMaxPowerDict = 18;

		public const ushort Count = 19;

		public static readonly string[] FieldId2FieldName = new string[19]
		{
			"MainAttributeProgress", "ConsummateLevelProgress", "FeatureProgress", "LifeSkillReadingProgress", "CombatSkillReadingProgress", "SkillBreakBonusDictObsolete", "CombatSkillAttainmentProgress", "LifeSkillAttainmentProgress", "NeiliType", "MainAttributes",
			"ConsummateLevel", "FeatureIds", "CombatSkills", "BaseCombatSkillQualifications", "LifeSkills", "BaseLifeSkillQualifications", "ExtraNeiliAllocationProgress", "SkillBreakBonusDict", "SkillBreakMaxPowerDict"
		};
	}

	[SerializableGameDataField(ArrayElementsCount = 6)]
	public int[] MainAttributeProgress;

	[SerializableGameDataField]
	public int ConsummateLevelProgress;

	[SerializableGameDataField]
	public int FeatureProgress;

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

	[SerializableGameDataField]
	public MainAttributes MainAttributes;

	[SerializableGameDataField]
	public sbyte ConsummateLevel;

	[SerializableGameDataField]
	public List<short> FeatureIds;

	[SerializableGameDataField]
	public Dictionary<short, GameData.Domains.CombatSkill.CombatSkill> CombatSkills;

	[SerializableGameDataField]
	public CombatSkillShorts BaseCombatSkillQualifications;

	[SerializableGameDataField]
	public List<LifeSkillItem> LifeSkills;

	[SerializableGameDataField]
	public LifeSkillShorts BaseLifeSkillQualifications;

	[SerializableGameDataField(ArrayElementsCount = 4)]
	public int[] ExtraNeiliAllocationProgress;

	public GearMateDreamBackData()
	{
		MainAttributeProgress = new int[6];
		LifeSkillReadingProgress = new Dictionary<short, TaiwuLifeSkill>();
		CombatSkillReadingProgress = new Dictionary<short, TaiwuCombatSkill>();
		SkillBreakBonusDictObsolete = new Dictionary<short, SkillBreakBonusCollection>();
		CombatSkillAttainmentProgress = new int[14];
		LifeSkillAttainmentProgress = new int[16];
		FeatureIds = new List<short>();
		CombatSkills = new Dictionary<short, GameData.Domains.CombatSkill.CombatSkill>();
		LifeSkills = new List<LifeSkillItem>();
		ExtraNeiliAllocationProgress = new int[4];
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 247;
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(LifeSkillReadingProgress);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(CombatSkillReadingProgress);
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(SkillBreakBonusDictObsolete);
		num = ((FeatureIds == null) ? (num + 2) : (num + (2 + 2 * FeatureIds.Count)));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, GameData.Domains.CombatSkill.CombatSkill>(CombatSkills);
		num = ((LifeSkills == null) ? (num + 2) : (num + (2 + 4 * LifeSkills.Count)));
		num += DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateBonusList>(SkillBreakBonusDict);
		num += DictionaryOfBasicTypePair.GetSerializedSize<short, int>((IReadOnlyDictionary<short, int>)SkillBreakMaxPowerDict);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = 19;
		ptr += 2;
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
		ptr += MainAttributes.Serialize(ptr);
		*ptr = (byte)ConsummateLevel;
		ptr++;
		if (FeatureIds != null)
		{
			int count = FeatureIds.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int l = 0; l < count; l++)
			{
				((short*)ptr)[l] = FeatureIds[l];
			}
			ptr += 2 * count;
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, GameData.Domains.CombatSkill.CombatSkill>(ptr, ref CombatSkills);
		ptr += BaseCombatSkillQualifications.Serialize(ptr);
		if (LifeSkills != null)
		{
			int count2 = LifeSkills.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int m = 0; m < count2; m++)
			{
				ptr += LifeSkills[m].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		ptr += BaseLifeSkillQualifications.Serialize(ptr);
		Tester.Assert(ExtraNeiliAllocationProgress.Length == 4);
		for (int n = 0; n < 4; n++)
		{
			((int*)ptr)[n] = ExtraNeiliAllocationProgress[n];
		}
		ptr += 16;
		ptr += DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateBonusList>(ptr, ref SkillBreakBonusDict);
		ptr += DictionaryOfBasicTypePair.Serialize<short, int>(ptr, ref SkillBreakMaxPowerDict);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
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
		if (num > 1)
		{
			ConsummateLevelProgress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 2)
		{
			FeatureProgress = *(int*)ptr;
			ptr += 4;
		}
		if (num > 3)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(ptr, ref LifeSkillReadingProgress);
		}
		if (num > 4)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(ptr, ref CombatSkillReadingProgress);
		}
		if (num > 5)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(ptr, ref SkillBreakBonusDictObsolete);
		}
		if (num > 6)
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
		if (num > 7)
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
		if (num > 8)
		{
			NeiliType = *(int*)ptr;
			ptr += 4;
		}
		if (num > 9)
		{
			ptr += MainAttributes.Deserialize(ptr);
		}
		if (num > 10)
		{
			ConsummateLevel = (sbyte)(*ptr);
			ptr++;
		}
		if (num > 11)
		{
			ushort num2 = *(ushort*)ptr;
			ptr += 2;
			if (num2 > 0)
			{
				if (FeatureIds == null)
				{
					FeatureIds = new List<short>(num2);
				}
				else
				{
					FeatureIds.Clear();
				}
				for (int l = 0; l < num2; l++)
				{
					FeatureIds.Add(((short*)ptr)[l]);
				}
				ptr += 2 * num2;
			}
			else
			{
				FeatureIds?.Clear();
			}
		}
		if (num > 12)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, GameData.Domains.CombatSkill.CombatSkill>(ptr, ref CombatSkills);
		}
		if (num > 13)
		{
			ptr += BaseCombatSkillQualifications.Deserialize(ptr);
		}
		if (num > 14)
		{
			ushort num3 = *(ushort*)ptr;
			ptr += 2;
			if (num3 > 0)
			{
				if (LifeSkills == null)
				{
					LifeSkills = new List<LifeSkillItem>(num3);
				}
				else
				{
					LifeSkills.Clear();
				}
				for (int m = 0; m < num3; m++)
				{
					LifeSkillItem item = default(LifeSkillItem);
					ptr += item.Deserialize(ptr);
					LifeSkills.Add(item);
				}
			}
			else
			{
				LifeSkills?.Clear();
			}
		}
		if (num > 15)
		{
			ptr += BaseLifeSkillQualifications.Deserialize(ptr);
		}
		if (num > 16)
		{
			if (ExtraNeiliAllocationProgress == null || ExtraNeiliAllocationProgress.Length != 4)
			{
				ExtraNeiliAllocationProgress = new int[4];
			}
			for (int n = 0; n < 4; n++)
			{
				ExtraNeiliAllocationProgress[n] = ((int*)ptr)[n];
			}
			ptr += 16;
		}
		if (num > 17)
		{
			ptr += DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateBonusList>(ptr, ref SkillBreakBonusDict);
		}
		if (num > 18)
		{
			ptr += DictionaryOfBasicTypePair.Deserialize<short, int>(ptr, ref SkillBreakMaxPowerDict);
		}
		int num4 = (int)(ptr - pData);
		return (num4 <= 4) ? num4 : ((num4 + 3) / 4 * 4);
	}
}
