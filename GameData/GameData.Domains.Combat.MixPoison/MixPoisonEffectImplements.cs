using System;
using System.Collections.Generic;
using GameData.Common;
using GameData.Domains.Character;
using GameData.Domains.CombatSkill;
using GameData.Domains.Item;
using GameData.Utilities;

namespace GameData.Domains.Combat.MixPoison;

public static class MixPoisonEffectImplements
{
	[MixPoisonEffect(12)]
	public static bool MixPoisonEffect012(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int delta = 300 * (poisonMarkList[0] + poisonMarkList[1] + poisonMarkList[2]);
		DomainManager.Combat.ChangeDisorderOfQiRandomRecovery(context, combatChar, delta);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1654, 0);
		return true;
	}

	[MixPoisonEffect(8)]
	public static bool MixPoisonEffect013(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = (poisonMarkList[0] + poisonMarkList[1] + poisonMarkList[3]) * 5 - 5;
		DomainManager.Combat.ChangeMobilityValue(context, combatChar, -MoveSpecialConstants.MaxMobility * num / 100, changedByEffect: true, combatChar);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1650, 0);
		return true;
	}

	[MixPoisonEffect(2)]
	public static bool MixPoisonEffect014(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int percentProb = poisonMarkList[4] * 20;
		if (!context.Random.CheckPercentProb(percentProb))
		{
			return false;
		}
		if (!combatChar.ChangeToEmptyHandOrOther(context))
		{
			return false;
		}
		ItemKey[] weapons = combatChar.GetWeapons();
		for (int i = 0; i < 7; i++)
		{
			if (i != combatChar.GetUsingWeaponIndex() && weapons[i].IsValid())
			{
				combatChar.GetWeaponData(i).SetCdFrame(30000, context);
			}
		}
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1644, 0);
		return true;
	}

	[MixPoisonEffect(18)]
	public static bool MixPoisonEffect015(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		NeiliAllocation neiliAllocation = combatChar.GetNeiliAllocation();
		int addValue = -10 * poisonMarkList[5];
		List<byte> list = ObjectPool<List<byte>>.Instance.Get();
		bool result = false;
		list.Clear();
		for (byte b = 0; b < 4; b++)
		{
			if (neiliAllocation[b] > 0)
			{
				list.Add(b);
			}
		}
		if (list.Count > 0)
		{
			combatChar.ChangeNeiliAllocation(context, list[context.Random.Next(0, list.Count)], addValue);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1660, 0);
			result = true;
		}
		ObjectPool<List<byte>>.Instance.Return(list);
		return result;
	}

	[MixPoisonEffect(9)]
	public static bool MixPoisonEffect023(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		Injuries injuries = combatChar.GetInjuries();
		int num = poisonMarkList[0] + poisonMarkList[2] + poisonMarkList[3];
		int percentProb = 20 * num;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (injuries.Get(b, isInnerInjury: false) < 6)
			{
				list.Add(b);
			}
		}
		if (list.Count == 0)
		{
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				list.Add(b2);
			}
		}
		sbyte random = list.GetRandom(context.Random);
		if (context.Random.CheckPercentProb(percentProb) && injuries.Get(random, isInnerInjury: false) < 6)
		{
			DomainManager.Combat.AddInjury(context, combatChar, random, isInner: false, 1, updateDefeatMark: true, changeToOld: true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1651, 0);
		}
		else
		{
			DomainManager.Combat.AddFlaw(context, combatChar, (sbyte)Math.Max(num / 3 - 1, 0), new CombatSkillKey(-1, -1), random);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1651, 1);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return true;
	}

	[MixPoisonEffect(3)]
	public static bool MixPoisonEffect024(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = poisonMarkList[4];
		DomainManager.Combat.PoisonProduce(context, combatChar, 0, num);
		combatChar.AddPoisonAffectValue(3, (short)(20 * num), needLessThanThreshold: true);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1645, 0);
		return true;
	}

	[MixPoisonEffect(15)]
	public static bool MixPoisonEffect025(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = poisonMarkList[5];
		DomainManager.Combat.PoisonProduce(context, combatChar, 5, num);
		combatChar.AddPoisonAffectValue(1, (short)(num * 5 + 5), needLessThanThreshold: true);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1657, 0);
		return true;
	}

	[MixPoisonEffect(0)]
	public static bool MixPoisonEffect034(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		DomainManager.Combat.AppendFatalDamageMark(context, combatChar, poisonMarkList[4], -1, -1);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1642, 0);
		return true;
	}

	[MixPoisonEffect(7)]
	public static bool MixPoisonEffect035(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		for (int i = 0; i < poisonMarkList[5]; i++)
		{
			DomainManager.Combat.AddWeaponAttackSelfInjury(context, combatChar, combatChar.GetUsingWeaponIndex());
		}
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1649, 0);
		return true;
	}

	[MixPoisonEffect(1)]
	public static bool MixPoisonEffect045(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		short randomBanableSkillId = combatChar.GetRandomBanableSkillId(context.Random, null, 2);
		if (randomBanableSkillId < 0)
		{
			return false;
		}
		DomainManager.Combat.ClearAffectingAgileSkillByEffect(context, combatChar);
		DomainManager.Combat.SilenceSkill(context, combatChar, randomBanableSkillId, (short)(600 * (poisonMarkList[4] + poisonMarkList[5])));
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1643, 0);
		return true;
	}

	[MixPoisonEffect(13)]
	public static bool MixPoisonEffect123(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		Injuries injuries = combatChar.GetInjuries();
		int num = poisonMarkList[1] + poisonMarkList[2] + poisonMarkList[3];
		int percentProb = 20 * num;
		List<sbyte> list = ObjectPool<List<sbyte>>.Instance.Get();
		list.Clear();
		for (sbyte b = 0; b < 7; b++)
		{
			if (injuries.Get(b, isInnerInjury: true) < 6)
			{
				list.Add(b);
			}
		}
		if (list.Count == 0)
		{
			for (sbyte b2 = 0; b2 < 7; b2++)
			{
				list.Add(b2);
			}
		}
		sbyte random = list.GetRandom(context.Random);
		if (context.Random.CheckPercentProb(percentProb) && injuries.Get(random, isInnerInjury: false) < 6)
		{
			DomainManager.Combat.AddInjury(context, combatChar, random, isInner: true, 1, updateDefeatMark: true, changeToOld: true);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1655, 0);
		}
		else
		{
			DomainManager.Combat.AddAcupoint(context, combatChar, (sbyte)Math.Max(num / 3 - 1, 0), new CombatSkillKey(-1, -1), random);
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1655, 1);
		}
		ObjectPool<List<sbyte>>.Instance.Return(list);
		return true;
	}

	[MixPoisonEffect(11)]
	public static bool MixPoisonEffect124(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		List<short> list = ObjectPool<List<short>>.Instance.Get();
		list.Clear();
		list.AddRange(combatChar.GetAttackSkillList());
		list.RemoveAll((short id) => id < 0);
		if (list.Count > 0)
		{
			for (int num = 0; num < poisonMarkList[4]; num++)
			{
				DomainManager.Combat.AddGoneMadInjury(context, combatChar, list.GetRandom(context.Random));
			}
			DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1653, 0);
		}
		ObjectPool<List<short>>.Instance.Return(list);
		return true;
	}

	[MixPoisonEffect(10)]
	public static bool MixPoisonEffect125(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		DomainManager.Combat.AppendMindDefeatMark(context, combatChar, poisonMarkList[5] * 2, -1);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1652, 0);
		return true;
	}

	[MixPoisonEffect(5)]
	public static bool MixPoisonEffect134(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = poisonMarkList[4];
		DomainManager.Combat.PoisonProduce(context, combatChar, 4, num);
		combatChar.AddPoisonAffectValue(0, (short)(num + 1), needLessThanThreshold: true);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1647, 0);
		return true;
	}

	[MixPoisonEffect(19)]
	public static bool MixPoisonEffect135(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = poisonMarkList[5];
		DomainManager.Combat.PoisonProduce(context, combatChar, 1, num);
		combatChar.AddPoisonAffectValue(2, (short)(20 * num), needLessThanThreshold: true);
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1661, 0);
		return true;
	}

	[MixPoisonEffect(17)]
	public static bool MixPoisonEffect145(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		short randomBanableSkillId = combatChar.GetRandomBanableSkillId(context.Random, null, 3);
		if (randomBanableSkillId < 0)
		{
			return false;
		}
		DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
		DomainManager.Combat.SilenceSkill(context, combatChar, randomBanableSkillId, (short)(600 * (poisonMarkList[4] + poisonMarkList[5])));
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1659, 0);
		return true;
	}

	[MixPoisonEffect(6)]
	public static bool MixPoisonEffect234(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = combatChar.GetPoison()[4];
		sbyte level = PoisonsAndLevels.CalcPoisonedLevel(num);
		int addValue = num * 10 * poisonMarkList[4] / 100;
		int[] array = (combatChar.IsAlly ? DomainManager.Combat.GetSelfTeam() : DomainManager.Combat.GetEnemyTeam());
		DomainManager.Combat.AddCombatState(context, combatChar, 2, 146, 100 * poisonMarkList[4]);
		foreach (int num2 in array)
		{
			if (num2 >= 0 && num2 != combatChar.GetId())
			{
				DomainManager.Combat.AddPoison(context, combatChar, DomainManager.Combat.GetElement_CombatCharacterDict(num2), 4, level, addValue, -1, applySpecialEffect: true, canBounce: false);
			}
		}
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1648, 0);
		return true;
	}

	[MixPoisonEffect(16)]
	public static bool MixPoisonEffect235(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		int num = combatChar.GetPoison()[5];
		sbyte level = PoisonsAndLevels.CalcPoisonedLevel(num);
		int addValue = num * 10 * poisonMarkList[5] / 100;
		int[] array = (combatChar.IsAlly ? DomainManager.Combat.GetSelfTeam() : DomainManager.Combat.GetEnemyTeam());
		DomainManager.Combat.AddCombatState(context, combatChar, 2, 147, 100 * poisonMarkList[5]);
		foreach (int num2 in array)
		{
			if (num2 >= 0 && num2 != combatChar.GetId())
			{
				DomainManager.Combat.AddPoison(context, combatChar, DomainManager.Combat.GetElement_CombatCharacterDict(num2), 5, level, addValue, -1, applySpecialEffect: true, canBounce: false);
			}
		}
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1658, 0);
		return true;
	}

	[MixPoisonEffect(14)]
	public static bool MixPoisonEffect245(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		short randomBanableSkillId = combatChar.GetRandomBanableSkillId(context.Random, null, 4);
		if (randomBanableSkillId < 0)
		{
			return false;
		}
		DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
		DomainManager.Combat.SilenceSkill(context, combatChar, randomBanableSkillId, (short)(600 * (poisonMarkList[4] + poisonMarkList[5])));
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1656, 0);
		return true;
	}

	[MixPoisonEffect(4)]
	public static bool MixPoisonEffect345(DataContext context, CombatCharacter combatChar, byte[] poisonMarkList)
	{
		short randomBanableSkillId = combatChar.GetRandomBanableSkillId(context.Random, null, 1);
		if (randomBanableSkillId < 0)
		{
			return false;
		}
		DomainManager.Combat.ClearAffectingDefenseSkill(context, combatChar);
		DomainManager.Combat.SilenceSkill(context, combatChar, randomBanableSkillId, (short)(600 * (poisonMarkList[4] + poisonMarkList[5])));
		DomainManager.Combat.ShowSpecialEffectTips(combatChar.GetId(), 1646, 0);
		return true;
	}
}
