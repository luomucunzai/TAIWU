using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.WealthDemand;

public class ScamResourceAction : IGeneralAction
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
		if (Phase <= 2)
		{
			monthlyNotificationCollection.AddCheatResourceFailure(id, location, id2, ResourceType);
			ApplyChanges(context, selfChar, targetChar);
		}
		else
		{
			monthlyEventCollection.AddScamResource(id, location, id2, ResourceType, Amount);
			CharacterDomain.AddLockMovementCharSet(id);
		}
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
			selfChar.ChangeCurrMainAttribute(context, 2, -GlobalConfig.Instance.HarmfulActionCost);
		}
		if (Phase >= 4)
		{
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddScamResource(id, id2, ResourceType);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		switch (Phase)
		{
		case 0:
			lifeRecordCollection.AddScamResourceFail1(id, currDate, id2, location, ResourceType);
			break;
		case 1:
			lifeRecordCollection.AddScamResourceFail2(id, currDate, id2, location, ResourceType);
			break;
		case 2:
			lifeRecordCollection.AddScamResourceFail3(id, currDate, id2, location, ResourceType);
			break;
		case 3:
			lifeRecordCollection.AddScamResourceFail4(id, currDate, id2, location, ResourceType);
			break;
		case 4:
			lifeRecordCollection.AddScamResourceSucceed(id, currDate, id2, location, ResourceType, Amount);
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
					lifeRecordCollection.AddScamResourceFailAndBeatenUp(id, currDate, id2, location, ResourceType, Amount);
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
			lifeRecordCollection.AddScamResourceSucceedAndEscaped(id, currDate, id2, location, ResourceType, Amount);
			break;
		}
		}
	}
}
