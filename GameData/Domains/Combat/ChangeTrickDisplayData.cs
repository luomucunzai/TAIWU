using System;
using GameData.Serializer;

namespace GameData.Domains.Combat
{
	// Token: 0x02000690 RID: 1680
	[SerializableGameData(NotForArchive = true)]
	public struct ChangeTrickDisplayData : ISerializableGameData
	{
		// Token: 0x06005F78 RID: 24440 RVA: 0x00365160 File Offset: 0x00363360
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06005F79 RID: 24441 RVA: 0x00365174 File Offset: 0x00363374
		public int GetSerializedSize()
		{
			int totalSize = 6;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005F7A RID: 24442 RVA: 0x00365198 File Offset: 0x00363398
		public unsafe int Serialize(byte* pData)
		{
			*pData = (this.CanChangeTrick ? 1 : 0);
			byte* pCurrData = pData + 1;
			*pCurrData = (byte)this.CostCount;
			pCurrData++;
			*(short*)pCurrData = this.AddHitRate;
			pCurrData += 2;
			*(short*)pCurrData = this.AddBreakBlock;
			pCurrData += 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06005F7B RID: 24443 RVA: 0x003651F4 File Offset: 0x003633F4
		public unsafe int Deserialize(byte* pData)
		{
			this.CanChangeTrick = (*pData != 0);
			byte* pCurrData = pData + 1;
			this.CostCount = *(sbyte*)pCurrData;
			pCurrData++;
			this.AddHitRate = *(short*)pCurrData;
			pCurrData += 2;
			this.AddBreakBlock = *(short*)pCurrData;
			pCurrData += 2;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0400194A RID: 6474
		[SerializableGameDataField]
		public bool CanChangeTrick;

		// Token: 0x0400194B RID: 6475
		[SerializableGameDataField]
		public sbyte CostCount;

		// Token: 0x0400194C RID: 6476
		[SerializableGameDataField]
		public short AddHitRate;

		// Token: 0x0400194D RID: 6477
		[SerializableGameDataField]
		public short AddBreakBlock;
	}
}
