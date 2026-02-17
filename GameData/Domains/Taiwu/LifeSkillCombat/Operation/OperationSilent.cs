using System;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x0200006A RID: 106
	public class OperationSilent : OperationBase
	{
		// Token: 0x060015DC RID: 5596 RVA: 0x0014C9C8 File Offset: 0x0014ABC8
		public OperationSilent()
		{
		}

		// Token: 0x060015DD RID: 5597 RVA: 0x0014C9D2 File Offset: 0x0014ABD2
		public override string Inspect()
		{
			return string.Empty;
		}

		// Token: 0x060015DE RID: 5598 RVA: 0x0014C9DC File Offset: 0x0014ABDC
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015DF RID: 5599 RVA: 0x0014CA08 File Offset: 0x0014AC08
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015E0 RID: 5600 RVA: 0x0014CA40 File Offset: 0x0014AC40
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015E1 RID: 5601 RVA: 0x0014CA76 File Offset: 0x0014AC76
		public OperationSilent(sbyte playerId, int stamp) : base(playerId, stamp)
		{
		}
	}
}
