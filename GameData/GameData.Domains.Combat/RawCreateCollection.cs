using System.Collections.Generic;
using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public class RawCreateCollection : ISerializableGameData
{
	[SerializableGameDataField]
	public Dictionary<ItemKey, int> Effects = new Dictionary<ItemKey, int>();

	public readonly Dictionary<ItemKey, ItemKey> Sources = new Dictionary<ItemKey, ItemKey>();

	public readonly Dictionary<ItemKey, long> SpecialEffects = new Dictionary<ItemKey, long>();

	public RawCreateCollection()
	{
	}

	public RawCreateCollection(RawCreateCollection other)
	{
		Effects = ((other.Effects == null) ? null : new Dictionary<ItemKey, int>(other.Effects));
	}

	public void Assign(RawCreateCollection other)
	{
		Effects = ((other.Effects == null) ? null : new Dictionary<ItemKey, int>(other.Effects));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<ItemKey, int>(Effects);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<ItemKey, int>(ptr, ref Effects);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<ItemKey, int>(ptr, ref Effects);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public int TryGetEffectId(ItemKey key)
	{
		int value;
		return Effects.TryGetValue(key, out value) ? value : (-1);
	}

	public bool EffectEquals(ItemKey lhs, ItemKey rhs)
	{
		return TryGetEffectId(lhs) == TryGetEffectId(rhs);
	}

	public bool Any()
	{
		return Effects.Count > 0 || Sources.Count > 0;
	}

	public bool Contains(ItemKey newKey)
	{
		return Effects.ContainsKey(newKey);
	}

	public void Add(ItemKey newKey, ItemKey oldKey, int effectId, long specialEffectId)
	{
		Effects.Add(newKey, effectId);
		Sources.Add(newKey, oldKey);
		SpecialEffects.Add(newKey, specialEffectId);
	}

	public void Remove(ItemKey newKey, out ItemKey oldKey)
	{
		oldKey = Sources[newKey];
		Effects.Remove(newKey);
		Sources.Remove(newKey);
		SpecialEffects.Remove(newKey);
	}

	public void Clear()
	{
		Effects.Clear();
		Sources.Clear();
		SpecialEffects.Clear();
	}
}
