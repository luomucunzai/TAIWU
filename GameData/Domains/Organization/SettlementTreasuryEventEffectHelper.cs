using System;
using Config;
using GameData.Domains.Character;
using GameData.Utilities;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064E RID: 1614
	public static class SettlementTreasuryEventEffectHelper
	{
		// Token: 0x06004864 RID: 18532 RVA: 0x0028CFE8 File Offset: 0x0028B1E8
		public static int CalcChangeFavorGuardCount(this SettlementTreasuryEventEffectItem effectCfg, int totalCount, int worth)
		{
			int rate = effectCfg.ChangeFavorGuardBaseRate + worth * effectCfg.ChangeFavorGuardFactor / Accessory.Instance[8].BaseValue;
			return MathUtils.Clamp(totalCount * rate / 100, effectCfg.MinAffectedGuardCount, totalCount);
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x0028D030 File Offset: 0x0028B230
		public static int CalcChangeFavorMemberCount(this SettlementTreasuryEventEffectItem effectCfg, int totalCount, int worth)
		{
			int rate = effectCfg.ChangeFavorMemberBaseRate + worth * effectCfg.ChangeFavorMemberFactor / Accessory.Instance[8].BaseValue;
			return MathUtils.Clamp(totalCount * rate / 100, effectCfg.MinAffectedMemberCount, totalCount);
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x0028D075 File Offset: 0x0028B275
		public static int CalcGuardFavorabilityChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
		{
			return effectCfg.GuardBaseFavorChange + worth * effectCfg.GuardFavorChangeFactor / 100;
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0028D089 File Offset: 0x0028B289
		public static int CalcMemberFavorabilityChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
		{
			return effectCfg.MemberBaseFavorChange + worth * effectCfg.MemberFavorChangeFactor / 100;
		}

		// Token: 0x06004868 RID: 18536 RVA: 0x0028D09D File Offset: 0x0028B29D
		public static int CalcSpiritualDebtChange(this SettlementTreasuryEventEffectItem effectCfg, int worth)
		{
			return effectCfg.BaseSpiritualDebtChange + worth * effectCfg.SpiritualDebtChangeFactor / Accessory.Instance[8].BaseValue;
		}

		// Token: 0x06004869 RID: 18537 RVA: 0x0028D0BF File Offset: 0x0028B2BF
		public static int CalcGuardDisapproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardDisapproveRate / 100;
		}

		// Token: 0x0600486A RID: 18538 RVA: 0x0028D0CC File Offset: 0x0028B2CC
		public static int CalcGuardApproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardApproveRate / 100;
		}

		// Token: 0x0600486B RID: 18539 RVA: 0x0028D0D9 File Offset: 0x0028B2D9
		public static int CalcMemberDisapproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberDisapproveRate / 100;
		}

		// Token: 0x0600486C RID: 18540 RVA: 0x0028D0E6 File Offset: 0x0028B2E6
		public static int CalcMemberApproveCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberApproveRate / 100;
		}

		// Token: 0x0600486D RID: 18541 RVA: 0x0028D0F3 File Offset: 0x0028B2F3
		public static int CalcGuardBecomeEnemyCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardBecomeEnemyRate / 100;
		}

		// Token: 0x0600486E RID: 18542 RVA: 0x0028D100 File Offset: 0x0028B300
		public static int CalcMemberBecomeEnemyCount(this SettlementTreasuryEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberBecomeEnemyRate / 100;
		}

		// Token: 0x0600486F RID: 18543 RVA: 0x0028D110 File Offset: 0x0028B310
		public static short CalcFameMultiplierByContribution(int contribution)
		{
			return (short)(contribution * 5 / Accessory.Instance[8].BaseValue + 1);
		}

		// Token: 0x02000A8C RID: 2700
		public struct EffectArgs
		{
			// Token: 0x06008880 RID: 34944 RVA: 0x004EC848 File Offset: 0x004EAA48
			public EffectArgs(SettlementTreasuryEventEffectItem effectCfg, int charCount, int totalWorth, bool isGuard)
			{
				this.ChangeFavorCharIds = default(CharacterSet);
				this.DisapproveCharIds = default(CharacterSet);
				this.ApproveCharIds = default(CharacterSet);
				this.BecomeEnemyCharIds = default(CharacterSet);
				if (isGuard)
				{
					this.FavorChange = effectCfg.CalcGuardFavorabilityChange(totalWorth);
					this.ChangeFavorCount = effectCfg.CalcChangeFavorGuardCount(charCount, totalWorth);
					this.DisapproveCount = effectCfg.CalcGuardDisapproveCount(this.ChangeFavorCount);
					this.ApproveCount = effectCfg.CalcGuardApproveCount(this.ChangeFavorCount);
					this.BecomeEnemyCount = effectCfg.CalcGuardBecomeEnemyCount(this.ChangeFavorCount);
				}
				else
				{
					this.FavorChange = effectCfg.CalcMemberFavorabilityChange(totalWorth);
					this.ChangeFavorCount = effectCfg.CalcChangeFavorMemberCount(charCount, totalWorth);
					this.DisapproveCount = effectCfg.CalcMemberDisapproveCount(this.ChangeFavorCount);
					this.ApproveCount = effectCfg.CalcMemberApproveCount(this.ChangeFavorCount);
					this.BecomeEnemyCount = effectCfg.CalcMemberBecomeEnemyCount(this.ChangeFavorCount);
				}
			}

			// Token: 0x06008881 RID: 34945 RVA: 0x004EC934 File Offset: 0x004EAB34
			public EffectArgs(SettlementPrisonEventEffectItem effectCfg, int charCount, bool isGuard)
			{
				this.ChangeFavorCharIds = default(CharacterSet);
				this.DisapproveCharIds = default(CharacterSet);
				this.ApproveCharIds = default(CharacterSet);
				this.BecomeEnemyCharIds = default(CharacterSet);
				if (isGuard)
				{
					this.FavorChange = effectCfg.GuardBaseFavorChange;
					this.ChangeFavorCount = effectCfg.CalcChangeFavorGuardCount(charCount);
					this.DisapproveCount = effectCfg.CalcGuardDisapproveCount(this.ChangeFavorCount);
					this.ApproveCount = effectCfg.CalcGuardApproveCount(this.ChangeFavorCount);
					this.BecomeEnemyCount = effectCfg.CalcGuardBecomeEnemyCount(this.ChangeFavorCount);
				}
				else
				{
					this.FavorChange = effectCfg.MemberBaseFavorChange;
					this.ChangeFavorCount = effectCfg.CalcChangeFavorMemberCount(charCount);
					this.DisapproveCount = effectCfg.CalcMemberDisapproveCount(this.ChangeFavorCount);
					this.ApproveCount = effectCfg.CalcMemberApproveCount(this.ChangeFavorCount);
					this.BecomeEnemyCount = effectCfg.CalcMemberBecomeEnemyCount(this.ChangeFavorCount);
				}
			}

			// Token: 0x04002B62 RID: 11106
			public int ChangeFavorCount;

			// Token: 0x04002B63 RID: 11107
			public int FavorChange;

			// Token: 0x04002B64 RID: 11108
			public int DisapproveCount;

			// Token: 0x04002B65 RID: 11109
			public int ApproveCount;

			// Token: 0x04002B66 RID: 11110
			public int BecomeEnemyCount;

			// Token: 0x04002B67 RID: 11111
			public CharacterSet ChangeFavorCharIds;

			// Token: 0x04002B68 RID: 11112
			public CharacterSet DisapproveCharIds;

			// Token: 0x04002B69 RID: 11113
			public CharacterSet ApproveCharIds;

			// Token: 0x04002B6A RID: 11114
			public CharacterSet BecomeEnemyCharIds;
		}
	}
}
