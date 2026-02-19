using GameData.Common;

namespace GameData.Dependencies;

public class SingleValueCollectionDependencyAttribute : BaseDataDependencyAttribute
{
	public SingleValueCollectionDependencyAttribute(ushort domainId, params ushort[] dataIds)
	{
		SourceType = DomainDataType.SingleValueCollection;
		int num = dataIds.Length;
		SourceUids = new DataUid[num];
		for (int i = 0; i < num; i++)
		{
			SourceUids[i] = new DataUid(domainId, dataIds[i], ulong.MaxValue);
		}
		Condition = InfluenceCondition.None;
		Scope = InfluenceScope.All;
	}
}
