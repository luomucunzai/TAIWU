using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.Item;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GainExpByCricketBattleAction : IGeneralAction
{
	public Wager Wager;

	public bool Succeed;

	public int ExpGain;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return DomainManager.Item.CheckCharacterHasWager(Succeed ? targetChar : selfChar, Wager);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		monthlyEventCollection.AddRequestCricketBattle(id, location, targetChar.GetId());
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (Succeed)
		{
			selfChar.ChangeExp(context, ExpGain);
			lifeRecordCollection.AddCricketBattleWin(id, currDate, id2, location);
			DomainManager.Item.TransferWager(context, targetChar, selfChar, Wager);
			SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset = secretInformationCollection.AddCricketBattleWin(id, id2);
			int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
		}
		else
		{
			targetChar.ChangeExp(context, ExpGain);
			lifeRecordCollection.AddCricketBattleLose(id, currDate, id2, location);
			DomainManager.Item.TransferWager(context, selfChar, targetChar, Wager);
			SecretInformationCollection secretInformationCollection2 = DomainManager.Information.GetSecretInformationCollection();
			int dataOffset2 = secretInformationCollection2.AddCricketBattleWin(id2, id);
			int num2 = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset2);
		}
	}
}
