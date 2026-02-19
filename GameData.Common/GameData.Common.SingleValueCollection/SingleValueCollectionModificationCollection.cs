using System.Collections.Generic;

namespace GameData.Common.SingleValueCollection;

public struct SingleValueCollectionModificationCollection<TKey> where TKey : unmanaged
{
	public List<SingleValueCollectionModification<TKey>> Items;

	private bool _recordingModifications;

	public static SingleValueCollectionModificationCollection<TKey> Create()
	{
		SingleValueCollectionModificationCollection<TKey> result = default(SingleValueCollectionModificationCollection<TKey>);
		result.Items = new List<SingleValueCollectionModification<TKey>>();
		result._recordingModifications = false;
		return result;
	}

	public void ChangeRecording(bool recording)
	{
		_recordingModifications = recording;
		if (!recording)
		{
			Reset();
		}
	}

	public void Reset()
	{
		Items.Clear();
	}

	public void RecordAdding(TKey id)
	{
		if (_recordingModifications)
		{
			Items.Add(new SingleValueCollectionModification<TKey>(0, id));
		}
	}

	public void RecordSetting(TKey id)
	{
		if (_recordingModifications)
		{
			Items.Add(new SingleValueCollectionModification<TKey>(1, id));
		}
	}

	public void RecordRemoving(TKey id)
	{
		if (_recordingModifications)
		{
			Items.Add(new SingleValueCollectionModification<TKey>(2, id));
		}
	}

	public void RecordClearing()
	{
		if (_recordingModifications)
		{
			Items.Clear();
			Items.Add(new SingleValueCollectionModification<TKey>(3, default(TKey)));
		}
	}
}
