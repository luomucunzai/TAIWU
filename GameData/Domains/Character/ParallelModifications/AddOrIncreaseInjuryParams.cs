using System;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000825 RID: 2085
	public readonly struct AddOrIncreaseInjuryParams
	{
		// Token: 0x0600756F RID: 30063 RVA: 0x0044A3AD File Offset: 0x004485AD
		public AddOrIncreaseInjuryParams(sbyte bodyPartType, bool isInnerInjury, sbyte injuryValue)
		{
			this.BodyPartType = bodyPartType;
			this.IsInnerInjury = isInnerInjury;
			this.InjuryValue = injuryValue;
		}

		// Token: 0x04001F4E RID: 8014
		public readonly sbyte BodyPartType;

		// Token: 0x04001F4F RID: 8015
		public readonly bool IsInnerInjury;

		// Token: 0x04001F50 RID: 8016
		public readonly sbyte InjuryValue;
	}
}
