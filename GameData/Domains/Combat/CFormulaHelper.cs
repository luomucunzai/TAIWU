using System;
using System.Collections.Generic;
using GameData.Combat.Math;
using GameData.Domains.Character;
using GameData.Domains.Item;
using GameData.Utilities;
using Redzen.Random;

namespace GameData.Domains.Combat
{
	// Token: 0x020006C7 RID: 1735
	public static class CFormulaHelper
	{
		// Token: 0x060066E7 RID: 26343 RVA: 0x003AF0F0 File Offset: 0x003AD2F0
		public static int CalcAttackStartupOrRecoveryFrame(this Weapon weapon, int attackSpeed, int baseFrame)
		{
			int weaponFrame = CFormula.CalcAttackStartupOrRecoveryWeaponFrame(weapon.GetWeight(), weapon.GetBaseWeight(), baseFrame);
			return CFormula.CalcAttackStartupOrRecoveryFrame(attackSpeed, weaponFrame);
		}

		// Token: 0x060066E8 RID: 26344 RVA: 0x003AF11C File Offset: 0x003AD31C
		public static CValuePercentBonus CalcConsummateChangeDamagePercent(CombatCharacter attacker, CombatCharacter defender)
		{
			Character attackerChar = attacker.GetCharacter();
			sbyte attackerConsummate = attackerChar.GetConsummateLevel();
			Character defenderChar = defender.GetCharacter();
			sbyte defenderConsummate = defenderChar.GetConsummateLevel();
			bool flag = attackerChar.IsLoseConsummateBonusByFeature();
			if (flag)
			{
				attackerConsummate = 0;
			}
			bool flag2 = defenderChar.IsLoseConsummateBonusByFeature();
			if (flag2)
			{
				defenderConsummate = 0;
			}
			CValuePercentBonus bonus = CFormula.CalcConsummateChangeDamagePercent(attackerConsummate, defenderConsummate);
			int charId = (attackerConsummate == defenderConsummate) ? -1 : ((attackerConsummate > defenderConsummate) ? attacker.GetId() : defender.GetId());
			return (charId < 0) ? bonus : DomainManager.SpecialEffect.ModifyValue(charId, 297, (int)bonus, -1, -1, -1, 0, 0, 0, 0);
		}

		// Token: 0x060066E9 RID: 26345 RVA: 0x003AF1BC File Offset: 0x003AD3BC
		public static int CalcCostChangeTrickCount(CombatCharacter combatChar, EFlawOrAcupointType flawOrAcupointType)
		{
			Weapon weapon = DomainManager.Item.GetElement_Weapons(DomainManager.Combat.GetUsingWeaponKey(combatChar).Id);
			int costChangeTrickCount = (int)(weapon.GetAttackPreparePointCost() + 1);
			int num = costChangeTrickCount;
			if (!true)
			{
			}
			int num2;
			switch (flawOrAcupointType)
			{
			case EFlawOrAcupointType.None:
				num2 = 1;
				break;
			case EFlawOrAcupointType.Flaw:
				num2 = GlobalConfig.Instance.ChangeTrickMultiplierFlaw;
				break;
			case EFlawOrAcupointType.Acupoint:
				num2 = GlobalConfig.Instance.ChangeTrickMultiplierAcupoint;
				break;
			default:
				throw new ArgumentOutOfRangeException("flawOrAcupointType", flawOrAcupointType, null);
			}
			if (!true)
			{
			}
			return num * num2;
		}

		// Token: 0x060066EA RID: 26346 RVA: 0x003AF248 File Offset: 0x003AD448
		public static int CalcTeamWisdomCount(IReadOnlyList<int> teamCharIds)
		{
			bool flag = teamCharIds == null || teamCharIds.Count <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int mainValue = DomainManager.Character.GetCharacterWisdomCount(teamCharIds[0]);
				int teammateValue = 0;
				int teammateCharCount = 0;
				for (int i = 1; i < teamCharIds.Count; i++)
				{
					bool flag2 = teamCharIds[i] < 0;
					if (!flag2)
					{
						teammateValue += DomainManager.Character.GetCharacterWisdomCount(teamCharIds[i]);
						teammateCharCount++;
					}
				}
				mainValue *= CFormula.CalcMainCharacterWisdomMultiplier(teammateCharCount);
				result = mainValue + teammateValue;
			}
			return result;
		}

		// Token: 0x060066EB RID: 26347 RVA: 0x003AF2E4 File Offset: 0x003AD4E4
		public static int CalcTeamLucky(IReadOnlyList<int> teamCharIds)
		{
			bool flag = teamCharIds == null || teamCharIds.Count <= 0;
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				int total = 0;
				foreach (int charId in teamCharIds)
				{
					bool flag2 = charId < 0;
					if (!flag2)
					{
						Character character = DomainManager.Character.GetElement_Objects(charId);
						total += (int)character.GetPersonality(5);
					}
				}
				result = total;
			}
			return result;
		}

		// Token: 0x060066EC RID: 26348 RVA: 0x003AF374 File Offset: 0x003AD574
		public static EPrepareCombatResult RandomPrepareResult(int selfCharId, int enemyCharId, IRandomSource random = null)
		{
			List<int> selfTeam = ObjectPool<List<int>>.Instance.Get();
			List<int> enemyTeam = ObjectPool<List<int>>.Instance.Get();
			selfTeam.Add(selfCharId);
			enemyTeam.Add(enemyCharId);
			EPrepareCombatResult type = CFormulaHelper.RandomPrepareResult(selfTeam, enemyTeam, random);
			ObjectPool<List<int>>.Instance.Return(selfTeam);
			ObjectPool<List<int>>.Instance.Return(enemyTeam);
			return type;
		}

		// Token: 0x060066ED RID: 26349 RVA: 0x003AF3D0 File Offset: 0x003AD5D0
		public static EPrepareCombatResult RandomPrepareResult(IReadOnlyList<int> selfTeam, IReadOnlyList<int> enemyTeam, IRandomSource random = null)
		{
			bool flag = selfTeam == null || selfTeam.Count <= 0 || (enemyTeam == null || enemyTeam.Count <= 0);
			EPrepareCombatResult result;
			if (flag)
			{
				result = EPrepareCombatResult.Invalid;
			}
			else
			{
				int selfWisdom = Math.Abs(CFormulaHelper.CalcTeamWisdomCount(selfTeam));
				int enemyWisdom = Math.Abs(CFormulaHelper.CalcTeamWisdomCount(enemyTeam));
				bool flag2 = selfWisdom < enemyWisdom;
				if (flag2)
				{
					result = EPrepareCombatResult.EnemyFirst;
				}
				else
				{
					bool flag3 = selfWisdom > enemyWisdom;
					if (flag3)
					{
						result = EPrepareCombatResult.SelfFirst;
					}
					else
					{
						bool flag4 = random == null;
						if (flag4)
						{
							result = EPrepareCombatResult.EqualsRandom;
						}
						else
						{
							int selfLuck = CFormulaHelper.CalcTeamLucky(selfTeam);
							int enemyLuck = CFormulaHelper.CalcTeamLucky(enemyTeam);
							int selfProb = 50 + selfLuck - enemyLuck;
							result = (random.CheckPercentProb(selfProb) ? EPrepareCombatResult.SelfFirst : EPrepareCombatResult.EnemyFirst);
						}
					}
				}
			}
			return result;
		}
	}
}
