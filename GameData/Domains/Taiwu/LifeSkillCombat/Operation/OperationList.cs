using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000061 RID: 97
	[SerializableGameData(NotForDisplayModule = true)]
	public class OperationList : List<OperationBase>, ISerializableGameData
	{
		// Token: 0x0600158B RID: 5515 RVA: 0x0014B376 File Offset: 0x00149576
		public bool IsSerializedSizeFixed()
		{
			return false;
		}

		// Token: 0x0600158C RID: 5516 RVA: 0x0014B37C File Offset: 0x0014957C
		public int GetSerializedSize()
		{
			int totalSize = 0;
			totalSize += 2;
			int i = 0;
			int len = base.Count;
			while (i < len)
			{
				totalSize += OperationBase.GetSerializeSizeWithPolymorphism(base[i]);
				i++;
			}
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600158D RID: 5517 RVA: 0x0014B3D0 File Offset: 0x001495D0
		public unsafe int Serialize(byte* pData)
		{
			Tester.Assert(base.Count <= 65535, "");
			*(short*)pData = (short)((ushort)base.Count);
			byte* pCurrData = pData + 2;
			int i = 0;
			int len = base.Count;
			while (i < len)
			{
				pCurrData += OperationBase.SerializeWithPolymorphism(base[i], pCurrData);
				i++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600158E RID: 5518 RVA: 0x0014B454 File Offset: 0x00149654
		public unsafe int Deserialize(byte* pData)
		{
			ushort count = *(ushort*)pData;
			byte* pCurrData = pData + 2;
			base.Clear();
			int i = 0;
			int len = (int)count;
			while (i < len)
			{
				OperationBase element = null;
				pCurrData += OperationBase.DeserializeWithPolymorphism(ref element, pCurrData);
				base.Add(element);
				i++;
			}
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}
	}
}
