using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C9 RID: 1737
	public static class CRandom
	{
		// Token: 0x060066EF RID: 26351 RVA: 0x003AF48C File Offset: 0x003AD68C
		public static void GenerateAddRandomInjuryPool(IList<sbyte> prefer, IList<sbyte> normal, Injuries injuries, bool inner, sbyte bodyPart = -1, bool isWorsen = false)
		{
			prefer.Clear();
			normal.Clear();
			foreach (sbyte part in CRandom.IterBodyPart(bodyPart))
			{
				sbyte injuryValue = injuries.Get(part, inner);
				bool flag = injuryValue < 6 && (!isWorsen || injuryValue > 0);
				if (flag)
				{
					prefer.Add(part);
				}
				else
				{
					bool flag2 = isWorsen && injuryValue > 0;
					if (flag2)
					{
						normal.Add(part);
					}
				}
			}
		}

		// Token: 0x060066F0 RID: 26352 RVA: 0x003AF52C File Offset: 0x003AD72C
		public static void GenerateRemoveInjuryRandomPool(ICollection<sbyte> bodyPartRandomPool, Injuries injuries, bool isInner, sbyte bodyPartType = -1)
		{
			foreach (sbyte bodyPart in CRandom.IterBodyPart(bodyPartType))
			{
				sbyte injuryValue = injuries.Get(bodyPart, isInner);
				for (int i = 0; i < (int)injuryValue; i++)
				{
					bodyPartRandomPool.Add(bodyPart);
				}
			}
		}

		// Token: 0x060066F1 RID: 26353 RVA: 0x003AF59C File Offset: 0x003AD79C
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool RandomIsInner(this IRandomSource random, bool anyInner, bool anyOuter)
		{
			return !anyOuter || (anyInner && random.CheckPercentProb(50));
		}

		// Token: 0x060066F2 RID: 26354 RVA: 0x003AF5B4 File Offset: 0x003AD7B4
		public static IEnumerable<sbyte> IterBodyPart(sbyte bodyPartType)
		{
			return CRandom.IterAnyType(bodyPartType, 7);
		}

		// Token: 0x060066F3 RID: 26355 RVA: 0x003AF5D0 File Offset: 0x003AD7D0
		public static IEnumerable<sbyte> IterPoisonType(sbyte poisonType)
		{
			return CRandom.IterAnyType(poisonType, 6);
		}

		// Token: 0x060066F4 RID: 26356 RVA: 0x003AF5E9 File Offset: 0x003AD7E9
		private static IEnumerable<sbyte> IterAnyType(sbyte typeValue, sbyte typeCount)
		{
			int begin = (int)((typeValue < 0 || typeValue >= typeCount) ? 0 : typeValue);
			int end = (int)((typeValue < 0 || typeValue >= typeCount) ? typeCount : (typeValue + 1));
			int num;
			for (int i = begin; i < end; i = num + 1)
			{
				yield return (sbyte)i;
				num = i;
			}
			yield break;
		}

		// Token: 0x02000B73 RID: 2931
		public enum EOuterAndInnerType
		{
			// Token: 0x040030D9 RID: 12505
			None,
			// Token: 0x040030DA RID: 12506
			Both,
			// Token: 0x040030DB RID: 12507
			Inner,
			// Token: 0x040030DC RID: 12508
			Outer
		}
	}
}
