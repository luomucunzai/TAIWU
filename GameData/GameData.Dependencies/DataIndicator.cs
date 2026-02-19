using System;
using GameData.Common;
using GameData.Domains;

namespace GameData.Dependencies;

public readonly struct DataIndicator : IEquatable<DataIndicator>
{
	public readonly DomainDataType DataType;

	public readonly ushort DomainId;

	public readonly ushort DataId;

	public DataIndicator(DomainDataType dataType, ushort domainId, ushort dataId)
	{
		DataType = dataType;
		DomainId = domainId;
		DataId = dataId;
	}

	public bool Equals(DataIndicator other)
	{
		return DataType == other.DataType && DomainId == other.DomainId && DataId == other.DataId;
	}

	public override bool Equals(object obj)
	{
		return obj is DataIndicator other && Equals(other);
	}

	public override int GetHashCode()
	{
		int dataType = (int)DataType;
		dataType = (dataType * 397) ^ DomainId.GetHashCode();
		return (dataType * 397) ^ DataId.GetHashCode();
	}

	public override string ToString()
	{
		string name = Enum.GetName(typeof(DomainDataType), DataType);
		string text = DomainHelper.DomainId2DomainName[DomainId];
		switch (DataType)
		{
		case DomainDataType.SingleValue:
		case DomainDataType.SingleValueCollection:
			return text + "(" + name + ")";
		case DomainDataType.ElementList:
		{
			string value2 = DomainHelper.DomainId2DataId2FieldName[DomainId][DataId];
			return $"{text}.{value2}({name})";
		}
		default:
		{
			string value = DomainHelper.DomainId2DataId2FieldName[DomainId][DataId];
			return $"{text}.{value}({name})";
		}
		}
	}
}
