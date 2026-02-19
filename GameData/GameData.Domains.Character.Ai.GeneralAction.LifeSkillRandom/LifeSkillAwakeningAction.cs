using System;
using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.Notification;

namespace GameData.Domains.Character.Ai.GeneralAction.LifeSkillRandom;

public class LifeSkillAwakeningAction : IGeneralAction
{
	public sbyte AwakeningLifeSkillType;

	public sbyte IncreasedLifeSkillType;

	public sbyte ActionEnergyType => 4;

	public unsafe bool CheckValid(Character selfChar, Character targetChar)
	{
		LifeSkillShorts baseLifeSkillQualifications = targetChar.GetBaseLifeSkillQualifications();
		return baseLifeSkillQualifications.Items[IncreasedLifeSkillType] < 90;
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		if (IncreasedLifeSkillType >= 0)
		{
			MonthlyNotificationCollection monthlyNotificationCollection = DomainManager.World.GetMonthlyNotificationCollection();
			switch (AwakeningLifeSkillType)
			{
			case 13:
				monthlyNotificationCollection.AddEnlightenedByBuddhism(selfChar.GetId(), selfChar.GetLocation(), targetChar.GetId(), IncreasedLifeSkillType);
				break;
			case 12:
				monthlyNotificationCollection.AddEnlightenedByDaoism(selfChar.GetId(), selfChar.GetLocation(), targetChar.GetId(), IncreasedLifeSkillType);
				break;
			default:
				throw new Exception($"{selfChar.GetId()} is trying to awake {targetChar.GetId()}'s {AwakeningLifeSkillType}, which is neither Buddhism nor Taoism");
			}
		}
		ApplyChanges(context, selfChar, targetChar);
	}

	public unsafe void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (IncreasedLifeSkillType >= 0)
		{
			switch (AwakeningLifeSkillType)
			{
			case 13:
				lifeRecordCollection.AddBuddismAwakeningSucceed(id, currDate, id2, location);
				break;
			case 12:
				lifeRecordCollection.AddTaoismAwakeningSucceed(id, currDate, id2, location);
				break;
			default:
				throw new Exception($"{selfChar.GetId()} is trying to awake {targetChar.GetId()}'s {AwakeningLifeSkillType}, which is neither Buddhism nor Taoism");
			}
			LifeSkillShorts baseLifeSkillQualifications = targetChar.GetBaseLifeSkillQualifications();
			ref short reference = ref baseLifeSkillQualifications.Items[IncreasedLifeSkillType];
			reference++;
			targetChar.SetBaseLifeSkillQualifications(ref baseLifeSkillQualifications, context);
		}
		else
		{
			switch (AwakeningLifeSkillType)
			{
			case 13:
				lifeRecordCollection.AddBuddismAwakeningFail(id, currDate, id2, location);
				return;
			case 12:
				lifeRecordCollection.AddTaoismAwakeningFail(id, currDate, id2, location);
				return;
			}
			throw new Exception($"{selfChar.GetId()} is trying to awake {targetChar.GetId()}'s {AwakeningLifeSkillType}, which is neither Buddhism nor Taoism");
		}
	}
}
