using GameData.Common;
using GameData.Domains.LifeRecord;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class SectStoryBaihuaToCureManic : BasePrioritizedAction
{
	public override short ActionType => 16;

	public override void OnStart(DataContext context, Character selfChar)
	{
		DomainManager.World.BaihuaAddCharIdToCureSpecialDebuffIntList(context, Target.TargetCharId);
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
		DomainManager.World.BaihuaRemoveCharIdToCureSpecialDebuffIntList(context, Target.TargetCharId);
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return !TryChangeTarget(context);
		}
		if (!element.IsInteractableAsIntelligentCharacter())
		{
			return !TryChangeTarget(context);
		}
		if (!element.RemoveFeatureGroup(context, 541))
		{
			return !TryChangeTarget(context);
		}
		int currDate = DomainManager.World.GetCurrDate();
		LifeRecordCollection lifeRecordCollection = DomainManager.LifeRecord.GetLifeRecordCollection();
		lifeRecordCollection.AddSectMainStoryBaihuaManiaCure(selfChar.GetId(), currDate, element.GetId(), selfChar.GetLocation());
		return !TryChangeTarget(context);
	}

	private bool TryChangeTarget(DataContext context)
	{
		if (!DomainManager.Character.BaihuaManicCharIds.TryTake(out var result))
		{
			return false;
		}
		DomainManager.World.BaihuaRemoveCharIdToCureSpecialDebuffIntList(context, Target.TargetCharId);
		Target.TargetCharId = result;
		DomainManager.World.BaihuaAddCharIdToCureSpecialDebuffIntList(context, Target.TargetCharId);
		return true;
	}
}
