using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C0 RID: 1728
	public class DefeatMarkCollection : ISerializableGameData
	{
		// Token: 0x170003F1 RID: 1009
		// (get) Token: 0x06006687 RID: 26247 RVA: 0x003AC5A4 File Offset: 0x003AA7A4
		private static short QiDisorderFirstExtra
		{
			get
			{
				return GlobalConfig.Instance.DefeatMarkQiDisorderFirstExtra;
			}
		}

		// Token: 0x170003F2 RID: 1010
		// (get) Token: 0x06006688 RID: 26248 RVA: 0x003AC5B0 File Offset: 0x003AA7B0
		private static short QiDisorderThreshold
		{
			get
			{
				return GlobalConfig.Instance.DefeatMarkQiDisorderThreshold;
			}
		}

		// Token: 0x06006689 RID: 26249 RVA: 0x003AC5BC File Offset: 0x003AA7BC
		public unsafe IEnumerable<DefeatMarkKey> GetAllKeys(short oldDisorderOfQi, PoisonInts oldPoisons, Injuries oldInjuries)
		{
			int num;
			for (int i = 0; i < this.DieMarkList.Count; i = num + 1)
			{
				yield return EMarkType.Die;
				num = i;
			}
			for (int j = 0; j < (int)this.HealthMarkCount; j = num + 1)
			{
				yield return EMarkType.Health;
				num = j;
			}
			for (int k = 0; k < (int)this.WugMarkCount; k = num + 1)
			{
				yield return EMarkType.Wug;
				num = k;
			}
			for (int l = 0; l < (int)this.StateMarkCount; l = num + 1)
			{
				yield return EMarkType.State;
				num = l;
			}
			for (int m = 0; m < (int)this.NeiliAllocationMarkCount.Item1; m = num + 1)
			{
				yield return EMarkType.NeiliAllocation;
				num = m;
			}
			for (int n = 0; n < (int)this.NeiliAllocationMarkCount.Item2; n = num + 1)
			{
				yield return new ValueTuple<EMarkType, int>(EMarkType.NeiliAllocation, 1);
				num = n;
			}
			sbyte oldMarkCountQiDisorder = DefeatMarkCollection.CalcQiDisorderMarkCount((int)oldDisorderOfQi);
			for (int i2 = 0; i2 < (int)this.QiDisorderMarkCount; i2 = num + 1)
			{
				yield return new ValueTuple<EMarkType, int, int>(EMarkType.QiDisorder, 0, (i2 < (int)oldMarkCountQiDisorder) ? 1 : 0);
				num = i2;
			}
			sbyte b;
			for (sbyte order = 0; order < 6; order = b + 1)
			{
				sbyte type = PoisonType.GetTypeBySortingOrder(order);
				int oldMarkCount = (int)PoisonsAndLevels.CalcPoisonedLevel(*oldPoisons[(int)type]);
				for (int i3 = 0; i3 < (int)this.PoisonMarkList[(int)type]; i3 = num + 1)
				{
					yield return new ValueTuple<EMarkType, int, int>(EMarkType.Poison, (int)type, (i3 < oldMarkCount) ? 1 : 0);
					num = i3;
				}
				b = order;
			}
			for (sbyte part = 0; part < 7; part = b + 1)
			{
				sbyte oldMarkCount2 = oldInjuries.Get(part, false);
				for (int i4 = 0; i4 < (int)this.OuterInjuryMarkList[(int)part]; i4 = num + 1)
				{
					yield return new ValueTuple<EMarkType, int, int>(EMarkType.Outer, (int)part, (i4 < (int)oldMarkCount2) ? 1 : 0);
					num = i4;
				}
				b = part;
			}
			for (sbyte part2 = 0; part2 < 7; part2 = b + 1)
			{
				sbyte oldMarkCount3 = oldInjuries.Get(part2, true);
				for (int i5 = 0; i5 < (int)this.InnerInjuryMarkList[(int)part2]; i5 = num + 1)
				{
					yield return new ValueTuple<EMarkType, int, int>(EMarkType.Inner, (int)part2, (i5 < (int)oldMarkCount3) ? 1 : 0);
					num = i5;
				}
				b = part2;
			}
			for (sbyte part3 = 0; part3 < 7; part3 = b + 1)
			{
				foreach (byte flaw in this.FlawMarkList[(int)part3])
				{
					yield return new ValueTuple<EMarkType, int, int>(EMarkType.Flaw, (int)part3, (int)flaw);
				}
				IEnumerator<byte> enumerator = null;
				b = part3;
			}
			for (sbyte part4 = 0; part4 < 7; part4 = b + 1)
			{
				foreach (byte acupoint in this.AcupointMarkList[(int)part4])
				{
					yield return new ValueTuple<EMarkType, int, int>(EMarkType.Acupoint, (int)part4, (int)acupoint);
				}
				IEnumerator<byte> enumerator2 = null;
				b = part4;
			}
			for (int i6 = 0; i6 < this.FatalDamageMarkCount; i6 = num + 1)
			{
				yield return EMarkType.Fatal;
				num = i6;
			}
			for (int i7 = 0; i7 < this.MindMarkList.Count; i7 = num + 1)
			{
				yield return new ValueTuple<EMarkType, int, int>(EMarkType.Mind, 0, this.MindMarkList[i7] ? 1 : 0);
				num = i7;
			}
			yield break;
			yield break;
		}

		// Token: 0x0600668A RID: 26250 RVA: 0x003AC5E4 File Offset: 0x003AA7E4
		public static sbyte CalcQiDisorderMarkCount(int disorderOfQi)
		{
			return (sbyte)MathUtils.Clamp((disorderOfQi - (int)DefeatMarkCollection.QiDisorderFirstExtra) / (int)DefeatMarkCollection.QiDisorderThreshold, 0, 6);
		}

		// Token: 0x0600668B RID: 26251 RVA: 0x003AC60C File Offset: 0x003AA80C
		public static short CalcQiDisorderMarkThreshold(int disorderOfQi)
		{
			return (DefeatMarkCollection.CalcQiDisorderMarkCount(disorderOfQi) == 0) ? (DefeatMarkCollection.QiDisorderThreshold + DefeatMarkCollection.QiDisorderFirstExtra) : DefeatMarkCollection.QiDisorderThreshold;
		}

		// Token: 0x0600668C RID: 26252 RVA: 0x003AC63C File Offset: 0x003AA83C
		public int GetTotalCount()
		{
			return this.OuterInjuryMarkList.Sum() + this.InnerInjuryMarkList.Sum() + this.GetTotalFlawCount() + this.GetTotalAcupointCount() + this.PoisonMarkList.Sum() + this.MindMarkList.Count + this.DieMarkList.Count + this.FatalDamageMarkCount + (int)this.WugMarkCount + (int)this.QiDisorderMarkCount + (int)this.StateMarkCount + (int)this.NeiliAllocationMarkCount.Item1 + (int)this.NeiliAllocationMarkCount.Item2 + (int)this.HealthMarkCount;
		}

		// Token: 0x0600668D RID: 26253 RVA: 0x003AC6D4 File Offset: 0x003AA8D4
		public int GetTotalInjuryCount()
		{
			int totalCount = 0;
			totalCount += this.OuterInjuryMarkList.Sum();
			return totalCount + this.InnerInjuryMarkList.Sum();
		}

		// Token: 0x0600668E RID: 26254 RVA: 0x003AC708 File Offset: 0x003AA908
		public int GetTotalFlawCount()
		{
			int totalCount = 0;
			for (sbyte part = 0; part < 7; part += 1)
			{
				totalCount += this.FlawMarkList[(int)part].Count;
			}
			return totalCount;
		}

		// Token: 0x0600668F RID: 26255 RVA: 0x003AC740 File Offset: 0x003AA940
		public int GetTotalAcupointCount()
		{
			int totalCount = 0;
			for (sbyte part = 0; part < 7; part += 1)
			{
				totalCount += this.AcupointMarkList[(int)part].Count;
			}
			return totalCount;
		}

		// Token: 0x06006690 RID: 26256 RVA: 0x003AC778 File Offset: 0x003AA978
		public void Clear()
		{
			for (sbyte part = 0; part < 7; part += 1)
			{
				this.OuterInjuryMarkList[(int)part] = 0;
				this.InnerInjuryMarkList[(int)part] = 0;
				this.FlawMarkList[(int)part].Clear();
				this.AcupointMarkList[(int)part].Clear();
			}
			for (int i = 0; i < 6; i++)
			{
				this.PoisonMarkList[i] = 0;
			}
			this.MindMarkList.Clear();
			this.DieMarkList.Clear();
			this.FatalDamageMarkCount = 0;
			this.WugMarkCount = (this.QiDisorderMarkCount = (this.StateMarkCount = (this.HealthMarkCount = 0)));
			this.NeiliAllocationMarkCount = new ValueTuple<sbyte, sbyte>(0, 0);
		}

		// Token: 0x06006691 RID: 26257 RVA: 0x003AC834 File Offset: 0x003AAA34
		public bool AnyMarkAdded(DefeatMarkCollection other)
		{
			for (sbyte part = 0; part < 7; part += 1)
			{
				bool flag = this.OuterInjuryMarkList[(int)part] > other.OuterInjuryMarkList[(int)part] || this.InnerInjuryMarkList[(int)part] > other.InnerInjuryMarkList[(int)part] || this.FlawMarkList[(int)part].Count > other.FlawMarkList[(int)part].Count || this.AcupointMarkList[(int)part].Count > other.AcupointMarkList[(int)part].Count;
				if (flag)
				{
					return true;
				}
			}
			for (sbyte type = 0; type < 6; type += 1)
			{
				bool flag2 = this.PoisonMarkList[(int)type] > other.PoisonMarkList[(int)type];
				if (flag2)
				{
					return true;
				}
			}
			bool flag3 = this.MindMarkList.Count > other.MindMarkList.Count || this.DieMarkList.Count > other.DieMarkList.Count;
			if (flag3)
			{
				return true;
			}
			bool flag4 = this.FatalDamageMarkCount > other.FatalDamageMarkCount;
			if (flag4)
			{
				return true;
			}
			bool flag5 = this.WugMarkCount > other.WugMarkCount;
			if (flag5)
			{
				return true;
			}
			bool flag6 = this.QiDisorderMarkCount > other.QiDisorderMarkCount;
			if (flag6)
			{
				return true;
			}
			bool flag7 = this.StateMarkCount > other.StateMarkCount;
			if (flag7)
			{
				return true;
			}
			bool flag8 = this.NeiliAllocationMarkCount.Item1 > other.NeiliAllocationMarkCount.Item1 || this.NeiliAllocationMarkCount.Item2 > other.NeiliAllocationMarkCount.Item2;
			if (flag8)
			{
				return true;
			}
			return this.HealthMarkCount > other.HealthMarkCount;
		}

		// Token: 0x06006692 RID: 26258 RVA: 0x003AC9FC File Offset: 0x003AABFC
		public DefeatMarkCollection()
		{
			for (sbyte part = 0; part < 7; part += 1)
			{
				this.FlawMarkList[(int)part] = new ByteList();
				this.AcupointMarkList[(int)part] = new ByteList();
			}
		}

		// Token: 0x06006693 RID: 26259 RVA: 0x003ACAC0 File Offset: 0x003AACC0
		public DefeatMarkCollection(DefeatMarkCollection other)
		{
			byte[] item = other.OuterInjuryMarkList;
			int elementsCount = item.Length;
			this.OuterInjuryMarkList = new byte[elementsCount];
			for (int i = 0; i < elementsCount; i++)
			{
				this.OuterInjuryMarkList[i] = item[i];
			}
			byte[] item2 = other.InnerInjuryMarkList;
			int elementsCount2 = item2.Length;
			this.InnerInjuryMarkList = new byte[elementsCount2];
			for (int j = 0; j < elementsCount2; j++)
			{
				this.InnerInjuryMarkList[j] = item2[j];
			}
			ByteList[] item3 = other.FlawMarkList;
			int elementsCount3 = item3.Length;
			this.FlawMarkList = new ByteList[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.FlawMarkList[k] = new ByteList(item3[k]);
			}
			ByteList[] item4 = other.AcupointMarkList;
			int elementsCount4 = item4.Length;
			this.AcupointMarkList = new ByteList[elementsCount4];
			for (int l = 0; l < elementsCount4; l++)
			{
				this.AcupointMarkList[l] = new ByteList(item4[l]);
			}
			byte[] item5 = other.PoisonMarkList;
			int elementsCount5 = item5.Length;
			this.PoisonMarkList = new byte[elementsCount5];
			for (int m = 0; m < elementsCount5; m++)
			{
				this.PoisonMarkList[m] = item5[m];
			}
			this.MindMarkList = ((other.MindMarkList == null) ? null : new List<bool>(other.MindMarkList));
			this.DieMarkList = ((other.DieMarkList == null) ? null : new List<CombatSkillKey>(other.DieMarkList));
			this.FatalDamageMarkCount = other.FatalDamageMarkCount;
			this.WugMarkCount = other.WugMarkCount;
			this.QiDisorderMarkCount = other.QiDisorderMarkCount;
			this.StateMarkCount = other.StateMarkCount;
			this.NeiliAllocationMarkCount = other.NeiliAllocationMarkCount;
			this.HealthMarkCount = other.HealthMarkCount;
		}

		// Token: 0x06006694 RID: 26260 RVA: 0x003ACD20 File Offset: 0x003AAF20
		public void Assign(DefeatMarkCollection other)
		{
			byte[] item = other.OuterInjuryMarkList;
			int elementsCount = item.Length;
			this.OuterInjuryMarkList = new byte[elementsCount];
			for (int i = 0; i < elementsCount; i++)
			{
				this.OuterInjuryMarkList[i] = item[i];
			}
			byte[] item2 = other.InnerInjuryMarkList;
			int elementsCount2 = item2.Length;
			this.InnerInjuryMarkList = new byte[elementsCount2];
			for (int j = 0; j < elementsCount2; j++)
			{
				this.InnerInjuryMarkList[j] = item2[j];
			}
			ByteList[] item3 = other.FlawMarkList;
			int elementsCount3 = item3.Length;
			this.FlawMarkList = new ByteList[elementsCount3];
			for (int k = 0; k < elementsCount3; k++)
			{
				this.FlawMarkList[k] = new ByteList(item3[k]);
			}
			ByteList[] item4 = other.AcupointMarkList;
			int elementsCount4 = item4.Length;
			this.AcupointMarkList = new ByteList[elementsCount4];
			for (int l = 0; l < elementsCount4; l++)
			{
				this.AcupointMarkList[l] = new ByteList(item4[l]);
			}
			byte[] item5 = other.PoisonMarkList;
			int elementsCount5 = item5.Length;
			this.PoisonMarkList = new byte[elementsCount5];
			for (int m = 0; m < elementsCount5; m++)
			{
				this.PoisonMarkList[m] = item5[m];
			}
			this.MindMarkList = ((other.MindMarkList == null) ? null : new List<bool>(other.MindMarkList));
			this.DieMarkList = ((other.DieMarkList == null) ? null : new List<CombatSkillKey>(other.DieMarkList));
			this.FatalDamageMarkCount = other.FatalDamageMarkCount;
			this.WugMarkCount = other.WugMarkCount;
			this.QiDisorderMarkCount = other.QiDisorderMarkCount;
			this.StateMarkCount = other.StateMarkCount;
			this.NeiliAllocationMarkCount = other.NeiliAllocationMarkCount;
			this.HealthMarkCount = other.HealthMarkCount;
		}

		// Token: 0x06006695 RID: 26261 RVA: 0x003ACEF4 File Offset: 0x003AB0F4
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006696 RID: 26262 RVA: 0x003ACF08 File Offset: 0x003AB108
		public int GetSerializedSize()
		{
			int totalSize = 10;
			bool flag = this.OuterInjuryMarkList != null;
			if (flag)
			{
				totalSize += 2 + this.OuterInjuryMarkList.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag2 = this.InnerInjuryMarkList != null;
			if (flag2)
			{
				totalSize += 2 + this.InnerInjuryMarkList.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag3 = this.FlawMarkList != null;
			if (flag3)
			{
				totalSize += 2;
				int elementsCount = this.FlawMarkList.Length;
				for (int i = 0; i < elementsCount; i++)
				{
					ByteList element = this.FlawMarkList[i];
					bool flag4 = element != null;
					if (flag4)
					{
						totalSize += 2 + element.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag5 = this.AcupointMarkList != null;
			if (flag5)
			{
				totalSize += 2;
				int elementsCount2 = this.AcupointMarkList.Length;
				for (int j = 0; j < elementsCount2; j++)
				{
					ByteList element2 = this.AcupointMarkList[j];
					bool flag6 = element2 != null;
					if (flag6)
					{
						totalSize += 2 + element2.GetSerializedSize();
					}
					else
					{
						totalSize += 2;
					}
				}
			}
			else
			{
				totalSize += 2;
			}
			bool flag7 = this.PoisonMarkList != null;
			if (flag7)
			{
				totalSize += 2 + this.PoisonMarkList.Length;
			}
			else
			{
				totalSize += 2;
			}
			bool flag8 = this.MindMarkList != null;
			if (flag8)
			{
				totalSize += 2 + this.MindMarkList.Count;
			}
			else
			{
				totalSize += 2;
			}
			bool flag9 = this.DieMarkList != null;
			if (flag9)
			{
				totalSize += 2 + 8 * this.DieMarkList.Count;
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006697 RID: 26263 RVA: 0x003AD0AC File Offset: 0x003AB2AC
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.OuterInjuryMarkList != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.OuterInjuryMarkList.Length;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					pCurrData[i] = this.OuterInjuryMarkList[i];
				}
				pCurrData += elementsCount;
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			bool flag2 = this.InnerInjuryMarkList != null;
			if (flag2)
			{
				int elementsCount2 = this.InnerInjuryMarkList.Length;
				Tester.Assert(elementsCount2 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount2);
				pCurrData += 2;
				for (int j = 0; j < elementsCount2; j++)
				{
					pCurrData[j] = this.InnerInjuryMarkList[j];
				}
				pCurrData += elementsCount2;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag3 = this.FlawMarkList != null;
			if (flag3)
			{
				int elementsCount3 = this.FlawMarkList.Length;
				Tester.Assert(elementsCount3 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount3);
				pCurrData += 2;
				for (int k = 0; k < elementsCount3; k++)
				{
					ByteList element = this.FlawMarkList[k];
					bool flag4 = element != null;
					if (flag4)
					{
						byte* pSubDataCount = pCurrData;
						pCurrData += 2;
						int subDataSize = element.Serialize(pCurrData);
						pCurrData += subDataSize;
						Tester.Assert(subDataSize <= 65535, "");
						*(short*)pSubDataCount = (short)((ushort)subDataSize);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag5 = this.AcupointMarkList != null;
			if (flag5)
			{
				int elementsCount4 = this.AcupointMarkList.Length;
				Tester.Assert(elementsCount4 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount4);
				pCurrData += 2;
				for (int l = 0; l < elementsCount4; l++)
				{
					ByteList element2 = this.AcupointMarkList[l];
					bool flag6 = element2 != null;
					if (flag6)
					{
						byte* pSubDataCount2 = pCurrData;
						pCurrData += 2;
						int subDataSize2 = element2.Serialize(pCurrData);
						pCurrData += subDataSize2;
						Tester.Assert(subDataSize2 <= 65535, "");
						*(short*)pSubDataCount2 = (short)((ushort)subDataSize2);
					}
					else
					{
						*(short*)pCurrData = 0;
						pCurrData += 2;
					}
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag7 = this.PoisonMarkList != null;
			if (flag7)
			{
				int elementsCount5 = this.PoisonMarkList.Length;
				Tester.Assert(elementsCount5 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount5);
				pCurrData += 2;
				for (int m = 0; m < elementsCount5; m++)
				{
					pCurrData[m] = this.PoisonMarkList[m];
				}
				pCurrData += elementsCount5;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag8 = this.MindMarkList != null;
			if (flag8)
			{
				int elementsCount6 = this.MindMarkList.Count;
				Tester.Assert(elementsCount6 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount6);
				pCurrData += 2;
				for (int n = 0; n < elementsCount6; n++)
				{
					pCurrData[n] = (this.MindMarkList[n] ? 1 : 0);
				}
				pCurrData += elementsCount6;
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			bool flag9 = this.DieMarkList != null;
			if (flag9)
			{
				int elementsCount7 = this.DieMarkList.Count;
				Tester.Assert(elementsCount7 <= 65535, "");
				*(short*)pCurrData = (short)((ushort)elementsCount7);
				pCurrData += 2;
				for (int i2 = 0; i2 < elementsCount7; i2++)
				{
					pCurrData += this.DieMarkList[i2].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pCurrData = 0;
				pCurrData += 2;
			}
			*(int*)pCurrData = this.FatalDamageMarkCount;
			pCurrData += 4;
			*pCurrData = (byte)this.WugMarkCount;
			pCurrData++;
			*pCurrData = (byte)this.QiDisorderMarkCount;
			pCurrData++;
			*pCurrData = (byte)this.StateMarkCount;
			pCurrData++;
			pCurrData += SerializationHelper.Serialize<sbyte, sbyte>(pCurrData, this.NeiliAllocationMarkCount);
			*pCurrData = (byte)this.HealthMarkCount;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006698 RID: 26264 RVA: 0x003AD4D4 File Offset: 0x003AB6D4
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.OuterInjuryMarkList == null || this.OuterInjuryMarkList.Length != (int)elementsCount;
				if (flag2)
				{
					this.OuterInjuryMarkList = new byte[(int)elementsCount];
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					this.OuterInjuryMarkList[i] = pCurrData[i];
				}
				pCurrData += elementsCount;
			}
			else
			{
				this.OuterInjuryMarkList = null;
			}
			ushort elementsCount2 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag3 = elementsCount2 > 0;
			if (flag3)
			{
				bool flag4 = this.InnerInjuryMarkList == null || this.InnerInjuryMarkList.Length != (int)elementsCount2;
				if (flag4)
				{
					this.InnerInjuryMarkList = new byte[(int)elementsCount2];
				}
				for (int j = 0; j < (int)elementsCount2; j++)
				{
					this.InnerInjuryMarkList[j] = pCurrData[j];
				}
				pCurrData += elementsCount2;
			}
			else
			{
				this.InnerInjuryMarkList = null;
			}
			ushort elementsCount3 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag5 = elementsCount3 > 0;
			if (flag5)
			{
				bool flag6 = this.FlawMarkList == null || this.FlawMarkList.Length != (int)elementsCount3;
				if (flag6)
				{
					this.FlawMarkList = new ByteList[(int)elementsCount3];
				}
				for (int k = 0; k < (int)elementsCount3; k++)
				{
					ushort subDataCount = *(ushort*)pCurrData;
					pCurrData += 2;
					bool flag7 = subDataCount > 0;
					if (flag7)
					{
						ByteList element = this.FlawMarkList[k] ?? new ByteList();
						pCurrData += element.Deserialize(pCurrData);
						this.FlawMarkList[k] = element;
					}
					else
					{
						this.FlawMarkList[k] = null;
					}
				}
			}
			else
			{
				this.FlawMarkList = null;
			}
			ushort elementsCount4 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag8 = elementsCount4 > 0;
			if (flag8)
			{
				bool flag9 = this.AcupointMarkList == null || this.AcupointMarkList.Length != (int)elementsCount4;
				if (flag9)
				{
					this.AcupointMarkList = new ByteList[(int)elementsCount4];
				}
				for (int l = 0; l < (int)elementsCount4; l++)
				{
					ushort subDataCount2 = *(ushort*)pCurrData;
					pCurrData += 2;
					bool flag10 = subDataCount2 > 0;
					if (flag10)
					{
						ByteList element2 = this.AcupointMarkList[l] ?? new ByteList();
						pCurrData += element2.Deserialize(pCurrData);
						this.AcupointMarkList[l] = element2;
					}
					else
					{
						this.AcupointMarkList[l] = null;
					}
				}
			}
			else
			{
				this.AcupointMarkList = null;
			}
			ushort elementsCount5 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag11 = elementsCount5 > 0;
			if (flag11)
			{
				bool flag12 = this.PoisonMarkList == null || this.PoisonMarkList.Length != (int)elementsCount5;
				if (flag12)
				{
					this.PoisonMarkList = new byte[(int)elementsCount5];
				}
				for (int m = 0; m < (int)elementsCount5; m++)
				{
					this.PoisonMarkList[m] = pCurrData[m];
				}
				pCurrData += elementsCount5;
			}
			else
			{
				this.PoisonMarkList = null;
			}
			ushort elementsCount6 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag13 = elementsCount6 > 0;
			if (flag13)
			{
				bool flag14 = this.MindMarkList == null;
				if (flag14)
				{
					this.MindMarkList = new List<bool>((int)elementsCount6);
				}
				else
				{
					this.MindMarkList.Clear();
				}
				for (int n = 0; n < (int)elementsCount6; n++)
				{
					this.MindMarkList.Add(pCurrData[n] != 0);
				}
				pCurrData += elementsCount6;
			}
			else
			{
				List<bool> mindMarkList = this.MindMarkList;
				if (mindMarkList != null)
				{
					mindMarkList.Clear();
				}
			}
			ushort elementsCount7 = *(ushort*)pCurrData;
			pCurrData += 2;
			bool flag15 = elementsCount7 > 0;
			if (flag15)
			{
				bool flag16 = this.DieMarkList == null;
				if (flag16)
				{
					this.DieMarkList = new List<CombatSkillKey>((int)elementsCount7);
				}
				else
				{
					this.DieMarkList.Clear();
				}
				for (int i2 = 0; i2 < (int)elementsCount7; i2++)
				{
					CombatSkillKey element3 = default(CombatSkillKey);
					pCurrData += element3.Deserialize(pCurrData);
					this.DieMarkList.Add(element3);
				}
			}
			else
			{
				List<CombatSkillKey> dieMarkList = this.DieMarkList;
				if (dieMarkList != null)
				{
					dieMarkList.Clear();
				}
			}
			this.FatalDamageMarkCount = *(int*)pCurrData;
			pCurrData += 4;
			this.WugMarkCount = *(sbyte*)pCurrData;
			pCurrData++;
			this.QiDisorderMarkCount = *(sbyte*)pCurrData;
			pCurrData++;
			this.StateMarkCount = *(sbyte*)pCurrData;
			pCurrData++;
			pCurrData += SerializationHelper.Deserialize<sbyte, sbyte>(pCurrData, out this.NeiliAllocationMarkCount);
			this.HealthMarkCount = *(sbyte*)pCurrData;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006699 RID: 26265 RVA: 0x003AD928 File Offset: 0x003ABB28
		public unsafe IEnumerable<DefeatMarkKey> GetAllKeys(CombatCharacter combatChar)
		{
			return this.GetAllKeys(combatChar.GetOldDisorderOfQi(), *combatChar.GetOldPoison(), combatChar.GetOldInjuries());
		}

		// Token: 0x0600669A RID: 26266 RVA: 0x003AD958 File Offset: 0x003ABB58
		public IEnumerable<DefeatMarkKey> GetAllKeysWithoutOld()
		{
			PoisonInts oldPoisons = default(PoisonInts);
			oldPoisons.Initialize();
			Injuries oldInjuries = default(Injuries);
			oldInjuries.Initialize();
			return this.GetAllKeys(0, oldPoisons, oldInjuries);
		}

		// Token: 0x0600669B RID: 26267 RVA: 0x003AD994 File Offset: 0x003ABB94
		public void SyncMindMark(DataContext context, CombatCharacter combatChar)
		{
			Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
			int oldCount = this.MindMarkList.Count;
			bool flag = this.SyncMindMark(combatChar);
			if (flag)
			{
				combatChar.SetDefeatMarkCollection(this, context);
			}
			int newCount = this.MindMarkList.Count;
			bool flag2 = newCount > oldCount;
			if (flag2)
			{
				DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
			}
		}

		// Token: 0x0600669C RID: 26268 RVA: 0x003AD9FC File Offset: 0x003ABBFC
		public void SyncOtherMark(DataContext context, CombatCharacter combatChar)
		{
			Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
			int oldCount = this.GetTotalCount();
			bool flag = this.SyncOtherMark(combatChar);
			if (flag)
			{
				combatChar.SetDefeatMarkCollection(this, context);
			}
			int newCount = this.GetTotalCount();
			bool flag2 = newCount > oldCount;
			if (flag2)
			{
				DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
			}
		}

		// Token: 0x0600669D RID: 26269 RVA: 0x003ADA5C File Offset: 0x003ABC5C
		public void SyncWugMark(DataContext context, DataUid uid)
		{
			int charId = (int)uid.SubId0;
			CombatCharacter combatChar;
			bool flag = !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar);
			if (!flag)
			{
				Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
				sbyte oldCount = this.WugMarkCount;
				bool flag2 = this.SyncWugMark(combatChar);
				if (flag2)
				{
					combatChar.SetDefeatMarkCollection(this, context);
				}
				sbyte newCount = this.WugMarkCount;
				bool flag3 = oldCount < newCount;
				if (flag3)
				{
					DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
				}
			}
		}

		// Token: 0x0600669E RID: 26270 RVA: 0x003ADAE0 File Offset: 0x003ABCE0
		public void SyncQiDisorderMark(DataContext context, DataUid uid)
		{
			int charId = (int)uid.SubId0;
			CombatCharacter combatChar;
			bool flag = !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar);
			if (!flag)
			{
				Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
				sbyte oldCount = this.QiDisorderMarkCount;
				bool flag2 = this.SyncQiDisorderMark(combatChar);
				if (flag2)
				{
					combatChar.SetDefeatMarkCollection(this, context);
				}
				sbyte newCount = this.QiDisorderMarkCount;
				bool flag3 = oldCount < newCount;
				if (flag3)
				{
					DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
				}
			}
		}

		// Token: 0x0600669F RID: 26271 RVA: 0x003ADB64 File Offset: 0x003ABD64
		public void SyncCombatStateMark(DataContext context, CombatCharacter combatChar)
		{
			Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
			sbyte oldCount = this.StateMarkCount;
			bool flag = this.SyncCombatStateMark(combatChar);
			if (flag)
			{
				combatChar.SetDefeatMarkCollection(this, context);
			}
			sbyte newCount = this.StateMarkCount;
			bool flag2 = oldCount < newCount;
			if (flag2)
			{
				DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
			}
		}

		// Token: 0x060066A0 RID: 26272 RVA: 0x003ADBC4 File Offset: 0x003ABDC4
		public void SyncNeiliAllocationMark(DataContext context, CombatCharacter combatChar)
		{
			Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
			ValueTuple<sbyte, sbyte> oldCount = this.NeiliAllocationMarkCount;
			bool flag = this.SyncNeiliAllocationMark(combatChar);
			if (flag)
			{
				combatChar.SetDefeatMarkCollection(this, context);
			}
			ValueTuple<sbyte, sbyte> newCount = this.NeiliAllocationMarkCount;
			bool flag2 = oldCount.Item1 + oldCount.Item2 < newCount.Item1 + newCount.Item2;
			if (flag2)
			{
				DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
			}
		}

		// Token: 0x060066A1 RID: 26273 RVA: 0x003ADC3C File Offset: 0x003ABE3C
		public void SyncHealthMark(DataContext context, DataUid uid)
		{
			int charId = (int)uid.SubId0;
			CombatCharacter combatChar;
			bool flag = !DomainManager.Combat.TryGetElement_CombatCharacterDict(charId, out combatChar);
			if (!flag)
			{
				Tester.Assert(combatChar.GetDefeatMarkCollection() == this, "");
				sbyte oldCount = this.HealthMarkCount;
				bool flag2 = this.SyncHealthMark(combatChar);
				if (flag2)
				{
					combatChar.SetDefeatMarkCollection(this, context);
				}
				sbyte newCount = this.HealthMarkCount;
				bool flag3 = oldCount < newCount;
				if (flag3)
				{
					DomainManager.Combat.AddToCheckFallenSet(combatChar.GetId());
				}
			}
		}

		// Token: 0x060066A2 RID: 26274 RVA: 0x003ADCC0 File Offset: 0x003ABEC0
		private bool SyncMindMark(CombatCharacter combatChar)
		{
			List<bool> newMindMark = ObjectPool<List<bool>>.Instance.Get();
			newMindMark.Clear();
			newMindMark.AddRange(from x in combatChar.GetMindMarkTime().MarkList
			select x.Infinity);
			CollectionUtils.Sort<bool>(newMindMark, (bool a, bool b) => b.CompareTo(a));
			bool flag = newMindMark.SequenceEqual(this.MindMarkList);
			bool anyChanged;
			if (flag)
			{
				anyChanged = false;
			}
			else
			{
				anyChanged = true;
				this.MindMarkList.Clear();
				this.MindMarkList.AddRange(newMindMark);
			}
			ObjectPool<List<bool>>.Instance.Return(newMindMark);
			return anyChanged;
		}

		// Token: 0x060066A3 RID: 26275 RVA: 0x003ADD80 File Offset: 0x003ABF80
		private bool SyncOtherMark(CombatCharacter combatChar)
		{
			bool anyChanged = this.SyncWugMark(combatChar);
			anyChanged = (this.SyncQiDisorderMark(combatChar) || anyChanged);
			anyChanged = (this.SyncCombatStateMark(combatChar) || anyChanged);
			anyChanged = (this.SyncNeiliAllocationMark(combatChar) || anyChanged);
			return this.SyncHealthMark(combatChar) || anyChanged;
		}

		// Token: 0x060066A4 RID: 26276 RVA: 0x003ADDC4 File Offset: 0x003ABFC4
		private bool SyncWugMark(CombatCharacter combatChar)
		{
			sbyte newCount = combatChar.GetCharacter().GetEatingItems().CountOfWugMark();
			bool flag = newCount == this.WugMarkCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.WugMarkCount = newCount;
				result = true;
			}
			return result;
		}

		// Token: 0x060066A5 RID: 26277 RVA: 0x003ADE00 File Offset: 0x003AC000
		private bool SyncQiDisorderMark(CombatCharacter combatChar)
		{
			sbyte newCount = DefeatMarkCollection.GetQiDisorderMarkCount(combatChar);
			bool flag = newCount == this.QiDisorderMarkCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.QiDisorderMarkCount = newCount;
				result = true;
			}
			return result;
		}

		// Token: 0x060066A6 RID: 26278 RVA: 0x003ADE34 File Offset: 0x003AC034
		private bool SyncCombatStateMark(CombatCharacter combatChar)
		{
			sbyte newCount = DefeatMarkCollection.GetCombatStateMarkCount(combatChar);
			bool flag = newCount == this.StateMarkCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.StateMarkCount = newCount;
				result = true;
			}
			return result;
		}

		// Token: 0x060066A7 RID: 26279 RVA: 0x003ADE68 File Offset: 0x003AC068
		private bool SyncNeiliAllocationMark(CombatCharacter combatChar)
		{
			ValueTuple<sbyte, sbyte> newCount = DefeatMarkCollection.GetNeiliAllocationMarkCount(combatChar);
			ValueTuple<sbyte, sbyte> valueTuple = newCount;
			sbyte item = valueTuple.Item1;
			sbyte item2 = valueTuple.Item2;
			ValueTuple<sbyte, sbyte> neiliAllocationMarkCount = this.NeiliAllocationMarkCount;
			sbyte item3 = neiliAllocationMarkCount.Item1;
			sbyte item4 = neiliAllocationMarkCount.Item2;
			bool flag = item == item3 && item2 == item4;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.NeiliAllocationMarkCount = newCount;
				result = true;
			}
			return result;
		}

		// Token: 0x060066A8 RID: 26280 RVA: 0x003ADEC4 File Offset: 0x003AC0C4
		private bool SyncHealthMark(CombatCharacter combatChar)
		{
			sbyte newCount = DefeatMarkCollection.GetHealthMarkCount(combatChar.GetCharacter());
			bool flag = newCount == this.HealthMarkCount;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				this.HealthMarkCount = newCount;
				result = true;
			}
			return result;
		}

		// Token: 0x060066A9 RID: 26281 RVA: 0x003ADEFC File Offset: 0x003AC0FC
		private static sbyte GetQiDisorderMarkCount(CombatCharacter combatChar)
		{
			return DefeatMarkCollection.CalcQiDisorderMarkCount((int)combatChar.GetCharacter().GetDisorderOfQi());
		}

		// Token: 0x060066AA RID: 26282 RVA: 0x003ADF20 File Offset: 0x003AC120
		private static sbyte GetCombatStateMarkCount(CombatCharacter combatChar)
		{
			short powerPerMark = GlobalConfig.Instance.DefeatMarkCombatStatePower;
			sbyte maxMarkCount = GlobalConfig.Instance.DefeatMarkCombatStateMaxCount;
			return (sbyte)Math.Clamp(-combatChar.GetCombatStateTotalBuffPower() / (int)powerPerMark, 0, (int)maxMarkCount);
		}

		// Token: 0x060066AB RID: 26283 RVA: 0x003ADF5C File Offset: 0x003AC15C
		[return: TupleElementNames(new string[]
		{
			"scatter",
			"bulge"
		})]
		private static ValueTuple<sbyte, sbyte> GetNeiliAllocationMarkCount(CombatCharacter combatChar)
		{
			sbyte scatter = 0;
			sbyte bulge = 0;
			for (byte i = 0; i < 4; i += 1)
			{
				NeiliAllocationStatusItem config = combatChar.GetNeiliAllocationStatus(i).GetConfig();
				bool flag = config.MarkCount < 0;
				if (flag)
				{
					scatter += Math.Abs(config.MarkCount);
				}
				else
				{
					bool flag2 = config.MarkCount > 0;
					if (flag2)
					{
						bulge += Math.Abs(config.MarkCount);
					}
				}
			}
			return new ValueTuple<sbyte, sbyte>(scatter, bulge);
		}

		// Token: 0x060066AC RID: 26284 RVA: 0x003ADFDC File Offset: 0x003AC1DC
		public static sbyte GetHealthMarkCount(GameData.Domains.Character.Character character)
		{
			EHealthType healthType = character.GetHealthType();
			if (!true)
			{
			}
			sbyte result;
			switch (healthType)
			{
			case EHealthType.Dying:
				result = 8;
				break;
			case EHealthType.CriticallyIll:
				result = 6;
				break;
			case EHealthType.Weak:
				result = 4;
				break;
			case EHealthType.Sick:
				result = 2;
				break;
			case EHealthType.Healthy:
			case EHealthType.Unknown:
				result = 0;
				break;
			default:
				result = 0;
				break;
			}
			if (!true)
			{
			}
			return result;
		}

		// Token: 0x04001BE8 RID: 7144
		[SerializableGameDataField]
		public byte[] OuterInjuryMarkList = new byte[7];

		// Token: 0x04001BE9 RID: 7145
		[SerializableGameDataField]
		public byte[] InnerInjuryMarkList = new byte[7];

		// Token: 0x04001BEA RID: 7146
		[SerializableGameDataField]
		public ByteList[] FlawMarkList = new ByteList[7];

		// Token: 0x04001BEB RID: 7147
		[SerializableGameDataField]
		public ByteList[] AcupointMarkList = new ByteList[7];

		// Token: 0x04001BEC RID: 7148
		[SerializableGameDataField]
		public byte[] PoisonMarkList = new byte[6];

		// Token: 0x04001BED RID: 7149
		[SerializableGameDataField]
		public List<bool> MindMarkList = new List<bool>();

		// Token: 0x04001BEE RID: 7150
		[SerializableGameDataField]
		public List<CombatSkillKey> DieMarkList = new List<CombatSkillKey>();

		// Token: 0x04001BEF RID: 7151
		[SerializableGameDataField]
		public int FatalDamageMarkCount = 0;

		// Token: 0x04001BF0 RID: 7152
		[SerializableGameDataField]
		public sbyte WugMarkCount = 0;

		// Token: 0x04001BF1 RID: 7153
		[SerializableGameDataField]
		public sbyte QiDisorderMarkCount = 0;

		// Token: 0x04001BF2 RID: 7154
		[SerializableGameDataField]
		public sbyte StateMarkCount = 0;

		// Token: 0x04001BF3 RID: 7155
		[TupleElementNames(new string[]
		{
			"scatter",
			"bulge"
		})]
		[SerializableGameDataField]
		public ValueTuple<sbyte, sbyte> NeiliAllocationMarkCount = new ValueTuple<sbyte, sbyte>(0, 0);

		// Token: 0x04001BF4 RID: 7156
		[SerializableGameDataField]
		public sbyte HealthMarkCount = 0;
	}
}
