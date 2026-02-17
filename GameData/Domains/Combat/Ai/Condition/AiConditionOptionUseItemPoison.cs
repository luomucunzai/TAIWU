using System;
using Config;
using GameData.Domains.Character;
using GameData.Domains.Combat.Ai.Selector;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x020007A7 RID: 1959
	[AiCondition(EAiConditionType.OptionUseItemPoison)]
	public class AiConditionOptionUseItemPoison : AiConditionOptionUseItemCommonBase
	{
		// Token: 0x06006A04 RID: 27140 RVA: 0x003BBBD2 File Offset: 0x003B9DD2
		public AiConditionOptionUseItemPoison() : base(EItemSelectorType.ThrowPoison)
		{
		}

		// Token: 0x06006A05 RID: 27141 RVA: 0x003BBBE0 File Offset: 0x003B9DE0
		protected override bool ExtraCheck(CombatCharacter combatChar)
		{
			GameData.Domains.Character.Character character = combatChar.GetCharacter();
			return Organization.Instance[character.GetOrganizationInfo().OrgTemplateId].AllowPoisoning;
		}
	}
}
