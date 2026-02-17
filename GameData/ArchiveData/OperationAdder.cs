using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Global;
using GameData.Domains.LifeRecord;

namespace GameData.ArchiveData
{
	// Token: 0x02000901 RID: 2305
	public static class OperationAdder
	{
		// Token: 0x06008276 RID: 33398 RVA: 0x004DB1DC File Offset: 0x004D93DC
		public unsafe static byte* FixedSingleValue_Set(ushort domainId, ushort dataId, int valueSize)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, valueSize, &pData);
			return pData;
		}

		// Token: 0x06008277 RID: 33399 RVA: 0x004DB208 File Offset: 0x004D9408
		public static uint FixedSingleValue_Get(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 1, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008278 RID: 33400 RVA: 0x004DB24C File Offset: 0x004D944C
		public unsafe static byte* DynamicSingleValue_Set(ushort domainId, ushort dataId, int valueSize)
		{
			bool flag = valueSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ValueSize must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, 4 + valueSize, &pData);
			*(int*)pData = valueSize;
			pData += 4;
			return pData;
		}

		// Token: 0x06008279 RID: 33401 RVA: 0x004DB2CC File Offset: 0x004D94CC
		public static uint DynamicSingleValue_Get(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 1, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600827A RID: 33402 RVA: 0x004DB310 File Offset: 0x004D9510
		public unsafe static byte* FixedSingleValueCollection_Add<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : struct, ValueType
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + elementSize, &pData);
			*(T*)pData = elementId;
			pData += sizeof(T);
			return pData;
		}

		// Token: 0x0600827B RID: 33403 RVA: 0x004DB354 File Offset: 0x004D9554
		public unsafe static byte* FixedSingleValueCollection_Set<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : struct, ValueType
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, sizeof(T) + elementSize, &pData);
			*(T*)pData = elementId;
			pData += sizeof(T);
			return pData;
		}

		// Token: 0x0600827C RID: 33404 RVA: 0x004DB398 File Offset: 0x004D9598
		public unsafe static uint FixedSingleValueCollection_Get<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId) where T : struct, ValueType
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 2, sizeof(T), (byte*)(&elementId));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600827D RID: 33405 RVA: 0x004DB3E1 File Offset: 0x004D95E1
		public unsafe static void FixedSingleValueCollection_Remove<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId) where T : struct, ValueType
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&elementId));
		}

		// Token: 0x0600827E RID: 33406 RVA: 0x004DB401 File Offset: 0x004D9601
		public static void FixedSingleValueCollection_Clear(ushort domainId, ushort dataId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
		}

		// Token: 0x0600827F RID: 33407 RVA: 0x004DB41C File Offset: 0x004D961C
		public static uint FixedSingleValueCollection_GetAll(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 5, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008280 RID: 33408 RVA: 0x004DB460 File Offset: 0x004D9660
		public unsafe static byte* DynamicSingleValueCollection_Add<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : struct, ValueType
		{
			bool flag = elementSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ElementSize must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + 4 + elementSize, &pData);
			*(T*)pData = elementId;
			pData += sizeof(T);
			*(int*)pData = elementSize;
			pData += 4;
			return pData;
		}

		// Token: 0x06008281 RID: 33409 RVA: 0x004DB4F8 File Offset: 0x004D96F8
		public unsafe static byte* DynamicSingleValueCollection_Set<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId, int elementSize) where T : struct, ValueType
		{
			bool flag = elementSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(32, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ElementSize must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, sizeof(T) + 4 + elementSize, &pData);
			*(T*)pData = elementId;
			pData += sizeof(T);
			*(int*)pData = elementSize;
			pData += 4;
			return pData;
		}

		// Token: 0x06008282 RID: 33410 RVA: 0x004DB590 File Offset: 0x004D9790
		public unsafe static uint DynamicSingleValueCollection_Get<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId) where T : struct, ValueType
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 2, sizeof(T), (byte*)(&elementId));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008283 RID: 33411 RVA: 0x004DB5D9 File Offset: 0x004D97D9
		public unsafe static void DynamicSingleValueCollection_Remove<[IsUnmanaged] T>(ushort domainId, ushort dataId, T elementId) where T : struct, ValueType
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&elementId));
		}

		// Token: 0x06008284 RID: 33412 RVA: 0x004DB5F9 File Offset: 0x004D97F9
		public static void DynamicSingleValueCollection_Clear(ushort domainId, ushort dataId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
		}

		// Token: 0x06008285 RID: 33413 RVA: 0x004DB614 File Offset: 0x004D9814
		public static uint DynamicSingleValueCollection_GetAll(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 5, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008286 RID: 33414 RVA: 0x004DB658 File Offset: 0x004D9858
		public unsafe static byte* FixedElementList_Set(ushort domainId, ushort dataId, int index, int elementSize)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 4 + elementSize, &pData);
			*(int*)pData = index;
			pData += 4;
			return pData;
		}

		// Token: 0x06008287 RID: 33415 RVA: 0x004DB68C File Offset: 0x004D988C
		public unsafe static uint FixedElementList_Get(ushort domainId, ushort dataId, int index)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 2, 4, (byte*)(&index));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008288 RID: 33416 RVA: 0x004DB6D0 File Offset: 0x004D98D0
		public unsafe static byte* FixedElementList_InsertRange(ushort domainId, ushort dataId, int startIndex, int count, int elementsSize)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 8 + elementsSize, &pData);
			*(int*)pData = startIndex;
			pData += 4;
			*(int*)pData = count;
			pData += 4;
			return pData;
		}

		// Token: 0x06008289 RID: 33417 RVA: 0x004DB70C File Offset: 0x004D990C
		public static uint FixedElementList_GetAll(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 8, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600828A RID: 33418 RVA: 0x004DB750 File Offset: 0x004D9950
		public unsafe static byte* DynamicElementList_Set(ushort domainId, ushort dataId, int index, int elementSize)
		{
			bool flag = elementSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ValueSize must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 8 + elementSize, &pData);
			*(int*)pData = index;
			pData += 4;
			*(int*)pData = elementSize;
			pData += 4;
			return pData;
		}

		// Token: 0x0600828B RID: 33419 RVA: 0x004DB7D8 File Offset: 0x004D99D8
		public unsafe static uint DynamicElementList_Get(ushort domainId, ushort dataId, int index)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 2, 4, (byte*)(&index));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600828C RID: 33420 RVA: 0x004DB81C File Offset: 0x004D9A1C
		public unsafe static byte* DynamicElementList_InsertRange(ushort domainId, ushort dataId, int startIndex, int count, int elementsDataSize)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 8 + elementsDataSize, &pData);
			*(int*)pData = startIndex;
			pData += 4;
			*(int*)pData = count;
			pData += 4;
			return pData;
		}

		// Token: 0x0600828D RID: 33421 RVA: 0x004DB858 File Offset: 0x004D9A58
		public static uint DynamicElementList_GetAll(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 8, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600828E RID: 33422 RVA: 0x004DB89C File Offset: 0x004D9A9C
		public unsafe static byte* FixedObjectCollection_Add<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId, int objectDataSize) where T : struct, ValueType
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + objectDataSize, &pData);
			*(T*)pData = objectId;
			pData += sizeof(T);
			return pData;
		}

		// Token: 0x0600828F RID: 33423 RVA: 0x004DB8DE File Offset: 0x004D9ADE
		public unsafe static void FixedObjectCollection_Remove<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId) where T : struct, ValueType
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&objectId));
		}

		// Token: 0x06008290 RID: 33424 RVA: 0x004DB8FE File Offset: 0x004D9AFE
		public static void FixedObjectCollection_Clear(ushort domainId, ushort dataId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
		}

		// Token: 0x06008291 RID: 33425 RVA: 0x004DB918 File Offset: 0x004D9B18
		public unsafe static byte* FixedObjectCollection_SetFixedField<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId, uint valueOffset, int valueSize) where T : struct, ValueType
		{
			bool flag = valueSize > 65535;
			if (flag)
			{
				throw new Exception("ValueSize must be less than 64KB");
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 5, sizeof(T) + 4 + 2 + valueSize, &pData);
			*(T*)pData = objectId;
			pData += sizeof(T);
			*(int*)pData = (int)valueOffset;
			pData += 4;
			*(short*)pData = (short)((ushort)valueSize);
			pData += 2;
			return pData;
		}

		// Token: 0x06008292 RID: 33426 RVA: 0x004DB988 File Offset: 0x004D9B88
		public static uint FixedObjectCollection_GetAllIds(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 7, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008293 RID: 33427 RVA: 0x004DB9CC File Offset: 0x004D9BCC
		public static uint FixedObjectCollection_GetAllObjects(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 8, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x06008294 RID: 33428 RVA: 0x004DBA10 File Offset: 0x004D9C10
		public unsafe static byte* DynamicObjectCollection_Add<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId, int objectDataSize) where T : struct, ValueType
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, sizeof(T) + objectDataSize, &pData);
			*(T*)pData = objectId;
			pData += sizeof(T);
			return pData;
		}

		// Token: 0x06008295 RID: 33429 RVA: 0x004DBA52 File Offset: 0x004D9C52
		public unsafe static void DynamicObjectCollection_Remove<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId) where T : struct, ValueType
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, sizeof(T), (byte*)(&objectId));
		}

		// Token: 0x06008296 RID: 33430 RVA: 0x004DBA72 File Offset: 0x004D9C72
		public static void DynamicObjectCollection_Clear(ushort domainId, ushort dataId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 4, 0, null);
		}

		// Token: 0x06008297 RID: 33431 RVA: 0x004DBA8C File Offset: 0x004D9C8C
		public unsafe static byte* DynamicObjectCollection_SetFixedField<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId, uint valueOffset, int valueSize) where T : struct, ValueType
		{
			bool flag = valueSize > 65535;
			if (flag)
			{
				throw new Exception("ValueSize must be less than 64KB");
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 5, sizeof(T) + 4 + 2 + valueSize, &pData);
			*(T*)pData = objectId;
			pData += sizeof(T);
			*(int*)pData = (int)valueOffset;
			pData += 4;
			*(short*)pData = (short)((ushort)valueSize);
			pData += 2;
			return pData;
		}

		// Token: 0x06008298 RID: 33432 RVA: 0x004DBAFC File Offset: 0x004D9CFC
		public unsafe static byte* DynamicObjectCollection_SetDynamicField<[IsUnmanaged] T>(ushort domainId, ushort dataId, T objectId, ushort dynamicDataId, int valueSize) where T : struct, ValueType
		{
			bool flag = valueSize > 4194304;
			if (flag)
			{
				DefaultInterpolatedStringHandler defaultInterpolatedStringHandler = new DefaultInterpolatedStringHandler(30, 1);
				defaultInterpolatedStringHandler.AppendLiteral("ValueSize must be less than ");
				defaultInterpolatedStringHandler.AppendFormatted<int>(4096);
				defaultInterpolatedStringHandler.AppendLiteral("KB");
				throw new Exception(defaultInterpolatedStringHandler.ToStringAndClear());
			}
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 7, sizeof(T) + 2 + 4 + valueSize, &pData);
			*(T*)pData = objectId;
			pData += sizeof(T);
			*(short*)pData = (short)dynamicDataId;
			pData += 2;
			*(int*)pData = valueSize;
			pData += 4;
			return pData;
		}

		// Token: 0x06008299 RID: 33433 RVA: 0x004DBBA0 File Offset: 0x004D9DA0
		public static uint DynamicObjectCollection_GetAllIds(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 9, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600829A RID: 33434 RVA: 0x004DBBE4 File Offset: 0x004D9DE4
		public static uint DynamicObjectCollection_GetAllObjects(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 10, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x0600829B RID: 33435 RVA: 0x004DBC28 File Offset: 0x004D9E28
		public unsafe static byte* Binary_Insert(ushort domainId, ushort dataId, int offset, int size)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 0, 8 + size, &pData);
			*(int*)pData = offset;
			pData += 4;
			*(int*)pData = size;
			pData += 4;
			return pData;
		}

		// Token: 0x0600829C RID: 33436 RVA: 0x004DBC64 File Offset: 0x004D9E64
		public unsafe static byte* Binary_Write(ushort domainId, ushort dataId, int offset, int size)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 1, 8 + size, &pData);
			*(int*)pData = offset;
			pData += 4;
			*(int*)pData = size;
			pData += 4;
			return pData;
		}

		// Token: 0x0600829D RID: 33437 RVA: 0x004DBCA0 File Offset: 0x004D9EA0
		public unsafe static void Binary_Remove(ushort domainId, ushort dataId, int offset, int size)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 2, 8, &pData);
			*(int*)pData = offset;
			pData += 4;
			*(int*)pData = size;
			pData += 4;
		}

		// Token: 0x0600829E RID: 33438 RVA: 0x004DBCD4 File Offset: 0x004D9ED4
		public unsafe static byte* Binary_SetMetadata(ushort domainId, ushort dataId, ushort size)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, domainId, dataId, 3, (int)size, &pData);
			return pData;
		}

		// Token: 0x0600829F RID: 33439 RVA: 0x004DBD00 File Offset: 0x004D9F00
		public static uint Binary_Get(ushort domainId, ushort dataId)
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, domainId, dataId, 4, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
			return operationId;
		}

		// Token: 0x060082A0 RID: 33440 RVA: 0x004DBD43 File Offset: 0x004D9F43
		public unsafe static void Global_FreeMemory(byte* pointer)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 0, sizeof(byte*), (byte*)(&pointer));
		}

		// Token: 0x060082A1 RID: 33441 RVA: 0x004DBD63 File Offset: 0x004D9F63
		public unsafe static void Global_EnterNewWorld(sbyte archiveId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 1, 1, (byte*)(&archiveId));
		}

		// Token: 0x060082A2 RID: 33442 RVA: 0x004DBD80 File Offset: 0x004D9F80
		public unsafe static void Global_LoadWorld(sbyte archiveId, long backupTimestamp)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 2, 9, &pData);
			*pData = (byte)archiveId;
			pData++;
			*(long*)pData = backupTimestamp;
			pData += 8;
		}

		// Token: 0x060082A3 RID: 33443 RVA: 0x004DBDB8 File Offset: 0x004D9FB8
		public unsafe static void Global_SaveWorld(WorldInfo worldInfo, sbyte maxBackupsCount, sbyte compressionLevel)
		{
			int contentSize = worldInfo.GetSerializedSize();
			uint operationId = ArchiveDataManager.GetNextOperationId();
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 2, 0, 0, 3, 4 + contentSize + 1 + 1, &pData);
			*(int*)pData = contentSize;
			pData += 4;
			pData += worldInfo.Serialize(pData);
			*pData = (byte)maxBackupsCount;
			pData++;
			*pData = (byte)compressionLevel;
			pData++;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082A4 RID: 33444 RVA: 0x004DBE23 File Offset: 0x004DA023
		public static void Global_LeaveWorld()
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 4, 0, null);
		}

		// Token: 0x060082A5 RID: 33445 RVA: 0x004DBE40 File Offset: 0x004DA040
		public static void Global_GetArchivesInfo(bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 2, 0, 0, 5, 0, null);
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082A6 RID: 33446 RVA: 0x004DBE87 File Offset: 0x004DA087
		public unsafe static void Global_DeleteArchive(sbyte archiveId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 6, 1, (byte*)(&archiveId));
		}

		// Token: 0x060082A7 RID: 33447 RVA: 0x004DBEA4 File Offset: 0x004DA0A4
		public unsafe static void Global_LoadEnding(sbyte archiveId)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 8, 1, &pData);
			*pData = (byte)archiveId;
			pData++;
		}

		// Token: 0x060082A8 RID: 33448 RVA: 0x004DBED4 File Offset: 0x004DA0D4
		public unsafe static void Global_SaveEnding(WorldInfo worldInfo, sbyte compressionLevel)
		{
			int contentSize = worldInfo.GetSerializedSize();
			uint operationId = ArchiveDataManager.GetNextOperationId();
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 2, 0, 0, 7, 4 + contentSize, &pData);
			*(int*)pData = contentSize;
			pData += 4;
			pData += worldInfo.Serialize(pData);
			*pData = (byte)compressionLevel;
			pData++;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082A9 RID: 33449 RVA: 0x004DBF34 File Offset: 0x004DA134
		public unsafe static void Global_GetEndingArchiveInfo(sbyte archiveId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 2, 0, 0, 9, 1, (byte*)(&archiveId));
		}

		// Token: 0x060082AA RID: 33450 RVA: 0x004DBF50 File Offset: 0x004DA150
		public unsafe static void Global_Dummy()
		{
			uint operationId = ArchiveDataManager.GetNextOperationId();
			byte* pMemory = null;
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 2, 0, 0, 0, sizeof(byte*), (byte*)(&pMemory));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082AB RID: 33451 RVA: 0x004DBF98 File Offset: 0x004DA198
		public unsafe static void LifeRecord_Add(LifeRecordCollection collection)
		{
			byte* pData;
			ArchiveDataManager.CurrOperationBlock.Allocate(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 0, 8 + collection.Size, &pData);
			*(int*)pData = collection.Count;
			pData += 4;
			*(int*)pData = collection.Size;
			pData += 4;
			byte[] array;
			byte* pRawData;
			if ((array = collection.RawData) == null || array.Length == 0)
			{
				pRawData = null;
			}
			else
			{
				pRawData = &array[0];
			}
			Buffer.MemoryCopy((void*)pRawData, (void*)pData, (long)collection.Size, (long)collection.Size);
			array = null;
			pData += collection.Size;
		}

		// Token: 0x060082AC RID: 33452 RVA: 0x004DC01C File Offset: 0x004DA21C
		public unsafe static void LifeRecord_Get(int charId, int beginIndex, int count, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 1, 12, &pData);
			*(int*)pData = charId;
			pData += 4;
			*(int*)pData = beginIndex;
			pData += 4;
			*(int*)pData = count;
			pData += 4;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082AD RID: 33453 RVA: 0x004DC080 File Offset: 0x004DA280
		public unsafe static void LifeRecord_GetByDate(int charId, int startDate, int monthCount, int currDate, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 2, 16, &pData);
			*(int*)pData = charId;
			pData += 4;
			*(int*)pData = startDate;
			pData += 4;
			*(int*)pData = monthCount;
			pData += 4;
			*(int*)pData = currDate;
			pData += 4;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082AE RID: 33454 RVA: 0x004DC0EC File Offset: 0x004DA2EC
		public unsafe static void LifeRecord_GetLast(int charId, int count, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 3, 8, &pData);
			*(int*)pData = charId;
			pData += 4;
			*(int*)pData = count;
			pData += 4;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082AF RID: 33455 RVA: 0x004DC144 File Offset: 0x004DA344
		public unsafe static void LifeRecord_Search(int charId, int date, List<short> types, int relatedCharId, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			int typesCount = types.Count;
			byte* pData;
			int offset = ArchiveDataManager.CurrOperationBlock.Allocate(operationId, 0, 13, 0, 4, 10 + 2 * typesCount + 4, &pData);
			*(int*)pData = charId;
			pData += 4;
			*(int*)pData = date;
			pData += 4;
			*(short*)pData = (short)typesCount;
			pData += 2;
			for (int i = 0; i < typesCount; i++)
			{
				*(short*)pData = types[i];
				pData += 2;
			}
			*(int*)pData = relatedCharId;
			pData += 4;
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082B0 RID: 33456 RVA: 0x004DC1E1 File Offset: 0x004DA3E1
		public unsafe static void LifeRecord_Remove(int charId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 5, 4, (byte*)(&charId));
		}

		// Token: 0x060082B1 RID: 33457 RVA: 0x004DC1FD File Offset: 0x004DA3FD
		public unsafe static void LifeRecord_GenerateDead(int charId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 6, 4, (byte*)(&charId));
		}

		// Token: 0x060082B2 RID: 33458 RVA: 0x004DC21C File Offset: 0x004DA41C
		public unsafe static void LifeRecord_GetDead(int charId, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, 13, 0, 7, 4, (byte*)(&charId));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}

		// Token: 0x060082B3 RID: 33459 RVA: 0x004DC265 File Offset: 0x004DA465
		public unsafe static void LifeRecord_RemoveDead(int charId)
		{
			ArchiveDataManager.CurrOperationBlock.Add(ArchiveDataManager.GetNextOperationId(), 1, 13, 0, 8, 4, (byte*)(&charId));
		}

		// Token: 0x060082B4 RID: 33460 RVA: 0x004DC284 File Offset: 0x004DA484
		public unsafe static void LifeRecord_GetAllByChar(int charId, bool isPassthrough, uint operationId = 0U)
		{
			bool flag = !isPassthrough;
			if (flag)
			{
				operationId = ArchiveDataManager.GetNextOperationId();
			}
			int offset = ArchiveDataManager.CurrOperationBlock.Add(operationId, 0, 13, 0, 11, 4, (byte*)(&charId));
			ArchiveDataManager.PendingOperations.Enqueue(new PendingOperation(operationId, offset, ArchiveDataManager.CurrOperationBlock));
		}
	}
}
