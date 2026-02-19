using System;
using GameData.Common;
using GameData.Domains.TaiwuEvent.EventHelper;
using GameData.Utilities;

namespace GameData.Domains.Item;

public static class CricketBattleSimulator
{
	private static CricketBattler _cricketBattlerA;

	private static CricketBattler _cricketBattlerB;

	private static DataContext _dataContext;

	public static void SetBattlers(ItemKey battlerAItemKey, ItemKey battlerBItemKey, DataContext context)
	{
		_dataContext = context;
		_cricketBattlerA = new CricketBattler(battlerAItemKey);
		_cricketBattlerB = new CricketBattler(battlerBItemKey);
	}

	private static bool CheckPercentProb(int percentProb)
	{
		return _dataContext.Random.CheckPercentProb(percentProb);
	}

	public static int GetBattleResult()
	{
		int num = CheckWinBeforeFight();
		if (num >= 0)
		{
			return num;
		}
		while (!_cricketBattlerA.IsFail && !_cricketBattlerB.IsFail)
		{
			int num2 = _cricketBattlerA.Vigor - _cricketBattlerB.Vigor;
			bool battlerAFirstAttack;
			if (num2 > 0)
			{
				_cricketBattlerB.SP -= _cricketBattlerA.Vigor;
				battlerAFirstAttack = CheckPercentProb(80);
			}
			else if (num2 < 0)
			{
				_cricketBattlerA.SP -= _cricketBattlerB.Vigor;
				battlerAFirstAttack = CheckPercentProb(20);
			}
			else
			{
				battlerAFirstAttack = CheckPercentProb(50);
			}
			if (num2 == 0 || ((num2 > 0) ? (!_cricketBattlerB.IsFail) : (!_cricketBattlerA.IsFail)))
			{
				DoNormalAttack(battlerAFirstAttack);
			}
		}
		num = (_cricketBattlerA.IsFail ? 1 : 0);
		_cricketBattlerA = null;
		_cricketBattlerB = null;
		_dataContext = null;
		return num;
	}

	private static int CheckWinBeforeFight()
	{
		int level = _cricketBattlerA.Level;
		int level2 = _cricketBattlerB.Level;
		if ((!_cricketBattlerA.IsTrash && _cricketBattlerB.IsTrash) || (level - level2 >= 6 && CheckPercentProb((level - level2) * 10)))
		{
			return 0;
		}
		if ((!_cricketBattlerB.IsTrash && _cricketBattlerA.IsTrash) || (level2 - level >= 6 && CheckPercentProb((level2 - level) * 10)))
		{
			return 1;
		}
		if (_cricketBattlerA.IsTrash && _cricketBattlerB.IsTrash)
		{
			return (!CheckPercentProb(50)) ? 1 : 0;
		}
		return -1;
	}

	private static void DoNormalAttack(bool battlerAFirstAttack, bool firstAttack = true)
	{
		CricketBattler cricketBattler = (battlerAFirstAttack ? _cricketBattlerA : _cricketBattlerB);
		CricketBattler cricketBattler2 = (battlerAFirstAttack ? _cricketBattlerB : _cricketBattlerA);
		bool flag = CheckPercentProb(cricketBattler.Deadliness);
		bool flag2 = CheckPercentProb(cricketBattler2.Defence);
		bool canCounter = CheckPercentProb(cricketBattler2.Counter);
		int num = cricketBattler.Bite + (flag ? cricketBattler.Damage : 0);
		if (flag2)
		{
			num = (int)MathF.Max(num - cricketBattler2.DamageReduce, 0f);
		}
		SettleNormalAttackDamage(cricketBattler, cricketBattler2, battlerAFirstAttack, num, flag, flag2, canCounter, isCounterAttack: false, firstAttack, 0);
	}

	private static void SettleNormalAttackDamage(CricketBattler attacker, CricketBattler defender, bool battlerAFirstAttack, int damage, bool critical, bool defend, bool canCounter, bool isCounterAttack, bool firstAttack, int counterTimes)
	{
		int num = ((critical || isCounterAttack) ? attacker.Vigor : 0);
		if (defend)
		{
			num = (int)MathF.Max(num - defender.DamageReduce, 0f);
		}
		else if (critical)
		{
			defender.Durability--;
			int percentProb = attacker.Deadliness + attacker.Cripple;
			if (CheckPercentProb(percentProb))
			{
				int random;
				short num2;
				if (CheckPercentProb(35))
				{
					random = EventHelper.GetRandom(2, 5);
					num2 = 1;
				}
				else
				{
					random = EventHelper.GetRandom(0, 2);
					num2 = 5;
				}
				defender.CricketData.Injuries[random] += num2;
			}
		}
		defender.HP = (int)MathF.Min((int)MathF.Max(defender.HP - damage, 0f), defender.MaxHP);
		defender.SP = (int)MathF.Min((int)MathF.Max(defender.SP - num, 0f), defender.MaxSP);
		if (defender.IsFail)
		{
			return;
		}
		if (canCounter)
		{
			bool critical2 = CheckPercentProb(defender.Deadliness);
			bool flag = CheckPercentProb(attacker.Defence);
			int num3 = ((counterTimes % 2 == 0) ? defender.Strength : defender.Bite) + (critical ? defender.Damage : 0);
			canCounter = CheckPercentProb(attacker.Counter - counterTimes * 5);
			if (flag)
			{
				num3 = (int)MathF.Max(num3 - attacker.DamageReduce, 0f);
			}
			SettleNormalAttackDamage(defender, attacker, !battlerAFirstAttack, num3, critical2, flag, canCounter, isCounterAttack: true, firstAttack, ++counterTimes);
		}
		else
		{
			bool flag2 = ((counterTimes % 2 == 0) ? battlerAFirstAttack : (!battlerAFirstAttack));
			if (firstAttack)
			{
				DoNormalAttack(!flag2, firstAttack: false);
			}
		}
	}
}
