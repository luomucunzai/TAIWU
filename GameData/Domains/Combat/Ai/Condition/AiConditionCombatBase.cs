using System;

namespace GameData.Domains.Combat.Ai.Condition
{
	// Token: 0x02000734 RID: 1844
	public abstract class AiConditionCombatBase : IAiCondition
	{
		// Token: 0x1700044E RID: 1102
		// (get) Token: 0x06006906 RID: 26886 RVA: 0x003B92A1 File Offset: 0x003B74A1
		public int GroupId
		{
			get
			{
				return 1;
			}
		}

		// Token: 0x1700044F RID: 1103
		// (get) Token: 0x06006907 RID: 26887 RVA: 0x003B92A4 File Offset: 0x003B74A4
		// (set) Token: 0x06006908 RID: 26888 RVA: 0x003B92AC File Offset: 0x003B74AC
		public int RuntimeId { get; set; }

		// Token: 0x06006909 RID: 26889 RVA: 0x003B92B8 File Offset: 0x003B74B8
		public bool Check(AiMemoryNew memory, IAiParticipant participant)
		{
			CombatCharacter combatChar = (CombatCharacter)participant;
			return this.Check(memory, combatChar);
		}

		// Token: 0x0600690A RID: 26890
		public abstract bool Check(AiMemoryNew memory, CombatCharacter combatChar);
	}
}
