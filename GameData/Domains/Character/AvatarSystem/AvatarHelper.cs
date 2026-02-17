using System;

namespace GameData.Domains.Character.AvatarSystem
{
	// Token: 0x02000841 RID: 2113
	public static class AvatarHelper
	{
		// Token: 0x060075FD RID: 30205 RVA: 0x0044E8E8 File Offset: 0x0044CAE8
		public static void InitializeGrowableElementsShowingAbilitiesAndStates(this AvatarData avatarData, Character character)
		{
			avatarData.ClearGrowableElementShowingAbilities();
			bool flag = character.IsAbleToGrowHair();
			if (flag)
			{
				avatarData.SetGrowableElementShowingAbility(0);
			}
			short physiologicalAge = character.GetPhysiologicalAge();
			ValueTuple<bool, bool> valueTuple = character.IsAbleToGrowBeards(physiologicalAge);
			bool canGrowBeard = valueTuple.Item1;
			bool canGrowBeard2 = valueTuple.Item2;
			bool flag2 = canGrowBeard;
			if (flag2)
			{
				avatarData.SetGrowableElementShowingAbility(1);
			}
			bool flag3 = canGrowBeard2;
			if (flag3)
			{
				avatarData.SetGrowableElementShowingAbility(2);
			}
			bool flag4 = Character.IsAbleToGrowWrinkle1(physiologicalAge);
			if (flag4)
			{
				avatarData.SetGrowableElementShowingAbility(3);
			}
			bool flag5 = Character.IsAbleToGrowWrinkle2(physiologicalAge);
			if (flag5)
			{
				avatarData.SetGrowableElementShowingAbility(4);
			}
			bool flag6 = Character.IsAbleToGrowWrinkle3(physiologicalAge);
			if (flag6)
			{
				avatarData.SetGrowableElementShowingAbility(5);
			}
			bool flag7 = Character.IsAbleToGrowEyebrow();
			if (flag7)
			{
				avatarData.SetGrowableElementShowingAbility(6);
			}
			avatarData.FillGrowableElementsShowingStates();
		}

		// Token: 0x060075FE RID: 30206 RVA: 0x0044E9A0 File Offset: 0x0044CBA0
		public static bool UpdateGrowableElementsShowingAbilities(this AvatarData avatarData, Character character)
		{
			byte oriAbilities = avatarData.GetGrowableElementShowingAbilities();
			short physiologicalAge = character.GetPhysiologicalAge();
			ValueTuple<bool, bool> valueTuple = character.IsAbleToGrowBeards(physiologicalAge);
			bool canGrowBeard = valueTuple.Item1;
			bool canGrowBeard2 = valueTuple.Item2;
			avatarData.SetGrowableElementShowingAbility(1, canGrowBeard);
			avatarData.SetGrowableElementShowingAbility(2, canGrowBeard2);
			avatarData.SetGrowableElementShowingAbility(3, Character.IsAbleToGrowWrinkle1(physiologicalAge));
			avatarData.SetGrowableElementShowingAbility(4, Character.IsAbleToGrowWrinkle2(physiologicalAge));
			avatarData.SetGrowableElementShowingAbility(5, Character.IsAbleToGrowWrinkle3(physiologicalAge));
			return oriAbilities != avatarData.GetGrowableElementShowingAbilities();
		}
	}
}
