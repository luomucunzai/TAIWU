using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.DomainEvents;
using GameData.Domains.Character;
using GameData.Domains.Combat;

namespace GameData.Domains.SpecialEffect.Animal.Loong.Carrier;

public class Chiwen : CarrierEffectBase
{
	private const int AddOrReduceNeiliAllocationValue = 2;

	protected override short CombatStateId => 207;

	public Chiwen(int charId)
		: base(charId)
	{
	}

	protected override void OnEnableSubClass(DataContext context)
	{
		Events.RegisterHandler_CastSkillTrickCosted(OnCastSkillTrickCosted);
	}

	protected override void OnDisableSubClass(DataContext context)
	{
		Events.UnRegisterHandler_CastSkillTrickCosted(OnCastSkillTrickCosted);
	}

	private void OnCastSkillTrickCosted(DataContext context, CombatCharacter combatChar, short skillId, List<NeedTrick> costTricks)
	{
		if (combatChar.GetId() != base.CharacterId && combatChar.IsAlly == base.CombatChar.IsAlly)
		{
			return;
		}
		bool flag = combatChar.GetId() == base.CharacterId;
		int num = costTricks.Sum((NeedTrick x) => x.NeedCount);
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
		int addValue = num * 2 * (flag ? 1 : (-1));
		for (byte b = 0; b < 4; b++)
		{
			if (!(flag ? (neiliAllocation[b] >= originNeiliAllocation[b]) : (neiliAllocation[b] <= originNeiliAllocation[b])))
			{
				combatChar.ChangeNeiliAllocation(context, b, addValue);
			}
		}
	}
}
