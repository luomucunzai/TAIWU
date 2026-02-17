using System;
using Config;
using GameData.Utilities;

namespace GameData.Domains.Organization
{
	// Token: 0x0200064C RID: 1612
	public static class SettlementPrisonEventEffectHelper
	{
		// Token: 0x06004857 RID: 18519 RVA: 0x0028CE1C File Offset: 0x0028B01C
		public static int CalcChangeFavorGuardCount(this SettlementPrisonEventEffectItem effectCfg, int totalCount)
		{
			int rate = effectCfg.ChangeFavorGuardBaseRate;
			return MathUtils.Clamp(totalCount * rate / 100, effectCfg.MinAffectedGuardCount, totalCount);
		}

		// Token: 0x06004858 RID: 18520 RVA: 0x0028CE48 File Offset: 0x0028B048
		public static int CalcChangeFavorMemberCount(this SettlementPrisonEventEffectItem effectCfg, int totalCount)
		{
			int rate = effectCfg.ChangeFavorMemberBaseRate;
			return MathUtils.Clamp(totalCount * rate / 100, effectCfg.MinAffectedMemberCount, totalCount);
		}

		// Token: 0x06004859 RID: 18521 RVA: 0x0028CE73 File Offset: 0x0028B073
		public static int CalcGuardDisapproveCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardDisapproveRate / 100;
		}

		// Token: 0x0600485A RID: 18522 RVA: 0x0028CE80 File Offset: 0x0028B080
		public static int CalcGuardApproveCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardApproveRate / 100;
		}

		// Token: 0x0600485B RID: 18523 RVA: 0x0028CE8D File Offset: 0x0028B08D
		public static int CalcMemberDisapproveCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberDisapproveRate / 100;
		}

		// Token: 0x0600485C RID: 18524 RVA: 0x0028CE9A File Offset: 0x0028B09A
		public static int CalcMemberApproveCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberApproveRate / 100;
		}

		// Token: 0x0600485D RID: 18525 RVA: 0x0028CEA7 File Offset: 0x0028B0A7
		public static int CalcGuardBecomeEnemyCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.GuardBecomeEnemyRate / 100;
		}

		// Token: 0x0600485E RID: 18526 RVA: 0x0028CEB4 File Offset: 0x0028B0B4
		public static int CalcMemberBecomeEnemyCount(this SettlementPrisonEventEffectItem effectCfg, int affectedCount)
		{
			return affectedCount * effectCfg.MemberBecomeEnemyRate / 100;
		}
	}
}
