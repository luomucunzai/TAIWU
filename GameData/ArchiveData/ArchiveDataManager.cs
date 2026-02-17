using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData.Common;
using GameData.Domains;

namespace GameData.ArchiveData
{
	// Token: 0x020008FF RID: 2303
	public static class ArchiveDataManager
	{
		// Token: 0x0600825C RID: 33372
		[DllImport("ArchiveData")]
		private unsafe static extern bool archive_data_manager_send_request(byte* pData, uint operationCount);

		// Token: 0x0600825D RID: 33373
		[DllImport("ArchiveData")]
		private unsafe static extern bool archive_data_manager_get_response(byte** ppData, uint* pOperationCount);

		// Token: 0x0600825E RID: 33374 RVA: 0x004DAE04 File Offset: 0x004D9004
		public static uint GetNextOperationId()
		{
			uint operationId = ArchiveDataManager._nextOperationId;
			ArchiveDataManager._nextOperationId += 1U;
			return operationId;
		}

		// Token: 0x0600825F RID: 33375 RVA: 0x004DAE2C File Offset: 0x004D902C
		public unsafe static int SendRequest()
		{
			bool flag = ArchiveDataManager.CurrOperationBlock.OperationCount <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				OperationAdder.Global_Dummy();
				OperationBlock block = ArchiveDataManager.CurrOperationBlock;
				ArchiveDataManager.CurrOperationBlock = new OperationBlock(65536);
				byte* pData = block.GetRawDataPointer();
				bool succeeded = ArchiveDataManager.archive_data_manager_send_request(pData, (uint)block.OperationCount);
				bool flag2 = !succeeded;
				if (flag2)
				{
					throw new Exception("ArchiveDataManager.SendRequest: " + Common.GetLastError());
				}
				result = block.OperationCount;
			}
			return result;
		}

		// Token: 0x06008260 RID: 33376 RVA: 0x004DAEB0 File Offset: 0x004D90B0
		public unsafe static int GetResponse()
		{
			byte* pData;
			uint operationCount;
			bool succeeded = ArchiveDataManager.archive_data_manager_get_response(&pData, &operationCount);
			bool flag = !succeeded;
			if (flag)
			{
				throw new Exception("ArchiveDataManager.GetResponse: " + Common.GetLastError());
			}
			bool flag2 = operationCount <= 0U;
			int result;
			if (flag2)
			{
				result = 0;
			}
			else
			{
				bool flag3 = ArchiveDataManager.PendingOperations.Count > 0;
				if (flag3)
				{
					PendingOperation currPendingOperation = ArchiveDataManager.PendingOperations.Peek();
					byte* pCurrData = pData;
					for (uint i = 0U; i < operationCount; i += 1U)
					{
						uint operationId = *(uint*)pCurrData;
						pCurrData += 4;
						byte* pResult = *(IntPtr*)pCurrData;
						pCurrData += sizeof(byte*);
						bool flag4 = currPendingOperation.OperationId != operationId;
						if (!flag4)
						{
							OperationWrapper currOperation = currPendingOperation.Block.GetOperation(currPendingOperation.Offset);
							BaseGameDataDomain domain = DomainManager.Domains[(int)currOperation.DomainId];
							domain.ProcessArchiveResponse(currOperation, pResult);
							bool flag5 = pResult != null;
							if (flag5)
							{
								Common.FreeMemory(pResult);
							}
							ArchiveDataManager.PendingOperations.Dequeue();
							bool flag6 = ArchiveDataManager.PendingOperations.Count <= 0;
							if (flag6)
							{
								break;
							}
							currPendingOperation = ArchiveDataManager.PendingOperations.Peek();
						}
					}
				}
				Common.FreeMemory(pData);
				result = (int)operationCount;
			}
			return result;
		}

		// Token: 0x04002474 RID: 9332
		private const int OperationBlockDefaultCapacity = 65536;

		// Token: 0x04002475 RID: 9333
		private static uint _nextOperationId;

		// Token: 0x04002476 RID: 9334
		public static OperationBlock CurrOperationBlock = new OperationBlock(65536);

		// Token: 0x04002477 RID: 9335
		public static readonly Queue<PendingOperation> PendingOperations = new Queue<PendingOperation>();
	}
}
