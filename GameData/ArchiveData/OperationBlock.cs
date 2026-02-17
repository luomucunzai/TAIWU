using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GameData.ArchiveData
{
	// Token: 0x02000902 RID: 2306
	public class OperationBlock : Stream
	{
		// Token: 0x17000520 RID: 1312
		// (get) Token: 0x060082B5 RID: 33461 RVA: 0x004DC2CE File Offset: 0x004DA4CE
		public int RawDataSize
		{
			get
			{
				return this._size;
			}
		}

		// Token: 0x17000521 RID: 1313
		// (get) Token: 0x060082B6 RID: 33462 RVA: 0x004DC2D6 File Offset: 0x004DA4D6
		public int Capacity
		{
			get
			{
				return this._rawData.Length;
			}
		}

		// Token: 0x17000522 RID: 1314
		// (get) Token: 0x060082B7 RID: 33463 RVA: 0x004DC2E0 File Offset: 0x004DA4E0
		public int OperationCount
		{
			get
			{
				return this._operationCount;
			}
		}

		// Token: 0x060082B8 RID: 33464 RVA: 0x004DC2E8 File Offset: 0x004DA4E8
		public OperationBlock(int defaultCapacity)
		{
			this._defaultCapacity = defaultCapacity;
			this._rawData = OperationBlock.EmptyArray;
		}

		// Token: 0x060082B9 RID: 33465 RVA: 0x004DC30C File Offset: 0x004DA50C
		protected override void Dispose(bool disposingManaged)
		{
			bool disposed = this._disposed;
			if (!disposed)
			{
				if (disposingManaged)
				{
				}
				bool flag = this._rawDataPointer != null;
				if (flag)
				{
					this._rawDataHandle.Free();
					this._rawDataPointer = null;
				}
				this._disposed = true;
				base.Dispose(disposingManaged);
			}
		}

		// Token: 0x060082BA RID: 33466 RVA: 0x004DC364 File Offset: 0x004DA564
		public unsafe void SetCapacity(int capacity)
		{
			bool flag = capacity < this._size;
			if (flag)
			{
				throw new ArgumentOutOfRangeException("capacity", "New capacity cannot be less than the used size");
			}
			int oriCapacity = this._rawData.Length;
			bool flag2 = capacity != oriCapacity;
			if (flag2)
			{
				bool flag3 = capacity > 0;
				if (flag3)
				{
					byte[] newRawData = new byte[capacity];
					bool flag4 = this._size > 0;
					if (flag4)
					{
						GCHandle newHandle = GCHandle.Alloc(newRawData, GCHandleType.Pinned);
						byte* newPointer = (byte*)newHandle.AddrOfPinnedObject().ToPointer();
						Buffer.MemoryCopy((void*)this._rawDataPointer, (void*)newPointer, (long)this._size, (long)this._size);
						this._rawData = newRawData;
						this._rawDataHandle.Free();
						this._rawDataHandle = newHandle;
						this._rawDataPointer = newPointer;
					}
					else
					{
						this._rawData = newRawData;
						bool flag5 = oriCapacity > 0;
						if (flag5)
						{
							this._rawDataHandle.Free();
						}
						this._rawDataHandle = GCHandle.Alloc(this._rawData, GCHandleType.Pinned);
						this._rawDataPointer = (byte*)this._rawDataHandle.AddrOfPinnedObject().ToPointer();
					}
				}
				else
				{
					this._rawData = OperationBlock.EmptyArray;
					bool flag6 = oriCapacity > 0;
					if (flag6)
					{
						this._rawDataHandle.Free();
					}
					this._rawDataPointer = null;
				}
			}
		}

		// Token: 0x060082BB RID: 33467 RVA: 0x004DC4A8 File Offset: 0x004DA6A8
		~OperationBlock()
		{
			this.Dispose(false);
		}

		// Token: 0x17000523 RID: 1315
		// (get) Token: 0x060082BC RID: 33468 RVA: 0x004DC4DC File Offset: 0x004DA6DC
		public override bool CanRead
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000524 RID: 1316
		// (get) Token: 0x060082BD RID: 33469 RVA: 0x004DC4DF File Offset: 0x004DA6DF
		public override bool CanSeek
		{
			get
			{
				return false;
			}
		}

		// Token: 0x17000525 RID: 1317
		// (get) Token: 0x060082BE RID: 33470 RVA: 0x004DC4E2 File Offset: 0x004DA6E2
		public override bool CanWrite
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000526 RID: 1318
		// (get) Token: 0x060082BF RID: 33471 RVA: 0x004DC4E5 File Offset: 0x004DA6E5
		public override long Length
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x17000527 RID: 1319
		// (get) Token: 0x060082C0 RID: 33472 RVA: 0x004DC4EC File Offset: 0x004DA6EC
		// (set) Token: 0x060082C1 RID: 33473 RVA: 0x004DC4F3 File Offset: 0x004DA6F3
		public override long Position
		{
			get
			{
				throw new NotImplementedException();
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		// Token: 0x060082C2 RID: 33474 RVA: 0x004DC4FC File Offset: 0x004DA6FC
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public int ReserveStreamWritingHeader(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId)
		{
			return this.ReserveHeader(operationId, typeId, domainId, dataId, methodId);
		}

		// Token: 0x060082C3 RID: 33475 RVA: 0x004DC51C File Offset: 0x004DA71C
		public void SetStreamWrittenDataSize(int offset)
		{
			int dataSize = this._size - (offset + 16);
			this.SetDataSize(offset, dataSize);
		}

		// Token: 0x060082C4 RID: 33476 RVA: 0x004DC53F File Offset: 0x004DA73F
		public override void Flush()
		{
		}

		// Token: 0x060082C5 RID: 33477 RVA: 0x004DC542 File Offset: 0x004DA742
		public override long Seek(long offset, SeekOrigin origin)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060082C6 RID: 33478 RVA: 0x004DC54A File Offset: 0x004DA74A
		public override void SetLength(long value)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060082C7 RID: 33479 RVA: 0x004DC552 File Offset: 0x004DA752
		public override int Read(byte[] buffer, int offset, int count)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060082C8 RID: 33480 RVA: 0x004DC55A File Offset: 0x004DA75A
		public override int ReadByte()
		{
			throw new NotImplementedException();
		}

		// Token: 0x060082C9 RID: 33481 RVA: 0x004DC564 File Offset: 0x004DA764
		public unsafe override void Write(byte[] buffer, int offset, int count)
		{
			int newSize = this._size + count;
			this.EnsureCapacity(newSize);
			int destOffset = this._size;
			this._size = newSize;
			fixed (byte[] array = buffer)
			{
				byte* pBuffer;
				if (buffer == null || array.Length == 0)
				{
					pBuffer = null;
				}
				else
				{
					pBuffer = &array[0];
				}
				Buffer.MemoryCopy((void*)(pBuffer + offset), (void*)(this._rawDataPointer + destOffset), (long)count, (long)count);
			}
		}

		// Token: 0x060082CA RID: 33482 RVA: 0x004DC5C4 File Offset: 0x004DA7C4
		public unsafe override void WriteByte(byte value)
		{
			int newSize = this._size + 1;
			this.EnsureCapacity(newSize);
			int destOffset = this._size;
			this._size = newSize;
			this._rawDataPointer[destOffset] = value;
		}

		// Token: 0x060082CB RID: 33483 RVA: 0x004DC5FC File Offset: 0x004DA7FC
		public unsafe int Add(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId, int dataSize, byte* pData)
		{
			int totalSize = OperationWrapper.GetTotalSize(dataSize);
			int newSize = this._size + totalSize;
			this.EnsureCapacity(newSize);
			int offset = this._size;
			this._size = newSize;
			byte* pOperation = this._rawDataPointer + offset;
			*pOperation = typeId;
			pOperation[1] = methodId;
			*(short*)(pOperation + 4) = (short)domainId;
			*(short*)(pOperation + 6) = (short)dataId;
			*(int*)(pOperation + 8) = (int)operationId;
			*(int*)(pOperation + 12) = dataSize;
			bool flag = dataSize > 0;
			if (flag)
			{
				Buffer.MemoryCopy((void*)pData, (void*)(pOperation + 16), (long)dataSize, (long)dataSize);
			}
			this._operationCount++;
			return offset;
		}

		// Token: 0x060082CC RID: 33484 RVA: 0x004DC68C File Offset: 0x004DA88C
		public unsafe int Allocate(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId, int dataSize, byte** ppData)
		{
			int totalSize = OperationWrapper.GetTotalSize(dataSize);
			int newSize = this._size + totalSize;
			this.EnsureCapacity(newSize);
			int offset = this._size;
			this._size = newSize;
			byte* pOperation = this._rawDataPointer + offset;
			*pOperation = typeId;
			pOperation[1] = methodId;
			*(short*)(pOperation + 4) = (short)domainId;
			*(short*)(pOperation + 6) = (short)dataId;
			*(int*)(pOperation + 8) = (int)operationId;
			*(int*)(pOperation + 12) = dataSize;
			*(IntPtr*)ppData = pOperation + 16;
			this._operationCount++;
			return offset;
		}

		// Token: 0x060082CD RID: 33485 RVA: 0x004DC708 File Offset: 0x004DA908
		public unsafe int ReserveHeader(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId)
		{
			int newSize = this._size + 16;
			this.EnsureCapacity(newSize);
			int offset = this._size;
			this._size = newSize;
			byte* pOperation = this._rawDataPointer + offset;
			*pOperation = typeId;
			pOperation[1] = methodId;
			*(short*)(pOperation + 4) = (short)domainId;
			*(short*)(pOperation + 6) = (short)dataId;
			*(int*)(pOperation + 8) = (int)operationId;
			return offset;
		}

		// Token: 0x060082CE RID: 33486 RVA: 0x004DC760 File Offset: 0x004DA960
		public unsafe void AddData(byte* pData, int dataSize)
		{
			int newSize = this._size + dataSize;
			this.EnsureCapacity(newSize);
			int offset = this._size;
			this._size = newSize;
			byte* pDest = this._rawDataPointer + offset;
			Buffer.MemoryCopy((void*)pData, (void*)pDest, (long)dataSize, (long)dataSize);
		}

		// Token: 0x060082CF RID: 33487 RVA: 0x004DC7A4 File Offset: 0x004DA9A4
		public unsafe void SetDataSize(int offset, int dataSize)
		{
			byte* pDataSize = this._rawDataPointer + offset + 12;
			*(int*)pDataSize = dataSize;
			int totalSize = OperationWrapper.GetTotalSize(dataSize);
			int newSize = offset + totalSize;
			this.EnsureCapacity(newSize);
			this._size = newSize;
			this._operationCount++;
		}

		// Token: 0x060082D0 RID: 33488 RVA: 0x004DC7EC File Offset: 0x004DA9EC
		public unsafe byte* GetRawDataPointer()
		{
			return this._rawDataPointer;
		}

		// Token: 0x060082D1 RID: 33489 RVA: 0x004DC804 File Offset: 0x004DAA04
		public OperationWrapper GetOperation(int offset)
		{
			return new OperationWrapper(this._rawDataPointer + offset);
		}

		// Token: 0x060082D2 RID: 33490 RVA: 0x004DC824 File Offset: 0x004DAA24
		private void EnsureCapacity(int min)
		{
			int oriCapacity = this._rawData.Length;
			bool flag = oriCapacity >= min;
			if (!flag)
			{
				int newCapacity = (oriCapacity == 0) ? this._defaultCapacity : (oriCapacity * 2);
				bool flag2 = newCapacity > int.MaxValue;
				if (flag2)
				{
					newCapacity = int.MaxValue;
				}
				bool flag3 = newCapacity < min;
				if (flag3)
				{
					newCapacity = min;
				}
				this.SetCapacity(newCapacity);
			}
		}

		// Token: 0x060082D3 RID: 33491 RVA: 0x004DC880 File Offset: 0x004DAA80
		public override string ToString()
		{
			return this.ToString(0, 10);
		}

		// Token: 0x060082D4 RID: 33492 RVA: 0x004DC89C File Offset: 0x004DAA9C
		public unsafe string ToString(int startIndex, int count)
		{
			StringBuilder sb = new StringBuilder();
			StringBuilder stringBuilder = sb;
			StringBuilder stringBuilder2 = stringBuilder;
			StringBuilder.AppendInterpolatedStringHandler appendInterpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder);
			appendInterpolatedStringHandler.AppendLiteral("Operations: ");
			appendInterpolatedStringHandler.AppendFormatted<int>(this._operationCount);
			stringBuilder2.AppendLine(ref appendInterpolatedStringHandler);
			int endIndex = Math.Min(startIndex + count, this._operationCount);
			byte* pOperation = this._rawDataPointer;
			for (int i = 0; i < endIndex; i++)
			{
				OperationWrapper operation = new OperationWrapper(pOperation);
				bool flag = i >= startIndex;
				if (flag)
				{
					sb.AppendLine(operation.ToString());
				}
				pOperation += operation.GetTotalSize();
			}
			return sb.ToString();
		}

		// Token: 0x0400247D RID: 9341
		private static readonly byte[] EmptyArray = Array.Empty<byte>();

		// Token: 0x0400247E RID: 9342
		private byte[] _rawData;

		// Token: 0x0400247F RID: 9343
		private GCHandle _rawDataHandle;

		// Token: 0x04002480 RID: 9344
		private unsafe byte* _rawDataPointer;

		// Token: 0x04002481 RID: 9345
		private readonly int _defaultCapacity;

		// Token: 0x04002482 RID: 9346
		private int _size;

		// Token: 0x04002483 RID: 9347
		private int _operationCount;

		// Token: 0x04002484 RID: 9348
		private bool _disposed = false;
	}
}
