using System;
using System.Collections.Generic;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.SpecialEffect;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007C5 RID: 1989
	[AiAction(EAiActionType.CastSkillCastBoost)]
	public class AiActionCastSkillCastBoost : AiActionCombatBase
	{
		// Token: 0x06006A53 RID: 27219 RVA: 0x003BC5DC File Offset: 0x003BA7DC
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			short skillId = combatChar.GetPreparingSkillId();
			this._costNeiliEffect.Clear();
			DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), skillId, 235, this._costNeiliEffect, -1, -1, -1);
			foreach (CastBoostEffectDisplayData data in this._costNeiliEffect)
			{
				bool flag = AiActionCastSkillCastBoost.AiCastBoost(combatChar, data);
				if (flag)
				{
					DomainManager.SpecialEffect.CostNeiliEffect(context, combatChar.GetId(), skillId, (short)data.EffectId);
				}
			}
		}

		// Token: 0x06006A54 RID: 27220 RVA: 0x003BC694 File Offset: 0x003BA894
		private unsafe static bool AiCastBoost(CombatCharacter combatChar, CastBoostEffectDisplayData data)
		{
			SpecialEffectItem config = SpecialEffect.Instance[data.EffectId];
			bool flag = config.AiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.None;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				bool flag2 = config.AiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.Always;
				if (flag2)
				{
					result = true;
				}
				else
				{
					byte type = data.NeiliAllocationType;
					NeiliAllocation current = combatChar.GetNeiliAllocation();
					NeiliAllocation origin = combatChar.GetOriginNeiliAllocation();
					bool flag3 = *current[(int)type] <= *origin[(int)type] * 50 / 100;
					if (flag3)
					{
						result = false;
					}
					else
					{
						ESpecialEffectAiCostNeiliAllocationType aiCostNeiliAllocationType = config.AiCostNeiliAllocationType;
						if (!true)
						{
						}
						bool flag4 = aiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.OnlyClever || (aiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.CheckRange && combatChar.AiCastCheckRange());
						if (!true)
						{
						}
						result = flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x04001D5C RID: 7516
		private readonly List<CastBoostEffectDisplayData> _costNeiliEffect = new List<CastBoostEffectDisplayData>();
	}
}
