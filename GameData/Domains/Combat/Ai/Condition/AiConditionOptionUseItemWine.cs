using System;
using GameData.Domains.Character;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A9 RID: 1961
	[AiCondition(EAiConditionType.OptionUseItemWine)]
	public class AiConditionOptionUseItemWine : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A07 RID: 27143 RVA: 0x003BBC1E File Offset: 0x003B9E1E
		public AiConditionOptionUseItemWine() : base(EItemSelectorType.Wine)
		{
		}

		// Token: 0x06006A08 RID: 27144 RVA: 0x003BBC2C File Offset: 0x003B9E2C
		protected override bool ExtraCheck(CombatCharacter combatChar)
		{
			Character charObj = combatChar.GetCharacter();
			bool flag = charObj.IsForbiddenToDrinkingWines();
			return !flag && charObj.GetAgeGroup() == 2;
		}
	}
}
