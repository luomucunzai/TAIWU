using System;
using System.Collections.Generic;
using Config;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation
{
	// Token: 0x0200005F RID: 95
	public class OperationAnswer : OperationPointBase
	{
		// Token: 0x06001575 RID: 5493 RVA: 0x0014AF44 File Offset: 0x00149144
		public OperationAnswer()
		{
		}

		// Token: 0x06001576 RID: 5494 RVA: 0x0014AF50 File Offset: 0x00149150
		public override int GetSerializedSize()
		{
			int totalSize = base.GetSerializedSize();
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001577 RID: 5495 RVA: 0x0014AF7C File Offset: 0x0014917C
		public unsafe override int Serialize(byte* pData)
		{
			byte* pCurrData = pData + base.Serialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001578 RID: 5496 RVA: 0x0014AFB4 File Offset: 0x001491B4
		public unsafe override int Deserialize(byte* pData)
		{
			byte* pCurrData = pData + base.Deserialize(pData);
			int totalSize = (int)((long)(pCurrData - pData));
			return (totalSize <= 4) ? totalSize : ((totalSize + 3) / 4 * 4);
		}

		// Token: 0x06001579 RID: 5497 RVA: 0x0014AFEA File Offset: 0x001491EA
		public override IEnumerable<sbyte> PickEffectiveEffectCards(IEnumerable<sbyte> wantUseEffectCardIds)
		{
			foreach (sbyte effectCardId in wantUseEffectCardIds)
			{
				ELifeSkillCombatEffectSubEffect subEffect = LifeSkillCombatEffect.Instance[effectCardId].SubEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect = subEffect;
				ELifeSkillCombatEffectSubEffect elifeSkillCombatEffectSubEffect2 = elifeSkillCombatEffectSubEffect;
				if (elifeSkillCombatEffectSubEffect2 - ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam > 1)
				{
					if (elifeSkillCombatEffectSubEffect2 == ELifeSkillCombatEffectSubEffect.PointChange)
					{
						yield return effectCardId;
					}
				}
				else
				{
					yield return effectCardId;
				}
			}
			IEnumerator<sbyte> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600157A RID: 5498 RVA: 0x0014B001 File Offset: 0x00149201
		public OperationAnswer(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards) : base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
		{
		}
	}
}
