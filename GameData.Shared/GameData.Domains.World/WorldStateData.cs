using System;
using System.Collections.Generic;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.World;

public struct WorldStateData : ISerializableGameData
{
	public enum EStorageType : sbyte
	{
		Invalid = -1,
		Warehouse,
		Trough,
		Count
	}

	[SerializableGameDataField]
	private ulong _worldStates;

	[SerializableGameDataField]
	private ushort _awakeningXiangshuAvatars;

	[SerializableGameDataField]
	private ushort _attackingXiangshuAvatars;

	[SerializableGameDataField]
	private byte _poisonTypes;

	[SerializableGameDataField]
	private byte _overloadingResourceTypes;

	[SerializableGameDataField]
	private byte _outerInjuryParts;

	[SerializableGameDataField]
	private byte _innerInjuryParts;

	[SerializableGameDataField]
	private byte _overloadStorageTypes;

	public void ClearWorldStates()
	{
		_worldStates = 0uL;
		_awakeningXiangshuAvatars = 0;
		_attackingXiangshuAvatars = 0;
		_poisonTypes = 0;
		_overloadingResourceTypes = 0;
		_outerInjuryParts = 0;
		_innerInjuryParts = 0;
	}

	public void SetWorldState(short templateId)
	{
		_worldStates = BitOperation.SetBit(_worldStates, (int)templateId, true);
	}

	public bool GetWorldState(short templateId)
	{
		return BitOperation.GetBit(_worldStates, (int)templateId);
	}

	public void AddAwakeningXiangshuAvatar(sbyte xiangshuAvatarId)
	{
		_awakeningXiangshuAvatars = BitOperation.SetBit(_awakeningXiangshuAvatars, (int)xiangshuAvatarId, true);
	}

	public bool IsXiangshuAvatarAwakening(sbyte xiangshuAvatarId)
	{
		return BitOperation.GetBit(_awakeningXiangshuAvatars, (int)xiangshuAvatarId);
	}

	public void AddAttackingXiangshuAvatar(sbyte xiangshuAvatarId)
	{
		_attackingXiangshuAvatars = BitOperation.SetBit(_attackingXiangshuAvatars, (int)xiangshuAvatarId, true);
	}

	public bool IsXiangshuAvatarAttacking(sbyte xiangshuAvatarId)
	{
		return BitOperation.GetBit(_attackingXiangshuAvatars, (int)xiangshuAvatarId);
	}

	public void AddPoisonType(sbyte poisonType)
	{
		_poisonTypes = BitOperation.SetBit(_poisonTypes, (int)poisonType, true);
	}

	public bool IsPoisonedWithType(sbyte poisonType)
	{
		return BitOperation.GetBit(_poisonTypes, (int)poisonType);
	}

	public void AddInnerInjuryBodyPart(sbyte bodyPart)
	{
		_innerInjuryParts = BitOperation.SetBit(_innerInjuryParts, (int)bodyPart, true);
	}

	public bool BodyPartHasInnerInjury(sbyte bodyPart)
	{
		return BitOperation.GetBit(_innerInjuryParts, (int)bodyPart);
	}

	public void AddOuterInjuryBodyPart(sbyte bodyPart)
	{
		_outerInjuryParts = BitOperation.SetBit(_outerInjuryParts, (int)bodyPart, true);
	}

	public bool BodyPartHasOuterInjury(sbyte bodyPart)
	{
		return BitOperation.GetBit(_outerInjuryParts, (int)bodyPart);
	}

	public bool AnyOuterInjury()
	{
		return _outerInjuryParts != 0;
	}

	public bool AnyInnerInjury()
	{
		return _innerInjuryParts != 0;
	}

	public void AddOverloadingResourceType(sbyte resourceType)
	{
		_overloadingResourceTypes = BitOperation.SetBit(_overloadingResourceTypes, (int)resourceType, true);
	}

	public bool IsResourceOverloaded(sbyte resourceType)
	{
		return BitOperation.GetBit(_overloadingResourceTypes, (int)resourceType);
	}

	public void AddOverloadStorageType(EStorageType storageType)
	{
		if (storageType != EStorageType.Warehouse && storageType != EStorageType.Trough)
		{
			throw new ArgumentOutOfRangeException("storageType", "Storage type must be 0 or 1.");
		}
		_overloadStorageTypes = BitOperation.SetBit(_overloadStorageTypes, (int)storageType, true);
	}

	public IEnumerable<EStorageType> IterateOverloadStorageTypes()
	{
		for (sbyte i = 0; i < 2; i++)
		{
			if (BitOperation.GetBit(_overloadStorageTypes, (int)i))
			{
				yield return (EStorageType)i;
			}
		}
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 17;
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}

	public unsafe int Serialize(byte* pData)
	{
		*(ulong*)pData = _worldStates;
		byte* num = pData + 8;
		*(ushort*)num = _awakeningXiangshuAvatars;
		byte* num2 = num + 2;
		*(ushort*)num2 = _attackingXiangshuAvatars;
		byte* num3 = num2 + 2;
		*num3 = _poisonTypes;
		byte* num4 = num3 + 1;
		*num4 = _overloadingResourceTypes;
		byte* num5 = num4 + 1;
		*num5 = _outerInjuryParts;
		byte* num6 = num5 + 1;
		*num6 = _innerInjuryParts;
		byte* num7 = num6 + 1;
		*num7 = _overloadStorageTypes;
		int num8 = (int)(num7 + 1 - pData);
		if (num8 > 4)
		{
			return (num8 + 3) / 4 * 4;
		}
		return num8;
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_worldStates = *(ulong*)ptr;
		ptr += 8;
		_awakeningXiangshuAvatars = *(ushort*)ptr;
		ptr += 2;
		_attackingXiangshuAvatars = *(ushort*)ptr;
		ptr += 2;
		_poisonTypes = *ptr;
		ptr++;
		_overloadingResourceTypes = *ptr;
		ptr++;
		_outerInjuryParts = *ptr;
		ptr++;
		_innerInjuryParts = *ptr;
		ptr++;
		_overloadStorageTypes = *ptr;
		ptr++;
		int num = (int)(ptr - pData);
		if (num > 4)
		{
			return (num + 3) / 4 * 4;
		}
		return num;
	}
}
