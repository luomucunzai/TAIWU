using System;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.CombatSkill;

public static class CombatSkillStateHelper
{
	public const int OutlinePagesCount = 5;

	private const int TotalNormalPagesCount = 10;

	public const int TotalPagesCount = 15;

	public const ushort CompleteReadingState = 32767;

	public const ushort BreakoutRequiredNormalPageCount = 5;

	public static byte GetPageInternalIndex(sbyte behaviorType, sbyte direction, byte pageId)
	{
		return (byte)((pageId == 0) ? behaviorType : (5 + direction * 5 + pageId - 1));
	}

	public static byte GetNormalPageOppositeInternalIndex(byte pageInternalIndex)
	{
		if (pageInternalIndex < 5)
		{
			throw new ArgumentOutOfRangeException("pageInternalIndex", $"The pageInternalIndex {pageInternalIndex} must be a normal page index.");
		}
		if (pageInternalIndex < 10)
		{
			return (byte)(pageInternalIndex + 5);
		}
		if (pageInternalIndex < 15)
		{
			return (byte)(pageInternalIndex - 5);
		}
		throw new ArgumentOutOfRangeException("pageInternalIndex", $"The pageInternalIndex {pageInternalIndex} is out of range.");
	}

	public static byte GetOutlinePageInternalIndex(sbyte behaviorType)
	{
		return (byte)behaviorType;
	}

	public static byte GetNormalPageInternalIndex(sbyte direction, byte pageId)
	{
		return (byte)(5 + direction * 5 + pageId - 1);
	}

	public static byte GetPageId(byte pageInternalIndex)
	{
		if (pageInternalIndex >= 5)
		{
			return (byte)((pageInternalIndex - 5) % 5 + 1);
		}
		return 0;
	}

	public static bool IsPageRead(ushort readingState, byte pageInternalIndex)
	{
		return (readingState & (1 << (int)pageInternalIndex)) != 0;
	}

	public static ushort SetPageRead(ushort readingState, byte pageInternalIndex)
	{
		return (ushort)(readingState | (1 << (int)pageInternalIndex));
	}

	public static ushort SetPageUnread(ushort readingState, byte pageInternalIndex)
	{
		return (ushort)(readingState & ~(1 << (int)pageInternalIndex));
	}

	public static bool HasReadOutlinePage(ushort readingState, sbyte behaviorType)
	{
		byte outlinePageInternalIndex = GetOutlinePageInternalIndex(behaviorType);
		return IsPageRead(readingState, outlinePageInternalIndex);
	}

	public static bool HasReadOutlinePages(ushort readingState)
	{
		return (readingState & 0x1F) != 0;
	}

	public static int GetReadPagesCount(ushort readingState)
	{
		uint num = readingState;
		int num2 = 0;
		while (num != 0)
		{
			num &= num - 1;
			num2++;
		}
		return num2;
	}

	public static int GetReadNormalPagesCount(ushort readingState)
	{
		uint num = (uint)readingState >> 5;
		int num2 = 0;
		while (num != 0)
		{
			num &= num - 1;
			num2++;
		}
		return num2;
	}

