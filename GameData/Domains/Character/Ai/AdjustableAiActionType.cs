using System;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000843 RID: 2115
	public class AdjustableAiActionType
	{
		// Token: 0x04002052 RID: 8274
		public const sbyte InvalidSubType = -1;

		// Token: 0x04002053 RID: 8275
		public const sbyte AcquireItem = 0;

		// Token: 0x04002054 RID: 8276
		public const sbyte AcquireResource = 1;

		// Token: 0x04002055 RID: 8277
		public const sbyte AcquireCombatSkill = 2;

		// Token: 0x04002056 RID: 8278
		public const sbyte AcquireLifeSkill = 3;

		// Token: 0x04002057 RID: 8279
		public const sbyte MakeLove = 4;

		// Token: 0x04002058 RID: 8280
		public const sbyte Harm = 5;

		// Token: 0x04002059 RID: 8281
		public const sbyte EatForbiddenFood = 6;

		// Token: 0x0400205A RID: 8282
		public const sbyte KillInPrivate = 7;

		// Token: 0x0400205B RID: 8283
		public const sbyte KidnapInPrivate = 8;

		// Token: 0x0400205C RID: 8284
		public const sbyte AddPoisonToItem = 9;

		// Token: 0x0400205D RID: 8285
		public const sbyte StartRelation = 10;

		// Token: 0x0400205E RID: 8286
		public const sbyte LifeSkillCombatForceSilent = 11;
	}
}
