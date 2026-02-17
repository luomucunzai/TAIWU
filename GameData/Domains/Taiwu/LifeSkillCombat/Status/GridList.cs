using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Status
{
	// Token: 0x0200005B RID: 91
	[SerializableGameData(NotForDisplayModule = true)]
	public class GridList : List<Grid>, ISerializableGameData
	{
		// Token: 0x06001541 RID: 5441 RVA: 0x00148BA7 File Offset: 0x00146DA7
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x06001542 RID: 5442 RVA: 0x00148BAC File Offset: 0x00146DAC
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += 2;
			int i = 0;
			int len = base.Count;
			while (i < len)
			{
				totalSize += base[i].GetSerializedSize();
				i++;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001543 RID: 5443 RVA: 0x00148C00 File Offset: 0x00146E00
		public unsafe int Serialize(byte* pData)
		{
			Tester.Assert(base.Count <= 65535, "");
			*(short*)pData = (short)((ushort)base.Count);
			byte* pCurrData = pData + 2;
			int i = 0;
			int len = base.Count;
			while (i < len)
			{
				pCurrData += base[i].Serialize(pCurrData);
				i++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001544 RID: 5444 RVA: 0x00148C84 File Offset: 0x00146E84
		public unsafe int Deserialize(byte* pData)
		{
			ushort count = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			base.Clear();
			int i = 0;
			int len = (int)count;
			while (i < len)
			{
				Grid element = new Grid();
				pCurrData += element.Deserialize(pCurrData);
				base.Add(element);
				i++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}
	}
}
