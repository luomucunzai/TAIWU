using GameData.Common;

namespace GameData.Dependencies;

public class SingleValueDependencyAttribute : BaseDataDependencyAttribute
{
	public SingleValueDependencyAttribute(ushort domainId, params ushort[] dataIds)
	{
		SourceType = DomainDataType.SingleValue;
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
