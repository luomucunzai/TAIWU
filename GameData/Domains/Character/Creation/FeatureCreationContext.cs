using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Character.Creation
{
	// Token: 0x0200083D RID: 2109
	public struct FeatureCreationContext
	{
		// Token: 0x060075F9 RID: 30201 RVA: 0x0044E648 File Offset: 0x0044C848
		public FeatureCreationContext(Character character, ref IntelligentCharacterCreationInfo charCreationInfo)
		{
			this.FeatureIds = character.GetFeatureIds();
			this.PotentialFeatureIds = character.GetPotentialFeatureIds();
			this.Mother = charCreationInfo.Mother;
			this.Father = charCreationInfo.ActualFather;
			this.DeadFather = charCreationInfo.ActualDeadFather;
			this.PregnantState = charCreationInfo.PregnantState;
			this.Gender = character.GetGender();
			this.BirthMonth = character.GetBirthMonth();
			this.CurrAge = character.GetCurrAge();
			this.PotentialFeaturesAge = ((AgeGroup.GetAgeGroup(this.CurrAge) != 2) ? this.CurrAge : -1);
			this.DestinyType = charCreationInfo.DestinyType;
			this.RandomFeaturesAtCreating = Character.Instance[charCreationInfo.CharTemplateId].RandomFeaturesAtCreating;
			this.AllGoodBasicFeature = false;
			this.IsProtagonist = false;
		}

		// Token: 0x060075FA RID: 30202 RVA: 0x0044E714 File Offset: 0x0044C914
		public FeatureCreationContext(Character character)
		{
			this.FeatureIds = character.GetFeatureIds();
			this.PotentialFeatureIds = character.GetPotentialFeatureIds();
			this.Mother = null;
			this.Father = null;
			this.DeadFather = null;
			this.PregnantState = null;
			this.Gender = character.GetGender();
			this.BirthMonth = character.GetBirthMonth();
			this.CurrAge = character.GetCurrAge();
			this.PotentialFeaturesAge = ((AgeGroup.GetAgeGroup(this.CurrAge) != 2) ? this.CurrAge : -1);
			this.RandomFeaturesAtCreating = Character.Instance[character.GetTemplateId()].RandomFeaturesAtCreating;
			this.DestinyType = -1;
			this.AllGoodBasicFeature = false;
			this.IsProtagonist = false;
		}

		// Token: 0x04002008 RID: 8200
		public List<short> FeatureIds;

		// Token: 0x04002009 RID: 8201
		public List<short> PotentialFeatureIds;

		// Token: 0x0400200A RID: 8202
		public sbyte Gender;

		// Token: 0x0400200B RID: 8203
		public sbyte BirthMonth;

		// Token: 0x0400200C RID: 8204
		public short CurrAge;

		// Token: 0x0400200D RID: 8205
		public PregnantState PregnantState;

		// Token: 0x0400200E RID: 8206
		public Character Mother;

		// Token: 0x0400200F RID: 8207
		public Character Father;

		// Token: 0x04002010 RID: 8208
		public DeadCharacter DeadFather;

		// Token: 0x04002011 RID: 8209
		public bool RandomFeaturesAtCreating;

		// Token: 0x04002012 RID: 8210
		public short PotentialFeaturesAge;

		// Token: 0x04002013 RID: 8211
		public int DestinyType;

		// Token: 0x04002014 RID: 8212
		public bool AllGoodBasicFeature;

		// Token: 0x04002015 RID: 8213
		public bool IsProtagonist;
	}
}
