using System;
using GameData.Serializer;

namespace GameData.Domains.Organization
{
	// Token: 0x02000642 RID: 1602
	[SerializableGameData(NotForDisplayModule = true)]
	public struct MaxApprovingRateTempBonus : ISerializableGameData
	{
		// Token: 0x0600467F RID: 18047 RVA: 0x002754E6 File Offset: 0x002736E6
		public MaxApprovingRateTempBonus(short settlementId, short bonus, int expireDate)
		{
			this.SettlementId = settlementId;
			this.Bonus = bonus;
			this.ExpireDate = expireDate;
		}

		// Token: 0x06004680 RID: 18048 RVA: 0x00275500 File Offset: 0x00273700
		public bool IsSerializedSizeFixed()
		{
			return true;
		}

		// Token: 0x06004681 RID: 18049 RVA: 0x00275514 File Offset: 0x00273714
		public int GetSerializedSize()
		{
			int totalSize = 8;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06004682 RID: 18050 RVA: 0x00275538 File Offset: 0x00273738
		public unsafe int Serialize(byte* pData)
		{
			*(short*)pData = this.SettlementId;
			byte* pCurrData = pData + 2;
			*(short*)pCurrData = this.Bonus;
			pCurrData += 2;
			*(int*)pCurrData = this.ExpireDate;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06004683 RID: 18051 RVA: 0x00275588 File Offset: 0x00273788
		public unsafe int Deserialize(byte* pData)
		{
			this.SettlementId = *(short*)pData;
			byte* pCurrData = pData + 2;
			this.Bonus = *(short*)pCurrData;
			pCurrData += 2;
			this.ExpireDate = *(int*)pCurrData;
			pCurrData += 4;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x040014AB RID: 5291
		[SerializableGameDataField]
		public short SettlementId;

		// Token: 0x040014AC RID: 5292
		[SerializableGameDataField]
		public short Bonus;

		// Token: 0x040014AD RID: 5293
		[SerializableGameDataField]
		public int ExpireDate;
	}
}
