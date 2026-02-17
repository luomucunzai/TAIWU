using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.SpecialEffect;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.World.SectMainStory
{
	// Token: 0x02000037 RID: 55
	[SerializableGameData(IsExtensible = true)]
	public class SectShaolinDemonSlayerData : ISerializableGameData
	{
		// Token: 0x06000F0C RID: 3852 RVA: 0x000FAAF4 File Offset: 0x000F8CF4
		public static IntList GenerateRestricts(IRandomSource random, int demonId, int totalPower)
		{
			IntList result = IntList.Create();
			SectShaolinDemonSlayerData.RestrictCacheGroups.Clear();
			int minRestrictPower = totalPower / 3 + ((totalPower % 3 != 0) ? 1 : 0);
			for (int i = 0; i < 3; i++)
			{
				bool flag = totalPower < minRestrictPower;
				if (flag)
				{
					minRestrictPower = totalPower;
					totalPower = minRestrictPower + 2;
				}
				SectShaolinDemonSlayerData.RestrictCacheIndexes.Clear();
				SectShaolinDemonSlayerData.RestrictCacheWeights.Clear();
				foreach (DemonSlayerTrialRestrictItem restrict in ((IEnumerable<DemonSlayerTrialRestrictItem>)DemonSlayerTrialRestrict.Instance))
				{
					bool flag2 = restrict.MutexDemonId.Contains(demonId);
					if (!flag2)
					{
						bool flag3 = SectShaolinDemonSlayerData.RestrictCacheGroups.Contains(restrict.MutexGroupId);
						if (!flag3)
						{
							bool flag4 = restrict.Power > totalPower || restrict.Power < minRestrictPower;
							if (!flag4)
							{
								bool flag5 = restrict.Weight <= 0;
								if (!flag5)
								{
									SectShaolinDemonSlayerData.RestrictCacheIndexes.Add(restrict.TemplateId);
									SectShaolinDemonSlayerData.RestrictCacheWeights.Add(restrict.Weight);
								}
							}
						}
					}
				}
				bool flag6 = SectShaolinDemonSlayerData.RestrictCacheIndexes.Count == 0;
				if (!flag6)
				{
					int restrictIndex = RandomUtils.GetRandomIndex(SectShaolinDemonSlayerData.RestrictCacheWeights, random);
					int restrictId = SectShaolinDemonSlayerData.RestrictCacheIndexes[restrictIndex];
					result.Items.Add(restrictId);
					DemonSlayerTrialRestrictItem restrictConfig = DemonSlayerTrialRestrict.Instance[restrictId];
					SectShaolinDemonSlayerData.RestrictCacheGroups.Add(restrictConfig.MutexGroupId);
					totalPower -= restrictConfig.Power;
					bool flag7 = totalPower <= 0;
					if (flag7)
					{
						break;
					}
				}
			}
			return result;
		}

		// Token: 0x1700001A RID: 26
		// (get) Token: 0x06000F0D RID: 3853 RVA: 0x000FACB0 File Offset: 0x000F8EB0
		public bool Trialing
		{
			get
			{
				List<int> trialingDemons = this._trialingDemons;
				return trialingDemons != null && trialingDemons.Count > 0 && this._trialingDemons.Count - this._trialingLevel * 2 >= 2;
			}
		}

		// Token: 0x1700001B RID: 27
		// (get) Token: 0x06000F0E RID: 3854 RVA: 0x000FACF0 File Offset: 0x000F8EF0
		public DemonSlayerTrialLevelItem TrialingLevel
		{
			get
			{
				bool flag = !this.Trialing;
				DemonSlayerTrialLevelItem result;
				if (flag)
				{
					result = null;
				}
				else
				{
					int templateId = MathUtils.Clamp(this._trialingLevel, 0, DemonSlayerTrialLevel.Instance.Count - 1);
					result = DemonSlayerTrialLevel.Instance[templateId];
				}
				return result;
			}
		}

		// Token: 0x06000F0F RID: 3855 RVA: 0x000FAD38 File Offset: 0x000F8F38
		public int GetRegenerateRestrictCount()
		{
			return this.Trialing ? this._trailingRegenerateRestrictCount[this._trialingLevel] : 0;
		}

		// Token: 0x06000F10 RID: 3856 RVA: 0x000FAD68 File Offset: 0x000F8F68
		public DemonSlayerTrialItem GetTrialingDemon(int index)
		{
			bool flag = index < 0 || index >= 2;
			bool flag2 = flag || !this.Trialing;
			DemonSlayerTrialItem result;
			if (flag2)
			{
				result = null;
			}
			else
			{
				int baseIndex = this._trialingLevel * 2;
				int templateId = this._trialingDemons[baseIndex + index];
				result = DemonSlayerTrial.Instance[templateId];
			}
			return result;
		}

		// Token: 0x06000F11 RID: 3857 RVA: 0x000FADC7 File Offset: 0x000F8FC7
		public IEnumerable<DemonSlayerTrialRestrictItem> GetTrialingRestricts(int index)
		{
			bool flag = index < 0 || index >= 2;
			bool flag2 = flag || !this.Trialing;
			if (flag2)
			{
				yield break;
			}
			int baseIndex = this._trialingLevel * 2;
			IntList restricts = this._trailingRestricts[baseIndex + index];
			List<int> items = restricts.Items;
			bool flag3 = items == null || items.Count <= 0;
			if (flag3)
			{
				yield break;
			}
			foreach (int restrictId in restricts.Items)
			{
				yield return DemonSlayerTrialRestrict.Instance[restrictId];
			}
			List<int>.Enumerator enumerator = default(List<int>.Enumerator);
			yield break;
			yield break;
		}

		// Token: 0x06000F12 RID: 3858 RVA: 0x000FADE0 File Offset: 0x000F8FE0
		public bool IsDemonDefeated(int templateId)
		{
			bool flag = templateId < 0 || templateId >= DemonSlayerTrial.Instance.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Tester.Assert(templateId < 32, "templateId < 32");
				result = this._demonFlags0[templateId];
			}
			return result;
		}

		// Token: 0x06000F13 RID: 3859 RVA: 0x000FAE38 File Offset: 0x000F9038
		public bool GenerateDemons(IRandomSource random)
		{
			bool trialing = this.Trialing;
			bool result;
			if (trialing)
			{
				result = false;
			}
			else
			{
				if (this._trialingDemons == null)
				{
					this._trialingDemons = new List<int>();
				}
				this._trialingDemons.Clear();
				foreach (DemonSlayerTrialItem config in ((IEnumerable<DemonSlayerTrialItem>)DemonSlayerTrial.Instance))
				{
					this._trialingDemons.Add(config.TemplateId);
				}
				CollectionUtils.Shuffle<int>(random, this._trialingDemons);
				if (this._trailingRestricts == null)
				{
					this._trailingRestricts = new List<IntList>();
				}
				this._trailingRestricts.Clear();
				for (int i = 0; i < this._trialingDemons.Count; i++)
				{
					int demonId = this._trialingDemons[i];
					int levelId = i / 2;
					DemonSlayerTrialLevelItem level = DemonSlayerTrialLevel.Instance[levelId];
					this._trailingRestricts.Add(SectShaolinDemonSlayerData.GenerateRestricts(random, demonId, level.TotalPower));
				}
				if (this._trailingRegenerateRestrictCount == null)
				{
					this._trailingRegenerateRestrictCount = new List<int>();
				}
				this._trailingRegenerateRestrictCount.Clear();
				foreach (DemonSlayerTrialLevelItem level2 in ((IEnumerable<DemonSlayerTrialLevelItem>)DemonSlayerTrialLevel.Instance))
				{
					this._trailingRegenerateRestrictCount.Add(level2.RestrictRandomCount);
				}
				this._trialingLevel = 0;
				result = true;
			}
			return result;
		}

		// Token: 0x06000F14 RID: 3860 RVA: 0x000FAFC8 File Offset: 0x000F91C8
		public bool ClearDemons()
		{
			List<int> list = this._trialingDemons;
			bool flag;
			if (list == null || list.Count <= 0)
			{
				List<IntList> trailingRestricts = this._trailingRestricts;
				if (trailingRestricts == null || trailingRestricts.Count <= 0)
				{
					list = this._trailingRegenerateRestrictCount;
					if (list == null || list.Count <= 0)
					{
						flag = (this._trialingLevel == 0);
						goto IL_46;
					}
				}
			}
			flag = false;
			IL_46:
			bool flag2 = flag;
			bool result;
			if (flag2)
			{
				result = false;
			}
			else
			{
				List<int> trialingDemons = this._trialingDemons;
				if (trialingDemons != null)
				{
					trialingDemons.Clear();
				}
				List<IntList> trailingRestricts2 = this._trailingRestricts;
				if (trailingRestricts2 != null)
				{
					trailingRestricts2.Clear();
				}
				List<int> trailingRegenerateRestrictCount = this._trailingRegenerateRestrictCount;
				if (trailingRegenerateRestrictCount != null)
				{
					trailingRegenerateRestrictCount.Clear();
				}
				this._trialingLevel = 0;
				result = true;
			}
			return result;
		}

		// Token: 0x06000F15 RID: 3861 RVA: 0x000FB068 File Offset: 0x000F9268
		public bool ReGenerateRestricts(IRandomSource random)
		{
			bool flag = this.GetRegenerateRestrictCount() <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				List<int> trailingRegenerateRestrictCount = this._trailingRegenerateRestrictCount;
				int trialingLevel = this._trialingLevel;
				trailingRegenerateRestrictCount[trialingLevel]--;
				int baseIndex = this._trialingLevel * 2;
				for (int i = 0; i < 2; i++)
				{
					int templateId = this._trialingDemons[baseIndex + i];
					this._trailingRestricts[baseIndex + i] = SectShaolinDemonSlayerData.GenerateRestricts(random, templateId, this.TrialingLevel.TotalPower);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000F16 RID: 3862 RVA: 0x000FB108 File Offset: 0x000F9308
		public bool ToNextLevel()
		{
			bool flag = !this.Trialing;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this._trialingLevel++;
				bool flag2 = !this.Trialing;
				if (flag2)
				{
					this.ClearDemons();
				}
				result = true;
			}
			return result;
		}

		// Token: 0x06000F17 RID: 3863 RVA: 0x000FB150 File Offset: 0x000F9350
		public bool MarkDemonAsDefeated(int templateId)
		{
			bool flag = templateId < 0 || templateId >= DemonSlayerTrial.Instance.Count;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				Tester.Assert(templateId < 32, "templateId < 32");
				BoolArray32 array = this._demonFlags0;
				array[templateId] = true;
				this._demonFlags0 = array;
				result = true;
			}
			return result;
		}

		// Token: 0x06000F18 RID: 3864 RVA: 0x000FB1B4 File Offset: 0x000F93B4
		public SectShaolinDemonSlayerData()
		{
		}

		// Token: 0x06000F19 RID: 3865 RVA: 0x000FB1C0 File Offset: 0x000F93C0
		public SectShaolinDemonSlayerData(SectShaolinDemonSlayerData other)
		{
			this._demonFlags0 = other._demonFlags0;
			this._trialingDemons = ((other._trialingDemons == null) ? null : new List<int>(other._trialingDemons));
			this._trialingLevel = other._trialingLevel;
			bool flag = other._trailingRestricts != null;
			if (flag)
			{
				List<IntList> item = other._trailingRestricts;
				int elementsCount = item.Count;
				this._trailingRestricts = new List<IntList>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this._trailingRestricts.Add(new IntList(item[i]));
				}
			}
			else
			{
				this._trailingRestricts = null;
			}
			this._trailingRegenerateRestrictCount = ((other._trailingRegenerateRestrictCount == null) ? null : new List<int>(other._trailingRegenerateRestrictCount));
		}

		// Token: 0x06000F1A RID: 3866 RVA: 0x000FB284 File Offset: 0x000F9484
		public void Assign(SectShaolinDemonSlayerData other)
		{
			this._demonFlags0 = other._demonFlags0;
			this._trialingDemons = ((other._trialingDemons == null) ? null : new List<int>(other._trialingDemons));
			this._trialingLevel = other._trialingLevel;
			bool flag = other._trailingRestricts != null;
			if (flag)
			{
				List<IntList> item = other._trailingRestricts;
				int elementsCount = item.Count;
				this._trailingRestricts = new List<IntList>(elementsCount);
				for (int i = 0; i < elementsCount; i++)
				{
					this._trailingRestricts.Add(new IntList(item[i]));
				}
			}
			else
			{
				this._trailingRestricts = null;
			}
			this._trailingRegenerateRestrictCount = ((other._trailingRegenerateRestrictCount == null) ? null : new List<int>(other._trailingRegenerateRestrictCount));
		}

		// Token: 0x06000F1B RID: 3867 RVA: 0x000FB340 File Offset: 0x000F9540
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06000F1C RID: 3868 RVA: 0x000FB354 File Offset: 0x000F9554
		public int GetSerializedSize()
		{
			int totalSize = 8;
			bool flag = this._trialingDemons != null;
			if (flag)
			{
				totalSize += 2 + 4 * this._trialingDemons.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this._trailingRestricts != null;
			if (flag2)
			{
				totalSize += 2;
				int elementsCount = this._trailingRestricts.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this._trailingRestricts[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag3 = this._trailingRegenerateRestrictCount != null;
			if (flag3)
			{
				totalSize += 2 + 4 * this._trailingRegenerateRestrictCount.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F1D RID: 3869 RVA: 0x000FB41C File Offset: 0x000F961C
		public unsafe int Serialize(byte* pData)
		{
			*(int*)pData = (int)this._demonFlags0;
			byte* pCurrData = pData + 4;
			bool flag = this._trialingDemons != null;
			if (flag)
			{
				int elementsCount = this._trialingDemons.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount);
				pCurrData += 2;
				for (int i = 0; i < elementsCount; i++)
				{
					*(int*)(pCurrData + (IntPtr)i * 4) = this._trialingDemons[i];
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this._trialingLevel;
			pCurrData += 4;
			bool flag2 = this._trailingRestricts != null;
			if (flag2)
			{
				int elementsCount2 = this._trailingRestricts.Count;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					int subDataSize = this._trailingRestricts[j].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this._trailingRegenerateRestrictCount != null;
			if (flag3)
			{
				int elementsCount3 = this._trailingRegenerateRestrictCount.Count;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					*(int*)(pCurrData + (IntPtr)k * 4) = this._trailingRegenerateRestrictCount[k];
				}
				pCurrData += 4 * elementsCount3;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06000F1E RID: 3870 RVA: 0x000FB5F0 File Offset: 0x000F97F0
		public unsafe int Deserialize(byte* pData)
		{
			this._demonFlags0 = *(uint*)pData;
			byte* pCurrData = pData + 4;
			ushort elementsCount = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this._trialingDemons == null;
				if (flag2)
				{
					this._trialingDemons = new List<int>((int)elementsCount);
				}
				else
				{
					this._trialingDemons.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this._trialingDemons.Add(*(int*)(pCurrData + (IntPtr)i * 4));
				}
				pCurrData += 4 * elementsCount;
			}
			else
			{
				List<int> trialingDemons = this._trialingDemons;
				if (trialingDemons != null)
				{
					trialingDemons.Clear();
				}
			}
			this._trialingLevel = *(int*)pCurrData;
			pCurrData += 4;
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this._trailingRestricts == null;
				if (flag4)
				{
					this._trailingRestricts = new List<IntList>((int)elementsCount2);
				}
				else
				{
					this._trailingRestricts.Clear();
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					IntList element = default(IntList);
					pCurrData += element.Deserialize(pCurrData);
					this._trailingRestricts.Add(element);
				}
			}
			else
			{
				List<IntList> trailingRestricts = this._trailingRestricts;
				if (trailingRestricts != null)
				{
					trailingRestricts.Clear();
				}
			}
			ushort elementsCount3 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag5 = elementsCount3 > 0;
			if (flag5)
			{
				bool flag6 = this._trailingRegenerateRestrictCount == null;
				if (flag6)
				{
					this._trailingRegenerateRestrictCount = new List<int>((int)elementsCount3);
				}
				else
				{
					this._trailingRegenerateRestrictCount.Clear();
				}
				for (int k = 0; k < (int)elementsCount3; k++)
				{
					this._trailingRegenerateRestrictCount.Add(*(int*)(pCurrData + (IntPtr)k * 4));
				}
				pCurrData += 4 * elementsCount3;
			}
			else
			{
				List<int> trailingRegenerateRestrictCount = this._trailingRegenerateRestrictCount;
				if (trailingRegenerateRestrictCount != null)
				{
					trailingRegenerateRestrictCount.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400020B RID: 523
		public const int DemonPerLevel = 2;

		// Token: 0x0400020C RID: 524
		public const int MaxRestrictCount = 3;

		// Token: 0x0400020D RID: 525
		private static readonly List<int> RestrictCacheIndexes = new List<int>();

		// Token: 0x0400020E RID: 526
		private static readonly List<short> RestrictCacheWeights = new List<short>();

		// Token: 0x0400020F RID: 527
		private static readonly HashSet<int> RestrictCacheGroups = new HashSet<int>();

		// Token: 0x04000210 RID: 528
		public List<SpecialEffectBase> TrialingRestrictEffects;

		// Token: 0x04000211 RID: 529
		[SerializableGameDataField(FieldIndex = 0)]
		private uint _demonFlags0;

		// Token: 0x04000212 RID: 530
		[SerializableGameDataField(FieldIndex = 1)]
		private List<int> _trialingDemons;

		// Token: 0x04000213 RID: 531
		[SerializableGameDataField(FieldIndex = 2)]
		private int _trialingLevel;

		// Token: 0x04000214 RID: 532
		[SerializableGameDataField(FieldIndex = 3)]
		private List<IntList> _trailingRestricts;

		// Token: 0x04000215 RID: 533
		[SerializableGameDataField(FieldIndex = 4)]
		private List<int> _trailingRegenerateRestrictCount;
	}
}
