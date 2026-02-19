using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat;

public static class CFormulaHelper
{
	public static int CalcAttackStartupOrRecoveryFrame(this Weapon weapon, int attackSpeed, int baseFrame)
	{
		int weaponFrame = CFormula.CalcAttackStartupOrRecoveryWeaponFrame(weapon.GetWeight(), weapon.GetBaseWeight(), baseFrame);
		return CFormula.CalcAttackStartupOrRecoveryFrame(attackSpeed, weaponFrame);
	}

	public static CValuePercentBonus CalcConsummateChangeDamagePercent(CombatCharacter attacker, CombatCharacter defender)
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		GameData.Domains.Character.Character character = attacker.GetCharacter();
		sbyte b = character.GetConsummateLevel();
		GameData.Domains.Character.Character character2 = defender.GetCharacter();
		sbyte b2 = character2.GetConsummateLevel();
		if (character.IsLoseConsummateBonusByFeature())
		{
			b = 0;
		}
		if (character2.IsLoseConsummateBonusByFeature())
		{
			b2 = 0;
		}
		CValuePercentBonus val = CFormula.CalcConsummateChangeDamagePercent(b, b2);
		int num = ((b == b2) ? (-1) : ((b > b2) ? attacker.GetId() : defender.GetId()));
		return (num < 0) ? val : CValuePercentBonus.op_Implicit(DomainManager.SpecialEffect.ModifyValue(num, 297, (int)val));
	}

	public static int CalcCostChangeTrickCount(CombatCharacter combatChar, EFlawOrAcupointType flawOrAcupointType)
	{
		Weapon element_Weapons = DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(combatChar).Id);
		int num = element_Weapons.GetAttackPreparePointCost() + 1;
		int num2 = num;
		if (1 == 0)
		{
		}
		int num3 = flawOrAcupointType switch
		{
			EFlawOrAcupointType.None => 1, 
			EFlawOrAcupointType.Flaw => GlobalConfig.Instance.ChangeTrickMultiplierFlaw, 
			EFlawOrAcupointType.Acupoint => GlobalConfig.Instance.ChangeTrickMultiplierAcupoint, 
			_ => throw new ArgumentOutOfRangeException("flawOrAcupointType", flawOrAcupointType, null), 
		};
		if (1 == 0)
		{
		}
		return num2 * num3;
	}

	public static int CalcTeamWisdomCount(IReadOnlyList<int> teamCharIds)
	{
		if (teamCharIds == null || teamCharIds.Count <= 0)
		{
			return 0;
		}
		int characterWisdomCount = DomainManager.Character.GetCharacterWisdomCount(teamCharIds[0]);
		int num = 0;
		int num2 = 0;
		for (int i = 1; i < teamCharIds.Count; i++)
		{
			if (teamCharIds[i] >= 0)
			{
				num += DomainManager.Character.GetCharacterWisdomCount(teamCharIds[i]);
				num2++;
			}
		}
		characterWisdomCount *= CFormula.CalcMainCharacterWisdomMultiplier(num2);
		return characterWisdomCount + num;
	}

	public static int CalcTeamLucky(IReadOnlyList<int> teamCharIds)
	{
		if (teamCharIds == null || teamCharIds.Count <= 0)
		{
			return 0;
		}
		int num = 0;
		foreach (int teamCharId in teamCharIds)
		{
			if (teamCharId >= 0)
			{
				GameData.Domains.Character.Character element_Objects = DomainManager.Character.GetElement_Objects(teamCharId);
				num += element_Objects.GetPersonality(5);
			}
		}
		return num;
	}

	public static EPrepareCombatResult RandomPrepareResult(int selfCharId, int enemyCharId, IRandomSource random = null)
	{
		List<int> list = ObjectPool<List<int>>.Instance.Get();
		List<int> list2 = ObjectPool<List<int>>.Instance.Get();
		list.Add(selfCharId);
		list2.Add(enemyCharId);
		EPrepareCombatResult result = RandomPrepareResult(list, list2, random);
		ObjectPool<List<int>>.Instance.Return(list);
		ObjectPool<List<int>>.Instance.Return(list2);
		return result;
	}

	public static EPrepareCombatResult RandomPrepareResult(IReadOnlyList<int> selfTeam, IReadOnlyList<int> enemyTeam, IRandomSource random = null)
	{
		if (selfTeam == null || selfTeam.Count <= 0 || enemyTeam == null || enemyTeam.Count <= 0)
		{
			return EPrepareCombatResult.Invalid;
		}
		int num = Math.Abs(CalcTeamWisdomCount(selfTeam));
		int num2 = Math.Abs(CalcTeamWisdomCount(enemyTeam));
		if (num < num2)
		{
			return EPrepareCombatResult.EnemyFirst;
		}
		if (num > num2)
		{
			return EPrepareCombatResult.SelfFirst;
		}
		if (random == null)
		{
			return EPrepareCombatResult.EqualsRandom;
		}
		int num3 = CalcTeamLucky(selfTeam);
		int num4 = CalcTeamLucky(enemyTeam);
		int percentProb = 50 + num3 - num4;
		return (!random.CheckPercentProb(percentProb)) ? EPrepareCombatResult.EnemyFirst : EPrepareCombatResult.SelfFirst;
	}
}
