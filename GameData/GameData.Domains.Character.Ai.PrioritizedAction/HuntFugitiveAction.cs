using Config;
using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
public class HuntFugitiveAction : ExtensiblePrioritizedAction
{
	public override short ActionType => 17;

	public override bool CheckValid(Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		if (!DomainManager.Organization.TryGetElement_Sects(organizationInfo.SettlementId, out var element))
		{
			return false;
		}
		SettlementBounty bounty = element.Prison.GetBounty(Target.TargetCharId);
		if (bounty == null)
		{
			return false;
		}
		if (bounty.CurrentHunterId >= 0 && bounty.CurrentHunterId != selfChar.GetId())
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(bounty.CharId, out var element2))
		{
			return false;
		}
		if (OrganizationDomain.IsLargeSect(element2.GetOrganizationInfo().OrgTemplateId))
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(organizationInfo.SettlementId);
		SettlementBounty bounty = element_Sects.Prison.GetBounty(Target.TargetCharId);
		if (bounty.CurrentHunterId < 0)
		{
			bounty.CurrentHunterId = id;
			DomainManager.Extra.SetSettlementPrison(context, organizationInfo.SettlementId, element_Sects.Prison);
			int leaderId = selfChar.GetLeaderId();
			if (leaderId >= 0 && leaderId != id)
			{
				DomainManager.Character.LeaveGroup(context, selfChar);
			}
			AdaptableLog.Info($"{selfChar} 开始追捕逃犯 {DomainManager.Character.GetElement_Objects(bounty.CharId)}.");
			Location location = selfChar.GetLocation();
			int currDate = DomainManager.World.GetCurrDate();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddDecideToHuntFugitive(id, currDate, Target.TargetCharId, location);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(Target.TargetCharId);
		if (fugitiveBountySect >= 0)
		{
			Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
			SettlementBounty bounty = sect.Prison.GetBounty(Target.TargetCharId);
			if (bounty.CurrentHunterId == selfChar.GetId())
			{
				bounty.CurrentHunterId = -1;
				DomainManager.Extra.SetSettlementPrison(context, organizationInfo.SettlementId, sect.Prison);
			}
		}
		int id = selfChar.GetId();
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddFinishHuntFugitive(id, currDate, Target.TargetCharId, location);
		AdaptableLog.Info($"{selfChar} 终止追捕逃犯.");
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return true;
		}
		int id = selfChar.GetId();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (element.GetId() == taiwuCharId)
		{
			if (DomainManager.World.GetWorldFunctionsStatus(4))
			{
				MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
				monthlyEventCollection.AddHuntCriminalTaiwu(id, Target.TargetCharId, selfChar.GetLocation());
				CharacterDomain.AddLockMovementCharSet(id);
			}
			return false;
		}
		if (element.GetLeaderId() == taiwuCharId)
		{
			MonthlyEventCollection monthlyEventCollection2 = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection2.AddHuntCriminal(taiwuCharId, id, Target.TargetCharId);
			CharacterDomain.AddLockMovementCharSet(id);
			return false;
		}
		if (!element.GetLocation().IsValid() || !element.IsInteractableAsIntelligentCharacter())
		{
			return false;
		}
		OrganizationInfo organizationInfo = selfChar.GetOrganizationInfo();
		Sect element_Sects = DomainManager.Organization.GetElement_Sects(organizationInfo.SettlementId);
		SettlementPrison prison = element_Sects.Prison;
		SettlementBounty bounty = prison.GetBounty(Target.TargetCharId);
		PunishmentSeverityItem punishmentSeverityItem = PunishmentSeverity.Instance[bounty.PunishmentSeverity];
		Location location = selfChar.GetLocation();
		int currDate = DomainManager.World.GetCurrDate();
		AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, selfChar, element, CombatType.Beat);
		if ((uint)npcCombatResultType <= 1u)
		{
			AdaptableLog.Info($"{selfChar} 成功捕获逃犯 {element}.");
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection.AddArrestedSuccessfullyCaptor(id, currDate, Target.TargetCharId, location, organizationInfo.SettlementId, 889);
			if (DomainManager.Character.TryGetCharacterPrioritizedAction(Target.TargetCharId, out var _))
			{
				DomainManager.Character.RemoveCharacterPrioritizedAction(context, Target.TargetCharId);
			}
			if (punishmentSeverityItem.PrisonTime > 0 || punishmentSeverityItem.Expel)
			{
				DomainManager.Character.CombatResultHandle_KidnapEnemy(context, selfChar, element, isInPublic: true);
			}
			else
			{
				DomainManager.Organization.PunishSectMember(context, element_Sects, element, bounty.PunishmentSeverity, bounty.PunishmentType, isArrested: true);
				element_Sects.RemoveBounty(context, element.GetId());
				selfChar.RecordFameAction(context, 83, element.GetId(), bounty.CaptorFameActionMultiplier);
			}
		}
		else
		{
			LifeRecordCollection lifeRecordCollection2 = DomainManager.LifeRecord.GetLifeRecordCollection();
			lifeRecordCollection2.AddArrestFailedCaptor(id, currDate, Target.TargetCharId, location, organizationInfo.SettlementId, 886);
			bounty.CurrentHunterId = -1;
			if (bounty.RequiredConsummateLevel < 0)
			{
				bounty.RequiredConsummateLevel = element.GetConsummateLevel();
			}
			bounty.RequiredConsummateLevel++;
			AdaptableLog.Info($"{selfChar} 未能战胜逃犯 {element}.");
			DomainManager.Extra.SetSettlementPrison(context, organizationInfo.SettlementId, prison);
		}
		return true;
	}
}
