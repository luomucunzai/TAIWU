using GameData.Common;
using GameData.Domains.World.MonthlyEvent;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class DejaVuAction : BasePrioritizedAction
{
	public override short ActionType => 14;

	public override bool CheckValid(Character selfChar)
	{
		if (!base.CheckValid(selfChar))
		{
			return false;
		}
		if (DomainManager.Extra.GetDejaVuEventCharacters().Contains(selfChar.GetId()))
		{
			return false;
		}
		if (Target.TargetCharId != DomainManager.Taiwu.GetTaiwuCharId())
		{
			return false;
		}
		if (!DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			return false;
		}
		return selfChar.GetLocation().AreaId == element.GetLocation().AreaId;
	}

	public override void OnStart(DataContext context, Character selfChar)
	{
		if (DomainManager.Character.TryGetElement_Objects(Target.TargetCharId, out var element))
		{
			if (selfChar.GetLeaderId() != selfChar.GetId())
			{
				DomainManager.Character.LeaveGroup(context, selfChar);
			}
			DomainManager.Character.GroupMove(context, selfChar, element.GetLocation());
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		monthlyEventCollection.AddCrossArchiveReunionWithAcquaintance(selfChar.GetId(), selfChar.GetLocation());
		return false;
	}
}
