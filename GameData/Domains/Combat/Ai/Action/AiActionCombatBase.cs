using System;

namespace GameData.Domains.Combat.Ai.Action
{
	// Token: 0x020007B8 RID: 1976
	public abstract class AiActionCombatBase : IAiAction
	{
		// Token: 0x1700045E RID: 1118
		// (get) Token: 0x06006A29 RID: 27177 RVA: 0x003BC121 File Offset: 0x003BA321
		public int GroupId
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700045F RID: 1119
		// (get) Token: 0x06006A2A RID: 27178 RVA: 0x003BC124 File Offset: 0x003BA324
		// (set) Token: 0x06006A2B RID: 27179 RVA: 0x003BC12C File Offset: 0x003BA32C
		public int RuntimeId { get; set; }

		// Token: 0x06006A2C RID: 27180 RVA: 0x003BC138 File Offset: 0x003BA338
		public void Execute(AiMemoryNew memory, IAiParticipant participant)
		{
			CombatCharacter combatChar = (CombatCharacter)participant;
			this.Execute(memory, combatChar);
		}

		// Token: 0x06006A2D RID: 27181
		public abstract void Execute(AiMemoryNew memory, CombatCharacter combatChar);
	}
}
