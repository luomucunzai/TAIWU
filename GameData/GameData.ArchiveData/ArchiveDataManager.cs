using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using GameData.Common;
using GameData.Domains;

namespace GameData.ArchiveData;

public static class ArchiveDataManager
{
	private const int OperationBlockDefaultCapacity = 65536;

	private static uint _nextOperationId;

	public static OperationBlock CurrOperationBlock = new OperationBlock(65536);

	public static readonly Queue<PendingOperation> PendingOperations = new Queue<PendingOperation>();

	[DllImport("ArchiveData")]
	private unsafe static extern bool archive_data_manager_send_request(byte* pData, uint operationCount);

	[DllImport("ArchiveData")]
	private unsafe static extern bool archive_data_manager_get_response(byte** ppData, uint* pOperationCount);

	public static uint GetNextOperationId()
	{
		uint nextOperationId = _nextOperationId;
		_nextOperationId++;
		return nextOperationId;
	}

	public unsafe static int SendRequest()
	{
		if (CurrOperationBlock.OperationCount <= 0)
		{
			return 0;
		}
		OperationAdder.Global_Dummy();
		OperationBlock currOperationBlock = CurrOperationBlock;
		CurrOperationBlock = new OperationBlock(65536);
		byte* rawDataPointer = currOperationBlock.GetRawDataPointer();
		if (!archive_data_manager_send_request(rawDataPointer, (uint)currOperationBlock.OperationCount))
		{
			throw new Exception("ArchiveDataManager.SendRequest: " + Common.GetLastError());
		}
		return currOperationBlock.OperationCount;
	}

	public unsafe static int GetResponse()
	{
		byte* ptr = default(byte*);
		uint num = default(uint);
		if (!archive_data_manager_get_response(&ptr, &num))
		{
			throw new Exception("ArchiveDataManager.GetResponse: " + Common.GetLastError());
		}
		if (num == 0)
		{
			return 0;
		}
		if (PendingOperations.Count > 0)
		{
			PendingOperation pendingOperation = PendingOperations.Peek();
			byte* ptr2 = ptr;
			for (uint num2 = 0u; num2 < num; num2++)
			{
				uint num3 = *(uint*)ptr2;
				ptr2 += 4;
				byte* ptr3 = *(byte**)ptr2;
				ptr2 += sizeof(byte*);
				if (pendingOperation.OperationId == num3)
				{
					OperationWrapper operation = pendingOperation.Block.GetOperation(pendingOperation.Offset);
					BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[operation.DomainId];
					baseGameDataDomain.ProcessArchiveResponse(operation, ptr3);
					if (ptr3 != null)
					{
						Common.FreeMemory(ptr3);
					}
					PendingOperations.Dequeue();
					if (PendingOperations.Count <= 0)
					{
						break;
					}
					pendingOperation = PendingOperations.Peek();
				}
			}
		}
		Common.FreeMemory(ptr);
		return (int)num;
	}
}
