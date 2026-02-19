using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace GameData.ArchiveData;

public class OperationBlock : Stream
{
	private static readonly byte[] EmptyArray = Array.Empty<byte>();

	private byte[] _rawData;

	private GCHandle _rawDataHandle;

	private unsafe byte* _rawDataPointer;

	private readonly int _defaultCapacity;

	private int _size;

	private int _operationCount;

	private bool _disposed = false;

	public int RawDataSize => _size;

	public int Capacity => _rawData.Length;

	public int OperationCount => _operationCount;

	public override bool CanRead => false;

	public override bool CanSeek => false;

	public override bool CanWrite => true;

	public override long Length
	{
		get
		{
			throw new NotImplementedException();
		}
	}

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

	public OperationBlock(int defaultCapacity)
	{
		_defaultCapacity = defaultCapacity;
		_rawData = EmptyArray;
	}

	protected unsafe override void Dispose(bool disposingManaged)
	{
		if (!_disposed)
		{
			if (disposingManaged)
			{
			}
			if (_rawDataPointer != null)
			{
				_rawDataHandle.Free();
				_rawDataPointer = null;
			}
			_disposed = true;
			base.Dispose(disposingManaged);
		}
	}

	public unsafe void SetCapacity(int capacity)
	{
		if (capacity < _size)
		{
			throw new ArgumentOutOfRangeException("capacity", "New capacity cannot be less than the used size");
		}
		int num = _rawData.Length;
		if (capacity == num)
		{
			return;
		}
		if (capacity > 0)
		{
			byte[] array = new byte[capacity];
			if (_size > 0)
			{
				GCHandle rawDataHandle = GCHandle.Alloc(array, GCHandleType.Pinned);
				byte* ptr = (byte*)rawDataHandle.AddrOfPinnedObject().ToPointer();
				Buffer.MemoryCopy(_rawDataPointer, ptr, _size, _size);
				_rawData = array;
				_rawDataHandle.Free();
				_rawDataHandle = rawDataHandle;
				_rawDataPointer = ptr;
			}
			else
			{
				_rawData = array;
				if (num > 0)
				{
					_rawDataHandle.Free();
				}
				_rawDataHandle = GCHandle.Alloc(_rawData, GCHandleType.Pinned);
				_rawDataPointer = (byte*)_rawDataHandle.AddrOfPinnedObject().ToPointer();
			}
		}
		else
		{
			_rawData = EmptyArray;
			if (num > 0)
			{
				_rawDataHandle.Free();
			}
			_rawDataPointer = null;
		}
	}

	~OperationBlock()
	{
		Dispose(disposingManaged: false);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public int ReserveStreamWritingHeader(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId)
	{
		return ReserveHeader(operationId, typeId, domainId, dataId, methodId);
	}

	public void SetStreamWrittenDataSize(int offset)
	{
		int dataSize = _size - (offset + 16);
		SetDataSize(offset, dataSize);
	}

	public override void Flush()
	{
	}

	public override long Seek(long offset, SeekOrigin origin)
	{
		throw new NotImplementedException();
	}

	public override void SetLength(long value)
	{
		throw new NotImplementedException();
	}

	public override int Read(byte[] buffer, int offset, int count)
	{
		throw new NotImplementedException();
	}

	public override int ReadByte()
	{
		throw new NotImplementedException();
	}

	public unsafe override void Write(byte[] buffer, int offset, int count)
	{
		int num = _size + count;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		fixed (byte* ptr = buffer)
		{
			Buffer.MemoryCopy(ptr + offset, _rawDataPointer + size, count, count);
		}
	}

	public unsafe override void WriteByte(byte value)
	{
		int num = _size + 1;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		_rawDataPointer[size] = value;
	}

	public unsafe int Add(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId, int dataSize, byte* pData)
	{
		int totalSize = OperationWrapper.GetTotalSize(dataSize);
		int num = _size + totalSize;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		byte* ptr = _rawDataPointer + size;
		*ptr = typeId;
		ptr[1] = methodId;
		((short*)ptr)[2] = (short)domainId;
		((short*)ptr)[3] = (short)dataId;
		((int*)ptr)[2] = (int)operationId;
		((int*)ptr)[3] = dataSize;
		if (dataSize > 0)
		{
			Buffer.MemoryCopy(pData, ptr + 16, dataSize, dataSize);
		}
		_operationCount++;
		return size;
	}

	public unsafe int Allocate(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId, int dataSize, byte** ppData)
	{
		int totalSize = OperationWrapper.GetTotalSize(dataSize);
		int num = _size + totalSize;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		byte* ptr = _rawDataPointer + size;
		*ptr = typeId;
		ptr[1] = methodId;
		((short*)ptr)[2] = (short)domainId;
		((short*)ptr)[3] = (short)dataId;
		((int*)ptr)[2] = (int)operationId;
		((int*)ptr)[3] = dataSize;
		*ppData = ptr + 16;
		_operationCount++;
		return size;
	}

	public unsafe int ReserveHeader(uint operationId, byte typeId, ushort domainId, ushort dataId, byte methodId)
	{
		int num = _size + 16;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		byte* ptr = _rawDataPointer + size;
		*ptr = typeId;
		ptr[1] = methodId;
		((short*)ptr)[2] = (short)domainId;
		((short*)ptr)[3] = (short)dataId;
		((int*)ptr)[2] = (int)operationId;
		return size;
	}

	public unsafe void AddData(byte* pData, int dataSize)
	{
		int num = _size + dataSize;
		EnsureCapacity(num);
		int size = _size;
		_size = num;
		byte* destination = _rawDataPointer + size;
		Buffer.MemoryCopy(pData, destination, dataSize, dataSize);
	}

	public unsafe void SetDataSize(int offset, int dataSize)
	{
		byte* ptr = _rawDataPointer + offset + 12;
		*(int*)ptr = dataSize;
		int totalSize = OperationWrapper.GetTotalSize(dataSize);
		int num = offset + totalSize;
		EnsureCapacity(num);
		_size = num;
		_operationCount++;
	}

	public unsafe byte* GetRawDataPointer()
	{
		return _rawDataPointer;
	}

	public unsafe OperationWrapper GetOperation(int offset)
	{
		return new OperationWrapper(_rawDataPointer + offset);
	}

	private void EnsureCapacity(int min)
	{
		int num = _rawData.Length;
		if (num < min)
		{
			int num2 = ((num == 0) ? _defaultCapacity : (num * 2));
			if ((uint)num2 > 2147483647u)
			{
				num2 = int.MaxValue;
			}
			if (num2 < min)
			{
				num2 = min;
			}
			SetCapacity(num2);
		}
	}

	public override string ToString()
	{
		return ToString(0, 10);
	}

	public unsafe string ToString(int startIndex, int count)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(12, 1, stringBuilder2);
		handler.AppendLiteral("Operations: ");
		handler.AppendFormatted(_operationCount);
		stringBuilder2.AppendLine(ref handler);
		int num = Math.Min(startIndex + count, _operationCount);
		byte* ptr = _rawDataPointer;
		for (int i = 0; i < num; i++)
		{
			OperationWrapper operationWrapper = new OperationWrapper(ptr);
			if (i >= startIndex)
			{
				stringBuilder.AppendLine(operationWrapper.ToString());
			}
			ptr += operationWrapper.GetTotalSize();
		}
		return stringBuilder.ToString();
	}
}
