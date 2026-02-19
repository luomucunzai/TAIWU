using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.SocialStatusRandom;

public class SocialStatusBegAction : IGeneralAction
{
	public bool Succeed;

	public int MoneyAmount;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return targetChar.GetResource(6) >= MoneyAmount;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddAskForMoney(id, location, targetChar.GetId());
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		sbyte behaviorType = selfChar.GetBehaviorType();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		if (Succeed)
		{
			selfChar.ChangeResource(context, 6, MoneyAmount);
			targetChar.ChangeResource(context, 6, -MoneyAmount);
			short begSucceedFavorabilityChange = AiHelper.GeneralActionConstants.GetBegSucceedFavorabilityChange(context.Random, behaviorType);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, begSucceedFavorabilityChange);
			lifeRecordCollection.AddAskForMoneySucceed(id, currDate, id2, location, 6, MoneyAmount);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddAcceptRequestGivingMoney(id2, id, MoneyAmount);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			short begFailFavorabilityChange = AiHelper.GeneralActionConstants.GetBegFailFavorabilityChange(context.Random, behaviorType);
			DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, begFailFavorabilityChange);
			lifeRecordCollection.AddAskForMoneyFail(id, currDate, id2, location, 6, MoneyAmount);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddRefuseRequestGivingMoney(id2, id, MoneyAmount);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
