using System;
using GameData.Utilities;

namespace GameData.Domains.Character;

public static class CombatSkillAttainmentPanelsHelper
{
	private const int Size = 252;

	public unsafe static void Initialize(short[] panels)
	{
		fixed (short* pDest = panels)
		{
			CollectionUtils.SetMemoryToMinusOne((byte*)pDest, 252);
		}
	}

	public unsafe static void CopyAll(short[] src, short[] dest)
	{
		fixed (short* source = src)
		{
			fixed (short* destination = dest)
			{
				Buffer.MemoryCopy(source, destination, 252L, 252L);
			}
		}
	}

	public unsafe static bool EqualAll(short[] lhs, short[] rhs)
	{
		fixed (short* pLhs = lhs)
		{
			fixed (short* pRhs = rhs)
			{
				return CollectionUtils.Equals((byte*)pLhs, (byte*)pRhs, 252);
			}
		}
	}

	public unsafe static void GetPanel(short[] panels, sbyte combatSkillType, short* pCombatSkillTemplateIds)
	{
		fixed (short* ptr = panels)
		{
			byte* ptr2 = (byte*)ptr + 18 * combatSkillType;
			*(long*)pCombatSkillTemplateIds = *(long*)ptr2;
			((long*)pCombatSkillTemplateIds)[1] = ((long*)ptr2)[1];
			pCombatSkillTemplateIds[8] = ((short*)ptr2)[8];
		}
	}

	public unsafe static void SetPanel(short[] panels, sbyte combatSkillType, short* pCombatSkillTemplateIds)
	{
		fixed (short* ptr = panels)
		{
			byte* num = (byte*)ptr + 18 * combatSkillType;
			*(long*)num = *(long*)pCombatSkillTemplateIds;
			((long*)num)[1] = ((long*)pCombatSkillTemplateIds)[1];
			((short*)num)[8] = pCombatSkillTemplateIds[8];
		}
	}

	public static short Get(short[] panels, sbyte combatSkillType, sbyte grade)
	{
		int num = 9 * combatSkillType + grade;
		return panels[num];
	}

	public static void Set(short[] panels, sbyte combatSkillType, sbyte grade, short combatSkillTemplateId)
	{
		int num = 9 * combatSkillType + grade;
		panels[num] = combatSkillTemplateId;
	}
}
