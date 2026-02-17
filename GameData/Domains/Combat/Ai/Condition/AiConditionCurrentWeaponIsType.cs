using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000753 RID: 1875
	[AiCondition(EAiConditionType.CurrentWeaponIsType)]
	public class AiConditionCurrentWeaponIsType : AiConditionCheckCharBase
	{
		// Token: 0x06006955 RID: 26965 RVA: 0x003B9CFA File Offset: 0x003B7EFA
		public AiConditionCurrentWeaponIsType(IReadOnlyList<int> ints) : base(ints)
		{
			this._itemSubType = (short)ints[1];
		}

		// Token: 0x06006956 RID: 26966 RVA: 0x003B9D14 File Offset: 0x003B7F14
		protected override bool Check(CombatCharacter checkChar)
		{
			ItemKey weaponKey = checkChar.GetWeapons()[checkChar.GetUsingWeaponIndex()];
			return ItemTemplateHelper.GetItemSubType(weaponKey.ItemType, weaponKey.TemplateId) == this._itemSubType;
		}

		// Token: 0x04001D03 RID: 7427
		private readonly short _itemSubType;
	}
}
