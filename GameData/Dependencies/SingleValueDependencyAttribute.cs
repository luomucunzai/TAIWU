using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008EC RID: 2284
	public class SingleValueDependencyAttribute : BaseDataDependencyAttribute
	{
		// Token: 0x060081F4 RID: 33268 RVA: 0x004D93D0 File Offset: 0x004D75D0
		public SingleValueDependencyAttribute(ushort domainId, params ushort[] dataIds)
		{
			this.SourceType = DomainDataType.SingleValue;
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
