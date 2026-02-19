using System;
using System.Runtime.CompilerServices;
using GameData.Serializer;

namespace GameData.Utilities;

public class RawDataBlock : IBinary, ISerializableGameData
{
	private const int DefaultInitialCapacity = 16;

	private static readonly byte[] EmptyArray = Array.Empty<byte>();

	public byte[] RawData;

	public int Size;

	private readonly int _initialCapacity;

	public RawDataBlock()
		: this(16)
	{
	}

	public RawDataBlock(int initialCapacity)
	{
		RawData = EmptyArray;
		Size = 0;
		_initialCapacity = ((initialCapacity > 0) ? initialCapacity : 16);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		return (4 + RawData.Length + 4 + 3) / 4 * 4;
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		int num = (*(int*)ptr = RawData.Length);
		ptr += 4;
		if (num > 0)
		{
			fixed (byte* rawData = RawData)
			{
				Buffer.MemoryCopy(rawData, ptr, num, num);
			}
			ptr += num;
		}
		*(int*)ptr = Size;
		ptr += 4;
		return ((int)(ptr - pData) + 3) / 4 * 4;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		int num = *(int*)ptr;
		ptr += 4;
		if (num > 0)
		{
			if (RawData.Length < num)
			{
				RawData = new byte[num];
			}
			fixed (byte* rawData = RawData)
			{
				Buffer.MemoryCopy(ptr, rawData, num, num);
			}
			ptr += num;
		}
		Size = *(int*)ptr;
		ptr += 4;
		return ((int)(ptr - pData) + 3) / 4 * 4;
	}

	public unsafe int AddUnmanaged<T>(T value) where T : unmanaged
	{
		int size = Size;
		int num = Size + sizeof(T);
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			*(T*)(rawData + size) = value;
		}
		return size;
	}

	public unsafe int AddSerializableGameData(ISerializableGameData value)
	{
		int size = Size;
		int num = Size + value.GetSerializedSize();
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			value.Serialize(rawData + size);
		}
		return size;
	}

	public unsafe T GetUnmanaged<T>(int offset) where T : unmanaged
	{
		fixed (byte* rawData = RawData)
		{
			return *(T*)(rawData + offset);
		}
	}

	public unsafe int GetSerializableGameData<T>(ref T data, int offset) where T : ISerializableGameData, new()
	{
		if (data == null)
		{
			data = new T();
		}
		fixed (byte* rawData = RawData)
		{
			return ((ISerializableGameData)data/*cast due to .constrained prefix*/).Deserialize(rawData + offset);
		}
	}

	public unsafe void Insert(byte* pSrc, int offset, int size)
	{
		Tester.Assert(offset >= 0 && offset <= Size);
		Tester.Assert(size >= 0);
		int size2 = Size;
		int num = size2 + size;
		EnsureCapacity(num);
		Size = num;
		fixed (byte* rawData = RawData)
		{
			int num2 = size2 - offset;
			if (num2 > 0)
			{
				Buffer.MemoryCopy(rawData + offset, rawData + (offset + size), num2, num2);
			}
			Buffer.MemoryCopy(pSrc, rawData + offset, size, size);
		}
	}

	public unsafe void Write(byte* pSrc, int offset, int size)
	{
		Tester.Assert(offset >= 0 && offset <= Size);
		Tester.Assert(size >= 0);
		int size2 = Size;
		int num = Math.Max(offset + size, size2);
		if (num > Size)
		{
			EnsureCapacity(num);
			Size = num;
		}
		fixed (byte* rawData = RawData)
		{
			Buffer.MemoryCopy(pSrc, rawData + offset, size, size);
		}
	}

	public unsafe void Remove(int offset, int size)
	{
		Tester.Assert(offset >= 0);
		Tester.Assert(size >= 0);
		Tester.Assert(offset + size <= Size);
		int num = Size - (offset + size);
		if (num > 0)
		{
			fixed (byte* rawData = RawData)
			{
				Buffer.MemoryCopy(rawData + (offset + size), rawData + offset, num, num);
			}
		}
		Size -= size;
	}

	public unsafe void Move(int srcOffset, int destOffset, int size)
	{
		Tester.Assert(srcOffset >= 0);
		Tester.Assert(destOffset >= 0);
		Tester.Assert(size >= 0);
		Tester.Assert(srcOffset + size <= Size);
		Tester.Assert(destOffset + size <= Size);
		fixed (byte* rawData = RawData)
		{
			Buffer.MemoryCopy(rawData + srcOffset, rawData + destOffset, size, size);
		}
	}

	public int GetSize()
	{
		return Size;
	}

	public void Clear()
	{
		Size = 0;
	}

	public void EnsureCapacity(int desiredSize)
	{
		int num = RawData.Length;
		if (num < desiredSize)
		{
			EnsureCapacityInternal(num, desiredSize);
		}
	}

	public byte[] GetRawData()
	{
		return RawData;
	}

	public void SetRawData(byte[] rawData)
	{
		RawData = rawData;
	}

	public unsafe void CopyTo(int offset, int size, byte* pDest)
	{
		Tester.Assert(offset >= 0);
		Tester.Assert(size >= 0);
		Tester.Assert(offset + size <= Size);
		fixed (byte* rawData = RawData)
		{
			Buffer.MemoryCopy(rawData + offset, pDest, size, size);
		}
	}

	public ushort GetSerializedFixedSizeOfMetadata()
	{
		return 4;
	}

	public unsafe int SerializeMetadata(byte* pData)
	{
		*(int*)pData = Size;
		return 4;
	}

	public unsafe int DeserializeMetadata(byte* pData)
	{
		Size = *(int*)pData;
		EnsureCapacity(Size);
		return 4;
	}

	[MethodImpl(MethodImplOptions.NoInlining)]
	private void EnsureCapacityInternal(int oriCapacity, int desiredSize)
	{
		int num = ((oriCapacity == 0) ? _initialCapacity : (oriCapacity * 2));
		if ((uint)num > 2147483647u)
		{
			num = int.MaxValue;
		}
		if (num < desiredSize)
		{
			num = desiredSize;
		}
		byte[] array = new byte[num];
		if (oriCapacity > 0)
		{
			Buffer.BlockCopy(RawData, 0, array, 0, oriCapacity);
		}
		RawData = array;
	}
}
