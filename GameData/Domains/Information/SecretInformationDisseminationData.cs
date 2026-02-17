using System;
using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Information
{
	// Token: 0x0200067F RID: 1663
	[SerializableGameData(NotForDisplayModule = true)]
	public class SecretInformationDisseminationData : ISerializableGameData
	{
		// Token: 0x060053E4 RID: 21476 RVA: 0x002DC32F File Offset: 0x002DA52F
		public SecretInformationDisseminationData()
		{
			this.DisseminationCounts = new Dictionary<int, int>();
		}

		// Token: 0x060053E5 RID: 21477 RVA: 0x002DC344 File Offset: 0x002DA544
		public SecretInformationDisseminationData(SecretInformationDisseminationData other) : this()
		{
			this.Assign(other);
		}

		// Token: 0x060053E6 RID: 21478 RVA: 0x002DC358 File Offset: 0x002DA558
		public void Assign(SecretInformationDisseminationData other)
		{
			this.DisseminationCounts.Clear();
			foreach (KeyValuePair<int, int> pair in other.DisseminationCounts)
			{
				this.DisseminationCounts.Add(pair.Key, pair.Value);
			}
		}

		// Token: 0x060053E7 RID: 21479 RVA: 0x002DC3CC File Offset: 0x002DA5CC
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x060053E8 RID: 21480 RVA: 0x002DC3D0 File Offset: 0x002DA5D0
		public int GetSerializedSize()
		{
			int totalSize = 0;
			bool flag = this.DisseminationCounts != null;
			if (flag)
			{
				totalSize += 4 + 8 * this.DisseminationCounts.Count;
			}
			else
			{
				totalSize += 4;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053E9 RID: 21481 RVA: 0x002DC41C File Offset: 0x002DA61C
		public unsafe int Serialize(byte* pData)
		{
			byte* pCurrData = pData;
			bool flag = this.DisseminationCounts != null;
			if (flag)
			{
				int elementsCount = this.DisseminationCounts.Count;
				*(int*)pCurrData = elementsCount;
				pCurrData += 4;
				foreach (KeyValuePair<int, int> pair in this.DisseminationCounts)
				{
					*(int*)pCurrData = pair.Key;
					pCurrData += 4;
					*(int*)pCurrData = pair.Value;
					pCurrData += 4;
				}
			}
			else
			{
				*(int*)pCurrData = 0;
				pCurrData += 4;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060053EA RID: 21482 RVA: 0x002DC4D8 File Offset: 0x002DA6D8
		public unsafe int Deserialize(byte* pData)
		{
			uint elementsCount = *(uint*)pData;
			byte* pCurrData = pData + 4;
			bool flag = elementsCount > 0U;
			if (flag)
			{
				bool flag2 = this.DisseminationCounts == null;
				if (flag2)
				{
					throw new NotImplementedException();
				}
				this.DisseminationCounts.Clear();
				int i = 0;
				while ((long)i < (long)((ulong)elementsCount))
				{
					int id = *(int*)pCurrData;
					pCurrData += 4;
					int time = *(int*)pCurrData;
					pCurrData += 4;
					this.DisseminationCounts.Add(id, time);
					i++;
				}
			}
			else
			{
				IDictionary<int, int> disseminationCounts = this.DisseminationCounts;
				if (disseminationCounts != null)
				{
					disseminationCounts.Clear();
				}
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x04001674 RID: 5748
		[SerializableGameDataField]
		public readonly IDictionary<int, int> DisseminationCounts;
	}
}
