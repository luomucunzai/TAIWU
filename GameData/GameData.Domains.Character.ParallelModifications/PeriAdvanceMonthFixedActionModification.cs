using System.Collections.Generic;
using GameData.Domains.Map;

namespace GameData.Domains.Character.ParallelModifications;

public class PeriAdvanceMonthFixedActionModification
{
	public enum MakeLoveState
	{
		Legal,
		Illegal,
		Wug,
		RapeSucceed,
		RapeFail
	}

	public readonly Character Character;

	public List<(Character target, MakeLoveState makeLoveState, bool isPregnant)> MakeLoveTargetList;

	public List<int> ReleaseKidnappedCharList;

	public List<MapBlockData> ModifiedMapBlocks;

	public bool LeaveGroup;

	public int NewGroupLeader;

	public short NewGroupActionTemplateId;

	public bool TravelTargetsChanged;

	public bool IsChanged => MakeLoveTargetList != null || ReleaseKidnappedCharList != null || ModifiedMapBlocks != null || LeaveGroup || NewGroupLeader >= 0;

	public PeriAdvanceMonthFixedActionModification(Character character)
	{
		Character = character;
		NewGroupLeader = -1;
		NewGroupActionTemplateId = -1;
	}
}
