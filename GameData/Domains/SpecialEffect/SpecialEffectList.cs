using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect
{
	// Token: 0x020000E3 RID: 227
	public class SpecialEffectList : ISerializableGameData
	{
		// Token: 0x0600294F RID: 10575 RVA: 0x001FB505 File Offset: 0x001F9705
		public SpecialEffectList()
		{
			this.EffectList = new List<SpecialEffectBase>();
		}

		// Token: 0x06002950 RID: 10576 RVA: 0x001FB51C File Offset: 0x001F971C
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06002951 RID: 10577 RVA: 0x001FB530 File Offset: 0x001F9730
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += 2;
			bool flag = this.EffectList != null;
			if (flag)
			{
				for (int i = 0; i < this.EffectList.Count; i++)
				{
					totalSize += 4;
					totalSize += this.EffectList[i].GetSerializedSize();
				}
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06002952 RID: 10578 RVA: 0x001FB5A0 File Offset: 0x001F97A0
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.EffectList != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.EffectList.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					SpecialEffectBase effect = this.EffectList[i];
					*(int*)pCurrData = effect.Type;
					pCurrData += 4;
					pCurrData += this.EffectList[i].Serialize(pCurrData);
				}
			}
			else
			{
				*(short*)pData = 0;
				pCurrData = pData + 2;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06002953 RID: 10579 RVA: 0x001FB660 File Offset: 0x001F9860
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				if (this.EffectList == null)
				{
					this.EffectList = new List<SpecialEffectBase>();
				}
				this.EffectList.Clear();
				for (int i = 0; i < (int)elementsCount; i++)
				{
					int type = *(int*)pCurrData;
					pCurrData += 4;
					SpecialEffectBase effect = SpecialEffectType.CreateEffectObj(type);
					pCurrData += effect.Deserialize(pCurrData);
					this.EffectList.Add(effect);
				}
				pCurrData += elementsCount;
			}
			else
			{
				List<SpecialEffectBase> effectList = this.EffectList;
				if (effectList != null)
				{
					effectList.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400083C RID: 2108
		public List<SpecialEffectBase> EffectList;
	}
}
