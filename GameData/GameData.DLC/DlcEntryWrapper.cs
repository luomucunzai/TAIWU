using GameData.Serializer;

namespace GameData.DLC;

[SerializableGameData(NotForDisplayModule = true)]
public class DlcEntryWrapper : ISerializableGameData
{
	[SerializableGameDataField]
	private DlcId _dlcId;

	[SerializableGameDataField]
	private IDlcEntry _dlcEntry;

	public DlcEntryWrapper()
	{
	}

	public DlcEntryWrapper(DlcId dlcId, IDlcEntry dlcEntry)
	{
		_dlcId = dlcId;
		_dlcEntry = dlcEntry;
	}

	public IDlcEntry GetDlcEntry()
	{
		return _dlcEntry;
	}

	public void Update(ulong version, IDlcEntry entry)
	{
		_dlcId.Version = version;
		_dlcEntry = entry;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = _dlcId.GetSerializedSize() + 4;
		IDlcEntry dlcEntry = _dlcEntry;
		return num + ((dlcEntry != null) ? ((ISerializableGameData)dlcEntry).GetSerializedSize() : 0);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _dlcId.Serialize(ptr);
		if (_dlcEntry != null)
		{
			byte* ptr2 = ptr;
			ptr += 4;
			int num = ((ISerializableGameData)_dlcEntry).Serialize(ptr);
			ptr += num;
			*(int*)ptr2 = num;
		}
		else
		{
			*(int*)ptr = 0;
			ptr += 4;
		}
		return (int)(ptr - pData);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += _dlcId.Deserialize(ptr);
		uint num = *(uint*)ptr;
		ptr += 4;
		_dlcEntry = DlcManager.CreateDlcEntry(_dlcId);
		if (num != 0)
		{
			ptr += ((ISerializableGameData)_dlcEntry).Deserialize(ptr);
		}
		return (int)(ptr - pData);
	}
}
