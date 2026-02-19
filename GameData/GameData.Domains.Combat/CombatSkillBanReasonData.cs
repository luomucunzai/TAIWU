using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Serializer;
using GameData.Utilities;

namespace GameData.Domains.Combat;

[SerializableGameData(NotForArchive = true)]
public struct CombatSkillBanReasonData : ISerializableGameData
{
	[SerializableGameDataField]
	private sbyte _internalType;

	[SerializableGameDataField]
	private sbyte _internalParam0;

	[SerializableGameDataField]
	private sbyte _internalParam1;

	[SerializableGameDataField]
	private sbyte _internalParam2;

	[SerializableGameDataField]
	public List<NeedTrick> CostTricks;

	[SerializableGameDataField]
	public List<NeedTrick> HasTricks;

	public ECombatSkillBanReasonType Type => (ECombatSkillBanReasonType)_internalType;

	public int CostMobility => (Type == ECombatSkillBanReasonType.MobilityNotEnough) ? _internalParam0 : 0;

	public int HasMobility => (Type == ECombatSkillBanReasonType.MobilityNotEnough) ? _internalParam1 : 0;

	public int CostBreath => (Type == ECombatSkillBanReasonType.BreathNotEnough) ? _internalParam0 : 0;

	public int HasBreath => (Type == ECombatSkillBanReasonType.BreathNotEnough) ? _internalParam1 : 0;

	public int CostStance => (Type == ECombatSkillBanReasonType.StanceNotEnough) ? _internalParam0 : 0;

	public int HasStance => (Type == ECombatSkillBanReasonType.StanceNotEnough) ? _internalParam1 : 0;

	public int CostWug => (Type == ECombatSkillBanReasonType.WugNotEnough) ? _internalParam0 : 0;

	public int HasWug => (Type == ECombatSkillBanReasonType.WugNotEnough) ? _internalParam1 : 0;

	public int CostNeiliAllocationType => (Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? _internalParam0 : (-1);

	public int CostNeiliAllocationValue => (Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? _internalParam1 : 0;

	public int HasNeiliAllocationValue => (Type == ECombatSkillBanReasonType.NeiliAllocationNotEnough) ? _internalParam2 : 0;

	public CombatSkillBanReasonData(ECombatSkillBanReasonType type, short templateId, GameData.Domains.CombatSkill.CombatSkill combatSkill, CombatCharacter combatChar)
	{
		_internalType = (sbyte)type;
		_internalParam0 = (_internalParam1 = (_internalParam2 = 0));
		CostTricks = (HasTricks = null);
		CombatSkillItem combatSkillItem = Config.CombatSkill.Instance[templateId];
		switch (type)
		{
		case ECombatSkillBanReasonType.None:
		case ECombatSkillBanReasonType.Undefined:
			break;
		case ECombatSkillBanReasonType.StanceNotEnough:
			_internalParam0 = (sbyte)DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), combatSkill).Outer;
			_internalParam1 = (sbyte)Math.Clamp(combatChar.GetStanceValue() * 100 / 4000, 0, 100);
			break;
		case ECombatSkillBanReasonType.BreathNotEnough:
			_internalParam0 = (sbyte)DomainManager.Combat.GetSkillCostBreathStance(combatChar.GetId(), combatSkill).Inner;
			_internalParam1 = (sbyte)Math.Clamp(combatChar.GetBreathValue() * 100 / 30000, 0, 100);
			break;
		case ECombatSkillBanReasonType.MobilityNotEnough:
			_internalParam0 = combatSkill.GetCostMobilityPercent();
			_internalParam1 = (sbyte)Math.Clamp(combatChar.GetMobilityValue() * 100 / MoveSpecialConstants.MaxMobility, 0, 100);
			break;
		case ECombatSkillBanReasonType.TrickNotEnough:
			CostTricks = new List<NeedTrick>();
			HasTricks = new List<NeedTrick>();
			DomainManager.CombatSkill.GetCombatSkillCostTrick(combatSkill, CostTricks);
			{
				foreach (NeedTrick costTrick in CostTricks)
				{
					NeedTrick needTrick = costTrick;
					needTrick.NeedCount = combatChar.GetTrickCount(costTrick.TrickType);
					NeedTrick item = needTrick;
					HasTricks.Add(item);
				}
				break;
			}
		case ECombatSkillBanReasonType.WugNotEnough:
			_internalParam0 = combatSkillItem.WugCost;
			_internalParam1 = (sbyte)Math.Clamp((int)combatChar.GetWugCount(), 0, 127);
			break;
		case ECombatSkillBanReasonType.NeiliAllocationNotEnough:
			SetNeiliAllocationParam(combatSkill, combatChar);
			break;
		case ECombatSkillBanReasonType.WeaponTrickMismatch:
		case ECombatSkillBanReasonType.WeaponDestroyed:
		case ECombatSkillBanReasonType.BodyPartBroken:
		case ECombatSkillBanReasonType.SpecialEffectBan:
		case ECombatSkillBanReasonType.CombatConfigBan:
		case ECombatSkillBanReasonType.Silencing:
			break;
		default:
			throw new ArgumentOutOfRangeException("type", type, null);
		}
	}

