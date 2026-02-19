using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;

public class SocialStatusHealAction : IGeneralAction
{
	public EHealActionType Type;

	public int HerbAmount;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return selfChar.GetResource(5) >= HerbAmount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		switch (Type)
		{
		case EHealActionType.Healing:
			monthlyEventCollection.AddAdviseHealInjury(id, location, id2, HerbAmount);
			break;
		case EHealActionType.Detox:
			monthlyEventCollection.AddAdviseHealPoison(id, location, id2, HerbAmount);
			break;
		case EHealActionType.Breathing:
			monthlyEventCollection.AddAdviseHealDisorderOfQi(id, location, id2, HerbAmount);
			break;
		case EHealActionType.Recover:
			monthlyEventCollection.AddAdviseHealHealth(id, location, id2, HerbAmount);
			break;
		}
		CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		selfChar.ChangeResource(context, 5, HerbAmount);
		DomainManager.Character.UseCombatResources(context, id, Type, 1);
		if (selfChar.DoHealAction(context, Type, targetChar))
		{
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, 3000);
		}
		lifeRecordCollection.AddCureSucceed(id, currDate, id2, location);
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddCure(id, id2);
		int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
	}
}
