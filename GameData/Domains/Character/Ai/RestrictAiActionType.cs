using System;

namespace GameData.Domains.Character.Ai
{
	// Token: 0x02000848 RID: 2120
	[Obsolete("Use AdjustableAiActionType instead.")]
	public static class RestrictAiActionType
	{
		// Token: 0x04002082 RID: 8322
		public const sbyte InvalidSubType = -1;

		// Token: 0x04002083 RID: 8323
		public const sbyte AcquireItem = 0;

		// Token: 0x04002084 RID: 8324
		public const sbyte AcquireResource = 1;

		// Token: 0x04002085 RID: 8325
		public const sbyte AcquireCombatSkill = 2;

		// Token: 0x04002086 RID: 8326
		public const sbyte AcquireLifeSkill = 3;

		// Token: 0x04002087 RID: 8327
		public const sbyte MakeLove = 4;

		// Token: 0x04002088 RID: 8328
		public const sbyte Harm = 5;

		// Token: 0x04002089 RID: 8329
		public const sbyte EatForbiddenFood = 6;

		// Token: 0x0400208A RID: 8330
		public const sbyte KillInPrivate = 7;

		// Token: 0x0400208B RID: 8331
		public const sbyte KidnapInPrivate = 8;

		// Token: 0x0400208C RID: 8332
		public const sbyte AddPoisonToItem = 9;

		// Token: 0x0400208D RID: 8333
		public const sbyte StartRelation = 10;
	}
}
