using System;
using GameData.Common;

namespace GameData.Dependencies
{
	// Token: 0x020008E6 RID: 2278
	public class ElementListDependencyAttribute : BaseDataDependencyAttribute
	{
		// Token: 0x060081C1 RID: 33217 RVA: 0x004D8014 File Offset: 0x004D6214
		public ElementListDependencyAttribute(ushort domainId, ushort dataId, params ulong[] subId0List)
		{
			this.SourceType = DomainDataType.ElementList;
			int subId0ListLength = subId0List.Length;
			this.SourceUids = new DataUid[subId0ListLength];
			for (int i = 0; i < subId0ListLength; i++)
			{
				this.SourceUids[i] = new DataUid(domainId, dataId, subId0List[i], uint.MaxValue);
			}
			this.Condition = InfluenceCondition.None;
			this.Scope = InfluenceScope.All;
		}

		// Token: 0x060081C2 RID: 33218 RVA: 0x004D8078 File Offset: 0x004D6278
		public ElementListDependencyAttribute(ushort domainId, ushort dataId, int count)
		{
			this.SourceType = DomainDataType.ElementList;
			this.SourceUids = new DataUid[count];
			for (int subId0 = 0; subId0 < count; subId0++)
			{
				this.SourceUids[subId0] = new DataUid(domainId, dataId, (ulong)((long)subId0), uint.MaxValue);
			}
			this.Condition = InfluenceCondition.None;
			this.Scope = InfluenceScope.All;
		}
	}
}
