using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.SpecialEffect;

namespace GameData.Domains.Combat.Ai.Action;

[AiAction(EAiActionType.CastSkillCastBoost)]
public class AiActionCastSkillCastBoost : AiActionCombatBase
{
	private readonly List<CastBoostEffectDisplayData> _costNeiliEffect = new List<CastBoostEffectDisplayData>();

	public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
	{
		DataContext context = DomainManager.Combat.Context;
		short preparingSkillId = combatChar.GetPreparingSkillId();
		_costNeiliEffect.Clear();
		DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), preparingSkillId, 235, _costNeiliEffect);
		foreach (CastBoostEffectDisplayData item in _costNeiliEffect)
		{
			if (AiCastBoost(combatChar, item))
			{
				DomainManager.SpecialEffect.CostNeiliEffect(context, combatChar.GetId(), preparingSkillId, (short)item.EffectId);
			}
		}
	}

	private static bool AiCastBoost(CombatCharacter combatChar, CastBoostEffectDisplayData data)
	{
		SpecialEffectItem specialEffectItem = Config.SpecialEffect.Instance[data.EffectId];
		if (specialEffectItem.AiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.None)
		{
			return false;
		}
		if (specialEffectItem.AiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.Always)
		{
			return true;
		}
		byte neiliAllocationType = data.NeiliAllocationType;
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		NeiliAllocation originNeiliAllocation = combatChar.GetOriginNeiliAllocation();
		if (neiliAllocation[neiliAllocationType] <= originNeiliAllocation[neiliAllocationType] * 50 / 100)
		{
			return false;
		}
		ESpecialEffectAiCostNeiliAllocationType aiCostNeiliAllocationType = specialEffectItem.AiCostNeiliAllocationType;
		if (1 == 0)
		{
		}
		bool result = aiCostNeiliAllocationType switch
		{
			ESpecialEffectAiCostNeiliAllocationType.OnlyClever => true, 
			ESpecialEffectAiCostNeiliAllocationType.CheckRange => combatChar.AiCastCheckRange(), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}
}
