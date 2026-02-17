using System;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000828 RID: 2088
	public class CreateNewBornChildrenModification
	{
		// Token: 0x04001F65 RID: 8037
		public CreateIntelligentCharacterModification[] ChildrenMods;

		// Token: 0x04001F66 RID: 8038
		public bool IsInsect;

		// Token: 0x04001F67 RID: 8039
		public bool IsDystocia;

		// Token: 0x04001F68 RID: 8040
		public bool IsMotherDead;

		// Token: 0x04001F69 RID: 8041
		public bool IsMotherFree;

		// Token: 0x04001F6A RID: 8042
		public bool IsBabyFree;

		// Token: 0x04001F6B RID: 8043
		public bool AdoptedByKidnapper;

		// Token: 0x04001F6C RID: 8044
		public sbyte LegacyGrowingGradeState = -1;
	}
}
