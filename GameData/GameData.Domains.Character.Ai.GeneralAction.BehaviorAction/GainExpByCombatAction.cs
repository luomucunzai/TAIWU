using GameData.Common;
using GameData.Domains.Combat;
using GameData.Domains.Map;
using GameData.Domains.World.MonthlyEvent;

namespace GameData.Domains.Character.Ai.GeneralAction.BehaviorAction;

public class GainExpByCombatAction : IGeneralAction
{
	public CombatType CombatType;

	public sbyte ActionEnergyType => 3;

	public bool CheckValid(Character selfChar, Character targetChar)
	{
		return !selfChar.NeedToAvoidCombat(CombatType) && !targetChar.NeedToAvoidCombat(CombatType);
	}

	public void ApplyInitialChangesForTaiwu(DataContext context, Character selfChar, Character targetChar)
	{
		MonthlyEventCollection monthlyEventCollection = DomainManager.World.GetMonthlyEventCollection();
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		Location location = targetChar.GetLocation();
		switch (CombatType)
		{
		case CombatType.Play:
			monthlyEventCollection.AddRequestPlayCombat(id, location, id2);
			break;
		case CombatType.Beat:
			monthlyEventCollection.AddRequestNormalCombat(id, location, id2);
			break;
		}
		CharacterDomain.AddLockMovementCharSet(selfChar.GetId());
	}

	public void ApplyChanges(DataContext context, Character selfChar, Character targetChar)
	{
		int id = selfChar.GetId();
		int id2 = targetChar.GetId();
		int currDate = DomainManager.World.GetCurrDate();
		Location location = selfChar.GetLocation();
		if (id2 != DomainManager.Taiwu.GetTaiwuCharId())
		{
			DomainManager.Character.SimulateCharacterCombat(context, selfChar, targetChar, CombatType, isGroupCombat: false);
		}
	}
}
