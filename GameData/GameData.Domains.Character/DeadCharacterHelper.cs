using System.Collections.Generic;
using Config;
using GameData.Domains.Character.AvatarSystem;
using Redzen.Random;

namespace GameData.Domains.Character;

public static class DeadCharacterHelper
{
	public static DeadCharacter CreateDeadCharacter(Character character, int deathDate)
	{
		List<short> titleIds = new List<short>();
		character.GetTitles(titleIds);
		return new DeadCharacter
		{
			TemplateId = character.GetTemplateId(),
			FullName = character.GetFullName(),
			MonasticTitle = character.GetMonasticTitle(),
			TitleIds = titleIds,
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
			BaseLifeSkillQualifications = character.GetBaseLifeSkillQualifications(),
			BaseCombatSkillQualifications = character.GetBaseCombatSkillQualifications(),
			PreexistenceCharIds = character.GetPreexistenceCharIds()
		};
	}

	public static DeadCharacter CreateDeadCharacter(IRandomSource randomSource, short charTemplateId, int deathDate)
	{
		CharacterItem characterItem = Config.Character.Instance[charTemplateId];
		sbyte gender = ((characterItem.Gender >= 0) ? characterItem.Gender : Gender.GetRandom(randomSource));
		return new DeadCharacter
		{
			TemplateId = charTemplateId,
			TitleIds = new List<short>(),
			Gender = gender,
			FameType = FameType.GetFameType(characterItem.PresetFame),
			Happiness = characterItem.Happiness,
			Morality = characterItem.BaseMorality,
			OrganizationInfo = characterItem.OrganizationInfo,
			Avatar = AvatarManager.Instance.GetRandomAvatar(randomSource, gender, characterItem.Transgender, characterItem.PresetBodyType, characterItem.BaseAttraction),
			Attraction = characterItem.BaseAttraction,
			BirthDate = CharacterDomain.CalcBirthDate(characterItem.ActualAge, characterItem.BirthMonth),
			CurrAge = ((characterItem.InitCurrAge >= 0) ? characterItem.InitCurrAge : characterItem.ActualAge),
			DeathDate = deathDate,
			MonkType = characterItem.MonkType,
			FeatureIds = new List<short>(characterItem.FeatureIds),
			BaseMainAttributes = characterItem.BaseMainAttributes,
			BaseLifeSkillQualifications = characterItem.BaseLifeSkillQualifications,
			BaseCombatSkillQualifications = characterItem.BaseCombatSkillQualifications
		};
	}
}
