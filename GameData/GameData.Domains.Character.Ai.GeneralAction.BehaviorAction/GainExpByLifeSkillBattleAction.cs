using GameData.Common;
using GameData.Domains.Information.Collection;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GainExpByLifeSkillBattleAction : IGeneralAction
{
	public bool Succeed;

	public int ExpGain;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return true;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		Location location = targetChar.GetLocation();
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddRequestLifeSkillBattle(id, location, targetChar.GetId());
		CharacterDomain.AddLockMovementCharSet(id);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		TempHashsetContainer<int> relatedCharIds = context.AdvanceMonthRelatedData.RelatedCharIds;
		Character character;
		if (Succeed)
		{
			lifeRecordCollection.AddLifeSkillBattleWin(id, currDate, id2, location);
			character = selfChar;
			Character character2 = targetChar;
		}
		else
		{
			lifeRecordCollection.AddLifeSkillBattleLose(id, currDate, id2, location);
			character = targetChar;
			Character character2 = selfChar;
		}
		character.ChangeExp(context, ExpGain);
		SecretInformationCollection secretInformationCollection = DomainManager.Information.GetSecretInformationCollection();
		int dataOffset = secretInformationCollection.AddLifeSkillBattleWin(id, id2);
		int num = DomainManager.Information.AddSecretInformationMetaData(context, dataOffset);
	}
}
