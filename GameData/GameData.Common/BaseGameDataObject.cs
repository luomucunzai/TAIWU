using GameData.Dependencies;
using GameData.Domains;
using GameData.Utilities;

namespace GameData.Common;

public abstract class BaseGameDataObject
{
	public ObjectCollectionHelperData CollectionHelperData;

	public int DataStatesOffset;

	protected BaseGameDataObject()
	{
		DataStatesOffset = -1;
	}

	protected void SetModifiedAndInvalidateInfluencedCache(ushort fieldId, DataContext context)
	{
		CollectionHelperData.DataStates.SetModified(DataStatesOffset, fieldId);
		DataInfluence[] array = CollectionHelperData.CacheInfluences[fieldId];
		if (array == null)
		{
			return;
		}
		Tester.Assert(context != null);
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			DataInfluence dataInfluence = array[i];
			if (InfluenceChecker.CheckCondition(context, this, dataInfluence.Condition))
			{
				BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataInfluence.TargetIndicator.DomainId];
				baseGameDataDomain.InvalidateCache(this, dataInfluence, context, unconditionallyInfluenceAll: false);
			}
		}
	}

	public void InvalidateSelfAndInfluencedCache(ushort fieldId, DataContext context)
	{
		ObjectCollectionDataStates dataStates = CollectionHelperData.DataStates;
		dataStates.SetModified(DataStatesOffset, fieldId);
		if (!dataStates.IsCached(DataStatesOffset, fieldId))
		{
			return;
		}
		dataStates.ResetCached(DataStatesOffset, fieldId);
		DataInfluence[] array = CollectionHelperData.CacheInfluences[fieldId];
		if (array == null)
		{
			return;
		}
		Tester.Assert(context != null);
		int num = array.Length;
		for (int i = 0; i < num; i++)
		{
			DataInfluence dataInfluence = array[i];
			if (InfluenceChecker.CheckCondition(context, this, dataInfluence.Condition))
			{
				BaseGameDataDomain baseGameDataDomain = DomainManager.Domains[dataInfluence.TargetIndicator.DomainId];
				baseGameDataDomain.InvalidateCache(this, dataInfluence, context, unconditionallyInfluenceAll: false);
			}
		}
	}
}
