using System;
using System.Collections.Generic;
using GameData.Domains.Item;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000752 RID: 1874
	[AiCondition(EAiConditionType.CurrentWeaponIsSpecial)]
	public class AiConditionCurrentWeaponIsSpecial : AiConditionCheckCharBase
	{
		// Token: 0x06006953 RID: 26963 RVA: 0x003B9CAD File Offset: 0x003B7EAD
		public AiConditionCurrentWeaponIsSpecial(IReadOnlyList<int> ints) : base(ints)
		{
			this._templateId = (short)ints[1];
		}

		// Token: 0x06006954 RID: 26964 RVA: 0x003B9CC8 File Offset: 0x003B7EC8
		protected override bool Check(CombatCharacter checkChar)
		{
			ItemKey weaponKey = checkChar.GetWeapons()[checkChar.GetUsingWeaponIndex()];
			return weaponKey.TemplateId == this._templateId;
		}

		// Token: 0x04001D02 RID: 7426
		private readonly short _templateId;
	}
}