	private unsafe void SetNeiliAllocationParam(GameData.Domains.CombatSkill.CombatSkill combatSkill, CombatCharacter combatChar)
	{
		(sbyte, sbyte) costNeiliAllocation = combatSkill.GetCostNeiliAllocation();
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		(_internalParam0, _) = costNeiliAllocation;
		if (_internalParam0 >= 0)
		{
			_internalParam1 = costNeiliAllocation.Item2;
			_internalParam2 = (sbyte)Math.Clamp((int)neiliAllocation.Items[_internalParam0], 0, 127);
		}
	}

	public CombatSkillBanReasonData(CombatSkillBanReasonData other)
	{
		_internalType = other._internalType;
		_internalParam0 = other._internalParam0;
		_internalParam1 = other._internalParam1;
		_internalParam2 = other._internalParam2;
		CostTricks = new List<NeedTrick>(other.CostTricks);
		HasTricks = new List<NeedTrick>(other.HasTricks);
	}

	public void Assign(CombatSkillBanReasonData other)
	{
		_internalType = other._internalType;
		_internalParam0 = other._internalParam0;
		_internalParam1 = other._internalParam1;
		_internalParam2 = other._internalParam2;
		CostTricks = new List<NeedTrick>(other.CostTricks);
		HasTricks = new List<NeedTrick>(other.HasTricks);
	}

	public bool IsSerializedSizeFixed()
	{
		return false;
	}

	public int GetSerializedSize()
	{
		int num = 4;
		num = ((CostTricks == null) ? (num + 2) : (num + (2 + 4 * CostTricks.Count)));
		num = ((HasTricks == null) ? (num + 2) : (num + (2 + 4 * HasTricks.Count)));
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Serialize(byte* pData)
	{
		byte* ptr = pData;
		*ptr = (byte)_internalType;
		ptr++;
		*ptr = (byte)_internalParam0;
		ptr++;
		*ptr = (byte)_internalParam1;
		ptr++;
		*ptr = (byte)_internalParam2;
		ptr++;
		if (CostTricks != null)
		{
			int count = CostTricks.Count;
			Tester.Assert(count <= 65535);
			*(ushort*)ptr = (ushort)count;
			ptr += 2;
			for (int i = 0; i < count; i++)
			{
				ptr += CostTricks[i].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		if (HasTricks != null)
		{
			int count2 = HasTricks.Count;
			Tester.Assert(count2 <= 65535);
			*(ushort*)ptr = (ushort)count2;
			ptr += 2;
			for (int j = 0; j < count2; j++)
			{
				ptr += HasTricks[j].Serialize(ptr);
			}
		}
		else
		{
			*(short*)ptr = 0;
			ptr += 2;
		}
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		_internalType = (sbyte)(*ptr);
		ptr++;
		_internalParam0 = (sbyte)(*ptr);
		ptr++;
		_internalParam1 = (sbyte)(*ptr);
		ptr++;
		_internalParam2 = (sbyte)(*ptr);
		ptr++;
		ushort num = *(ushort*)ptr;
		ptr += 2;
		if (num > 0)
		{
			if (CostTricks == null)
			{
				CostTricks = new List<NeedTrick>(num);
			}
			else
			{
				CostTricks.Clear();
			}
			for (int i = 0; i < num; i++)
			{
				NeedTrick item = default(NeedTrick);
				ptr += item.Deserialize(ptr);
				CostTricks.Add(item);
			}
		}
		else
		{
			CostTricks?.Clear();
		}
		ushort num2 = *(ushort*)ptr;
		ptr += 2;
		if (num2 > 0)
		{
			if (HasTricks == null)
			{
				HasTricks = new List<NeedTrick>(num2);
			}
			else
			{
				HasTricks.Clear();
			}
			for (int j = 0; j < num2; j++)
			{
				NeedTrick item2 = default(NeedTrick);
				ptr += item2.Deserialize(ptr);
				HasTricks.Add(item2);
			}
		}
		else
		{
			HasTricks?.Clear();
		}
		int num3 = (int)(ptr - pData);
		return (num3 <= 4) ? num3 : ((num3 + 3) / 4 * 4);
	}
}
