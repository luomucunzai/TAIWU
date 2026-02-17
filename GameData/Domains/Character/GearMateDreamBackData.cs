using System;
using System.Collections.Generic;
using GameData.Domains.CombatSkill;
using GameData.Domains.Taiwu;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character
{
	// Token: 0x02000811 RID: 2065
	[SerializableGameData(IsExtensible = true, NotForDisplayModule = true, NoCopyConstructors = true)]
	public class GearMateDreamBackData : ISerializableGameData
	{
		// Token: 0x0600747D RID: 29821 RVA: 0x00443F00 File Offset: 0x00442100
		public GearMateDreamBackData()
		{
			this.MainAttributeProgress = new int[6];
			this.LifeSkillReadingProgress = new Dictionary<short, TaiwuLifeSkill>();
			this.CombatSkillReadingProgress = new Dictionary<short, TaiwuCombatSkill>();
			this.SkillBreakBonusDictObsolete = new Dictionary<short, SkillBreakBonusCollection>();
			this.CombatSkillAttainmentProgress = new int[14];
			this.LifeSkillAttainmentProgress = new int[16];
			this.FeatureIds = new List<short>();
			this.CombatSkills = new Dictionary<short, CombatSkill>();
			this.LifeSkills = new List<LifeSkillItem>();
			this.ExtraNeiliAllocationProgress = new int[4];
		}

		// Token: 0x0600747E RID: 29822 RVA: 0x00443F8C File Offset: 0x0044218C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600747F RID: 29823 RVA: 0x00443FA0 File Offset: 0x004421A0
		public int GetSerializedSize()
		{
			int totalSize = 247;
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuLifeSkill>(this.LifeSkillReadingProgress);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, TaiwuCombatSkill>(this.CombatSkillReadingProgress);
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakBonusCollection>(this.SkillBreakBonusDictObsolete);
			bool flag = this.FeatureIds != null;
			if (flag)
			{
				totalSize += 2 + 2 * this.FeatureIds.Count;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, CombatSkill>(this.CombatSkills);
			bool flag2 = this.LifeSkills != null;
			if (flag2)
			{
				totalSize += 2 + 4 * this.LifeSkills.Count;
			}
			else
			{
				totalSize += 2;
			}
			totalSize += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.GetSerializedSize<short, SkillBreakPlateBonusList>(this.SkillBreakBonusDict);
			totalSize += SerializationHelper.DictionaryOfBasicTypePair.GetSerializedSize<short, int>(this.SkillBreakMaxPowerDict);
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007480 RID: 29824 RVA: 0x00444068 File Offset: 0x00442268
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = 19;
			byte* pCurrData = pData + 2;
			Tester.Assert(this.MainAttributeProgress.Length == 6, "");
			for (int i = 0; i < 6; i++)
			{
				*(int*)(pCurrData + (IntPtr)i * 4) = this.MainAttributeProgress[i];
			}
			pCurrData += 24;
			*(int*)pCurrData = this.ConsummateLevelProgress;
			pCurrData += 4;
			*(int*)pCurrData = this.FeatureProgress;
			pCurrData += 4;
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuLifeSkill>(pCurrData, ref this.LifeSkillReadingProgress);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, TaiwuCombatSkill>(pCurrData, ref this.CombatSkillReadingProgress);
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SkillBreakBonusDictObsolete);
			Tester.Assert(this.CombatSkillAttainmentProgress.Length == 14, "");
			for (int j = 0; j < 14; j++)
			{
				*(int*)(pCurrData + (IntPtr)j * 4) = this.CombatSkillAttainmentProgress[j];
			}
			pCurrData += 56;
			Tester.Assert(this.LifeSkillAttainmentProgress.Length == 16, "");
			for (int k = 0; k < 16; k++)
			{
				*(int*)(pCurrData + (IntPtr)k * 4) = this.LifeSkillAttainmentProgress[k];
			}
			pCurrData += 64;
			*(int*)pCurrData = this.NeiliType;
			pCurrData += 4;
			pCurrData += this.MainAttributes.Serialize(pCurrData);
			*pCurrData = (byte)this.ConsummateLevel;
			pCurrData++;
			bool flag = this.FeatureIds != null;
			if (flag)
			{
				int elementsCount = this.FeatureIds.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int l = 0; l < elementsCount; l++)
				{
					*(short*)(pCurrData + (IntPtr)l * 2) = this.FeatureIds[l];
				}
				pCurrData += 2 * elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, CombatSkill>(pCurrData, ref this.CombatSkills);
			pCurrData += this.BaseCombatSkillQualifications.Serialize(pCurrData);
			bool flag2 = this.LifeSkills != null;
			if (flag2)
			{
				int elementsCount2 = this.LifeSkills.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int m = 0; m < elementsCount2; m++)
				{
					pCurrData += this.LifeSkills[m].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			pCurrData += this.BaseLifeSkillQualifications.Serialize(pCurrData);
			Tester.Assert(this.ExtraNeiliAllocationProgress.Length == 4, "");
			for (int n = 0; n < 4; n++)
			{
				*(int*)(pCurrData + (IntPtr)n * 4) = this.ExtraNeiliAllocationProgress[n];
			}
			pCurrData += 16;
			pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Serialize<short, SkillBreakPlateBonusList>(pCurrData, ref this.SkillBreakBonusDict);
			pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Serialize<short, int>(pCurrData, ref this.SkillBreakMaxPowerDict);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06007481 RID: 29825 RVA: 0x00444354 File Offset: 0x00442554
		public unsafe int Deserialize(byte* pData)
		{
			ushort fieldCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = fieldCount > 0;
			if (flag)
			{
				bool flag2 = this.MainAttributeProgress == null || this.MainAttributeProgress.Length != 6;
				if (flag2)
				{
					this.MainAttributeProgress = new int[6];
				}
				for (int i = 0; i < 6; i++)
				{
					this.MainAttributeProgress[i] = *(int*)(pCurrData + (IntPtr)i * 4);
				}
				pCurrData += 24;
			}
			bool flag3 = fieldCount > 1;
			if (flag3)
			{
				this.ConsummateLevelProgress = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag4 = fieldCount > 2;
			if (flag4)
			{
				this.FeatureProgress = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag5 = fieldCount > 3;
			if (flag5)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuLifeSkill>(pCurrData, ref this.LifeSkillReadingProgress);
			}
			bool flag6 = fieldCount > 4;
			if (flag6)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, TaiwuCombatSkill>(pCurrData, ref this.CombatSkillReadingProgress);
			}
			bool flag7 = fieldCount > 5;
			if (flag7)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakBonusCollection>(pCurrData, ref this.SkillBreakBonusDictObsolete);
			}
			bool flag8 = fieldCount > 6;
			if (flag8)
			{
				bool flag9 = this.CombatSkillAttainmentProgress == null || this.CombatSkillAttainmentProgress.Length != 14;
				if (flag9)
				{
					this.CombatSkillAttainmentProgress = new int[14];
				}
				for (int j = 0; j < 14; j++)
				{
					this.CombatSkillAttainmentProgress[j] = *(int*)(pCurrData + (IntPtr)j * 4);
				}
				pCurrData += 56;
			}
			bool flag10 = fieldCount > 7;
			if (flag10)
			{
				bool flag11 = this.LifeSkillAttainmentProgress == null || this.LifeSkillAttainmentProgress.Length != 16;
				if (flag11)
				{
					this.LifeSkillAttainmentProgress = new int[16];
				}
				for (int k = 0; k < 16; k++)
				{
					this.LifeSkillAttainmentProgress[k] = *(int*)(pCurrData + (IntPtr)k * 4);
				}
				pCurrData += 64;
			}
			bool flag12 = fieldCount > 8;
			if (flag12)
			{
				this.NeiliType = *(int*)pCurrData;
				pCurrData += 4;
			}
			bool flag13 = fieldCount > 9;
			if (flag13)
			{
				pCurrData += this.MainAttributes.Deserialize(pCurrData);
			}
			bool flag14 = fieldCount > 10;
			if (flag14)
			{
				this.ConsummateLevel = *(sbyte*)pCurrData;
				pCurrData++;
			}
			bool flag15 = fieldCount > 11;
			if (flag15)
			{
				ushort elementsCount = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag16 = elementsCount > 0;
				if (flag16)
				{
					bool flag17 = this.FeatureIds == null;
					if (flag17)
					{
						this.FeatureIds = new List<short>((int)elementsCount);
					}
					else
					{
						this.FeatureIds.Clear();
					}
					for (int l = 0; l < (int)elementsCount; l++)
					{
						this.FeatureIds.Add(*(short*)(pCurrData + (IntPtr)l * 2));
					}
					pCurrData += 2 * elementsCount;
				}
				else
				{
					List<short> featureIds = this.FeatureIds;
					if (featureIds != null)
					{
						featureIds.Clear();
					}
				}
			}
			bool flag18 = fieldCount > 12;
			if (flag18)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, CombatSkill>(pCurrData, ref this.CombatSkills);
			}
			bool flag19 = fieldCount > 13;
			if (flag19)
			{
				pCurrData += this.BaseCombatSkillQualifications.Deserialize(pCurrData);
			}
			bool flag20 = fieldCount > 14;
			if (flag20)
			{
				ushort elementsCount2 = *(ushort*)pCurrData;
				pCurrData += 2;
				bool flag21 = elementsCount2 > 0;
				if (flag21)
				{
					bool flag22 = this.LifeSkills == null;
					if (flag22)
					{
						this.LifeSkills = new List<LifeSkillItem>((int)elementsCount2);
					}
					else
					{
						this.LifeSkills.Clear();
					}
					for (int m = 0; m < (int)elementsCount2; m++)
					{
						LifeSkillItem element = default(LifeSkillItem);
						pCurrData += element.Deserialize(pCurrData);
						this.LifeSkills.Add(element);
					}
				}
				else
				{
					List<LifeSkillItem> lifeSkills = this.LifeSkills;
					if (lifeSkills != null)
					{
						lifeSkills.Clear();
					}
				}
			}
			bool flag23 = fieldCount > 15;
			if (flag23)
			{
				pCurrData += this.BaseLifeSkillQualifications.Deserialize(pCurrData);
			}
			bool flag24 = fieldCount > 16;
			if (flag24)
			{
				bool flag25 = this.ExtraNeiliAllocationProgress == null || this.ExtraNeiliAllocationProgress.Length != 4;
				if (flag25)
				{
					this.ExtraNeiliAllocationProgress = new int[4];
				}
				for (int n = 0; n < 4; n++)
				{
					this.ExtraNeiliAllocationProgress[n] = *(int*)(pCurrData + (IntPtr)n * 4);
				}
				pCurrData += 16;
			}
			bool flag26 = fieldCount > 17;
			if (flag26)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypeCustomTypePair.Deserialize<short, SkillBreakPlateBonusList>(pCurrData, ref this.SkillBreakBonusDict);
			}
			bool flag27 = fieldCount > 18;
			if (flag27)
			{
				pCurrData += SerializationHelper.DictionaryOfBasicTypePair.Deserialize<short, int>(pCurrData, ref this.SkillBreakMaxPowerDict);
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001EC5 RID: 7877
		[SerializableGameDataField(ArrayElementsCount = 6)]
		public int[] MainAttributeProgress;

		// Token: 0x04001EC6 RID: 7878
		[SerializableGameDataField]
		public int ConsummateLevelProgress;

		// Token: 0x04001EC7 RID: 7879
		[SerializableGameDataField]
		public int FeatureProgress;

		// Token: 0x04001EC8 RID: 7880
		[SerializableGameDataField]
		public Dictionary<short, TaiwuLifeSkill> LifeSkillReadingProgress;

		// Token: 0x04001EC9 RID: 7881
		[SerializableGameDataField]
		public Dictionary<short, TaiwuCombatSkill> CombatSkillReadingProgress;

		// Token: 0x04001ECA RID: 7882
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakPlateBonusList> SkillBreakBonusDict;

		// Token: 0x04001ECB RID: 7883
		[SerializableGameDataField]
		public Dictionary<short, int> SkillBreakMaxPowerDict;

		// Token: 0x04001ECC RID: 7884
		[SerializableGameDataField]
		public Dictionary<short, SkillBreakBonusCollection> SkillBreakBonusDictObsolete;

		// Token: 0x04001ECD RID: 7885
		[SerializableGameDataField(ArrayElementsCount = 14)]
		public int[] CombatSkillAttainmentProgress;

		// Token: 0x04001ECE RID: 7886
		[SerializableGameDataField(ArrayElementsCount = 16)]
		public int[] LifeSkillAttainmentProgress;

		// Token: 0x04001ECF RID: 7887
		[SerializableGameDataField]
		public int NeiliType;

		// Token: 0x04001ED0 RID: 7888
		[SerializableGameDataField]
		public MainAttributes MainAttributes;

		// Token: 0x04001ED1 RID: 7889
		[SerializableGameDataField]
		public sbyte ConsummateLevel;

		// Token: 0x04001ED2 RID: 7890
		[SerializableGameDataField]
		public List<short> FeatureIds;

		// Token: 0x04001ED3 RID: 7891
		[SerializableGameDataField]
		public Dictionary<short, CombatSkill> CombatSkills;

		// Token: 0x04001ED4 RID: 7892
		[SerializableGameDataField]
		public CombatSkillShorts BaseCombatSkillQualifications;

		// Token: 0x04001ED5 RID: 7893
		[SerializableGameDataField]
		public List<LifeSkillItem> LifeSkills;

		// Token: 0x04001ED6 RID: 7894
		[SerializableGameDataField]
		public LifeSkillShorts BaseLifeSkillQualifications;

		// Token: 0x04001ED7 RID: 7895
		[SerializableGameDataField(ArrayElementsCount = 4)]
		public int[] ExtraNeiliAllocationProgress;

		// Token: 0x02000C05 RID: 3077
		private static class FieldIds
		{
			// Token: 0x040033F3 RID: 13299
			public const ushort MainAttributeProgress = 0;

			// Token: 0x040033F4 RID: 13300
			public const ushort ConsummateLevelProgress = 1;

			// Token: 0x040033F5 RID: 13301
			public const ushort FeatureProgress = 2;

			// Token: 0x040033F6 RID: 13302
			public const ushort LifeSkillReadingProgress = 3;

			// Token: 0x040033F7 RID: 13303
			public const ushort CombatSkillReadingProgress = 4;

			// Token: 0x040033F8 RID: 13304
			public const ushort SkillBreakBonusDictObsolete = 5;

			// Token: 0x040033F9 RID: 13305
			public const ushort CombatSkillAttainmentProgress = 6;

			// Token: 0x040033FA RID: 13306
			public const ushort LifeSkillAttainmentProgress = 7;

			// Token: 0x040033FB RID: 13307
			public const ushort NeiliType = 8;

			// Token: 0x040033FC RID: 13308
			public const ushort MainAttributes = 9;

			// Token: 0x040033FD RID: 13309
			public const ushort ConsummateLevel = 10;

			// Token: 0x040033FE RID: 13310
			public const ushort FeatureIds = 11;

			// Token: 0x040033FF RID: 13311
			public const ushort CombatSkills = 12;

			// Token: 0x04003400 RID: 13312
			public const ushort BaseCombatSkillQualifications = 13;

			// Token: 0x04003401 RID: 13313
			public const ushort LifeSkills = 14;

			// Token: 0x04003402 RID: 13314
			public const ushort BaseLifeSkillQualifications = 15;

			// Token: 0x04003403 RID: 13315
			public const ushort ExtraNeiliAllocationProgress = 16;

			// Token: 0x04003404 RID: 13316
			public const ushort SkillBreakBonusDict = 17;

			// Token: 0x04003405 RID: 13317
			public const ushort SkillBreakMaxPowerDict = 18;

			// Token: 0x04003406 RID: 13318
			public const ushort Count = 19;

			// Token: 0x04003407 RID: 13319
			public static readonly string[] FieldId2FieldName = new string[]
			{
				"MainAttributeProgress",
				"ConsummateLevelProgress",
				"FeatureProgress",
				"LifeSkillReadingProgress",
				"CombatSkillReadingProgress",
				"SkillBreakBonusDictObsolete",
				"CombatSkillAttainmentProgress",
				"LifeSkillAttainmentProgress",
				"NeiliType",
				"MainAttributes",
				"ConsummateLevel",
				"FeatureIds",
				"CombatSkills",
				"BaseCombatSkillQualifications",
				"LifeSkills",
				"BaseLifeSkillQualifications",
				"ExtraNeiliAllocationProgress",
				"SkillBreakBonusDict",
				"SkillBreakMaxPowerDict"
			};
		}
	}
}
