using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008EA RID: 2282
	public class ObjectCollectionDependencyAttribute : BaseDataDependencyAttribute
	{
		// Token: 0x060081F2 RID: 33266 RVA: 0x004D9308 File Offset: 0x004D7508
		public ObjectCollectionDependencyAttribute(ushort domainId, ushort dataId, params ushort[] subId1List)
		{
			this.SourceType = DomainDataType.ObjectCollection;
			int subId1ListLength = subId1List.Length;
			this.SourceUids = new DataUid[subId1ListLength];
			for (int i = 0; i < subId1ListLength; i++)
			{
				this.SourceUids[i] = new DataUid(domainId, dataId, 18446744073709551614UL, (uint)subId1List[i]);
			}
			this.Condition = InfluenceCondition.None;
			this.Scope = InfluenceScope.All;
		}
	}
}
