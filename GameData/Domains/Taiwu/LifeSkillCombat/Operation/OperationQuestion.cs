using System;
using System.Collections.Generic;
using Config;
using GameData.Domains.Taiwu.LifeSkillCombat.Snapshot;
using GameData.Domains.Taiwu.LifeSkillCombat.Status;
using GameData.Utilities;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x02000067 RID: 103
	public class OperationQuestion : OperationPointBase
	{
		// Token: 0x060015C9 RID: 5577 RVA: 0x0014C3C3 File Offset: 0x0014A5C3
		public OperationQuestion()
		{
		}

		// Token: 0x060015CA RID: 5578 RVA: 0x0014C3D0 File Offset: 0x0014A5D0
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015CB RID: 5579 RVA: 0x0014C3FC File Offset: 0x0014A5FC
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015CC RID: 5580 RVA: 0x0014C434 File Offset: 0x0014A634
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x060015CD RID: 5581 RVA: 0x0014C46A File Offset: 0x0014A66A
		public override IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds)
		{
			foreach (sbyte effectCardId in wantUseEffectCardIds)
			{
				ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectCardId].SubEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect2 = elifeSkillCombatEffectSubEffect;
				if (elifeSkillCombatEffectSubEffect2 - ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion <= 1)
				{
					goto IL_A5;
				}
				if (elifeSkillCombatEffectSubEffect2 - ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam > 1)
				{
					if (elifeSkillCombatEffectSubEffect2 - ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow <= 2)
					{
						goto IL_A5;
					}
				}
				else
				{
					yield return effectCardId;
				}
				continue;
				IL_A5:
				yield return effectCardId;
			}
			IEnumerator<sbyte> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060015CE RID: 5582 RVA: 0x0014C484 File Offset: 0x0014A684
		public override void ProcessOnGridActiveFixed(Match lifeSkillCombat, IList<StatusSnapshotDiff.GridTrapStateExtraDiff> gridTrapStateExtraDiffs)
		{
			foreach (sbyte effectCardId in base.EffectiveEffectCardTemplateIds)
			{
				sbyte[] subEffectParameters = LifeSkillCombatEffect.Instance[effectCardId].SubEffectParameters;
				sbyte b = lifeSkillCombat.CalcTargetPlayerId(subEffectParameters[0], base.PlayerId);
				sbyte b2 = subEffectParameters[1];
				sbyte b3 = subEffectParameters[2];
				sbyte value = subEffectParameters[3];
				sbyte cross = b3;
				sbyte straight = b2;
				sbyte house = b;
				switch (LifeSkillCombatEffect.Instance[effectCardId].SubEffect)
				{
				case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
					foreach (Coordinate2D<sbyte> target in Grid.OffsetIterate(base.Coordinate, straight, cross, null))
					{
						Grid grid = lifeSkillCombat.GetGrid(target);
						bool flag = grid != null;
						if (flag)
						{
							OperationPointBase op = grid.GetThesis();
							bool flag2 = op == null;
							if (flag2)
							{
								op = (grid.ActiveOperation as OperationPointBase);
							}
							bool flag3 = op != null && op.PlayerId == house;
							if (flag3)
							{
								int i = op.EffectiveEffectCardTemplateIds.Count - 1;
								while (i >= 0)
								{
									sbyte e = op.EffectiveEffectCardTemplateIds[i];
									op.DropCard(e);
									ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[e].SubEffect;
									ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
									if (elifeSkillCombatEffectSubEffect - ELifeSkillCombatEffectSubEffect.SelfGridCoverBookStatesWhenAllQuestion <= 1 || elifeSkillCombatEffectSubEffect == ELifeSkillCombatEffectSubEffect.SelfDoPickByPoint)
									{
										goto IL_174;
									}
									switch (elifeSkillCombatEffectSubEffect)
									{
									case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
									case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedEraseEffectsAroundHouseAllQuestionLowAndThesisLow:
									case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
									case ELifeSkillCombatEffectSubEffect.PointChange:
										goto IL_174;
									}
									IL_1AC:
									i--;
									continue;
									IL_174:
									gridTrapStateExtraDiffs.Add(new StatusSnapshotDiff.GridTrapStateExtraDiff
									{
										GridIndex = op.GridIndex,
										OwnerPlayerId = op.PlayerId,
										Type = StatusSnapshotDiff.GridTrapStateExtraDiff.TrapChangeType.Lost
									});
									goto IL_1AC;
								}
							}
						}
					}
					break;
				case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoCancelAroundHouseAllQuestionLowAndThesisLow:
					foreach (Coordinate2D<sbyte> target2 in Grid.OffsetIterate(base.Coordinate, straight, cross, null))
					{
						Grid grid2 = lifeSkillCombat.GetGrid(target2);
						bool flag4 = grid2 != null;
						if (flag4)
						{
							OperationPointBase op2 = grid2.GetThesis();
							bool flag5 = op2 != null && op2.PlayerId == house && op2.Point < base.Point;
							if (flag5)
							{
								grid2.DropHistoryOperations();
								grid2.SetActiveOperation(null, lifeSkillCombat, gridTrapStateExtraDiffs);
							}
						}
					}
					break;
				case ELifeSkillCombatEffectSubEffect.SelfThesisWhenFixedDoPickWithAroundHouseThesisCount:
				{
					int count = 0;
					foreach (Coordinate2D<sbyte> target3 in Grid.OffsetIterate(base.Coordinate, straight, cross, null))
					{
						Grid grid3 = lifeSkillCombat.GetGrid(target3);
						bool flag6 = grid3 != null;
						if (flag6)
						{
							OperationPointBase op3 = grid3.GetThesis();
							bool flag7 = op3 != null && op3.PlayerId == house;
							if (flag7)
							{
								count++;
							}
						}
					}
					bool flag8 = count > 0;
					if (flag8)
					{
						lifeSkillCombat.GetPlayer(house).RecruitEffectCards(null, lifeSkillCombat, count);
					}
					break;
				}
				}
			}
			base.ProcessOnGridActiveFixed(lifeSkillCombat, gridTrapStateExtraDiffs);
		}

		// Token: 0x060015CF RID: 5583 RVA: 0x0014C83C File Offset: 0x0014AA3C
		internal OperationQuestion(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards) : base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
		{
		}
	}
}
