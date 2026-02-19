using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

public static class CRandom
{
	public enum EOuterAndInnerType
	{
		None,
		Both,
		Inner,
		Outer
	}

	public static void GenerateAddRandomInjuryPool(IList<sbyte> prefer, IList<sbyte> normal, Injuries injuries, bool inner, sbyte bodyPart = -1, bool isWorsen = false)
	{
		prefer.Clear();
		normal.Clear();
		foreach (sbyte item in IterBodyPart(bodyPart))
		{
			sbyte b = injuries.Get(item, inner);
			if (b < 6 && (!isWorsen || b > 0))
			{
				prefer.Add(item);
			}
			else if (isWorsen && b > 0)
			{
				normal.Add(item);
			}
		}
	}

	public static void GenerateRemoveInjuryRandomPool(ICollection<sbyte> bodyPartRandomPool, Injuries injuries, bool isInner, sbyte bodyPartType = -1)
	{
		foreach (sbyte item in IterBodyPart(bodyPartType))
		{
			sbyte b = injuries.Get(item, isInner);
			for (int i = 0; i < b; i++)
			{
				bodyPartRandomPool.Add(item);
			}
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static bool RandomIsInner(this IRandomSource random, bool anyInner, bool anyOuter)
	{
		return !anyOuter || (anyInner && random.CheckPercentProb(50));
	}

	public static IEnumerable<sbyte> IterBodyPart(sbyte bodyPartType)
	{
		return IterAnyType(bodyPartType, 7);
	}

	public static IEnumerable<sbyte> IterPoisonType(sbyte poisonType)
	{
		return IterAnyType(poisonType, 6);
	}

	private static IEnumerable<sbyte> IterAnyType(sbyte typeValue, sbyte typeCount)
	{
		int begin = ((typeValue >= 0 && typeValue < typeCount) ? typeValue : 0);
		int end = ((typeValue < 0 || typeValue >= typeCount) ? typeCount : (typeValue + 1));
		for (int i = begin; i < end; i++)
		{
			yield return (sbyte)i;
		}
	}
}
