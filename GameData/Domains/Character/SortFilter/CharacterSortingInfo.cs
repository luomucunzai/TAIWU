using System;
using System.Text;
using GameData.Domains.Character.Relation;
using GameData.Domains.Combat;

namespace GameData.Domains.Character.SortFilter
{
	// Token: 0x0200081F RID: 2079
	public class CharacterSortingInfo
	{
		// Token: 0x0600754E RID: 30030 RVA: 0x0044963C File Offset: 0x0044783C
		public CharacterSortingInfo(Character character, Encoding encoding, CharacterSortFilter characterSortFilter = null)
		{
			this.Character = character;
			int charId = character.GetId();
			int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
			this.FavorabilityToTaiwu = DomainManager.Character.GetFavorability(charId, taiwuCharId);
			ValueTuple<string, string> monasticTitleOrDisplayName = DomainManager.Character.GetMonasticTitleOrDisplayName(charId);
			string surname = monasticTitleOrDisplayName.Item1;
			string givenName = monasticTitleOrDisplayName.Item2;
			this.NameBytes = encoding.GetBytes(surname + givenName);
			this.HitValues = character.GetHitValues();
			this.AvoidValues = character.GetAvoidValues();
			this.Morality = character.GetMorality();
			this.Fame = character.GetFame();
			this.Attraction = character.GetAttraction();
			this.HealthPercentage = (float)character.GetHealth() / (float)character.GetLeftMaxHealth(false);
			this.KidnappedCharCount = (character.IsActiveExternalRelationState(2) ? DomainManager.Character.GetKidnappedCharacters(charId).GetCount() : 0);
			this.AttackMedal = character.GetFeatureMedalValue(0);
			this.DefenseMedal = character.GetFeatureMedalValue(1);
			this.WisdomMedal = character.GetFeatureMedalValue(2);
			this.DefeatMarkCount = (sbyte)CombatDomain.GetDefeatMarksCountOutOfCombat(character);
			this.LifeSkillAgeAdjust = character.GetLifeSkillQualificationAgeAdjust();
			this.CombatSkillAgeAdjust = character.GetCombatSkillQualificationAgeAdjust();
			this.BehaviorType = GameData.Domains.Character.BehaviorType.GetBehaviorType(this.Morality);
			this.FavorType = FavorabilityType.GetFavorabilityType(this.FavorabilityToTaiwu);
			this.AttractionType = GameData.Domains.Character.AttractionType.GetAttractionType(this.Attraction);
			this.HappinessType = character.GetHappiness();
			this.FameType = character.GetFameType();
			bool flag = this.Character.GetOrganizationInfo().OrgTemplateId == 16;
			if (flag)
			{
				this.WorkStatus = DomainManager.Taiwu.GetVillagerWorkStatus(this.Character);
				this.LivingStatus = DomainManager.Building.GetLivingStatus(this.Character.GetId());
				this.VillagerNeedWaitTime = DomainManager.Taiwu.GetVillagerNeedWaitTime(charId, characterSortFilter);
			}
		}

		// Token: 0x0600754F RID: 30031 RVA: 0x00449811 File Offset: 0x00447A11
		public int GetId()
		{
			return this.Character.GetId();
		}

		// Token: 0x04001F22 RID: 7970
		public Character Character;

		// Token: 0x04001F23 RID: 7971
		public byte[] NameBytes;

		// Token: 0x04001F24 RID: 7972
		public float HealthPercentage;

		// Token: 0x04001F25 RID: 7973
		public short FavorabilityToTaiwu;

		// Token: 0x04001F26 RID: 7974
		public int KidnappedCharCount;

		// Token: 0x04001F27 RID: 7975
		public int AttackMedal;

		// Token: 0x04001F28 RID: 7976
		public int DefenseMedal;

		// Token: 0x04001F29 RID: 7977
		public int WisdomMedal;

		// Token: 0x04001F2A RID: 7978
		public sbyte DefeatMarkCount;

		// Token: 0x04001F2B RID: 7979
		public sbyte CombatSkillAgeAdjust;

		// Token: 0x04001F2C RID: 7980
		public sbyte LifeSkillAgeAdjust;

		// Token: 0x04001F2D RID: 7981
		public short Morality;

		// Token: 0x04001F2E RID: 7982
		public sbyte Fame;

		// Token: 0x04001F2F RID: 7983
		public short Attraction;

		// Token: 0x04001F30 RID: 7984
		public HitOrAvoidInts HitValues;

		// Token: 0x04001F31 RID: 7985
		public HitOrAvoidInts AvoidValues;

		// Token: 0x04001F32 RID: 7986
		public sbyte BehaviorType;

		// Token: 0x04001F33 RID: 7987
		public sbyte FameType;

		// Token: 0x04001F34 RID: 7988
		public sbyte FavorType;

		// Token: 0x04001F35 RID: 7989
		public sbyte AttractionType;

		// Token: 0x04001F36 RID: 7990
		public sbyte HappinessType;

		// Token: 0x04001F37 RID: 7991
		public byte LivingStatus;

		// Token: 0x04001F38 RID: 7992
		public byte WorkStatus;

		// Token: 0x04001F39 RID: 7993
		public sbyte VillagerNeedWaitTime;
	}
}