	public static int GetCanActivateNormalPagesCount(ushort readingState)
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			byte b = (byte)(5 + i);
			byte b2 = (byte)(b + 5);
			if ((readingState & (1 << (int)b)) != 0 || (readingState & (1 << (int)b2)) != 0)
			{
				num++;
			}
		}
		return num;
	}

	public static bool IsReadNormalPagesMeetConditionOfBreakout(ushort readingState)
	{
		return GetCanActivateNormalPagesCount(readingState) >= 5;
	}

	public static byte GetNextPageToRead(ushort readingState)
	{
		for (int i = 0; i < 5; i++)
		{
			byte b = (byte)(5 + i);
			if ((readingState & (1 << (int)b)) == 0)
			{
				byte b2 = (byte)(b + 5);
				if ((readingState & (1 << (int)b2)) == 0)
				{
					return (byte)(i + 1);
				}
			}
		}
		return 6;
	}

	public static int CalcPagesToBeReadForActivation(ushort readingState)
	{
		int num = ((!HasReadOutlinePages(readingState)) ? 1 : 0);
		for (int i = 0; i < 5; i++)
		{
			byte b = (byte)(5 + i);
			byte b2 = (byte)(b + 5);
			if ((readingState & (1 << (int)b)) == 0 && (readingState & (1 << (int)b2)) == 0)
			{
				num++;
			}
		}
		return num;
	}

	public static ushort GenerateReadingStateFromSkillBook(byte pageTypes)
	{
		ushort readingState = 0;
		byte outlinePageInternalIndex = GetOutlinePageInternalIndex(SkillBookStateHelper.GetOutlinePageType(pageTypes));
		readingState = SetPageRead(readingState, outlinePageInternalIndex);
		for (byte b = 1; b < 6; b++)
		{
			outlinePageInternalIndex = GetNormalPageInternalIndex(SkillBookStateHelper.GetNormalPageType(pageTypes, b), b);
			readingState = SetPageRead(readingState, outlinePageInternalIndex);
		}
		return readingState;
	}

	public static byte GeneratePageTypesFromReadingState(IRandomSource random, ushort readingState)
	{
		byte pageTypes = 0;
		SpanList<byte> spanList = stackalloc byte[5];
		for (byte b = 0; b < 5; b++)
		{
			if (IsPageRead(readingState, b))
			{
				spanList.Add(b);
			}
		}
		int num = ((spanList.Count > 0) ? spanList.GetRandom(random) : random.Next(5));
		pageTypes = SkillBookStateHelper.SetOutlinePageType(pageTypes, (sbyte)num);
		spanList.Clear();
		for (byte b2 = 1; b2 < 6; b2++)
		{
			byte normalPageInternalIndex = GetNormalPageInternalIndex(0, b2);
			bool flag = IsPageRead(readingState, normalPageInternalIndex);
			byte normalPageInternalIndex2 = GetNormalPageInternalIndex(1, b2);
			bool flag2 = IsPageRead(readingState, normalPageInternalIndex2);
			sbyte direction = ((flag == flag2) ? CombatSkillDirection.GetRandomDirection(random) : ((!flag) ? ((sbyte)1) : ((sbyte)0)));
			pageTypes = SkillBookStateHelper.SetNormalPageType(pageTypes, b2, direction);
		}
		return pageTypes;
	}

	public static bool IsPageActive(ushort activationState, byte pageInternalIndex)
	{
		return (activationState & (1 << (int)pageInternalIndex)) != 0;
	}

	public static sbyte GetPageActiveDirection(ushort activationState, byte pageId)
	{
		int num = 5 + pageId - 1;
		if ((activationState & (1 << num)) != 0)
		{
			return 0;
		}
		int num2 = num + 5;
		if ((activationState & (1 << num2)) != 0)
		{
			return 1;
		}
		return -1;
	}

	public static ushort SetPageActive(ushort activationState, byte pageInternalIndex)
	{
		return (ushort)(activationState | (1 << (int)pageInternalIndex));
	}

	public static ushort SetPageInactive(ushort activationState, byte pageInternalIndex)
	{
		return (ushort)(activationState & ~(1 << (int)pageInternalIndex));
	}

	public static bool IsBrokenOut(ushort activationState)
	{
		return GetActiveOutlinePageType(activationState) >= 0;
	}

	public static bool CanGenerateBookFromActivationState(ushort activationState)
	{
		if (GetActiveOutlinePageType(activationState) >= 0)
		{
			return GetNormalPagesActivationCount(activationState) >= 5;
		}
		return false;
	}

	public static sbyte GetActiveOutlinePageType(ushort activationState)
	{
		uint num = activationState;
		for (int i = 0; i < 5; i++)
		{
			if ((num & 1) != 0)
			{
				return (sbyte)i;
			}
			num >>= 1;
		}
		return -1;
	}

	public static bool IsAllPagesActive(ushort activationState, sbyte direction)
	{
		return ((activationState >>> 5 + direction * 5) & 0x1F) == 31;
	}

	public static sbyte GetCombatSkillDirection(ushort activationState)
	{
		if (!IsBrokenOut(activationState))
		{
			return -1;
		}
		int normalPagesActivationCount = GetNormalPagesActivationCount(activationState, 0);
		int normalPagesActivationCount2 = GetNormalPagesActivationCount(activationState, 1);
		if (normalPagesActivationCount > normalPagesActivationCount2)
		{
			return 0;
		}
		if (normalPagesActivationCount < normalPagesActivationCount2)
		{
			return 1;
		}
		return -1;
	}

	public static int GetNormalPagesActivationCount(ushort activationState, sbyte direction)
	{
		uint num = (uint)((activationState >>> 5 + direction * 5) & 0x1F);
		int num2 = 0;
		while (num != 0)
		{
			num &= num - 1;
			num2++;
		}
		return num2;
	}

	public static int GetNormalPagesActivationCount(ushort activationState)
	{
		return GetNormalPagesActivationCount(activationState, 0) + GetNormalPagesActivationCount(activationState, 1);
	}

	public static ushort SwitchNormalPageDirect(ushort activationState, int pageId)
	{
		byte b = (byte)(5 + pageId);
		byte b2 = (byte)(b + 5);
		if ((activationState & (1 << (int)b)) != 0)
		{
			activationState = SetPageInactive(activationState, b);
			activationState = SetPageActive(activationState, b2);
		}
		else if ((activationState & (1 << (int)b2)) != 0)
		{
			activationState = SetPageInactive(activationState, b2);
			activationState = SetPageActive(activationState, b);
		}
		return activationState;
	}

	public unsafe static ushort GenerateRandomActivatedOutlinePage(IRandomSource random, ushort readingState, ushort activationState, sbyte behaviorType = -1)
	{
		if (behaviorType >= 0 && HasReadOutlinePage(readingState, behaviorType))
		{
			return (ushort)(activationState | (1 << (int)behaviorType));
		}
		uint num = (uint)(readingState & 0x1F);
		byte* ptr = stackalloc byte[5];
		int num2 = 0;
		for (int i = 0; i < 5; i++)
		{
			if ((num & 1) != 0)
			{
				ptr[num2++] = (byte)i;
			}
			num >>= 1;
		}
		byte b = ptr[(num2 != 1) ? random.Next(num2) : 0];
		return (ushort)(activationState | (1 << (int)b));
	}

	public static ushort GenerateRandomActivatedNormalPages(IRandomSource random, ushort readingState, ushort activationState)
	{
		for (int i = 0; i < 5; i++)
		{
			byte b = (byte)(5 + i);
			bool flag = (readingState & (1 << (int)b)) != 0;
			byte b2 = (byte)(b + 5);
			bool flag2 = (readingState & (1 << (int)b2)) != 0;
			if ((flag || flag2) && !IsPageActive(activationState, b) && !IsPageActive(activationState, b2))
			{
				byte b3 = ((!(flag && flag2)) ? (flag ? b : b2) : ((random.Next(2) == 0) ? b : b2));
				activationState |= (ushort)(1 << (int)b3);
			}
		}
		return activationState;
	}
}
