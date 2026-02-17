using System;
using GameData.Domains.CombatSkill;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000826 RID: 2086
	public struct CombatSkillInitialBreakoutData
	{
		// Token: 0x06007570 RID: 30064 RVA: 0x0044A3C5 File Offset: 0x004485C5
		public CombatSkillInitialBreakoutData(CombatSkill combatSkill, ushort activationState, sbyte breakoutStepsCount, sbyte forceBreakoutStepsCount)
		{
			this.CombatSkill = combatSkill;
			this.ActivationState = activationState;
			this.BreakoutStepsCount = breakoutStepsCount;
			this.ForceBreakoutStepsCount = forceBreakoutStepsCount;
			this.ObtainedNeili = 0;
		}

		// Token: 0x04001F51 RID: 8017
		public readonly CombatSkill CombatSkill;

		// Token: 0x04001F52 RID: 8018
		public readonly ushort ActivationState;

		// Token: 0x04001F53 RID: 8019
		public readonly sbyte BreakoutStepsCount;

		// Token: 0x04001F54 RID: 8020
		public readonly sbyte ForceBreakoutStepsCount;

		// Token: 0x04001F55 RID: 8021
		public short ObtainedNeili;
	}
}
