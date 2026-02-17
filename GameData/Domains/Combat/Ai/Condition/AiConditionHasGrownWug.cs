using System;
using System.Collections.Generic;
using GameData.Domains.Character;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000770 RID: 1904
	[AiCondition(EAiConditionType.HasGrownWug)]
	public class AiConditionHasGrownWug : AiConditionCheckCharBase
	{
		// Token: 0x06006990 RID: 27024 RVA: 0x003BA465 File Offset: 0x003B8665
		public AiConditionHasGrownWug(IReadOnlyList<int> ints) : base(ints)
		{
			this._wugType = (sbyte)ints[1];
		}

		// Token: 0x06006991 RID: 27025 RVA: 0x003BA480 File Offset: 0x003B8680
		protected unsafe override bool Check(CombatCharacter checkChar)
		{
			short templateId = ItemDomain.GetWugTemplateId(this._wugType, 4);
			return (*checkChar.GetCharacter().GetEatingItems()).IndexOfWug(templateId) >= 0;
		}

		// Token: 0x04001D13 RID: 7443
		private readonly sbyte _wugType;
	}
}
