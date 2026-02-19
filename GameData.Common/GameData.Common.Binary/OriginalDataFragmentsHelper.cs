using System.Collections.Generic;

namespace GameData.Common.Binary;

public static class OriginalDataFragmentsHelper
{
	public static void ApplyInsertRecord(List<OriginalDataFragment> fragments, int offset, int size)
	{
		int index = FindFirstSegment(fragments, offset);
		ChangeOffsetOfFragments(fragments, index, size);
	}

	public static void ApplyWriteRecord(List<OriginalDataFragment> fragments, int offset, int size)
	{
		int index = FindFirstSegment(fragments, offset);
		RemoveFragments(fragments, index, offset + size);
	}

	public static void ApplyDeleteRecord(List<OriginalDataFragment> fragments, int offset, int size)
	{
		int index = FindFirstSegment(fragments, offset);
		RemoveFragments(fragments, index, offset + size);
		ChangeOffsetOfFragments(fragments, index, -size);
	}

	private static int FindFirstSegment(List<OriginalDataFragment> fragments, int offset)
	{
		int num = fragments.BinarySearch(new OriginalDataFragment(offset, -1, -1));
		if (num >= 0)
		{
			return num;
		}
		num = ~num;
		if (num == 0)
		{
			return num;
		}
		OriginalDataFragment originalDataFragment = fragments[num - 1];
		if (originalDataFragment.CurrOffset + originalDataFragment.Size <= offset)
		{
			return num;
		}
		Split(fragments, num - 1, offset);
		return num;
	}

	private static void Split(List<OriginalDataFragment> fragments, int index, int splitOffset)
	{
		OriginalDataFragment value = fragments[index];
		int num = (value.Size = splitOffset - value.CurrOffset);
		fragments[index] = value;
		int size = value.Size - num;
		int oriOffset = value.OriOffset + num;
		OriginalDataFragment item = new OriginalDataFragment(splitOffset, size, oriOffset);
		fragments.Insert(index + 1, item);
	}

	private static void ChangeOffsetOfFragments(List<OriginalDataFragment> fragments, int index, int delta)
	{
		int i = index;
		for (int count = fragments.Count; i < count; i++)
		{
			OriginalDataFragment value = fragments[i];
			value.CurrOffset += delta;
			fragments[i] = value;
		}
	}

	private static void RemoveFragments(List<OriginalDataFragment> fragments, int index, int endOffset)
	{
		int num = -1;
		int i = index;
		for (int count = fragments.Count; i < count; i++)
		{
			OriginalDataFragment value = fragments[i];
			if (value.CurrOffset >= endOffset)
			{
				break;
			}
			if (value.CurrOffset + value.Size <= endOffset)
			{
				num = i;
				continue;
			}
			int num2 = endOffset - value.CurrOffset;
			value.CurrOffset = endOffset;
			value.Size -= num2;
			value.OriOffset += num2;
			fragments[i] = value;
			break;
		}
		if (num >= 0)
		{
			fragments.RemoveRange(index, num - index + 1);
		}
	}
}
