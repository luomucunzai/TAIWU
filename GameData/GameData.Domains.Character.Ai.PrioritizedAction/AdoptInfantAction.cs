using GameData.Common;
using GameData.Domains.Character.Relation;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class AdoptInfantAction : BasePrioritizedAction
{
	public override short ActionType => 10;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (DomainManager.Character.InfantHasPotentialAdopter(Target.TargetCharId) && !DomainManager.Character.IsCharacterPotentialInfantAdopter(Target.TargetCharId, selfChar.GetId()))
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return false;
		}
		if (element.GetAgeGroup() != 0)
		{
			return false;
		}
		if (selfChar.GetLocation().AreaId != element.GetLocation().AreaId)
		{
			return false;
		}
		return element.GetLeaderId() < 0 || element.GetLeaderId() == selfChar.GetLeaderId();
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		if (selfChar.GetLeaderId() != selfChar.GetId())
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
		DomainManager.LifeRecord.GetLifeRecordCollection().AddDecideToAdoptFoundling(selfChar.GetId(), DomainManager.World.GetCurrDate(), Target.TargetCharId, selfChar.GetLocation());
		DomainManager.Character.AddPotentialAdopterToInfant(Target.TargetCharId, selfChar.GetId());
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		if (DomainManager.Character.IsCharacterPotentialInfantAdopter(Target.TargetCharId, selfChar.GetId()))
		{
			DomainManager.Character.RemovePotentialAdopterToInfant(Target.TargetCharId);
		}
		DomainManager.LifeRecord.GetLifeRecordCollection().AddAdoptFoundlingFail(selfChar.GetId(), DomainManager.World.GetCurrDate(), Target.TargetCharId, selfChar.GetLocation());
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int targetCharId = Target.TargetCharId;
		DomainManager.Character.TryGetElement_Objects(targetCharId, out var element);
		int leaderId = element.GetLeaderId();
		if (leaderId >= 0 && leaderId == selfChar.GetLeaderId())
		{
			selfChar.DeactivateAdvanceMonthStatus(7);
			bool flag = RelationTypeHelper.AllowAddingAdoptiveChildRelation(id, targetCharId);
			int currDate = DomainManager.World.GetCurrDate();
			Location location = selfChar.GetLocation();
			LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset;
			if (flag)
			{
				int aliveSpouse = DomainManager.Character.GetAliveSpouse(id);
				DomainManager.Character.AddAdoptiveParentRelations(context, targetCharId, id, currDate);
				if (aliveSpouse >= 0 && RelationTypeHelper.AllowAddingAdoptiveChildRelation(aliveSpouse, targetCharId))
				{
					DomainManager.Character.AddAdoptiveParentRelations(context, targetCharId, aliveSpouse, currDate);
				}
				lifeRecordCollection.AddAdoptFoundlingSucceed(id, currDate, targetCharId, location);
				dataOffset = secretInformationCollection.AddAdoptChild(id, targetCharId);
			}
			else
			{
				lifeRecordCollection.AddClaimFoundlingSucceed(id, currDate, targetCharId, location);
				dataOffset = secretInformationCollection.AddRetrieveChild(id, targetCharId);
			}
			DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
			DomainManager.Character.MarkInfantAsAdopted(targetCharId);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, element, 12000);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, element, selfChar, 12000);
			selfChar.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[selfChar.GetBehaviorType()]);
			element.ChangeHappiness(context, AiHelper.RelationsRelatedConstants.BecomeSwornOrAdoptedFamilyHappinessChange[element.GetBehaviorType()]);
			return true;
		}
		element.SetHealth(element.GetLeftMaxHealth(), context);
		DomainManager.Character.JoinGroup(context, element, selfChar);
		selfChar.ActivateAdvanceMonthStatus(7);
		return false;
	}
}
