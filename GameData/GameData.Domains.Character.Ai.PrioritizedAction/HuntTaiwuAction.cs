using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true, IsExtensible = true, NoCopyConstructors = true)]
public class HuntTaiwuAction : ExtensiblePrioritizedAction
{
	public override short ActionType => 22;

	public override bool CheckValid(Character selfChar)
	{
		if (DomainManager.World.GetMainStoryLineProgress() >= 27)
		{
			return false;
		}
		if (!DomainManager.Character.IsCharacterAlive(selfChar.GetId()))
		{
			return false;
		}
		if (!DomainManager.Character.IsCharacterAlive(Target.TargetCharId))
		{
			return false;
		}
		return base.CheckValid(selfChar);
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		AdaptableLog.Info($"{selfChar} start huntTaiwuAction,target is {DomainManager.Character.GetElement_Objects(Target.TargetCharId)}.");
		int id = selfChar.GetId();
		int targetCharId = Target.TargetCharId;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddJieQingPunishmentAssassinSetOut(id, currDate, targetCharId);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		AdaptableLog.Info(DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element) ? $"{selfChar} end huntTaiwuAction {element}." : $"{selfChar} end huntTaiwuAction {Target.TargetCharId}.");
		int id = selfChar.GetId();
		int targetCharId = Target.TargetCharId;
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddJieQingPunishmentAssassinGiveUp(id, currDate, targetCharId);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (DomainManager.World.ClearMonthlyEventCollectionNotEndGame())
		{
			OnInterrupt(context, selfChar);
			return true;
		}
		Location location = DomainManager.Taiwu.GetTaiwu().GetLocation();
		Location location2 = selfChar.GetLocation();
		if (location.Equals(location2))
		{
			DomainManager.Taiwu.JieqingPunishmentAssassinAlreadyAdd = true;
			MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
			monthlyEventCollection.AddJieQingPunishmentAssassin(DomainManager.Taiwu.GetTaiwuCharId(), location2, selfChar.GetId());
			return true;
		}
		AdaptableLog.Info($"{selfChar} cannot hunt taiwu, because taiwu is not in the same location.selfLocation is {location2}, taiwuLocation is {location}.");
		return false;
	}
}
