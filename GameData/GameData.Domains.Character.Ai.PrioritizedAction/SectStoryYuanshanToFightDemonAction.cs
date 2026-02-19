using GameData.Common;
using GameData.Serializer;

namespace GameData.Domains.Character.Ai.PrioritizedAction;

[SerializableGameData(NotForDisplayModule = true)]
public class SectStoryYuanshanToFightDemonAction : BasePrioritizedAction
{
	public override short ActionType => 11;

	public override void OnStart(DataContext context, Character selfChar)
	{
		int id = selfChar.GetId();
		int leaderId = selfChar.GetLeaderId();
		if (leaderId >= 0 && leaderId != id)
		{
			DomainManager.Character.LeaveGroup(context, selfChar);
		}
	}

	public override void OnInterrupt(DataContext context, Character selfChar)
	{
	}

	public override bool Execute(DataContext context, Character selfChar)
	{
		return false;
	}

	public override bool CheckValid(Character selfChar)
	{
		return false;
	}
}
