using Config;
using GameData.Domains.Character;
using GameData.Utilities;

namespace GameData.Domains.Organization;

public static class SettlementTreasuryEventEffectHelper
{
	public struct EffectArgs
	{
		public int ChangeFavorCount;

		public int FavorChange;

		public int DisapproveCount;

		public int ApproveCount;

		public int BecomeEnemyCount;

		public CharacterSet ChangeFavorCharIds;

		public CharacterSet DisapproveCharIds;

		public CharacterSet ApproveCharIds;

		public CharacterSet BecomeEnemyCharIds;

		public EffectArgs(SettlementTreasuryEventEffectItem effectCfg, int charCount, int totalWorth, bool isGuard)
		{
			ChangeFavorCharIds = default(CharacterSet);
			DisapproveCharIds = default(CharacterSet);
			ApproveCharIds = default(CharacterSet);
			BecomeEnemyCharIds = default(CharacterSet);
			if (isGuard)
			{
				FavorChange = effectCfg.CalcGuardFavorabilityChange(totalWorth);
				ChangeFavorCount = effectCfg.CalcChangeFavorGuardCount(charCount, totalWorth);
				DisapproveCount = effectCfg.CalcGuardDisapproveCount(ChangeFavorCount);
				ApproveCount = effectCfg.CalcGuardApproveCount(ChangeFavorCount);
				BecomeEnemyCount = effectCfg.CalcGuardBecomeEnemyCount(ChangeFavorCount);
			}
			else
			{
				FavorChange = effectCfg.CalcMemberFavorabilityChange(totalWorth);
				ChangeFavorCount = effectCfg.CalcChangeFavorMemberCount(charCount, totalWorth);
				DisapproveCount = effectCfg.CalcMemberDisapproveCount(ChangeFavorCount);
				ApproveCount = effectCfg.CalcMemberApproveCount(ChangeFavorCount);
				BecomeEnemyCount = effectCfg.CalcMemberBecomeEnemyCount(ChangeFavorCount);
			}
		}

		public EffectArgs(SettlementPrisonEventEffectItem effectCfg, int charCount, bool isGuard)
		{
			ChangeFavorCharIds = default(CharacterSet);
			DisapproveCharIds = default(CharacterSet);
			ApproveCharIds = default(CharacterSet);
			BecomeEnemyCharIds = default(CharacterSet);
			if (isGuard)
			{
				FavorChange = effectCfg.GuardBaseFavorChange;
				ChangeFavorCount = effectCfg.CalcChangeFavorGuardCount(charCount);
				DisapproveCount = effectCfg.CalcGuardDisapproveCount(ChangeFavorCount);
				ApproveCount = effectCfg.CalcGuardApproveCount(ChangeFavorCount);
				BecomeEnemyCount = effectCfg.CalcGuardBecomeEnemyCount(ChangeFavorCount);
			}
			else
			{
				FavorChange = effectCfg.MemberBaseFavorChange;
				ChangeFavorCount = effectCfg.CalcChangeFavorMemberCount(charCount);
				DisapproveCount = effectCfg.CalcMemberDisapproveCount(ChangeFavorCount);
				ApproveCount = effectCfg.CalcMemberApproveCount(ChangeFavorCount);
				BecomeEnemyCount = effectCfg.CalcMemberBecomeEnemyCount(ChangeFavorCount);
			}
		}
	}

	public static int CalcChangeFavorGuardCount(this SettlementTreasuryEventEffectItem effectCfg, int totalCount, int worth)
	{
		int num = effectCfg.ChangeFavorGuardBaseRate + worth * effectCfg.ChangeFavorGuardFactor / Accessory.Instance[(short)8].BaseValue;
		return MathUtils.Clamp(totalCount * num / 100, effectCfg.MinAffectedGuardCount, totalCount);
	}

	public static int CalcChangeFavorMemberCount(this SettlementTreasuryEventEffectItem effectCfg, int totalCount, int worth)
	{
		int num = effectCfg.ChangeFavorMemberBaseRate + worth * effectCfg.ChangeFavorMemberFactor / Accessory.Instance[(short)8].BaseValue;
		return MathUtils.Clamp(totalCount * num / 100, effectCfg.MinAffectedMemberCount, totalCount);
	}

	public static int CalcGuardFavorabilityChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
	{
		return effectCfg.GuardBaseFavorChange + worth * effectCfg.GuardFavorChangeFactor / 100;
	}

	public static int CalcMemberFavorabilityChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
	{
		return effectCfg.MemberBaseFavorChange + worth * effectCfg.MemberFavorChangeFactor / 100;
	}

	public static int CalcSpiritualDebtChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
	{
		return effectCfg.BaseSpiritualDebtChange + worth * effectCfg.SpiritualDebtChangeFactor / Accessory.Instance[(short)8].BaseValue;
	}

	public static int CalcGuardDisapproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.GuardDisapproveRate / 100;
	}

	public static int CalcGuardApproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.GuardApproveRate / 100;
	}

	public static int CalcMemberDisapproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.MemberDisapproveRate / 100;
	}

	public static int CalcMemberApproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.MemberApproveRate / 100;
	}

	public static int CalcGuardBecomeEnemyCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.GuardBecomeEnemyRate / 100;
	}

	public static int CalcMemberBecomeEnemyCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
	{
		return affectedCount * effectCfg.MemberBecomeEnemyRate / 100;
	}

	public static short CalcFameMultiplierByContribution(int contribution)
	{
		return (short)(contribution * 5 / Accessory.Instance[(short)8].BaseValue + 1);
	}
}
