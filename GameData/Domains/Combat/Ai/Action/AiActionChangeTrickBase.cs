using System;
using System.Collections.Generic;
using GameData.Common;
using Redzen.Random;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007B9 RID: 1977
	public abstract class AiActionChangeTrickBase : AiActionCombatBase
	{
		// Token: 0x17000460 RID: 1120
		// (get) Token: 0x06006A2F RID: 27183 RVA: 0x003BC15F File Offset: 0x003BA35F
		protected virtual EFlawOrAcupointType ChangeTrickType
		{
			get
			{
				return EFlawOrAcupointType.None;
			}
		}

		// Token: 0x06006A30 RID: 27184 RVA: 0x003BC162 File Offset: 0x003BA362
		protected virtual sbyte GetTargetBodyPart(IRandomSource random, sbyte trickType, CombatCharacter combatChar)
		{
			return -1;
		}

		// Token: 0x06006A31 RID: 27185 RVA: 0x003BC165 File Offset: 0x003BA365
		protected AiActionChangeTrickBase(IReadOnlyList<string> strings)
		{
			this._key = strings[0];
		}

		// Token: 0x06006A32 RID: 27186 RVA: 0x003BC17C File Offset: 0x003BA37C
		public override void Execute(AiMemoryNew memory, CombatCharacter combatChar)
		{
			DataContext context = DomainManager.Combat.Context;
			short skillId = (short)memory.Ints.GetValueOrDefault(this._key, -1);
			DomainManager.Combat.StartChangeTrick(context, combatChar.IsAlly);
			sbyte trickType = combatChar.AiGetCombatSkillRequireTrickType(skillId);
			trickType = ((trickType < 0) ? combatChar.GetOrRandomChangeTrickType(context.Random) : trickType);
			sbyte bodyPart = this.GetTargetBodyPart(context.Random, trickType, combatChar);
			bodyPart = ((bodyPart < 0) ? combatChar.RandomChangeTrickBodyPart(context.Random, trickType, skillId) : bodyPart);
			DomainManager.Combat.SelectChangeTrick(context, trickType, bodyPart, combatChar.IsAlly, this.ChangeTrickType);
		}

		// Token: 0x04001D50 RID: 7504
		private readonly string _key;
	}
}
