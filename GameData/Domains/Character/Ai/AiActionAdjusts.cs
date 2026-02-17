using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000847 RID: 2119
	[SerializableGameData(NotForDisplayModule = true)]
	public class AiActionAdjusts : ISerializableGameData
	{
		// Token: 0x06007609 RID: 30217 RVA: 0x0044F148 File Offset: 0x0044D348
		public AiActionAdjusts()
		{
			this.Collection = new Dictionary<AiActionKey, ValueTuple<short, int>>();
		}

		// Token: 0x0600760A RID: 30218 RVA: 0x0044F160 File Offset: 0x0044D360
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600760B RID: 30219 RVA: 0x0044F174 File Offset: 0x0044D374
		public int GetSerializedSize()
		{
			int totalSize = 2 + this.Collection.Count * 8;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600760C RID: 30220 RVA: 0x0044F1A8 File Offset: 0x0044D3A8
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			*(short*)pCurrData = (short)((ushort)this.Collection.Count);
			pCurrData += 2;
			foreach (KeyValuePair<AiActionKey, ValueTuple<short, int>> pair in this.Collection)
			{
				pCurrData += pair.Key.Serialize(pCurrData);
				*(short*)pCurrData = pair.Value.Item1;
				pCurrData += 2;
				*(int*)pCurrData = pair.Value.Item2;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600760D RID: 30221 RVA: 0x0044F264 File Offset: 0x0044D464
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			this.Collection.Clear();
			for (int i = 0; i < (int)elementCount; i++)
			{
				AiActionKey actionKey = default(AiActionKey);
				pCurrData += actionKey.Deserialize(pCurrData);
				short rateAdjust = *(short*)pCurrData;
				pCurrData += 2;
				int expireDate = *(int*)pCurrData;
				pCurrData += 4;
				this.Collection.Add(actionKey, new ValueTuple<short, int>(rateAdjust, expireDate));
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04002081 RID: 8321
		[TupleElementNames(new string[]
		{
			"RateAdjust",
			"ExpireDate"
		})]
		public readonly Dictionary<AiActionKey, ValueTuple<short, int>> Collection;
	}
}
