using System;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000066 RID: 102
	public class OperationPrepareForceAdversary : OperationBase
	{
		// Token: 0x17000095 RID: 149
		// (get) Token: 0x060015C1 RID: 5569 RVA: 0x0014C2D6 File Offset: 0x0014A4D6
		// (set) Token: 0x060015C2 RID: 5570 RVA: 0x0014C2DE File Offset: 0x0014A4DE
		public OperationPrepareForceAdversary.ForceAdversaryOperation Type { get; private set; }

		// Token: 0x060015C3 RID: 5571 RVA: 0x0014C2E7 File Offset: 0x0014A4E7
		public OperationPrepareForceAdversary()
		{
		}

		// Token: 0x060015C4 RID: 5572 RVA: 0x0014C2F1 File Offset: 0x0014A4F1
		public override string Inspect()
		{
			return string.Empty;
		}

		// Token: 0x060015C5 RID: 5573 RVA: 0x0014C2F8 File Offset: 0x0014A4F8
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			totalSize++;
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015C6 RID: 5574 RVA: 0x0014C328 File Offset: 0x0014A528
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			*pCurrData = (byte)this.Type;
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015C7 RID: 5575 RVA: 0x0014C36C File Offset: 0x0014A56C
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			this.Type = (OperationPrepareForceAdversary.ForceAdversaryOperation)(*(sbyte*)pCurrData);
			pCurrData++;
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015C8 RID: 5576 RVA: 0x0014C3AF File Offset: 0x0014A5AF
		public OperationPrepareForceAdversary(sbyte playerId, int stamp, OperationPrepareForceAdversary.ForceAdversaryOperation type) : base(playerId, stamp)
		{
			this.Type = type;
		}

		// Token: 0x0200098E RID: 2446
		public enum ForceAdversaryOperation : sbyte
		{
			// Token: 0x04002833 RID: 10291
			Silent,
			// Token: 0x04002834 RID: 10292
			GiveUp
		}
	}
}
