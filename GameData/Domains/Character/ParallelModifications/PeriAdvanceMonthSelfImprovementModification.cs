using System;
using System.Runtime.CompilerServices;
using GameData.Domains.Item;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000830 RID: 2096
	public class PeriAdvanceMonthSelfImprovementModification
	{
		// Token: 0x1700048D RID: 1165
		// (get) Token: 0x0600757F RID: 30079 RVA: 0x0044A588 File Offset: 0x00448788
		public bool IsChanged
		{
			get
			{
				return this.LoopingNeigong.Item1 >= 0 || this.ConsummateLevelChanged || this.ResourcesChanged || this.ExtraNeiliChanged || this.ReadingResult.Item1 != null;
			}
		}

		// Token: 0x06007580 RID: 30080 RVA: 0x0044A5C1 File Offset: 0x004487C1
		public PeriAdvanceMonthSelfImprovementModification(Character character)
		{
			this.Character = character;
			this.LoopingNeigong = new ValueTuple<short, short, short>(-1, 0, 0);
			this.ReadingResult = new ValueTuple<SkillBook, int, byte, sbyte>(null, -1, 0, 0);
			this.ExtraNeiliAllocationProgress = new int[4];
		}

		// Token: 0x04001F8E RID: 8078
		public readonly Character Character;

		// Token: 0x04001F8F RID: 8079
		[TupleElementNames(new string[]
		{
			"combatSkillTemplateId",
			"neili",
			"qiDisorder"
		})]
		public ValueTuple<short, short, short> LoopingNeigong;

		// Token: 0x04001F90 RID: 8080
		public bool ConsummateLevelChanged;

		// Token: 0x04001F91 RID: 8081
		public int ConsummateLevelProgress;

		// Token: 0x04001F92 RID: 8082
		public bool ResourcesChanged;

		// Token: 0x04001F93 RID: 8083
		public bool ExtraNeiliChanged;

		// Token: 0x04001F94 RID: 8084
		[TupleElementNames(new string[]
		{
			"readingBook",
			"learnedSkillIndex",
			"page",
			"succeedPageCount"
		})]
		public ValueTuple<SkillBook, int, byte, sbyte> ReadingResult;

		// Token: 0x04001F95 RID: 8085
		[TupleElementNames(new string[]
		{
			"readingBook",
			"learnedSkillIndex",
			"page",
			"succeed"
		})]
		[Obsolete]
		public ValueTuple<SkillBook, int, byte, bool> ReadingProgress;

		// Token: 0x04001F96 RID: 8086
		public int[] ExtraNeiliAllocationProgress;
	}
}
