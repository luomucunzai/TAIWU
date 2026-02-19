using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public class WeaponExpectInnerRatioData : ISerializableGameData
{
	[SerializableGameDataField]
	private Dictionary<IntPair, sbyte> _internalValue = new Dictionary<IntPair, sbyte>();

	public void SetValue(int charId, int weaponIndex, sbyte expectInnerRatio)
	{
		_internalValue[new IntPair(charId, weaponIndex)] = expectInnerRatio;
	}

	public sbyte GetValue(int charId, int weaponIndex)
	{
		return _internalValue.GetOrDefault<IntPair, sbyte>(new IntPair(charId, weaponIndex), -1);
	}

	public void Clear()
	{
		_internalValue.Clear();
	}

	public WeaponExpectInnerRatioData()
	{
	}

	public WeaponExpectInnerRatioData(WeaponExpectInnerRatioData other)
	{
		_internalValue = ((other._internalValue == null) ? null : new Dictionary<IntPair, sbyte>(other._internalValue));
	}

	public void Assign(WeaponExpectInnerRatioData other)
	{
		_internalValue = ((other._internalValue == null) ? null : new Dictionary<IntPair, sbyte>(other._internalValue));
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 0;
		num += DictionaryOfCustomTypeBasicTypePair.GetSerializedSize<IntPair, sbyte>(_internalValue);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Serialize<IntPair, sbyte>(ptr, ref _internalValue);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += DictionaryOfCustomTypeBasicTypePair.Deserialize<IntPair, sbyte>(ptr, ref _internalValue);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
