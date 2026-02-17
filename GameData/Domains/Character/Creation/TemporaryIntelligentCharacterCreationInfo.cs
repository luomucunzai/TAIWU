using System;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Creation
{
	// Token: 0x02000840 RID: 2112
	public struct TemporaryIntelligentCharacterCreationInfo
	{
		// Token: 0x0400203E RID: 8254
		public Location Location;

		// Token: 0x0400203F RID: 8255
		public OrganizationInfo OrgInfo;

		// Token: 0x04002040 RID: 8256
		public short CharTemplateId;

		// Token: 0x04002041 RID: 8257
		public short? ActualAge;

		// Token: 0x04002042 RID: 8258
		public sbyte? Happiness;

		// Token: 0x04002043 RID: 8259
		public short? Morality;

		// Token: 0x04002044 RID: 8260
		public short? BaseAttraction;

		// Token: 0x04002045 RID: 8261
		public byte? MonkType;

		// Token: 0x04002046 RID: 8262
		public bool? IsCompletelyInfected;

		// Token: 0x04002047 RID: 8263
		public sbyte? ConsummateLevel;

		// Token: 0x04002048 RID: 8264
		public ResourceInts Resources;

		// Token: 0x04002049 RID: 8265
		public sbyte? GoodAtCombatSkillType;

		// Token: 0x0400204A RID: 8266
		public sbyte? GoodAtLifeSkillType;

		// Token: 0x0400204B RID: 8267
		public bool HairNoSkinHead;
	}
}
