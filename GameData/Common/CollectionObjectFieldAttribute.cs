using System;

namespace GameData.Common
{
	// Token: 0x020008EF RID: 2287
	[AttributeUsage(AttributeTargets.Method | AttributeTargets.Field)]
	public class CollectionObjectFieldAttribute : Attribute
	{
		// Token: 0x06008217 RID: 33303 RVA: 0x004D9925 File Offset: 0x004D7B25
		public CollectionObjectFieldAttribute(bool isTemplate, bool isArchive, bool isCache, bool isReadonly, bool isCharacterProperty)
		{
			this.IsTemplate = isTemplate;
			this.IsArchive = isArchive;
			this.IsCache = isCache;
			this.IsReadonly = isReadonly;
			this.IsCharacterProperty = isCharacterProperty;
			this.ArrayElementsCount = 0;
		}

		// Token: 0x0400241A RID: 9242
		public readonly bool IsTemplate;

		// Token: 0x0400241B RID: 9243
		public readonly bool IsArchive;

		// Token: 0x0400241C RID: 9244
		public readonly bool IsCache;

		// Token: 0x0400241D RID: 9245
		public readonly bool IsReadonly;

		// Token: 0x0400241E RID: 9246
		public readonly bool IsCharacterProperty;

		// Token: 0x0400241F RID: 9247
		public int ArrayElementsCount;
	}
}
