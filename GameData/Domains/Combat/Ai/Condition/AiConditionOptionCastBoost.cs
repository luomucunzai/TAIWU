using System;
using System.Collections.Generic;
using System.Linq;
using Config;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.SpecialEffect;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000795 RID: 1941
	[AiCondition(EAiConditionType.OptionCastBoost)]
	public class AiConditionOptionCastBoost : AiConditionCombatBase
	{
		// Token: 0x060069DE RID: 27102 RVA: 0x003BB508 File Offset: 0x003B9708
		public override bool Check(AiMemoryNew memory, CombatCharacter combatChar)
		{
			bool flag = combatChar.IsAlly && !DomainManager.Combat.AiOptions.AutoCostNeiliAllocation;
			bool result;
			if (flag)
			{
				result = false;
			}
			else
			{
				DataContext context = DomainManager.Combat.Context;
				short skillId = combatChar.GetPreparingSkillId();
				this._costNeiliEffect.Clear();
				DomainManager.SpecialEffect.ModifyData(combatChar.GetId(), skillId, 235, this._costNeiliEffect, -1, -1, -1);
				result = this._costNeiliEffect.Any((CastBoostEffectDisplayData data) => AiConditionOptionCastBoost.AiCastBoost(context.Random, combatChar, data));
			}
			return result;
		}

		// Token: 0x060069DF RID: 27103 RVA: 0x003BB5B8 File Offset: 0x003B97B8
		private unsafe static bool AiCastBoost(IRandomSource random, CombatCharacter combatChar, CastBoostEffectDisplayData data)
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
						bool flag4;
						if (aiCostNeiliAllocationType != ESpecialEffectAiCostNeiliAllocationType.OnlyClever)
						{
							flag4 = (aiCostNeiliAllocationType == ESpecialEffectAiCostNeiliAllocationType.CheckRange && combatChar.AiCastCheckRange());
						}
						else
						{
							flag4 = AiConditionOptionCastBoost.CheckClever(random, combatChar);
						}
						if (!true)
						{
						}
						result = flag4;
					}
				}
			}
			return result;
		}

		// Token: 0x060069E0 RID: 27104 RVA: 0x003BB680 File Offset: 0x003B9880
		private static bool CheckClever(IRandomSource random, CombatCharacter combatChar)
		{
			sbyte clever = combatChar.GetCharacter().GetPersonality(1);
			return random.CheckPercentProb((int)(50 + 50 * clever / 100));
		}

		// Token: 0x04001D39 RID: 7481
		private readonly List<CastBoostEffectDisplayData> _costNeiliEffect = new List<CastBoostEffectDisplayData>();
	}
}
