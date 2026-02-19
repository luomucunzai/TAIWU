using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.SpecialEffect;

[SerializableGameData(NotForArchive = true)]
public struct CastBoostEffectDisplayData : ISerializableGameData
{
	[SerializableGameDataField]
	private sbyte _internalType;

	[SerializableGameDataField]
	private int _internalParam0;

	[SerializableGameDataField]
	private int _internalParam1;

	[SerializableGameDataField]
	public CombatSkillEffectDescriptionDisplayData EffectDescription;

	public ECastBoostType Type => (ECastBoostType)_internalType;

	public int EffectId => EffectDescription.EffectId;

	public byte NeiliAllocationType => (byte)((Type == ECastBoostType.CostNeiliAllocation) ? ((uint)_internalParam0) : 4u);

	public int NeiliAllocationValue => (Type == ECastBoostType.CostNeiliAllocation) ? _internalParam1 : (-1);

	public short WugMedicineTemplateId => (short)((Type == ECastBoostType.CostWugKing) ? _internalParam0 : (-1));

	public int WugKingCount => (Type == ECastBoostType.CostWugKing) ? _internalParam1 : 0;

	private CastBoostEffectDisplayData(CombatSkillKey skillKey)
	{
		_internalType = -1;
		_internalParam0 = (_internalParam1 = 0);
		EffectDescription = DomainManager.CombatSkill.GetEffectDisplayData(skillKey);
	}

	public static CastBoostEffectDisplayData CostNeiliAllocation(CombatSkillKey skillKey, byte type, int value)
	{
		CastBoostEffectDisplayData result = new CastBoostEffectDisplayData(skillKey);
		result._internalType = 0;
		result._internalParam0 = type;
		result._internalParam1 = value;
		return result;
	}

	public static CastBoostEffectDisplayData CostWugKing(CombatSkillKey skillKey, short wugTemplateId, int count)
	{
		CastBoostEffectDisplayData result = new CastBoostEffectDisplayData(skillKey);
		result._internalType = 1;
		result._internalParam0 = wugTemplateId;
		result._internalParam1 = count;
		return result;
	}

	public CastBoostEffectDisplayData(CastBoostEffectDisplayData other)
	{
		_internalType = other._internalType;
		_internalParam0 = other._internalParam0;
		_internalParam1 = other._internalParam1;
		EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
	}

	public void Assign(CastBoostEffectDisplayData other)
	{
		_internalType = other._internalType;
		_internalParam0 = other._internalParam0;
		_internalParam1 = other._internalParam1;
		EffectDescription = new CombatSkillEffectDescriptionDisplayData(other.EffectDescription);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 9;
		num += EffectDescription.GetSerializedSize();
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)_internalType;
		ptr++;
		*(int*)ptr = _internalParam0;
		ptr += 4;
		*(int*)ptr = _internalParam1;
		ptr += 4;
		int num = EffectDescription.Serialize(ptr);
		ptr += num;
		Tester.Assert(num <= 65535);
		int num2 = (int)(ptr - pData);
		return (num2 <= 4) ? num2 : ((num2 + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_internalType = (sbyte)(*ptr);
		ptr++;
		_internalParam0 = *(int*)ptr;
		ptr += 4;
		_internalParam1 = *(int*)ptr;
		ptr += 4;
		ptr += EffectDescription.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}
}
