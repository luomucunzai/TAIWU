using System;
using System.IO;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using GameData.GameDataBridge.VnPipe;

namespace GameData.Utilities;

public class RawDataPool : Stream
{
	public const int InvalidOffset = -1;

	public const int DataSizeLimit = 16777216;

	private const int DefaultCapacity = 16;

	private static readonly byte[] EmptyArray = Array.Empty<byte>();

	private byte[] _rawData;

	private GCHandle _rawDataHandle;

	private unsafe byte* _rawDataPointer;

	private readonly int _initialCapacity;

	private int _size;

	private bool _disposed;

	private int _streamCurrReadingOffset;

	public int RawDataSize => _size;

	public int Capacity => _rawData.Length;

	public override bool CanRead => true;

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

	public RawDataPool(int initialCapacity)
	{
		_initialCapacity = initialCapacity;
		_rawData = EmptyArray;
	}

	public unsafe RawDataPool(Socket socket, int size)
	{
		if (size <= 0)
		{
			_initialCapacity = 16;
			_rawData = EmptyArray;
			return;
		}
		_rawData = new byte[size];
		int num;
		for (int i = 0; i < size; i += num)
		{
			num = socket.Receive(_rawData, i, size - i, SocketFlags.None);
			if (num == 0)
			{
				throw new Exception("The socket has been shut down.");
			}
		}
		_rawDataHandle = GCHandle.Alloc(_rawData, GCHandleType.Pinned);
		_rawDataPointer = (byte*)_rawDataHandle.AddrOfPinnedObject().ToPointer();
		_initialCapacity = size;
		_size = size;
	}

	public unsafe RawDataPool(IPipe pipe, int size)
	{
		if (size <= 0)
		{
			_initialCapacity = 16;
			_rawData = EmptyArray;
			return;
		}
		_rawData = new byte[size];
		int num;
		for (int i = 0; i < size; i += num)
		{
			num = pipe.Read(_rawData, i, size - i);
		}
		_rawDataHandle = GCHandle.Alloc(_rawData, GCHandleType.Pinned);
		_rawDataPointer = (byte*)_rawDataHandle.AddrOfPinnedObject().ToPointer();
		_initialCapacity = size;
		_size = size;
	}

	protected unsafe override void Dispose(bool disposingManaged)
	{
		if (!_disposed)
		{
			if (_rawDataPointer != null)
			{
				_rawDataHandle.Free();
				_rawDataPointer = null;
			}
			_disposed = true;
			base.Dispose(disposingManaged);
		}
	}

