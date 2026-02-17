using System;
using System.Collections.Generic;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007E3 RID: 2019
	[AiAction(EAiActionType.ChangeTrickAcupoint)]
	public class AiActionChangeTrickAcupoint : AiActionChangeTrickBase
	{
		// Token: 0x17000468 RID: 1128
		// (get) Token: 0x06006A8F RID: 27279 RVA: 0x003BCC7D File Offset: 0x003BAE7D
		protected override EFlawOrAcupointType ChangeTrickType
		{
			get
			{
				return EFlawOrAcupointType.Acupoint;
			}
		}

		// Token: 0x06006A90 RID: 27280 RVA: 0x003BCC80 File Offset: 0x003BAE80
		protected override sbyte GetTargetBodyPart(IRandomSource random, sbyte trickType, CombatCharacter combatChar)
		{
			return this._bodyPart;
		}

		// Token: 0x06006A91 RID: 27281 RVA: 0x003BCC88 File Offset: 0x003BAE88
		public AiActionChangeTrickAcupoint(IReadOnlyList<string> strings, IReadOnlyList<int> ints) : base(strings)
		{
			this._bodyPart = (sbyte)ints[0];
		}

		// Token: 0x04001D6D RID: 7533
		private readonly sbyte _bodyPart;
	}
}
