using GameData.Serializer;

namespace GameData.Domains.Building.Display;

[SerializableGameData(NotRestrictCollectionSerializedSize = true)]
public class CricketCollectionBatchButtonStateDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	public bool HasCricketInCollection;

	[SerializableGameDataField]
	public bool HasJarInCollection;

	[SerializableGameDataField]
	public bool HasCricketInSources;

	[SerializableGameDataField]
	public bool HasEmptyJarInCollection;

	[SerializableGameDataField]
	public bool HasJarInSources;

	[SerializableGameDataField]
	public bool HasEmptyPositionInCollection;

	public CricketCollectionBatchButtonStateDisplayData()
	{
	}

	public CricketCollectionBatchButtonStateDisplayData(CricketCollectionBatchButtonStateDisplayData other)
	{
		HasCricketInCollection = other.HasCricketInCollection;
		HasJarInCollection = other.HasJarInCollection;
		HasCricketInSources = other.HasCricketInSources;
		HasEmptyJarInCollection = other.HasEmptyJarInCollection;
		HasJarInSources = other.HasJarInSources;
		HasEmptyPositionInCollection = other.HasEmptyPositionInCollection;
	}

	public void Assign(CricketCollectionBatchButtonStateDisplayData other)
	{
		HasCricketInCollection = other.HasCricketInCollection;
		HasJarInCollection = other.HasJarInCollection;
		HasCricketInSources = other.HasCricketInSources;
		HasEmptyJarInCollection = other.HasEmptyJarInCollection;
		HasJarInSources = other.HasJarInSources;
		HasEmptyPositionInCollection = other.HasEmptyPositionInCollection;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 6;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*pData = (HasCricketInCollection ? ((byte)1) : ((byte)0));
		byte* num = pData + 1;
		*num = (HasJarInCollection ? ((byte)1) : ((byte)0));
		byte* num2 = num + 1;
		*num2 = (HasCricketInSources ? ((byte)1) : ((byte)0));
		byte* num3 = num2 + 1;
		*num3 = (HasEmptyJarInCollection ? ((byte)1) : ((byte)0));
		byte* num4 = num3 + 1;
		*num4 = (HasJarInSources ? ((byte)1) : ((byte)0));
		byte* num5 = num4 + 1;
		*num5 = (HasEmptyPositionInCollection ? ((byte)1) : ((byte)0));
		int num6 = (int)(num5 + 1 - pData);
		if (num6 > 4)
		{
			return (num6 + 3) / 4 * 4;
		}
		return num6;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		HasCricketInCollection = *ptr != 0;
		ptr++;
		HasJarInCollection = *ptr != 0;
		ptr++;
		HasCricketInSources = *ptr != 0;
		ptr++;
		HasEmptyJarInCollection = *ptr != 0;
		ptr++;
		HasJarInSources = *ptr != 0;
		ptr++;
		HasEmptyPositionInCollection = *ptr != 0;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
