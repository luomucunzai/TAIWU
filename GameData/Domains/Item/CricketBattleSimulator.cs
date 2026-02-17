using System;
using GameData.Common;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.Item
{
	// Token: 0x02000664 RID: 1636
	public static class CricketBattleSimulator
	{
		// Token: 0x06004F7C RID: 20348 RVA: 0x002B4866 File Offset: 0x002B2A66
		public static void SetBattlers(ItemKey battlerAItemKey, ItemKey battlerBItemKey, DataContext context)
		{
			CricketBattleSimulator._dataContext = context;
			CricketBattleSimulator._cricketBattlerA = new CricketBattler(battlerAItemKey);
			CricketBattleSimulator._cricketBattlerB = new CricketBattler(battlerBItemKey);
		}

		// Token: 0x06004F7D RID: 20349 RVA: 0x002B4888 File Offset: 0x002B2A88
		private static bool CheckPercentProb(int percentProb)
		{
			return CricketBattleSimulator._dataContext.Random.CheckPercentProb(percentProb);
		}

		// Token: 0x06004F7E RID: 20350 RVA: 0x002B48AC File Offset: 0x002B2AAC
		public static int GetBattleResult()
		{
			int resultCode = CricketBattleSimulator.CheckWinBeforeFight();
			bool flag = resultCode >= 0;
			int result;
			if (flag)
			{
				result = resultCode;
			}
			else
			{
				while (!CricketBattleSimulator._cricketBattlerA.IsFail && !CricketBattleSimulator._cricketBattlerB.IsFail)
				{
					int vigorDiff = CricketBattleSimulator._cricketBattlerA.Vigor - CricketBattleSimulator._cricketBattlerB.Vigor;
					bool flag2 = vigorDiff > 0;
					bool battlerAFirstAttack;
					if (flag2)
					{
						CricketBattleSimulator._cricketBattlerB.SP -= CricketBattleSimulator._cricketBattlerA.Vigor;
						battlerAFirstAttack = CricketBattleSimulator.CheckPercentProb(80);
					}
					else
					{
						bool flag3 = vigorDiff < 0;
						if (flag3)
						{
							CricketBattleSimulator._cricketBattlerA.SP -= CricketBattleSimulator._cricketBattlerB.Vigor;
							battlerAFirstAttack = CricketBattleSimulator.CheckPercentProb(20);
						}
						else
						{
							battlerAFirstAttack = CricketBattleSimulator.CheckPercentProb(50);
						}
					}
					bool flag4 = vigorDiff == 0 || ((vigorDiff > 0) ? (!CricketBattleSimulator._cricketBattlerB.IsFail) : (!CricketBattleSimulator._cricketBattlerA.IsFail));
					if (flag4)
					{
						CricketBattleSimulator.DoNormalAttack(battlerAFirstAttack, true);
					}
				}
				resultCode = (CricketBattleSimulator._cricketBattlerA.IsFail ? 1 : 0);
				CricketBattleSimulator._cricketBattlerA = null;
				CricketBattleSimulator._cricketBattlerB = null;
				CricketBattleSimulator._dataContext = null;
				result = resultCode;
			}
			return result;
		}

		// Token: 0x06004F7F RID: 20351 RVA: 0x002B49E4 File Offset: 0x002B2BE4
		private static int CheckWinBeforeFight()
		{
			int selfLevel = CricketBattleSimulator._cricketBattlerA.Level;
			int enemyLevel = CricketBattleSimulator._cricketBattlerB.Level;
			bool flag = (!CricketBattleSimulator._cricketBattlerA.IsTrash && CricketBattleSimulator._cricketBattlerB.IsTrash) || (selfLevel - enemyLevel >= 6 && CricketBattleSimulator.CheckPercentProb((selfLevel - enemyLevel) * 10));
			int result;
			if (flag)
			{
				result = 0;
			}
			else
			{
				bool flag2 = (!CricketBattleSimulator._cricketBattlerB.IsTrash && CricketBattleSimulator._cricketBattlerA.IsTrash) || (enemyLevel - selfLevel >= 6 && CricketBattleSimulator.CheckPercentProb((enemyLevel - selfLevel) * 10));
				if (flag2)
				{
					result = 1;
				}
				else
				{
					bool flag3 = CricketBattleSimulator._cricketBattlerA.IsTrash && CricketBattleSimulator._cricketBattlerB.IsTrash;
					if (flag3)
					{
						result = (CricketBattleSimulator.CheckPercentProb(50) ? 0 : 1);
					}
					else
					{
						result = -1;
					}
				}
			}
			return result;
		}

		// Token: 0x06004F80 RID: 20352 RVA: 0x002B4AAC File Offset: 0x002B2CAC
		private static void DoNormalAttack(bool battlerAFirstAttack, bool firstAttack = true)
		{
			CricketBattler attacker = battlerAFirstAttack ? CricketBattleSimulator._cricketBattlerA : CricketBattleSimulator._cricketBattlerB;
			CricketBattler defender = battlerAFirstAttack ? CricketBattleSimulator._cricketBattlerB : CricketBattleSimulator._cricketBattlerA;
			bool critical = CricketBattleSimulator.CheckPercentProb(attacker.Deadliness);
			bool defend = CricketBattleSimulator.CheckPercentProb(defender.Defence);
			bool counter = CricketBattleSimulator.CheckPercentProb(defender.Counter);
			int damage = attacker.Bite + (critical ? attacker.Damage : 0);
			bool flag = defend;
			if (flag)
			{
				damage = (int)MathF.Max((float)(damage - defender.DamageReduce), 0f);
			}
			CricketBattleSimulator.SettleNormalAttackDamage(attacker, defender, battlerAFirstAttack, damage, critical, defend, counter, false, firstAttack, 0);
		}

		// Token: 0x06004F81 RID: 20353 RVA: 0x002B4B44 File Offset: 0x002B2D44
		private static void SettleNormalAttackDamage(CricketBattler attacker, CricketBattler defender, bool battlerAFirstAttack, int damage, bool critical, bool defend, bool canCounter, bool isCounterAttack, bool firstAttack, int counterTimes)
		{
			int spDamage = (critical || isCounterAttack) ? attacker.Vigor : 0;
			if (defend)
			{
				spDamage = (int)MathF.Max((float)(spDamage - defender.DamageReduce), 0f);
			}
			else if (critical)
			{
				defender.Durability--;
				int injuryOdds = attacker.Deadliness + attacker.Cripple;
				bool flag = CricketBattleSimulator.CheckPercentProb(injuryOdds);
				if (flag)
				{
					bool flag2 = CricketBattleSimulator.CheckPercentProb(35);
					int index;
					short value;
					if (flag2)
					{
						index = EventHelper.GetRandom(2, 5);
						value = 1;
					}
					else
					{
						index = EventHelper.GetRandom(0, 2);
						value = 5;
					}
					short[] injuries = defender.CricketData.Injuries;
					int num = index;
					injuries[num] += value;
				}
			}
			defender.HP = (int)MathF.Min((float)((int)MathF.Max((float)(defender.HP - damage), 0f)), (float)defender.MaxHP);
			defender.SP = (int)MathF.Min((float)((int)MathF.Max((float)(defender.SP - spDamage), 0f)), (float)defender.MaxSP);
			bool isFail = defender.IsFail;
			if (!isFail)
			{
				bool flag3 = canCounter;
				if (flag3)
				{
					bool counterCritical = CricketBattleSimulator.CheckPercentProb(defender.Deadliness);
					bool counterDefend = CricketBattleSimulator.CheckPercentProb(attacker.Defence);
					int counterDamage = ((counterTimes % 2 == 0) ? defender.Strength : defender.Bite) + (critical ? defender.Damage : 0);
					canCounter = CricketBattleSimulator.CheckPercentProb(attacker.Counter - counterTimes * 5);
					bool flag4 = counterDefend;
					if (flag4)
					{
						counterDamage = (int)MathF.Max((float)(counterDamage - attacker.DamageReduce), 0f);
					}
					CricketBattleSimulator.SettleNormalAttackDamage(defender, attacker, !battlerAFirstAttack, counterDamage, counterCritical, counterDefend, canCounter, true, firstAttack, ++counterTimes);
				}
				else
				{
					bool originSelfAttack = (counterTimes % 2 == 0) ? battlerAFirstAttack : (!battlerAFirstAttack);
					if (firstAttack)
					{
						CricketBattleSimulator.DoNormalAttack(!originSelfAttack, false);
					}
				}
			}
		}

		// Token: 0x040015A7 RID: 5543
		private static CricketBattler _cricketBattlerA;

		// Token: 0x040015A8 RID: 5544
		private static CricketBattler _cricketBattlerB;

		// Token: 0x040015A9 RID: 5545
		private static DataContext _dataContext;
	}
}
