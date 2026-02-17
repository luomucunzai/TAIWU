using System;
using System.Collections.Generic;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Snapshot
{
	// Token: 0x0200005D RID: 93
	public class StatusSnapshot
	{
		// Token: 0x0600156C RID: 5484 RVA: 0x00149F18 File Offset: 0x00148118
		public StatusSnapshot(Match match, IList<Grid> gridStatus, Player self, Player adversary, sbyte currentPlayerId, sbyte winnerPlayerId)
		{
			this.GridStatus = new GridList();
			foreach (Grid grid in gridStatus)
			{
				this.GridStatus.Add(Serializer.CreateCopy<Grid>(grid));
			}
			this.Self = Serializer.CreateCopy<Player>(self);
			this.Adversary = Serializer.CreateCopy<Player>(adversary);
			this.CurrentPlayerId = currentPlayerId;
			this.WinnerPlayerId = winnerPlayerId;
			this.SuicideIsForced = match.SuicideIsForced;
			this.ScoreSelf = match.CalcPlayerScore(0);
			this.ScoreAdversary = match.CalcPlayerScore(1);
			this.PlayerSwitchCount = match.PlayerSwitchCount;
		}

		// Token: 0x04000366 RID: 870
		public readonly GridList GridStatus;

		// Token: 0x04000367 RID: 871
		public readonly Player Self;

		// Token: 0x04000368 RID: 872
		public readonly Player Adversary;

		// Token: 0x04000369 RID: 873
		public readonly int ScoreSelf;

		// Token: 0x0400036A RID: 874
		public readonly int ScoreAdversary;

		// Token: 0x0400036B RID: 875
		public readonly sbyte CurrentPlayerId;

		// Token: 0x0400036C RID: 876
		public readonly sbyte WinnerPlayerId;

		// Token: 0x0400036D RID: 877
		public readonly bool SuicideIsForced;

		// Token: 0x0400036E RID: 878
		public readonly int PlayerSwitchCount;
	}
}
