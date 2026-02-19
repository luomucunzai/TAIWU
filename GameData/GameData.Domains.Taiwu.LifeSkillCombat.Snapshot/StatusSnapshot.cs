using System.Collections.Generic;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Serializer;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;

public class StatusSnapshot
{
	public readonly GridList GridStatus;

	public readonly Player Self;

	public readonly Player Adversary;

	public readonly int ScoreSelf;

	public readonly int ScoreAdversary;

	public readonly sbyte CurrentPlayerId;

	public readonly sbyte WinnerPlayerId;

	public readonly bool SuicideIsForced;

	public readonly int PlayerSwitchCount;

	public StatusSnapshot(Match match, IList<Grid> gridStatus, Player self, Player adversary, sbyte currentPlayerId, sbyte winnerPlayerId)
	{
		GridStatus = new GridList();
		foreach (Grid item in gridStatus)
		{
			GridStatus.Add(GameData.Serializer.Serializer.CreateCopy(item));
		}
		Self = GameData.Serializer.Serializer.CreateCopy(self);
		Adversary = GameData.Serializer.Serializer.CreateCopy(adversary);
		CurrentPlayerId = currentPlayerId;
		WinnerPlayerId = winnerPlayerId;
		SuicideIsForced = match.SuicideIsForced;
		ScoreSelf = match.CalcPlayerScore(0);
		ScoreAdversary = match.CalcPlayerScore(1);
		PlayerSwitchCount = match.PlayerSwitchCount;
	}
}
