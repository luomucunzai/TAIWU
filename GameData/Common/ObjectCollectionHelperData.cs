using System;
using GameData.Dependencies;

namespace GameData.Common
{
	// Token: 0x020008F9 RID: 2297
	public class ObjectCollectionHelperData
	{
		// Token: 0x06008249 RID: 33353 RVA: 0x004DA22F File Offset: 0x004D842F
		public ObjectCollectionHelperData(ushort domainId, ushort dataId, DataInfluence[][] cacheInfluences, ObjectCollectionDataStates dataStates, bool isArchive)
		{
			this.DomainId = domainId;
			this.DataId = dataId;
			this.CacheInfluences = cacheInfluences;
			this.DataStates = dataStates;
			this.IsArchive = isArchive;
		}

		// Token: 0x04002442 RID: 9282
		public readonly ushort DomainId;

		// Token: 0x04002443 RID: 9283
		public readonly ushort DataId;

		// Token: 0x04002444 RID: 9284
		public readonly DataInfluence[][] CacheInfluences;

		// Token: 0x04002445 RID: 9285
		public readonly ObjectCollectionDataStates DataStates;

		// Token: 0x04002446 RID: 9286
		public readonly bool IsArchive;
	}
}
