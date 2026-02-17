using System;

namespace GameData.Domains.Combat.MixPoison
{
	// Token: 0x0200070C RID: 1804
	[AttributeUsage(AttributeTargets.Method)]
	public class MixPoisonEffectAttribute : Attribute
	{
		// Token: 0x0600682E RID: 26670 RVA: 0x003B3D87 File Offset: 0x003B1F87
		public MixPoisonEffectAttribute(sbyte templateId)
		{
			this.TemplateId = templateId;
		}

		// Token: 0x04001C83 RID: 7299
		public sbyte TemplateId;
	}
}