	~RawDataPool()
	{
		Dispose(disposingManaged: false);
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

	public void Clear()
	{
		_size = 0;
	}

	public void SetStreamReadingOffset(int offset)
	{
		_streamCurrReadingOffset = offset;
	}

	public int GetStreamReadingOffset()
	{
		return _streamCurrReadingOffset;
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

	public unsafe override int Read(byte[] buffer, int offset, int count)
	{
		if (_streamCurrReadingOffset + count > _size)
		{
			return 0;
		}
		fixed (byte* ptr = buffer)
		{
			Buffer.MemoryCopy(_rawDataPointer + _streamCurrReadingOffset, ptr + offset, count, count);
		}
		_streamCurrReadingOffset += count;
		return count;
	}

	public unsafe override int ReadByte()
	{
		if (_streamCurrReadingOffset >= _size)
		{
			return -1;
		}
		byte* num = _rawDataPointer + _streamCurrReadingOffset;
		_streamCurrReadingOffset++;
		return *num;
	}

	public unsafe override void Write(byte[] buffer, int offset, int count)
	{
		int size = _size;
		int num = _size + count;
		EnsureCapacity(num);
		_size = num;
		fixed (byte* ptr = buffer)
		{
			Buffer.MemoryCopy(ptr + offset, _rawDataPointer + size, count, count);
		}
	}

	public unsafe override void WriteByte(byte value)
	{
		int size = _size;
		int num = _size + 1;
		EnsureCapacity(num);
		_size = num;
		_rawDataPointer[size] = value;
	}

	public int GetWritingOffset()
	{
		return _size;
	}

	public int GetWrittenDataSize(int offset)
	{
		return _size - offset;
	}

	public unsafe byte* GetPointer(int offset)
	{
		return _rawDataPointer + offset;
	}

	public unsafe byte* GetPointerWithHeader(int offset, uint* pHeader)
	{
		byte* ptr = _rawDataPointer + offset;
		*pHeader = *(uint*)ptr;
		return ptr + 4;
	}

	public unsafe int AddUnmanaged<T>(T value) where T : unmanaged
	{
		int size = _size;
		int num = _size + sizeof(T);
		EnsureCapacity(num);
		_size = num;
		*(T*)(_rawDataPointer + size) = value;
		return size;
	}

	public unsafe void SetUnmanaged<T>(int offset, T value) where T : unmanaged
	{
		if (offset + sizeof(T) > _size)
		{
			throw new Exception(string.Format("Trying to write value of type {0} at offset {1}, which should not exceed the allocated size {2}.", "T", offset, _size));
		}
		*(T*)(_rawDataPointer + offset) = value;
	}

	public unsafe T GetUnmanaged<T>(int offset) where T : unmanaged
	{
		return *(T*)(_rawDataPointer + offset);
	}

	public unsafe int Add(byte* pData, int dataSize)
	{
		int size = _size;
		int num = _size + dataSize;
		EnsureCapacity(num);
		_size = num;
		if (dataSize > 0)
		{
			Buffer.MemoryCopy(pData, _rawDataPointer + size, dataSize, dataSize);
		}
		return size;
	}

	public unsafe int AddWithHeader(byte* pData, int dataSize, uint header, bool checkMaxSize = true)
	{
		if (checkMaxSize && dataSize > 16777216)
		{
			AdaptableLog.TagWarning("RawDataPool", $"dataSize {dataSize / 1024} is expected to be less than {16384} kb.", appendWarningMessage: true);
		}
		int size = _size;
		int num = _size + 4 + dataSize;
		EnsureCapacity(num);
		_size = num;
		byte* ptr = _rawDataPointer + size;
		*(uint*)ptr = header;
		ptr += 4;
		if (dataSize > 0)
		{
			Buffer.MemoryCopy(pData, ptr, dataSize, dataSize);
		}
		return size;
	}

	public unsafe int Allocate(int dataSize, byte** ppData)
	{
		int size = _size;
		int num = _size + dataSize;
		EnsureCapacity(num);
		_size = num;
		*ppData = _rawDataPointer + size;
		return size;
	}

	public unsafe int AllocateWithHeader(int dataSize, byte** ppData, uint header, bool checkMaxSize = true)
	{
		if (checkMaxSize && dataSize > 16777216)
		{
			AdaptableLog.TagWarning("RawDataPool", $"dataSize {dataSize / 1024} is expected to be less than {16384} kb.", appendWarningMessage: true);
		}
		int size = _size;
		int num = _size + 4 + dataSize;
		EnsureCapacity(num);
		_size = num;
		byte* ptr = _rawDataPointer + size;
		*(uint*)ptr = header;
		*ppData = ptr + 4;
		return size;
	}

	public int CopyTo(Socket socket)
	{
		if (_size <= 0)
		{
			return 0;
		}
		return socket.Send(_rawData, 0, _size, SocketFlags.None);
	}

	public int CopyTo(IPipe pipe)
	{
		if (_size <= 0)
		{
			return 0;
		}
		return pipe.Write(_rawData, 0, _size);
	}

	private void EnsureCapacity(int min)
	{
		int num = _rawData.Length;
		if (num < min)
		{
			int num2 = ((num == 0) ? _initialCapacity : (num * 2));
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
}
