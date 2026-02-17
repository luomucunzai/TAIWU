using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008EB RID: 2283
	public class SingleValueCollectionDependencyAttribute : BaseDataDependencyAttribute
	{
		// Token: 0x060081F3 RID: 33267 RVA: 0x004D936C File Offset: 0x004D756C
		public SingleValueCollectionDependencyAttribute(ushort domainId, params ushort[] dataIds)
		{
			this.SourceType = DomainDataType.SingleValueCollection;
			int dataIdsLength = dataIds.Length;
			this.SourceUids = new DataUid[dataIdsLength];
			for (int i = 0; i < dataIdsLength; i++)
			{
				this.SourceUids[i] = new DataUid(domainId, dataIds[i], ulong.MaxValue, uint.MaxValue);
			}
			this.Condition = InfluenceCondition.None;
			this.Scope = InfluenceScope.All;
		}
	}
}
