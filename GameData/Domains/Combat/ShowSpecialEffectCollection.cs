using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat
{
	// Token: 0x020006D0 RID: 1744
	[SerializableGameData]
	public class ShowSpecialEffectCollection : ISerializableGameData
	{
		// Token: 0x06006734 RID: 26420 RVA: 0x003B0858 File Offset: 0x003AEA58
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06006735 RID: 26421 RVA: 0x003B086C File Offset: 0x003AEA6C
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.ShowEffectList != null;
			if (flag)
			{
				totalSize += 2;
				int elementsCount = this.ShowEffectList.Count;
				for (int i = 0; i < elementsCount; i++)
				{
					totalSize += this.ShowEffectList[i].GetSerializedSize();
				}
			}
			else
			{
				totalSize += 2;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06006736 RID: 26422 RVA: 0x003B08E8 File Offset: 0x003AEAE8
		public unsafe int Serialize(byte* pData)
		{
			bool flag = this.ShowEffectList != null;
			byte* pCurrData;
			if (flag)
			{
				int elementsCount = this.ShowEffectList.Count;
				Tester.Assert(elementsCount <= 65535, "");
				*(short*)pData = (short)((ushort)elementsCount);
				pCurrData = pData + 2;
				for (int i = 0; i < elementsCount; i++)
				{
					int subDataSize = this.ShowEffectList[i].Serialize(pCurrData);
					pCurrData += subDataSize;
					Tester.Assert(subDataSize <= 65535, "");
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

		// Token: 0x06006737 RID: 26423 RVA: 0x003B09AC File Offset: 0x003AEBAC
		public unsafe int Deserialize(byte* pData)
		{
			ushort elementsCount = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			bool flag = elementsCount > 0;
			if (flag)
			{
				bool flag2 = this.ShowEffectList == null;
				if (flag2)
				{
					this.ShowEffectList = new List<ShowSpecialEffectDisplayData>((int)elementsCount);
				}
				else
				{
					this.ShowEffectList.Clear();
				}
				for (int i = 0; i < (int)elementsCount; i++)
				{
					ShowSpecialEffectDisplayData element = default(ShowSpecialEffectDisplayData);
					pCurrData += element.Deserialize(pCurrData);
					this.ShowEffectList.Add(element);
				}
			}
			else
			{
				List<ShowSpecialEffectDisplayData> showEffectList = this.ShowEffectList;
				if (showEffectList != null)
				{
					showEffectList.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001C18 RID: 7192
		[SerializableGameDataField]
		public List<ShowSpecialEffectDisplayData> ShowEffectList = new List<ShowSpecialEffectDisplayData>();
	}
}
