using System.Collections.Generic;

namespace GameData.Common.Binary;

public struct BinaryModificationCollection
{
	public List<BinaryModification> Items;

	public int OriSize;

	public bool MetadataModified;

	private bool _recordingModifications;

	public static BinaryModificationCollection Create()
	{
		BinaryModificationCollection result = default(BinaryModificationCollection);
		result.Items = new List<BinaryModification>();
		result.OriSize = 0;
		result.MetadataModified = false;
		result._recordingModifications = false;
		return result;
	}

	public void ChangeRecording(bool recording, int size)
	{
		_recordingModifications = recording;
		if (!recording)
		{
			Reset(size);
		}
	}

	public void Reset(int size)
	{
		Items.Clear();
		OriSize = size;
		MetadataModified = false;
	}

	public void RecordInserting(int offset, int size)
	{
		if (_recordingModifications)
		{
			Items.Add(new BinaryModification(0, offset, size));
		}
	}

	public void RecordWriting(int offset, int size)
	{
		if (_recordingModifications)
		{
			Items.Add(new BinaryModification(1, offset, size));
		}
	}

	public void RecordRemoving(int offset, int size)
	{
		if (_recordingModifications)
		{
			Items.Add(new BinaryModification(2, offset, size));
		}
	}

	public void RecordSettingMetadata()
	{
		if (_recordingModifications)
		{
			MetadataModified = true;
		}
	}
}
