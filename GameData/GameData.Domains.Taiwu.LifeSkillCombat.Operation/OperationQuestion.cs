using System.Collections.Generic;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationQuestion : OperationPointBase
{
	public OperationQuestion()
	{
	}

	public override int GetSerializedSize()
	{
		int serializedSize = base.GetSerializedSize();
		return (serializedSize <= 4) ? serializedSize : ((serializedSize + 3) / 4 * 4);
	}

	public unsafe override int Serialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Serialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public unsafe override int Deserialize(byte* pData)
	{
		byte* ptr = pData;
		ptr += base.Deserialize(ptr);
		int num = (int)(ptr - pData);
		return (num <= 4) ? num : ((num + 3) / 4 * 4);
	}

	public override IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds)
	{
		foreach (sbyte effectCardId in wantUseEffectCardIds)
		{
			switch (LifeSkillCombatEffect.Instance[effectCardId].SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
			case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount:
				yield return effectCardId;
				break;
			case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
			case ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates:
				yield return effectCardId;
				break;
			}
		}
	}

	public override void ProcessOnGridActiveFixed(Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
	{
		foreach (sbyte effectiveEffectCardTemplateId in base.EffectiveEffectCardTemplateIds)
		{
			sbyte[] subEffectParameters = LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffectParameters;
			sbyte num = lifeSkillCombat.CalcTargetPlayerId(subEffectParameters[0], base.PlayerId);
			sbyte num2 = subEffectParameters[1];
			sbyte num3 = subEffectParameters[2];
			sbyte b = subEffectParameters[3];
			sbyte cross = num3;
			sbyte straight = num2;
			sbyte b2 = num;
			switch (LifeSkillCombatEffect.Instance[effectiveEffectCardTemplateId].SubEffect)
			{
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
				foreach (Coordinate2D<sbyte> item in Grid.OffsetIterate(base.Coordinate, straight, cross))
				{
					Grid grid3 = lifeSkillCombat.GetGrid(item);
					if (grid3 == null)
					{
						continue;
					}
					OperationPointBase operationPointBase = grid3.GetThesis();
					if (operationPointBase == null)
					{
						operationPointBase = grid3.ActiveOperation as OperationPointBase;
					}
					if (operationPointBase == null || operationPointBase.PlayerId != b2)
					{
						continue;
					}
					for (int num5 = operationPointBase.EffectiveEffectCardTemplateIds.Count - 1; num5 >= 0; num5--)
					{
						sbyte b3 = operationPointBase.EffectiveEffectCardTemplateIds[num5];
						operationPointBase.DropCard(b3);
						switch (LifeSkillCombatEffect.Instance[b3].SubEffect)
						{
						case ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion:
						case ELifeSkillCombatEffectSubEffect.SelfTrapedInCell:
						case ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint:
						case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
						case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
						case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
						case ELifeSkillCombatEffectSubEffect.PointChange:
							gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
							{
								GridIndex = operationPointBase.GridIndex,
								OwnerPlayerId = operationPointBase.PlayerId,
								Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Lost
							});
							break;
						}
					}
				}
				break;
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
				foreach (Coordinate2D<sbyte> item2 in Grid.OffsetIterate(base.Coordinate, straight, cross))
				{
					Grid grid2 = lifeSkillCombat.GetGrid(item2);
					if (grid2 != null)
					{
						OperationPointBase thesis2 = grid2.GetThesis();
						if (thesis2 != null && thesis2.PlayerId == b2 && thesis2.Point < base.Point)
						{
							grid2.DropHistoryOperations();
							grid2.SetActiveOperation(null, lifeSkillCombat, gridTrapStateExtraDiffs);
						}
					}
				}
				break;
			case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount:
			{
				int num4 = 0;
				foreach (Coordinate2D<sbyte> item3 in Grid.OffsetIterate(base.Coordinate, straight, cross))
				{
					Grid grid = lifeSkillCombat.GetGrid(item3);
					if (grid != null)
					{
						OperationPointBase thesis = grid.GetThesis();
						if (thesis != null && thesis.PlayerId == b2)
						{
							num4++;
						}
					}
				}
				if (num4 > 0)
				{
					lifeSkillCombat.GetPlayer(b2).RecruitEffectCards(null, lifeSkillCombat, num4);
				}
				break;
			}
			}
		}
		base.ProcessOnGridActiveFixed(lifeSkillCombat, gridTrapStateExtraDiffs);
	}

	internal OperationQuestion(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards)
		: base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
	{
	}
}
