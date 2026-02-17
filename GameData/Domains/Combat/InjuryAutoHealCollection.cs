using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C6 RID: 1734
	public class InjuryAutoHealCollection : ISerializableGameData
	{
		// Token: 0x060066DF RID: 26335 RVA: 0x003AECA0 File Offset: 0x003ACEA0
		public InjuryAutoHealCollection()
		{
			this.OuterBodyPartList = new List<short>[7];
			this.InnerBodyPartList = new List<short>[7];
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				this.OuterBodyPartList[(int)bodyPart] = new List<short>();
				this.InnerBodyPartList[(int)bodyPart] = new List<short>();
			}
		}

		// Token: 0x060066E0 RID: 26336 RVA: 0x003AECFC File Offset: 0x003ACEFC
		public void SyncInjuries(ref Injuries injuries)
		{
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				ValueTuple<sbyte, sbyte> valueTuple = injuries.Get(bodyPart);
				sbyte outer = valueTuple.Item1;
				sbyte inner = valueTuple.Item2;
				List<short> outerProgressList = this.OuterBodyPartList[(int)bodyPart];
				List<short> innerProgressList = this.InnerBodyPartList[(int)bodyPart];
				outer = Math.Max(outer, 0);
				inner = Math.Max(inner, 0);
				while ((int)outer < outerProgressList.Count)
				{
					outerProgressList.RemoveAt(0);
				}
				while ((int)outer > outerProgressList.Count)
				{
					outerProgressList.Add(0);
				}
				while ((int)inner < innerProgressList.Count)
				{
					innerProgressList.RemoveAt(0);
				}
				while ((int)inner > innerProgressList.Count)
				{
					innerProgressList.Add(0);
				}
			}
		}

		// Token: 0x060066E1 RID: 26337 RVA: 0x003AEDC4 File Offset: 0x003ACFC4
		public bool UpdateProgress(Dictionary<sbyte, OuterAndInnerInts> bodyPart2Deltas, int outerSpeed, int innerSpeed)
		{
			bodyPart2Deltas.Clear();
			bool flag = outerSpeed <= 0 && innerSpeed <= 0;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
				{
					int outerDelta = this.UpdateProgress(false, bodyPart, outerSpeed);
					int innerDelta = this.UpdateProgress(true, bodyPart, innerSpeed);
					bodyPart2Deltas[bodyPart] = new OuterAndInnerInts(outerDelta, innerDelta);
				}
				result = true;
			}
			return result;
		}

		// Token: 0x060066E2 RID: 26338 RVA: 0x003AEE30 File Offset: 0x003AD030
		private int UpdateProgress(bool isInner, sbyte bodyPart, int speed)
		{
			bool flag = speed <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int delta = 0;
				List<short> progress = (isInner ? this.InnerBodyPartList : this.OuterBodyPartList)[(int)bodyPart];
				for (int i = progress.Count - 1; i >= 0; i--)
				{
					progress[i] = (short)Math.Clamp((int)progress[i] + speed, 0, 900);
					bool flag2 = progress[i] < 900;
					if (!flag2)
					{
						delta++;
						progress.RemoveAt(i);
					}
				}
				result = delta;
			}
			return result;
		}

		// Token: 0x060066E3 RID: 26339 RVA: 0x003AEED0 File Offset: 0x003AD0D0
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060066E4 RID: 26340 RVA: 0x003AEEE4 File Offset: 0x003AD0E4
		public int GetSerializedSize()
		{
			int totalSize = 0;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				totalSize += 1 + 2 * this.OuterBodyPartList[(int)bodyPart].Count;
				totalSize += 1 + 2 * this.InnerBodyPartList[(int)bodyPart].Count;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060066E5 RID: 26341 RVA: 0x003AEF48 File Offset: 0x003AD148
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				List<short> dataList = this.OuterBodyPartList[(int)bodyPart];
				*pCurrData = (byte)((sbyte)dataList.Count);
				pCurrData++;
				for (int i = 0; i < dataList.Count; i++)
				{
					*(short*)pCurrData = dataList[i];
					pCurrData += 2;
				}
				dataList = this.InnerBodyPartList[(int)bodyPart];
				*pCurrData = (byte)((sbyte)dataList.Count);
				pCurrData++;
				for (int j = 0; j < dataList.Count; j++)
				{
					*(short*)pCurrData = dataList[j];
					pCurrData += 2;
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060066E6 RID: 26342 RVA: 0x003AF01C File Offset: 0x003AD21C
		public unsafe int Deserialize(byte* pData)
		{
			byte* pCurrData = pData;
			for (sbyte bodyPart = 0; bodyPart < 7; bodyPart += 1)
			{
				List<short> dataList = this.OuterBodyPartList[(int)bodyPart];
				sbyte count = *(sbyte*)pCurrData;
				pCurrData++;
				dataList.Clear();
				for (int i = 0; i < (int)count; i++)
				{
					short dataValue = *(short*)pCurrData;
					pCurrData += 2;
					dataList.Add(dataValue);
				}
				dataList = this.InnerBodyPartList[(int)bodyPart];
				count = *(sbyte*)pCurrData;
				pCurrData++;
				dataList.Clear();
				for (int j = 0; j < (int)count; j++)
				{
					short dataValue2 = *(short*)pCurrData;
					pCurrData += 2;
					dataList.Add(dataValue2);
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C00 RID: 7168
		public readonly List<short>[] OuterBodyPartList;

		// Token: 0x04001C01 RID: 7169
		public readonly List<short>[] InnerBodyPartList;
	}
}
