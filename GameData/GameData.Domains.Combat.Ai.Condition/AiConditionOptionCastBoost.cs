using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.SpecialEffect;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Condition;

[AiCondition(EAiConditionType.OptionCastBoost)]
public class AiConditionOptionCastBoost : AiConditionCombatBase
{
	private readonly List<CastBoostEffectDisplayData> _costNeiliEffect = new List<CastBoostEffectDisplayData>();

	public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
	{
		if (combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCostNeiliAllocation)
		{
			return false;
		}
		DataContext context = DomainManager.Combat.Context;
		short preparingSkillId = combatChar.GetPreparingSkillId();
		_costNeiliEffect.Clear();
		DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), preparingSkillId, 235, _costNeiliEffect);
		return _costNeiliEffect.Any((CastBoostEffectDisplayData data) => AiCastBoost(context.Random, combatChar, data));
	}

	private static bool AiCastBoost(IRandomSource random, CombatCharacter combatChar, CastBoostEffectDisplayData data)
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
			ESpecialEffectAiCostNeiliAllocationType.OnlyClever => CheckClever(random, combatChar), 
			ESpecialEffectAiCostNeiliAllocationType.CheckRange => combatChar.AiCastCheckRange(), 
			_ => false, 
		};
		if (1 == 0)
		{
		}
		return result;
	}

	private static bool CheckClever(IRandomSource random, CombatCharacter combatChar)
	{
		sbyte personality = combatChar.GetCharacter().GetPersonality(1);
		return random.CheckPercentProb(50 + 50 * personality / 100);
	}
}
