using System;

namespace GameData.ArchiveData
{
	// Token: 0x02000905 RID: 2309
	public readonly struct PendingOperation
	{
		// Token: 0x060082E3 RID: 33507 RVA: 0x004DCC18 File Offset: 0x004DAE18
		public PendingOperation(uint operationId, int offset, OperationBlock block)
		{
			this.OperationId = operationId;
			this.Offset = offset;
			this.Block = block;
		}

		// Token: 0x0400248E RID: 9358
		public readonly uint OperationId;

		// Token: 0x0400248F RID: 9359
		public readonly int Offset;

		// Token: 0x04002490 RID: 9360
		public readonly OperationBlock Block;
	}
}
