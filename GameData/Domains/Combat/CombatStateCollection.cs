using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x020006BD RID: 1725
	public class CombatStateCollection : ISerializableGameData
	{
		// Token: 0x06006657 RID: 26199 RVA: 0x003AB8CC File Offset: 0x003A9ACC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006658 RID: 26200 RVA: 0x003AB8E0 File Offset: 0x003A9AE0
		public int GetSerializedSize()
		{
			return 2 + 9 * this.StateDict.Count;
		}

		// Token: 0x06006659 RID: 26201 RVA: 0x003AB904 File Offset: 0x003A9B04
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			*(short*)pCurrData = (short)this.StateDict.Count;
			pCurrData += 2;
			foreach (KeyValuePair<short, ValueTuple<short, bool, int>> entry in this.StateDict)
			{
				*(short*)pCurrData = entry.Key;
				pCurrData += 2;
				*(short*)pCurrData = entry.Value.Item1;
				pCurrData += 2;
				*pCurrData = (entry.Value.Item2 ? 1 : 0);
				pCurrData++;
				*(int*)pCurrData = entry.Value.Item3;
				pCurrData += 4;
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x0600665A RID: 26202 RVA: 0x003AB9B8 File Offset: 0x003A9BB8
		public unsafe int Deserialize(byte* pData)
		{
			short count = *(short*)pData;
			byte* pCurrData = pData + 2;
			this.StateDict.Clear();
			for (int i = 0; i < (int)count; i++)
			{
				short key = *(short*)pCurrData;
				pCurrData += 2;
				short power = *(short*)pCurrData;
				pCurrData += 2;
				bool reverse = *pCurrData != 0;
				pCurrData++;
				int srcCharId = *(int*)pCurrData;
				pCurrData += 4;
				this.StateDict.Add(key, new ValueTuple<short, bool, int>(power, reverse, srcCharId));
			}
			return (int)((long)(pCurrData - pData));
		}

		// Token: 0x04001BCE RID: 7118
		[TupleElementNames(new string[]
		{
			"power",
			"reverse",
			"srcCharId"
		})]
		public readonly Dictionary<short, ValueTuple<short, bool, int>> StateDict = new Dictionary<short, ValueTuple<short, bool, int>>();

		// Token: 0x04001BCF RID: 7119
		public readonly Dictionary<short, long> State2EffectId = new Dictionary<short, long>();
	}
}
