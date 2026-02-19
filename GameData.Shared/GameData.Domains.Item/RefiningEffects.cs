using System;
using Config;
using GameData.Serializer;

namespace GameData.Domains.Item;

public struct RefiningEffects : ISerializableGameData
{
	private unsafe fixed short _materialTemplateIds[5];

	public const int MaxRefineCount = 5;

	public unsafe void Initialize()
	{
		fixed (short* materialTemplateIds = _materialTemplateIds)
		{
			*(long*)materialTemplateIds = -1L;
			materialTemplateIds[4] = -1;
		}
	}

	public short[] GetAllMaterialTemplateIds()
	{
		short[] array = new short[5];
		for (int i = 0; i < 5; i++)
		{
			array[i] = GetMaterialTemplateIdAt(i);
		}
		return array;
	}

	public unsafe short GetMaterialTemplateIdAt(int index)
	{
		if (index < 0 || index >= 5)
		{
			throw new ArgumentOutOfRangeException("index", index, "refining slot index is out of range.");
		}
		return _materialTemplateIds[index];
	}

	public unsafe void RemoveAt(int index)
	{
		if (index < 0 || index >= 5)
		{
			throw new ArgumentOutOfRangeException("index", index, "refining slot index is out of range.");
		}
		_materialTemplateIds[index] = -1;
	}

	public unsafe void Set(int index, short materialTemplateId)
	{
		if (index < 0 || index >= 5)
		{
			throw new ArgumentOutOfRangeException("index", index, "refining slot index is out of range.");
		}
		_materialTemplateIds[index] = materialTemplateId;
	}

	public unsafe sbyte GetTotalRefiningCount()
	{
		sbyte b = 0;
		for (int i = 0; i < 5; i++)
		{
			if (_materialTemplateIds[i] >= 0)
			{
				b++;
			}
		}
		return b;
	}

	public unsafe int GetWeaponPropertyBonus(ERefiningEffectWeaponType effectType)
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			short num2 = _materialTemplateIds[i];
			if (num2 >= 0)
			{
				MaterialItem materialItem = Material.Instance[num2];
				RefiningEffectItem refiningEffectItem = RefiningEffect.Instance[materialItem.RefiningEffect];
				if (refiningEffectItem.WeaponType == effectType)
				{
					num += refiningEffectItem.WeaponBonusValues[materialItem.Grade];
				}
			}
		}
		return num;
	}

	public unsafe int GetArmorPropertyBonus(ERefiningEffectArmorType effectType)
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			short num2 = _materialTemplateIds[i];
			if (num2 >= 0)
			{
				MaterialItem materialItem = Material.Instance[num2];
				RefiningEffectItem refiningEffectItem = RefiningEffect.Instance[materialItem.RefiningEffect];
				if (refiningEffectItem.ArmorType == effectType)
				{
					num += refiningEffectItem.ArmorBonusValues[materialItem.Grade];
				}
			}
		}
		return num;
	}

	public unsafe int GetAccessoryPropertyBonus(ERefiningEffectAccessoryType effectType)
	{
		int num = 0;
		for (int i = 0; i < 5; i++)
		{
			short num2 = _materialTemplateIds[i];
			if (num2 >= 0)
			{
				MaterialItem materialItem = Material.Instance[num2];
				RefiningEffectItem refiningEffectItem = RefiningEffect.Instance[materialItem.RefiningEffect];
				if (refiningEffectItem.AccessoryType == effectType)
				{
					num += refiningEffectItem.AccessoryBonusValues[materialItem.Grade];
				}
			}
		}
		return num;
	}

	public static ERefiningEffectAccessoryType CharPropertyTypeToRefiningEffectAccessoryType(ECharacterPropertyReferencedType propertyType)
	{
		return propertyType switch
		{
			ECharacterPropertyReferencedType.HitRateStrength => ERefiningEffectAccessoryType.HitRateStrength, 
			ECharacterPropertyReferencedType.HitRateTechnique => ERefiningEffectAccessoryType.HitRateTechnique, 
			ECharacterPropertyReferencedType.HitRateSpeed => ERefiningEffectAccessoryType.HitRateSpeed, 
			ECharacterPropertyReferencedType.HitRateMind => ERefiningEffectAccessoryType.HitRateMind, 
			ECharacterPropertyReferencedType.AvoidRateStrength => ERefiningEffectAccessoryType.AvoidRateStrength, 
			ECharacterPropertyReferencedType.AvoidRateTechnique => ERefiningEffectAccessoryType.AvoidRateTechnique, 
			ECharacterPropertyReferencedType.AvoidRateSpeed => ERefiningEffectAccessoryType.AvoidRateSpeed, 
			ECharacterPropertyReferencedType.AvoidRateMind => ERefiningEffectAccessoryType.AvoidRateMind, 
			_ => ERefiningEffectAccessoryType.Invalid, 
		};
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		return 10;
	}

	public unsafe int Serialize(byte* pData)
	{
		fixed (short* materialTemplateIds = _materialTemplateIds)
		{
			*(long*)pData = *(long*)materialTemplateIds;
			((short*)pData)[4] = materialTemplateIds[4];
		}
		return 10;
	}

	public unsafe int Deserialize(byte* pData)
	{
		fixed (short* materialTemplateIds = _materialTemplateIds)
		{
			*(long*)materialTemplateIds = *(long*)pData;
			materialTemplateIds[4] = ((short*)pData)[4];
		}
		return 10;
	}
}
