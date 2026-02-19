using System.Collections.Generic;
using Config;

namespace GameData.Domains.Taiwu.LifeSkillCombat.Operation;

public class OperationAnswer : OperationPointBase
{
	public OperationAnswer()
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
			case ELifeSkillCombatEffectSubEffect.SelfThesisChangeAroundHouseActivePointWithParam:
			case ELifeSkillCombatEffectSubEffect.SelfNotCostBookStates:
				yield return effectCardId;
				break;
			case ELifeSkillCombatEffectSubEffect.PointChange:
				yield return effectCardId;
				break;
			}
		}
	}

	public OperationAnswer(sbyte playerId, int stamp, int gridIndex, int basePoint, IEnumerable<sbyte> wantUseEffectCards)
		: base(playerId, stamp, gridIndex, basePoint, wantUseEffectCards)
	{
	}
}
