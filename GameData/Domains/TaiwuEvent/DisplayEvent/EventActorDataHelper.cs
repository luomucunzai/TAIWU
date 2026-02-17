using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using Redzen.Random;

namespace GameData.Domains.TaiwuEvent.DisplayEvent
{
	// Token: 0x020000D4 RID: 212
	public static class EventActorDataHelper
	{
		// Token: 0x060025BC RID: 9660 RVA: 0x001CCFA4 File Offset: 0x001CB1A4
		public static EventActorData CreateActor(IRandomSource random, short templateId)
		{
			EventActorData actor = new EventActorData();
			actor.TemplateId = templateId;
			EventActorsItem config = EventActors.Instance[templateId];
			bool flag = string.IsNullOrEmpty(config.Name);
			if (flag)
			{
				actor.FullName = CharacterDomain.GenerateRandomHanName(random, -1, -1, actor.Gender, -1);
			}
			actor.Gender = config.Gender;
			bool flag2 = actor.Gender == -1;
			if (flag2)
			{
				actor.Gender = Gender.GetRandom(random);
			}
			actor.Age = (byte)random.Next((int)config.Age[0], (int)(config.Age[1] + 1));
			short attraction = (short)random.Next((int)config.Attraction[0], (int)(config.Attraction[1] + 1));
			actor.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, actor.Gender, false, config.PresetBodyType, attraction);
			actor.UpdateDisplayName();
			bool flag3 = !string.IsNullOrEmpty(config.Texture);
			EventActorData result;
			if (flag3)
			{
				result = actor;
			}
			else
			{
				actor.ClothDisplayId = AvatarManager.Instance.GetAvatarGroup((int)actor.AvatarData.AvatarId).GetRandomCloth(random, true, false);
				bool flag4 = -1 != config.Clothing;
				if (flag4)
				{
					ClothingItem clothingItem = Clothing.Instance[config.Clothing];
					actor.ClothDisplayId = clothingItem.DisplayId;
				}
				bool flag5 = !config.IsMonk;
				if (flag5)
				{
					actor.AvatarData.SetGrowableElementShowingState(0, true);
					actor.AvatarData.SetGrowableElementShowingAbility(0, true);
				}
				actor.AvatarData.SetGrowableElementShowingState(1, (int)actor.Age >= GlobalConfig.Instance.AgeShowBeard1);
				actor.AvatarData.SetGrowableElementShowingAbility(1, true);
				actor.AvatarData.SetGrowableElementShowingState(2, (int)actor.Age >= GlobalConfig.Instance.AgeShowBeard2);
				actor.AvatarData.SetGrowableElementShowingAbility(2, true);
				actor.AvatarData.SetGrowableElementShowingState(3, (int)actor.Age >= GlobalConfig.Instance.AgeShowWrinkle1);
				actor.AvatarData.SetGrowableElementShowingAbility(3, true);
				actor.AvatarData.SetGrowableElementShowingState(4, (int)actor.Age >= GlobalConfig.Instance.AgeShowWrinkle2);
				actor.AvatarData.SetGrowableElementShowingAbility(4, true);
				actor.AvatarData.SetGrowableElementShowingState(5, (int)actor.Age >= GlobalConfig.Instance.AgeShowWrinkle3);
				actor.AvatarData.SetGrowableElementShowingAbility(5, true);
				result = actor;
			}
			return result;
		}

		// Token: 0x060025BD RID: 9661 RVA: 0x001CD20B File Offset: 0x001CB40B
		public static void SetSurName(this EventActorData actor, FullName fullName)
		{
			actor.FullName = DomainManager.Character.GenerateRandomChildName(DomainManager.TaiwuEvent.MainThreadDataContext, actor.Gender, fullName);
			actor.UpdateDisplayName();
		}

		// Token: 0x060025BE RID: 9662 RVA: 0x001CD238 File Offset: 0x001CB438
		private static void UpdateDisplayName(this EventActorData actor)
		{
			EventActorsItem config = EventActors.Instance[actor.TemplateId];
			bool flag = !string.IsNullOrEmpty(config.Name);
			if (flag)
			{
				actor.DisplayName = config.Name;
			}
			else
			{
				IReadOnlyDictionary<int, string> customTexts = DomainManager.World.GetCustomTexts();
				ValueTuple<string, string> name = actor.FullName.GetName(actor.Gender, customTexts);
				string surName = name.Item1;
				string givenName = name.Item2;
				actor.DisplayName = surName + givenName;
			}
		}
	}
}
