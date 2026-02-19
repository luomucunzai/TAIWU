using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom;

public class LifeSkillEntertainmentAction : IGeneralAction
{
	public sbyte LifeSkillType;

	public sbyte ActionEnergyType => 4;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return true;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		int id = targetChar.GetId();
		int id2 = selfChar.GetId();
		MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
		Location location = selfChar.GetLocation();
		switch (LifeSkillType)
		{
		case 0:
			monthlyNotificationCollection.AddAmuseOthersByMusic(id, location, id2);
			break;
		case 1:
			monthlyNotificationCollection.AddAmuseOthersByChess(id, location, id2);
			break;
		case 2:
			monthlyNotificationCollection.AddAmuseOthersByPoem(id, location, id2);
			break;
		case 3:
			monthlyNotificationCollection.AddAmuseOthersByPainting(id, location, id2);
			break;
		default:
			throw new Exception($"Invalid life skill type for entertainment {LifeSkillType}");
		}
		ApplyChanges(context, selfChar, targetChar);
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		int id = targetChar.GetId();
		int id2 = selfChar.GetId();
		short num = Math.Max((short)1, selfChar.GetLifeSkillAttainment(LifeSkillType));
		short num2 = Math.Max((short)1, targetChar.GetLifeSkillAttainment(LifeSkillType));
		int delta = Math.Clamp(num2 * 5 / num - 5, -5, 10);
		int delta2 = Math.Clamp(num * 5 / num2 - 5, -5, 10);
		selfChar.ChangeHappiness(context, delta);
		targetChar.ChangeHappiness(context, delta2);
		int baseDelta = Math.Clamp(num2 * 1500 / num - 1500, -1500, 3000);
		int baseDelta2 = Math.Clamp(num * 1500 / num2 - 1500, -1500, 3000);
		DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, selfChar, targetChar, baseDelta);
		DomainManager.Character.ChangeFavorabilityOptionalMonthlyEvolution(context, targetChar, selfChar, baseDelta2);
		switch (LifeSkillType)
		{
		case 0:
			lifeRecordCollection.AddEntertainWithMusic(id2, currDate, id, location);
			break;
		case 1:
			lifeRecordCollection.AddEntertainWithChess(id2, currDate, id, location);
			break;
		case 2:
			lifeRecordCollection.AddEntertainWithPoem(id2, currDate, id, location);
			break;
		case 3:
			lifeRecordCollection.AddEntertainWithPainting(id2, currDate, id, location);
			break;
		default:
			throw new Exception($"Invalid life skill type for entertainment {LifeSkillType}");
		}
	}
}
