using System;

namespace GameData.Common
{
	// Token: 0x020008F3 RID: 2291
	[AttributeUsage(AttributeTargets.Field)]
	public class DomainDataAttribute : Attribute
	{
		// Token: 0x06008226 RID: 33318 RVA: 0x004D9D1C File Offset: 0x004D7F1C
		public DomainDataAttribute(DomainDataType domainDataType, bool isArchive, bool isCache, bool isReadable = true, bool isWritable = true)
		{
			this.DomainDataType = domainDataType;
			this.IsArchive = isArchive;
			this.IsCache = isCache;
			this.IsReadable = isReadable;
			this.IsWritable = isWritable;
			this.ArrayElementsCount = 0;
			this.CollectionCapacity = 0;
			this.HoldIdsOnly = false;
			this.GenerateModificationFreeMethods = false;
			this.ThreadSafe = false;
			this.IsCompressed = true;
		}

		// Token: 0x0400242B RID: 9259
		public readonly DomainDataType DomainDataType;

		// Token: 0x0400242C RID: 9260
		public readonly bool IsArchive;

		// Token: 0x0400242D RID: 9261
		public readonly bool IsCache;

		// Token: 0x0400242E RID: 9262
		public readonly bool IsReadable;

		// Token: 0x0400242F RID: 9263
		public readonly bool IsWritable;

		// Token: 0x04002430 RID: 9264
		public int ArrayElementsCount;

		// Token: 0x04002431 RID: 9265
		public int CollectionCapacity;

		// Token: 0x04002432 RID: 9266
		public bool HoldIdsOnly;

		// Token: 0x04002433 RID: 9267
		public bool GenerateModificationFreeMethods;

		// Token: 0x04002434 RID: 9268
		public bool ThreadSafe;

		// Token: 0x04002435 RID: 9269
		public bool IsCompressed;
	}
}
