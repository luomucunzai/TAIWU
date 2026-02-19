namespace GameData.Domains.Item;

public static class SkillBookStateHelper
{
	public static byte SetOutlinePageType(byte pageTypes, sbyte behaviorType)
	{
		return (byte)((pageTypes & 0xF8) | (byte)behaviorType);
	}

	public static sbyte GetOutlinePageType(byte pageTypes)
	{
		return (sbyte)(pageTypes & 7);
	}

	public static byte SetNormalPageType(byte pageTypes, byte pageId, sbyte direction)
	{
		int num = 3 + pageId - 1;
		return (byte)((direction == 0) ? (pageTypes & ~(1 << num)) : (pageTypes | (1 << num)));
	}

	public static sbyte GetNormalPageType(byte pageTypes, byte pageId)
	{
		int num = 3 + pageId - 1;
		return ((pageTypes & (1 << num)) != 0) ? ((sbyte)1) : ((sbyte)0);
	}

	public static ushort SetPageIncompleteState(ushort pageIncompleteState, byte pageId, sbyte state)
	{
		int num = pageId * 2;
		return (ushort)((pageIncompleteState & ~(3 << num)) | (state << num));
	}

	public static sbyte GetPageIncompleteState(ushort pageIncompleteState, byte pageId)
	{
		int num = pageId * 2;
		return (sbyte)((pageIncompleteState >> num) & 3);
	}

	public static int GetTotalIncompleteStateValue(ushort pageIncompleteState, int pageCount)
	{
		int num = 0;
		for (byte b = 0; b < pageCount; b++)
		{
			sbyte pageIncompleteState2 = GetPageIncompleteState(pageIncompleteState, b);
			num += SkillBookPageIncompleteState.BaseReadingSpeed[pageIncompleteState2] / 2;
		}
		return num;
	}
}
