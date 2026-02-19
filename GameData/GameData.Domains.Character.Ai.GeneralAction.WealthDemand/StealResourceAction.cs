using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class StealResourceAction : IGeneralAction
{
	public sbyte ResourceType;

	public int Amount;

	public sbyte Phase;

	public sbyte ActionEnergyType => 1;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return targetChar.GetResource(ResourceType) >= Amount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = selfChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		if (Phase <= 3)
		{
			monthlyNotificationCollection.AddStealResourceFailure(id, location, id2, ResourceType);
			ApplyChanges(context, selfChar, targetChar);
			return;
		}
		monthlyNotificationCollection.AddStealResourceSuccess(id, location, id2, ResourceType);
		if (Phase == 4)
		{
			monthlyEventCollection.AddStealResourceButBeCaught(id, location, id2, ResourceType, Amount);
		}
		else
		{
			monthlyEventCollection.AddStealResourceAndEscape(id, location, id2, ResourceType, Amount);
			ApplyChanges(context, selfChar, targetChar);
		}
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int taiwuCharId = DomainManager.Taiwu.GetTaiwuCharId();
		if (id != taiwuCharId)
		{
			selfChar.ChangeCurrMainAttribute(context, 1, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (Phase >= 4)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddStealResource(id, id2, ResourceType);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		switch (Phase)
		{
		case 0:
			lifeRecordCollection.AddStealResourceFail1(id, currDate, id2, location, ResourceType);
			break;
		case 1:
			lifeRecordCollection.AddStealResourceFail2(id, currDate, id2, location, ResourceType);
			break;
		case 2:
			lifeRecordCollection.AddStealResourceFail3(id, currDate, id2, location, ResourceType);
			break;
		case 3:
			lifeRecordCollection.AddStealResourceFail4(id, currDate, id2, location, ResourceType);
			break;
		case 4:
			lifeRecordCollection.AddStealResourceSucceed(id, currDate, id2, location, ResourceType, Amount);
			if (id2 != taiwuCharId)
			{
				AiHelper.NpcCombatResultType npcCombatResultType = DomainManager.Character.SimulateCharacterCombat(context, targetChar, selfChar, CombatType.Beat);
				if ((uint)(npcCombatResultType - 2) <= 1u)
				{
					selfChar.ChangeResource(context, ResourceType, Amount);
					targetChar.ChangeResource(context, ResourceType, -Amount);
					int delta2 = -AiHelper.GeneralActionConstants.GetResourceHappinessChange(ResourceType, Amount);
					targetChar.ChangeHappiness(context, delta2);
					DomainManager.Character.SimulateCharacterCombatResult(context, selfChar, targetChar, -40, -20, 0);
				}
				else
				{
					lifeRecordCollection.AddStealResourceFailAndBeatenUp(id, currDate, id2, location, ResourceType, Amount);
					DomainManager.Character.SimulateCharacterCombatResult(context, targetChar, selfChar, -40, -20, 0);
				}
			}
			break;
		default:
		{
			selfChar.ChangeResource(context, ResourceType, Amount);
			targetChar.ChangeResource(context, ResourceType, -Amount);
			int delta = -AiHelper.GeneralActionConstants.GetResourceHappinessChange(ResourceType, Amount);
			targetChar.ChangeHappiness(context, delta);
			lifeRecordCollection.AddStealResourceSucceedAndEscaped(id, currDate, id2, location, ResourceType, Amount);
			break;
		}
		}
	}
}
