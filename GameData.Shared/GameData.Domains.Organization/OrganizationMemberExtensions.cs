using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Organization;

public static class OrganizationMemberExtensions
{
	public static int GetAdjustedResourceSatisfyingThreshold(this OrganizationMemberItem memberCfg, sbyte resourceType)
	{
		return memberCfg.ResourceSatisfyingThreshold * (100 + memberCfg.ResourcesAdjust[resourceType]) / 100;
	}

	public static int GetAdjustedResourceSatisfyingAmount(this OrganizationMemberItem memberCfg, sbyte resourceType)
	{
		int adjustedResourceSatisfyingThreshold = memberCfg.GetAdjustedResourceSatisfyingThreshold(resourceType);
		return ResourceTypeHelper.WorthToResourceAmount(resourceType, adjustedResourceSatisfyingThreshold);
	}

	public static int AdjustResourceValue(this OrganizationMemberItem memberCfg, sbyte resourceType, int amount)
	{
		long val = (long)ResourceTypeHelper.ResourceAmountToWorth(resourceType, amount) * (long)(100 + memberCfg.ResourcesAdjust[resourceType]) / 100;
		return (int)Math.Min(2147483647L, val);
	}

	public static int AdjustResourceAmount(this OrganizationMemberItem memberCfg, sbyte resourceType, int value)
	{
		int worth = value * 100 / (100 + memberCfg.ResourcesAdjust[resourceType]);
		int num = ResourceTypeHelper.WorthToResourceAmount(resourceType, worth);
		if (memberCfg.AdjustResourceValue(resourceType, num) < value)
		{
			return num + 1;
		}
		return num;
	}

	public static sbyte GetRejoinGrade(this OrganizationMemberItem orgMemberCfg)
	{
		sbyte b = ((orgMemberCfg.RejoinGrade >= 0) ? orgMemberCfg.RejoinGrade : orgMemberCfg.Grade);
		OrganizationItem organizationItem = ((orgMemberCfg.Organization >= 0) ? Config.Organization.Instance[orgMemberCfg.Organization] : Config.Organization.Instance[(sbyte)21]);
		while (OrganizationMember.Instance[organizationItem.Members[b]].RestrictPrincipalAmount)
		{
			b--;
		}
		return b;
	}

	public static bool CanCraftItem(this OrganizationMemberItem memberCfg, sbyte itemType, short itemTemplateId)
	{
		if (memberCfg.CraftTypes == null)
		{
			return false;
		}
		sbyte craftRequiredLifeSkillType = ItemTemplateHelper.GetCraftRequiredLifeSkillType(itemType, itemTemplateId);
		return memberCfg.CraftTypes.Exist(craftRequiredLifeSkillType);
	}
}
