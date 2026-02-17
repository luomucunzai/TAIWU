using System;
using GameData.Domains.Character.AvatarSystem;
using GameData.Domains.Map;

namespace GameData.Domains.Character.Creation
{
	// Token: 0x0200083F RID: 2111
	public struct IntelligentCharacterCreationInfo
	{
		// Token: 0x060075FC RID: 30204 RVA: 0x0044E7D4 File Offset: 0x0044C9D4
		public IntelligentCharacterCreationInfo(Location location, OrganizationInfo orgInfo, short charTemplateId)
		{
			this.Location = location;
			this.OrgInfo = orgInfo;
			this.CharTemplateId = charTemplateId;
			this.GrowingSectId = -1;
			this.GrowingSectGrade = -1;
			this.MotherCharId = -1;
			this.Mother = null;
			this.PregnantState = null;
			this.FatherCharId = -1;
			this.Father = null;
			this.DeadFather = null;
			this.ActualFatherCharId = -1;
			this.ActualFather = null;
			this.ActualDeadFather = null;
			this.ReferenceFullName = default(FullName);
			this.MultipleBirthCount = 1;
			this.Age = -1;
			this.BirthMonth = -1;
			this.BaseAttraction = -1;
			this.Avatar = null;
			this.SpecifyGenome = false;
			this.Genome = default(Genome);
			this.ReincarnationCharId = -1;
			this.DestinyType = -1;
			this.LifeSkillsLowerBound = null;
			this.CombatSkillsLowerBound = null;
			this.InitializeSectSkills = true;
			this.AllowRandomGrowingGradeAdjust = false;
			this.CombatSkillQualificationGrowthType = -1;
			this.LifeSkillQualificationGrowthType = -1;
			this.Gender = -1;
			this.Transgender = false;
			this.Race = -1;
			this.LifeSkillsAdjustBonus = null;
			this.CombatSkillsAdjustBonus = null;
			this.DisableBeReincarnatedBySavedSoul = false;
		}

		// Token: 0x04002017 RID: 8215
		public readonly Location Location;

		// Token: 0x04002018 RID: 8216
		public readonly OrganizationInfo OrgInfo;

		// Token: 0x04002019 RID: 8217
		public readonly short CharTemplateId;

		// Token: 0x0400201A RID: 8218
		public sbyte GrowingSectId;

		// Token: 0x0400201B RID: 8219
		public sbyte GrowingSectGrade;

		// Token: 0x0400201C RID: 8220
		public int MotherCharId;

		// Token: 0x0400201D RID: 8221
		public Character Mother;

		// Token: 0x0400201E RID: 8222
		public PregnantState PregnantState;

		// Token: 0x0400201F RID: 8223
		public int FatherCharId;

		// Token: 0x04002020 RID: 8224
		public Character Father;

		// Token: 0x04002021 RID: 8225
		public DeadCharacter DeadFather;

		// Token: 0x04002022 RID: 8226
		public int ActualFatherCharId;

		// Token: 0x04002023 RID: 8227
		public Character ActualFather;

		// Token: 0x04002024 RID: 8228
		public DeadCharacter ActualDeadFather;

		// Token: 0x04002025 RID: 8229
		public FullName ReferenceFullName;

		// Token: 0x04002026 RID: 8230
		public sbyte MultipleBirthCount;

		// Token: 0x04002027 RID: 8231
		public short Age;

		// Token: 0x04002028 RID: 8232
		public sbyte BirthMonth;

		// Token: 0x04002029 RID: 8233
		public short BaseAttraction;

		// Token: 0x0400202A RID: 8234
		public AvatarData Avatar;

		// Token: 0x0400202B RID: 8235
		public bool SpecifyGenome;

		// Token: 0x0400202C RID: 8236
		public Genome Genome;

		// Token: 0x0400202D RID: 8237
		public int ReincarnationCharId;

		// Token: 0x0400202E RID: 8238
		public int DestinyType;

		// Token: 0x0400202F RID: 8239
		public short[] LifeSkillsLowerBound;

		// Token: 0x04002030 RID: 8240
		public short[] CombatSkillsLowerBound;

		// Token: 0x04002031 RID: 8241
		public bool InitializeSectSkills;

		// Token: 0x04002032 RID: 8242
		public MainAttributes ParentMainAttributeValues;

		// Token: 0x04002033 RID: 8243
		public LifeSkillShorts ParentLifeSkillQualificationValues;

		// Token: 0x04002034 RID: 8244
		public CombatSkillShorts ParentCombatSkillQualificationValues;

		// Token: 0x04002035 RID: 8245
		public bool AllowRandomGrowingGradeAdjust;

		// Token: 0x04002036 RID: 8246
		public sbyte CombatSkillQualificationGrowthType;

		// Token: 0x04002037 RID: 8247
		public sbyte LifeSkillQualificationGrowthType;

		// Token: 0x04002038 RID: 8248
		public sbyte Gender;

		// Token: 0x04002039 RID: 8249
		public bool Transgender;

		// Token: 0x0400203A RID: 8250
		public sbyte Race;

		// Token: 0x0400203B RID: 8251
		[Obsolete]
		public short[] LifeSkillsAdjustBonus;

		// Token: 0x0400203C RID: 8252
		[Obsolete]
		public short[] CombatSkillsAdjustBonus;

		// Token: 0x0400203D RID: 8253
		public bool DisableBeReincarnatedBySavedSoul;
	}
}
