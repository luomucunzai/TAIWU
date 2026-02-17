using System;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000063 RID: 99
	public class OperationGiveUp : OperationBase
	{
		// Token: 0x06001598 RID: 5528 RVA: 0x0014B74A File Offset: 0x0014994A
		public OperationGiveUp()
		{
		}

		// Token: 0x06001599 RID: 5529 RVA: 0x0014B754 File Offset: 0x00149954
		public override string Inspect()
		{
			return string.Empty;
		}

		// Token: 0x0600159A RID: 5530 RVA: 0x0014B75C File Offset: 0x0014995C
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600159B RID: 5531 RVA: 0x0014B788 File Offset: 0x00149988
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600159C RID: 5532 RVA: 0x0014B7C0 File Offset: 0x001499C0
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x0600159D RID: 5533 RVA: 0x0014B7F6 File Offset: 0x001499F6
		public OperationGiveUp(sbyte playerId, int stamp) : base(playerId, stamp)
		{
		}
	}
}
