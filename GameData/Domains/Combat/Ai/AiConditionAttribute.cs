using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000713 RID: 1811
	[AttributeUsage(AttributeTargets.Class)]
	public class AiConditionAttribute : Attribute
	{
		// Token: 0x06006854 RID: 26708 RVA: 0x003B4A47 File Offset: 0x003B2C47
		public AiConditionAttribute(EAiConditionType type)
		{
			this.Type = type;
		}

		// Token: 0x04001C86 RID: 7302
		public EAiConditionType Type;
	}
}
