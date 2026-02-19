using System;
using Config;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class JoinSectAction : BasePrioritizedAction
{
	[SerializableGameDataField]
	public short SettlementId;

	public override short ActionType => 0;

	public override void OnStart(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		lifeRecordCollection.AddDecideToJoinSect(id, currDate, location, SettlementId);
		if (DomainManager.Character.IsTaiwuPeople(id))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddGoToJoinOrganization(id, SettlementId);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		int id = selfChar.GetId();
		lifeRecordCollection.AddJoinSectFail(id, currDate, SettlementId);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		if (Config.Organization.Instance[organizationInfo.OrgTemplateId].IsSect)
		{
			return true;
		}
		Settlement settlement = DomainManager.Organization.GetSettlement(SettlementId);
		OrganizationInfo organizationInfo2 = new OrganizationInfo(settlement.GetOrgTemplateId(), 0, principal: true, SettlementId);
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(organizationInfo2);
		sbyte gender = selfChar.GetGender();
		if (!OrganizationDomain.MeetGenderRestriction(organizationInfo2.OrgTemplateId, gender))
		{
			return false;
		}
		if (orgMemberConfig.Gender >= 0 && orgMemberConfig.Gender != gender)
		{
			return false;
		}
		if (orgMemberConfig.ChildGrade < 0 && (DomainManager.Character.GetAliveSpouse(selfChar.GetId()) >= 0 || DomainManager.Character.GetAliveChild(selfChar.GetId()) >= 0))
		{
			return false;
		}
		int joinOrgSuccessRate = GetJoinOrgSuccessRate(context.Random, selfChar, organizationInfo2);
		if (!context.Random.CheckPercentProb(joinOrgSuccessRate))
		{
			return false;
		}
		DomainManager.Organization.JoinSect(context, selfChar, organizationInfo2);
		int id = selfChar.GetId();
		if (DomainManager.Character.IsTaiwuPeople(id))
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			monthlyNotificationCollection.AddJoinOrganization(id, SettlementId);
		}
		return true;
	}

	private unsafe int GetJoinOrgSuccessRate(IRandomSource random, Character selfChar, OrganizationInfo targetOrgInfo)
	{
		int num = 0;
		OrganizationItem organizationItem = Config.Organization.Instance[targetOrgInfo.OrgTemplateId];
		OrganizationMemberItem orgMemberConfig = OrganizationDomain.GetOrgMemberConfig(targetOrgInfo);
		int num2 = 0;
		int num3 = 0;
		CombatSkillShorts combatSkillQualifications = selfChar.GetCombatSkillQualifications();
		for (sbyte b = 0; b < 14; b++)
		{
			short num4 = orgMemberConfig.CombatSkillsAdjust[b];
			if (num4 >= 6)
			{
				num2 += combatSkillQualifications.Items[b] * num4 / 12;
				num3++;
			}
		}
		if (num3 > 0)
		{
			num += num2 / num3;
		}
		int num5 = 0;
		int num6 = 0;
		LifeSkillShorts lifeSkillQualifications = selfChar.GetLifeSkillQualifications();
		for (sbyte b2 = 0; b2 < 16; b2++)
		{
			short num7 = orgMemberConfig.LifeSkillsAdjust[b2];
			if (num7 >= 6)
			{
				num5 += lifeSkillQualifications.Items[b2] * num7 / 12;
				num6++;
			}
		}
		if (num6 > 0)
		{
			num += num5 / num6;
		}
		int num8 = 0;
		int num9 = 0;
		MainAttributes maxMainAttributes = selfChar.GetMaxMainAttributes();
		for (sbyte b3 = 0; b3 < 6; b3++)
		{
			short num10 = orgMemberConfig.MainAttributesAdjust[b3];
			if (num10 >= 6)
			{
				num8 += maxMainAttributes.Items[b3] * num10 / 12;
				num9++;
			}
		}
		if (num9 > 0)
		{
			num += num8 / num9;
		}
		sbyte b4 = selfChar.GetFameType();
		if (b4 == -2)
		{
			b4 = (sbyte)(random.NextBool() ? 4 : 2);
		}
		int num11 = num;
		sbyte goodness = organizationItem.Goodness;
		if (1 == 0)
		{
		}
		int num12 = goodness switch
		{
			1 => 25 * (b4 - 3), 
			0 => 75 - 25 * Math.Abs(b4 - 3), 
			-1 => 25 * (3 - b4), 
			_ => 0, 
		};
		if (1 == 0)
		{
		}
		num = num11 + num12;
		sbyte behaviorType = selfChar.GetBehaviorType();
		sbyte behaviorType2 = BehaviorType.GetBehaviorType(organizationItem.Goodness);
		if (behaviorType == behaviorType2)
		{
			num += 50;
		}
		else if (BehaviorType.IsContradictory(behaviorType, behaviorType2))
		{
			num -= 50;
		}
		return num / 5;
	}

	public override bool IsSerializedSizeFixed()
	{
		return true;
	}

	public override int GetSerializedSize()
	{
		int num = 19;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*(short*)ptr = SettlementId;
		ptr += 2;
		ptr += Target.Serialize(ptr);
		*ptr = (HasArrived ? ((byte)1) : ((byte)0));
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		SettlementId = *(short*)ptr;
		ptr += 2;
		ptr += Target.Deserialize(ptr);
		HasArrived = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
