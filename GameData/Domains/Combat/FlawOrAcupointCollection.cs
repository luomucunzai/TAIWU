using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Config;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C2 RID: 1730
	public class FlawOrAcupointCollection : ISerializableGameData
	{
		// Token: 0x060066CB RID: 26315 RVA: 0x003AE4C0 File Offset: 0x003AC6C0
		public FlawOrAcupointCollection()
		{
			this.BodyPartDict = new SortedDictionary<sbyte, List<ValueTuple<sbyte, int, int>>>();
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				this.BodyPartDict.Add(bodyPart, new List<ValueTuple<sbyte, int, int>>());
			}
		}

		// Token: 0x060066CC RID: 26316 RVA: 0x003AE510 File Offset: 0x003AC710
		public int GetTotalCount()
		{
			int totalCount = 0;
			foreach (List<ValueTuple<sbyte, int, int>> lst in this.BodyPartDict.Values)
			{
				totalCount += lst.Count;
			}
			return totalCount;
		}

		// Token: 0x060066CD RID: 26317 RVA: 0x003AE574 File Offset: 0x003AC774
		public FlawOrAcupointCollection.ReduceKeepTimeResult ReduceKeepTime(CombatCharacter combatChar, int recoverSpeed, byte[] countArray, bool isFlaw)
		{
			recoverSpeed = CFormula.CalcFlawOrAcupointRecoverSpeed(recoverSpeed);
			ushort fieldId = isFlaw ? 316 : 300;
			recoverSpeed = DomainManager.SpecialEffect.ModifyValue(combatChar.GetId(), fieldId, recoverSpeed, -1, -1, -1, 0, 0, 0, 0);
			recoverSpeed = Math.Max(recoverSpeed, 1);
			return this.ReduceKeepTimePercentInternal(combatChar, (int _) => recoverSpeed, countArray, isFlaw);
		}

		// Token: 0x060066CE RID: 26318 RVA: 0x003AE604 File Offset: 0x003AC804
		public FlawOrAcupointCollection.ReduceKeepTimeResult ReduceKeepTimePercent(CombatCharacter combatChar, int reducePercent, byte[] countArray, bool isFlaw)
		{
			return this.ReduceKeepTimePercentInternal(combatChar, (int totalFrame) => totalFrame * reducePercent / 100, countArray, isFlaw);
		}

		// Token: 0x060066CF RID: 26319 RVA: 0x003AE63C File Offset: 0x003AC83C
		private FlawOrAcupointCollection.ReduceKeepTimeResult ReduceKeepTimePercentInternal(CombatCharacter combatChar, FlawOrAcupointCollection.ReduceFrameDelegate reduceFrameDelegate, byte[] countArray, bool isFlaw)
		{
			bool dataChanged = false;
			bool countChanged = false;
			this._removedList.Clear();
			foreach (KeyValuePair<sbyte, List<ValueTuple<sbyte, int, int>>> keyValuePair in this.BodyPartDict)
			{
				sbyte b;
				List<ValueTuple<sbyte, int, int>> list;
				keyValuePair.Deconstruct(out b, out list);
				sbyte bodyPart = b;
				List<ValueTuple<sbyte, int, int>> dataList = list;
				for (int i = dataList.Count - 1; i >= 0; i--)
				{
					ValueTuple<sbyte, int, int> entry = dataList[i];
					entry.Item3 -= reduceFrameDelegate(entry.Item2);
					entry.Item3 = Math.Min(entry.Item3, entry.Item2);
					bool flag = entry.Item3 <= 0;
					if (flag)
					{
						this._removedList.Add(new ValueTuple<sbyte, sbyte>(bodyPart, entry.Item1));
						dataList.RemoveAt(i);
						sbyte b2 = bodyPart;
						countArray[(int)b2] = countArray[(int)b2] - 1;
						countChanged = true;
					}
					else
					{
						dataList[i] = entry;
					}
					dataChanged = true;
				}
			}
			return new FlawOrAcupointCollection.ReduceKeepTimeResult
			{
				DataChanged = dataChanged,
				CountChanged = countChanged,
				RemovedList = this._removedList
			};
		}

		// Token: 0x060066D0 RID: 26320 RVA: 0x003AE7A4 File Offset: 0x003AC9A4
		public bool OfflineRecoverKeepTimePercent(int recoverPercent)
		{
			bool flag = recoverPercent <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool anyChanged = false;
				foreach (List<ValueTuple<sbyte, int, int>> dataList in this.BodyPartDict.Values)
				{
					for (int i = 0; i < dataList.Count; i++)
					{
						anyChanged = true;
						ValueTuple<sbyte, int, int> entry = dataList[i];
						entry.Item3 = Math.Clamp(entry.Item3 + entry.Item2 * recoverPercent / 100, 0, entry.Item2);
						dataList[i] = entry;
					}
				}
				result = anyChanged;
			}
			return result;
		}

		// Token: 0x060066D1 RID: 26321 RVA: 0x003AE874 File Offset: 0x003ACA74
		public int CalcAcupointParam(sbyte bodyPart)
		{
			List<ValueTuple<sbyte, int, int>> entries;
			bool flag = !this.BodyPartDict.TryGetValue(bodyPart, out entries) || entries.Count == 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int maxPercent = 0;
				foreach (ValueTuple<sbyte, int, int> entry in entries)
				{
					int percent = entry.Item3 * 100 / entry.Item2;
					maxPercent = Math.Max(maxPercent, percent);
				}
				bool flag2 = maxPercent == 0;
				if (flag2)
				{
					result = 0;
				}
				else
				{
					BodyPartItem config = BodyPart.Instance[bodyPart];
					Tester.Assert(config.AcupointParam.Length == config.AcupointTime.Length, "");
					for (int i = 0; i < config.AcupointTime.Length; i++)
					{
						int timePercent = config.AcupointTime[i];
						bool flag3 = timePercent > maxPercent;
						if (flag3)
						{
							return (i > 0) ? config.AcupointParam[i - 1] : 0;
						}
					}
					int[] acupointParam = config.AcupointParam;
					result = acupointParam[acupointParam.Length - 1];
				}
			}
			return result;
		}

		// Token: 0x060066D2 RID: 26322 RVA: 0x003AE9A4 File Offset: 0x003ACBA4
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060066D3 RID: 26323 RVA: 0x003AE9B8 File Offset: 0x003ACBB8
		public int GetSerializedSize()
		{
			int totalSize = 0;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				totalSize += 1 + 9 * this.BodyPartDict[bodyPart].Count;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060066D4 RID: 26324 RVA: 0x003AEA08 File Offset: 0x003ACC08
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				List<ValueTuple<sbyte, int, int>> dataList = this.BodyPartDict[bodyPart];
				*pCurrData = (byte)((sbyte)dataList.Count);
				pCurrData++;
				for (int i = 0; i < dataList.Count; i++)
				{
					ValueTuple<sbyte, int, int> entry = dataList[i];
					*pCurrData = (byte)entry.Item1;
					pCurrData++;
					*(int*)pCurrData = entry.Item2;
					pCurrData += 4;
					*(int*)pCurrData = entry.Item3;
					pCurrData += 4;
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060066D5 RID: 26325 RVA: 0x003AEAB8 File Offset: 0x003ACCB8
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				List<ValueTuple<sbyte, int, int>> dataList = this.BodyPartDict[bodyPart];
				sbyte count = *(sbyte*)pCurrData;
				pCurrData++;
				dataList.Clear();
				for (int i = 0; i < (int)count; i++)
				{
					ValueTuple<sbyte, int, int> entry;
					entry.Item1 = *(sbyte*)pCurrData;
					pCurrData++;
					entry.Item2 = *(int*)pCurrData;
					pCurrData += 4;
					entry.Item3 = *(int*)pCurrData;
					pCurrData += 4;
					dataList.Add(entry);
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001BF8 RID: 7160
		[TupleElementNames(new string[]
		{
			"level",
			"totalFrame",
			"leftFrame"
		})]
		public readonly SortedDictionary<sbyte, List<ValueTuple<sbyte, int, int>>> BodyPartDict;

		// Token: 0x04001BF9 RID: 7161
		[TupleElementNames(new string[]
		{
			"part",
			"level"
		})]
		private readonly List<ValueTuple<sbyte, sbyte>> _removedList = new List<ValueTuple<sbyte, sbyte>>();

		// Token: 0x02000B6E RID: 2926
		public struct ReduceKeepTimeResult
		{
			// Token: 0x040030D2 RID: 12498
			public bool DataChanged;

			// Token: 0x040030D3 RID: 12499
			public bool CountChanged;

			// Token: 0x040030D4 RID: 12500
			[TupleElementNames(new string[]
			{
				"part",
				"level"
			})]
			public List<ValueTuple<sbyte, sbyte>> RemovedList;
		}

		// Token: 0x02000B6F RID: 2927
		// (Invoke) Token: 0x06008B41 RID: 35649
		public delegate int ReduceFrameDelegate(int totalFrame);
	}
}
