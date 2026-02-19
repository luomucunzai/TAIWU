using System;
using System.Text;

namespace GameData.ArchiveData;

public readonly struct OperationWrapper
{
	public const int OffsetTypeId = 0;

	public const int OffsetMethodId = 1;

	public const int OffsetDomainId = 4;

	public const int OffsetDataId = 6;

	public const int OffsetId = 8;

	public const int OffsetDataSize = 12;

	public const int OffsetData = 16;

	public const int HeaderSize = 16;

	private unsafe readonly byte* _pOperation;

	public unsafe uint Id => ((uint*)_pOperation)[2];

	public unsafe byte TypeId => *_pOperation;

	public unsafe ushort DomainId => ((ushort*)_pOperation)[2];

	public unsafe ushort DataId => ((ushort*)_pOperation)[3];

	public unsafe byte MethodId => _pOperation[1];

	public unsafe uint DataSize => ((uint*)_pOperation)[3];

	public unsafe byte* PData => _pOperation + 16;

	public unsafe OperationWrapper(byte* pOperation)
	{
		_pOperation = pOperation;
	}

	public unsafe int GetTotalSize()
	{
		int num = ((int*)_pOperation)[3];
		int num2 = 16 + num;
		return (num2 + 3) / 4 * 4;
	}

	public unsafe static int GetTotalSize(byte* pOperation)
	{
		int num = ((int*)pOperation)[3];
		int num2 = 16 + num;
		return (num2 + 3) / 4 * 4;
	}

	public static int GetTotalSize(int dataSize)
	{
		int num = 16 + dataSize;
		return (num + 3) / 4 * 4;
	}

	public unsafe static void SetMemory(byte* pOperation, uint id, byte typeId, ushort domainId, ushort dataId, byte methodId, uint dataSize)
	{
		*pOperation = typeId;
		pOperation[1] = methodId;
		((short*)pOperation)[2] = (short)domainId;
		((short*)pOperation)[3] = (short)dataId;
		((int*)pOperation)[2] = (int)id;
		((int*)pOperation)[3] = (int)dataSize;
	}

	public unsafe override string ToString()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringBuilder stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder3 = stringBuilder2;
		StringBuilder.AppendInterpolatedStringHandler handler = new StringBuilder.AppendInterpolatedStringHandler(1, 1, stringBuilder2);
		handler.AppendFormatted(Id);
		handler.AppendLiteral(":");
		stringBuilder3.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder4 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder2);
		handler.AppendLiteral("  TypeId\t");
		handler.AppendFormatted(TypeId);
		stringBuilder4.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder5 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
		handler.AppendLiteral("  DomainId\t");
		handler.AppendFormatted(DomainId);
		stringBuilder5.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder6 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(9, 1, stringBuilder2);
		handler.AppendLiteral("  DataId\t");
		handler.AppendFormatted(DataId);
		stringBuilder6.AppendLine(ref handler);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder7 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
		handler.AppendLiteral("  MethodId\t");
		handler.AppendFormatted(MethodId);
		stringBuilder7.AppendLine(ref handler);
		int dataSize = (int)DataSize;
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder8 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(11, 1, stringBuilder2);
		handler.AppendLiteral("  DataSize\t");
		handler.AppendFormatted(dataSize);
		stringBuilder8.AppendLine(ref handler);
		byte[] array = new byte[dataSize];
		fixed (byte* destination = array)
		{
			Buffer.MemoryCopy(_pOperation + 16, destination, dataSize, dataSize);
		}
		string value = BitConverter.ToString(array, 0, dataSize);
		stringBuilder2 = stringBuilder;
		StringBuilder stringBuilder9 = stringBuilder2;
		handler = new StringBuilder.AppendInterpolatedStringHandler(7, 1, stringBuilder2);
		handler.AppendLiteral("  Data\t");
		handler.AppendFormatted(value);
		stringBuilder9.AppendLine(ref handler);
		return stringBuilder.ToString();
	}
}
