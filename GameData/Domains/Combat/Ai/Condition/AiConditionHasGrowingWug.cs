using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x0200076F RID: 1903
	[AiCondition(EAiConditionType.HasGrowingWug)]
	public class AiConditionHasGrowingWug : AiConditionCheckCharBase
	{
		// Token: 0x0600698E RID: 27022 RVA: 0x003BA3B9 File Offset: 0x003B85B9
		public AiConditionHasGrowingWug(IReadOnlyList<int> ints) : base(ints)
		{
			this._isNotInCombat = (ints[1] == 1);
			this._isGood = (ints[2] == 1);
			this._wugType = (sbyte)ints[3];
		}

		// Token: 0x0600698F RID: 27023 RVA: 0x003BA3F4 File Offset: 0x003B85F4
		protected unsafe override bool Check(CombatCharacter checkChar)
		{
			bool isGood = this._isGood;
			if (!true)
			{
			}
			sbyte b;
			if (!isGood)
			{
				b = (this._isNotInCombat ? 3 : 2);
			}
			else
			{
				b = (this._isNotInCombat ? 1 : 0);
			}
			if (!true)
			{
			}
			sbyte growthType = b;
			short templateId = ItemDomain.GetWugTemplateId(this._wugType, growthType);
			return (*checkChar.GetCharacter().GetEatingItems()).IndexOfWug(templateId) >= 0;
		}

		// Token: 0x04001D10 RID: 7440
		private readonly bool _isNotInCombat;

		// Token: 0x04001D11 RID: 7441
		private readonly bool _isGood;

		// Token: 0x04001D12 RID: 7442
		private readonly sbyte _wugType;
	}
}
