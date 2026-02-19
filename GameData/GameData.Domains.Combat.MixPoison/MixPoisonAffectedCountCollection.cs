using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat.MixPoison;

[SerializableGameData]
public class MixPoisonAffectedCountCollection : ISerializableGameData
{
	[SerializableGameDataField]
	private Dictionary<sbyte, int> _mixPoisonAffectedCount;

	private Dictionary<sbyte, int> GetOrCreateMixPoisonAffectedCount()
	{
		return _mixPoisonAffectedCount ?? (_mixPoisonAffectedCount = new Dictionary<sbyte, int>());
	}

	public int GetAffectedCount(sbyte templateId)
	{
		return GetOrCreateMixPoisonAffectedCount().GetOrDefault(templateId);
	}

	public MixPoisonAffectedCountCollection AddAffectedCount(sbyte templateId)
	{
		GetOrCreateMixPoisonAffectedCount()[templateId] = GetAffectedCount(templateId) + 1;
		return this;
	}

	public MixPoisonAffectedCountCollection Clear()
	{
		GetOrCreateMixPoisonAffectedCount().Clear();
		return this;
	}

	public MixPoisonAffectedCountCollection()
	{
	}

	public MixPoisonAffectedCountCollection(MixPoisonAffectedCountCollection other)
	{
		_mixPoisonAffectedCount = ((other._mixPoisonAffectedCount == null) ? null : new Dictionary<sbyte, int>(other._mixPoisonAffectedCount));
	}

	public void Assign(MixPoisonAffectedCountCollection other)
	{
		_mixPoisonAffectedCount = ((other._mixPoisonAffectedCount == null) ? null : new Dictionary<sbyte, int>(other._mixPoisonAffectedCount));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfBasicTypePair.GetSerializedSize<sbyte, int>((IReadOnlyDictionary<sbyte, int>)_mixPoisonAffectedCount);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypePair.Serialize<sbyte, int>(ptr, ref _mixPoisonAffectedCount);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfBasicTypePair.Deserialize<sbyte, int>(ptr, ref _mixPoisonAffectedCount);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
