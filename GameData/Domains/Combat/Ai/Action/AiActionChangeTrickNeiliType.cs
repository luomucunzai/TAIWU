using System;
using System.Collections.Generic;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E4 RID: 2020
	[AiAction(EAiActionType.ChangeTrickNeiliType)]
	public class AiActionChangeTrickNeiliType : AiActionChangeTrickBase
	{
		// Token: 0x06006A92 RID: 27282 RVA: 0x003BCCA1 File Offset: 0x003BAEA1
		public AiActionChangeTrickNeiliType(IReadOnlyList<string> strings) : base(strings)
		{
		}

		// Token: 0x06006A93 RID: 27283 RVA: 0x003BCCAC File Offset: 0x003BAEAC
		protected override sbyte GetTargetBodyPart(IRandomSource random, sbyte trickType, CombatCharacter combatChar)
		{
			return combatChar.RandomChangeTrickBodyPartByNeiliType(random, trickType);
		}
	}
}
