using System;

namespace GameData.Domains.Combat.Ai
{
	// Token: 0x02000721 RID: 1825
	[AttributeUsage(AttributeTargets.Class)]
	public class AiNodeAttribute : Attribute
	{
		// Token: 0x060068A6 RID: 26790 RVA: 0x003B7873 File Offset: 0x003B5A73
		public AiNodeAttribute(EAiNodeType type)
		{
			this.Type = type;
		}

		// Token: 0x04001CB1 RID: 7345
		public EAiNodeType Type;
	}
}
