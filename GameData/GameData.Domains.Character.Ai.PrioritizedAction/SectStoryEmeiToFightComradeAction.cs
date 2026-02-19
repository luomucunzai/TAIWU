using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.TaiwuEvent;
using GameData.Domains.World.Notification;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class SectStoryEmeiToFightComradeAction : BasePrioritizedAction
{
	public override short ActionType => 13;

	private bool CheckAndUpdateTarget(DataContext context, Character selfChar, out Character target)
	{
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out target))
		{
			DomainManager.World.GetEmeiPotentialVictims(selfChar, out var charIds);
			if (charIds.Count != 0)
			{
				Target = new NpcTravelTarget(charIds.GetRandom(context.Random), int.MaxValue);
			}
			return false;
		}
		return true;
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		DomainManager.Extra.SectEmeiAddInsaneCharacterId(selfChar.GetId());
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		DomainManager.Extra.SectEmeiRemoveInsaneCharacterId(selfChar.GetId());
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (!CheckAndUpdateTarget(context, selfChar, out var target))
		{
			return false;
		}
		DomainManager.Character.SimulateCharacterCombat(context, selfChar, target, CombatType.Die);
		DomainManager.Extra.SectEmeiRemoveInsaneCharacterId(selfChar.GetId());
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		monthlyNotificationCollection.AddSectMainStoryEmeiInfighting(selfChar.GetId(), selfChar.GetLocation());
		return true;
	}

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return false;
		}
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		OrganizationInfo organizationInfo2 = element.GetOrganizationInfo();
		if (organizationInfo.OrgTemplateId != 2)
		{
			return false;
		}
		if (organizationInfo2.OrgTemplateId != 2)
		{
			return false;
		}
		if (selfChar.GetLocation().AreaId != element.GetLocation().AreaId)
		{
			return false;
		}
		if (selfChar.GetLocation().AreaId != DomainManager.Organization.GetSettlementByOrgTemplateId(2).GetLocation().AreaId)
		{
			return false;
		}
		if (element.GetAgeGroup() != 2)
		{
			return false;
		}
		if (organizationInfo2.Grade > organizationInfo.Grade)
		{
			return false;
		}
		EventArgBox sectMainStoryEventArgBox = DomainManager.Extra.GetSectMainStoryEventArgBox(2);
		return sectMainStoryEventArgBox.Contains<int>("ConchShip_PresetKey_EmeiKillEachOtherStage");
	}
}
