using System;
using System.Collections.Generic;

namespace GameData.Domains.Taiwu.DebateAI
{
	// Token: 0x0200006E RID: 110
	public class DebateAiConstants
	{
		// Token: 0x17000099 RID: 153
		// (get) Token: 0x06001668 RID: 5736 RVA: 0x001510D9 File Offset: 0x0014F2D9
		public static List<int[]> AttackLineWeight
		{
			get
			{
				return GlobalConfig.Instance.AttackLineWeight;
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x06001669 RID: 5737 RVA: 0x001510E5 File Offset: 0x0014F2E5
		public static List<int[]> MidLineWeight
		{
			get
			{
				return GlobalConfig.Instance.MidLineWeight;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x0600166A RID: 5738 RVA: 0x001510F1 File Offset: 0x0014F2F1
		public static List<int[]> DefenseLineWeight
		{
			get
			{
				return GlobalConfig.Instance.DefenseLineWeight;
			}
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x0600166B RID: 5739 RVA: 0x001510FD File Offset: 0x0014F2FD
		public static int[] EarlyBases
		{
			get
			{
				return GlobalConfig.Instance.EarlyBases;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x0600166C RID: 5740 RVA: 0x00151109 File Offset: 0x0014F309
		public static int[] MidBases
		{
			get
			{
				return GlobalConfig.Instance.MidBases;
			}
		}

		// Token: 0x1700009E RID: 158
		// (get) Token: 0x0600166D RID: 5741 RVA: 0x00151115 File Offset: 0x0014F315
		public static int[] LateBases
		{
			get
			{
				return GlobalConfig.Instance.LateBases;
			}
		}

		// Token: 0x1700009F RID: 159
		// (get) Token: 0x0600166E RID: 5742 RVA: 0x00151121 File Offset: 0x0014F321
		public static List<int[]> EarlyStrategyPoint
		{
			get
			{
				return GlobalConfig.Instance.EarlyStrategyPoint;
			}
		}

		// Token: 0x170000A0 RID: 160
		// (get) Token: 0x0600166F RID: 5743 RVA: 0x0015112D File Offset: 0x0014F32D
		public static List<int[]> MidStrategyPoint
		{
			get
			{
				return GlobalConfig.Instance.MidStrategyPoint;
			}
		}

		// Token: 0x170000A1 RID: 161
		// (get) Token: 0x06001670 RID: 5744 RVA: 0x00151139 File Offset: 0x0014F339
		public static List<int[]> LateStrategyPoint
		{
			get
			{
				return GlobalConfig.Instance.LateStrategyPoint;
			}
		}

		// Token: 0x170000A2 RID: 162
		// (get) Token: 0x06001671 RID: 5745 RVA: 0x00151145 File Offset: 0x0014F345
		public static int[] DamageLineWeight
		{
			get
			{
				return GlobalConfig.Instance.DamageLineWeight;
			}
		}

		// Token: 0x170000A3 RID: 163
		// (get) Token: 0x06001672 RID: 5746 RVA: 0x00151151 File Offset: 0x0014F351
		public static int[] DamagedLineWeight
		{
			get
			{
				return GlobalConfig.Instance.DamagedLineWeight;
			}
		}

		// Token: 0x170000A4 RID: 164
		// (get) Token: 0x06001673 RID: 5747 RVA: 0x0015115D File Offset: 0x0014F35D
		public static List<int[]> StateGamePointPressureInfluence
		{
			get
			{
				return GlobalConfig.Instance.StateGamePointPressureInfluence;
			}
		}

		// Token: 0x170000A5 RID: 165
		// (get) Token: 0x06001674 RID: 5748 RVA: 0x00151169 File Offset: 0x0014F369
		public static List<int[]> StatePawnCountInfluence
		{
			get
			{
				return GlobalConfig.Instance.StatePawnCountInfluence;
			}
		}

		// Token: 0x170000A6 RID: 166
		// (get) Token: 0x06001675 RID: 5749 RVA: 0x00151175 File Offset: 0x0014F375
		public static int[] StateRoundInfluence
		{
			get
			{
				return GlobalConfig.Instance.StateRoundInfluence;
			}
		}

		// Token: 0x170000A7 RID: 167
		// (get) Token: 0x06001676 RID: 5750 RVA: 0x00151181 File Offset: 0x0014F381
		public static int EgoisticNodeEffectWeightPercent
		{
			get
			{
				return GlobalConfig.Instance.EgoisticNodeEffectWeightPercent;
			}
		}

		// Token: 0x170000A8 RID: 168
		// (get) Token: 0x06001677 RID: 5751 RVA: 0x0015118D File Offset: 0x0014F38D
		public static int[] EvenNodeEffectMaxGradeProb
		{
			get
			{
				return GlobalConfig.Instance.EvenNodeEffectMaxGradeProb;
			}
		}

		// Token: 0x170000A9 RID: 169
		// (get) Token: 0x06001678 RID: 5752 RVA: 0x00151199 File Offset: 0x0014F399
		public static int RoundBeforeEarly
		{
			get
			{
				return GlobalConfig.Instance.RoundBeforeEarly;
			}
		}

		// Token: 0x170000AA RID: 170
		// (get) Token: 0x06001679 RID: 5753 RVA: 0x001511A5 File Offset: 0x0014F3A5
		public static int MinGradeIfEnoughBases
		{
			get
			{
				return GlobalConfig.Instance.MinGradeIfEnoughBases;
			}
		}

		// Token: 0x170000AB RID: 171
		// (get) Token: 0x0600167A RID: 5754 RVA: 0x001511B1 File Offset: 0x0014F3B1
		public static int[] ZeroGradePawnProb
		{
			get
			{
				return GlobalConfig.Instance.ZeroGradePawnProb;
			}
		}

		// Token: 0x170000AC RID: 172
		// (get) Token: 0x0600167B RID: 5755 RVA: 0x001511BD File Offset: 0x0014F3BD
		public static int MakeMoveOnOverwhelmingLineProb
		{
			get
			{
				return GlobalConfig.Instance.MakeMoveOnOverwhelmingLineProb;
			}
		}

		// Token: 0x170000AD RID: 173
		// (get) Token: 0x0600167C RID: 5756 RVA: 0x001511C9 File Offset: 0x0014F3C9
		public static int RemoveStrategyTargetPawnBasesPercent
		{
			get
			{
				return GlobalConfig.Instance.RemoveStrategyTargetPawnBasesPercent;
			}
		}
	}
}
