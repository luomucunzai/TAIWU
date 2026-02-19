namespace GameData.Domains.Character.AvatarSystem;

public static class AvatarHelper
{
	public static void InitializeGrowableElementsShowingAbilitiesAndStates(this AvatarData avatarData, Character character)
	{
		avatarData.ClearGrowableElementShowingAbilities();
		if (character.IsAbleToGrowHair())
		{
			avatarData.SetGrowableElementShowingAbility(0);
		}
		short physiologicalAge = character.GetPhysiologicalAge();
		var (flag, flag2) = character.IsAbleToGrowBeards(physiologicalAge);
		if (flag)
		{
			avatarData.SetGrowableElementShowingAbility(1);
		}
		if (flag2)
		{
			avatarData.SetGrowableElementShowingAbility(2);
		}
		if (Character.IsAbleToGrowWrinkle1(physiologicalAge))
		{
			avatarData.SetGrowableElementShowingAbility(3);
		}
		if (Character.IsAbleToGrowWrinkle2(physiologicalAge))
		{
			avatarData.SetGrowableElementShowingAbility(4);
		}
		if (Character.IsAbleToGrowWrinkle3(physiologicalAge))
		{
			avatarData.SetGrowableElementShowingAbility(5);
		}
		if (Character.IsAbleToGrowEyebrow())
		{
			avatarData.SetGrowableElementShowingAbility(6);
		}
		avatarData.FillGrowableElementsShowingStates();
	}

	public static bool UpdateGrowableElementsShowingAbilities(this AvatarData avatarData, Character character)
	{
		byte growableElementShowingAbilities = avatarData.GetGrowableElementShowingAbilities();
		short physiologicalAge = character.GetPhysiologicalAge();
		var (showable, showable2) = character.IsAbleToGrowBeards(physiologicalAge);
		avatarData.SetGrowableElementShowingAbility(1, showable);
		avatarData.SetGrowableElementShowingAbility(2, showable2);
		avatarData.SetGrowableElementShowingAbility(3, Character.IsAbleToGrowWrinkle1(physiologicalAge));
		avatarData.SetGrowableElementShowingAbility(4, Character.IsAbleToGrowWrinkle2(physiologicalAge));
		avatarData.SetGrowableElementShowingAbility(5, Character.IsAbleToGrowWrinkle3(physiologicalAge));
		return growableElementShowingAbilities != avatarData.GetGrowableElementShowingAbilities();
	}
}
