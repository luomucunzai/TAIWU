using GameData.Domains.Item;
using GameData.Serializer;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct CombatReserveData : ISerializableGameData
{
	[SerializableGameDataField]
	private sbyte _internalType;

	[SerializableGameDataField]
	private long _internalValue0;

	public static CombatReserveData Invalid => new CombatReserveData
	{
		_internalType = 0
	};

	public ECombatReserveType Type => (ECombatReserveType)_internalType;

	public bool AnyReserve => Type != ECombatReserveType.Invalid;

	public short NeedUseSkillId => (short)((Type == ECombatReserveType.Skill) ? ((short)_internalValue0) : (-1));

	public bool NeedShowChangeTrick => Type == ECombatReserveType.ChangeTrick;

	public int NeedChangeWeaponIndex => (int)((Type == ECombatReserveType.ChangeWeapon) ? _internalValue0 : (-1));

	public int NeedUnlockWeaponIndex => (int)((Type == ECombatReserveType.UnlockAttack) ? _internalValue0 : (-1));

	public ItemKey NeedUseItem => (Type == ECombatReserveType.UseItem) ? ((ItemKey)(ulong)_internalValue0) : ItemKey.Invalid;

	public sbyte NeedUseOtherAction => (sbyte)((Type == ECombatReserveType.OtherAction) ? ((sbyte)_internalValue0) : (-1));

	public int TeammateCharId => (int)((Type == ECombatReserveType.TeammateCommand) ? (_internalValue0 >> 32) : (-1));

	public int TeammateCmdIndex => (int)((Type == ECombatReserveType.TeammateCommand) ? _internalValue0 : (-1));

	public static CombatReserveData CreateSkill(short skillId)
	{
		if (skillId < 0)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 1,
			_internalValue0 = skillId
		};
	}

	public static CombatReserveData CreateChangeTrick(bool valid)
	{
		if (!valid)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 2
		};
	}

	public static CombatReserveData CreateChangeWeapon(int weaponIndex)
	{
		if (weaponIndex < 0)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 3,
			_internalValue0 = weaponIndex
		};
	}

	public static CombatReserveData CreateUnlockAttack(int weaponIndex)
	{
		if (weaponIndex < 0)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 4,
			_internalValue0 = weaponIndex
		};
	}

	public static CombatReserveData CreateUseItem(ItemKey itemKey)
	{
		if (!itemKey.IsValid())
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 5,
			_internalValue0 = (long)(ulong)itemKey
		};
	}

	public static CombatReserveData CreateOtherAction(sbyte otherActionType)
	{
		if (otherActionType < 0)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 6,
			_internalValue0 = otherActionType
		};
	}

	public static CombatReserveData CreateTeammateCommand(int teammateCharId, int teammateCmdIndex)
	{
		if (teammateCharId < 0 || teammateCmdIndex < 0)
		{
			return Invalid;
		}
		return new CombatReserveData
		{
			_internalType = 7,
			_internalValue0 = (((long)teammateCharId << 32) | teammateCmdIndex)
		};
	}

	public CombatReserveData(CombatReserveData other)
	{
		_internalType = other._internalType;
		_internalValue0 = other._internalValue0;
	}

	public bool IsSerializedSizeFixed()
	{
		return true;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)_internalType;
		ptr++;
		*(long*)ptr = _internalValue0;
		ptr += 8;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_internalType = (sbyte)(*ptr);
		ptr++;
		_internalValue0 = *(long*)ptr;
		ptr += 8;
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
