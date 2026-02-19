using GameData.Common;

namespace GameData.Dependencies;

public class ElementListDependencyAttribute : BaseDataDependencyAttribute
{
	public ElementListDependencyAttribute(ushort domainId, ushort dataId, params ulong[] subId0List)
	{
		SourceType = DomainDataType.ElementList;
		int num = subId0List.Length;
		SourceUids = new DataUid[num];
		for (int i = 0; i < num; i++)
		{
			SourceUids[i] = new DataUid(domainId, dataId, subId0List[i]);
		}
		Condition = InfluenceCondition.None;
		Scope = InfluenceScope.All;
	}

	public ElementListDependencyAttribute(ushort domainId, ushort dataId, int count)
	{
		SourceType = DomainDataType.ElementList;
		SourceUids = new DataUid[count];
		for (int i = 0; i < count; i++)
		{
			SourceUids[i] = new DataUid(domainId, dataId, (ulong)i);
		}
		Condition = InfluenceCondition.None;
		Scope = InfluenceScope.All;
	}
}
