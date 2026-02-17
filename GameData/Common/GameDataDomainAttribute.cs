using System;

namespace GameData.Common
{
	// Token: 0x020008F7 RID: 2295
	[AttributeUsage(AttributeTargets.Class, Inherited = false)]
	public class GameDataDomainAttribute : Attribute
	{
		// Token: 0x06008239 RID: 33337 RVA: 0x004D9E63 File Offset: 0x004D8063
		public GameDataDomainAttribute(ushort id)
		{
			this.Id = id;
			this.ArchiveAttached = true;
			this.CustomArchiveModuleCode = false;
		}

		// Token: 0x04002438 RID: 9272
		public readonly ushort Id;

		// Token: 0x04002439 RID: 9273
		public bool ArchiveAttached;

		// Token: 0x0400243A RID: 9274
		public bool CustomArchiveModuleCode;
	}
}
