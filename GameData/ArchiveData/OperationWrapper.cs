using System;
using System.Text;

namespace GameData.ArchiveData
{
	// Token: 0x02000904 RID: 2308
	public readonly struct OperationWrapper
	{
		// Token: 0x17000528 RID: 1320
		// (get) Token: 0x060082D6 RID: 33494 RVA: 0x004DC95B File Offset: 0x004DAB5B
		public unsafe uint Id
		{
			get
			{
				return *(uint*)(this._pOperation + 8);
			}
		}

		// Token: 0x17000529 RID: 1321
		// (get) Token: 0x060082D7 RID: 33495 RVA: 0x004DC966 File Offset: 0x004DAB66
		public unsafe byte TypeId
		{
			get
			{
				return *this._pOperation;
			}
		}

		// Token: 0x1700052A RID: 1322
		// (get) Token: 0x060082D8 RID: 33496 RVA: 0x004DC96F File Offset: 0x004DAB6F
		public unsafe ushort DomainId
		{
			get
			{
				return *(ushort*)(this._pOperation + 4);
			}
		}

		// Token: 0x1700052B RID: 1323
		// (get) Token: 0x060082D9 RID: 33497 RVA: 0x004DC97A File Offset: 0x004DAB7A
		public unsafe ushort DataId
		{
			get
			{
				return *(ushort*)(this._pOperation + 6);
			}
		}

		// Token: 0x1700052C RID: 1324
		// (get) Token: 0x060082DA RID: 33498 RVA: 0x004DC985 File Offset: 0x004DAB85
		public unsafe byte MethodId
		{
			get
			{
				return this._pOperation[1];
			}
		}

		// Token: 0x1700052D RID: 1325
		// (get) Token: 0x060082DB RID: 33499 RVA: 0x004DC990 File Offset: 0x004DAB90
		public unsafe uint DataSize
		{
			get
			{
				return *(uint*)(this._pOperation + 12);
			}
		}

		// Token: 0x1700052E RID: 1326
		// (get) Token: 0x060082DC RID: 33500 RVA: 0x004DC99C File Offset: 0x004DAB9C
		public unsafe byte* PData
		{
			get
			{
				return this._pOperation + 16;
			}
		}

		// Token: 0x060082DD RID: 33501 RVA: 0x004DC9A7 File Offset: 0x004DABA7
		public unsafe OperationWrapper(byte* pOperation)
		{
			this._pOperation = pOperation;
		}

		// Token: 0x060082DE RID: 33502 RVA: 0x004DC9B4 File Offset: 0x004DABB4
		public unsafe int GetTotalSize()
		{
			int dataSize = (int)(*(uint*)(this._pOperation + 12));
			int actualSize = 16 + dataSize;
			return (actualSize + 3) / 4 * 4;
		}

		// Token: 0x060082DF RID: 33503 RVA: 0x004DC9E0 File Offset: 0x004DABE0
		public unsafe static int GetTotalSize(byte* pOperation)
		{
			int dataSize = (int)(*(uint*)(pOperation + 12));
			int actualSize = 16 + dataSize;
			return (actualSize + 3) / 4 * 4;
		}

		// Token: 0x060082E0 RID: 33504 RVA: 0x004DCA08 File Offset: 0x004DAC08
		public static int GetTotalSize(int dataSize)
		{
			int actualSize = 16 + dataSize;
			return (actualSize + 3) / 4 * 4;
		}

		// Token: 0x060082E1 RID: 33505 RVA: 0x004DCA28 File Offset: 0x004DAC28
		public unsafe static void SetMemory(byte* pOperation, uint id, byte typeId, ushort domainId, ushort dataId, byte methodId, uint dataSize)
		{
			*pOperation = typeId;
			pOperation[1] = methodId;
			*(short*)(pOperation + 4) = (short)domainId;
			*(short*)(pOperation + 6) = (short)dataId;
			*(int*)(pOperation + 8) = (int)id;
			*(int*)(pOperation + 12) = (int)dataSize;
		}

		// Token: 0x060082E2 RID: 33506 RVA: 0x004DCA4C File Offset: 0x004DAC4C
		public unsafe override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			StringBuilder stringBuilder = sb;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendFormatted<uint>(this.Id);
			appendInterpolatedStringHandler.AppendLiteral(":");
			stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder = sb;
			StringBuilder stringBuilder3 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  TypeId\t");
			appendInterpolatedStringHandler.AppendFormatted<byte>(this.TypeId);
			stringBuilder3.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder = sb;
			StringBuilder stringBuilder4 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  DomainId\t");
			appendInterpolatedStringHandler.AppendFormatted<ushort>(this.DomainId);
			stringBuilder4.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder = sb;
			StringBuilder stringBuilder5 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  DataId\t");
			appendInterpolatedStringHandler.AppendFormatted<ushort>(this.DataId);
			stringBuilder5.AppendLine(ref appendInterpolatedStringHandler);
			stringBuilder = sb;
			StringBuilder stringBuilder6 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  MethodId\t");
			appendInterpolatedStringHandler.AppendFormatted<byte>(this.MethodId);
			stringBuilder6.AppendLine(ref appendInterpolatedStringHandler);
			int dataSize = (int)this.DataSize;
			stringBuilder = sb;
			StringBuilder stringBuilder7 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  DataSize\t");
			appendInterpolatedStringHandler.AppendFormatted<int>(dataSize);
			stringBuilder7.AppendLine(ref appendInterpolatedStringHandler);
			byte[] data = new byte[dataSize];
			byte[] array;
			byte* pData;
			if ((array = data) == null || array.Length == 0)
			{
				pData = null;
			}
			else
			{
				pData = &array[0];
			}
			Buffer.MemoryCopy((void*)(this._pOperation + 16), (void*)pData, (long)dataSize, (long)dataSize);
			array = null;
			string dataString = BitConverter.ToString(data, 0, dataSize);
			stringBuilder = sb;
			StringBuilder stringBuilder8 = stringBuilder;
			appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("  Data\t");
			appendInterpolatedStringHandler.AppendFormatted(dataString);
			stringBuilder8.AppendLine(ref appendInterpolatedStringHandler);
			return sb.ToString();
		}

		// Token: 0x04002485 RID: 9349
		public const int OffsetTypeId = 0;

		// Token: 0x04002486 RID: 9350
		public const int OffsetMethodId = 1;

		// Token: 0x04002487 RID: 9351
		public const int OffsetDomainId = 4;

		// Token: 0x04002488 RID: 9352
		public const int OffsetDataId = 6;

		// Token: 0x04002489 RID: 9353
		public const int OffsetId = 8;

		// Token: 0x0400248A RID: 9354
		public const int OffsetDataSize = 12;

		// Token: 0x0400248B RID: 9355
		public const int OffsetData = 16;

		// Token: 0x0400248C RID: 9356
		public const int HeaderSize = 16;

		// Token: 0x0400248D RID: 9357
		private unsafe readonly byte* _pOperation;
	}
}
