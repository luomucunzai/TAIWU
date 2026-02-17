using System;

namespace GameData.Domains.Character.ParallelModifications
{
	// Token: 0x02000829 RID: 2089
	public readonly struct InventoryCombatSkillBookParams
	{
		// Token: 0x06007573 RID: 30067 RVA: 0x0044A406 File Offset: 0x00448606
		public InventoryCombatSkillBookParams(short templateId, byte pageTypes)
		{
			this.TemplateId = templateId;
			this.PageTypes = pageTypes;
		}

		// Token: 0x04001F6D RID: 8045
		public readonly short TemplateId;

		// Token: 0x04001F6E RID: 8046
		public readonly byte PageTypes;
	}
}
