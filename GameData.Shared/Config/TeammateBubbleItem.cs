using System;
using System.Collections.Generic;
using Config.Common;

namespace Config;

[Serializable]
public class TeammateBubbleItem : ConfigItem<TeammateBubbleItem, short>
{
	public readonly short TemplateId;

	public readonly string Name;

	public readonly ETeammateBubbleBubbleElementType BubbleElementType;

	public readonly int Duration;

	public readonly sbyte MapStateTemplateId;

	public readonly short MapBlockTemplateId;

	public readonly List<short> CharacterTemplateIdList;

	public readonly List<short> CharacterFeatureTemplateIdList;

	public readonly List<short> AdventureTemplateIdList;

	public readonly List<sbyte> AdventureTypeTemplateIdList;

	public readonly sbyte PersonalityType;

	public readonly string SpecialDesc0;

	public readonly string SpecialDesc1;

	public readonly string SpecialDesc2;

	public readonly string SpecialDesc3;

	public readonly string SpecialDesc4;

	public readonly string FamilyDesc;

	public readonly string FriendDesc;

	public readonly string[] BehaviorDesc;

	public readonly string[] Parameters;

	public TeammateBubbleItem(short templateId, int name, ETeammateBubbleBubbleElementType bubbleElementType, int duration, sbyte mapStateTemplateId, short mapBlockTemplateId, List<short> characterTemplateIdList, List<short> characterFeatureTemplateIdList, List<short> adventureTemplateIdList, List<sbyte> adventureTypeTemplateIdList, sbyte personalityType, int specialDesc0, int specialDesc1, int specialDesc2, int specialDesc3, int specialDesc4, int familyDesc, int friendDesc, int[] behaviorDesc, string[] parameters)
	{
		TemplateId = templateId;
		Name = LocalStringManager.GetConfig("TeammateBubble_language", name);
		BubbleElementType = bubbleElementType;
		Duration = duration;
		MapStateTemplateId = mapStateTemplateId;
		MapBlockTemplateId = mapBlockTemplateId;
		CharacterTemplateIdList = characterTemplateIdList;
		CharacterFeatureTemplateIdList = characterFeatureTemplateIdList;
		AdventureTemplateIdList = adventureTemplateIdList;
		AdventureTypeTemplateIdList = adventureTypeTemplateIdList;
		PersonalityType = personalityType;
		SpecialDesc0 = LocalStringManager.GetConfig("TeammateBubble_language", specialDesc0);
		SpecialDesc1 = LocalStringManager.GetConfig("TeammateBubble_language", specialDesc1);
		SpecialDesc2 = LocalStringManager.GetConfig("TeammateBubble_language", specialDesc2);
		SpecialDesc3 = LocalStringManager.GetConfig("TeammateBubble_language", specialDesc3);
		SpecialDesc4 = LocalStringManager.GetConfig("TeammateBubble_language", specialDesc4);
		FamilyDesc = LocalStringManager.GetConfig("TeammateBubble_language", familyDesc);
		FriendDesc = LocalStringManager.GetConfig("TeammateBubble_language", friendDesc);
		BehaviorDesc = LocalStringManager.ConvertConfigList("TeammateBubble_language", behaviorDesc);
		Parameters = parameters;
	}

	public TeammateBubbleItem()
	{
		TemplateId = 0;
		Name = null;
		BubbleElementType = ETeammateBubbleBubbleElementType.Traveling;
		Duration = 180;
		MapStateTemplateId = 0;
		MapBlockTemplateId = 0;
		CharacterTemplateIdList = null;
		CharacterFeatureTemplateIdList = null;
		AdventureTemplateIdList = null;
		AdventureTypeTemplateIdList = null;
		PersonalityType = 0;
		SpecialDesc0 = null;
		SpecialDesc1 = null;
		SpecialDesc2 = null;
		SpecialDesc3 = null;
		SpecialDesc4 = null;
		FamilyDesc = null;
		FriendDesc = null;
		BehaviorDesc = LocalStringManager.ConvertConfigList("TeammateBubble_language", null);
		Parameters = new string[3] { "", "", "" };
	}

	public TeammateBubbleItem(short templateId, TeammateBubbleItem other)
	{
		TemplateId = templateId;
		Name = other.Name;
		BubbleElementType = other.BubbleElementType;
		Duration = other.Duration;
		MapStateTemplateId = other.MapStateTemplateId;
		MapBlockTemplateId = other.MapBlockTemplateId;
		CharacterTemplateIdList = other.CharacterTemplateIdList;
		CharacterFeatureTemplateIdList = other.CharacterFeatureTemplateIdList;
		AdventureTemplateIdList = other.AdventureTemplateIdList;
		AdventureTypeTemplateIdList = other.AdventureTypeTemplateIdList;
		PersonalityType = other.PersonalityType;
		SpecialDesc0 = other.SpecialDesc0;
		SpecialDesc1 = other.SpecialDesc1;
		SpecialDesc2 = other.SpecialDesc2;
		SpecialDesc3 = other.SpecialDesc3;
		SpecialDesc4 = other.SpecialDesc4;
		FamilyDesc = other.FamilyDesc;
		FriendDesc = other.FriendDesc;
		BehaviorDesc = other.BehaviorDesc;
		Parameters = other.Parameters;
	}

	public override int GetTemplateId()
	{
		return TemplateId;
	}

	public override TeammateBubbleItem Duplicate(int templateId)
	{
		return new TeammateBubbleItem((short)templateId, this);
	}
}
