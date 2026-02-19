using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom;

public class LifeSkillDivinationAction : IGeneralAction
{
	public int SecretInfoMetaDataId;

	public bool Succeed;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		if (selfChar.GetHealth() < selfChar.GetLeftMaxHealth() || selfChar.GetHealth() <= 12)
		{
			return false;
		}
		if (DomainManager.Information.TryGetElement_CharacterSecretInformation(selfChar.GetId(), out var value) && value.Collection.ContainsKey(SecretInfoMetaDataId))
		{
			return false;
		}
		return true;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		if (Succeed)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			int id = selfChar.GetId();
			int id2 = targetChar.GetId();
			Location location = selfChar.GetLocation();
			monthlyNotificationCollection.AddPractiseDivination(id, location, id2);
		}
		ApplyChanges(context, selfChar, targetChar);
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
			lifeRecordCollection.AddDivinationSucceed(id, currDate, id2, location);
			DomainManager.Information.ReceiveSecretInformation(context, SecretInfoMetaDataId, id, id2);
		}
		else
		{
			lifeRecordCollection.AddDivinationFail(id, currDate, location);
			selfChar.ChangeHealth(context, -12);
		}
	}
}
