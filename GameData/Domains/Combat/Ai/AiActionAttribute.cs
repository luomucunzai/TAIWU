using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000710 RID: 1808
	[AttributeUsage(AttributeTargets.Class)]
	public class AiActionAttribute : Attribute
	{
		// Token: 0x0600684B RID: 26699 RVA: 0x003B48A0 File Offset: 0x003B2AA0
		public AiActionAttribute(EAiActionType type)
		{
			this.Type = type;
		}

		// Token: 0x04001C84 RID: 7300
		public EAiActionType Type;
	}
}
