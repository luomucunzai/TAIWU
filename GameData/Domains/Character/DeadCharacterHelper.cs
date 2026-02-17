using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character.AvatarSystem;
using Redzen.Random;

namespace GameData.Domains.Character
{
	// Token: 0x0200080D RID: 2061
	public static class DeadCharacterHelper
	{
		// Token: 0x06007467 RID: 29799 RVA: 0x004433EC File Offset: 0x004415EC
		public unsafe static DeadCharacter CreateDeadCharacter(Character character, int deathDate)
		{
			List<short> titles = new List<short>();
			character.GetTitles(titles);
			return new DeadCharacter
			{
				TemplateId = character.GetTemplateId(),
				FullName = character.GetFullName(),
				MonasticTitle = character.GetMonasticTitle(),
				TitleIds = titles,
				Gender = character.GetGender(),
				FameType = character.GetFameType(),
				Happiness = character.GetHappiness(),
				Morality = character.GetMorality(),
				OrganizationInfo = character.GetOrganizationInfo(),
				Avatar = new AvatarData(character.GetAvatar()),
				ClothingDisplayId = character.GetClothingDisplayId(),
				Attraction = character.GetAttraction(),
				BirthDate = character.GetBirthDate(),
				CurrAge = character.GetCurrAge(),
				DeathDate = deathDate,
				MonkType = character.GetMonkType(),
				FeatureIds = new List<short>(character.GetFeatureIds()),
				BaseMainAttributes = character.GetBaseMainAttributes(),
				BaseLifeSkillQualifications = *character.GetBaseLifeSkillQualifications(),
				BaseCombatSkillQualifications = *character.GetBaseCombatSkillQualifications(),
				PreexistenceCharIds = *character.GetPreexistenceCharIds()
			};
		}

		// Token: 0x06007468 RID: 29800 RVA: 0x0044351C File Offset: 0x0044171C
		public static DeadCharacter CreateDeadCharacter(IRandomSource randomSource, short charTemplateId, int deathDate)
		{
			CharacterItem template = Character.Instance[charTemplateId];
			sbyte gender = (template.Gender >= 0) ? template.Gender : Gender.GetRandom(randomSource);
			return new DeadCharacter
			{
				TemplateId = charTemplateId,
				TitleIds = new List<short>(),
				Gender = gender,
				FameType = FameType.GetFameType(template.PresetFame),
				Happiness = template.Happiness,
				Morality = template.BaseMorality,
				OrganizationInfo = template.OrganizationInfo,
				Avatar = AvatarManager.Instance.GetRandomAvatar(randomSource, gender, template.Transgender, template.PresetBodyType, template.BaseAttraction),
				Attraction = template.BaseAttraction,
				BirthDate = CharacterDomain.CalcBirthDate(template.ActualAge, template.BirthMonth),
				CurrAge = ((template.InitCurrAge >= 0) ? template.InitCurrAge : template.ActualAge),
				DeathDate = deathDate,
				MonkType = template.MonkType,
				FeatureIds = new List<short>(template.FeatureIds),
				BaseMainAttributes = template.BaseMainAttributes,
				BaseLifeSkillQualifications = template.BaseLifeSkillQualifications,
				BaseCombatSkillQualifications = template.BaseCombatSkillQualifications
			};
		}
	}
}
