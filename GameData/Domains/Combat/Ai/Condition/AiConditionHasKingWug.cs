using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000771 RID: 1905
	[AiCondition(EAiConditionType.HasKingWug)]
	public class AiConditionHasKingWug : AiConditionCheckCharBase
	{
		// Token: 0x06006992 RID: 27026 RVA: 0x003BA4BB File Offset: 0x003B86BB
		public AiConditionHasKingWug(IReadOnlyList<int> ints) : base(ints)
		{
			this._wugType = (sbyte)ints[1];
		}

		// Token: 0x06006993 RID: 27027 RVA: 0x003BA4D4 File Offset: 0x003B86D4
		protected unsafe override bool Check(CombatCharacter checkChar)
		{
			short templateId = ItemDomain.GetWugTemplateId(this._wugType, 5);
			return (*checkChar.GetCharacter().GetEatingItems()).IndexOfWug(templateId) >= 0;
		}

		// Token: 0x04001D14 RID: 7444
		private readonly sbyte _wugType;
	}
}
