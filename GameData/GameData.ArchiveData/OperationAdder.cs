using System;
using System.Collections.Generic;
using GameData.Domains.Global;
using GameData.Domains.LifeRecord;

namespace GameData.ArchiveData;

public static class OperationAdder
{
	public unsafe static byte* FixedSingleValue_Set(ushort domainId, ushort dataId, int valueSize)
	{
		byte* result = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, valueSize, &result);
		return result;
	}

	public unsafe static uint FixedSingleValue_Get(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 1, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* DynamicSingleValue_Set(ushort domainId, ushort dataId, int valueSize)
	{
		if (valueSize > 4194304)
		{
			throw new Exception($"ValueSize must be less than {4096}KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, 4 + valueSize, &ptr);
		*(int*)ptr = valueSize;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint DynamicSingleValue_Get(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 1, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* FixedSingleValueCollection_Add<T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : unmanaged
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + elementSize, &ptr);
		*(T*)ptr = elementId;
		ptr += sizeof(T);
		return ptr;
	}

	public unsafe static byte* FixedSingleValueCollection_Set<T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : unmanaged
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, sizeof(T) + elementSize, &ptr);
		*(T*)ptr = elementId;
		ptr += sizeof(T);
		return ptr;
	}

	public unsafe static uint FixedSingleValueCollection_Get<T>(ushort domainId, ushort dataId, T elementId) where T : unmanaged
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 2, sizeof(T), (byte*)(&elementId));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static void FixedSingleValueCollection_Remove<T>(ushort domainId, ushort dataId, T elementId) where T : unmanaged
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&elementId));
	}

	public unsafe static void FixedSingleValueCollection_Clear(ushort domainId, ushort dataId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
	}

	public unsafe static uint FixedSingleValueCollection_GetAll(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 5, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* DynamicSingleValueCollection_Add<T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : unmanaged
	{
		if (elementSize > 4194304)
		{
			throw new Exception($"ElementSize must be less than {4096}KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + 4 + elementSize, &ptr);
		*(T*)ptr = elementId;
		ptr += sizeof(T);
		*(int*)ptr = elementSize;
		ptr += 4;
		return ptr;
	}

	public unsafe static byte* DynamicSingleValueCollection_Set<T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : unmanaged
	{
		if (elementSize > 4194304)
		{
			throw new Exception($"ElementSize must be less than {4096}KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, sizeof(T) + 4 + elementSize, &ptr);
		*(T*)ptr = elementId;
		ptr += sizeof(T);
		*(int*)ptr = elementSize;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint DynamicSingleValueCollection_Get<T>(ushort domainId, ushort dataId, T elementId) where T : unmanaged
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 2, sizeof(T), (byte*)(&elementId));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static void DynamicSingleValueCollection_Remove<T>(ushort domainId, ushort dataId, T elementId) where T : unmanaged
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&elementId));
	}

	public unsafe static void DynamicSingleValueCollection_Clear(ushort domainId, ushort dataId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
	}

	public unsafe static uint DynamicSingleValueCollection_GetAll(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 5, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* FixedElementList_Set(ushort domainId, ushort dataId, int index, int elementSize)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 4 + elementSize, &ptr);
		*(int*)ptr = index;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint FixedElementList_Get(ushort domainId, ushort dataId, int index)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 2, 4, (byte*)(&index));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* FixedElementList_InsertRange(ushort domainId, ushort dataId, int startIndex, int count, int elementsSize)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 8 + elementsSize, &ptr);
		*(int*)ptr = startIndex;
		ptr += 4;
		*(int*)ptr = count;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint FixedElementList_GetAll(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 8, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* DynamicElementList_Set(ushort domainId, ushort dataId, int index, int elementSize)
	{
		if (elementSize > 4194304)
		{
			throw new Exception($"ValueSize must be less than {4096}KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 8 + elementSize, &ptr);
		*(int*)ptr = index;
		ptr += 4;
		*(int*)ptr = elementSize;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint DynamicElementList_Get(ushort domainId, ushort dataId, int index)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 2, 4, (byte*)(&index));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* DynamicElementList_InsertRange(ushort domainId, ushort dataId, int startIndex, int count, int elementsDataSize)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 8 + elementsDataSize, &ptr);
		*(int*)ptr = startIndex;
		ptr += 4;
		*(int*)ptr = count;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint DynamicElementList_GetAll(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 8, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* FixedObjectCollection_Add<T>(ushort domainId, ushort dataId, T objectId, int objectDataSize) where T : unmanaged
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + objectDataSize, &ptr);
		*(T*)ptr = objectId;
		ptr += sizeof(T);
		return ptr;
	}

	public unsafe static void FixedObjectCollection_Remove<T>(ushort domainId, ushort dataId, T objectId) where T : unmanaged
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&objectId));
	}

	public unsafe static void FixedObjectCollection_Clear(ushort domainId, ushort dataId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
	}

	public unsafe static byte* FixedObjectCollection_SetFixedField<T>(ushort domainId, ushort dataId, T objectId, uint valueOffset, int valueSize) where T : unmanaged
	{
		if (valueSize > 65535)
		{
			throw new Exception("ValueSize must be less than 64KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 5, sizeof(T) + 4 + 2 + valueSize, &ptr);
		*(T*)ptr = objectId;
		ptr += sizeof(T);
		*(uint*)ptr = valueOffset;
		ptr += 4;
		*(ushort*)ptr = (ushort)valueSize;
		ptr += 2;
		return ptr;
	}

	public unsafe static uint FixedObjectCollection_GetAllIds(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 7, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static uint FixedObjectCollection_GetAllObjects(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 8, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* DynamicObjectCollection_Add<T>(ushort domainId, ushort dataId, T objectId, int objectDataSize) where T : unmanaged
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + objectDataSize, &ptr);
		*(T*)ptr = objectId;
		ptr += sizeof(T);
		return ptr;
	}

	public unsafe static void DynamicObjectCollection_Remove<T>(ushort domainId, ushort dataId, T objectId) where T : unmanaged
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&objectId));
	}

	public unsafe static void DynamicObjectCollection_Clear(ushort domainId, ushort dataId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
	}

	public unsafe static byte* DynamicObjectCollection_SetFixedField<T>(ushort domainId, ushort dataId, T objectId, uint valueOffset, int valueSize) where T : unmanaged
	{
		if (valueSize > 65535)
		{
			throw new Exception("ValueSize must be less than 64KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 5, sizeof(T) + 4 + 2 + valueSize, &ptr);
		*(T*)ptr = objectId;
		ptr += sizeof(T);
		*(uint*)ptr = valueOffset;
		ptr += 4;
		*(ushort*)ptr = (ushort)valueSize;
		ptr += 2;
		return ptr;
	}

	public unsafe static byte* DynamicObjectCollection_SetDynamicField<T>(ushort domainId, ushort dataId, T objectId, ushort dynamicDataId, int valueSize) where T : unmanaged
	{
		if (valueSize > 4194304)
		{
			throw new Exception($"ValueSize must be less than {4096}KB");
		}
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 7, sizeof(T) + 2 + 4 + valueSize, &ptr);
		*(T*)ptr = objectId;
		ptr += sizeof(T);
		*(ushort*)ptr = dynamicDataId;
		ptr += 2;
		*(int*)ptr = valueSize;
		ptr += 4;
		return ptr;
	}

	public unsafe static uint DynamicObjectCollection_GetAllIds(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 9, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static uint DynamicObjectCollection_GetAllObjects(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 10, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static byte* Binary_Insert(ushort domainId, ushort dataId, int offset, int size)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, 8 + size, &ptr);
		*(int*)ptr = offset;
		ptr += 4;
		*(int*)ptr = size;
		ptr += 4;
		return ptr;
	}

	public unsafe static byte* Binary_Write(ushort domainId, ushort dataId, int offset, int size)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 8 + size, &ptr);
		*(int*)ptr = offset;
		ptr += 4;
		*(int*)ptr = size;
		ptr += 4;
		return ptr;
	}

	public unsafe static void Binary_Remove(ushort domainId, ushort dataId, int offset, int size)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 2, 8, &ptr);
		*(int*)ptr = offset;
		ptr += 4;
		*(int*)ptr = size;
		ptr += 4;
	}

	public unsafe static byte* Binary_SetMetadata(ushort domainId, ushort dataId, ushort size)
	{
		byte* result = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, size, &result);
		return result;
	}

	public unsafe static uint Binary_Get(ushort domainId, ushort dataId)
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 0, domainId, dataId, 4, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
		return nextOperationId;
	}

	public unsafe static void Global_FreeMemory(byte* pointer)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 0, sizeof(byte*), (byte*)(&pointer));
	}

	public unsafe static void Global_EnterNewWorld(sbyte archiveId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 1, 1, (byte*)(&archiveId));
	}

	public unsafe static void Global_LoadWorld(sbyte archiveId, long backupTimestamp)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 2, 9, &ptr);
		*ptr = (byte)archiveId;
		ptr++;
		*(long*)ptr = backupTimestamp;
		ptr += 8;
	}

	public unsafe static void Global_SaveWorld(WorldInfo worldInfo, sbyte maxBackupsCount, sbyte compressionLevel)
	{
		int serializedSize = worldInfo.GetSerializedSize();
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(nextOperationId, 2, 0, 0, 3, 4 + serializedSize + 1 + 1, &ptr);
		*(int*)ptr = serializedSize;
		ptr += 4;
		ptr += worldInfo.Serialize(ptr);
		*ptr = (byte)maxBackupsCount;
		ptr++;
		*ptr = (byte)compressionLevel;
		ptr++;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void Global_LeaveWorld()
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 4, 0, null);
	}

	public unsafe static void Global_GetArchivesInfo(bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 2, 0, 0, 5, 0, null);
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void Global_DeleteArchive(sbyte archiveId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 6, 1, (byte*)(&archiveId));
	}

	public unsafe static void Global_LoadEnding(sbyte archiveId)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 8, 1, &ptr);
		*ptr = (byte)archiveId;
		ptr++;
	}

	public unsafe static void Global_SaveEnding(WorldInfo worldInfo, sbyte compressionLevel)
	{
		int serializedSize = worldInfo.GetSerializedSize();
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(nextOperationId, 2, 0, 0, 7, 4 + serializedSize, &ptr);
		*(int*)ptr = serializedSize;
		ptr += 4;
		ptr += worldInfo.Serialize(ptr);
		*ptr = (byte)compressionLevel;
		ptr++;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void Global_GetEndingArchiveInfo(sbyte archiveId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 9, 1, (byte*)(&archiveId));
	}

	public unsafe static void Global_Dummy()
	{
		uint nextOperationId = ArchiveDataManager.GetNextOperationId();
		byte* ptr = null;
		int offset = ArchiveDataManager.CurrOperationBlock.Add(nextOperationId, 2, 0, 0, 0, sizeof(byte*), (byte*)(&ptr));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(nextOperationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_Add(LifeRecordCollection collection)
	{
		byte* ptr = default(byte*);
		ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 0, 8 + collection.Size, &ptr);
		*(int*)ptr = collection.Count;
		ptr += 4;
		*(int*)ptr = collection.Size;
		ptr += 4;
		fixed (byte* rawData = collection.RawData)
		{
			Buffer.MemoryCopy(rawData, ptr, collection.Size, collection.Size);
		}
		ptr += collection.Size;
	}

	public unsafe static void LifeRecord_Get(int charId, int beginIndex, int count, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 1, 12, &ptr);
		*(int*)ptr = charId;
		ptr += 4;
		*(int*)ptr = beginIndex;
		ptr += 4;
		*(int*)ptr = count;
		ptr += 4;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_GetByDate(int charId, int startDate, int monthCount, int currDate, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 2, 16, &ptr);
		*(int*)ptr = charId;
		ptr += 4;
		*(int*)ptr = startDate;
		ptr += 4;
		*(int*)ptr = monthCount;
		ptr += 4;
		*(int*)ptr = currDate;
		ptr += 4;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_GetLast(int charId, int count, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 3, 8, &ptr);
		*(int*)ptr = charId;
		ptr += 4;
		*(int*)ptr = count;
		ptr += 4;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_Search(int charId, int date, List<short> types, int relatedCharId, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		int count = types.Count;
		byte* ptr = default(byte*);
		int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 4, 10 + 2 * count + 4, &ptr);
		*(int*)ptr = charId;
		ptr += 4;
		*(int*)ptr = date;
		ptr += 4;
		*(short*)ptr = (short)count;
		ptr += 2;
		for (int i = 0; i < count; i++)
		{
			*(short*)ptr = types[i];
			ptr += 2;
		}
		*(int*)ptr = relatedCharId;
		ptr += 4;
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_Remove(int charId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 5, 4, (byte*)(&charId));
	}

	public unsafe static void LifeRecord_GenerateDead(int charId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 6, 4, (byte*)(&charId));
	}

	public unsafe static void LifeRecord_GetDead(int charId, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, 13, 0, 7, 4, (byte*)(&charId));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}

	public unsafe static void LifeRecord_RemoveDead(int charId)
	{
		ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 8, 4, (byte*)(&charId));
	}

	public unsafe static void LifeRecord_GetAllByChar(int charId, bool isPassthrough, uint operationId = 0u)
	{
		if (!isPassthrough)
		{
			operationId = ArchiveDataManager.GetNextOperationId();
		}
		int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, 13, 0, 11, 4, (byte*)(&charId));
		ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
	}
}
