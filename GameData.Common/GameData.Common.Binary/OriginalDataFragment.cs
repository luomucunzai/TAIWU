using System;

namespace GameData.Common.Binary;

public struct OriginalDataFragment : IComparable<OriginalDataFragment>
{
	public int CurrOffset;

	public int Size;

	public int OriOffset;

	public OriginalDataFragment(int currOffset, int size, int oriOffset)
	{
		CurrOffset = currOffset;
		Size = size;
		OriOffset = oriOffset;
	}

	public int CompareTo(OriginalDataFragment other)
	{
		return CurrOffset - other.CurrOffset;
	}
}
