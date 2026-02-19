using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.Organization;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
public class EscortPrisonerAction : ExtensiblePrioritizedAction
{
	public override short ActionType => 20;

	public override bool CheckValid(Character selfChar)
	{
		if (!selfChar.IsActiveExternalRelationState(2))
		{
			return false;
		}
		int id = selfChar.GetId();
		sbyte orgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
		List<KidnappedCharacter> collection = DomainManager.Character.GetKidnappedCharacters(id).GetCollection();
		foreach (KidnappedCharacter item in collection)
		{
			sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(item.CharId);
			if (fugitiveBountySect < 0 || fugitiveBountySect != orgTemplateId)
			{
				continue;
			}
			return base.CheckValid(selfChar);
		}
		return false;
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		AdaptableLog.Info($"{selfChar} start escorting prisoner {DomainManager.Character.GetElement_Objects(Target.TargetCharId)}.");
		int id = selfChar.GetId();
		int targetCharId = Target.TargetCharId;
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		short settlementId = selfChar.GetOrganizationInfo().SettlementId;
		lifeRecordCollection.AddDecideToEscortPrisoner(id, currDate, targetCharId, location, settlementId);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		AdaptableLog.Info(DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element) ? $"{selfChar} end escorting prisoner {element}." : $"{selfChar} end escorting prisoner {Target.TargetCharId}.");
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		sbyte orgTemplateId = selfChar.GetOrganizationInfo().OrgTemplateId;
		KidnappedCharacterList kidnappedCharacters = DomainManager.Character.GetKidnappedCharacters(id);
		List<KidnappedCharacter> collection = kidnappedCharacters.GetCollection();
		for (int num = collection.Count - 1; num >= 0; num--)
		{
			KidnappedCharacter kidnappedCharacter = collection[num];
			sbyte fugitiveBountySect = DomainManager.Organization.GetFugitiveBountySect(kidnappedCharacter.CharId);
			if (fugitiveBountySect >= 0 && fugitiveBountySect == orgTemplateId)
			{
				int currDate = DomainManager.World.GetCurrDate();
				Sect sect = (Sect)DomainManager.Organization.GetSettlementByOrgTemplateId(fugitiveBountySect);
				SettlementBounty bounty = sect.Prison.GetBounty(kidnappedCharacter.CharId);
				Character element_Objects = DomainManager.Character.GetElement_Objects(kidnappedCharacter.CharId);
				DomainManager.Character.RemoveKidnappedCharacter(context, selfChar, kidnappedCharacters, num, isEscaped: false);
				DomainManager.Organization.PunishSectMember(context, sect, element_Objects, bounty.PunishmentSeverity, bounty.PunishmentType, isArrested: true);
				AdaptableLog.Info($"{selfChar} successfully escorted {element_Objects}.");
				int charId = kidnappedCharacter.CharId;
				Location location = selfChar.GetLocation();
				LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
				lifeRecordCollection.AddEscortPrisonerSucceed(id, currDate, charId, location);
				selfChar.RecordFameAction(context, 83, charId, bounty.CaptorFameActionMultiplier);
			}
		}
		return true;
	}
}
