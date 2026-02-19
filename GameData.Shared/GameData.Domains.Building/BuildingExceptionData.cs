using System.Collections.Generic;
using GameData.Serializer;

namespace GameData.Domains.Building;

public class BuildingExceptionData : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<BuildingBlockKey, BuildingExceptionItem> BuildingExceptionDict = new Dictionary<BuildingBlockKey, BuildingExceptionItem>();

	public BuildingExceptionData()
	{
	}

	public BuildingExceptionData(BuildingExceptionData other)
	{
		if (other.BuildingExceptionDict != null)
		{
			Dictionary<BuildingBlockKey, BuildingExceptionItem> buildingExceptionDict = other.BuildingExceptionDict;
			int count = buildingExceptionDict.Count;
			BuildingExceptionDict = new Dictionary<BuildingBlockKey, BuildingExceptionItem>(count);
			{
				foreach (KeyValuePair<BuildingBlockKey, BuildingExceptionItem> item in buildingExceptionDict)
				{
					BuildingExceptionDict.Add(item.Key, new BuildingExceptionItem(item.Value));
				}
				return;
			}
		}
		BuildingExceptionDict = null;
	}

	public void Assign(BuildingExceptionData other)
	{
		if (other.BuildingExceptionDict != null)
		{
			Dictionary<BuildingBlockKey, BuildingExceptionItem> buildingExceptionDict = other.BuildingExceptionDict;
			int count = buildingExceptionDict.Count;
			BuildingExceptionDict = new Dictionary<BuildingBlockKey, BuildingExceptionItem>(count);
			{
				foreach (KeyValuePair<BuildingBlockKey, BuildingExceptionItem> item in buildingExceptionDict)
				{
					BuildingExceptionDict.Add(item.Key, new BuildingExceptionItem(item.Value));
				}
				return;
			}
		}
		BuildingExceptionDict = null;
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfCustomTypePair.GetSerializedSize<BuildingBlockKey, BuildingExceptionItem>(BuildingExceptionDict);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		int num = (int)(pData + DictionaryOfCustomTypePair.Serialize<BuildingBlockKey, BuildingExceptionItem>(pData, ref BuildingExceptionDict) - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Deserialize(byte* pData)
	{
		int num = (int)(pData + DictionaryOfCustomTypePair.Deserialize<BuildingBlockKey, BuildingExceptionItem>(pData, ref BuildingExceptionDict) - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
