using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using GameData.Domains.Character;

namespace GameData.Domains.Combat.Ai.Memory
{
	// Token: 0x02000732 RID: 1842
	public class RecordCollection
	{
		// Token: 0x060068FB RID: 26875 RVA: 0x003B90A0 File Offset: 0x003B72A0
		public RecordCollection()
		{
			this.MaxHits = new HitOrAvoidInts[11];
			this.MaxAvoid = new HitOrAvoidInts(new int[4]);
			this.MaxPenetrates = new OuterAndInnerInts[11];
			this.MaxPenetrateResist = new OuterAndInnerInts(0, 0);
			this.MaxDamages = new OuterAndInnerInts[11];
			this.MaxMindDamages = new int[11];
			this.SkillRecord = new Dictionary<short, ValueTuple<int, int>>();
			this.WeaponRecord = new Dictionary<int, ValueTuple<int, int>>();
		}

		// Token: 0x060068FC RID: 26876 RVA: 0x003B9120 File Offset: 0x003B7320
		public static int GetIndexByDistance(short distance)
		{
			return (int)(distance / 10 - 2);
		}

		// Token: 0x060068FD RID: 26877 RVA: 0x003B9138 File Offset: 0x003B7338
		public void ClearAll()
		{
			for (int i = 0; i < 11; i++)
			{
				this.MaxHits[i].Initialize();
				this.MaxPenetrates[i] = new OuterAndInnerInts(0, 0);
				this.MaxDamages[i] = new OuterAndInnerInts(0, 0);
				this.MaxMindDamages[i] = 0;
			}
			this.MaxAvoid.Initialize();
			this.MaxPenetrateResist = new OuterAndInnerInts(0, 0);
			this.SkillRecord.Clear();
			this.WeaponRecord.Clear();
		}

		// Token: 0x060068FE RID: 26878 RVA: 0x003B91CC File Offset: 0x003B73CC
		public int GetSkillRecordMaxScore()
		{
			int maxScore = 0;
			foreach (ValueTuple<int, int> record in this.SkillRecord.Values)
			{
				maxScore = Math.Max(maxScore, record.Item1);
			}
			return maxScore;
		}

		// Token: 0x04001CD8 RID: 7384
		private const int DistanceCount = 11;

		// Token: 0x04001CD9 RID: 7385
		public const int BlockMemoryCount = 6;

		// Token: 0x04001CDA RID: 7386
		public readonly HitOrAvoidInts[] MaxHits;

		// Token: 0x04001CDB RID: 7387
		public HitOrAvoidInts MaxAvoid;

		// Token: 0x04001CDC RID: 7388
		public readonly OuterAndInnerInts[] MaxPenetrates;

		// Token: 0x04001CDD RID: 7389
		public OuterAndInnerInts MaxPenetrateResist;

		// Token: 0x04001CDE RID: 7390
		public readonly OuterAndInnerInts[] MaxDamages;

		// Token: 0x04001CDF RID: 7391
		public readonly int[] MaxMindDamages;

		// Token: 0x04001CE0 RID: 7392
		[TupleElementNames(new string[]
		{
			"score",
			"zeroScoreCount"
		})]
		public readonly Dictionary<short, ValueTuple<int, int>> SkillRecord;

		// Token: 0x04001CE1 RID: 7393
		[TupleElementNames(new string[]
		{
			"score",
			"zeroScoreCount"
		})]
		public readonly Dictionary<int, ValueTuple<int, int>> WeaponRecord;
	}
}
