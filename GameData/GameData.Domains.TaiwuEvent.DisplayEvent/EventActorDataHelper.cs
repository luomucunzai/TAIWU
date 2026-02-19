using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Character.AvatarSystem;
using Redzen.Random;

namespace GameData.Domains.TaiwuEvent.DisplayEvent;

public static class EventActorDataHelper
{
	public static EventActorData CreateActor(IRandomSource random, short templateId)
	{
		EventActorData eventActorData = new EventActorData();
		eventActorData.TemplateId = templateId;
		EventActorsItem eventActorsItem = EventActors.Instance[templateId];
		if (string.IsNullOrEmpty(eventActorsItem.Name))
		{
			eventActorData.FullName = CharacterDomain.GenerateRandomHanName(random, -1, -1, eventActorData.Gender, -1);
		}
		eventActorData.Gender = eventActorsItem.Gender;
		if (eventActorData.Gender == -1)
		{
			eventActorData.Gender = Gender.GetRandom(random);
		}
		eventActorData.Age = (byte)random.Next((int)eventActorsItem.Age[0], eventActorsItem.Age[1] + 1);
		short baseAttraction = (short)random.Next((int)eventActorsItem.Attraction[0], eventActorsItem.Attraction[1] + 1);
		eventActorData.AvatarData = AvatarManager.Instance.GetRandomAvatar(random, eventActorData.Gender, transgender: false, eventActorsItem.PresetBodyType, baseAttraction);
		eventActorData.UpdateDisplayName();
		if (!string.IsNullOrEmpty(eventActorsItem.Texture))
		{
			return eventActorData;
		}
		eventActorData.ClothDisplayId = AvatarManager.Instance.GetAvatarGroup(eventActorData.AvatarData.AvatarId).GetRandomCloth(random, canCreateOnly: true);
		if (-1 != eventActorsItem.Clothing)
		{
			ClothingItem clothingItem = Clothing.Instance[eventActorsItem.Clothing];
			eventActorData.ClothDisplayId = clothingItem.DisplayId;
		}
		if (!eventActorsItem.IsMonk)
		{
			eventActorData.AvatarData.SetGrowableElementShowingState(0, show: true);
			eventActorData.AvatarData.SetGrowableElementShowingAbility(0, showable: true);
		}
		eventActorData.AvatarData.SetGrowableElementShowingState(1, eventActorData.Age >= GlobalConfig.Instance.AgeShowBeard1);
		eventActorData.AvatarData.SetGrowableElementShowingAbility(1, showable: true);
		eventActorData.AvatarData.SetGrowableElementShowingState(2, eventActorData.Age >= GlobalConfig.Instance.AgeShowBeard2);
		eventActorData.AvatarData.SetGrowableElementShowingAbility(2, showable: true);
		eventActorData.AvatarData.SetGrowableElementShowingState(3, eventActorData.Age >= GlobalConfig.Instance.AgeShowWrinkle1);
		eventActorData.AvatarData.SetGrowableElementShowingAbility(3, showable: true);
		eventActorData.AvatarData.SetGrowableElementShowingState(4, eventActorData.Age >= GlobalConfig.Instance.AgeShowWrinkle2);
		eventActorData.AvatarData.SetGrowableElementShowingAbility(4, showable: true);
		eventActorData.AvatarData.SetGrowableElementShowingState(5, eventActorData.Age >= GlobalConfig.Instance.AgeShowWrinkle3);
		eventActorData.AvatarData.SetGrowableElementShowingAbility(5, showable: true);
		return eventActorData;
	}

	public static void SetSurName(this EventActorData actor, FullName fullName)
	{
		actor.FullName = DomainManager.Character.GenerateRandomChildName(DomainManager.TaiwuEvent.MainThreadDataContext, actor.Gender, fullName);
		actor.UpdateDisplayName();
	}

	private static void UpdateDisplayName(this EventActorData actor)
	{
		EventActorsItem eventActorsItem = EventActors.Instance[actor.TemplateId];
		if (!string.IsNullOrEmpty(eventActorsItem.Name))
		{
			actor.DisplayName = eventActorsItem.Name;
			return;
		}
		IReadOnlyDictionary<int, string> customTexts = DomainManager.World.GetCustomTexts();
		(string, string) name = actor.FullName.GetName(actor.Gender, customTexts);
		string item = name.Item1;
		string item2 = name.Item2;
		actor.DisplayName = item + item2;
	}
}
